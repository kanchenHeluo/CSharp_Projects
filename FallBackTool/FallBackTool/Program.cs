using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using FallBackTool.Converter;
using FallBackTool.Converter.Models;
using FallBackTool.Producer;
using FallBackTool.Consumer;
using FallBackTool.Consumer.Common;
using AutoMapper;
using FallBackTool.Producer.Models;
using log4net;

namespace FallBackTool
{
    class Program
    {
        static void Main(string[] args)
        {
            ArgumentBuilder builder;
            if (args == null || args.Length == 0)
                builder = new ArgumentBuilder();
            else
                builder = new ArgumentBuilder(args);

            if (!builder.Valid)
                return;

            try
            {
                DateTime start = DateTime.Now;
                Console.WriteLine("[Utility Tool Begin]:" + start.ToString());
                ILog log = LogManager.GetLogger(typeof(Program));
                log.Info("[Utility Tool Begin]:" + start.ToString());

                if (builder.EnablePullFromStaging)
                {
                    DBProducerInput dbinput = new DBProducerInput();
                    dbinput.LastSyncDate = builder.LastSyncDate;
                    dbinput.LastSyncId = builder.LastSyncId;
                    dbinput.LastSyncVersion = builder.LastSyncVersion;
                    dbinput.FilterKey = builder.FilterKey;
                    dbinput.ValueList = builder.ValueList;
                    DBProducer producer = new DBProducer(dbinput);
                    List<IDBModel> data = producer.Produce();
                    if (data != null)
                    {
                        if (builder.EnableMergeToAzureDB)
                        {
                            AzureDBConsumer adbconsumer = new AzureDBConsumer();
                            adbconsumer.Consume(producer.results);
                        }

                        if (builder.EnableProcessToCRM)
                        {
                            Mapper.Initialize(cfg =>
                            {
                                cfg.AddProfile<OrganizationProfile>();
                            });

                            List<ICRMModel> result = map(data.ToList());

                            CRMConsumer consumer = new CRMConsumer();
                            consumer.Consume(result); //inside will split to sperate parrell threads to handle
                        }
                    }
                }

                DateTime end = DateTime.Now;
                Console.WriteLine("[Utility Tool END]:" + end.ToString());
                Console.WriteLine("[TimeSpan]:" + end.Subtract(start).ToString());
                log.Info("[Utility Tool END]:" + end.ToString());

                #if DEBUG
                Console.ReadKey();
                #endif
            }
            catch (Exception ex)
            {
                ILog log = LogManager.GetLogger(ex.GetType());
                log.Error("[Exception]:", ex);
                Console.WriteLine("[Exception]: Exception occur, check Log file for the detail.");
            }
        }

        private static List<ICRMModel> map(List<IDBModel> data)
        {
            List<ICRMModel> result = new List<ICRMModel>();
            string appName = ConfigurationManager.AppSettings["application"];
            string entityName = ConfigurationManager.AppSettings["EntityName"];

            if (appName == "mpc" && entityName == "Contact")
            {
                foreach (ContactDBModel d in data)
                {
                   result.Add(Mapper.Map<ContactDBModel, MPCContactCRMModel>(d));
                }
            }
            else if (appName == "onepayroll" && entityName == "Contact")
            {
                foreach (ContactDBModel d in data)
                {
                    result.Add(Mapper.Map<ContactDBModel, OPRContactCRMModel>(d));
                }
            }
            else
            {
                //default common one
                foreach (ContactDBModel d in data)
                {
                    result.Add(Mapper.Map<ContactDBModel, MPCContactCRMModel>(d));
                }
            }

            return result;
        }

    }
}

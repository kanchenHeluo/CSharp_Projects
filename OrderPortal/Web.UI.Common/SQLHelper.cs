using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;
using System.Collections.ObjectModel;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.IO;

namespace Web.UI.Common
{
    public class SqlHelper
    {

        /// <summary>
        /// Executes Procedure and returns xml reader
        /// </summary>
        /// <param name="ProcName"></param>
        /// <param name="Params"></param>
        /// <param name="DBEnum"></param>
        /// <returns></returns>
        public static StringReader ExecXMLReader(string ProcName, SqlParameter[] Params, string dbName)
        {
            try
            {
                string DbConnectionString = "";
                AppSettingsReader appSettings = new AppSettingsReader();
                StringReader readerValue = null;
                DbConnectionString = ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
                using (SqlConnection connection = new SqlConnection(DbConnectionString))
                {
                    connection.Open();
                    SqlCommand storedProcCommand = new SqlCommand(ProcName, connection);
                    storedProcCommand.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter param in Params)
                    {
                        storedProcCommand.Parameters.Add(param);
                    }
                    using (XmlReader resultXml = storedProcCommand.ExecuteXmlReader())
                    {
                        resultXml.Read();
                        while (resultXml.ReadState != System.Xml.ReadState.EndOfFile)
                        {
                            string xmlValue = resultXml.ReadOuterXml();
                            readerValue = new StringReader(xmlValue);
                        }
                    }
                    storedProcCommand.Dispose();
                    connection.Close();
                    connection.Dispose();
                    return readerValue;
                }
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Executes Procedure and returns DataTable
        /// </summary>
        /// <param name="ProcName"></param>
        /// <param name="Params"></param>
        /// <param name="DBEnum"></param>
        /// <returns></returns>
        public static DataTable ExecProcSqlDataReader(string ProcName, SqlParameter[] Params, string dbName)
        {
            DataTable dtClaims = null;
            try
            {
                string DbConnectionString = string.Empty;
                AppSettingsReader appSettings = new AppSettingsReader();                
                DbConnectionString = ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
                using (SqlConnection connection = new SqlConnection(DbConnectionString))
                {
                    connection.Open();
                    SqlCommand storedProcCommand = new SqlCommand(ProcName, connection);
                    storedProcCommand.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter param in Params)
                    {
                        storedProcCommand.Parameters.Add(param);
                    }
                    using (SqlDataReader dbReader = storedProcCommand.ExecuteReader())
                    {
                        dtClaims = new DataTable();
                        dtClaims.Load(dbReader);
                    }
                    storedProcCommand.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
            catch (SqlException sqlEx)
            {
                dtClaims = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }           
              return dtClaims;
        }
    }
}

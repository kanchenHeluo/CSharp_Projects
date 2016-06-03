using Efiling;
using System;
using System.Collections.Generic;
using Efiling.Common;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            //read from data/input -> xml/json
            Dictionary<string, string> inputDict = new Dictionary<string, string>();
            inputDict.Add("Name", "test");
            inputDict.Add("Content","test content");

            //transmit: generate as efile, save as history
            EfileSummary summary = null;
            Efile efile = new Efile(); //change to get efile def from efile config
            
            USState s = USState.CA;
            var def = s.GetStateEfileDefinition("DE9");
            if (def != null)
            {
                summary  = new EfileSummary();
                summary = def.TransmitEfile(inputDict);
            }
            
            //show eile summary
            if (summary != null)
            {
                Console.WriteLine(summary.Id);
                Console.WriteLine(summary.TransmissionId);
                Console.WriteLine(summary.Status);
            }
            else
            {
                Console.WriteLine("error");
            }
        }
    }
}

using System;
using Efiling.DataAccess.Interface;
using System.Collections.Generic;

namespace Efiling.DataAccess
{
    public class EfileService : IEfileService
    {
        public Efile GenerateEfile(IDictionary<string, string> input)
        {
            //fake
            Efile efile = new Efile();
            efile.TransmissionId = Guid.NewGuid();
            efile.EfileName = input["Name"].ToString();
            efile.Content = input["Content"].ToString();

            return efile;
        }
    }
}

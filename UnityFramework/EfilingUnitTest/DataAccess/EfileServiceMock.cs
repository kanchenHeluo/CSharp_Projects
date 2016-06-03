using System;
using System.Collections.Generic;
using Efiling.DataAccess.Interface;
using Efiling;

namespace EfilingUnitTest.DataAccess
{
    public class EfileServiceMock : IEfileService
    {
        public Efile GenerateEfile(IDictionary<string, string> input)
        {
            Efile efile = new Efile();
            efile.TransmissionId = Guid.NewGuid();
            efile.Content = input["Content"].ToString();
            efile.EfileName = input["Name"].ToString();
            return efile;
        }

    }
}

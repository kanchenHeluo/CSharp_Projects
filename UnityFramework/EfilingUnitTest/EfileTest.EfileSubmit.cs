
using System.Collections.Generic;
using Efiling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EfilingUnitTest
{
    public partial class EfileTest
    {
        [TestMethod]
        public void TestEfileTransmit()
        {
            Dictionary<string, string> inputDict = new Dictionary<string, string>();
            inputDict.Add("Name", "test");
            inputDict.Add("Content", "test content");

            EfileSummary summary = new EfileSummary();
            Efile efile = new Efile(); //change to get efile def from efile config
            summary = efile.TransmitEfile(inputDict);

            Assert.IsNotNull(summary);
            Assert.IsNotNull(summary.TransmissionId);

        }
    }
}

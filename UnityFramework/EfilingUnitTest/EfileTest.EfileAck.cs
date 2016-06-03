using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EfilingUnitTest
{
    public partial class EfileTest
    {
        [TestMethod]
        public void TestUpdateAck()
        {
            /*
            var content = TestDataContext.GetAckContent("FLUTC6Ack.xml");
            var hiss = new List<EfileHistory>
            {
                //TestDataContext.CreateEfileHistory("FL_" + EfileForms.UTC6, EfileStatus.Sent, "2157940", "40585-2015-01-15-013"),
                TestDataContext.CreateEfileHistory("FL_" + EfileForms.UTC6, EfileStatus.Sent, "2157940", "40585-2015-01-15-014")
            };
            TestDataContext.SetContextValue(TestDataContext.MethodName.GetEfileHistory, hiss);
            var hash = new HashSet<string>();
            var called = false;
            Func<object[], object> action = list =>
            {
                called = true;
                foreach (var pair in ((IDictionary<EfileHistory, Pair<EfileContentType, string>>)list[0]))
                {
                    if (hash.Contains(pair.Key.SubmissionId))
                    {
                        throw new Exception("Duplicated submission Id");
                    }
                    if (pair.Key.Status == EfileStatus.Sent)
                    {
                        throw new Exception("Invalid status updated");
                    }
                    hash.Add(pair.Key.SubmissionId);
                }
                return 1;
            };
            TestDataContext.SetContextValue(TestDataContext.MethodName.BatchUpdateEfileHistory, action);
            var def = USState.FL.GetStateEfileDefinition(EfileForms.UTC6, EfileInputType.All, false);
            var ret = def.UpdateAck(1L, content);
            Assert.IsTrue(ret.Status == EfileStatus.Rejected);
            Assert.AreEqual(ret.Logs.Count, 3);
            Assert.AreEqual(called, true);*/
        }
    }
}

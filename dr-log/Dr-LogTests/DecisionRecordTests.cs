using System.IO;
using dr_log;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dr_LogTests
{
    [TestClass]
    public class DecisionRecordTests
    {
        [TestMethod]
        public void TestDecisionRecordFileRead()
        {
            string filePath = Path.GetFullPath("sample.md");

            DecisionRecord decisionRecord = new DecisionRecord(filePath);

            Assert.IsNotNull(decisionRecord.Risks);

        }
    }
}

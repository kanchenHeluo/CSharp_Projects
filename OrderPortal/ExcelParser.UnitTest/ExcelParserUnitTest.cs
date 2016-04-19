using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExcelParser;
using Web.UI.Repositories.DomainModels;
using System.Data;
using System.Data.OleDb;
namespace ExcelParser.UnitTest
{
    [TestClass]
    public class ExcelParserUnitTest
    {
        public TestContext TestContext { set; get; }

        [TestMethod]
        [DeploymentItem(@".\ExcelTemplate\VolumeLicensingmultiplebatch.xlsx")]
        public void TestXlsx()
        {
            var parser = new ExcelParser();
            var actual =  parser.Parse(new FileStream(Path.Combine(TestContext.TestDeploymentDir, "VolumeLicensingmultiplebatch.xlsx"),FileMode.Open));
            Assert.AreEqual(5, actual.Count);
        }

        [TestMethod]
        [DeploymentItem(@".\ExcelTemplate\VolumeLicensingmultiplebatch.xls")]

        public void TestXls()
        {
            string filePath = Path.Combine(TestContext.TestDeploymentDir, "VolumeLicensingmultiplebatch.xls");
            var parser = new ExcelParser();
            var actual = parser.ParseXls(filePath);
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void TestConfigDedegate()
        {
            var parser = new ExcelParser();
            var model = new OrderHeader();
            parser.orderHeaderConfig["UsageDate"].SetValueMethod(model, parser.orderHeaderConfig["UsageDate"].GetValueMethod("2011/01/01"));
            Assert.AreEqual(new DateTime(2011,01,01),model.UsageDate);
        }

        [TestMethod]
        [DeploymentItem(@".\ExcelTemplate\VolumeLicensingmultiplebatch.xls")]
        public void TestOLEDB()
        {
            string filePath = Path.Combine(TestContext.TestDeploymentDir, "VolumeLicensingmultiplebatch.xls");
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\"";
            OleDbConnection conn = null;
            StreamWriter wrtr = null;
            OleDbCommand cmd = null;
            OleDbDataAdapter da = null;
            conn = new OleDbConnection(strConn);
            conn.Open();
            cmd = new OleDbCommand("SELECT * from [Batch PO$]", conn);
            cmd.CommandType = CommandType.Text;
            da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                int i = 0;
                foreach (var column in  row.ItemArray)
                {
                    i++;
                    //sb.Append(column ?? "\t");
                    if (column == DBNull.Value)
                    {
                        if (i < row.ItemArray.Count())
                        {
                            sb.Append("\t");
                        }
                    }
                    else
                    {
                        sb.Append(column);
                        if (i < row.ItemArray.Count())
                        {
                            sb.Append("\t");
                        }
                    }

                }
                sb.Append("\n");
            }
            ExcelParser parser = new ExcelParser();
       //     var actual = parser.Parse(sb.ToString());
        }

        [TestMethod]
        [DeploymentItem(@".\ExcelTemplate\VolumeLicensingmultiplebatch.xls")]
        public void TestOLEDBReader()
        {
            string filePath = Path.Combine(TestContext.TestDeploymentDir, "VolumeLicensingmultiplebatch.xls");
            var parser = new ExcelParser();
            var actual = parser.ParseXls(filePath);
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void TestGetCellReferenceCode()
        {
            var parser = new ExcelParser();
            var actual = parser.GetCellReferenceCode(1, 1);
            Assert.AreEqual("A1", actual);
            actual = parser.GetCellReferenceCode(1, 26);
            Assert.AreEqual("Z1", actual);
            actual = parser.GetCellReferenceCode(8, 28);
            Assert.AreEqual("ERROR", actual);
        }

        [TestMethod]
        public void TestAs()
        {
            object a = null;
            Assert.AreEqual(a as string , null);
        }
    }
}

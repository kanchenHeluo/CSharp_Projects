using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace ExcelParser
{
    public class IssueAndError
    {
        public string Message { get; set; }
        public Severity Severity { get; set; }
        public Category Category { get; set; }
        public object ReferenceObject { get; set; }

    }
    public enum Severity
    {
        Critical,
        Medium,
        Low
    }

    public enum Category
    {
        Information,
        Issue,
        Error
    }
}

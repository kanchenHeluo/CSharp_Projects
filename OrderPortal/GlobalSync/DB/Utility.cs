using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalSync.UnitTest.UnitTest.DB
{
    public static class Utility
    {
        public static string GenerateKeyHash(string keyColumnName, string[] keyColumnNames, Dictionary<string, object> row)
        {
            StringBuilder builder = new StringBuilder();
            if (keyColumnNames.Length > 1)
            {                
                foreach (var columnName in keyColumnNames)
                {
                    object c = row[columnName];
                    builder.Append(c.ToString());
                }
            }
            else
            {
                builder.Append(row[keyColumnNames[0]].ToString());                
            }
            byte[] by = Encoding.UTF8.GetBytes(builder.ToString());
            string value =  Convert.ToBase64String(by);

            row.Add(keyColumnName, value);

            return value;
        }

     
    }
}

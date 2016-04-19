using FallBackTool.Converter.Models;
using System;
using System.Collections.Generic;

namespace FallBackTool.Consumer.Common
{
    public class ApplicationCache
    {
        public static Guid accountId = Guid.Empty;
        public static List<ICRMModel> retryList = null;
        public static int totalRecords = 0;
        public static int processedRecords = 0;
        public static int percent = 0;
    }
}

using MemoApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing profile service:");
            var memoClient = new MemoClient();
            var memoList = memoClient.Get();;
            foreach (var item in memoList)
            {
                Console.WriteLine("key:{0} - value:{1}", item.Key, item.Value);
            }
            Console.WriteLine();


            Console.WriteLine("Done. Press any key to exit...");
            Console.ReadKey(true);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YieldExample
{
    class Program
    {
        public class Item
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        static List<Item> GetInitData()
        {
            List<Item> li = new List<Item>();
            for (int i = 0; i < 10000000; i++ )
            {
                Item ins = new Item();
                ins.Id = i;
                ins.Name = "n" + i.ToString();
                li.Add(ins);
            }
            return li;
        }

        static IEnumerable<Item> FilterWithYield(List<Item> li)
        {
            foreach(var item in li){
                if (item.Id % 2 == 0)
                {
                    yield return item;
                }
            }
            yield break;
        }

        static IEnumerable<Item> FilterWithoutYield(List<Item> li)
        {
            List<Item> result = new List<Item>();
            //List<Item> li = GetInitData();
            foreach (var item in li)
            {
                if (item.Id % 2 == 0)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        static void Main(string[] args)
        {
            List<Item> li1 = GetInitData();
            List<Item> li2 = GetInitData();
            
            
            Console.WriteLine("[2]:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
            foreach (var item in FilterWithoutYield(li1))
            {
                if (item.Id % 2 == 0)
                {
                    break;
                }
            }
            Console.WriteLine("[2]:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
            Console.WriteLine("[1]:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
            foreach (var item in FilterWithYield(li2))
            {
                if (item.Id % 2 == 0)
                {
                    break;
                }
            }
            Console.WriteLine("[1]:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
            Console.Read();
        }
    }
}

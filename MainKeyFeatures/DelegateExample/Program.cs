using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //delegate声明，实例化和 调用（单独调用和多播）
            //可指向多个函数的函数指针 
            Car c = new Car();
            c.OpenCarWindow();
            c.openConditioner();
            c.OpenCarWindow();
            Console.Read();
        }
    }
}

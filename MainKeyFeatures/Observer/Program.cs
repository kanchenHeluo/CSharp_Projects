using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer
{
    class Program
    {
        static void Main(string[] args)
        {
            //abstract vs interface -- 语法上看：抽象类只能被继承一个，但是接口可以被实现多个。
            //OCP
            //abstract class: target -> delegate notify; call notify;
            //abstract class: observer -> input target, register notify;
            
            //用不用event区别？
            Cat cat = new Cat();
            People p = new People(cat);
            Mouse m = new Mouse(cat);

            cat.Yell();

            Console.Read();

        }
    }
}

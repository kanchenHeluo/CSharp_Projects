using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer
{
    public class People: Observer
    {
        public People(Target tar):base(tar)
        {

        }
        public override void Response()
        {
            Console.WriteLine("I cry");
        }

        public override void Response2()
        {
            Console.WriteLine("I run");
        }
    }
}

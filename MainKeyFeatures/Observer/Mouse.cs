using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer
{
    public class Mouse : Observer
    {
        public Mouse(Target tar) : base(tar)
        {

        }

        public override void Response()
        {
            Console.WriteLine("mouse cry");
        }

        public override void Response2()
        {
            Console.WriteLine("mouse run home");
        }

        
    }
}

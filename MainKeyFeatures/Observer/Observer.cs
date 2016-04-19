using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer
{
    public abstract class Observer
    {
        public Observer(Target target)
        {
            target.eventHandler += new Target.EventHandler(Response);
            target.eventHandler += new Target.EventHandler(Response2);
        }



        public abstract void Response();
        public abstract void Response2();
    }
}

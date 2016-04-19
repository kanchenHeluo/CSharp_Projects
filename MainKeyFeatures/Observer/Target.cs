using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer
{
    public abstract class Target
    {
        public Target()
        {

        }

        public delegate void EventHandler();
        public event EventHandler eventHandler;

        protected void Notify()
        {
            if (this.eventHandler != null)
            {
                this.eventHandler();
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    public class Producer
    {
        private Resources rr { get; set; }
        public Producer(Resources r)
        {
            rr = r;
        }

        public void produce()
        {
            int i=0;
            while(true){
                rr.Write(i);
                i = (i+1)%5;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    public class Consumer
    {
        private Resources rr { get; set; }
        public Consumer(Resources r)
        {
            rr = r;
        }

        public void consume()
        {
            while(true){
                rr.Read();
            }   
        }
    }
}

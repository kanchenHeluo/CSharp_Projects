using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Resources r = new Resources();
            Producer p = new Producer(r);
            Consumer c1 = new Consumer(r);
            Consumer c2 = new Consumer(r);

            Thread t1 = new Thread(p.produce);
            t1.Start();

            Thread t2 = new Thread(c1.consume);
            t2.Start();            

            Thread t3 = new Thread(c2.consume);
            t3.Start();
            
        }
    }
}

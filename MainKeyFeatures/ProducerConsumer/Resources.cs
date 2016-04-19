using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerConsumer
{
    public class Resources
    {
        public int[] Rs = { -1,-1,-1,-1,-1};

        public int ReadIdx = 0, WriteIdx = 0, Len=5;

        public Resources(){
           
        }

        public int Read()
        {
            Thread.Sleep(5000);
            lock (this)
            {   
                int content = Rs[ReadIdx];
                if (content == -1)
                {
                    Monitor.Wait(this);
                }

                Rs[ReadIdx] = -1;
                ReadIdx = (ReadIdx + 1) % Len;
                Console.WriteLine("read:" + content.ToString());
                Monitor.Pulse(this);
                return content;
            }
        }
        public void Write(int s)
        {
            Thread.Sleep(1000);
            lock (this)
            {               
                if (Rs[WriteIdx] != -1)
                {
                    Monitor.Wait(this);                        
                }
                Console.WriteLine("write:" + s.ToString());
                Rs[WriteIdx] = s;
                WriteIdx = (WriteIdx + 1) % Len;
                Monitor.Pulse(this);
            }
        }
    }
}

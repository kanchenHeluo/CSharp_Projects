using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateExample
{
    public class CarConditioner
    {
        public CarConditioner()
        {

        }

        public void Open(){
            Console.WriteLine("conditioner opened");
        }
        public void Close(){
            Console.WriteLine("conditioner closed");
        }
        
    }
}

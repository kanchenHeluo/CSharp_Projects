using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateExample
{
    public class CarWindow
    {
        public CarWindow()
        {

        }

        public void Open()
        {
            Console.WriteLine("window opened");
        }

        public void Close()
        {
            Console.WriteLine("window closed");
        }
    }
}

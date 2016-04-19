using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observer
{
    public class Cat :Target
    {
        public Cat()
        {

        }

        public void Yell(){
            Console.WriteLine("cat is yelling ...");
            this.Notify();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateExample
{
    public class Car
    {
        public delegate void OpenWindow();
        public OpenWindow openWindow;

        public delegate void OpenConditioner();
        public OpenConditioner openConditioner;

        CarWindow windows { get; set; }
        CarConditioner conditioners { get; set; }
        public Car()
        {
            windows = new CarWindow();
            conditioners = new CarConditioner();
            openWindow = new OpenWindow(windows.Open);
            openConditioner = new OpenConditioner(conditioners.Open);
        }

        public void OpenCarWindow()
        {
            openWindow.Invoke();
            openConditioner += new OpenConditioner(windows.Close);
        }
        public void OpenCarConditioner()
        {
            openConditioner.Invoke();
            openWindow += new OpenWindow(conditioners.Close);
        }

        public void CloseCarWindow()
        {
            windows.Close();
            openConditioner -= new OpenConditioner(windows.Close);
        }

        public void CloseCarConditioner()
        {
            conditioners.Close();
            openWindow -= new OpenWindow(conditioners.Close);
        }
    }
}

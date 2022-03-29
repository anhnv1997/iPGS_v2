using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.iZCU.Devices
{
    public class ErrorsEventArgs : EventArgs
    {
        private string errorString = "";
        public string ErrorString
        {
            get
            {
                return this.errorString;
            }
            set
            {
                this.errorString = value;
            }
        }
        public ErrorsEventArgs()
        {
        }
        public ErrorsEventArgs(string errorString)
        {
            this.errorString = errorString;
        }
    }
}

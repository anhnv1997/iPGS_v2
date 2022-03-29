using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.iZCU.Devices
{
    public interface ICameraController
    {
        event StateEventHandler StateEvent;
        event ErrorsEventHandler ErrorEvent;
        string IP
        {
            set;
            get;
        }
        int Port
        {
            set;
            get;
        }
        string Login
        {
            set;
            get;
        }
        string Password
        {
            set;
            get;
        }
        bool IsConnected
        {
            get;
        }    

        bool Connect(string ip);
        bool Disconnect();
        void PollingStart();
        void PollingStop();
        void SignalToStop();
        bool ControlLight(int color);
    }
}

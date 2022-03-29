using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.iZCU.Devices
{
    public class StateEventArgs : EventArgs
    {
        public DateTime _EventDateTime;
        public string _CameraIP;
        public string _EventStatus; // = "New Event" // Lost connection
        public int _TotalSlot;
        public int _EmptySlot;
        public string[] _ParkingNo; // Name of position
        public string[] _PlateNum; // Plate Number of Car parking
        public int[] _CarStatus;  // State of each position ( 0 = Empty, 1 = Occupy )
    }
}

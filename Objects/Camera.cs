using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kztek.iZCU.Devices;

namespace Kztek.iZCU.Objects
{
    public class Camera
    {
        public ICameraController cameraController;
        public event StateEventHandler StateEvent;
        public event ErrorsEventHandler ErrorEvent;

        private string _ID = "";
        public string ID
        {
            set { _ID = value; }
            get { return _ID; }
        }

        private CameraType _Type = CameraType.HIKVision;
        public CameraType Type
        {            
            set { _Type = value; }
            get { return _Type; }
        }

        private string _CameraName = "";
        public string CameraName
        {
            set { _CameraName = value; }
            get { return _CameraName; }
        }

        private string _IP = "";
        public string IP
        {
            set { _IP = value; }
            get { return _IP; }
        }

        private int _Port = 80;
        public int Port
        {
            set { _Port = value; }
            get { return _Port; }
        }

        private string _Login = "";
        public string Login
        {
            set { _Login = value; }
            get { return _Login; }
        }

        private string _Password = "";
        public string Password
        {
            set { _Password = value; }
            get { return _Password; }
        }

        // Event date
        #region: Last Data
        public string lastEventStatus; // = "New Event" // Lost connection
        public int lastTotalSlot;
        public int lastEmptySlot;
        public string lastParkingNo; // Name of position
        public string  lastCarStatus;  // State of each position ( 0 = Empty, 1 = Occupy )
        public string lastPlateNum; // Plate Number of Car parking
        public string lastEventDateTime;
        #endregion

        #region: Current Data
        public string currentEventStatus; // = "New Event" // Lost connection
        public int currentTotalSlot;
        public int currentEmptySlot;
        public string[] currentParkingNo; // Name of position
        public int[] currentCarStatus;  // State of each position ( 0 = Empty, 1 = Occupy )
        public string[] currentPlateNum; // Plate Number of Car parking
        public DateTime currentEventDateTime;
        #endregion

        public string ImgBase64;
        public string ImgPath;


        public void Start()
        {
            switch(_Type)
            {
                case CameraType.HIKVision:
                    cameraController = new HIKCameraController();
                    break;
                case CameraType.Dahua:
                    break;
                default:
                    break;
            }
            if(cameraController != null)
            {                
                cameraController.IP = this._IP;
                cameraController.Port = this._Port;
                cameraController.Login = this._Login;
                cameraController.Password = this._Password;
                cameraController.Connect(this._IP);
                cameraController.PollingStart();
                cameraController.StateEvent += _StateEvent; // dky event cho Controller
                cameraController.ErrorEvent += _ErrorEvent; // dky event cho Controller, thưc hiện qua hàm kích hoạt của Camera
            }
        }

        public void Stop()
        {
            if (cameraController != null)
            {
                cameraController.SignalToStop();
                cameraController.PollingStop();
                cameraController.StateEvent -=  _StateEvent;
                cameraController.ErrorEvent -=  _ErrorEvent;
                cameraController.Disconnect();
            }
        }

        private void _ErrorEvent(object sender,Devices.ErrorsEventArgs e)
        {
            if (ErrorEvent != null)
                ErrorEvent(this, e);
        }

        private void _StateEvent(object sender, StateEventArgs e)
        {
            if (StateEvent != null)
                StateEvent(this, e);
        }
    }
}

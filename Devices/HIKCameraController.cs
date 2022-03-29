using RestSharp;
using RestSharp.Authenticators.Digest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Kztek.iZCU.Objects;
using System.Data;

namespace Kztek.iZCU.Devices
{
    public class HIKCameraController : ICameraController
    {
        public event StateEventHandler StateEvent;
        public event ErrorsEventHandler ErrorEvent;

        private RestClient restClient;
        private RestRequest restRequest;

        #region _______________ Properties _______________
        string _IP = "";
        public string IP
        {
            set { _IP = value; }
            get { return _IP; }
        }
        int _Port = 80;
        public int Port
        {
            set { _Port = value; }
            get { return _Port; }
        }
        string _Login = "";
        public string Login
        {
            set { _Login = value; }
            get { return _Login; }
        }
        string _Password = "";
        public string Password
        {
            set { _Password = value; }
            get { return _Password; }
        }
        bool _IsConnected = false;
        public bool IsConnected
        {
            get { return _IsConnected; }
        }

        bool isBusy = false;
        private int _lastEmptySlot = -2;
        private string _lastEventStatus = "";
        #endregion

        private Thread thread;
        private ManualResetEvent stopEvent;
        private const string API_TEST_CONNECT = "/ISAPI/Security/sessionLogin/capabilities";
        private const string API_GET_PARKING_STATUS = "/ISAPI/Custom/SelfExt/ContentMgmt/ParkingStatus";

        #region _______________ Event _______________
        public void PollingStart()
        {
            if (this.thread == null)
            {
                this._IsConnected = false;
                this.stopEvent = new ManualResetEvent(false);
                this.thread = new Thread(new ThreadStart(this.WorkerThread));
                this.thread.Name = string.Format(CultureInfo.InvariantCulture, "HIKCamera_{0}", new object[]
                {
                    Guid.NewGuid()
                });
                this.thread.IsBackground = true;
                this.thread.Start();
            }
        }

        #region:Event
        private void WorkerThread()
        {
            while (!this.stopEvent.WaitOne(0, true))
            {
                try
                {
                    if (!isBusy)
                    {
                        isBusy = true;
                        // 20s send 1 request
                        if ((DateTime.Now.Second / 60) == 0)
                        {
                            IRestResponse response = GetApiResponse(this._Login, this._Password, Method.GET, API_TEST_CONNECT, 500);
                            if (response.IsSuccessful)
                            {
                                _IsConnected = true;
                                string content = GetApiResponse(this._Login, this.Password, Method.GET, API_GET_PARKING_STATUS, 500).Content;

                                int parkingNum = 0;
                                string[] parkingNo = new string[1];
                                string[] plateNum = new string[1];
                                int[] carStatus = new int[1];
                                if (Get_Parking_Info(content, ref parkingNum, ref parkingNo, ref plateNum, ref carStatus))
                                {
                                    ExcecuteNewEvent(parkingNum, parkingNo, plateNum, carStatus);
                                }
                            }
                            else
                            {
                                ExcecuteDisconnectEvent();
                            }
                        }
                        isBusy = false;
                    }
                }
                catch (Exception ex)
                {
                    if (this.ErrorEvent != null)
                    {
                        this.ErrorEvent(this, new ErrorsEventArgs(ex.ToString()));
                    }
                }
                finally
                {
                    isBusy = false;
                    Thread.Sleep(1000);
                }

            }
        }

        private void ExcecuteDisconnectEvent()
        {
            _IsConnected = false;
            if (_lastEventStatus != "Lost Connection")
            {
                _lastEventStatus = "Lost Connection";
                _lastEmptySlot = 0;
                StateEventArgs e = new StateEventArgs();
                e._EventDateTime = DateTime.Now;
                e._CameraIP = this.IP;
                e._EventStatus = "Lost Connection";
                e._TotalSlot = 3; // Default of HIK Camera
                e._EmptySlot = 0;
                e._ParkingNo = null;
                e._PlateNum = null;
                e._CarStatus = null;
                if (StateEvent != null)
                    StateEvent(this, e);
            }
        }
        private void ExcecuteNewEvent(int parkingNum, string[] parkingNo, string[] plateNum, int[] carStatus)
        {
            // Empty Slot
            int emptySlot = 3;
            foreach (int i in carStatus)
            {
                if (i == 1) emptySlot -= 1;
            }
            _lastEmptySlot = emptySlot;
            StateEventArgs e = new StateEventArgs();
            e._EventDateTime = DateTime.Now;
            e._CameraIP = this.IP;
            e._EventStatus = "New Event";
            e._TotalSlot = parkingNum;
            e._EmptySlot = emptySlot < parkingNum ? emptySlot : parkingNum;
            e._ParkingNo = parkingNo;
            e._PlateNum = plateNum;
            e._CarStatus = carStatus;
            if (StateEvent != null)
                StateEvent(this, e);
            _lastEventStatus = "New Event";
        }
        #region Sample
        /*
         *  <?xml version="1.0" encoding="UTF-8"?>
            <ParkingStatus>
                <ParkingNum>3</ParkingNum>
                <ParkingStatusList>
                    <ParkingStatusSingle>
                        <ParkingNo>Slot1</ParkingNo>
                        <PlateNum>No</PlateNum>
                        <CarStatus>0</CarStatus>
                        <LampFink>0</LampFink>
                        <LampColor>2</LampColor>
                        <VehicleColor></VehicleColor>
                        <AutoLog></AutoLog>
                    </ParkingStatusSingle>
                    <ParkingStatusSingle>
                        <ParkingNo>Slot2</ParkingNo>
                        <PlateNum>No</PlateNum>
                        <CarStatus>0</CarStatus>
                        <LampFink>0</LampFink>
                        <LampColor>2</LampColor>
                        <VehicleColor></VehicleColor>
                        <AutoLog></AutoLog>
                    </ParkingStatusSingle>
                    <ParkingStatusSingle>
                        <ParkingNo>Slot3</ParkingNo>
                        <PlateNum>No</PlateNum>
                        <CarStatus>0</CarStatus>
                        <LampFink>0</LampFink>
                        <LampColor>2</LampColor>
                        <VehicleColor></VehicleColor>
                        <AutoLog></AutoLog>
                    </ParkingStatusSingle>
                </ParkingStatusList>
            </ParkingStatus>
        */
        #endregion
        private bool Get_Parking_Info(string response, ref int parkingNum, ref string[] parkingNo, ref string[] plateNum, ref int[] carStatus)
        {
            bool ret = false;
            try
            {
                if (response.Contains("xml") && response.Contains("ParkingStatus") && response.Contains("ParkingStatusList"))
                {
                    System.Text.ASCIIEncoding myEncoder = new System.Text.ASCIIEncoding();
                    byte[] bytes = myEncoder.GetBytes(response);
                    MemoryStream ms = new MemoryStream(bytes);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ms);
                    XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/ParkingStatus"); //XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/ParkingStatus/ParkingNum");
                    if (nodeList.Count > 0)
                    {
                        string parkingNumStr = nodeList[0].SelectSingleNode("ParkingNum").InnerText;
                        int.TryParse(parkingNumStr, out parkingNum);
                    }
                    if (parkingNum > 0)
                    {
                        parkingNo = new string[parkingNum];
                        plateNum = new string[parkingNum];
                        carStatus = new int[parkingNum];

                        nodeList = xmlDoc.DocumentElement.SelectNodes("/ParkingStatus/ParkingStatusList/ParkingStatusSingle");
                        if (nodeList.Count == parkingNum)
                        {
                            int index = 0;
                            foreach (XmlNode node in nodeList)
                            {
                                parkingNo[index] = node.SelectSingleNode("ParkingNo").InnerText;
                                plateNum[index] = node.SelectSingleNode("PlateNum").InnerText;
                                int.TryParse(node.SelectSingleNode("CarStatus").InnerText, out carStatus[index]);
                                index += 1;
                            }
                            ret = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ErrorEvent != null)
                {
                    ErrorsEventArgs error_Args = new ErrorsEventArgs();
                    error_Args.ErrorString = "Get_Parking_Info: " + ex.ToString();
                    ErrorEvent(this, error_Args);
                }
            }
            return ret;
        }
        #endregion


        #endregion

        #region: Connect       
        public bool Connect(string ip)
        {
            this._IP = ip;
            return IsPingSuccess(ip);
        }
        #endregion

        #region:Internal
        private IRestResponse GetApiResponse(string username, string password, Method method, string APIcommand, int timeOut)
        {
            restClient = new RestClient(GetAPICommand(APIcommand, this._IP, this._Port));
            restClient.Authenticator = new DigestAuthenticator(username, password);
            restClient.Timeout = timeOut;
            restRequest = new RestRequest(method);
            return restClient.Execute(restRequest);
        }
        private string GetAPICommand(string API, string ip, int port)
        {
            return $"http://" + ip + ":" + port + API;
        }
        private static bool IsPingSuccess(string ip)
        {
            Ping pingSender = new Ping();
            PingReply reply = null;
            reply = pingSender.Send(ip, 500);
            bool result = reply != null && reply.Status == IPStatus.Success;
            return result;
        }
        #endregion

        #region:Thread
        public bool IsRunning
        {
            get
            {
                if (this.thread != null)
                {
                    if (!this.thread.Join(0))
                    {
                        return true;
                    }
                    this.Free();
                }
                return false;
            }
        }
        public bool Disconnect()
        {
            bool result;
            try
            {
                result = true;
            }
            catch (Exception ex)
            {
                if (this.ErrorEvent != null)
                {
                    this.ErrorEvent(this, new ErrorsEventArgs(ex.ToString()));
                }
                result = false;
            }
            return result;
        }
        public void SignalToStop()
        {
            this._IsConnected = false;
        }
        private void WaitForStop()
        {
            if (this.thread != null)
            {
                this.thread.Join();
                this.Free();
            }
        }
        private void Free()
        {
            this.thread = null;
            this.stopEvent.Close();
            this.stopEvent = null;
        }
        public void PollingStop()
        {
            if (this.thread != null)
            {
                this.stopEvent.Set();
            }
            if (this.IsRunning)
            {
                this.WaitForStop();
            }
        }
        #endregion

        public bool ControlLight(int color)
        {
            return false;
        }
    }
}

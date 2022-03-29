using RestSharp;
using RestSharp.Authenticators.Digest;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Kztek.iZCU.Objects;
using Kztek.iZCU.Devices;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Kztek.iZCU
{
    public partial class frmMainForm : Form
    {
        public static Configuration configs = new Configuration();
        private BackgroundWorker udpServer_Worker;
        private const int RED = 32769;
        private const int GREEN = 32770;
        private const int YELLOW = 32771;
        #region __________________________Form__________________________
        public frmMainForm()
        {
            InitializeComponent();
            UISync.Init(this);
            InitializeUdpServer_Worker();
        }
        private void frmMainForm_Load(object sender, EventArgs e)
        {
            configs.Logger_Info("Start App");
            configs.LoadCameras();
            StartCamera();
            SetInfo(DateTime.Now, "", "", "", "", "", "");
            deviceTreeView1.BuildTreeView();
            timerStatus.Start();
            Start_TCPServer();
            deviceTreeView1.ExpandAll();
        }
        private void frmMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerStatus.Stop();
            StopCamera();
            CloseUdpServer_Worker();
            configs.Logger_Info("Close App");
        }
        #endregion

        //Truyền nhận dữ liệu
        #region __________________________UDP,TCP server worker__________________________

        private Socket newsock, socket_listener;
        int udpServerPort = 100;
        private void Start_TCPServer()
        {
            DataTable dataTable = configs.GetTable_AccessConnection("Select * from tblComputer");
            string IP = "";
            int Port = 0;
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                IP = row["IP"].ToString();
                Port = Convert.ToInt32(row["Port"].ToString());
                var localIP = IPAddress.Parse(IP);
                var LocalPort = Port;
                var localEndPoint = new IPEndPoint(localIP, LocalPort);
                socket_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    socket_listener.Bind(localEndPoint);
                    socket_listener.Listen(10);
                    configs.Logger_Info("Khởi tạo TCP Server thành công");
                }
                catch
                {
                    configs.Logger_Error("Thiết lập sai IP");
                }

                if (udpServer_Worker != null && !udpServer_Worker.IsBusy)
                    udpServer_Worker.RunWorkerAsync();
            }
        }
        // Init database background worker
        private void InitializeUdpServer_Worker()
        {
            try
            {
                this.udpServer_Worker = new BackgroundWorker();
                this.udpServer_Worker.WorkerSupportsCancellation = true;
                this.udpServer_Worker.WorkerReportsProgress = true;
                this.udpServer_Worker.DoWork += new DoWorkEventHandler(this.udpServer_Worker_DoWork);
                this.udpServer_Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.udpServer_Worker_RunWorkerCompleted);
                this.udpServer_Worker.ProgressChanged += new ProgressChangedEventHandler(this.udpServer_Worker_ProgressChanged);
            }
            catch (Exception ex)
            {
                configs.Logger_Error(ex.ToString());
            }
        }

        // copy dowork
        bool isLock = false;

        private void udpServer_Worker_DoWork(object sender1, DoWorkEventArgs e)
        {
            try
            {
                if (!isLock)
                {
                    isLock = true;
                    BackgroundWorker worker = sender1 as BackgroundWorker;
                    if (!worker.CancellationPending)
                    {
                        int recv;
                        byte[] data = new byte[1024];

                        while (true)
                        {
                            var socket = socket_listener.Accept();
                            #region: __________________________Receive command__________________________
                            data = new byte[1024];
                            recv = socket.Receive(data);
                            socket.Shutdown(SocketShutdown.Receive);
                            string receiceCMD = Encoding.ASCII.GetString(data, 0, recv);
                            #endregion
                            #region __________________________Command = GetStateAllSensor?/__________________________
                            if (receiceCMD != null && receiceCMD.Equals("GetStateAllSensor?/"))
                            {
                                ExcecuteGetAllStatusEvent(socket, receiceCMD);
                            }
                            #endregion
                            else if (receiceCMD.Contains("GetInfor"))
                            {
                                configs.Logger_Info($"Receive Command{receiceCMD}");
                                string IP = receiceCMD.Substring(12, receiceCMD.IndexOf("A") - 12);
                                string Address = receiceCMD.Substring(receiceCMD.IndexOf("A") + 2, 2);
                                int AddressInt = Convert.ToInt32(Address);

                                // 96 Zone <=> 32 cam
                                // 3 Zone có chung 1 hình ảnh
                                int addr1, addr2; // addr1:Vi tri camera; addr2: Vi tri zone
                                addr2 = AddressInt % 3 == 0 ? 0 : AddressInt % 3;
                                addr1 = AddressInt % 3 == 0 ? AddressInt / 3 - 1 : AddressInt / 3;
                                // Get IP Camera
                                if (configs.cameras != null)
                                {
                                    if (addr1 >= configs.cameras.Count)
                                    {
                                        ExcecuteNoValidEvent(socket, addr1);
                                    }
                                    else
                                    {
                                        Camera camera = configs.cameras[addr1];
                                        //if (camera == null)
                                        //{
                                        //    ExcecuteNoValidEvent(socket, addr1);
                                        //}
                                        //else
                                        DataTable dtGetImage = configs.GetTable_AccessConnection($"select * from tblCamera WHERE IP = '{camera.IP}' order by ID");
                                        DataTable dtGetStatus = configs.GetTable_AccessConnection($"select * from tblCameraEvent WHERE IP = '{camera.IP}' order by ID");

                                        if (dtGetImage == null || dtGetImage.Rows.Count == 0 || dtGetStatus == null || dtGetStatus.Rows.Count == 0)
                                        {
                                            ExcecuteNoValidEvent(socket, addr1);
                                        }
                                        else
                                        {
                                            #region: Get ImagePath
                                            DataRow dtrImage = dtGetImage.Rows[dtGetImage.Rows.Count - 1];
                                            camera.ImgPath = dtrImage["ImagePath"].ToString();
                                            if (camera.ImgPath == "")
                                            {
                                                configs.Logger_Error("Hình ảnh không có sẵn");
                                            }
                                            #endregion

                                            #region: Get Address Status
                                            DataRow dtrStatus = dtGetStatus.Rows[dtGetStatus.Rows.Count - 1];
                                            string currentCarStatus = "";
                                            string currentPlateNum = "";
                                            if (dtrStatus["CarStatus"].ToString() == "")
                                            {
                                                configs.Logger_Error("Status không có sẵn");
                                                return;
                                            }
                                            switch (dtrStatus["CarStatus"].ToString().Length)
                                            {
                                                case 1:
                                                    switch (addr2)
                                                    {
                                                        case 1:
                                                            //vi tri 1
                                                            currentCarStatus = dtrStatus["CarStatus"].ToString().Substring(0, 1);
                                                            currentPlateNum = dtrStatus["PlateNumber"].ToString().Substring(0);
                                                            break;
                                                        default:
                                                            currentCarStatus = "-1";
                                                            currentPlateNum = "No";
                                                            break;
                                                    }
                                                    break;
                                                case 3:
                                                    switch (addr2)
                                                    {
                                                        case 1:
                                                            //vi tri 1
                                                            currentCarStatus = dtrStatus["CarStatus"].ToString().Substring(0, 1);
                                                            currentPlateNum = dtrStatus["PlateNumber"].ToString().Substring(0, dtrStatus["PlateNumber"].ToString().IndexOf("|"));
                                                            break;
                                                        case 2:
                                                            currentCarStatus = dtrStatus["CarStatus"].ToString().Substring(2, 1);
                                                            currentPlateNum = dtrStatus["PlateNumber"].ToString().Substring(dtrStatus["PlateNumber"].ToString().IndexOf("|") + 1);
                                                            break;
                                                        default:
                                                            currentCarStatus = "-1";
                                                            currentPlateNum = "No";
                                                            break;

                                                    }
                                                    break;
                                                case 5:
                                                    switch (addr2)
                                                    {
                                                        case 1:
                                                            //vi tri 1
                                                            currentCarStatus = dtrStatus["CarStatus"].ToString().Substring(0, 1);
                                                            currentPlateNum = dtrStatus["PlateNumber"].ToString().Substring(0, dtrStatus["PlateNumber"].ToString().IndexOf("|"));
                                                            break;
                                                        case 2:
                                                            currentCarStatus = dtrStatus["CarStatus"].ToString().Substring(2, 1);
                                                            string temp2 = dtrStatus["PlateNumber"].ToString().Substring(dtrStatus["PlateNumber"].ToString().IndexOf("|") + 1);
                                                            currentPlateNum = temp2.Substring(0, temp2.IndexOf("|"));
                                                            break;
                                                        case 0:
                                                            currentCarStatus = dtrStatus["CarStatus"].ToString().Substring(4, 1);
                                                            string temp3 = dtrStatus["PlateNumber"].ToString().Substring(dtrStatus["PlateNumber"].ToString().IndexOf("|") + 1);
                                                            currentPlateNum = temp3.Substring(temp3.IndexOf("|") + 1);

                                                            break;
                                                        default:
                                                            currentCarStatus = "-1";
                                                            currentPlateNum = "No";
                                                            break;
                                                    }
                                                    break;
                                            }
                                            #endregion
                                            string cmdSendImagePath = "ImagePath" + camera.ImgPath + "," + currentCarStatus + "," + currentPlateNum;
                                            byte[] ImgSendByte = Encoding.UTF8.GetBytes(cmdSendImagePath);
                                            int byteSend = socket.Send(ImgSendByte);
                                            socket.Shutdown(SocketShutdown.Send);
                                            if (byteSend == ImgSendByte.Length)
                                            {
                                                configs.Logger_Info("Send Data Success: " + cmdSendImagePath);
                                            }

                                        }
                                    }

                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                configs.Logger_Error(ex.ToString());
                isLock = false;
            }
        }

        private static void ExcecuteGetAllStatusEvent(Socket socket, string command)
        {
            configs.Logger_Info($"Nhận lệnh {command}");
            // Send back
            //0x02 C0~C30 0x03
            byte[] array = new byte[34];
            array[0] = 0x02; // start of text
            array[33] = 0x03; // end of text
            int index = 0;
            foreach (Camera camera in configs.cameras)
            {
                configs.LoadCameraLastEvent(camera);
                string _status2 = "";
                string status1 = "";
                string status2 = "";
                string status3 = "";
                index++;
                if (index <= 30)
                {
                    string carStatus_Str = "01";
                    if (camera.lastCarStatus != null)
                    {
                        switch (camera.lastCarStatus.Length)
                        {
                            case 1:
                                //MessageBox.Show("case1");
                                carStatus_Str += camera.lastCarStatus.Equals("0") ? "00" + "1010" : "01" + "1010";
                                break;
                            case 3:
                                status1 = camera.lastCarStatus.Substring(0, camera.lastCarStatus.IndexOf('|'));
                                status2 = camera.lastCarStatus.Substring(camera.lastCarStatus.IndexOf("|") + 1, camera.lastCarStatus.Length - camera.lastCarStatus.IndexOf("|") - 1);
                                carStatus_Str += status1.Equals("0") ? "00" : "01";
                                carStatus_Str += status2.Equals("0") ? "00" + "10" : "01" + "10";
                                break;
                            case 5:
                                status1 = camera.lastCarStatus.Substring(0, camera.lastCarStatus.IndexOf('|'));
                                _status2 = camera.lastCarStatus.Substring(camera.lastCarStatus.IndexOf("|") + 1, camera.lastCarStatus.Length - camera.lastCarStatus.IndexOf("|") - 1);
                                status2 = _status2.Substring(0, _status2.IndexOf("|"));
                                status3 = _status2.Substring(camera.lastCarStatus.IndexOf("|") + 1, _status2.Length - _status2.IndexOf("|") - 1);
                                carStatus_Str += status1.Equals("0") ? "00" : "01";
                                carStatus_Str += status2.Equals("0") ? "00" : "01";
                                carStatus_Str += status3.Equals("0") ? "00" : "01";
                                break;
                        }
                    }
                    else
                    {
                        carStatus_Str = "01101010";
                    }
                    if (carStatus_Str.Length == 8)
                    {
                        array[index] = (byte)(Convert.ToInt64(carStatus_Str, 2));
                    }
                }
                // Send to CCU                                    
            }
            socket.Send(array);
            socket.Shutdown(SocketShutdown.Send);
            System.Threading.Thread.Sleep(2);
        }

        private static void ExcecuteNoValidEvent(Socket socket, int addr1)
        {
            configs.Logger_Error($"Camera address {addr1} chưa có trong CSDL");
            string smdSendNoCameraValid = "No Valid";
            byte[] NoValidSendByte = Encoding.UTF8.GetBytes(smdSendNoCameraValid);

            socket.Send(NoValidSendByte);
            socket.Shutdown(SocketShutdown.Send);
        }

        private void udpServer_Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }
        private void udpServer_Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
        public void CloseUdpServer_Worker()
        {
            if (udpServer_Worker != null)
            {
                this.udpServer_Worker.DoWork -= new DoWorkEventHandler(this.udpServer_Worker_DoWork);
                this.udpServer_Worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(this.udpServer_Worker_RunWorkerCompleted);
                this.udpServer_Worker.ProgressChanged -= new ProgressChangedEventHandler(this.udpServer_Worker_ProgressChanged);
                udpServer_Worker.CancelAsync();
                udpServer_Worker = null;
            }
        }

        #endregion        
        // Xử lý Event
        #region __________________________Camera__________________________
        private void StartCamera()
        {
            foreach (Camera camera in configs.cameras)
            {
                camera.Start();
                camera.ErrorEvent += Camera_ErrorEvent;
                camera.StateEvent += Camera_StateEvent;
            }
            System.Threading.Thread.Sleep(5);
        }

        private void StopCamera()
        {
            foreach (Camera camera in configs.cameras)
            {
                camera.ErrorEvent -= Camera_ErrorEvent;
                camera.StateEvent -= Camera_StateEvent;
                camera.Stop();
            }
        }

        private void Camera_ErrorEvent(object sender, ErrorsEventArgs e)
        {
            configs.Logger_Error("Camera Error: " + e.ErrorString);
        }
        private void Camera_StateEvent(object sender, StateEventArgs currentEventData)
        {
            // Log
            configs.Logger_Info("New_Camera_State_Event: " + currentEventData._CameraIP + " - " + currentEventData._EventStatus + " - EmptySlot: " + currentEventData._EmptySlot);
            // Process
            Camera cam = configs.cameras.GetCameraByIP(currentEventData._CameraIP);
            if (cam != null)
            {
                // Get Last Data
                configs.LoadCameraLastEvent(cam);
                // Get Current Data
                Configuration.GetCurrentData(currentEventData, cam);
                //Check Same Data
                string currentParkingNo_str = "", currentCarStatus_str = "", currentPlateNum_str = "";
                // Check trung dữ liệu
                if (cam.currentParkingNo == null || cam.currentCarStatus == null || cam.currentPlateNum == null)
                {
                    return;
                }
                if (IsNewEvent(cam, ref currentParkingNo_str, ref currentCarStatus_str, ref currentPlateNum_str))
                {
                    //Save Event
                    currentParkingNo_str = "";
                    currentCarStatus_str = "";
                    currentPlateNum_str = "";
                    currentParkingNo_str = SplitArray<string>(cam.currentParkingNo, "|");
                    currentCarStatus_str = SplitArray<int>(cam.currentCarStatus, "|");
                    for (int i = 0; i < cam.currentPlateNum.Length; i++)
                    {
                        string data = cam.currentPlateNum[i];
                        if (i < cam.currentPlateNum.Length - 1)
                        {
                            if (IsPlateNumber(data))
                            {
                                currentPlateNum_str += standardizedPlateNumber(data).Substring(0, 3) + "-" + standardizedPlateNumber(data).Substring(3) + "|";
                            }
                            else
                            {
                                currentPlateNum_str += data + "|";
                            }
                        }
                        currentPlateNum_str = currentPlateNum_str.Substring(0, currentPlateNum_str.Length - 1);
                    }
                    if (!configs.Insert_CameraEvent(cam.IP, currentEventData._EventDateTime, currentEventData._TotalSlot, currentEventData._EmptySlot, currentParkingNo_str, currentCarStatus_str, currentPlateNum_str))
                    {
                        if (!configs.Insert_CameraEvent(cam.IP, currentEventData._EventDateTime, currentEventData._TotalSlot, currentEventData._EmptySlot, currentParkingNo_str, currentCarStatus_str, currentPlateNum_str))
                        {
                            configs.Logger_Error("Thêm dữ liệu Event không thành công");
                            MessageBox.Show("Thêm dữ liệu Event không thành công");
                        }
                    }
                    //Update Image Path
                    string imageSavePath = "";
                    GetEventImagePath(cam, ref imageSavePath);
                    UpdateEventImagePath(cam, imageSavePath);
                    //Display Event
                    // Set Info
                    string[] getPlateNum = currentPlateNum_str.Split("|");
                    string plateNumber1 = "", plateNumber2 = "", plateNumber3 = "";
                    if (currentEventData._PlateNum != null)
                    {
                        if (currentEventData._PlateNum.Length > 0) plateNumber1 = getPlateNum[0];
                        if (currentEventData._PlateNum.Length > 1) plateNumber2 = getPlateNum[1];
                        if (currentEventData._PlateNum.Length > 2) plateNumber3 = getPlateNum[2];
                        UISync.Execute(() =>
                        {
                            foreach (TreeNode treeNode in deviceTreeView1.Nodes)
                            {
                                foreach (TreeNode deviceNode in treeNode.Nodes)
                                {
                                    if (deviceNode.Name == cam.ID)
                                    {
                                        deviceNode.Nodes.Clear();
                                        string[] str = currentPlateNum_str.Split("|");
                                        foreach (string s in str)
                                        {
                                            TreeNode childNode = new TreeNode();
                                            childNode.Text = s;
                                            childNode.Name = "Zone:" + cam.IP + "";
                                            if (childNode.Text.Length > 2)
                                            {
                                                childNode.ImageIndex = 3;
                                                childNode.SelectedImageIndex = 3;
                                            }
                                            else
                                            {
                                                childNode.ImageIndex = 4;
                                                childNode.SelectedImageIndex = 4;
                                            }
                                            deviceNode.Nodes.Add(childNode);
                                        }
                                    }
                                }

                            }
                            deviceTreeView1.ExpandAll();
                        });
                    }
                    SetInfo(currentEventData._EventDateTime, cam.CameraName, currentEventData._EmptySlot + "/" + currentEventData._TotalSlot, plateNumber1, plateNumber2, plateNumber3, imageSavePath);


                    // Add to GridView
                    string eventString = currentEventData._EventStatus + " - " + plateNumber1 + " | " + plateNumber2 + " | " + plateNumber3;
                    AddInfo_To_GridView(currentEventData._EventDateTime, cam.CameraName, eventString);

                    //Change Cam Color
                    #region: __________________________Control Color Camera__________________________
                    int OccupiedSlot = 3;
                    for (int k = 0; k < cam.currentPlateNum.Length; k++)
                    {
                        if (cam.currentPlateNum[k].Equals("No"))
                            OccupiedSlot--;
                    }

                    switch (OccupiedSlot)
                    {
                        case 3:
                            //UISync.Execute(() => setColorCam(cam.IP, cam.Port, RED));
                            //setColorCam(37769);
                            break;
                        case 0:
                            //UISync.Execute(() => setColorCam(cam.IP, cam.Port, GREEN));
                            //setColorCam(37770);
                            break;
                        default:
                            // UISync.Execute(() => setColorCam(cam.IP, cam.Port, YELLOW));
                            //setColorCam(37771);
                            break;
                    }
                    #endregion
                }
            }
        }

        #region:Internal
        public bool IsNewEvent(Camera cam, ref string currentParkingNo_str, ref string currentCarStatus_str, ref string currentPlateNum_str)
        {
            if (IsSameTotalSlot(cam))
            {
                if (IsSameEmptySlot(cam))
                {
                    currentParkingNo_str = SplitArray<string>(cam.currentParkingNo, "|");
                    bool isSameParkingNo = cam.lastParkingNo.Equals(currentParkingNo_str);
                    if (isSameParkingNo)
                    {
                        currentCarStatus_str = SplitArray<int>(cam.currentCarStatus, "|");
                        bool isSameCarStatus = cam.lastCarStatus.Equals(currentCarStatus_str);
                        if (isSameCarStatus)
                        {
                            for (int i = 0; i < cam.currentPlateNum.Length; i++)
                            {
                                string data = cam.currentPlateNum[i];
                                if (i < cam.currentPlateNum.Length - 1)
                                {
                                    if (IsPlateNumber(data))
                                    {
                                        currentPlateNum_str += standardizedPlateNumber(data).Substring(0, 3) + "-" + standardizedPlateNumber(data).Substring(3) + "|";
                                    }
                                    else
                                    {
                                        currentPlateNum_str += data + "|";
                                    }
                                }
                                else
                                {
                                    if (IsPlateNumber(data))
                                    {
                                        currentPlateNum_str += standardizedPlateNumber(data).Substring(0, 3) + "-" + standardizedPlateNumber(data).Substring(3);
                                    }
                                    else
                                    {
                                        currentPlateNum_str += data;
                                    }
                                }
                            }
                            bool isSamePlateNumber = cam.lastPlateNum.Equals(currentPlateNum_str);
                            if (isSamePlateNumber)
                                return false;
                        }
                    }
                }
            }
            return true;
        }
        private static bool IsSameEmptySlot(Camera cam)
        {
            return cam.lastEmptySlot == cam.currentEmptySlot;
        }
        private static bool IsSameTotalSlot(Camera cam)
        {
            return cam.lastTotalSlot == cam.currentTotalSlot;
        }
        public void GetEventImagePath(Camera cam, ref string imagePath)
        {
            #region __________________________Get Image From API__________________________
            RestClient client = new($"http://{cam.IP}:{cam.Port}/ISAPI/Streaming/channels/1/picture")
            {
                Authenticator = new DigestAuthenticator("admin", "Kztek123456"),
                Timeout = 1200
            };
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            //Kiểm tra có lấy được dữ liệu hình ảnh hay không?
            if (!response.IsSuccessful)
            {
                client.Timeout = 1500;
                request = new RestRequest(Method.GET);
                response = client.Execute(request);
                if (!response.IsSuccessful)
                {
                    imagePath = "";
                    configs.Logger_Error("Không lấy được hình ảnh");
                    return;
                }
            }
            // Get hình ảnh, cập nhât cơ sở dữ liệu
            var fileBytes = client.DownloadData(new RestRequest(Method.GET));
            string filePathSave = @".\Data";
            string fileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".png";
            imagePath = Path.Combine(filePathSave, fileName);
            File.WriteAllBytes(imagePath, fileBytes);
            #endregion

        }
        private static string standardizedPlateNumber(string data)
        {
            if (data.Contains("O"))
            {
                data = data.Replace("O", "0");
            }
            int x = 0;
            if (!int.TryParse(data.Substring(0, 1), out x))
            {
                string temp1 = data.Substring(0, 1);
                string temp2 = data.Substring(2, 1);
                data = data.Substring(1, 1) + temp2 + temp1 + data.Substring(3);
            }
            return data;
        }
        private static bool IsPlateNumber(string data)
        {
            return data != "No" && data != "";
        }
        private static string SplitArray<ArrayType>(ArrayType[] arrayString, string splitChar)
        {
            string data = "";
            for (int i = 0; i < arrayString.Length; i++)
            {
                if (i < arrayString.Length - 1)
                {
                    data += arrayString[i] + splitChar;
                }
                else
                {
                    data += arrayString[i];
                }
            }
            return data;
        }
        private static void UpdateEventImagePath(Camera cam, string imageSavePath)
        {
            if (!configs.Update_Camera_Image(cam.IP, Path.GetFullPath(imageSavePath)))
            {
                {
                    configs.Logger_Error("Cập nhật dữ liệu Image không thành công");
                    MessageBox.Show("Cập nhật dữ liệu Image không thành công");
                }
            }
        }
        #endregion

        #endregion

        public void TsmSetting_Click(object sender, EventArgs e)
        {
            frmSetting frm = new frmSetting();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Restart app to apply changes");
            }
        }

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            deviceTreeView1.UpdateTreeView();
        }

        #region:Multy Thread
        private class UISync
        {
            private static ISynchronizeInvoke Sync;
            public static void Init(ISynchronizeInvoke sync)
            {
                Sync = sync;
            }
            public static void Execute(Action action)
            {
                Sync.BeginInvoke(action, null);
            }
        }
        #endregion
        private void SetInfo(DateTime eventDateTime, string cameraName, string emptySlot, string plateNumber1, string plateNumber2, string plateNumber3, string ImageSavePath)
        {
            if (cameraName != "")
            {
                if (!ImageSavePath.Equals(""))
                {
                    UISync.Execute(() =>
                    {
                        try
                        {
                            FileStream fs = new FileStream(ImageSavePath, FileMode.Open, FileAccess.Read);
                            pictureBox1.Image = Image.FromStream(fs);
                            fs.Close();
                            fs.Dispose();
                        }
                        catch
                        {
                            configs.Logger_Error("Load Image Error");
                        }
                    });
                }
                UISync.Execute(() =>
                {
                    lblEventDateTime.Text = eventDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    lblCameraName.Text = cameraName;
                    lblEmptySlot.Text = emptySlot;
                    lblPlateNumber1.Text = plateNumber1;
                    lblPlateNumber2.Text = plateNumber2;
                    lblPlateNumber3.Text = plateNumber3;
                });
            }
            else
            {
                UISync.Execute(() =>
                {
                    lblEventDateTime.Text = "...";
                    lblCameraName.Text = "...";
                    lblEmptySlot.Text = "...";
                    lblPlateNumber1.Text = "...";
                    lblPlateNumber2.Text = "...";
                    lblPlateNumber3.Text = "...";
                });
            }
        }

        private void AddInfo_To_GridView(DateTime eventDateTime, string cameraName, string eventString)
        {
            UISync.Execute(() =>
            {
                dataGridViewEvent.Rows.Add(dataGridViewEvent.RowCount + 1, eventDateTime.ToString("yyyy-MM-dd HH:mm:ss"), cameraName, eventString);
            });
        }

        private void deviceTreeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Camera camera = configs.cameras.GetCameraByID(e.Node.Name);
            if (camera != null)
            {
                if (camera.cameraController.IsConnected)
                {
                    frmLiveView frm = new frmLiveView(camera.IP, camera.Port, camera.Login, camera.Password);
                    frm.Text = camera.CameraName;
                    frm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Thiết bị chưa được kết nối");
                }
            }
            else
            {
                if (e.Node.Name.Contains("Zone"))
                {
                    string ip = e.Node.Name.Substring(e.Node.Name.IndexOf(":") + 1);
                    DataTable dtGetImage = configs.GetTable_AccessConnection($"select * from tblCamera WHERE IP = '{ip}' order by ID");
                    if (dtGetImage != null && dtGetImage.Rows.Count > 0)
                    {
                        int lastImageRow = dtGetImage.Rows.Count - 1;
                        DataRow dtrImage = dtGetImage.Rows[lastImageRow];
                        string _ImgPath = dtrImage["ImagePath"].ToString();
                        Image img = Image.FromFile(_ImgPath);
                        frmShowPIC frm = new frmShowPIC();
                        frm.setImage(img);
                        frm.ShowDialog();
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                frmShowPIC frm = new frmShowPIC();
                frm.setImage(pictureBox1.Image);
                frm.ShowDialog();
            }
        }

        public void SetCameraColor(string IP, int port, int color)
        {
            var client = new RestClient($"http://{IP}:{port}/ISAPI/Custom/SelfExt/ContentMgmt/LampCtrl");
            client.Authenticator = new DigestAuthenticator("admin", "Kztek123456");
            client.Timeout = 500;
            var request = new RestRequest(Method.PUT);
            string xml = $"<?xml version=\"1.0\" encoding=\"UTF - 8\"?><LampCtrlDescription><LampCtrlMode>1</LampCtrlMode><ParkingTotalNum>3</ParkingTotalNum><nocarLampStats>32768</nocarLampStats><haveCarLampStats>32768</haveCarLampStats><pressLineLampStatus>32768</pressLineLampStatus><specialParkingLampStatus>{color}</specialParkingLampStatus></LampCtrlDescription>";
            request.AddParameter("text/xml", xml, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }
    }
}

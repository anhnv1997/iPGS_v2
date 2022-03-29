using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Kztek.iZCU
{
    public partial class frmLiveView : Form
    {
        int userID = -1;
        int mrealHandle = -1;
        string cameraIP = "";
        int cameraPort = 0;
        string cameraUserName = "";
        string cameraPassword = "";
        public frmLiveView()
        {
            InitializeComponent();
        }
        public frmLiveView(string IP, int port, string username, string password)
        {
            InitializeComponent();
            this.cameraIP = IP;
            this.cameraPort = port;
            this.cameraUserName = username;
            this.cameraPassword = password;
            UISync.Init(this);
        }
        void LoadCamera()
        {
            HIKSDK.NET_DVR_Init();
            string DVRIPAddress = this.cameraIP;
            int DVRPortNumber = 8005;
            string DVRUserName = this.cameraUserName;
            string DVRPassword = this.cameraPassword;

            HIKSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new HIKSDK.NET_DVR_DEVICEINFO_V30();

            // Login the device
            userID = HIKSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref DeviceInfo);
            if (userID < 0)
            {
                pictureBox1.Image = null;
                MessageBox.Show(HIKSDK.NET_DVR_GetLastError() + "");
                return;
            }
            else
            {
                if (mrealHandle < 0)
                {
                    UISync.Execute(() =>
                    {
                        try
                        {
                            HIKSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new HIKSDK.NET_DVR_PREVIEWINFO();
                            lpPreviewInfo.hPlayWnd = pictureBox1.Handle;
                            lpPreviewInfo.lChannel = 0;
                            lpPreviewInfo.dwStreamType = 0;
                            lpPreviewInfo.dwLinkMode = 0;
                            lpPreviewInfo.bBlocked = true;
                            lpPreviewInfo.dwDisplayBufNum = 1;
                            lpPreviewInfo.byProtoType = 0;
                            lpPreviewInfo.byPreviewMode = 0;

                            IntPtr pUser = new IntPtr();

                            // Start live view 
                            mrealHandle = HIKSDK.NET_DVR_RealPlay_V40(userID, ref lpPreviewInfo, null/*RealData*/, pUser);
                        }
                        catch
                        {
                            MessageBox.Show("Liveview Error");
                        }   
                    });
                }
            }
        }
        private void frmLiveView_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                LoadCamera();
            });
            t.Start();

        }
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (userID >= 0 && mrealHandle >= 0)
            {
                HIKSDK.NET_DVR_StopRealPlay(mrealHandle);
                HIKSDK.NET_DVR_Logout(userID);
                mrealHandle = -1;
                userID = -1;
            }
        }
    }
}

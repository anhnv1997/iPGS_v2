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
using Kztek.iZCU.Objects;
namespace Kztek.iZCU
{
    public partial class frmCamera : Form
    {
        Configuration configs = new Configuration();
        private CameraView currentCamera;
        private Camera insertCamera;
        public frmCamera()
        {
            InitializeComponent();
        }
        public frmCamera(CameraView cam)
        {
            InitializeComponent();
            this.currentCamera = cam;
            if (cam != null)
            {
                this.currentCamera = cam;
                tbID.Text = cam.CameraId.ToString();
                tbCameraIP.Text = cam.CameraIP;
                tbCameraName.Text = cam.CamerName;
                cbCameraType.Text = cam.CameraType;
                tbCameraPort.Text = cam.CameraPort.ToString();
                tbCameraUsername.Text = cam.CameraUserame;
                tbCameraPassword.Text = cam.cameraPassword;
            }                        
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public Camera getInsertCamera()
        {
            Camera camera = new Camera();
            camera.CameraName = tbCameraName.Text;
            switch (cbCameraType.Text)
            {
                case "HIKVision":
                    camera.Type = CameraType.HIKVision;
                    break;
                case "Dahua":
                    camera.Type = CameraType.Dahua;
                    break;
                default:
                    break;

            }
            camera.IP = tbCameraIP.Text;
            camera.Port = Convert.ToInt32(tbCameraPort.Text);
            camera.Login = tbCameraUsername.Text;
            camera.Password = tbCameraPassword.Text;
            return camera;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbCameraName.Text.Trim().Equals(""))
            {
                MessageBox.Show("Tên Camera không được để trống");
                return;
            }
            if (tbCameraIP.Text.Trim().Equals(""))
            {
                MessageBox.Show("IP Camera không được để trống");
                return;
            }
            if (tbCameraPort.Text.Trim().Equals(""))
            {
                MessageBox.Show("Port Camera không được để trống");
                return;
            }
            if (currentCamera == null)
            {
                //Insert
                var nullValue = DBNull.Value.ToString();
                if (configs.ExecuteCommand_AccessConnection($"Insert into tblCamera(CameraType,CameraName,[IP],[Port],[Login],[Password],[ImagePath]) values('{cbCameraType.Text}','{tbCameraName.Text}','{tbCameraIP.Text}',{Convert.ToInt32(tbCameraPort.Text)},'{tbCameraUsername.Text}','{tbCameraPassword.Text}','{nullValue}')"))
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                //Update
                if (configs.ExecuteCommand_AccessConnection($"Update tblCamera set CameraType ='{cbCameraType.Text}',CameraName='{tbCameraName.Text}',IP='{tbCameraIP.Text}',Port={Convert.ToInt32(tbCameraPort.Text)},[Login]='{tbCameraUsername.Text}',[Password]='{tbCameraPassword.Text}' where IP = '{currentCamera.CameraIP}'"))
                {
                    currentCamera.CamerName = tbCameraName.Text;
                    currentCamera.CameraType = cbCameraType.Text;
                    currentCamera.CameraIP = tbCameraIP.Text;
                    currentCamera.CameraPort = Convert.ToInt32(tbCameraPort.Text);
                    currentCamera.CameraUserame = tbCameraUsername.Text;
                    currentCamera.cameraPassword = tbCameraPassword.Text;
                    this.DialogResult = DialogResult.OK;
                }
            }
            if (this.DialogResult != DialogResult.OK)
            {                
                MessageBox.Show("Lưu dữ liệu không thành công!", "Lưu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}

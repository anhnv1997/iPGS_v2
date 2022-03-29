using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kztek.iZCU.Objects;

namespace Kztek.iZCU
{
    public partial class frmSetting : Form
    {
        Configuration configs = new Configuration();
        CameraView SelectedCamera;

        Camera camera = new Camera(); 
        public frmSetting()
        {
            InitializeComponent();
            LoadComputer();
            this.Shown += FrmSetting_Shown;            
        }
        private void FrmSetting_Shown(object sender, EventArgs e)
        {
            GetGridView();
            
        }
        //Add
        private void tsbAdd_Click(object sender, EventArgs e)
        {
            frmCamera frm = new frmCamera(null);
            if (frm.ShowDialog() == DialogResult.OK)
            {                
                dgvCameraShow.DataSource = null;
                // call get insert Camera
                Camera camera = frm.getInsertCamera();
                configs.cameras.Add(camera);
                dgvCameraShow.DataSource = configs.cameras;
            }                
        }

        //Refresh
        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            dgvCameraShow.DataSource = null;            
            configs.cameras.Clear();
            configs.LoadCameras();
            dgvCameraShow.DataSource = configs.cameras;
        }

        // Delete
        private void tbsDelete_Click(object sender, EventArgs e)
        {
            if (this.GetSelectedCamera() == null)
            {
                MessageBox.Show("Hãy chọn bản ghi muốn xóa");//"Làm ơn chọn bản ghi cần xóa");
                return;
            }

            if (MessageBox.Show("Bạn có muốn xóa bản ghi này?",
                "Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (configs.ExecuteCommand_AccessConnection($"delete from tblCamera where IP = '{SelectedCamera.CameraIP}'"))
                {
                    dgvCameraShow.DataSource = null;
                    Camera camera = new Camera();
                    camera = configs.cameras.GetCameraByIP(SelectedCamera.CameraIP);
                    configs.cameras.Remove(camera);
                    dgvCameraShow.DataSource = configs.cameras;
                }
            }
        }

        // Edit
        private void tbsEdit_Click(object sender, EventArgs e)
        {
            if (dgvCameraShow.Rows.Count > 0)
            {
                if (this.GetSelectedCamera() != null)
                {
                    frmCamera frm = new frmCamera(this.GetSelectedCamera());
                    if (frm.ShowDialog() == DialogResult.OK)
                    {         
                        UpdateGridCamera();
                    }                       
                }
                else
                {
                    MessageBox.Show("Bạn phải chọn dữ liệu để sửa");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Chưa có dữ liệu, hãy thêm dữ liệu");
                return;
            }
        }
        public void UpdateGridCamera()
        {
            // Edit
            if (SelectedCamera != null)
            {
                foreach(DataGridViewRow data in dgvCameraShow.Rows)
                {
                    if(SelectedCamera.CameraIP == data.Cells[3].Value.ToString())
                    {
                        data.Cells[2].Value = SelectedCamera.CamerName;
                        data.Cells[1].Value = SelectedCamera.CameraType;
                        data.Cells[3].Value = SelectedCamera.CameraIP;
                        data.Cells[4].Value = SelectedCamera.CameraPort.ToString();
                        data.Cells[5].Value = SelectedCamera.CameraUserame;
                        data.Cells[6].Value = SelectedCamera.cameraPassword; 
                    }
                }
            }
        }

        //// <summary>
        /// Get selected Cam
        /// </summary>
        /// <returns></returns>
        public CameraView GetSelectedCamera()
        {
            DataGridViewRow _drv = dgvCameraShow.CurrentRow;
            try
            {
                SelectedCamera = new CameraView();
                SelectedCamera.CamerName = _drv.Cells[2].Value.ToString();
                SelectedCamera.CameraType = _drv.Cells[1].Value.ToString();
                SelectedCamera.CameraIP = _drv.Cells[3].Value.ToString();
                SelectedCamera.CameraPort = Convert.ToInt32(_drv.Cells[4].Value.ToString());
                SelectedCamera.CameraUserame = _drv.Cells[5].Value.ToString();
                SelectedCamera.cameraPassword = _drv.Cells[6].Value.ToString();
            }
            catch
            {
                SelectedCamera = null;
            }
            return SelectedCamera;
        }
        public void GetGridView()
        {
            dgvCameraShow.DataSource = null;
            configs.LoadCameras();
            configs.LoadUsers();
            dgvCameraShow.DataSource = configs.cameras;
        }
        public void LoadComputer()
        {
            DataTable dtbComputer = configs.GetTable_AccessConnection("Select * from tblComputer");
            if (dtbComputer != null && dtbComputer.Rows.Count > 0)
            {
                DataRow row = dtbComputer.Rows[0];
                txtComputerIP.Text = row["IP"].ToString();
                txtComputerPort.Text = row["Port"].ToString();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            configs.ExecuteCommand_AccessConnection($"Update tblComputer set IP ='{txtComputerIP.Text}', Port ={Convert.ToInt32(txtComputerPort.Text)}");
            this.DialogResult = DialogResult.OK;
        }
    }
}

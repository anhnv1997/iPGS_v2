using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

using Kztek.Databases;
using Kztek.iZCU.Devices;
using Kztek.iZCU.Objects;

namespace Kztek.iZCU
{
    public class Configuration
    {
        #region Variable
        public IConnection accessConnection = new AccessConnection();
        
        //Camera
        public CameraCollection cameras = new CameraCollection();
        public UserCollecttion users = new UserCollecttion();
        #endregion

        public Configuration()
        {
            #region Database
            accessConnection = new AccessConnection();
            accessConnection.ServerName = "Microsoft.Jet.OLEDB.4.0";
            accessConnection.DatabaseName = "configs\\iZCU.mdb";
            accessConnection.Authentication = 1;
            accessConnection.UserName = "";
            accessConnection.Password = "nxt";
            accessConnection.EnableMARS = true;
            accessConnection.DatabaseConnectionErrorEvent += new DatabaseConnectionErrorEventHandler(databaseConnection_DatabaseConnectionErrorEvent);
            accessConnection.Open();
            #endregion
        }


        #region Access Connection

        public void databaseConnection_DatabaseConnectionErrorEvent(string errorEvent)
        {
            Logger_Error("Access DB: " + errorEvent);
        }

        public void OpenAccessDatabaseConnection()
        {
            if (accessConnection != null)
                accessConnection.Open();
        }

        public DataTable GetTable_AccessConnection(string command)
        {
            if (accessConnection != null)
                return accessConnection.GetTable(command);
            else
            {
                Logger_Error("Access Connection NULL");
                return null;
            }
        }

        public bool ExecuteCommand_AccessConnection(string commandString)
        {
            if (accessConnection != null)
            {
                bool ret = false;

                if (accessConnection.ExecuteCommand(commandString))
                {
                    ret = true;
                }

                return ret;
            }
            else
            {
                Logger_Error("Access Connection NULL");
                return false;
            }
        }

        public bool IsAccessDatabaseConnected()
        {
            bool ret = true;

            if (accessConnection != null)
            {
                if (accessConnection.State == ConnectionState.Closed || accessConnection.State == ConnectionState.Broken)
                    ret = false;
            }
            else
                ret = false;

            return ret;
        }


        public void CloseAccessConnection()
        {
            if (accessConnection != null)
                accessConnection.Close();
        }

        #endregion


        #region User
        public void LoadUsers()
        {
            users.Clear();
            try
            {
                DataTable dt = GetTable_AccessConnection("select * from tblUser order by ID");

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dtr in dt.Rows)
                    {
                        User user = new User();
                        user.UerId = Convert.ToInt32(dtr["ID"].ToString());
                        user.UserFullname = dtr["FullName"].ToString();
                        user.UserUsername = dtr["Username"].ToString();
                        user.UserPassword = dtr["Password"].ToString();
                        user.UserDescription = dtr["Description"].ToString();
                        users.Add(user);
                    }
                    dt.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger_Error("LoadCamera: " + ex.ToString());
            }
        }

        #endregion


        #region Cameras
        public void LoadCameras()
        {
            cameras.Clear();
            try
            {
                DataTable dt = GetTable_AccessConnection("select * from tblCamera order by ID");

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dtr in dt.Rows)
                    {
                        Camera camera = new Camera();
                        camera.ID = dtr["ID"].ToString();
                        camera.Type = CameraTypeCollection.GetType(dtr["CameraType"].ToString());
                        camera.CameraName = dtr["CameraName"].ToString();
                        camera.IP = dtr["IP"].ToString();
                        int temp = 80;
                        int.TryParse(dtr["Port"].ToString(), out temp); if (temp <= 0) temp = 80;
                        camera.Port = temp;
                        camera.Login = dtr["Login"].ToString();
                        camera.Password = dtr["Password"].ToString();
                        // Add to Pool
                        cameras.Add(camera);
                    }
                    dt.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger_Error("LoadCamera: " + ex.ToString());
            }
        }

        public void LoadCameraLastEvent(Camera camera)
        {
            try
            {
                Ping pingSender = new Ping();
                PingReply reply = null;
                reply = pingSender.Send(camera.IP, 500);
                if (reply != null && reply.Status == IPStatus.Success)
                {
                    DataTable dt = GetTable_AccessConnection($"select * from tblCameraEvent WHERE IP = '{camera.IP}' order by ID");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int count = dt.Rows.Count;
                        DataRow dtr = dt.Rows[count - 1];
                        //set last data ********************************                    
                        int temp_TotalSlot = 3;
                        int.TryParse(dtr["TotalSlot"].ToString(), out temp_TotalSlot);
                        if (temp_TotalSlot < 0) temp_TotalSlot = 3;
                        camera.lastTotalSlot = temp_TotalSlot;

                        int temp_EmptySlot = 3;
                        int.TryParse(dtr["EmptySlot"].ToString(), out temp_EmptySlot);
                        if (temp_EmptySlot < 0) temp_EmptySlot = 3;
                        camera.lastEmptySlot = temp_EmptySlot;

                        camera.lastParkingNo = dtr["ParkingNo"].ToString();
                        camera.lastCarStatus = dtr["CarStatus"].ToString();
                        camera.lastPlateNum = dtr["PlateNumber"].ToString();
                        camera.lastEventDateTime = dtr["EventDateTime"].ToString();
                        dt.Dispose();
                    }
                }
                else
                {
                    camera.lastTotalSlot = -1;
                    camera.lastEmptySlot = -1;

                    camera.lastParkingNo = null;
                    camera.lastCarStatus = null;
                    camera.lastPlateNum = null;
                }
                   
            }
            catch (Exception ex)
            {
                Logger_Error("LoadCamera: " + ex.ToString());
            }
        }
        public bool Insert_CameraEvent(string IP, DateTime EventDateTime, int TotalSlot, int EmptySlot, string ParkingNo, string CarStatus, string PlateNumber)
        {
            var nullValue = DBNull.Value;
            string cmd = $"INSERT INTO tblCameraEvent(IP,EventDateTime,TotalSlot,EmptySlot,ParkingNo,CarStatus,PlateNumber) values('{IP}','{EventDateTime}',{TotalSlot},{EmptySlot},'{ParkingNo}','{CarStatus}','{PlateNumber}')";
            return ExecuteCommand_AccessConnection(cmd);
        }

        public bool Update_Camera_Image(string IP ,string ImagePath)
        {
            string cmd = $"UPDATE tblCamera SET  [ImagePath] = '{ImagePath}' WHERE IP='{IP}'";
            if (!ExecuteCommand_AccessConnection(cmd))
            {
                if (!ExecuteCommand_AccessConnection(cmd))
                {
                    return false;
                }
            }
            return true;
        }
        public bool Update_Camera(Camera camera, CameraType _cameraType, string _cameraName, string _ip, int _port, string _login, string _password)
        {
            bool ret = false;
            return ret;
        }
        public bool Delete_Camera(Camera camera)
        {
            bool ret = false;
            return ret;
        }
        public void DisplayCamera(DataGridView dataGridView, string condition)
        {
            dataGridView.Rows.Clear();
            try
            {

            }
            catch (Exception ex)
            {
                Logger_Error("Display camera: " + ex.ToString());
            }
        }


        #endregion

        #region LogEvent
        public void Logger_Error(string s)
        {
            try
            {
                string pathFile = Path.GetDirectoryName(Application.ExecutablePath) + @"\logs\ERROR_LOG_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
                using (StreamWriter writer = new StreamWriter(pathFile, true))
                {
                    try
                    {
                        writer.WriteLine("ERROR " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - " + s);
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        public void Logger_Info(string s)
        {
            try
            {
                string pathFile = Path.GetDirectoryName(Application.ExecutablePath) + @"\logs\INFO_LOG_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
                using (StreamWriter writer = new StreamWriter(pathFile, true))
                {
                    try
                    {
                        writer.WriteLine("INFO " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - " + s);
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        public void Logger_Warn(string s)
        {
            try
            {
                string pathFile = Path.GetDirectoryName(Application.ExecutablePath) + @"\logs\WARN_LOG_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
                using (StreamWriter writer = new StreamWriter(pathFile, true))
                {
                    try
                    {
                        writer.WriteLine("WARN " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " - " + s);
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        #endregion

        public static void GetCurrentData(StateEventArgs e, Camera cam)
        {
            cam.IP = e._CameraIP;
            cam.currentEventStatus = e._EventStatus;
            cam.currentTotalSlot = e._TotalSlot;
            cam.currentEmptySlot = e._EmptySlot;
            cam.currentParkingNo = e._ParkingNo;
            cam.currentCarStatus = e._CarStatus;
            cam.currentPlateNum = e._PlateNum;
            cam.currentEventDateTime = e._EventDateTime;
        }
        
    }
}

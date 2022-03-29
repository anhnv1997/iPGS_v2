using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kztek.iZCU.Devices
{
    public class DahuaCameraController: ICameraController
    {
        public event StateEventHandler StateEvent;
        public event ErrorsEventHandler ErrorEvent;
        string _IP = "";
        int _Port = 80;
        string _Login = "";
        string _Password = "";
        bool _IsConnected = false;

        private Thread thread;
        private ManualResetEvent stopEvent;

        public string IP
        {
            set { _IP = ""; }
            get { return _IP; }
        }
        public int Port
        {
            set { _Port = value; }
            get { return _Port; }
        }
        public string Login
        {
            set { _Login = ""; }
            get { return _Login; }
        }
        public string Password
        {
            set { _Password = ""; }
            get { return _Password; }
        }
        public bool IsConnected
        {
            get { return _IsConnected; }
        }
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
        public bool Connect(string ip)
        {
            this._IP = ip;
            Ping pingSender = new Ping();
            PingReply reply = null;
            reply = pingSender.Send(ip, 500);
            if (reply != null && reply.Status == IPStatus.Success)
            {
                return true;
            }
            else
                return false;

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

        bool isBusy = false;
        private void WorkerThread()
        {
            while (!this.stopEvent.WaitOne(0, true))
            {
                try
                {
                    if (!isBusy)
                    {
                        isBusy = true;
                        Ping pingSender = new Ping();
                        PingReply reply = null;
                        reply = pingSender.Send(this._IP, 500);
                        if (reply != null && reply.Status == IPStatus.Success)
                        {
                            _IsConnected = true;


                        }
                        else
                            _IsConnected = false;
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
                    isBusy = true;
                }
            }
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

        public bool ControlLight(int color)
        {
            return false;
        }
    }
}

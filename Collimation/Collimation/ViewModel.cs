using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Collimation
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _aEnabled = false;
        private bool _bEnabled = false;
        private bool _cEnabled = false;

        private bool _isConnected = false;
        private string _connectStatus = "Connect";
        private string _runningStatus = "Idle";

        private SerialPort _serialPort;
        private delegate void SetTextDeleg(string text);

        public ViewModel()
        {

        }

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool AEnabled
        {
            get { return _aEnabled; }
            set
            {
                if (_aEnabled != value)
                {
                    _aEnabled = value;
                    OnPropertyChange("AEnabled");
                }
            }
        }

        public bool BEnabled
        {
            get { return _bEnabled; }
            set
            {
                if (_bEnabled != value)
                {
                    _bEnabled = value;
                    OnPropertyChange("BEnabled");
                }
            }
        }

        public bool CEnabled
        {
            get { return _cEnabled; }
            set
            {
                if (_cEnabled != value)
                {
                    _cEnabled = value;
                    OnPropertyChange("CEnabled");
                }
            }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    OnPropertyChange("IsConnected");
                }
            }
        }

        public string ConnectStatus
        {
            get { return _connectStatus; }
            set
            {
                if (_connectStatus != value)
                {
                    _connectStatus = value;
                    OnPropertyChange("ConnectStatus");
                }
            }
        }

        public string RunningStatus
        {
            get { return _runningStatus; }
            set
            {
                if (_runningStatus != value)
                {
                    _runningStatus = value;
                    OnPropertyChange("RunningStatus");
                }
            }
        }

        public void Connect(string comPort)
        {
            _serialPort = new SerialPort(comPort, 19200, Parity.None, 8, StopBits.One);
            _serialPort.Handshake = Handshake.None;

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            _serialPort.WriteTimeout = 500;
            _serialPort.Open();
            if(_serialPort.IsOpen)
            {
                IsConnected = true;
                ConnectStatus = "Disconnect";
                Debug.WriteLine("Connected...");
            }
            
        }

        public void Disconnect()
        {
            if (!IsConnected)
                return;

            _serialPort.Close();
            IsConnected = false;
            ConnectStatus = "Connect";
        }

        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(500);
            string data = _serialPort.ReadLine();
            // Invokes the delegate on the UI thread, and sends the data that was received to the invoked method.  
            // ---- The "si_DataReceived" method will be executed on the UI thread which allows populating of the textbox.  
            // this.BeginInvoke(new SetTextDeleg(si_DataReceived), new object[] { data });
            if(data.Contains("running"))
            {
                RunningStatus = "Running";
            }
            else if(data.Contains("idle"))
            {
                RunningStatus = "Idle";
            }
            else if (data.Contains("stopping"))
            {
                RunningStatus = "Stopping";
            }
            Debug.WriteLine(data);
        }

        public void DispatchCommand(string dir, int state)
        {
            if (!_isConnected)
                return;

            if (_aEnabled)
            {
                RunningStatus = "Running";
                string cmd = "A ";

                if (dir == "C" && state == 0)
                {
                    cmd += "C 0";
                }
                else if(dir == "CC" && state == 0)
                {
                    cmd += "CC 0";
                }

                else if (dir == "C" && state == 1)
                {
                    cmd += "C 1";
                }
                else if (dir == "CC" && state == 1)
                {
                    cmd += "CC 1";
                }
                Debug.WriteLine(cmd);
                _serialPort.WriteLine(cmd);

            }
            else if(_bEnabled)
            {
                RunningStatus = "Running";
                string cmd = "B ";

                if (dir == "C" && state == 0)
                {
                    cmd += "C 0";
                }
                else if (dir == "CC" && state == 0)
                {
                    cmd += "CC 0";
                }

                else if (dir == "C" && state == 1)
                {
                    cmd += "C 1";
                }
                else if (dir == "CC" && state == 1)
                {
                    cmd += "CC 1";
                }
                Debug.WriteLine(cmd);
                _serialPort.WriteLine(cmd);
            }
            else if(_cEnabled)
            {
                RunningStatus = "Running";
                string cmd = "C ";

                if (dir == "C" && state == 0)
                {
                    cmd += "C 0";
                }
                else if (dir == "CC" && state == 0)
                {
                    cmd += "CC 0";
                }

                else if (dir == "C" && state == 1)
                {
                    cmd += "C 1";
                }
                else if (dir == "CC" && state == 1)
                {
                    cmd += "CC 1";
                }
                Debug.WriteLine(cmd);
                _serialPort.WriteLine(cmd);
            }
           
        }
    }
}
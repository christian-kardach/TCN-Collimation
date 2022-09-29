using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Controls.Primitives;

namespace Collimation
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModel _viewModel;


        
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ViewModel();
            DataContext = _viewModel;

            ListPorts();

            //_serialPort = new SerialPort("COM1", 19200, Parity.None, 8, StopBits.One);
        }

        private void ListPorts()
        {
            foreach (string s in SerialPort.GetPortNames())
            {
                Debug.WriteLine("   {0}", s);
            }
        }

        private void HandleCheck(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = (sender as ToggleButton);

            string content = (sender as ToggleButton).Content.ToString();
            if (content == "A")
            {
                _viewModel.AEnabled = true;
                _viewModel.BEnabled = false;
                _viewModel.CEnabled = false;
                //btn.Background = btn.Background == Brushes.Red ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.Red;
            }
            else if (content == "B")
            {
                _viewModel.AEnabled = false;
                _viewModel.BEnabled = true;
                _viewModel.CEnabled = false;
            }
            else if (content == "C")
            {
                _viewModel.AEnabled = false;
                _viewModel.BEnabled = false;
                _viewModel.CEnabled = true;
            }
            Debug.WriteLine("ON!");
        }

        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
            string content = (sender as ToggleButton).Content.ToString();
            if (content == "A")
            {
                _viewModel.AEnabled = false;
            }
            else if (content == "B")
            {
                _viewModel.BEnabled = false;
            }
            else if (content == "C")
            {
                _viewModel.CEnabled = false;
            }
            Debug.WriteLine("OFF!");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            this.KeyUp += new KeyEventHandler(MainWindow_KeyUp);
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(_viewModel.AEnabled || _viewModel.BEnabled || _viewModel.CEnabled)
            {
                if (e.Key == Key.A && !e.IsRepeat)
                {
                    _viewModel.DispatchCommand("CC", 1);
                }
                else if (e.Key == Key.D && !e.IsRepeat)
                {
                    _viewModel.DispatchCommand("C", 1);
                }
            }
        }

        void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (_viewModel.AEnabled || _viewModel.BEnabled || _viewModel.CEnabled)
            {
                if (e.Key == Key.A)
                {
                    _viewModel.DispatchCommand("CC", 0);
                }
                else if (e.Key == Key.D)
                {
                    _viewModel.DispatchCommand("C", 0);
                }
            }
        }

        private void mConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            if(!_viewModel.IsConnected)
            {
                var comPort = mComPort.Text;
                _viewModel.Connect(comPort);
            }
            else
            {
                _viewModel.Disconnect();
            }
            
        }
    }
}

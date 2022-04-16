using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFTest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate void MyDelegate();
        bool ReceiveFormat = true;
        bool SendForamt = true;
        byte[] rxFrameData = new byte[1024];
        byte rByte, checkSum;
        static int rxFrameDataIndex = 0;
        static bool bRxHeaderFound = false;

        SerialPort _spCOM = new SerialPort();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(InitSerialPort);
        }

        void InitSerialPort(object sender, EventArgs e)
        {
            _spCOM.DataReceived += new SerialDataReceivedEventHandler(spCOM_DataReceived);

            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                cbPortName.Items.Add(port);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbox_reciv.Text = "";
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
                _spCOM.PortName = "COM4";
                _spCOM.BaudRate = Convert.ToInt32(921600);
                */
                _spCOM.Handshake = System.IO.Ports.Handshake.None;
                _spCOM.Parity = Parity.None;
                _spCOM.DataBits = 8;
                _spCOM.StopBits = StopBits.One;
                _spCOM.ReadTimeout = 200;
                _spCOM.WriteTimeout = 50;

                _spCOM.Open();
                _spCOM.DataReceived += new SerialDataReceivedEventHandler(spCOM_DataReceived);
                //_spCOM.RtsEnable = true;+
                btnClose.IsEnabled = true;
                btnOpen.IsEnabled = false;
            }

            catch
            {
                MessageBox.Show("해당포트의 연결을 다시 확인하십시요");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            _spCOM.DataReceived -= new SerialDataReceivedEventHandler(spCOM_DataReceived);
            _spCOM.Close();
            btnClose.IsEnabled = false;
            btnOpen.IsEnabled = true;
        }

        void spCOM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] array = new byte[1024];
            int rxCnt;
            string str = string.Empty;
            rxCnt = _spCOM.Read(array, 0, 1024);

            /*
            for ( int i=0; i<rxCnt; i++)
            {
                rByte = array[i];
                if ( rByte==0x02 && !bRxHeaderFound)
                {
                    bRxHeaderFound = true;
                    rxFrameDataIndex = 0;
                    //rxHeadFindingIndex = 1;
                }

                if (bRxHeaderFound)
                {
                    if ( rxFrameDataIndex>=1023)
                    {
                        rxHeadFindingIndex = 0;
                        bRxHeaderFound = false;
                        rxFrameDataIndex = 0;
                        continue;
                    }
                    else
                        rxFrameData[rxFrameDataIndex] = rByte;
                }

                if (bRxHeaderFound && rxFrameDataIndex == 12 && rByte == 0x03)
                {
                    checkSum = 0;
                    for (int k = 1; k < 11; k++)
                        checkSum += rxFrameData[k];
                    if (checkSum == rxFrameData[11])
                    {
                        for (int k = 0; k < rxCnt; k++)
                        {
                            str += string.Format("{0:x2} ", rxFrameData[k]);
                        }

                        MyDelegate dt = delegate ()
                        {
                            tbox_reciv.Text = "";
                            tbox_reciv.Text += "Receive Data : ";
                            tbox_reciv.Text += str;
                            tbox_reciv.Text += "\r\n";
                        };
                        this.Dispatcher.Invoke(dt);
                    }
                    else
                    {
                        MyDelegate dt = delegate ()
                        {
                            tbox_reciv.Text = "";
                            tbox_reciv.Text += "Receive Data : CRC Error";
                            tbox_reciv.Text += "\r\n";
                        };
                        this.Dispatcher.Invoke(dt);
                    }
                    rxHeadFindingIndex = 0;
                    bRxHeaderFound = false;
                    rxFrameDataIndex = 0;
                } else if ( bRxHeaderFound)
                    rxFrameDataIndex++;
            }
            */

            //아스키 값으로 받기
            if (ReceiveFormat)
            {
                MyDelegate dt = delegate ()
                {
                    tbox_reciv.Text += "Receive Data : ";
                    for (int i = 0; i < rxCnt; i++)
                    {
                        tbox_reciv.Text += (char)array[i];
                    }
                    tbox_reciv.Text += "\r\n";
                };
                this.Dispatcher.Invoke(dt);
            }
            //HEX 값으로 받기
            else
            {
                /*
                for (int i = 0; i < rxCnt; i++)
                {
                    str += string.Format("{0:x2} ", array[i]);
                }*/

                for (int i = 0; i < rxCnt; i++)
                {
                    rByte = array[i];
                    if (rByte == 0x02 && !bRxHeaderFound)
                    {
                        bRxHeaderFound = true;
                        rxFrameDataIndex = 0;
                    }

                    if (bRxHeaderFound)
                    {
                        if (rxFrameDataIndex >= 1023)
                        {
                            Debug.Assert(false, "rxFrameDataIndex is over 1023");
                            bRxHeaderFound = false;
                            rxFrameDataIndex = 0;
                            continue;
                        }
                        else
                            rxFrameData[rxFrameDataIndex] = rByte;
                    }

                    if (bRxHeaderFound && rxFrameDataIndex == 12 && rByte == 0x03)
                    {
                        checkSum = 0;
                        for (int k = 1; k < 11; k++)
                            checkSum += rxFrameData[k];
                        if (checkSum == rxFrameData[11])
                        {
                            str = "Rx  OK : ";
                            for (int k = 0; k < 13; k++)
                            {
                                str += string.Format("{0:X2} ", rxFrameData[k]);
                            }
                            int data = (int)((0xFF & rxFrameData[2]) << 16 | (0xFF & rxFrameData[3]) << 8 | (0xFF & rxFrameData[4]));

                            // << 24) | ((0xFF & rxFrameData[3]) << 8) | ((0xFF & rxFrameData[4]));
                            str += string.Format(" {0:d} ", data);
                            data = (int)((0xFF & rxFrameData[5]) << 16 | (0xFF & rxFrameData[6]) << 8 | (0xFF & rxFrameData[7]));
                            str += string.Format(" {0:d} ", data);
                            data = (int)((0xFF & rxFrameData[8]) << 16 | (0xFF & rxFrameData[9]) << 8 | (0xFF & rxFrameData[10]));
                            str += string.Format(" {0:d} ", data);
                        }
                        else
                        {
                            str = string.Format("CRC Error");
                        }
                        bRxHeaderFound = false;
                        rxFrameDataIndex = 0;
                    }
                    else
                        rxFrameDataIndex++;
                }



                //헥사로 바꿔서 출력
                MyDelegate dt = delegate ()
                {
                    // listBox.AddStr
                    tbox_reciv.Text = "";
                    //tbox_reciv.Text += "Receive Data : ";
                    tbox_reciv.Text += str;
                    tbox_reciv.Text += "\r\n";
                };
                this.Dispatcher.Invoke(dt);
            }
        }

        private void cbPortName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_spCOM.IsOpen)
            {
                _spCOM.Close();
            }

            _spCOM.PortName = cbPortName.SelectedItem.ToString();
        }

        void OpenComPort(object sender, RoutedEventArgs e)
        {
            try
            {
                _spCOM.Open();
            }
            catch (Exception ex)
            {
                cbPortName.SelectedItem = "";
            }
        }
        private void cbBaudrate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] names = cbBaudrate.SelectedItem.ToString().Split(':');
            _spCOM.BaudRate = int.Parse(names[1].ToString().Trim());

        }

        private void cbDataBits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] names = cbDataBits.SelectedItem.ToString().Split(':');
            _spCOM.DataBits = int.Parse(names[1].ToString().Trim());
        }

        private void cbStopBits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbStopBits.SelectedIndex)
            {
                case 0:
                    _spCOM.StopBits = StopBits.One;
                    break;
                case 1:
                    _spCOM.StopBits = StopBits.Two;
                    break;
                default:
                    _spCOM.StopBits = StopBits.One;
                    break;
            }
        }

        private void cbParity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbStopBits.SelectedIndex)
            {
                case 0:
                    _spCOM.Parity = Parity.None;
                    break;
                case 1:
                    _spCOM.Parity = Parity.Odd;
                    break;
                case 2:
                    _spCOM.Parity = Parity.Even;
                    break;
                default:
                    _spCOM.Parity = Parity.None;
                    break;
            }
        }

        private void cbReceiveFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbReceiveFormat.Text == "ASCII")
            {
                ReceiveFormat = true;
            }
            else
            {
                ReceiveFormat = false;
            }
        }

        private void cbSendFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSendFormat.Text == "ASCII")
            {
                SendForamt = true;
            }
            else
            {
                SendForamt = false;
            }
        }

        private void btn_send_Click(object sender, RoutedEventArgs e)
        {
            if (_spCOM.IsOpen)
            {
                //아스키 형태로 보내라
                if (SendForamt)
                {
                    _spCOM.Write(tbox_send.Text);
                }

                //HEX 형태로 보내라
                else
                {
                    string str = string.Empty;
                    byte[] arr = Encoding.ASCII.GetBytes(tbox_send.Text.ToCharArray());
                    foreach (byte b in arr)
                    {
                        str += string.Format("{0:x2} ", b);
                    }
                    _spCOM.Write(str);
                }
            }
            else
            {
                MessageBox.Show("포트가 연결되지 않았습니다.");
            }
            tbox_send.Text = "";
        }
    }
}

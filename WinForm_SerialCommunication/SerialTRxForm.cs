using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace SerialCommunication
{
    public partial class SerialTRxForm : Form
    {
        public SerialTRxForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Bind Port lists to ComboBox's Data source
            comboBox_port.DataSource = SerialPort.GetPortNames();
        }

        private void Button_connect_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                serialPort1.PortName = comboBox_port.Text;
                serialPort1.BaudRate = 9600;
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Parity = Parity.None;

                serialPort1.RtsEnable = true;
                serialPort1.DtrEnable = true;
                serialPort1.ReceivedBytesThreshold = 5;

                serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);

                serialPort1.Open();

                label_status.Text = "Port is Open";
                comboBox_port.Enabled = false;
            }
            else
            {
                label_status.Text = "Port is already Open";
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(MySerialReceived));
        }

        private void MySerialReceived(object s, EventArgs e)
        {
            string ReceiveData = serialPort1.ReadExisting();
            richTextBox_received.Text += ReceiveData;
        }

        private void Button_send_Click(object sender, EventArgs e)
        {
            serialPort1.Write(textBox_send.Text);
        }

        private void Button_disconnect_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();

                label_status.Text = "Port is Closed";
                comboBox_port.Enabled = true;
            }
            else
            {
                label_status.Text = "Port is already Closed";
            }
        }
    }
}

```cs
serial.RtsEnable = true;
serial.DtrEnable = true;
serial.ReceivedBytesThreshold = 5;
```

```cs
serial.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

public static void port_DataReceived(object sender, SerialDataReceivedEventArgs args)
{
  string text = serial.ReadExisting();
  Console.WriteLine("Rx: " + text);
  
  MessageBox.Show(text);
}

```

[Serial Port Class](https://learn.microsoft.com/en-us/dotnet/api/system.io.ports.serialport?view=dotnet-plat-ext-6.0)


```cs
namespace Serial_Communication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                
                serialPort1.Open();

                label_status.Text = "포트가 열렸습니다.";
                comboBox_port.Enabled = false;
            }
            else
            {
                label_status.Text = "포트가 이미 열려 있습니다.";
            }
        }
                
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {            
            this.Invoke(new EventHandler(MySerialReceived));
        }

        private void MySerialReceived(object s, EventArgs e)
        {            
            int ReceiveData = serialPort1.ReadByte();
            richTextBox_received.Text = richTextBox_received.Text + string.Format("{0:X2}", ReceiveData);
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

                label_status.Text = "포트가 닫혔습니다.";
                comboBox_port.Enabled = true;
            }
            else
            {
                label_status.Text = "포트가 이미 닫혀 있습니다.";
            }
        }
    }
}
```

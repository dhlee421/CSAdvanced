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
  Console.WriteLine("Rx: " + text;
  
  MessageBox.Show(text);
}

```

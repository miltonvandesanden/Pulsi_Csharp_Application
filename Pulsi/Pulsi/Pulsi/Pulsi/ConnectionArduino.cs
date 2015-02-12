using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows;
using System.Threading;

namespace Pulsi
{
    class ConnectionArduino
    {
        //Fields
        SerialPort serialPort;
        ConnectionDatabase connectionDatabase;

        string message = "";
        string command = "";
        string par = "";

        int baudRate = 9600;
        String[] ports;

        int meting = 0;

        int babyID;

        public System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public int Bpm { get; set; }

        public double Temperatuur { get; set; }

        //constructor
        public ConnectionArduino(int babyID)
        {
            this.babyID = babyID;

            serialPort = new SerialPort();
            connectionDatabase = new ConnectionDatabase();

            serialPort.BaudRate = baudRate;

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            dispatcherTimer.IsEnabled = true;
        }
        //Methods
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (serialPort.IsOpen
                && serialPort.BytesToRead > 0)
            {
                try
                {
                    message += serialPort.ReadExisting();

                    if (meting == 1)
                    {
                        if (message.IndexOf("%") != -1)
                        {
                            dispatcherTimer.IsEnabled = false;
                            Bpm = GetBpm();
                            meting = 0;

                        }
                    }
                    if (meting == 2)
                    {
                        if (message.IndexOf("%") != -1)
                        {
                            dispatcherTimer.IsEnabled = false;
                            Temperatuur = GetTemperature();
                            meting = 0;

                        }
                    }

                }
                catch (Exception exception)
                {
                    MessageBox.Show("er is iets misgegaan");
                }
            }
        }
        public String[] SearchPorts()
        {
            String[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);

            return ports;
        }
        public int Connect(string selectedPort)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();

                return 2;
            }
            else if (!serialPort.IsOpen)
            {
                string portname = selectedPort;
                try
                {
                    serialPort.PortName = portname;
                    serialPort.Open();

                    if (serialPort.IsOpen)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch
                {
                    return 0;
                }
            }
            return 0;
        }

        public void SendMessage(string message)
        {
            serialPort.Write(message);
        }

        public void SendBpm()
        {
            SendMessage("#SHOW:BPM%");
            meting = 1;
            dispatcherTimer.IsEnabled = true;

        }

        public int GetBpm()
        {
            MessageBox.Show("BPM ontvangen");
            int commandStart = message.IndexOf("#");
            int commandEnd = message.IndexOf(":");
            int parEnd = message.IndexOf("%");

            if (commandStart != -1 && commandEnd != -1 && parEnd != -1)
            {
                int commandLength = commandEnd - commandStart + 1;
                command = message.Substring(commandStart, commandLength);

                int parLength = parEnd - commandEnd + 1;
                par = message.Substring(commandEnd + 1, parLength - 2);

                if (command == "#BPM:")
                {
                    int bpm = Convert.ToInt32(par);

                    message = "";
                    command = "";
                    par = "";

                    MessageBox.Show(Convert.ToString(bpm));

                    connectionDatabase.SendBpmDatabase(babyID, bpm);

                    return bpm;

                }
                message = "";
                command = "";
                par = "";

                return -1;
            }
            message = "";
            command = "";
            par = "";

            return -1;
        }


        public void SendTemperatuur()
        {
            SendMessage("#SHOW:temperature%");
            meting = 2;
            dispatcherTimer.IsEnabled = true;

        }
        public double GetTemperature()
        {
            MessageBox.Show("Temperatuur ontvangen");
            int commandStart = message.IndexOf("#");
            int commandEnd = message.IndexOf(":");
            int parEnd = message.IndexOf("%");

            if (commandStart != -1 && commandEnd != -1 && parEnd != -1)
            {
                int commandLength = commandEnd - commandStart + 1;
                command = message.Substring(commandStart, commandLength);

                int parLength = parEnd - commandEnd + 1;
                par = message.Substring(commandEnd + 1, parLength - 2);

                if (command == "#TEMPERATURE:")
                {
                    double temperature = Convert.ToDouble(par);

                    message = "";
                    command = "";
                    par = "";

                    MessageBox.Show(Convert.ToString(temperature));

                    connectionDatabase.SendTemperatureDatabase(babyID, temperature);

                    return temperature;
                }
                message = "";
                command = "";
                par = "";

                return -1;
            }
            message = "";
            command = "";
            par = "";

            return -1;
        }

        public void CloseConnection()
        {
            dispatcherTimer.IsEnabled = false;
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }
    }
}

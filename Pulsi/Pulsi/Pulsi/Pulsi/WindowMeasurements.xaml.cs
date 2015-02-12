using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pulsi
{
    /// <summary>
    /// Interaction logic for windowMeasurements.xaml
    /// </summary>
    public partial class WindowMeasurements : Window
    {
        //fields
        ConnectionArduino connectionArduino;
        ConnectionDatabase connectionDatabase;

        //the data that is measured with the Pulsi Cobear
        int bpm =  69;
        double temperature = 666.66;

        //de bij de ingelogde ouder horende baby
        int babyID;

        //constructor
        public WindowMeasurements()
        {
            InitializeComponent();

            connectionDatabase = new ConnectionDatabase();

            if (connectionDatabase.CurrentUsername == "Boersma")
            {
                babyID = 1;
            }
            else if (connectionDatabase.CurrentUsername == "Koenders")
            {
                babyID = 2;
            }
            else if (connectionDatabase.CurrentUsername == "Barends")
            {
                babyID = 3;
            }
            else if (connectionDatabase.CurrentUsername == "de Boer")
            {
                babyID = 4;
            }

            connectionArduino = new ConnectionArduino(babyID);
        }

        //methods
        private void btnSearchPorts_Click(object sender, RoutedEventArgs e)
        {
            String[] ports = connectionArduino.SearchPorts();

            comboxPorts.Items.Clear();
            foreach (String port in ports)
            {
                comboxPorts.Items.Add(port);
            }

            if (comboxPorts.Items.Count > 0)
            {
                comboxPorts.IsEnabled = true;
            }
        }

        private void comboxPorts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboxPorts.SelectedIndex != -1)
            {
                btnConnect.IsEnabled = true;
            } else
            {
                btnConnect.IsEnabled = false;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            int connectionSuccesType;

            connectionSuccesType = connectionArduino.Connect(Convert.ToString(comboxPorts.SelectedItem));

            if (connectionSuccesType == 0)
            {
                MessageBox.Show("Er is geen verbinding");
            }
            else if (connectionSuccesType == 1)
            {
                btnStartMeasurement.IsEnabled = true;

                btnConnect.Content = "verbinding verbreken";
                btnStartMeasurement.IsEnabled = true;
                comboxTemperatureOrBpm.IsEnabled = true;
                comboxPorts.IsEnabled = false;
                btnSearchPorts.IsEnabled = false;
            }
            else if (connectionSuccesType == 2)
            {
                MessageBox.Show("De verbinding met de Arduino is verbroken");
                
                btnConnect.Content = "verbinden";
                btnConnect.IsEnabled = false;
                comboxPorts.Items.Clear();
                comboxPorts.IsEnabled = false;
                btnSearchPorts.IsEnabled = true;
            }
        }

        private void btnStartMeasurement_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (comboxTemperatureOrBpm.SelectedIndex == -1)
            {
                MessageBox.Show("U moet selecteren wat u wilt meten!");
            }
            else if (comboxTemperatureOrBpm.SelectedIndex == 0)
            {
                connectionArduino.SendBpm();
                bpm = connectionArduino.Bpm;
            }
            else if (comboxTemperatureOrBpm.SelectedIndex == 1)
            {
                connectionArduino.SendTemperatuur();
                temperature = connectionArduino.Temperatuur;
            }
             */

            if (comboxTemperatureOrBpm.SelectedIndex == 0)
            {
                connectionDatabase.SendBpmDatabase(babyID, bpm);
            }
            else if (comboxTemperatureOrBpm.SelectedIndex == 1)
            {
                connectionDatabase.SendTemperatureDatabase(babyID, temperature);
            }
            else
            {
                MessageBox.Show("U moet selecteren wat u wilt meten!");
            }
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            connectionDatabase = null;

            bpm = 0;
            temperature = 0;

            WindowLogIn windowLogIn = new WindowLogIn();
            windowLogIn.Show();
            this.Close();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            connectionArduino.CloseConnection();
            connectionArduino = null;
        }
    }
}

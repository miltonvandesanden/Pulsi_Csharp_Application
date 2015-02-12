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
using System.Data;

namespace Pulsi
{
    /// <summary>
    /// Interaction logic for WindowData.xaml
    /// </summary>
    public partial class WindowData : Window
    {
        //fields
        ConnectionDatabase connectionDatabase;
        
        List<string> babyName;
        List<int> babyID;
        List<double> temperature;

        List<int> savedTemperatureDataID;
        List<double> savedTemperature;
        List<int> savedBpm;
        List<string> savedTemperatureDatumTijd;
        List<int> savedBpmDataID;
        List<string> savedBpmDatumTijd;

        //constructor
        public WindowData(string CurrentUsername)
        {
            InitializeComponent();

            babyName  = new List<string>();
            babyID = new List<int>();
            temperature = new List<double>();

            connectionDatabase = new ConnectionDatabase();

            SetComboxBaby(CurrentUsername);
        }

        private void SetComboxBaby(string CurrentUsername)
        {
            if (CurrentUsername == "De Wit")
            {
                babyID.Add(1);
                babyName.Add("Boersma, T.");
            }
            else if (CurrentUsername == "De Jong")
            {
                babyID.Add(2);
                babyID.Add(4);

                babyName.Add("Koenders, S.");
                babyName.Add("De Boer, G.");
            }
            else if (CurrentUsername == "Paulisma")
            {
                babyID.Add(3);

                babyName.Add("Barends, F.");
            }

            foreach (string name in babyName)
            {
                comboxBaby.Items.Add(name);
            }
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            connectionDatabase = null;

            WindowLogIn windowLogIn = new WindowLogIn();
            windowLogIn.Show();
            this.Close();
        }

        private void comboxBaby_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string babyName = Convert.ToString(comboxBaby.SelectedItem);

            SetDataGrid(babyName);
        }

        private void SetDataGrid(string babyName)
        {
            savedTemperatureDataID = connectionDatabase.GetSavedTemperatureDataID(babyName);
            savedBpmDataID = connectionDatabase.GetSavedBpmDataID(babyName);
            savedTemperature = connectionDatabase.GetSavedTemperature(babyName);
            savedBpm = connectionDatabase.GetSavedBpm(babyName);
            savedTemperatureDatumTijd = connectionDatabase.GetSavedTemperatureDateTime(babyName);
            savedBpmDatumTijd = connectionDatabase.GetSavedBpmDateTime(babyName);

            // Create new DataTable and DataSource objects.
            DataTable table = new DataTable();

            // Declare DataColumn and DataRow variables.
            DataColumn dataID;
            DataColumn temperature;
            DataColumn bpm;
            DataColumn dateTime;

            DataRow row;
            DataView view;

            // Create DataID column
            dataID = new DataColumn();
            dataID.DataType = System.Type.GetType("System.Int32");
            dataID.ColumnName = "DataID";
            table.Columns.Add(dataID);

            // Create Temperature column.
            temperature = new DataColumn();
            temperature.DataType = Type.GetType("System.Double");
            temperature.ColumnName = "Temperatuur";
            table.Columns.Add(temperature);

            // Create BPM column
            bpm = new DataColumn();
            bpm.DataType = Type.GetType("System.Int32");
            bpm.ColumnName = "BPM";
            table.Columns.Add(bpm);

            // Create DateTime column
            dateTime = new DataColumn();
            dateTime.DataType = Type.GetType("System.String");
            dateTime.ColumnName = "DatumTijd";
            table.Columns.Add(dateTime);

            int i = 0;
            foreach (double temp in savedTemperature)
            {
                row = table.NewRow();
                row["DataID"] = savedTemperatureDataID[i];
                row["Temperatuur"] = savedTemperature[i];
                row["DatumTijd"] = savedTemperatureDatumTijd[i];
                i++;
                table.Rows.Add(row);
                //datagridBaby.Items.Add(row);
            }

            int j = 0;
            foreach (int beatsPerMinute in savedBpm)
            {
                row = table.NewRow();
                row["DataID"] = savedBpmDataID[j];
                row["BPM"] = savedBpm[j];
                row["DatumTijd"] = savedBpmDatumTijd[j];
                j++;
                //datagridBaby.Items.Add(row);
                table.Rows.Add(row);
            }

            view = new DataView(table);
            datagridBaby.ItemsSource = view;
        }
    }
}

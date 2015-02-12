using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows;

namespace Pulsi
{
    class ConnectionDatabase
    {
        private OleDbConnection oleDbConnection;
        private OleDbDataReader oleDbDataReader;


        private string query;
        private string currentUsername;
        private string currentPassword;
        private int currentUserType;

        //properties
        public string CurrentUsername
        {
            get { return currentUsername; }
        }
        public string CurrentPassword
        {
            get { return currentPassword; }
        }
        public int CurrentUsertype
        {
            get { return currentUserType; }
        }

        //constructor
        public ConnectionDatabase()
        {
            string provider;
            string bestand;
            string connectionString;

            //provider voor ACCES database
            provider = "Provider=Microsoft.ACE.OLEDB.12.0";
            //bestandsnaam voor de ACCES database
            bestand = "Pulsi.accdb";

            connectionString = provider + ";Data Source=" + bestand;

            oleDbConnection = new OleDbConnection(connectionString);
        }
        public ConnectionDatabase(string currentUsername)
        {
            string provider;
            string bestand;
            string connectionString;

            //provider voor ACCES database
            provider = "Provider=Microsoft.ACE.OLEDB.12.0";
            //bestandsnaam voor de ACCES database
            bestand = "Pulsi.accdb";

            connectionString = provider + ";Data Source=" + bestand;

            oleDbConnection = new OleDbConnection(connectionString);

            this.currentUsername = currentUsername;
        }
        //methods
        public bool VerifyAccount(string username, string password, int usertype)
        {
            try
            {
                string usernameCheck;
                string passwordCheck;

                OleDbCommand oleDbCommand;

                AccountList accountList;

                accountList = new AccountList();

                if (usertype == 0)
                {
                    query = "SELECT Gebruikersnaam, Wachtwoord FROM Ouder";
                }
                else if (usertype == 1)
                {
                    query = "SELECT Gebruikersnaam, Wachtwoord FROM Dokter";
                }
                else
                {
                    return false;
                }

                oleDbConnection.Open();

                oleDbCommand = new OleDbCommand(query, oleDbConnection);
                oleDbCommand.ExecuteNonQuery();

                oleDbDataReader = oleDbCommand.ExecuteReader();

                while (oleDbDataReader.Read())
                {
                    usernameCheck = Convert.ToString(oleDbDataReader["Gebruikersnaam"]);
                    passwordCheck = Convert.ToString(oleDbDataReader["Wachtwoord"]);

                    accountList.AddAccount(usernameCheck, passwordCheck);
                }

                foreach (Account account in accountList.AccountListProperty)
                {
                    if (username == account.Username && password == account.Password)
                    {
                        currentUsername = username;
                        currentPassword = password;
                        currentUserType = usertype;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                if (oleDbDataReader != null)
                {
                    if (!oleDbDataReader.IsClosed)
                    {
                        oleDbDataReader.Close();
                    }
                }

                if (oleDbConnection.State.Equals(System.Data.ConnectionState.Open))
                {
                    oleDbConnection.Close();
                }
            }
        }

        public void SendBpmDatabase(int babyId, int bpm)
        {
            try
            {
                oleDbConnection.Open();
                OleDbCommand oleDbCommand;
                DateTime dateTime = DateTime.Now;

                string query = "INSERT INTO Data (BabyID, BPM, DatumTijd) VALUES (" + babyId + ", " + bpm + ", '" + dateTime + "')";

                oleDbCommand = new OleDbCommand(query, oleDbConnection);

                oleDbCommand.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                if (oleDbConnection.State.Equals(System.Data.ConnectionState.Open))
                {
                    oleDbConnection.Close();
                }
            }
        }
        public void SendTemperatureDatabase(int babyID, double temperature)
        {
            try
            {
                oleDbConnection.Open();
                OleDbCommand oleDbCommand;
                DateTime dateTime = DateTime.Now;

                string query = "INSERT INTO Data (BabyID, Temperatuur, DatumTijd) VALUES(" + babyID + ", " + temperature + ", '" + dateTime + "')";

                oleDbCommand = new OleDbCommand(query, oleDbConnection);
                oleDbCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("ERROR");
            }
            finally
            {
                if (oleDbConnection.State.Equals(System.Data.ConnectionState.Open))
                {
                    oleDbConnection.Close();
                }
            }
        }
        public List<int> GetSavedTemperatureDataID(string babyName)
        {
            try
            {
                OleDbCommand oleDbCommand;
                string query;
                OleDbDataReader oleDbDataReader;
                int dataID;
                List<int> savedDataID = new List<int>();

                oleDbConnection.Open();

                query = "SELECT DataID FROM Data WHERE Temperatuur IS NOT NULL AND BabyID = (SELECT BabyID FROM Baby WHERE Naam = '" + babyName + "')";

                oleDbCommand = new OleDbCommand(query, oleDbConnection);
                oleDbCommand.ExecuteNonQuery();

                oleDbDataReader = oleDbCommand.ExecuteReader();
                while (oleDbDataReader.Read())
                {
                    dataID = Convert.ToInt32(oleDbDataReader["DataID"]);

                    savedDataID.Add(dataID);
                }
                return savedDataID;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (oleDbDataReader != null)
                {
                    if (!oleDbDataReader.IsClosed)
                    {
                        oleDbDataReader.Close();
                    }
                }

                if (oleDbConnection.State.Equals(System.Data.ConnectionState.Open))
                {
                    oleDbConnection.Close();
                }
            }
        }

        public List<int> GetSavedBpmDataID(string babyName)
        {
            try
            {
                OleDbCommand oleDbCommand;
                string query;
                OleDbDataReader oleDbDataReader;
                int dataID;
                List<int> savedDataID = new List<int>();

                oleDbConnection.Open();

                query = "SELECT DataID FROM Data WHERE BPM IS NOT NULL AND BabyID = (SELECT BabyID FROM Baby WHERE Naam = '" + babyName + "')";

                oleDbCommand = new OleDbCommand(query, oleDbConnection);
                oleDbCommand.ExecuteNonQuery();

                oleDbDataReader = oleDbCommand.ExecuteReader();
                while (oleDbDataReader.Read())
                {
                    dataID = Convert.ToInt32(oleDbDataReader["DataID"]);

                    savedDataID.Add(dataID);
                }
                return savedDataID;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (oleDbDataReader != null)
                {
                    if (!oleDbDataReader.IsClosed)
                    {
                        oleDbDataReader.Close();
                    }
                }

                if (oleDbConnection.State.Equals(System.Data.ConnectionState.Open))
                {
                    oleDbConnection.Close();
                }
            }
        }

        public List<double> GetSavedTemperature(string babyName)
        {
            try
            {
                OleDbCommand oleDbCommand;
                string query;
                double temperature;
                List<double> savedTemperature = new List<double>();

                query = "SELECT Temperatuur FROM Data WHERE BabyID = (SELECT BabyID FROM Baby WHERE Naam = '" + babyName + "')";

                oleDbConnection.Open();

                oleDbCommand = new OleDbCommand(query, oleDbConnection);
                oleDbCommand.ExecuteNonQuery();

                oleDbDataReader = oleDbCommand.ExecuteReader();

                while (oleDbDataReader.Read())
                {
                    temperature = Convert.ToDouble(oleDbDataReader["Temperatuur"]);

                    savedTemperature.Add(temperature);
                }

                return savedTemperature;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (oleDbDataReader != null)
                {
                    if (!oleDbDataReader.IsClosed)
                    {
                        oleDbDataReader.Close();
                    }
                }

                if (oleDbConnection.State.Equals(System.Data.ConnectionState.Open))
                {
                    oleDbConnection.Close();
                }
            }
        }
        public List<int> GetSavedBpm(string babyName)
        {
            try
            {
                OleDbCommand oleDbCommand;
                string query;
                OleDbDataReader oleDbDataReader;
                int bpm;
                List<int> savedBpm = new List<int>();

                oleDbConnection.Open();

                query = "SELECT BPM FROM Data WHERE BabyID = (SELECT BabyID FROM Baby WHERE Naam = '" + babyName + "')";

                oleDbCommand = new OleDbCommand(query, oleDbConnection);
                oleDbCommand.ExecuteNonQuery();

                oleDbDataReader = oleDbCommand.ExecuteReader();
                while (oleDbDataReader.Read())
                {
                    bpm = Convert.ToInt32(oleDbDataReader["BPM"]);

                    savedBpm.Add(bpm);
                }

                return savedBpm;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (oleDbDataReader != null)
                {
                    if (!oleDbDataReader.IsClosed)
                    {
                        oleDbDataReader.Close();
                    }
                }

                if (oleDbConnection.State.Equals(System.Data.ConnectionState.Open))
                {
                    oleDbConnection.Close();
                }
            }
        }
        public List<string> GetSavedTemperatureDateTime(string babyName)
        {
            try
            {
                OleDbCommand oleDbCommand;
                string query;
                OleDbDataReader oleDbDataReader;
                string dateTime;
                List<string> savedDateTime = new List<string>();

                oleDbConnection.Open();

                query = "SELECT DatumTijd FROM Data WHERE Temperatuur IS NOT NULL AND BabyID = (SELECT BabyID FROM Baby WHERE Naam = '" + babyName + "')";

                oleDbCommand = new OleDbCommand(query, oleDbConnection);
                oleDbCommand.ExecuteNonQuery();

                oleDbDataReader = oleDbCommand.ExecuteReader();
                while (oleDbDataReader.Read())
                {
                    dateTime = Convert.ToString(oleDbDataReader["DatumTijd"]);

                    savedDateTime.Add(dateTime);
                }

                return savedDateTime;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (oleDbDataReader != null)
                {
                    if (!oleDbDataReader.IsClosed)
                    {
                        oleDbDataReader.Close();
                    }
                }

                if (oleDbConnection.State.Equals(System.Data.ConnectionState.Open))
                {
                    oleDbConnection.Close();
                }
            }
        }
        public List<string> GetSavedBpmDateTime(string babyName)
        {
            try
            {
                OleDbCommand oleDbCommand;
                string query;
                OleDbDataReader oleDbDataReader;
                string dateTime;
                List<string> savedDateTime = new List<string>();

                oleDbConnection.Open();

                query = "SELECT DatumTijd FROM Data WHERE Temperatuur IS NOT NULL AND BabyID = (SELECT BabyID FROM Baby WHERE Naam = '" + babyName + "')";

                oleDbCommand = new OleDbCommand(query, oleDbConnection);
                oleDbCommand.ExecuteNonQuery();

                oleDbDataReader = oleDbCommand.ExecuteReader();
                while (oleDbDataReader.Read())
                {
                    dateTime = Convert.ToString(oleDbDataReader["DatumTijd"]);

                    savedDateTime.Add(dateTime);
                }

                return savedDateTime;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (oleDbDataReader != null)
                {
                    if (!oleDbDataReader.IsClosed)
                    {
                        oleDbDataReader.Close();
                    }
                }

                if (oleDbConnection.State.Equals(System.Data.ConnectionState.Open))
                {
                    oleDbConnection.Close();
                }
            }
        }
    }
}
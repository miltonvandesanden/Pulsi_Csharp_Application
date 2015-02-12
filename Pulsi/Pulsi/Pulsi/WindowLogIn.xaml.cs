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
    /// Interaction logic for windowLogIn.xaml
    /// </summary>
    public partial class WindowLogIn : Window
    {
        //fields
        private ConnectionDatabase connectionDatabase;
        private string currentUsername;

        //constructor
        public WindowLogIn()
        {
            InitializeComponent();

            connectionDatabase = new ConnectionDatabase();

        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            string username;
            string password;
            int usertype;

            username = tbUsername.Text;
            password = tbPassword.Text;
            usertype = comboxUserType.SelectedIndex;

            if (connectionDatabase.VerifyAccount(username, password, usertype))
            {
                if (usertype == 0)
                {
                    currentUsername = connectionDatabase.CurrentUsername;
                    WindowMeasurements windowMeasurements = new WindowMeasurements(currentUsername);
                    windowMeasurements.Show();
                    this.Close();
                } else if (usertype == 1)
                {
                    WindowData windowData = new WindowData(connectionDatabase.CurrentUsername);
                    windowData.Show();
                    this.Close();
                } else
                {
                    MessageBox.Show("uw inloggegevens zijn onjuist, controleer of u uw inloggegevens correct hebt ingevuld en probeer het opnieuw");
                }
            } else
            {
                MessageBox.Show("uw inloggegevens zijn onjuist, controleer of u uw inloggegevens correct hebt ingevuld en probeer het opnieuw");
            }
        }
    }
}

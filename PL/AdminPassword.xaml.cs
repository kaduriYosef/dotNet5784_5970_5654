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
using PL.ManagerInterface;

namespace PL
{
    /// <summary>
    /// Interaction logic for AdminPassword.xaml
    /// </summary>
    public partial class AdminPassword : Window
    {
        private int attemptCount = 0;

        public AdminPassword()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string correctPassword = "1234";
            if (passwordBox.Password == correctPassword)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                attemptCount++;
                if (attemptCount < 5)
                {
                    
                    MessageBox.Show($"You have {5 - attemptCount} more attempts",
                        "Incorrect password", MessageBoxButton.OK, MessageBoxImage.Error);
                    passwordBox.Clear();
                }
                else
                {
                    
                    this.DialogResult = false;
                    this.Close();
                }
            }
        }
    }
}

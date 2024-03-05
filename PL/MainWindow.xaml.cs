using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using PL.Engineer;
using PL.EngineerInterface;
using PL.ManagerInterface;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private void clock_run() 
        { 
            Thread.Sleep(1000);
            Time += new TimeSpan(1, 0, 0, 0);
        }
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public MainWindow()
        {
            InitializeComponent();
            new Thread(clock_run).Start();
        }



        public TimeSpan Time      
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Time.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(TimeSpan), typeof(MainWindow), new PropertyMetadata(new TimeSpan(0,0,0,0)));



        private void Button_Admin(object sender, RoutedEventArgs e)
        {
            var passwordWindow = new AdminPassword();
            var dialogResult = passwordWindow.ShowDialog();

            if (dialogResult == true)
                new ManagerInterfaceMainWindow().ShowDialog();
            else
                MessageBox.Show("Incorrect Password and too many attempts",
                        "Access Denied", MessageBoxButton.OK, MessageBoxImage.Error);
            
            
            //new ManagerInterfaceMainWindow().ShowDialog();
        }



        private void Button_Engineer(object sender, RoutedEventArgs e)
        {
            new EngineerInterfaceMainWindow().ShowDialog();
        }

    }
}
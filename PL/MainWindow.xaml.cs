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
using System.Windows.Threading;

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

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        // Changed the default value to DateTime.Now
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(s_bl.Clock));

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update CurrentTime from s_bl.Clock every time the timer ticks.
            s_bl.Clock.AddSeconds(1);
            CurrentTime = s_bl.Clock;
        }

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
        
        private void button_click_Add_hour(object sender, RoutedEventArgs e) {
            s_bl.AddHour(); CurrentTime = s_bl.Clock; }
        private void button_click_Add_day(object sender, RoutedEventArgs e) { s_bl.AddDay(); CurrentTime = s_bl.Clock; }
        private void button_click_Add_year(object sender, RoutedEventArgs e) { s_bl.AddYear(); CurrentTime = s_bl.Clock; }
        private void button_click_Reset_current_time(object sender, RoutedEventArgs e) { s_bl.ResetClock(); CurrentTime = s_bl.Clock; }
    }
}
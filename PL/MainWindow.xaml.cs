﻿using System.Text;
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
        private DispatcherTimer _timer;
        private bool isEnd = false;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            //isEnd = false;
            //Thread clockTicker = new Thread(Timer_Tick_2);
            //clockTicker.Start();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Your code here, for example, a confirmation dialog
            MessageBoxResult result = MessageBox.Show("Do you really want to close the window?", "Closing", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                // Cancel the closing process
                e.Cancel = true;
            }
            else
            {
                isEnd = true;
                s_bl.SaveClock(s_bl.Clock);
            }
        }


        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        // Changed the default value to DateTime.Now
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(s_bl.Clock));

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update CurrentTime from s_bl.Clock every time the timer ticks.
            s_bl.AddSeconds(1);
            CurrentTime = s_bl.Clock;
        }

        private void Timer_Tick_2()
        {
            while(isEnd==false)
            {
                Thread.Sleep(1000);
                s_bl.AddSeconds(1);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CurrentTime = s_bl.Clock;
                });
            }
        }

        private void Button_Admin(object sender, RoutedEventArgs e)
        {
            var passwordWindow = new AdminPassword();
            var dialogResult = passwordWindow.ShowDialog();

            if (dialogResult == true)
                new ManagerInterfaceMainWindow().ShowDialog();
            else
                MessageBox.Show("Incorrect ID or U and too many attempts",
                        "Access Denied", MessageBoxButton.OK, MessageBoxImage.Error);


            //new ManagerInterfaceMainWindow().ShowDialog();
        }

        private void Button_Engineer(object sender, RoutedEventArgs e)
        {
            var engineerId= new EngineerID();
            var dialogResult = engineerId.ShowDialog();

            //if (dialogResult == true)
            //    new EngineerInterfaceMainWindow(engineerId.id).ShowDialog();
            //else
            //    MessageBox.Show("Incorrect ID or User name and too many attempts",
            //            "Access Denied", MessageBoxButton.OK, MessageBoxImage.Error);

        }
        
        private void button_click_Add_hour(object sender, RoutedEventArgs e) {
            s_bl.AddHour(); CurrentTime = s_bl.Clock; }
        private void button_click_Add_day(object sender, RoutedEventArgs e) { s_bl.AddDay(); CurrentTime = s_bl.Clock; }
        private void button_click_Add_year(object sender, RoutedEventArgs e) { s_bl.AddYear(); CurrentTime = s_bl.Clock; }
        private void button_click_Reset_current_time(object sender, RoutedEventArgs e) { s_bl.ResetClock(); CurrentTime = s_bl.Clock; }
    }
}
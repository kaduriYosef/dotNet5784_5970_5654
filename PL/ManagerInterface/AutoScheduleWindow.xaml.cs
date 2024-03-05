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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL.ManagerInterface
{


    /// <summary>
    /// Interaction logic for AutoScheduleWindow.xaml
    /// </summary>
    public partial class AutoScheduleWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Date.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(AutoScheduleWindow), new PropertyMetadata());

        private ManagerInterfaceMainWindow mainWindow;



        public AutoScheduleWindow(ManagerInterfaceMainWindow mainWindow)
        {
            InitializeComponent();
            this.DataContext = this;
            Date = DateTime.Now;
            this.mainWindow = mainWindow; // Set the reference to the passed mainWindow
        }


        private void Button_OK(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    s_bl.Task.ScheduleAllDates(Date);
                    MessageBox.Show("All task were scheduled");
                    mainWindow.isStartDate = true; // Access isStartDate through mainWindow reference
                    this.Close();

                
            }
            catch(Exception ex) { MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

    }
}

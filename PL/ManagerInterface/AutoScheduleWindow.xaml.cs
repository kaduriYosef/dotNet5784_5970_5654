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




        public AutoScheduleWindow()
        {
            InitializeComponent();
            Date=DateTime.Now;
        }
        private void Button_OK(object sender, RoutedEventArgs e)
        {
            if(Date<DateTime.Now)
                MessageBox.Show("can't select a past date", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            

        }
    }
}

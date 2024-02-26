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

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public MainWindow()
        {
            InitializeComponent();
        }
       


        private void Button_Admin(object sender, RoutedEventArgs e)
        {
            new ManagerInterfaceMainWindow().ShowDialog();
        }



        private void Button_Engineer(object sender, RoutedEventArgs e)
        {
            new EngineerInterfaceMainWindow().ShowDialog();
        }

    }
}
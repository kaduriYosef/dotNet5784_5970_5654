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
using PL.Engineer;

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
        private void Button_Engineers(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().ShowDialog();
            
            //MessageBox.Show("it works");
        }

        private void Button_Init(object sender, RoutedEventArgs e)
        {
            MessageBoxResult a = MessageBox.Show("Would you like to create Initial data? (Y/N)", "Initialization data", 
                                   MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
            if(a==MessageBoxResult.Yes) { s_bl.InitializeDB(); }
        }

        private void Button_Admin(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("you're the manager");
        }

    }
}
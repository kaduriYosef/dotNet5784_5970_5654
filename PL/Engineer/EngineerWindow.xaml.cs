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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window 
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        int id = 0;
        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerWindowProperty); }
            set { SetValue(EngineerWindowProperty, value); }
        }

        public static readonly DependencyProperty EngineerWindowProperty =
            DependencyProperty.Register("Engineer",
            typeof(BO.Engineer),
            typeof(EngineerWindow),
            new PropertyMetadata(null));
        public EngineerWindow(int Id=0)
        {
            InitializeComponent();
            id = Id;
            if (Id != 0)
            {
                Engineer = s_bl.Engineer.Read(Id);
            }
            else
            {
                Engineer = new BO.Engineer() { Id = 0, Name = "Israel Israeli", Cost = 180, Email = "Israel", Level = 0 };
            }

        }
    

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {

            if (id == 0)
            {
                try
                {
                    s_bl.Engineer.Create(Engineer);
                    MessageBox.Show("The addition was made successfully", "Creat engineer",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    s_bl.Engineer.Update(Engineer);
                    MessageBox.Show("The Update was made successfully", "Creat engineer",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            Close();
            
        }
        
        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.All;
    }
}

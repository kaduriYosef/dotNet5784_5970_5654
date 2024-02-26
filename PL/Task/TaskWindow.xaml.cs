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
using PL.Engineer;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        int id = 0;

        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task",
                typeof(BO.Task),
                typeof(TaskWindow),
                new PropertyMetadata(null));

        public TaskWindow(int Id = 0)
        {
            InitializeComponent();
            id = Id;
            if (Id != 0)
                Task = s_bl.Task.Read(Id);
            else
                Task = new BO.Task()
                {
                    Id = 0,
                    Alias = null,
                    Description = null,
                    CreatedAtDate = DateTime.Now,
                    RequiredEffortTime = null,
                    Dependencies = null,
                    Complexity = 0,
                    ScheduledDate = DateTime.Today,
                    DeadlineDate = null,
                    Deliverables = null,
                    Remarks = null,
                    Engineer = null
                };
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (id == 0)
            {

                try
                {
                    s_bl.Task.Create(Task);
                    MessageBox.Show("The addition was made successfully", "Create Task",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    s_bl.Task.Update(Task);
                    MessageBox.Show("The Update was made successfully", "Update Task",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            Close();
            new TaskListWindow().ShowDialog();


        }

    }


        
}

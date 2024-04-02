using BO;
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

namespace PL.EngineerInterface
{
    /// <summary>
    /// Interaction logic for TaskChoice.xaml
    /// </summary>
    public partial class TaskChoice : Window
    { 
  // Access the business logic layer
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        // Dependency property for binding the list of tasks
        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskChoice), new PropertyMetadata(null));

        // Current engineer for whom the tasks are being displayed
        public BO.Engineer currentEngineer = new BO.Engineer();

    // Constructor for TaskForEngineer window
    public TaskChoice(BO.Engineer CurrentEngineer)
    {
        InitializeComponent();
        // Retrieve tasks based on engineer's level and availability
        TaskList = s_bl.Task.ReadAllSimplified(item => (int)item.Complexity <= (int)CurrentEngineer.Level && item.Engineer == null && item.StartDate == null);
        currentEngineer = CurrentEngineer;
    }

    // Event handler for selecting a task from the list
    private void ChooseTask_Button(object sender, MouseButtonEventArgs e)
    {
            // Check if a schedule of tasks has been set up
            if (s_bl.StartDate == null)
            {
                MessageBox.Show("A schedule of tasks needs to be set up first");
                return;
            }
            // Get the selected task
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            // Close the current window and open the window to add task for engineer
            Close();
            new AddingTaskToEngineerWindow(currentEngineer, task.Id).Show();
        }

    // Event handler for the Home button click
    private void Home_Click(object sender, RoutedEventArgs e)
    {
        // Close the current window and open the EngineerInterface main window for the current engineer
        Close();
        new EngineerInterfaceMainWindow(currentEngineer.Id).Show();
    }
}
}

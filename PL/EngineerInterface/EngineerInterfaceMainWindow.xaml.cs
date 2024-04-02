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
using BO;

namespace PL.EngineerInterface
{
    /// <summary>
    /// Interaction logic for EngineerInterfaceMain.xaml
    /// </summary>
    public partial class EngineerInterfaceMainWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); // Initializing s_bl with Factory.Get()

        private TaskWindowForEngineer? taskOfThisEngineer;

        // Dependency Property for TaskInList
        public IEnumerable<TaskInList> TaskInList
        {
            get { return (IEnumerable<TaskInList>)GetValue(TaskInListProperty); }
            set { SetValue(TaskInListProperty, value); }
        }
        public static readonly DependencyProperty TaskInListProperty =
            DependencyProperty.Register("TaskInList", typeof(IEnumerable<TaskInList>), typeof(EngineerInterfaceMainWindow), new PropertyMetadata(s_bl.Task.ReadAllSimplified()));

        // Dependency  Property for CurrentEngineer
        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }
        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerInterfaceMainWindow), new PropertyMetadata(null));


        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }
        public static readonly DependencyProperty CurrentTaskProperty =
     DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(EngineerInterfaceMainWindow), new PropertyMetadata(null));


        public EngineerInterfaceMainWindow(int Id = 0)
        {
            InitializeComponent();
            try
            {
                CurrentEngineer = s_bl.Engineer.Read(Id);
                CurrentTask = s_bl.Task.Read(item => item.Engineer!=null&&item.Engineer.Id == Id);
            }
            catch
            {
                CurrentTask = new BO.Task { Id = 0 };  // Creating a new Task if an exception occurs
            }
        }
        private void UpdateTask_Button(object sender, RoutedEventArgs e)
        {
            if (CurrentTask.Id == 0)
            {
                MessageBox.Show("You don't have a task yet");
                return;
            }
            if (s_bl.StartDate==null)
            {
                MessageBox.Show("A schedule of tasks needs to be set up first");
                return;
            }

            try
            {
                Close();
                new TaskWindowForEngineer(CurrentTask.Id).ShowDialog();
                new EngineerInterfaceMainWindow(CurrentEngineer.Id).Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void TaskOption_Button(object sender, RoutedEventArgs e)
        {
            if (s_bl.StartDate==null)   
                MessageBox.Show("the manager needs to choose a start date for the project first");
            else if (CurrentTask.Id != 0)    //check if the current task was finished
                MessageBox.Show("you need to finish your task first");
            else      //shoes the task that the engineer can do
            {
                Close();
                new TaskChoice(CurrentEngineer).ShowDialog();
                new EngineerInterfaceMainWindow(CurrentEngineer.Id).Show();
            }

        }

        private void TaskCompleted_Button(object sender, RoutedEventArgs e)
        {
            if (CurrentTask.Id == 0)
            {
                MessageBox.Show("you can finish your task if you don't have any task");
                return;
            }
            try
            {

                CurrentTask.CompleteDate = s_bl.Clock;// Setting CompleteDate to current date
                CurrentTask.Engineer = null;    // Clearing Engineer
                CurrentTask.Status = BO.Status.Done;
                s_bl.Task.Update(CurrentTask);  // Updating Task

                CurrentEngineer.Task = null;
                s_bl.Engineer.Update(CurrentEngineer);

                MessageBox.Show("thank you for doing your job  and completing the task");
                Close();
                new EngineerInterfaceMainWindow(CurrentEngineer.Id).Show();
            }
            catch { MessageBox.Show("Error"); }// Show error message if an exception occurs

        }
    }
}
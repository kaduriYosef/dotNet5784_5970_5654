using System;
using System.Collections;
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


namespace PL.Task
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public TaskListWindow()
        {
            InitializeComponent();
            TaskList = s_bl.Task.ReadAll()!;
        }
        public IEnumerable<BO.Task> TaskList
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }

        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList",
            typeof(IEnumerable<BO.Task>),
            typeof(TaskListWindow),
            new PropertyMetadata(null));

        public BO.EngineerExperience Complexity { get; set; } = BO.EngineerExperience.All;

        private void cbTaskComplexitySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Complexity == BO.EngineerExperience.All) ?
                (s_bl?.Task.ReadAll()!) : (s_bl?.Task.ReadAll(item => item.Complexity == Complexity)!);
        }

        private void click_update(object sender, MouseButtonEventArgs e)
        {
            BO.Task? task = (sender as ListView)?.SelectedItem as BO.Task;
            if (task != null)
            {
                this.Close();
                Window Update = new TaskWindow(task.Id);


                Update.ShowDialog();
                TaskList = s_bl.Task.ReadAll();

            }

        }

        private void add_engineer(object sender, RoutedEventArgs e)
        {
            this.Close();
            new TaskWindow().ShowDialog();
            TaskList = s_bl.Task.ReadAll();
        }
    }
}

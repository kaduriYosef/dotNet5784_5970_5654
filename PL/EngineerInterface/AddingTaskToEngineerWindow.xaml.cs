using BO;
using PL.Task;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PL.EngineerInterface;

/// <summary>
/// Interaction logic for AddingTaskToEngineer.xaml
/// </summary>
public partial class AddingTaskToEngineerWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.Task CurrentTask
    {
        get { return (BO.Task)GetValue(CurrentTaskProperty); }
        set { SetValue(CurrentTaskProperty, value); }
    }
    public static readonly DependencyProperty CurrentTaskProperty =
    DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(AddingTaskToEngineerWindow), new PropertyMetadata(null));

    public IEnumerable<BO.TaskInList> TaskList
    {
        get { return (IEnumerable<TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(AddingTaskToEngineerWindow), new PropertyMetadata(s_bl.Task.ReadAllSimplified()));

    public BO.Engineer currentEngineer = new BO.Engineer();
    public ObservableCollection<int> Ids { get; set; }

    private void CheckBox_Loaded(object sender, RoutedEventArgs e)
    {
        CheckBox checkBox = sender as CheckBox;
        if (checkBox != null && checkBox.Content is int id && Ids.Contains(id))
        {
            checkBox.IsChecked = true;
        }
    }

    public AddingTaskToEngineerWindow(BO.Engineer CurrentEngineer, int id)
    {

        
            InitializeComponent();
            Ids = new ObservableCollection<int>();


        try
        {
            CurrentTask = s_bl.Task.Read(id);
            currentEngineer = CurrentEngineer;

            if (CurrentTask != null)
            {
                foreach (var t in CurrentTask.Dependencies)
                    Ids.Add(t.Id);

            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void addTaskToEngineer_Button(object sender, RoutedEventArgs e)
    {
        try {
            BO.TaskInEngineer taskInEngineer = BO.Tools.fromTaskToTaskInEngineer(CurrentTask);
            currentEngineer.Task = taskInEngineer;
            //will throw an error if it is impossible to take this task
            s_bl.Engineer.Update(currentEngineer);


            CurrentTask.StartDate = s_bl.Clock;
            CurrentTask.Engineer = new EngineerInTask { Id = currentEngineer.Id, Name = currentEngineer.Name };
            CurrentTask.Status = BO.Status.OnTrack;

            s_bl.Task.Update(CurrentTask);
            Close();
            MessageBox.Show("congrajulations, from now this task is binded to you forever until you finish");
            //new EngineerInterfaceMainWindow(currentEngineer.Id).Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}

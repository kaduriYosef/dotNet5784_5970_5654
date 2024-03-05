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

namespace PL.ManagerInterface;

/// <summary>
/// Interaction logic for GanttWindow.xaml
/// </summary>
public partial class GanttWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    //רשימת המשימות עם תאריכי התחלה ומשך זמן
    public List<TaskForGantt> ListOfTask = new List<TaskForGantt>();


    
    public GanttWindow()
    {
        InitializeComponent();
        //ניצור רשימה של המשימות עם התאריך התחלה
        foreach (var task in s_bl.Task.ReadAll().Where(x=>x is not null))
        {
            var taskFromDal = s_bl.Task.Read(task!.Id);
            var stringOfDay = taskFromDal!.RequiredEffortTime.ToString();
            TaskForGantt newTaskForGantt = new TaskForGantt { id = task.Id, alias = task.Alias, taskDuration = int.Parse(stringOfDay.Substring(0, stringOfDay.IndexOf('.'))), startDate = taskFromDal.ScheduledDate };
            //TaskForGantt newTaskForGantt = new TaskForGantt { id = task.Id, alias = task.Alias, taskDuration = taskFromDal.RequiredEffortTime.days(), startDate = taskFromDal.ScheduledDate };
            ListOfTask.Add(newTaskForGantt);
        }
        //נמיין את הרשימה
        ListOfTask = ListOfTask.OrderBy(task => task.startDate).ToList();

        InitializeComponent();
        DataContext = new GanttChartViewModel();
    }
}

public class GanttTask
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public int Duration { get; set; } // Duration in days
}
public class GanttChartViewModel
{
    public ObservableCollection<GanttTask> Tasks { get; set; }

    public GanttChartViewModel()
    {
        Tasks = new ObservableCollection<GanttTask>
    {
        new GanttTask { Name = "Task 1", StartDate = DateTime.Today, Duration = 5 },
        new GanttTask { Name = "Task 2", StartDate = DateTime.Today.AddDays(3), Duration = 7 },
        // Add more tasks as needed
    };
    }
}

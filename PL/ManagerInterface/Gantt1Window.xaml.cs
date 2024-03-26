using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for GanttWindow.xaml
    /// </summary>
    public partial class Gantt1Window : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        //רשימת המשימות עם תאריכי התחלה ומשך זמן
        public List<TaskForGantt> ListOfTask = new List<TaskForGantt>();

        public Gantt1Window()
        {
            InitializeComponent();
            //ניצור רשימה של המשימות עם התאריך התחלה
            foreach (var task in s_bl.Task.ReadAll().Where(x=>x is not null))
            {
               // var taskFromDal = s_bl.Task.Read(task.Id);
                var stringOfDay = task.RequiredEffortTime.ToString();
                TaskForGantt newTaskForGantt = new TaskForGantt { id = task.Id, alias = task.Alias, taskDuration = int.Parse(stringOfDay.Substring(0, stringOfDay.IndexOf('.'))), scheduledDate = task.ScheduledDate,completeDate=task.CompleteDate };
                ListOfTask.Add(newTaskForGantt);
            }
            //sort the list by start date
            ListOfTask = ListOfTask.OrderBy(task => task.scheduledDate).ToList();

            this.Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Canvas canvas = FindVisualChild<Canvas>(this);
            if (canvas == null) return;

            DateTime minStartDate = ListOfTask.Min(task => task.scheduledDate ?? DateTime.MaxValue);
            DateTime maxEndDate = ListOfTask.Max(task => (task.scheduledDate ?? DateTime.MinValue).AddDays(task.taskDuration));

            double maxAliasWidth = GetMaxAliasWidth(); // קביעת הרוחב המקסימלי של הכינויים

            double maxWidth = ListOfTask.Max(task => ((task.scheduledDate ?? DateTime.Today) - minStartDate).TotalDays * 10 + task.taskDuration * 10) + maxAliasWidth + 20; // הוספת רווח קצת בסוף
            canvas.Width = maxWidth;
            canvas.Height = ListOfTask.Count * 30 + 60; // הוספת רווח לסרגל התאריכים ולמשימות

            double topPosition = 40; // מתחילים מ-40 פיקסלים למעלה כדי לתת מקום לסרגל התאריכים

            foreach (var task in ListOfTask)
            {
                double offsetDays = ((task.scheduledDate ?? DateTime.Today) - minStartDate).TotalDays;
                double leftPosition = offsetDays * 10 + maxAliasWidth; // המלבנים מתחילים לאחר הטקסט הארוך ביותר
                bool is_late = false;
                if(task.scheduledDate<s_bl.Clock)
                    is_late = true;
                if(s_bl.Task.ReadAll(t=> s_bl.Task.Read(task.id).Dependencies.Any(t1=>t1.Id==t.Id )).Where(t2=>t2.ScheduledDate<s_bl.Clock).Any())
                    is_late=true;
                // הוספת תווית של שם המשימה
                TextBlock aliasLabel = new TextBlock
                {
                    Text = task.alias,
                    Foreground = Brushes.Black,
                    FontWeight = FontWeights.Bold
                };
                canvas.Children.Add(aliasLabel);
                Canvas.SetLeft(aliasLabel, 5); // קצת רווח מהשוליים השמאליים
                Canvas.SetTop(aliasLabel, topPosition);


                var red = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x00, 0x00));
                var green = new SolidColorBrush(Color.FromArgb(0xFF,0x00,0xFF,0x00));
                var normal = new SolidColorBrush(Color.FromArgb(0x88, 0x1E, 0x2F, 0x47));
                var color =(is_late ? red : normal);
                color = (task.completeDate!=null)?green:color;
                Rectangle rectangle = new Rectangle
                {
                    Fill = color,
                    Width = task.taskDuration * 10, // כפל המשך המשימה ב-10 לדוגמה
                    Height = 20,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 1,
                    RadiusX = 5, // רדיוס העיגול של הקצוות בציר X
                    RadiusY = 5 // רדיוס העיגול של הקצוות בציר Y
                };

                canvas.Children.Add(rectangle);
                Canvas.SetLeft(rectangle, leftPosition);
                Canvas.SetTop(rectangle, topPosition);

                // הוספת תווית תאריך ומשך זמן למלבן
                TextBlock dateLabel = new TextBlock
                {
                    Text = $"{task.scheduledDate?.ToString("dd/MM")} + {task.taskDuration}",
                    Foreground = new SolidColorBrush(Colors.White),
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center
                };
                canvas.Children.Add(dateLabel);
                Canvas.SetLeft(dateLabel, leftPosition + 2); // כוונון עדין למיקום התווית
                Canvas.SetTop(dateLabel, topPosition + 2); // כוונון עדין למיקום התווית

                topPosition += 30; // עדכון המיקום האנכי למשימה הבאה
            }

            // הוספת סרגל התאריכים
            AddMonthLabels(canvas, minStartDate, maxEndDate);
        }

        private double GetMaxAliasWidth()
        {
            double maxAliasWidth = 0;
            foreach (var task in ListOfTask)
            {
                TextBlock tempTextBlock = new TextBlock
                {
                    Text = task.alias,
                    FontWeight = FontWeights.Bold
                };
                tempTextBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                double width = tempTextBlock.DesiredSize.Width;
                if (width > maxAliasWidth)
                {
                    maxAliasWidth = width;
                }
            }
            return maxAliasWidth + 10; // נוסף רווח של 10 פיקסלים
        }


        private void AddMonthLabels(Canvas canvas, DateTime minStartDate, DateTime maxEndDate)
        {
            double pixelsPerDay = 10; // נניח שכל יום מיוצג על ידי 10 פיקסלים

            // חישוב הרוחב המקסימלי של הכינויים
            double maxAliasWidth = GetMaxAliasWidth();

            DateTime currentMonth = new DateTime(minStartDate.Year, minStartDate.Month, 1);
            while (currentMonth <= maxEndDate)
            {
                DateTime nextMonth = currentMonth.AddMonths(1);
                bool isMonthStartVisible = ListOfTask.Any(task => task.scheduledDate.HasValue && task.scheduledDate.Value.Month == currentMonth.Month && task.scheduledDate.Value.Year == currentMonth.Year);

                double leftPosition;
                if (isMonthStartVisible)
                {
                    // אם יש מלבן שמתחיל בחודש זה, בדוק את המיקום המדויק של התחלת המלבן
                    DateTime firstTaskStart = ListOfTask.Where(task => task.scheduledDate.HasValue && task.scheduledDate.Value.Month == currentMonth.Month && task.scheduledDate.Value.Year == currentMonth.Year)
                                                         .Min(task => task.scheduledDate.Value);

                    if (firstTaskStart.Day > 1)
                    {
                        // אם המלבן הראשון מתחיל אחרי ה-1 בחודש, מקם את תווית החודש משמאל למלבן
                        leftPosition = ((firstTaskStart - minStartDate).TotalDays - firstTaskStart.Day + 1) * pixelsPerDay + maxAliasWidth;
                    }
                    else
                    {
                        // אם המלבן מתחיל ב-1 בחודש, מקם את תווית החודש בהתאם למיקום הסטנדרטי
                        leftPosition = ((currentMonth - minStartDate).TotalDays) * pixelsPerDay + maxAliasWidth;
                    }
                }
                else
                {
                    // אם אין מלבן שמתחיל בחודש זה, מקם את תווית החודש בהתאם למיקום הסטנדרטי
                    leftPosition = ((currentMonth - minStartDate).TotalDays) * pixelsPerDay + maxAliasWidth;
                }

                TextBlock monthLabel = new TextBlock
                {
                    Text = currentMonth.ToString("MMM yyyy"),
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center
                };

                Canvas.SetLeft(monthLabel, leftPosition);
                Canvas.SetTop(monthLabel, 0); // אתה יכול להתאים את הגובה כמו שאתה רוצה

                canvas.Children.Add(monthLabel);

                currentMonth = nextMonth;
            }
        }




        // מתודה עזר לחיפוש רכיב בעץ הוויזואלי לפי סוג
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    var childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
    public class TaskForGantt
    {
        public int id { get; set; }
        public string alias { get; set; }
        public int taskDuration { get; set; }
        public DateTime? scheduledDate { get; set; }
        public DateTime? completeDate { get; set; }
    }
}
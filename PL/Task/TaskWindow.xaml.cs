﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
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



        public IEnumerable<BO.EngineerInTask> EngineerList
        {
            get { return (IEnumerable<BO.EngineerInTask>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EngineerList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList",
                typeof(IEnumerable<BO.EngineerInTask>),
                typeof(TaskWindow),
                new PropertyMetadata(null));


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

        public IEnumerable<BO.TaskInList> TaskListDep
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListDepProperty); }
            set { SetValue(TaskListDepProperty, value); }

        }

        public static readonly DependencyProperty TaskListDepProperty =
            DependencyProperty.Register("TaskListDep",
            typeof(IEnumerable<BO.TaskInList>),
            typeof(TaskListWindow),
            new PropertyMetadata(null));

        public ObservableCollection<int> Ids { get; set; }
        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {

            if (Ids != null && sender is CheckBox checkBox && checkBox.Tag is int id && Ids.Contains(id))
            {
                checkBox.IsChecked = true;
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.Tag is int id)
            {
                if (Ids != null&&!Ids.Contains(id))
                    Ids.Add(id);
            }
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Ids != null && sender is CheckBox checkBox && checkBox.Tag is int id)
            {
                Ids.Remove(id);
            }
        }
        public TaskWindow(int Id = 0)
        {

            InitializeComponent();


            EngineerList = from eng in s_bl.Engineer.ReadAll()
            select new BO.EngineerInTask()
            {
                Id = eng.Id,
                Name = eng.Name,
            };
            TaskListDep = from task in s_bl.Task.ReadAllSimplified()
                       select new BO.TaskInList()
                       {
                           Id = task.Id,
                           Description = task.Description,
                           Alias = task.Alias,
                           Status = task.Status,
                       };
                                                            //looks unnecessary

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
                    Dependencies = TaskListDep.ToList(),
                    Complexity = 0,
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

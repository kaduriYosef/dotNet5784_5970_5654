using System;
using DalTest;

namespace BlTest;

using System.Reflection.Emit;
using BO; // Assuming the namespace for the classes
using System.Xml;

static class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static void Main(string[] args)
    {
        // BO.Engineer engineer = new BO.Engineer() 
        // {Id=100,Name="who",Task=new TaskInEngineer() { Id=10,Alias="report"},
        // Cost=100.22,
        // Email="email",
        // Level=EngineerExperience.Intermediate,

        //AdditionalTasks=new List<TaskInEngineer>() { new TaskInEngineer() { Id = 10, Alias = "report" }, new TaskInEngineer() { Id = 10, Alias = "report" }, new TaskInEngineer() { Id = 10, Alias = "report" } }
        // };
        // Console.WriteLine(engineer);
        BO.Task task = new BO.Task()
        {
            Id = 1,
            Alias = "first",
            Description = "desc",
            CreatedAtDate = DateTime.Now,
            Status = null,
            StartDate = null,
            ScheduledDate = null,
            ForecastDate = null,
            CompleteDate = null,
            Complexity = EngineerExperience.Advanced,
            DeadlineDate = null,
            RequiredEffortTime = null,
            Milestone = null,
            Dependencies = new List<TaskInList>()
                {
                    new TaskInList(){Id=0, Alias="zero",Description="aer",Status=BO.Status.Scheduled},
                    new TaskInList(){Id=1, Alias="one",Description="aer",Status=BO.Status.Scheduled},
                    new TaskInList(){Id=2, Alias="two",Description="aer",Status=BO.Status.Scheduled},
                    new TaskInList(){Id=3, Alias="three",Description="aer",Status=BO.Status.Scheduled},


                },
            Deliverables = null,
            Remarks = null,
            Engineer = new BO.EngineerInTask()
            {
                Id = 100,
                Name = "who"
            }



        };
        Console.WriteLine(task);



    }



}

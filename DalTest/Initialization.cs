
namespace DalTest;

using System.Data;
using System.Data.Common;
using Dal;
using DalApi;
using DO;

public static class Initialization
{
    private static ITask? s_dalTask; //stage 1
    private static IEngineer? s_dalEngineer; //stage 1
    private static IDependency? s_dalDependency; //stage 1


    private static readonly Random s_rand = new();

    public static bool DependencyImplemination { get; private set; }

    private static void createTask()
    {
        string[] tasksForEngineer = {
            "Set Up Development Environment",
            "Implement User Registration",
            "Optimize Database Performance",
            "Create Dashboard Layout",
            "Resolve Server Downtime Issues",
            "Automate Deployment Process",
            "Integrate Predictive Analytics",
            "Implement Secure Payment Processing",
            "Review and Refactor Codebase",
            "Design RESTful API Endpoints",
            "Update Software Libraries",
            "Address and Fix QA Findings",
            "Implement Data Encryption Feature",
            "Write Unit Tests for Core Functions",
            "Enhance Front-end Responsiveness",
            "Connect with External API",
            "Refine Legacy Code Structure",
            "Deploy to Staging Environment",
            "Craft UI for New Module",
            "Establish CI/CD Pipeline",
            "Optimize Server Resource Usage",
            "Develop GraphQL Query Endpoints",
            "Migrate Data with Scripting",
            "Implement Data Caching Strategy",
            "Troubleshoot Production Bugs",
            "Document Coding Standards",
            "Optimize Mobile UI",
            "Integrate Cloud Storage API",
            "Enable Push Notifications",
            "Implement New Feature Module",
        };

        string[] tasksDescription = {
            "Prepare the development environment for efficient coding and testing.",
            "Build user registration functionality to enable account creation.",
            "Enhance database performance for quicker data retrieval.",
            "Create a visually appealing layout for the main dashboard.",
            "Investigate and resolve issues causing server downtime.",
            "Automate the deployment process for smoother releases.",
            "Incorporate predictive analytics to enhance decision-making.",
            "Implement secure payment processing for financial transactions.",
            "Review and refactor existing codebase for improved maintainability.",
            "Design and implement RESTful API endpoints for seamless integration.",
            "Update software libraries to leverage new features and security updates.",
            "Address and fix any issues identified during QA testing.",
            "Implement data encryption features to enhance security.",
            "Write comprehensive unit tests to ensure the reliability of core functions.",
            "Enhance front-end responsiveness for a better user experience.",
            "Connect with an external API to leverage external services.",
            "Refine the structure of legacy code for better readability.",
            "Deploy the application to a staging environment for testing.",
            "Craft a user interface for a new module in the application.",
            "Establish a CI/CD pipeline for automated testing and deployment.",
            "Optimize server resource usage for improved efficiency.",
            "Develop GraphQL query endpoints for flexible data retrieval.",
            "Migrate data between databases using scripting techniques.",
            "Implement a data caching strategy to optimize performance.",
            "Troubleshoot and fix bugs reported in the production environment.",
            "Document coding standards to maintain consistency in development.",
            "Optimize the mobile user interface for cross-device compatibility.",
            "Integrate with a cloud storage API for efficient data storage.",
            "Enable push notifications to keep users informed of updates.",
            "Implement a new feature module to expand application functionality.",
        };

        int[] engineerLevelsForTask = {
    3, // Set Up Development Environment
    2, // Implement User Registration
    4, // Optimize Database Performance
    3, // Create Dashboard Layout
    5, // Resolve Server Downtime Issues
    3, // Automate Deployment Process
    4, // Integrate Predictive Analytics
    5, // Implement Secure Payment Processing
    2, // Review and Refactor Codebase
    4, // Design RESTful API Endpoints
    3, // Update Software Libraries
    1, // Address and Fix QA Findings
    4, // Implement Data Encryption Feature
    2, // Write Unit Tests for Core Functions
    3, // Enhance Front-end Responsiveness
    4, // Connect with External API
    2, // Refine Legacy Code Structure
    3, // Deploy to Staging Environment
    1, // Craft UI for New Module
    4, // Establish CI/CD Pipeline
    3, // Optimize Server Resource Usage
    5, // Develop GraphQL Query Endpoints
    3, // Migrate Data with Scripting
    4, // Implement Data Caching Strategy
    1, // Troubleshoot Production Bugs
    2, // Document Coding Standards
    3, // Optimize Mobile UI
    4, // Integrate Cloud Storage API
    2, // Enable Push Notifications
    5, // Implement New Feature Module
};

        List<Task> tasks = new List<Task>();
        foreach (var task in tasksForEngineer)
        {
            int Id = 0;
            int rand = s_rand.Next();
            string Alias = task;
            string Description = task;
            DateTime start = new DateTime(2000, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime CreatedAtDate = start.AddDays(s_rand.Next(range));
            TimeSpan? RequiredEffortTime = TimeSpan.FromDays(s_rand.Next(30, 365));
            bool IsMilestone = range % 2 == 0;
            EngineerExperience? Copmlexity = (EngineerExperience)(s_rand.Next(5));
            DateTime? StartDate = null;
            int i = RequiredEffortTime?.Days ?? 0;
            DateTime? ScheduledDate = CreatedAtDate.AddDays(s_rand.Next(range - i));
            DateTime? DeadlineDate = ScheduledDate?.AddDays(i + s_rand.Next(10, 40));
            DateTime? CompleteDate = null;
            string? Deliverables = null;
            string? Remarks = null;
            int? EngineerId = null;
            Task newTask = new Task(Id, Alias, Description, CreatedAtDate, RequiredEffortTime, IsMilestone, Copmlexity, StartDate,
                                    ScheduledDate, DeadlineDate, CompleteDate, Deliverables, Remarks, EngineerId);
            s_dalTask!.Create(newTask);
        }
    }

    private static void createEngineer()
    {
        string[] engineerNames =
           { "Avi", "Barak", "Gal", "Dana", "Ilana",
            "Shira", "Amit", "Noa", "Israel", "Rachel" };
        foreach (var en in engineerNames)
        {
            int id;
            do
                id = s_rand.Next(200000000, 400000000);
            while (s_dalEngineer!.Read(id) != null);
            string name = en;
            string email = en + "@gmail.com";
            double cost = s_rand.Next(10000, 40000);
            bool activ = (id % 1000 > cost) ? true : false;
            EngineerExperience level = (EngineerExperience)s_rand.Next(5);
            Engineer engineer = new Engineer(id, email, cost, name, level, activ);

        }
    }
    private static void createDependency()
    {

        int Id = 0;
        int DependentTask;
        int DependsOnTask;
        DependencyImplementation tmpDependencyImplement = new DependencyImplementation();
        Dependency[] necessaryDependencies = new Dependency[] 
        {new Dependency(1006,1003),new Dependency(1006,1002),new Dependency(1007,1003),new Dependency(1007,1002) };

        foreach (var dep in necessaryDependencies)
        {
            tmpDependencyImplement.Create(dep);
            s_dalDependency!.Create(dep);
        }
        for (int i = 0; i < 50; ++i)
        {
            do
            {
                DependentTask = s_rand.Next(1000, 1030);
                DependsOnTask = s_rand.Next(1000, DependentTask);
            } while (tmpDependencyImplement.DoesExist(DependentTask, DependsOnTask));
            Dependency depNew = new Dependency(Id, DependentTask, DependsOnTask);
            tmpDependencyImplement.Create(depNew);
            s_dalDependency!.Create(depNew);
        }

    }

    public static void Do(ITask? dalTask,IEngineer? dalEngineer, IDependency? dalDependency)
    {
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        createTask();
        createEngineer();
        createDependency();
    }
}


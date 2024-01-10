
namespace DalTest;

using System.Data;
using System.Data.Common;
using Dal;
using DalApi;
using DO;

/// <summary>
/// Provides methods to initialize and manage tasks, engineers, and dependencies for a project. 
/// This class cannot be inherited.
/// </summary>
public static class Initialization
{
    private static ITask? s_dalTask; //stage 1
    private static IEngineer? s_dalEngineer; //stage 1
    private static IDependency? s_dalDependency; //stage 1

    private static readonly Random s_rand = new();

    

    /// <summary>
    /// Creates a collection of tasks with predefined names and descriptions.
    /// Each task is associated with a unique ID, creation date, effort time, and other properties.
    /// Tasks are then added to the data access layer (DAL).
    /// </summary>
    private static void createTask()
    {

        // Array of 30 fundraising tasks
        string[] tasksNames = new string[]
        {
            "Define fundraising goals", // 0
            "Identify target audience", // 1
            "Create a fundraising plan", // 2
            "Establish a budget", // 3
            "Form a fundraising team", // 4
            "Train the team", // 5
            "Develop fundraising materials", // 6
            "Create a donor database", // 7
            "Plan fundraising events", // 8
            "Develop online donation platform", // 9
            "Create promotional materials", // 10
            "Launch social media campaign", // 11
            "Contact potential sponsors", // 12
            "Organize community outreach", // 13
            "Plan direct mail campaign", // 14
            "Prepare press releases", // 15
            "Conduct sponsor meetings", // 16
            "Launch fundraising events", // 17
            "Monitor fundraising progress", // 18
            "Adjust strategies as needed", // 19
            "Engage with donors", // 20
            "Process donations", // 21
            "Send thank-you notes", // 22
            "Evaluate event success", // 23
            "Update donor database", // 24
            "Report to stakeholders", // 25
            "Plan for next campaign", // 26
            "Conduct financial audit", // 27
            "Prepare final report", // 28
            "Celebrate success"  // 29
        };

        // Array of corresponding task descriptions
        string[] taskDescriptions = new string[]
        {
            "Outline specific, measurable goals for the fundraising campaign.",
            "Research and determine the demographic and psychographic characteristics of potential donors.",
            "Draft a detailed plan covering strategies, timelines, and responsibilities.",
            "Determine the financial requirements and create a budget for the campaign.",
            "Assemble a team of individuals with skills pertinent to fundraising.",
            "Provide necessary training and resources to the fundraising team.",
            "Create engaging materials like brochures, videos, and flyers for the campaign.",
            "Build a database to manage donor information and track contributions.",
            "Design and organize fundraising events, including logistics and programming.",
            "Develop a secure and user-friendly online platform for accepting donations.",
            "Produce promotional content for various media channels.",
            "Implement a campaign on social media platforms to increase awareness and donations.",
            "Reach out to potential corporate sponsors to seek financial or in-kind support.",
            "Engage with the local community through events and activities to boost support.",
            "Plan and execute a direct mail campaign to solicit donations.",
            "Write and distribute press releases to media outlets to gain publicity.",
            "Hold meetings with potential and existing sponsors to secure funding.",
            "Execute the planned fundraising events, ensuring they run smoothly.",
            "Regularly track and analyze the progress of the fundraising efforts.",
            "Modify and adapt fundraising strategies based on performance and feedback.",
            "Actively communicate with donors to build relationships and encourage ongoing support.",
            "Handle the receipt, recording, and acknowledgment of donations.",
            "Send personalized thank-you notes to donors to show appreciation and foster loyalty.",
            "Assess the success of each event and overall campaign effectiveness.",
            "Update the donor database with the latest information post-campaign.",
            "Compile and present a report of the campaign's outcomes to stakeholders.",
            "Begin planning for future fundraising efforts based on learned insights.",
            "Perform a thorough audit of the campaign's finances for transparency and accountability.",
            "Prepare a comprehensive report detailing the campaign's activities and financials.",
            "Celebrate the success of the campaign with the team and stakeholders."
        };

        int[] engineerLevelsForTask = {
            2, // Define fundraising goals
            1, // Identify target audience
            3, // Create a fundraising plan
            2, // Establish a budget
            4, // Form a fundraising team
            2, // Train the team
            3, // Develop fundraising materials
            4, // Create a donor database
            1, // Plan fundraising events
            3, // Develop online donation platform
            2, // Create promotional materials
            0, // Launch social media campaign
            3, // Contact potential sponsors
            1, // Organize community outreach
            2, // Plan direct mail campaign
            3, // Prepare press releases
            1, // Conduct sponsor meetings
            2, // Launch fundraising events
            0, // Monitor fundraising progress
            3, // Adjust strategies as needed
            2, // Engage with donors
            4, // Process donations
            2, // Send thank-you notes
            3, // Evaluate event success
            0, // Update donor database
            1, // Report to stakeholders
            2, // Plan for next campaign
            3, // Conduct financial audit
            1, // Prepare final report
            4  // Celebrate success
};



        for (int i = 0; i < tasksNames.Length; i++) 
        {
            int Id = 0;
            int rand = s_rand.Next();
            string Alias = tasksNames[i];
            string Description = taskDescriptions[i];
            DateTime start = new DateTime(2000, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime CreatedAtDate = start.AddDays(s_rand.Next(range));
            TimeSpan? RequiredEffortTime = TimeSpan.FromDays(s_rand.Next(30, 365));
            bool IsMilestone = range % 2 == 0;
            //since the engineers are sorted by thier experience
            EngineerExperience? Copmlexity = (EngineerExperience)(engineerLevelsForTask[i]);
            DateTime? StartDate = null;
            int timeSpent= RequiredEffortTime?.Days ?? 0;
            DateTime? ScheduledDate = CreatedAtDate.AddDays(s_rand.Next(range - timeSpent));
            DateTime? DeadlineDate = ScheduledDate?.AddDays(timeSpent + s_rand.Next(10, 40));
            DateTime? CompleteDate = null;
            string? Deliverables = null;
            string? Remarks = null;
            int? EngineerId = null;
            Task newTask = new Task(
                Id,
                Alias, 
                Description, 
                CreatedAtDate, 
                RequiredEffortTime, 
                IsMilestone, 
                Copmlexity,
                StartDate,
                ScheduledDate, 
                DeadlineDate, 
                CompleteDate, 
                Deliverables, 
                Remarks, 
                EngineerId);
            s_dalTask!.Create(newTask);
        }
    }
    /// <summary>
    /// Creates a set of engineers with predefined names.
    /// Each engineer is associated with a unique ID, email, cost, name, experience level, and active status.
    /// Engineers are then added to the data access layer (DAL).
    /// </summary>
    private static void createEngineer()
    {
        
        int[] idForEngineer = 
        { 
            s_rand.Next(1000000,2999999), 
            s_rand.Next(3000000,3999999),
            s_rand.Next(4000000,4999999),
            s_rand.Next(5000000,5999999),
            s_rand.Next(6000000,6999999) 
        };
        string[] names = new string[] { "Dan", "Sam", "Jordan", "Taylor", "Morgan" };
        for (int i = 0; i < 5; i++)
        {
            int id = idForEngineer[i]; // 7-digit ID
            string email = names[i] + s_rand.Next(34)+"@gmail.com";
            string name = names[i];
            
            //the engineers are sorted by their levels
            EngineerExperience level = (EngineerExperience)i;
            
            double cost = 40 * s_rand.Next(i, i+2);
            bool active = true;

           
            // Add engineer to your collection or process them as needed
            s_dalEngineer!.Create(new Engineer(id, email, cost, name, level, active));
        }
    }
    /// <summary>
    /// Creates dependencies between different tasks.
    /// Each dependency is represented by a pair of tasks, where one task depends on the completion of another.
    /// Dependencies are then added to the data access layer (DAL).
    /// </summary>
    private static void createDependency()
    {

        int Id = 0;
        // Array indicating the index of each dependent task
        int[] dependentTasks = new int[50]
        {
    2, 3, 4, 4, 5, 6, 6, 7, 8, 8,
    9, 10, 11, 12, 13, 14, 15, 16, 17, 18,
    19, 20, 21, 22, 23, 24, 25, 26, 27, 28,
    2, 5, 8, 11, 12, 14, 17, 18, 20, 21,
    22, 23, 24, 26, 27, 28, 29, 9, 15, 19
        };

        // Array indicating the indices of the tasks each dependent task relies on
        int[] dependsOnTask = new int[50]
        {
    0, 1, 2, 3, 0, 4, 2, 6, 5, 7,
    8, 9, 10, 11, 12, 13, 14, 15, 16, 17,
    18, 19, 20, 21, 22, 23, 24, 25, 26, 27,
    1, 3, 6, 9, 10, 12, 14, 16, 18, 19,
    20, 21, 22, 24, 25, 26, 27, 2, 13, 17
        };
       
        for(int i=0; i < 50; i++) 
        {
            s_dalDependency!.Create(new Dependency(Id,1000 + dependentTasks[i], 1000 + dependsOnTask[i]));
        }
    

    }
    /// <summary>
    /// Initializes the data access layer (DAL) with tasks, engineers, and dependencies.
    /// Throws a NullReferenceException if any of the DAL components are null.
    /// </summary>
    /// <param name="dalTask">Interface for task management.</param>
    /// <param name="dalEngineer">Interface for engineer management.</param>
    /// <param name="dalDependency">Interface for dependency management.</param>
    /// <exception cref="NullReferenceException">Thrown when any of the DAL parameters are null.</exception>
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


using System.Diagnostics.Contracts;

namespace BO;

/// <summary>
/// Represents the experience level of an engineer, ranging from beginner to expert.
/// This helps in assigning tasks appropriate to the engineer's skill level.
/// </summary>
public enum EngineerExperience
{
    /// <summary>Engineer with basic understanding and limited practical experience.</summary>
    Beginner = 0,
    /// <summary>Engineer who has surpassed the novice level but still requires guidance.</summary>
    AdvancedBeginner,
    /// <summary>Engineer with a moderate level of experience and some degree of independence.</summary>
    Intermediate,
    /// <summary>Engineer with a high level of expertise and ability to handle complex tasks independently.</summary>
    Advanced,
    /// <summary>Engineer with exceptional skill and experience, often capable of leadership roles in technical projects.</summary>
    Expert,
    /// <summary>without sinun</summary>
    All
}

/// <summary>
/// the same enum but only levels without the option "All" 
/// </summary>
//public enum EngineerExperienceOnlyLevels
//{
//    /// <summary>Engineer with basic understanding and limited practical experience.</summary>
//    Beginner = 0,
//    /// <summary>Engineer who has surpassed the novice level but still requires guidance.</summary>
//    AdvancedBeginner,
//    /// <summary>Engineer with a moderate level of experience and some degree of independence.</summary>
//    Intermediate,
//    /// <summary>Engineer with a high level of expertise and ability to handle complex tasks independently.</summary>
//    Advanced,
//    /// <summary>Engineer with exceptional skill and experience, often capable of leadership roles in technical projects.</summary>
//    Expert
//}

/// <summary>
/// Represents the current status of a task or project, indicating its progress or completion state.
/// </summary>
public enum Status
{
    /// <summary>Task or project has not been scheduled yet.</summary>
    Unscheduled,
    /// <summary>Task or project has been scheduled but not started.</summary>
    Scheduled,
    /// <summary>Task or project is underway and progressing as planned.</summary>
    OnTrack,
    /// <summary>Task or project is at risk of not meeting its deadlines.</summary>
    InJeopardy,   // Note: may not be used depending on business logic implementation.
    /// <summary>Task or project has been completed.</summary>
    Done
}

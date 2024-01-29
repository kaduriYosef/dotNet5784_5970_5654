
namespace BO;

/// <summary>
/// 
/// </summary>
public class Engineer
{

    public int Id { get; init; }
    public string Name {  get; init; }
    public string Email {  get; set; }
    public EngineerExperience Level {  get; set; }
    public double Cost {  get; set; }
    public TaskInEngineer? Task {  get; set; }

    
    //added by my initiative 
    
    public List<TaskInEngineer>? Tasks { get; set; }



}

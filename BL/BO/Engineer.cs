
namespace BO;

/// <summary>
/// 
/// </summary>
public class Engineer
{

    public int Id { get; init; }
    public string Name {  get; set; }
    public string Email {  get; set; }
    public EngineerExperience Level {  get; set; }
    public double Cost {  get; set; }
    public TaskInEngineer? Task {  get; set; }

    
    //added by my initiative 
    
    public List<TaskInEngineer>? AdditionalTasks { get; set; }

    public override string ToString() => this.ToStringProperty();

}

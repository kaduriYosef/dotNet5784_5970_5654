
namespace BO;

public class Milestone
{
    public int Id { get; init; }
    public string Description;
    public string Alias;
    public DateTime CreatedAtDate;
    public Status? Status;
    public DateTime? ForecastDate;
    public DateTime? DeadlineDate;
    public DateTime? CompleteDate;
    public double? CompletionPercentage;
    public string? Remarks;
    public List<TaskInList>? Dependencies;
}

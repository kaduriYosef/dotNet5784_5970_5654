﻿
namespace BO;

/// <summary>
/// 
/// </summary>
public class Milestone
{
    public int Id { get; init; }
    public string Description {  get; set; }
    public string Alias {  get; set; }
    public DateTime CreatedAtDate {  get; init; }
    public Status? Status {  get; set; }
    public DateTime? ForecastDate {  get; set; }
    public DateTime? DeadlineDate {  get; set; }
    public DateTime? CompleteDate {  get; set; }
    public double? CompletionPercentage {  get; set; }
    public string? Remarks {  get; set; }
    public List<TaskInList>? Dependencies {  get; set; }
}

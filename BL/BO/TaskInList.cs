
using System.ComponentModel;

namespace BO;

public class TaskInList
{
    public int Id { get; init; }
    public string Description;
    public string Alias;
    public Status? Status; 
}

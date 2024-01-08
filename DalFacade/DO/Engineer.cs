
using System.Net.Security;

namespace DO;

public record Engineer
(
    int Id,
    string Email="",
    double Cost=0.0,
    string Name="",
    EngineerExperience Level=EngineerExperience.Beginner,
    bool Active= true
)
{
    public Engineer() :this(0){ }
}

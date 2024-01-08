
using System.Net.Security;

namespace DO;

public record Engineer
(
    int Id,
    string? Email=null,
    double? Cost=null,
    string? Name=null,
    EngineerExperience? Level=null,
    bool? Active= true
)
{
    public Engineer() :this(0){ }
}

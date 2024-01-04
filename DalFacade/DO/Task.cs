
namespace DO;

public record Task
(
int Id,
string Alias,
string Description,
DateTime CreatedAtDate,
TimeSpan RequiredEffortTime,
bool IsMilestone
    );

namespace Middleware_Bridge_ASPNet;

// Job class definition
public class Job
{
    public int JobId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<Interview> Interviews { get; set; }
}
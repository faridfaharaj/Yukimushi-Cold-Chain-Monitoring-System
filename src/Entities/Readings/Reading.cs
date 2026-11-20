namespace Entities;

public abstract class Reading
{
    public int id { get; set; }

    public string device { get; set; }

    public string type { get; set; }

    public DateTime timeStamp { get; set; }
}

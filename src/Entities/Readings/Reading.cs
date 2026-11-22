using System.Text.Json.Serialization;

namespace Entities;

[JsonDerivedType(typeof(BooleanReading))]
[JsonDerivedType(typeof(TriggerReading))]
[JsonDerivedType(typeof(FloatReading))]
[JsonDerivedType(typeof(GeospatialReading))]
public abstract class Reading
{
    public int? id { get; set; }

    public required string device { get; set; }

    public required string kind { get; set; }

    public DateTime timeStamp { get; set; }
}

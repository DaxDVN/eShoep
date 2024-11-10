using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Shoep.Management.Enum;
[JsonConverter(typeof(StringEnumConverter), true)]
public enum OrderStatus
{
    Draft = 1,
    Pending = 2,
    Completed = 3,
    Cancelled = 4
}
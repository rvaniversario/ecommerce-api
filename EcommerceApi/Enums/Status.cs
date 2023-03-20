using System.Text.Json.Serialization;

namespace EcommerceApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        Pending = 1,
        Processed = 2,
        Cancelled = 3
    }
}

using Newtonsoft.Json;

namespace API.Models.DeviceInterface
{
    public class DevicePingRequestBody
    {

        [JsonProperty("device_id")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("device_version")]
        public string DeviceVersion { get; set; } = string.Empty;

        [JsonProperty("battery_level")]
        public int BatteryLevel { get; set; } = 0;

    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models.DeviceInterface
{
    public class DeviceDataRequestBody
    {

        [JsonProperty("device_id")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("device_version")]
        public string DeviceVersion { get; set; } = string.Empty;

        [JsonProperty("battery_level")]
        public int BatteryLevel { get; set; } = 0;

        [JsonProperty("latitude")]
        public double Latitude { get; set; } = 0.0;

        [JsonProperty("longitude")]
        public double Longitude { get; set; } = 0.0;

        [JsonProperty("iccid")]
        public string ICCID { get; set; } = "890000000000000000F";

    }
}

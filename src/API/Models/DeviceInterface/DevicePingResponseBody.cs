using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models.DeviceInterface
{
    public class DevicePingResponseBody
    {

        [JsonProperty("is_tracking")]
        public bool IsTracking { get; set; } = false;

        [JsonProperty("tracking_frequency")]
        public int TrackingFrequency { get; set; } = 120;

    }
}

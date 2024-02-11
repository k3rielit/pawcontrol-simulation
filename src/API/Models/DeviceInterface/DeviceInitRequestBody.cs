using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models.DeviceInterface
{
    public class DeviceInitRequestBody
    {

        [JsonProperty("device_id")]
        public string DeviceId { get; set; } = string.Empty;

        [JsonProperty("device_version")]
        public string DeviceVersion { get; set; } = string.Empty;

    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models.DeviceInterface
{
    public class DeviceInitResponseBody
    {

        [JsonProperty("auth_token")]
        public string AuthToken { get; set; } = string.Empty;

        [JsonProperty("is_update")]
        public bool IsUpdate { get; set; } = false;

        [JsonProperty("update_new_version")]
        public string UpdateNewVersion { get; set; } = string.Empty;

        [JsonProperty("update_url")]
        public string UpdateUrl { get; set; } = string.Empty;

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Endpoints.DeviceInterface
{
    public class DeviceDataEndpoint : BaseEndpoint
    {
        public DeviceDataEndpoint() : base()
        {
            Method = HttpMethod.Post;
            Route = "/api/device/data";
            AuthHeader = "auth_token";
            AuthTokenPrefix = "";
        }
    }
}

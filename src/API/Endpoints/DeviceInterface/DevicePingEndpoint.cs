namespace API.Endpoints.DeviceInterface
{
    public class DevicePingEndpoint : BaseEndpoint
    {
        public DevicePingEndpoint() : base()
        {
            Method = HttpMethod.Post;
            Route = "/api/device/ping";
            AuthHeader = "auth_token";
            AuthTokenPrefix = "";
        }
    }
}

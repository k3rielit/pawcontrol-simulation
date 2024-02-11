namespace API.Endpoints.DeviceInterface
{
    public class DeviceInitEndpoint : BaseEndpoint
    {
        public DeviceInitEndpoint() : base()
        {
            Method = HttpMethod.Post;
            Route = "/api/device/init";
            AuthHeader = "auth_token";
            AuthTokenPrefix = "";
        }
    }
}

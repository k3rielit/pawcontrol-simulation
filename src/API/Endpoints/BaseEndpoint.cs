namespace API.Endpoints
{
    public class BaseEndpoint : IEndpoint
    {
        public HttpMethod Method { get; set; }
        public Uri BaseAddress { get; set; }
        public string Route { get; set; }
        public string AuthHeader { get; set; }
        public string AuthTokenPrefix { get; set; }

        public Uri GetUri()
        {
            UriBuilder builder = new(BaseAddress)
            {
                Path = Route
            };
            return builder.Uri;
        }

        public BaseEndpoint()
        {
            Method = HttpMethod.Get;
            BaseAddress = new Uri("http://127.0.0.1:8000");
            Route = "";
            AuthHeader = "Authorization";
            AuthTokenPrefix = "Bearer ";
        }

    }
}

namespace API.Endpoints
{
    public interface IEndpoint
    {
        HttpMethod Method { get; set; }
        Uri BaseAddress { get; set; }
        string Route { get; set; }
        string AuthHeader { get; set; }
        string AuthTokenPrefix { get; set; }
        Uri GetUri();
    }
}

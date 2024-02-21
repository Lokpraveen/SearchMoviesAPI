namespace SearchMoviesAPI.Authentication
{
    using System.Net;

    public class LoginModel
    {
        public string? UserName { get; set; }    
        public string? Password { get; set; }    
    }

    public class LoginResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Response { get; set; }
    }
}

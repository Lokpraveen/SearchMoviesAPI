namespace SearchMoviesAPI.Model
{
    using System.Net;

    public class Movies
    {
        public int Id { get; set; } 
        public string? Name { get; set; }   
        public string? Director { get; set; }
        public string? Banner { get; set; }
        public int MovieRating { get; set; }
        public int year { get; set; }
    }

    public class MoviesResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Response { get; set; }
    }
}

namespace SearchMoviesAPI.Services
{
    using Microsoft.Extensions.Options;
    using SearchMoviesAPI.Interface;
    using SearchMoviesAPI.Model;

    public class MoviesService :IMoviesService
    {
        private readonly ILogger<MoviesService> _logger;
        private readonly IOptions<List<Movies>> _movies;

        public MoviesService(ILogger<MoviesService> logger,
                             IOptions<List<Movies>> movies)
        {
            _logger = logger;
            _movies = movies;
        }
        public Task<List<Movies>> GetMovies()
        { 
            var movies = new List<Movies>();
            try
            {
                _logger.LogInformation($"Get movies started...");
                movies = _movies.Value;
            }
            catch
            {
                _logger.LogError($"Get movies thorws excepttion...");
            }
            return Task.FromResult(movies);
            
        }

        public Task<List<Movies>>GetMoviesBySearch(string search)
        {
            var movies = new List<Movies>();
            try
            {
                _logger.LogInformation($"Search movies started with... {search}");
                movies = _movies.Value.FindAll(m => m.Name.ToLower().Contains(search)
                                                   || m.Banner.ToLower().Contains(search)
                                                   || m.Director.ToLower().Contains(search)
                                                   || Convert.ToString(m.year).Contains(search)
                                                   || Convert.ToString(m.MovieRating).Contains(search));
            }
            catch
            {
                _logger.LogError($"Get movies by search throws exception...");
            }
            return Task.FromResult(movies);
        }
    }
}

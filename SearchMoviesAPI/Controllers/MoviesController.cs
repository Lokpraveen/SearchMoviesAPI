using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SearchMoviesAPI.Authentication;
using SearchMoviesAPI.Interface;
using SearchMoviesAPI.Model;
using System.Net;

namespace SearchMoviesAPI.Controllers
{
    [Authorize(Roles = UserRoles.User)]
    [Route("[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        public readonly IMoviesService _moviesService;
        public MovieController(ILogger<MovieController> logger, IMoviesService moviesService)
        {
            _logger = logger;
            _moviesService = moviesService;
        }

        [HttpGet]
        [Route("GetMovies")]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("Get movies API calling.. ");
                var movies = await _moviesService.GetMovies();
                if (movies == null || movies?.Count() == 0)
                {
                    return NotFound(new MoviesResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Response = $"Movies not found",
                    });
                }
                _logger.LogInformation("Get movies API calling end.. ");
                return Ok(movies);
            }
            catch
            {
                _logger.LogError($"Get movies API thorws execption.. ");
                return StatusCode(StatusCodes.Status500InternalServerError,
             "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Route("SearchMovies")]
        public async Task<IActionResult> GetMoviesBySearch(string searchString)
        {
            try
            {
                _logger.LogInformation("Search movies API calling.. ");
                var movies = await _moviesService.GetMoviesBySearch(searchString.ToLower());
                _logger.LogInformation("Search movies API calling end.. ");
                if (movies == null || movies?.Count() == 0)
                {
                    return NotFound(new MoviesResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Response = $"Results not found for the search - {searchString}",
                    });
                }
                return Ok(movies);
            }
            catch
            {
                _logger.LogError($"Search movies API thorws execption.. ");
                return StatusCode(StatusCodes.Status500InternalServerError,
             "Error retrieving data from the database");
            }
        }
    }
}

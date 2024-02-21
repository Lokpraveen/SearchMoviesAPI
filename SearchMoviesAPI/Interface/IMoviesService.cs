using SearchMoviesAPI.Model;

namespace SearchMoviesAPI.Interface
{
    public interface IMoviesService
    {
       Task<List<Movies>> GetMovies();
       Task<List<Movies>>GetMoviesBySearch(string search);
    }
}

using System.Collections.Generic;

namespace fletnix.Models
{
    public interface IMovieRepsitory
         {
             IEnumerable<Movie> GetMoviesByName(string movieName, int movieId);
         }
}
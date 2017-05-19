using System.Collections.Generic;

namespace fletnix.Models
{
    public interface IMovieCastRepository
    {
        IEnumerable<MovieCast> getMovieCastById(int movieId);
    }
}
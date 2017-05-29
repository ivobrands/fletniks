using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace fletnix.Models
{
    public class MovieRepository: IMovieRepsitory
    {
        private fletnixContext _context;

        public MovieRepository(fletnixContext context)
        {
            _context = context;
        }

        public IEnumerable<Movie> GetMoviesByName(string movieName, int movieId)
        {
            return _context.Movie.AsNoTracking().Where(m => m.Title.Contains(movieName) && movieId != m.MovieId);
        }
    }
}
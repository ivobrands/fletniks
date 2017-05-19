using System.Collections.Generic;
using System.Linq;

namespace fletnix.Models
{
    public class MovieRepository: IMovieRepsitory
    {
        private fletnixContext _context;

        public MovieRepository(fletnixContext context)
        {
            _context = context;
        }

        public IEnumerable<Movie> GetMoviesByName(string movieName)
        {
            return _context.Movie.Where(m => m.Title.Contains(movieName));
        }
    }
}
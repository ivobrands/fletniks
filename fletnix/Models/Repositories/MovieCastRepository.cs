using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace fletnix.Models
{
    public class MovieCastRepository: IMovieCastRepository
    {
        private fletnixContext _context;

        public MovieCastRepository(fletnixContext context)
        {
            _context = context;
        }

        public IEnumerable<MovieCast> getMovieCastById(int movieId)
        {
            var cast = _context.MovieCast.Where(mc => mc.MovieId == movieId);
            return cast;
        }
    }
}
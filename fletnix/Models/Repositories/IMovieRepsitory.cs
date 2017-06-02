using System.Collections.Generic;
using System.Security.Claims;
using fletnix.ViewModels;

namespace fletnix.Models
{
    public interface IMovieRepsitory
         {
             IEnumerable<Movie> GetMoviesByName(string movieName, int movieId);
             bool UpdateGenre(List<MovieGenre> genres, int movieId);
             object GetPersonByName(string personName);
             bool AddDirector(MovieDirector newDirector);
             bool AddMovieCast(MovieCast movieCast);
             bool RemoveDirector(MovieDirector movieDirectorModel);
             bool RemoveMovieCast(MovieCast movieCast);
             bool AddMovieAward(MovieAward movieAward);
             bool RemoveMovieAward(MovieAward movieAward);
             bool AddToWatchHistory(Watchhistory watchhistory);
             Watchhistory HasSeenMovie(ClaimsPrincipal user, int movieId);
         }
}
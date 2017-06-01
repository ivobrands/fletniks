using System.Collections.Generic;
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
         }
}
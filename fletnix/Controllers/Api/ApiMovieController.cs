using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using fletnix.Models;
using Microsoft.AspNetCore.Mvc;
using fletnix.Models;
using fletnix.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace fletnix.Controllers.Api

{
    [Authorize]
    public class ApiMovieController: Controller
    {
        private IMovieRepsitory _repsitory;


        public ApiMovieController(IMovieRepsitory repository)
        {
            _repsitory = repository;
        }

        [HttpGet("api/movie/{movieName}/{movieId}")]
        public JsonResult Get(string movieName, int movieId)
        {
           movieName = Uri.UnescapeDataString(movieName);
           var movieList = _repsitory.GetMoviesByName(movieName, movieId);
            return Json(movieList);
        }
        
        [HttpGet("api/persons/{personName}")]
        public JsonResult Get(string personName)
        {
            personName = Uri.UnescapeDataString(personName);
            var personList = _repsitory.GetPersonByName(personName);
            return Json(personList);
        }

        [HttpPost("api/movie/genre/{movieId}")]
        public bool updateGenre([FromBody]genreModel genres, int movieId)
        {
            var movieGenreList = new List<MovieGenre>();
            foreach (var genre in genres.genres)
            {
                var genreModel = new MovieGenre {GenreName = genre, MovieId = movieId};
                movieGenreList.Add(genreModel);
            }
            return _repsitory.UpdateGenre(movieGenreList, movieId);
        }
        
        [HttpPost("/api/movie/movieDirector/{movieId}/{personId}")]
        public bool AddDirector(int movieId, int personId)
        {
            var director = new MovieDirector {MovieId = movieId, PersonId = personId};
            return _repsitory.AddDirector(director);
        }
        
        [HttpPost("/api/movie/movieCast/{movieId}/{personId}/{personRole}")]
        public bool AddMovieCast(int movieId, int personId, string personRole)
        {
            var movieCast = new MovieCast {MovieId = movieId, PersonId = personId, Role = personRole};
            return _repsitory.AddMovieCast(movieCast);
        }
        
        [HttpPost("/api/movie/movieDirector/remove")]
        public bool RemoveDirector([FromBody]MovieDirectorModel newDirectorModel)
        {
            var newDirector = new MovieDirector
            {
                MovieId = newDirectorModel.MovieId,
                PersonId = newDirectorModel.PersonId
            };
           
            return _repsitory.RemoveDirector(newDirector);
        }
        
        [HttpPost("/api/movie/movieCast/remove")]
        public bool RemoveMovieCast([FromBody]MovieCastModel newMovieCastModel)
        {
            var movieCast = new MovieCast {MovieId = newMovieCastModel.MovieId, PersonId = newMovieCastModel.PersonId, Role = newMovieCastModel.Role};
            return _repsitory.RemoveMovieCast(movieCast);
        }
       
        [HttpPost("/api/movie/movieAward/add")]
        public bool AddMovieAward([FromBody]MovieAwardModel movieAwardModel)
        {
            var movieAward = new MovieAward
            {
                MovieId = movieAwardModel.MovieId,
                PersonId = movieAwardModel.PersonId,
                Name = movieAwardModel.Name,
                Type = movieAwardModel.Type,
                Result = movieAwardModel.Result,
                Year = movieAwardModel.Year
            };
            return _repsitory.AddMovieAward(movieAward);
        }
        
        [HttpPost("/api/movie/movieAward/remove")]
        public bool RemoveMovieAward([FromBody]MovieAwardModel movieAwardModel)
        {
            var movieAward = new MovieAward
            {
                MovieId = movieAwardModel.MovieId,
                PersonId = movieAwardModel.PersonId,
                Name = movieAwardModel.Name,
                Type = movieAwardModel.Type,
                Result = movieAwardModel.Result,
                Year = movieAwardModel.Year
            };
            return _repsitory.RemoveMovieAward(movieAward);
        }
        
        [HttpPost("/api/movie/movieWatch/{movieId}/{price}")]
        public bool WatchMovie(int movieId, int price)
        {
            
            var hasSeen = _repsitory.HasSeenMovie(User, movieId);

            if (hasSeen != null && (!ModelState.IsValid || hasSeen.CustomerMailAddress == User.Identity.Name)) return false;
            
            _repsitory.AddToWatchHistory(new Watchhistory
            {
                CustomerMailAddress = User.Identity.Name,
                Invoiced = true,
                MovieId = movieId,
                Price = price,
                WatchDate = DateTime.Now
            });
            
            return true;
        }
    }
}
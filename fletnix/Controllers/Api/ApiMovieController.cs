using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using fletnix.Models;
using Microsoft.AspNetCore.Mvc;
using fletnix.Models;
using fletnix.ViewModels;
using Newtonsoft.Json;

namespace fletnix.Controllers.Api

{

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
        public bool addDirector(int movieId, int personId)
        {
            var director = new MovieDirector {MovieId = movieId, PersonId = personId};
            return _repsitory.AddDirector(director);
        }
    }
}
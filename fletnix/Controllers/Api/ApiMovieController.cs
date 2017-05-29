using System;
using System.Linq;
using fletnix.Models;
using Microsoft.AspNetCore.Mvc;
using fletnix.Models;

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

        [HttpPost("api/movie/genre/{movieId}")]
        public int Get(int movieId)
        {
            return movieId;
        }
    }
}
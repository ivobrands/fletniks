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

        [HttpGet("api/movie/{movieName}")]
        public JsonResult Get(string movieName)
        {
           movieName = Uri.UnescapeDataString(movieName);
            return Json(_repsitory.GetMoviesByName(movieName));
        }
    }
}
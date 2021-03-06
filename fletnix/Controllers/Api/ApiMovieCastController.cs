﻿using System;
using fletnix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fletnix.Controllers.Api

{
    [Authorize]
    public class ApiMovieCastController: Controller
    {
        private IMovieCastRepository _repsitory;


        public ApiMovieCastController(IMovieCastRepository repository)
        {
            _repsitory = repository;
        }

        [HttpGet("api/moviecast/{id}")]
        public JsonResult Get(int id)
        {
            return Json(_repsitory.getMovieCastById(id));
        }
    }

}
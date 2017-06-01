using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using fletnix.Models;
using IdentityModel;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fletnix.Controllers
{
    public class HomeController : Controller
    {
        private readonly fletnixContext _context;

        public HomeController(fletnixContext context) {
            _context = context;
        }

        public IActionResult Index()
        {
//
//            SELECT *, avg_rating/m.price as prIndex
//            FROM Movie m INNER JOIN(
//                SELECT TOP 10 c.movie_id, AVG(rating) as avg_rating
//            FROM CustomerFeedback c
//                GROUP BY c.movie_id
//                ORDER BY avg_rating DESC
//                ) as c on m.movie_id = c.movie_id
            
            
//            var movies = _context.Movie
//                .AsNoTracking()
//                .Join(
//                    _context.Watchhistory.Where(
//                            w => w.WatchDate >= Convert.ToDateTime(DateTime.Now).AddDays(-30000))
//                        .GroupBy(w => w.MovieId)
//                        .Select(m => new { amount = m.Count(), mID = m.Key })
//                        .OrderByDescending(a => a.amount)
//                        .Select(a => new { movieid = a.mID }),
//                    m => m.MovieId, w => w.movieid, (m, w) => m).Take(25);
//
//            //var movies = movies.ToList
//
//            ViewData["Movies"] = movies;
//
//            var watchHistory = _context.Movie
//                .AsNoTracking()
//                .Join(
//                    _context.Watchhistory.Where(
//                            w => w.CustomerMailAddress == User.Identity.Name)
//                        .GroupBy(w => w.MovieId)
//                        .Select(m => new { amount = m.Count(), mID = m.Key })
//                        .OrderByDescending(a => a.amount)
//                        .Select(a => new { movieid = a.mID }),
//                    m => m.MovieId, w => w.movieid, (m, w) => m).Take(25).ToList();;
//
//            ViewData["watchHistory"] = watchHistory;
//
//
//            var MostPopularMoviesOfLastNDays = _context.Watchhistory
//                .Where(w => w.WatchDate >= Convert.ToDateTime(DateTime.Now).AddDays(-14))
//                .Join(_context.Movie, w => w.MovieId, m => m.MovieId, (w, m) => new {w, m})
//                .AsNoTracking().Take(20);
//
//            ViewData["MostPopularMoviesOfLastNDays"] = MostPopularMoviesOfLastNDays;
            return View();
        }

        public IActionResult About()
        {
            Task Task1 = Task.Factory.StartNew(() =>
            {
                ViewData["Movies"] = _context.Movie
                    .AsNoTracking()
                    .Join(
                        _context.Watchhistory.Where(
                                w => w.WatchDate >= Convert.ToDateTime(DateTime.Now).AddDays(-30000))
                            .GroupBy(w => w.MovieId)
                            .Select(m => new {amount = m.Count(), mID = m.Key})
                            .OrderByDescending(a => a.amount)
                            .Select(a => new {movieid = a.mID}),
                        m => m.MovieId, w => w.movieid, (m, w) => m)
                    .Take(25)
                    .ToList();
            });


            Task Task2 = Task.Factory.StartNew(() =>
            {
                ViewData["watchHistory"] = _context.Movie
                    .AsNoTracking()
                    .Join(
                        _context.Watchhistory.Where(
                                w => w.CustomerMailAddress == User.Identity.Name)
                            .GroupBy(w => w.MovieId)
                            .Select(m => new { amount = m.Count(), mID = m.Key })
                            .OrderByDescending(a => a.amount)
                            .Select(a => new { movieid = a.mID }),
                        m => m.MovieId, w => w.movieid, (m, w) => m).Take(25).ToList();
            });

            Task Task3 = Task.Factory.StartNew(() =>
            {
                ViewData["MostPopularMoviesOfLastNDays"] = _context.Watchhistory
                    .Where(w => w.WatchDate >= Convert.ToDateTime(DateTime.Now).AddDays(-14))
                    .Join(_context.Movie, w => w.MovieId, m => m.MovieId, (w, m) => new {w, m})
                    .AsNoTracking().Take(20);
            });

            Task.WaitAll(Task1, Task2, Task3);

            return View();
        }

        [Authorize]
        public IActionResult Contact()
        {


            ViewData["Message"] = null;


            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

    }
}

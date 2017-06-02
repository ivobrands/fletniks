using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fletnix.Models;
using fletnix.Services;
using fletnix.ViewModels;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace fletnix
{
    public class CustomerMovieController : Controller
    {
        private readonly fletnixContext _context;

        public CustomerMovieController(fletnixContext context)
        {
            _context = context;
        }

        // GET: Movie
        //[Authorize(Policy = "customer")]
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            if (currentFilter.IsNullOrEmpty() && searchString.IsNullOrEmpty() && sortOrder.IsNullOrEmpty())
            {
                Task Task1 = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("start all time weeks");

                    using (var context = fletnixContext.ContextFactory())
                    {
                        ViewData["MostPopularOfAllTime"] = (
                            from w in context.Watchhistory
                            join m in context.Movie on w.MovieId equals m.MovieId
                            group m by new {m}
                            into g
                            orderby g.Count() descending
                            select new PopularMoviesViewModel
                            {
                                Movie = g.Key.m,
                                TimesViewed = g.Count()
                            }).Take(50).AsNoTracking().ToList();
                    }
                });


                Task Task2 = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("start last 2 weeks");
                    using (var context = fletnixContext.ContextFactory())
                    {
                        ViewData["MostPopularMoviesOfLastTwoWeeks"] = (from w in context.Watchhistory
                                where w.WatchDate >= Convert.ToDateTime(DateTime.Now).AddDays(-14)
                                join m in context.Movie on w.MovieId equals m.MovieId
                                group m by new {m}
                                into g
                                orderby g.Count() descending
                                select new PopularMoviesViewModel
                                {
                                    Movie = g.Key.m,
                                    TimesViewed = g.Count()
                                }).Take(15).AsNoTracking()
                            .ToList();
                    }
                });

                Task.WaitAll(Task1, Task2);
                return View();
            }


            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DurationSortParm"] = sortOrder == "duration" ? "duration_desc" : "duration";
            ViewData["PublicationYearSortParm"] = sortOrder == "publicationYear" ? "publicationYear_desc" : "publicationYear";

            ViewData["CurrentSort"] = sortOrder;

            var movie = from s in _context.Movie
                select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                movie = movie.Where(m => m.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    movie = movie.OrderByDescending(m => m.Title);
                    break;
                case "duration":
                    movie = movie.OrderBy(m => m.Duration);
                    break;
                case "duration_desc":
                    movie = movie.OrderByDescending(m => m.Duration);
                    break;
                case "publicationYear":
                    movie = movie.OrderBy(m => m.PublicationYear);
                    break;
                case "publicationYear_desc":
                    movie = movie.OrderByDescending(m => m.PublicationYear);
                    break;
                default:
                    movie = movie.OrderBy(m => m.Title);
                    break;
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

           
            int pageSize = 15;
            return View(await PaginatedList<Movie>.CreateAsync(movie.AsNoTracking(), page ?? 1, pageSize));
        }


        // GET: Movie/Details/5
        [Authorize(Policy = "customer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _context.Movie
                .Include(director => director.MovieDirector).ThenInclude(person => person.Person)
                .Include(cast => cast.MovieCast).ThenInclude(person => person.Person)
                .Include(award => award.MovieAward)
                .Include(genre => genre.MovieGenre).First(m => m.MovieId == id);

            movie.PreviousPartNavigation = _context.Movie.FirstOrDefault(m => movie.PreviousPart == m.MovieId);

            ViewData["AwardTypes"] = _context.AwardType.ToList();
            ViewData["Genres"] = _context.Genre.ToList();
            ViewData["AwardResults"] = new List<String>{"nominated","won"};
            @ViewData["title"] = movie.Title;

            return View(movie);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fletnix.Models;
using fletnix.Services;
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


            int pageSize = 25;
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

            var movie = await _context.Movie
                .Include(m => m.PreviousPartNavigation)
                .SingleOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
    }
}

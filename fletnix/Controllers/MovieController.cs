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
    public class MovieController : Controller
    {
        private readonly fletnixContext _context;

        public MovieController(fletnixContext context)
        {
            _context = context;
        }

        // GET: Movie
        [Authorize(Policy = "admin")]
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


            int pageSize = 15;
            return View(await PaginatedList<Movie>.CreateAsync(movie.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Movie/Details/5
        [Authorize(Policy = "admin")]
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

        // GET: Movie/Create
        [Authorize(Policy = "admin")]
        public IActionResult Create()
        {
            ViewData["PreviousPart"] = new SelectList(_context.Movie, "MovieId", "Title");
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Create([Bind("MovieId,Title,Duration,Description,PublicationYear,CoverImage,PreviousPart,Price,Url")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new {id = movie.MovieId});
            }
            ViewData["PreviousPart"] = new SelectList(_context.Movie, "MovieId", "Title", movie.PreviousPart);
            return View(movie);
        }

        // GET: Movie/Edit/5
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Edit(int? id)
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

             return View(movie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,Duration,Description,PublicationYear,CoverImage,PreviousPart,Price,Url")] Movie movie)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["PreviousPart"] = new SelectList(_context.Movie, "MovieId", "Title", movie.PreviousPart);
            return View(movie);
        }

        // GET: Movie/Delete/5
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.MovieId == id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.MovieId == id);
        }
    }
}

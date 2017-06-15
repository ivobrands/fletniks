using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using fletnix.Models;
using fletnix.Services;
using fletnix.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;


namespace fletnix.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private fletnixContext _context;

        public ReportController(fletnixContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        
        [Authorize(Policy = "financial")]
        public async Task<IActionResult> Awards(int? startYear, int? endYear,  int? page)
        {

            Dictionary<int, AwardReportViewModel> dict = new Dictionary<int, AwardReportViewModel>();
 
            object pagedData = null;
            if (startYear == null)
            {
                startYear = 2005;
            }
            if (endYear == null)
            {
                endYear = 2017;
            }
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction(
                IsolationLevel.ReadUncommitted))
            {
                
                var mAward = (from m in _context.Movie
                    where m.PublicationYear >= startYear && m.PublicationYear <= endYear
                    from ma in _context.MovieAward
                        .Where(ma => m.MovieId == ma.MovieId).DefaultIfEmpty()
                    group m by new {m, ma}
                    into g
                    select new AwardReportViewModel
                    {
                        Movie = g.Key.m,
                        MovieAward = g.Key.ma
                    }).OrderByDescending(m => m.Movie.PublicationYear);
                pagedData = await PaginatedList<AwardReportViewModel>.CreateAsync(mAward.AsNoTracking(), page ?? 1, 15);
                transaction.Commit(); 
            }
            
            foreach(var movie in (List<AwardReportViewModel>)pagedData)
            {
                if (!dict.ContainsKey(movie.Movie.MovieId))
                {
                    dict.Add(movie.Movie.MovieId, movie);
                }
                
                if (movie.MovieAward != null)
                {
                    dict[movie.Movie.MovieId].MovieAwardList.Add(movie.MovieAward);
                    if (movie.MovieAward.Result == "Won" || movie.MovieAward.Result == "won")
                    {
                        dict[movie.Movie.MovieId].WonCount++;
                    }
                    else
                    {
                        dict[movie.Movie.MovieId].NominationCount++; 
                    }
                }
            }

            Console.WriteLine(dict.Count);
            ViewData["dict"] = dict;
            ViewData["report"] = pagedData;
            ViewData["start"] = startYear;
            ViewData["end"] = endYear;
            return View(pagedData);
        }

        [Authorize(Policy = "CEO")]
        public async Task<IActionResult> Rating(string filter)
        {
            List<RatingPriceIndexViewModel> report = null;
            ViewData["filter"] = filter;
                switch(filter)
                {
                    case "highestAverage":
                        using (IDbContextTransaction transaction = _context.Database.BeginTransaction(
                            IsolationLevel.ReadUncommitted))
                        {
                            var result = (from movie in _context.Movie
                                join customerFeedback in _context.CustomerFeedback on movie.MovieId equals customerFeedback
                                    .MovieId
                                group customerFeedback by new {T1 = movie}
                                into g
                                orderby g.Average(t2 => t2.Rating) descending
                                select new RatingPriceIndexViewModel
                                {
                                    Movie = g.Key.T1,
                                    AverageRating = g.Average(t2 => t2.Rating),
                                    RpRating = g.Average(t2 => t2.Rating) / (double) g.Key.T1.Price
                                }).OrderByDescending(m => m.AverageRating).Take(10).ToList();
                            transaction.Commit();
                            report = result;
                        }
                        break;
                    case "lowestAverage":
                        using (IDbContextTransaction transaction = _context.Database.BeginTransaction(
                            IsolationLevel.ReadUncommitted))
                        {
                            var result = (from movie in _context.Movie
                                join customerFeedback in _context.CustomerFeedback on movie.MovieId equals
                                customerFeedback.MovieId
                                group customerFeedback by new {T1 = movie}
                                into g
                                orderby g.Average(t2 => t2.Rating) descending
                                select new RatingPriceIndexViewModel
                                {
                                    Movie = g.Key.T1,
                                    AverageRating = g.Average(t2 => t2.Rating),
                                    RpRating = g.Average(t2 => t2.Rating) / (double) g.Key.T1.Price
                                }).OrderBy(m => m.AverageRating).Take(10).ToList();
                            transaction.Commit();
                            report = result;
                        }
                        break;
                    case "highestAveragePriceIndex":
                        using (IDbContextTransaction transaction = _context.Database.BeginTransaction(
                            IsolationLevel.ReadUncommitted))
                        {
                            var result = (from movie in _context.Movie
                                join customerFeedback in _context.CustomerFeedback on movie.MovieId equals customerFeedback.MovieId
                                group customerFeedback by new {T1 = movie}
                                into g
                                orderby g.Average(t2 => t2.Rating) descending
                                select new RatingPriceIndexViewModel
                                {
                                    Movie = g.Key.T1,
                                    AverageRating = g.Average(t2 => t2.Rating),
                                    RpRating = g.Average(t2 => t2.Rating) / (double) g.Key.T1.Price
                                }).OrderByDescending(m => m.RpRating).Take(10).ToList();
                            transaction.Commit();
                            report = result;
                        }
                        break;
                    case "lowestAveragePriceIndex":
                        using (IDbContextTransaction transaction = _context.Database.BeginTransaction(
                            IsolationLevel.ReadUncommitted))
                        {
                            var result = (from movie in _context.Movie
                                join customerFeedback in _context.CustomerFeedback on movie.MovieId equals customerFeedback.MovieId
                                group customerFeedback by new {T1 = movie}
                                into g
                                orderby g.Average(t2 => t2.Rating) descending
                                select new RatingPriceIndexViewModel()
                                {
                                    Movie = g.Key.T1,
                                    AverageRating = g.Average(t2 => t2.Rating),
                                    RpRating = g.Average(t2 => t2.Rating) / (double) g.Key.T1.Price
                                }).OrderBy(m => m.RpRating).Take(10).ToList();
                            transaction.Commit();
                            report = result;
                        }
                        break;
                    default:
                        using (IDbContextTransaction transaction = _context.Database.BeginTransaction(
                            IsolationLevel.ReadUncommitted))
                        {
                            var result = (from movie in _context.Movie
                                join customerFeedback in _context.CustomerFeedback on movie.MovieId equals customerFeedback
                                    .MovieId
                                group customerFeedback by new {T1 = movie}
                                into g
                                orderby g.Average(t2 => t2.Rating) descending
                                select new RatingPriceIndexViewModel
                                {
                                    Movie = g.Key.T1,
                                    AverageRating = g.Average(t2 => t2.Rating),
                                    RpRating = g.Average(t2 => t2.Rating) / (double) g.Key.T1.Price
                                }).OrderByDescending(m => m.AverageRating).Take(10).ToList();
                            transaction.Commit();
                            report = result;
                        }
                        break;
                }

                return View(report);
        }
    }
}

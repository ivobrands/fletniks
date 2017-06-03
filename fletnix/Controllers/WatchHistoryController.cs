using System.Linq;
using System.Threading.Tasks;
using fletnix.Models;
using fletnix.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace fletnix.Controllers
{
    [Authorize(Policy = "customer")]
    public class WatchHistoryController : Controller
    {
        private fletnixContext _context;
        
        public WatchHistoryController(fletnixContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var email = User.Identity.Name;
            var watchHistoryUser = (from w in _context.Watchhistory
                    where w.CustomerMailAddress == email
                    from comment in _context.CustomerFeedback
                        .Where(r => r.MovieId == w.MovieId).Where(r=>r.CustomerMailAddress == email).DefaultIfEmpty()
                    join m in _context.Movie on w.MovieId equals m.MovieId
                    group m by new {m,w,Comment = comment}
                    into g
                    select new PopularMoviesViewModel
                    {
                        Movie = g.Key.m,
                        WatchDate = g.Key.w.WatchDate,
                        Review = g.Key.Comment
                    }).AsNoTracking()
                .Take(50).OrderByDescending(p => p.WatchDate).ToList();

            return View(watchHistoryUser);
        }
    }
}

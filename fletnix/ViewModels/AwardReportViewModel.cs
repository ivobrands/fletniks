using System.Collections.Generic;
using fletnix.Models;

namespace fletnix.ViewModels
{
    public class AwardReportViewModel
    {
        public Movie Movie { get; set; }
        public MovieAward MovieAward { get; set; }
        public List<MovieAward> MovieAwardList { get; set; } = new List<MovieAward>();
        public int WonCount { get; set; } = 0;
        public int NominationCount { get; set; } = 0;
    }
}
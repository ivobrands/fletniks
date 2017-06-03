using System;

namespace fletnix.Models
{
    public class CustomerFeedback
    {
        public int MovieId { get; set; }
        public string CustomerMailAddress { get; set; }
        public DateTime FeedbackDate { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
       
        public virtual Customer CustomerMailAddressNavigation { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
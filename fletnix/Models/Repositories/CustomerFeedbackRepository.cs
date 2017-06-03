using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace fletnix.Models
{
    public class CustomerFeedbackRepository: ICustomerFeedbackRepository
    {
        private fletnixContext _context;

        public CustomerFeedbackRepository(fletnixContext context)
        {
            _context = context;
        }

        public bool SaveCustomerFeedback(CustomerFeedback customerFeedback)
        {
            _context.CustomerFeedback.Add(customerFeedback);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteCustomerFeedback(CustomerFeedback customerFeedback)
        {
            _context.CustomerFeedback.Remove(customerFeedback);
            _context.SaveChanges();
            return true;
        }

        public bool EditCustomerFeedback(CustomerFeedback customerFeedback)
        {
            _context.CustomerFeedback.Update(customerFeedback);
            _context.SaveChanges();
            return true;
        }
    }
}
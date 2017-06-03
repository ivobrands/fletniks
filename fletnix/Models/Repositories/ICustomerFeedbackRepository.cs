namespace fletnix.Models
{
    public interface ICustomerFeedbackRepository
    {
        bool SaveCustomerFeedback(CustomerFeedback customerFeedback);
        bool DeleteCustomerFeedback(CustomerFeedback customerFeedback);
        bool EditCustomerFeedback(CustomerFeedback customerFeedback);
    }
}
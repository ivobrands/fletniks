using System.ComponentModel.DataAnnotations;

namespace fletnix.ViewModels
{
    public class CustomerFeedbackModel
    {
        [Required] public int MovieId;
        [Required] public string Comment;
        [Required] public int Rating;
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fletnix.ViewModels
{
    public class MovieDirectorModel
    {
        [Required] public int MovieId;
        [Required] public int PersonId;
    }
}
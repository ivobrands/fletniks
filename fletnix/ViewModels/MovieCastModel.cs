using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fletnix.ViewModels
{
    public class MovieCastModel
    {
        [Required] public int MovieId;
        [Required] public int PersonId;
        [Required] public string Role;
    }
}
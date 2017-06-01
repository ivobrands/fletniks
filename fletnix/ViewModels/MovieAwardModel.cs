using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fletnix.ViewModels
{
    public class MovieAwardModel
    {
        [Required] public int MovieId;
        [Required] public int PersonId;
        [Required] public string Type;
        [Required] public string Name;
        [Required] public string Result;
        [Required] public int Year;
    }
}
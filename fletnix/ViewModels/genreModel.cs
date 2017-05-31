using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fletnix.ViewModels
{
    public class genreModel
    {
        public genreModel()
        {
            genres = new List<string>();
        }

        [Required] public List<string> genres;
    }
}
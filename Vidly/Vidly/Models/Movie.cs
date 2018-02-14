using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [Display(Name ="Release Date")]
        public DateTime? ReleaseDate { get; set; }

        [Display(Name = "Date Added")]
        public DateTime? DateAdded { get; set; }
        [Required]
        [Display(Name ="Number in Stock")]
        [Range(1,20)]
        public int Stock { get; set; }
        public Genre Genres { get; set; }
        [Required]
        [Display(Name ="Genre")]
        public int GenreId { get; set; }

        public byte NumberAvailable { get; set; }

    }

    // /movies/random => lets use these properties
}
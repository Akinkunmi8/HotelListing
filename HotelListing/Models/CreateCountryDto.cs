using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Models
{
    public class CreateCountryDto
    {

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Is too Long")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 2, ErrorMessage = "Is too Long")]
        public string ShortName { get; set; }

    }
}

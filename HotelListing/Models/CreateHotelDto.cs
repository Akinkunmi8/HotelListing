using HotelListing.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Models
{
    public class CreateHotelDto
    {
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Is too long")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 200, ErrorMessage = "Is too long")]
        public string Address { get; set; }
        [Required]
        [Range(1,5)]
        public double Rating { get; set; }
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Models
{
    public class CountryDto : CreateCountryDto
    {
        public int Id { get; set; }
        public virtual IList<HotelDto> HotelDtos { get; set; }
    }
}

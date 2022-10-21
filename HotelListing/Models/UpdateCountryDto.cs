using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Models
{
    public class UpdateCountryDto : CreateCountryDto
    {
        public IList<HotelDto> Hotels { get; set; } 
    }
}

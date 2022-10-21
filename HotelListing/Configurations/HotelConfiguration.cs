using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(

                new Hotel
                {
                    Id = 1,
                    Name = "Eko hotel",
                    Address = "Nigeria",
                    CountryId = 2,
                    Rating = 5
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Oriental hotel",
                    Address = "Nigeria",
                    CountryId = 2,
                    Rating = 5
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Grand palldium",
                    Address = "Jamaical",
                    CountryId = 1,
                    Rating = 5
                },
                new Hotel
                {
                    Id = 4,
                    Name = "Sharaton Hotel",
                    Address = "Nigeria",
                    CountryId = 2,
                    Rating = 4
                },
                new Hotel
                {
                    Id = 5,
                    Name = "Accra hotel",
                    Address = "Ghana",
                    CountryId = 3,
                    Rating = 4.5
                },
                new Hotel
                {
                    Id = 6,
                    Name = "Kumashi hotel",
                    Address = "Ghana",
                    CountryId = 3,
                    Rating = 4
                },
                new Hotel
                {
                    Id = 7,
                    Name = "Sandals Resort and Spa",
                    Address = "Jamaica ",
                    CountryId = 1,
                    Rating = 5
                }

             );
        }
    }
}

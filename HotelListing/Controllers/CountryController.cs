using AutoMapper;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();
                var results = _mapper.Map<IList<CountryDto>>(countries);
                return Ok(results);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,$"Something Went wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Internal server error.. please try again later");
            }
        }

        // get country by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWork.Countries.Get(opt => opt.Id == id, new List<string>{"Hotels" });
                var result = _mapper.Map<CountryDto>(country);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Somthing went wrong in the{nameof(GetCountry)}");
                return StatusCode(500, "Internal Server Error.... please try again later");

            }
        }
    }
}
 
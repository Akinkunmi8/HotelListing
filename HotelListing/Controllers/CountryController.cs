using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries([FromQuery] RequestParams requestParams)
        {

            var countries = await _unitOfWork.Countries.GetPageList(requestParams);
            var results = _mapper.Map<IList<CountryDto>>(countries);
            return Ok(results);

        }
        // get country by id
        [Authorize]
        [HttpGet("{id}", Name = "GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {


            var country = await _unitOfWork.Countries.Get(opt => opt.Id == id, new List<string> { "Hotels" });
            var result = _mapper.Map<CountryDto>(country);
            return Ok(result);

        }
        // create country
        [Authorize(Roles = "Admin")]
        [HttpPost("create country")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto countryDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }


            var country = _mapper.Map<Country>(countryDto);
            await _unitOfWork.Countries.Insert(country);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetCountry", new { id = country.Id }, country);


        }

        // update country
        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDto countryDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid Update Attempt of {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }

            var country = await _unitOfWork.Countries.Get(q => q.Id == id);
            if (country == null)
            {
                _logger.LogError($"Invalid Update Attempt of {nameof(UpdateCountry)}");
                return BadRequest("Sumitted Data is Invalid");
            }
            _mapper.Map<CountryDto>(countryDto);
            _unitOfWork.Countries.Update(country);
            await _unitOfWork.Save();

            return NoContent();
        }
        // delete country
        [HttpDelete("{int: id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalide Delete Attempt of {nameof(Delete)}");
                return BadRequest();
            }

            var country = _unitOfWork.Countries.Get(q => q.Id == id);
            if (country == null)
            {
                _logger.LogError($"Invalide Delete Attempt of {nameof(Delete)}");
                return BadRequest("Submitted data is invalid");
            }
            await _unitOfWork.Countries.Delete(id);
            await _unitOfWork.Save();

            return NoContent();

        }
    }
}

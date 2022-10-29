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
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HotelController> _logger;
        private readonly IMapper _mapper;

        public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        // to get all the hotels
        [HttpGet("GetAllHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetHotels()
        {

            var hotels = await _unitOfWork.Hotels.GetAll();
            var results = _mapper.Map<IList<HotelDto>>(hotels);
            return Ok(results);
        }
        // to get particular hotel in a country
        [Authorize]
        [HttpGet("{id}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(opt => opt.Id == id);
                var result = _mapper.Map<HotelDto>(hotel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"somthing went wrong in {nameof(GetHotel)}");
                return StatusCode(500, "interner server error.. please try again later");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create hotel")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDto hotelDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            var hotel = _mapper.Map<Hotel>(hotelDto);
            await _unitOfWork.Hotels.Insert(hotel);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetHotel", new { id = hotel.Id }, hotel);

        }
        //update hotel

        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDto hotelDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }

            var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
            if (hotel == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
                return BadRequest("Submitted data is invalid");
            }

            _mapper.Map(hotelDto, hotel);
            _unitOfWork.Hotels.Update(hotel);
            await _unitOfWork.Save();

            return NoContent();

        }
        // delete hotel
        [Authorize]
        [HttpDelete("{id: int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE Attempt in {nameof(DeleteHotel)}");
                return BadRequest();
            }

            var hotel = await _unitOfWork.Hotels.Get(opt => opt.Id == id);
            if (hotel == null)
            {
                _logger.LogError($"Invalid DELETE Attempt in {nameof(DeleteHotel)}");
                return BadRequest("Submitted data is invalid");
            }
            await _unitOfWork.Hotels.Delete(id);
            await _unitOfWork.Save();

            return NoContent();

        }


    }
}

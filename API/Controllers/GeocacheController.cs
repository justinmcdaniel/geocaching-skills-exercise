using API.Contracts.Request;
using API.Contracts.Response;
using API.Data.Interfaces;
using API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("geocaches")]
    [ApiController]
    public class GeocacheController : ControllerBase
    {
        private readonly IGeocacheRepository _geocacheRepository;

        public GeocacheController(IGeocacheRepository geocacheRepository)
        {
            _geocacheRepository = geocacheRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<GeocacheResponseDTO>> Get()
        {
            var geocaches = await _geocacheRepository.ListAsync();
            
            return geocaches.Select(gc =>
            {
                return new GeocacheResponseDTO(gc);
            });
        }

        [HttpPost("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(Geocache), 201)]
        public async Task<IActionResult> Create([FromBody] GeocacheCreateRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponseDTO("The model is not valid.", ModelState));
            }

            Geocache newGeocache = new()
            {
                Name = model.Name,
                Latitude = model.Latitude,
                Longitude = model.Longitude
            };
            await _geocacheRepository.AddAsync(newGeocache);

            return CreatedAtAction(nameof(Get), new { id = newGeocache.ID }, new GeocacheResponseDTO(newGeocache));
        }

    }
}

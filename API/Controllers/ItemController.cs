using API.Contracts.Request;
using API.Contracts.Response;
using API.Data.Interfaces;
using API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace API.Controllers
{
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IGeocacheRepository _geocacheRepository;
        private readonly IItemRepository _itemRepository;

        public ItemController(IGeocacheRepository geocacheRepository, IItemRepository itemRepository)
        {
            _geocacheRepository = geocacheRepository;
            _itemRepository = itemRepository;
        }

        [HttpGet("geocaches/{geocacheID}/items")]
        public async Task<IEnumerable<ItemResponseDTO>> GetGeocacheItems(int geocacheID)
        {
            var items = await _itemRepository.ListAsync(geocacheID);

            return items.Select(item =>
            {
                return new ItemResponseDTO(item);
            });
        }

        [HttpGet("items/{itemID}")]
        public async Task<IActionResult> Get(int itemID)
        {
            var item = await _itemRepository.FindAsync(itemID);

            if (item == default(Item))
            {
                return NotFound(new ErrorResponseDTO($"No {nameof(Item)} with ID '{itemID}' was found."));
            }

            return new JsonResult(new ItemResponseDTO(item));
        }

        [HttpPost("geocaches/{geocacheID}/items/create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(Geocache), 201)]
        public async Task<IActionResult> Create(int geocacheID, [FromBody] ItemCreateRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponseDTO("The model is not valid.", ModelState));
            }

            Geocache owner = await _geocacheRepository.FindAsync(geocacheID);
            if (owner == default(Geocache))
            {
                return NotFound(new ErrorResponseDTO($"A {nameof(Geocache)} with ID '{geocacheID}' was not found."));
            }

            Item newItem = new()
            {
                Name = model.Name,
                ActiveStartDate = model.ActiveStartDate,
                ActiveEndDate = model.ActiveEndDate,
                OwnerGeocacheID = owner.ID,
                Owner = owner
            };
            await _itemRepository.CreateAsync(newItem);

            return CreatedAtAction(nameof(Get), new { itemID = newItem.ID,  }, new ItemResponseDTO(newItem));
        }

        [HttpPut("geocaches/{targetGeocacheID}/items/{itemID}/move")]
        [ProducesResponseType(422)]
        [ProducesResponseType(typeof(Geocache), 204)]
        public async Task<IActionResult> MoveItem(int targetGeocacheID, int itemID)
        {
            Geocache newOwner = await _geocacheRepository.FindAsync(targetGeocacheID);
            if (newOwner == default(Geocache))
            {
                return NotFound(new ErrorResponseDTO($"A {nameof(Geocache)} with ID '{targetGeocacheID}' was not found."));
            }

            if (newOwner.CacheItems?.FirstOrDefault(i => i.ID == itemID) != default(Item))
            {
                // Just return success if the target cache already includes the item
                return NoContent();
            }

            if (newOwner.CacheItems.Count() >= 3)
            {
                return UnprocessableEntity(new ErrorResponseDTO($"The specified {nameof(Geocache)} has too many items. Your request cannot be completed."));
            }

            Item targetItem = await _itemRepository.FindAsync(itemID);
            if (targetItem == default(Item))
            {
                return NotFound(new ErrorResponseDTO($"An {nameof(Item)} with ID '{itemID}' was not found."));
            }

            if (targetItem.IsActive == false)
            {
                return UnprocessableEntity(new ErrorResponseDTO($"{nameof(Item)} with ID '{itemID}' is not active and cannot be moved."));
            }

            targetItem.Owner = newOwner;
            targetItem.OwnerGeocacheID = newOwner.ID;
            await _itemRepository.UpdateAsync(targetItem);

            return NoContent();
        }

    }
}

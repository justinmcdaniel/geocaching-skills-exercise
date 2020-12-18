using API.Data.Models;
using System.Linq;
using System.Collections.Generic;

namespace API.Contracts.Response
{
    public record GeocacheResponseDTO
    {
        public int GeocacheID { get; init; }
        public string Name { get; init; }
        public decimal Latitude { get; init; }
        public decimal Longitude { get; init; }
        public List<ItemResponseDTO> Items { get; init; }

        public GeocacheResponseDTO() { }

        // The "includeItems" parameter was added to take advantage of the JSON Serializer's option
        // to ignore nulls. This combination allows for a slightly cleaner JSON output without
        // having to create unique classes when serialzing from Geocache -> List<Items> and Item -> Owner
        public GeocacheResponseDTO(Geocache geocache, bool includeItems = true)
        {
            this.GeocacheID = geocache.ID;
            this.Name = geocache.Name;
            this.Latitude = geocache.Latitude;
            this.Longitude = geocache.Longitude;

            if (includeItems)
            {
                this.Items = new();
                if (geocache.CacheItems != null && geocache.CacheItems.Count > 0)
                {
                    this.Items.AddRange(geocache.CacheItems.Select(i => new ItemResponseDTO(i, false)));
                }
            }
        }
    }
}

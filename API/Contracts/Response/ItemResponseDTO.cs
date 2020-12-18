using API.Data.Models;
using System;

namespace API.Contracts.Response
{
    public record ItemResponseDTO
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public DateTimeOffset ActiveStartDate { get; set; }
        public DateTimeOffset? ActiveEndDate { get; set; }
        public GeocacheResponseDTO Owner { get; set; }

        public ItemResponseDTO() { }

        // The "includeItems" parameter was added to take advantage of the JSON Serializer's option
        // to ignore nulls. This combination allows for a slightly cleaner JSON output without
        // having to create unique classes when serialzing from Geocache -> List<Items> and Item -> Owner
        public ItemResponseDTO(Item item, bool includeOwner = true)
        {
            this.ItemID = item.ID;
            this.Name = item.Name;
            this.ActiveStartDate = item.ActiveStartDate;
            this.ActiveEndDate = item.ActiveEndDate;

            if (includeOwner)
            {
                this.Owner = new GeocacheResponseDTO(item.Owner, false);
            }
        }
    }
}

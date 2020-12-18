using System;

namespace API.Contracts.Request
{
    public class ItemCreateRequestDTO
    {
        public string Name { get; set; }
        public DateTimeOffset ActiveStartDate { get; set; }
        public DateTimeOffset? ActiveEndDate { get; set; }
    }
}

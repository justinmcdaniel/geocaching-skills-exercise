using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    public class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, DefaultValue(true)]

        public DateTimeOffset ActiveStartDate { get; set; } = DateTimeOffset.UtcNow - DateTimeOffset.UtcNow.TimeOfDay;
        public DateTimeOffset? ActiveEndDate { get; set; }

        public bool IsActive {
            get
            {
                // This calculation does not take the client's time into consideration.

                var now = DateTimeOffset.UtcNow;

                if (ActiveStartDate > now) return false;
                if (ActiveEndDate is null) return true;

                return now >= ActiveStartDate && now <= ActiveEndDate;
            } 
        }

        public int? OwnerGeocacheID { get; set; }

        [ForeignKey("OwnerGeocacheID")]
        public Geocache Owner { get; set; }
    }
}

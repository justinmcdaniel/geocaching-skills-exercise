using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Models
{
    public class Geocache
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, Column(TypeName = "decimal(9,6)")]
        public decimal Latitude { get; set; }
        [Required, Column(TypeName = "decimal(9,6)")]
        public decimal Longitude { get; set; }

        [InverseProperty("Owner")]
        public ICollection<Item> CacheItems { get; set; }
    }
}

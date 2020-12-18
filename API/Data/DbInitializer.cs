using API.Data.Models;
using System.Linq;

namespace API.Data
{
    public class DbInitializer
    {
        public static void Initialize(GeocacheDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Geocaches.Any())
            {
                return;   // DB has been seeded
            }

            Geocache[] geocaches = new[] {
                new Geocache { Name = "Cache 1", Latitude = 10, Longitude = 20 },
                new Geocache { Name = "Cache 2", Latitude = 11, Longitude = 21 },
                new Geocache { Name = "Cache 3", Latitude = 12, Longitude = 22 }
            };

            context.Geocaches.AddRange(geocaches);
            context.SaveChanges();

            Item[] cacheItems = new[] {
                new Item { Name = "Item A", OwnerGeocacheID = 1},
                new Item { Name = "Item B", OwnerGeocacheID = 2 },
                new Item { Name = "Item C", OwnerGeocacheID = 3 },
                new Item { Name = "Item D", OwnerGeocacheID = 2 },
                new Item { Name = "Item E", OwnerGeocacheID = 1 },
                new Item { Name = "Item F", OwnerGeocacheID = 1 },
            };

            context.Items.AddRange(cacheItems);
            context.SaveChanges();
        }
    }
}

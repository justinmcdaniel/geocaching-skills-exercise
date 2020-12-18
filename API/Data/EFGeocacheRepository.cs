using API.Data.Interfaces;
using API.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace API.Data
{
    public class EFGeocacheRepository : IGeocacheRepository
    {
        private readonly GeocacheDbContext _dbContext;

        public EFGeocacheRepository(GeocacheDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Geocache>> ListAsync()
        {
            var result = await _dbContext.Geocaches
                .Include(gc => gc.CacheItems)
                .ToListAsync();

            result.ForEach(gc => _removeInactiveItems(gc));

            return result;
        }

        public async Task<bool> AddAsync(Geocache geocache)
        {
            _dbContext.Geocaches.Add(geocache);
            int writtenEntryCount = await _dbContext.SaveChangesAsync();
            return writtenEntryCount > 0;
        }

        public async Task<Geocache> FindAsync(int geocacheID)
        {
            var result = await _dbContext.Geocaches
                .Include(gc => gc.CacheItems)
                .FirstOrDefaultAsync(gc => gc.ID == geocacheID);

            _removeInactiveItems(result);

            return result;
        }

        private void _removeInactiveItems(Geocache geocache)
        {
            // Embedding this filter into Geocache fetch via EF seems challenging. I saw some fancy solutions
            // online, but they looked like overkill for this case. Should additional optimization be needed
            // in a production setting, barring a good generic EF solution is truly not available, there is
            // always the option to write custom T-SQL.
            //
            // For this coding exercise, there were so few items that a geocache would have, I chose not to seek
            // additional optimization and simply remove the inactive items locally.
            geocache.CacheItems = geocache.CacheItems.Where(i => i.IsActive).ToList();
        }
    }
}

using API.Data.Interfaces;
using API.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Interfaces
{
    public class EFItemRepository : IItemRepository
    {
        private readonly GeocacheDbContext _dbContext;

        public EFItemRepository(GeocacheDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Item>> ListAsync(int geocacheID)
        {
            return _dbContext.Items
                .Include(item => item.Owner)
                .Where(item => 
                    item.OwnerGeocacheID == geocacheID
                    && item.IsActive
                )
                .ToListAsync();
        }

        public async Task<bool> CreateAsync(Item item)
        {
            _dbContext.Items.Add(item);
            int writtenEntryCount = await _dbContext.SaveChangesAsync();
            return writtenEntryCount > 0;
        }

        public Task<Item> FindAsync(int itemID)
        {
            return _dbContext.Items
                .Include(item => item.Owner)
                .FirstOrDefaultAsync(item => item.ID == itemID);
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            var existingItem = await _dbContext.Items
                .FirstOrDefaultAsync(item => item.Name == name);

            return existingItem != default(Item);
        }

        public async Task<bool> UpdateAsync(Item item)
        {
            _dbContext.Attach(item);
            int rowsAffected = await _dbContext.SaveChangesAsync();

            return rowsAffected > 0;
        }
    }
}

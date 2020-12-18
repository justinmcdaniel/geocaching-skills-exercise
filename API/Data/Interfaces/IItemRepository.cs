using API.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Data.Interfaces
{
    public interface IItemRepository
    {
        public Task<List<Item>> ListAsync(int geocacheID);
        public Task<bool> CreateAsync(Item item);
        public Task<Item> FindAsync(int itemID);
        public Task<bool> NameExistsAsync(string Name);
        public Task<bool> UpdateAsync(Item item);
    }
}

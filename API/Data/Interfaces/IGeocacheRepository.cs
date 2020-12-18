using API.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Data.Interfaces
{
    public interface IGeocacheRepository
    {
        public Task<List<Geocache>> ListAsync();
        public Task<bool> AddAsync(Geocache geocache);

        public Task<Geocache> FindAsync(int geocacheID);
    }
}

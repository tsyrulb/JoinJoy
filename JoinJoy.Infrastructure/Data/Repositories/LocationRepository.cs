using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JoinJoy.Infrastructure.Data.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        private readonly ApplicationDbContext _context;

        public LocationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsLocationReferencedAsync(int locationId)
        {
            return await _context.Activities.AnyAsync(a => a.LocationId == locationId);
        }
    }
}

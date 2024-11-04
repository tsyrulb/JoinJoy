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

        public async Task<Location> AddOrUpdateAsync(Location location)
        {
            // Check if a location with the same address and coordinates already exists
            var existingLocation = await _context.Locations.FirstOrDefaultAsync(l =>
                l.Address == location.Address &&
                l.Latitude == location.Latitude &&
                l.Longitude == location.Longitude);

            if (existingLocation != null)
            {
                // If a matching location is found, return it without adding a new entry
                return existingLocation;
            }

            // Otherwise, add the new location entry to the database
            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();

            return location; // Return the newly added location
        }

    }
}

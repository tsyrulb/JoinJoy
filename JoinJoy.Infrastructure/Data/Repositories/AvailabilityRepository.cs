using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Data.Repositories
{
    public class AvailabilityRepository : Repository<Availability>, IAvailabilityRepository
    {
        public AvailabilityRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

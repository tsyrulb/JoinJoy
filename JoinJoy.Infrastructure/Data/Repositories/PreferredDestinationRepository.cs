using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Data.Repositories
{
    public class PreferredDestinationRepository : Repository<PreferredDestination>, IPreferredDestinationRepository
    {
        public PreferredDestinationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

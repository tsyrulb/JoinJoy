using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Data.Repositories
{
    public class ActivityPreferenceRepository : Repository<ActivityPreference>, IActivityPreferenceRepository
    {
        public ActivityPreferenceRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Data.Repositories
{
    public class HobbyRepository : Repository<Hobby>, IHobbyRepository
    {
        public HobbyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Services
{
    public interface IGISService
    {
        Task<Location> GetCoordinatesAsync(string address);
    }
}

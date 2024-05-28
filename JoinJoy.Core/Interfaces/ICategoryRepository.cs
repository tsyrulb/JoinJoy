using JoinJoy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task DeleteAsync(Category category);
    }
}

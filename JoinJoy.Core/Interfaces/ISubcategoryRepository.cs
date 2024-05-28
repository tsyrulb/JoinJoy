using JoinJoy.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface ISubcategoryRepository : IRepository<Subcategory>
    {
        Task DeleteAsync(Subcategory subcategory);
    }
}

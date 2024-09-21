using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserWithSubcategoriesAsync(int userId);
        Task<IEnumerable<UserSubcategory>> GetUserSubcategoriesAsync(int userId);
        Task AddUserSubcategoryAsync(UserSubcategory userSubcategory);
        Task RemoveUserSubcategoryAsync(UserSubcategory userSubcategory);
        Task SaveChangesAsync();
    }
}

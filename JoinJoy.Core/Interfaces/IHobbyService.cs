using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Core.Interfaces
{
    public interface IHobbyService
    {
        Task<IEnumerable<Hobby>> GetAllHobbiesAsync();
        Task<Hobby> GetHobbyByIdAsync(int id);
        Task<ServiceResult> AddHobbyAsync(Hobby hobby);
        Task<ServiceResult> UpdateHobbyAsync(Hobby hobby);
        Task<ServiceResult> DeleteHobbyAsync(int id);
    }
}

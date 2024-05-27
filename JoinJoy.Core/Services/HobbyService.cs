using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoinJoy.Infrastructure.Services
{
    public class HobbyService : IHobbyService
    {
        private readonly IRepository<Hobby> _hobbyRepository;

        public HobbyService(IRepository<Hobby> hobbyRepository)
        {
            _hobbyRepository = hobbyRepository;
        }

        public async Task<IEnumerable<Hobby>> GetAllHobbiesAsync()
        {
            return await _hobbyRepository.GetAllAsync();
        }

        public async Task<Hobby> GetHobbyByIdAsync(int id)
        {
            return await _hobbyRepository.GetByIdAsync(id);
        }

        public async Task<ServiceResult> AddHobbyAsync(Hobby hobby)
        {
            await _hobbyRepository.AddAsync(hobby);
            return new ServiceResult { Success = true, Message = "Hobby added successfully" };
        }

        public async Task<ServiceResult> UpdateHobbyAsync(Hobby hobby)
        {
            await _hobbyRepository.UpdateAsync(hobby);
            return new ServiceResult { Success = true, Message = "Hobby updated successfully" };
        }

        public async Task<ServiceResult> DeleteHobbyAsync(int id)
        {
            var hobby = await _hobbyRepository.GetByIdAsync(id);
            if (hobby != null)
            {
                await _hobbyRepository.RemoveAsync(hobby);
                return new ServiceResult { Success = true, Message = "Hobby deleted successfully" };
            }
            return new ServiceResult { Success = false, Message = "Hobby not found" };
        }
    }
}

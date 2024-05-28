using System.Collections.Generic;
using System.Threading.Tasks;
using JoinJoy.Core.Interfaces;
using JoinJoy.Core.Models;

namespace JoinJoy.Infrastructure.Services
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly ISubcategoryRepository _subcategoryRepository;

        public SubcategoryService(ISubcategoryRepository subcategoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
        }

        public async Task<IEnumerable<Subcategory>> GetAllSubcategoriesAsync()
        {
            return await _subcategoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryIdAsync(int categoryId)
        {
            return await _subcategoryRepository.FindAsync(sc => sc.CategoryId == categoryId);
        }

        public async Task<Subcategory> GetSubcategoryByIdAsync(int id)
        {
            return await _subcategoryRepository.GetByIdAsync(id);
        }

        public async Task AddSubcategoryAsync(Subcategory subcategory)
        {
            await _subcategoryRepository.AddAsync(subcategory);
        }

        public async Task UpdateSubcategoryAsync(Subcategory subcategory)
        {
            await _subcategoryRepository.UpdateAsync(subcategory);
        }

        public async Task DeleteSubcategoryAsync(int id)
        {
            var subcategory = await _subcategoryRepository.GetByIdAsync(id);
            if (subcategory != null)
            {
                await _subcategoryRepository.DeleteAsync(subcategory);
            }
        }
    }
}

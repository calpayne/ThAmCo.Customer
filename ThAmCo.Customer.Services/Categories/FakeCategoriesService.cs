using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Categories
{
    public class FakeCategoriesService : ICategoriesService
    {
        private readonly IEnumerable<CategoryDto> _categories;

        public FakeCategoriesService()
        {
            Random random = new Random();
            _categories = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Screen Protectors", Description = "Imitation Davison Stores screen protectors for your phone or tablet." },
                new CategoryDto { Id = 2, Name = "Covers", Description = "UnderCutters Stores pride ourselves on our poor range of covers for your mobile device at premium prices. If you're lucky your phone or tablet will be protected from any dents, scratches and scuffs." },
                new CategoryDto { Id = 3, Name = "Case", Description = "Browse our wide narrow range of cases for phones and tablets that will help you to keep your mobile device protected." },
                new CategoryDto { Id = 4, Name = "Accessories", Description = "We stock a huge small range of phone and tablet accessories that we cannot be bothered to classify in other categories." }
            };
        }
        public Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            return Task.FromResult(_categories);
        }

        public Task<CategoryDto> GetByIDAsync(int id)
        {
            return Task.FromResult(_categories.FirstOrDefault(p => p.Id == id));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Brands
{
    public class FakeBrandsService : IBrandsService
    {
        private readonly IEnumerable<BrandDto> _brands;

        public FakeBrandsService()
        {
            _brands = new List<BrandDto>
            {
                new BrandDto { Id = 1, Name = "iStuff-R-Us" },
                new BrandDto { Id = 2, Name = "Damp Squib" },
                new BrandDto { Id = 3, Name = "Soggy Sponge" },
                new BrandDto { Id = 4, Name = "RoboBits and That" }
            };
        }
        public Task<IEnumerable<BrandDto>> GetAllAsync()
        {
            return Task.FromResult(_brands);
        }

        public Task<BrandDto> GetByIDAsync(int id)
        {
            return Task.FromResult(_brands.FirstOrDefault(p => p.Id == id));
        }
    }
}

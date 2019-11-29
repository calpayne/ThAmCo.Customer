using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Reviews
{
    public class FakeReviewsService : IReviewsService
    {
        private IEnumerable<ReviewDto> _reviews;

        public FakeReviewsService()
        {
            _reviews = new List<ReviewDto>
            {
                new ReviewDto { Id = 1, ProductId = 1, Title = "Amazing!", Description = "I really think that this product is outstanding!", Rating = 5 },
                new ReviewDto { Id = 2, ProductId = 1, Title = "Good!", Description = "I really think that this product is good!", Rating = 4 },
                new ReviewDto { Id = 3, ProductId = 1, Title = "Ok!", Description = "I really think that this product is ok!", Rating = 3 },
                new ReviewDto { Id = 4, ProductId = 1, Title = "Meh!", Description = "I really think that this product is meh!", Rating = 2 },
                new ReviewDto { Id = 5, ProductId = 1, Title = "Bad!", Description = "I really think that this product is bad!", Rating = 1 }
            };
        }

        public Task<bool> CreateReview(ReviewDto review)
        {
            _reviews = _reviews.Concat(new List<ReviewDto> { review });
            return Task.FromResult(true);
        }

        public Task<IEnumerable<ReviewDto>> GetAllAsync(int id)
        {
            return Task.FromResult(_reviews.Where(r => r.ProductId == id));
        }
    }
}

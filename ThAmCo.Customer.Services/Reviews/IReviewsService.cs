using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Reviews
{
    public interface IReviewsService
    {
        Task<IEnumerable<ReviewDto>> GetAllAsync(int id);
        Task<bool> CreateReview(ReviewDto review);
    }
}

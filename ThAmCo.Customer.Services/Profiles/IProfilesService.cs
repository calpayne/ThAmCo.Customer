using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Customer.Models;

namespace ThAmCo.Customer.Services.Profiles
{
    public interface IProfilesService
    {
        Task<bool> UpdateProfileAsync(ProfileDto profile);
        Task<ProfileDto> GetProfileAsync(string id);
    }
}

using AutoMapper;
using PoopNPour.Application.Users.Models;
using PoopNPour.Domain.Common.Identity;

namespace PoopNPour.Application.Users.Mappings;

/// <summary>
/// AutoMapper profile for User mappings
/// </summary>
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles will be populated separately
    }
}

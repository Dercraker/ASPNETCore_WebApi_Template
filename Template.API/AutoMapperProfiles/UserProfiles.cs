using AutoMapper;
using Template.Domain.Dto.User;
using Template.Domain.Entities;

namespace Template.API.AutoMapperProfiles;

public class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<UserApi, UserDto>();
        CreateMap<UserDto, UserApi>();
    }
}

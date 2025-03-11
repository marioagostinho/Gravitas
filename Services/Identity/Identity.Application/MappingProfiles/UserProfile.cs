using AutoMapper;
using Identity.Application.Dtos;
using Identity.Application.Features.User.Commands.DeleteUser;
using Identity.Application.Features.User.Commands.UpdateUser;
using Identity.Domain.Entities;

namespace Identity.Application.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UpdateUserCommand>().ReverseMap();
            CreateMap<User, DeleteUserCommand>().ReverseMap();
        }
    }
}

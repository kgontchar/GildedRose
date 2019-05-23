using AutoMapper;
using GildedRose.Api.Models;
using GildedRose.BusinessLogic.DTOs;
using GildedRose.DataAccess.Entities;

namespace GildedRose.Api.Profiles
{
    public class UserProfile : Profile

    {
        public UserProfile()
        {
            CreateMap<UserEntity, UserDto>();
            CreateMap<UserDto, UserEntity>();

            CreateMap<UserDto, UserModel>();
            CreateMap<UserModel, UserDto>()
                .ForMember(x => x.Username, opt => opt.Ignore())
                .ForMember(x => x.Password, opt => opt.Ignore());
        }
    }
}

using AutoMapper;
using GildedRose.BusinessLogic.DTOs;
using GildedRose.BusinessLogic.Interfaces.Services;
using GildedRose.DataAccess.Entities;
using GildedRose.DataAccess.Interfaces.Repositories;

namespace GildedRose.BusinessLogic.Services
{
    public class UserService : BaseService<UserEntity, UserDto>, IUserService
    {
        public UserService(IUserRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}

using AutoMapper;
using GildedRose.Api.Models;
using GildedRose.BusinessLogic.DTOs;
using GildedRose.BusinessLogic.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GildedRose.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : BaseApiController<UserDto, UserModel>
    {
        public UserController(IUserService userService, IMapper mapper) : base(userService, mapper)
        {

        }
    }
}
using System;
using System.Security.Claims;
using AutoMapper;
using GildedRose.Api.Models;
using GildedRose.BusinessLogic.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GildedRose.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult Authenticate([FromBody] AuthModel model)
        {
            try
            {
                var token = _authService.GetUserToken(model?.Username, model?.Password);
                return Ok(token);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("claims")]
        [Authorize]
        public IActionResult GetUserClaims()
        {
            try
            {
                var identityClaims = (ClaimsIdentity)User.Identity;
                var userDto = _authService.GetUserClaims(identityClaims);
                var user = Mapper.Map<UserModel>(userDto);

                return Ok(user);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
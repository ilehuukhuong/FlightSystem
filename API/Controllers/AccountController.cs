using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            if (!IsVietjetAirEmail(loginDto.Email.ToLower()))
            {
                return BadRequest("Invalid Email Domain");
            }

            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());

            if (user == null) return BadRequest("User Not Found");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result) return BadRequest("Invalid Password");

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> EmailExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }

        private bool IsVietjetAirEmail(string email)
        {
            return email.EndsWith("@vietjetair.com", StringComparison.OrdinalIgnoreCase);
        }
    }
}

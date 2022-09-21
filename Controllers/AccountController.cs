using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _Context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext Context, ITokenService tokenService)
        {
            _Context = Context;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerdto)
        {
            if (await UserExists(registerdto.Username)) return BadRequest("UserName IS Taken");


            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerdto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerdto.Password)),
                PasswordSalt = hmac.Key,

            };
            _Context.Users.Add(user);
            await _Context.SaveChangesAsync();
            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
            };
        }

        [HttpPost("login")]

        public async Task<ActionResult<UserDto>> login(LoginDto logindto)
        {
            var user =
                await _Context.Users.SingleOrDefaultAsync(x => x.UserName == logindto.username);
            if (user == null) return Unauthorized("Invalid User Name");
            var hmac = new HMACSHA512(user.PasswordSalt);
            var comutedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.password));

            for (int i = 0; i < comutedHash.Length; i++)
            {

                if
                   (comutedHash[i] != user.PasswordHash[i])
                    return BadRequest("Invalid Password");

            }
            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
            };
        }

        private async Task<bool> UserExists(string username)
        {
          return  await _Context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
    
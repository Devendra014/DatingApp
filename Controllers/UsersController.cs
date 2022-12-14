using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  
    public class UsersController : BaseApiController
    {
        private readonly DataContext _Context;
        public UsersController(DataContext Context)
        {
            _Context = Context;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task< ActionResult<IEnumerable<AppUser>>> GetUsers()

        {
            return await _Context.Users.ToListAsync();
          
        }

        [Authorize]
        //Api/User/3
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)

        {
            return await _Context.Users.FindAsync(id);
           
        }

    }
}

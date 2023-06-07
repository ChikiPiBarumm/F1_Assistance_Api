using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using F1_Assistance_Bot.Data;
using F1_Assistance_Bot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace F1_Assistance_Bot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly F1APIDbContext dbContext;

        public UsersController(F1APIDbContext  dbContext)
        {
            this.dbContext = dbContext;
        }


        // GET: /<controller>/
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await dbContext.Users.ToListAsync());
             
        }


        [HttpGet]
        [Route ("{id:guid}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            var user = await dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        [HttpPost]
        public async Task<IActionResult> AddUserAsync(AddUserRequest addUserRequest)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                ChatId = addUserRequest.ChatId,
                DriverId = addUserRequest.DriverId,
                TeamId = addUserRequest.TeamId
            };


            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPut]
        [Route ("{id:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id,UpdateUserRequest updateUserRequest)
        {
            var user = await dbContext.Users.FindAsync(id);

            if (user != null)
            {
                user.DriverId = updateUserRequest.DriverId;
                user.TeamId = updateUserRequest.TeamId;

                await dbContext.SaveChangesAsync();

                return Ok(user);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var user = await dbContext.Users.FindAsync(id);

            if (user != null)
            {
                dbContext.Remove(user);
                await dbContext.SaveChangesAsync();
                return Ok("Deleted");
            }
            return NotFound();
        }
    }
}


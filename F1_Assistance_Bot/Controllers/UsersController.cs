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
        [Route("api/{chatId}/users/check")]
        public async Task<IActionResult> CheckUserExist([FromRoute] string chatId)
        {
            var user_count = dbContext.Users.Where(u => u.ChatId == chatId).Count();

            if (user_count > 0)
            {
                return Ok(true);
            }

            return Ok(false);
        }
        
        [HttpGet]
        [Route("api/{chatId}/users")]
        public async Task<IActionResult> GetUser([FromRoute] string chatId)
        {
            var user = await this.GetUserByChatId(chatId);

            return Ok(user);
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
        [Route("api/register")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
        {
            var users = dbContext.Users.Where(u => u.ChatId == request.ChatId).Count();

            if (users > 0)
            {
                return BadRequest("User already exists");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                ChatId = request.ChatId,
                DriverId = request.DriverId
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
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
                await dbContext.SaveChangesAsync();

                return Ok(user);
            }

            return NotFound();
        }
        
        [HttpPut]
        [Route("api/{chatId}/users/update")]
        public async Task<IActionResult> UpdateUser([FromRoute] string chatId, UpdateUserRequest request)
        {
            var user = await this.GetUserByChatId(chatId);

            user.DriverId = request.DriverId;
            
        
            await dbContext.SaveChangesAsync();

            return Ok(user);
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
        
        private async Task<User> GetUserByChatId(string chatId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.ChatId == chatId);
            if (user == null)
            {
                BadRequest("User not found");
            }

            return user;
        }
    }
    
}


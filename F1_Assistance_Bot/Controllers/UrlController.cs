using F1_Assistance_Bot.Data;
using F1_Assistance_Bot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace F1_Assistance_Bot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlController: Controller
{
    private readonly F1APIDbContext dbContext;

    public UrlController(F1APIDbContext  dbContext)
    {
        this.dbContext = dbContext;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddUrlAsync(AddUrlRequest addUrlRequest)
    {
        var url = new Url()
        {
            name = addUrlRequest.name
        };


        await dbContext.Urls.AddAsync(url);
        await dbContext.SaveChangesAsync();

        return Ok(url);
    }
    
    [HttpPut]
    [Route ("{id:guid}")]
    public async Task<IActionResult> UpdateUrl([FromRoute] Guid id,UpdateUrlRequest updateUrlRequest)
    {
        var url = await dbContext.Urls.FindAsync(id);

        if (url != null)
        {
            url.name = updateUrlRequest.name;
            

            await dbContext.SaveChangesAsync();

            return Ok(url);
        }

        return NotFound();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUrl()
    {
        
        return Ok(await dbContext.Urls.ToListAsync());
             
    }
}
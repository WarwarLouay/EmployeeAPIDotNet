using EmployeeAPI.Data;
using EmployeeAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly DataContext _context;

        public PostController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("{uid}")]
        public async Task<ActionResult<Post>> AddPost(int uid, Post p)
        {
            var po = new Post
            {
                Title = p.Title,
                Description = p.Description,
                UserId = uid
            };

            _context.post.Add(po);
            await _context.SaveChangesAsync();

            return Ok(po);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, Post p)
        {
            var po = await _context.post.FindAsync(id);

            if (po != null)
            {
                if (p.Title != null) po.Title = p.Title; else po.Title = po.Title;
                if (p.Description != null) po.Description = p.Description; else po.Description = po.Description;
            }
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

using Board.Api.Data;
using Board.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Board.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context; 
        }

        // [GET] /api/posts - 모든 게시글 조회
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.posts.ToListAsync();
        }

        // [GET] /api/posts/{id} - 특정 게시글 조회
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var findPost = await _context.posts.FindAsync(id);

            if (findPost == null)
            {
                return NotFound();
            }
            return Ok(findPost);
        }

        // [POST] /api/posts - 새 게시글 작성
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost([FromBody] Post post)
        {
            post.CreatedAt = DateTime.Now;
            _context.posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }

        //// [PUT] /api/posts/{id} - 특정 게시글 수정
        //[HttpPut("{id}")]
        //public ActionResult<Post> UpdatePost(int id, [FromBody] Post updatePost)
        //{
        //    var post = _posts.FirstOrDefault(p => p.Id == id);

        //    if (post == null)
        //    {
        //        return NotFound();
        //    }

        //    if (!string.IsNullOrEmpty(updatePost.Title))
        //    {
        //        post.Title = updatePost.Title;
        //    }

        //    if (!string.IsNullOrEmpty(updatePost.Description))
        //    {
        //        post.Description = updatePost.Description;
        //    }

        //    if (!string.IsNullOrEmpty(updatePost.Author))
        //    {
        //        post.Author = updatePost.Author;
        //    }

        //    return Ok(post);
        //}

        //// [DELETE] /api/posts/{id} - 특정 게시글 삭제
        //[HttpDelete("{id}")]
        //public ActionResult DeletePost(int id)
        //{
        //    var post = _posts.FirstOrDefault(p => p.Id==id);

        //    if (post == null)
        //    {
        //        return NotFound();
        //    }
        //    _posts.Remove(post);

        //    return NoContent();
        //}
    }
}

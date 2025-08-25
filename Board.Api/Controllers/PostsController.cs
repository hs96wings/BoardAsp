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
            return await _context.Posts.ToListAsync();
        }

        // [GET] /api/posts/{id} - 특정 게시글 조회
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var findPost = await _context.Posts.FindAsync(id);

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
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }

        // [PUT] /api/posts/{id} - 특정 게시글 수정
        [HttpPut("{id}")]
        public async Task<ActionResult<Post>> UpdatePost(int id, [FromBody] Post updatePost)
        {
            // ID가 일치하지 않으면 잘못된 요청으로 처리
            if (id != updatePost.Id)
            {
                return BadRequest();
            }

            // EF Core가 post 객체를 추적하도록 상태를 변경
            _context.Entry(updatePost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException)
            {
                // 업데이트 하려는 사이 다른 사용자가 삭제했을 경우 등 예외 처리
                if (!_context.Posts.Any(e => e.Id == id))
                {
                    return NotFound();
                } else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // [DELETE] /api/posts/{id} - 특정 게시글 삭제
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var deletePost = await _context.Posts.FindAsync(id);

            if (deletePost == null)
            {
                return NotFound();
            }
            _context.Posts.Remove(deletePost);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

using Board.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Board.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        // --- 가짜 DB 역할을 할 static 리스트 ---
        private static List<Post> _posts = new List<Post>
        {
            new Post { Id = 1, Title = "첫 번째 글", Description = "Hello World!", Author = "user1", CreatedAt = DateTime.Now },
            new Post { Id = 2, Title = "Test Title", Description = "Test Description", Author = "testuser", CreatedAt= DateTime.Now },
        };
        private static int _nextId = 3;

        // [GET] /api/posts - 모든 게시글 조회
        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetPosts()
        {
            return Ok(_posts);
        }

        // [GET] /api/posts/{id} - 특정 게시글 조회
        [HttpGet("{id}")]
        public ActionResult<Post> GetPost(int id)
        {
            var post = _posts.FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        // [POST] /api/posts - 새 게시글 작성
        [HttpPost]
        public ActionResult<Post> CreatePost(Post post)
        {
            post.Id = _nextId++;
            post.CreatedAt = DateTime.Now;
            _posts.Add(post);
            return CreatedAtAction(nameof(Post), new { id = post.Id }, _posts);
        }
    }
}

using CommentedPosts.Models;
using CommentedPosts.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CommentedPosts.Controllers
{
	[Route("/[controller]")]
	public class CommentsController : Controller
	{
		private readonly ICommentsRepository commentsRepository;

		public CommentsController(ICommentsRepository commentsRepository)
		{
			this.commentsRepository = commentsRepository;
		}

		// GET api/posts
		[HttpGet]
		[Route("")]
		public IActionResult GetAll()
		{
			var posts = commentsRepository.GetAll();
			return Ok(posts);
		}

		// GET api/posts/5
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var post = this.commentsRepository.Get(id);
			return post == null ? (IActionResult)NotFound() : Ok(post);
		}

		// POST api/posts/5
		[HttpPost]
		[Route("{postId}")]
		public IActionResult Add(int postId, [FromBody]Comment comment)
		{
			var result = this.commentsRepository.Post(postId, comment);

			return Ok(result);
		}

		// DELETE api/posts/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			this.commentsRepository.Delete(id);

			return Ok();
		}
	}
}

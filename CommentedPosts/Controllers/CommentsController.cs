using CommentedPosts.Models;
using CommentedPosts.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommentedPosts.Controllers
{
	[Route("/[controller]")]
	public class CommentsController : Controller
	{
		private readonly ICommentsRepository commentsRepository;

		private HttpContext context;

		public CommentsController(ICommentsRepository commentsRepository)
		{
			this.commentsRepository = commentsRepository;
		}

		public HttpContext Context
		{
			get => context ?? HttpContext;
			set => context = value;
		}

	// POST api/comments/5
		[HttpPost]
		[Route("{postId}")]
		public IActionResult Add(int postId, [FromBody]Comment comment)
		{
			comment.Author = context.User.Identity.Name;
			var result = this.commentsRepository.Post(postId, comment);

			return Ok(result);
		}

		// PUT api/comments/5
		[HttpPut]
		[Route("{id}")]
		public IActionResult Update(int id, [FromBody]Comment comment)
		{
			this.commentsRepository.Put(id, comment);

			return Ok();
		}

		// DELETE api/comments/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			this.commentsRepository.Delete(id);

			return Ok();
		}
	}
}

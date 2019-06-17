using CommentedPosts.Models;
using CommentedPosts.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CommentedPosts.Controllers
{
	[Route("/[controller]")]
	public class PostsController : Controller
	{
		private readonly IPostsRepository postsRepository;

		public PostsController(IPostsRepository postsRepository)
		{
			this.postsRepository = postsRepository;
		}

		// GET api/posts
		[HttpGet]
		[Route("")]
		public IActionResult GetAll()
		{
			var posts = postsRepository.GetAll();
			return Ok(posts);
		}

		// GET api/posts/5
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var post = this.postsRepository.Get(id);
			return post == null ? (IActionResult)NotFound() : Ok(post);
		}

		// POST api/posts
		[HttpPost]
		[Route("")]
		public IActionResult Create([FromBody]Post post)
		{
			var result = this.postsRepository.Post(post);

			return Ok(result);
		}

		// PUT api/posts/5
		[HttpPut("{id}")]
		public IActionResult Edit(int id, [FromBody]Post post)
		{
			this.postsRepository.Put(id, post);

			return Ok();
		}

		// DELETE api/posts/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			this.postsRepository.Delete(id);

			return Ok();
		}
	}
}

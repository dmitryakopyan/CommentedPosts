using CommentedPosts.Models;
using CommentedPosts.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CommentedPosts.Controllers
{
	public class PostsController : Controller
	{
		private readonly IPostsRepository postsRepository;

		public PostsController(IPostsRepository postsRepository)
		{
			this.postsRepository = postsRepository;
		}

		// GET api/posts
		[HttpGet]
		public IActionResult Index()
		{
			var posts = postsRepository.Index();
			return View(posts);
		}

		// GET api/posts
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		// GET api/posts
		[HttpGet]
		public IActionResult Edit(int id)
		{
			var student = this.postsRepository.Get(id);
			return View(student);
		}

		// GET api/posts/5
		[HttpGet]
		public IActionResult Get(int id)
		{
			var student = this.postsRepository.Get(id);
			return student == null ? (IActionResult)NotFound() : Ok(student);
		}

		// POST api/posts
		[HttpPost]
		public IActionResult Create(Post post)
		{
			var result = this.postsRepository.Post(post);

			return RedirectToAction("Index");
		}

		// PUT api/posts/5
		[HttpPost]
		public IActionResult Edit(int id, Post post)
		{
			this.postsRepository.Put(id, post);

			return RedirectToAction("Index");
		}

		// DELETE api/posts/5
		public IActionResult Delete(int id)
		{
			this.postsRepository.Delete(id);

			return RedirectToAction("Index");
		}
	}
}

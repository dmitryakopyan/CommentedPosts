using System.Collections.Generic;
using AutoMapper;
using CommentedPosts.DTO;
using CommentedPosts.Interfaces;
using CommentedPosts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommentedPosts.Controllers
{
	[Route("/[controller]")]
	public class PostsController : Controller
	{
		private readonly IPostsRepository postsRepository;

		private HttpContext context;

		private IMapper mapper;

		public PostsController(IPostsRepository postsRepository, IMapper mapper)
		{
			this.postsRepository = postsRepository;
			this.mapper = mapper;
		}

		public HttpContext Context
		{
			get => context ?? HttpContext;
			set => context = value;
		}

		// GET api/posts
		[HttpGet]
		[Route("")]
		public IActionResult GetAll()
		{
			var posts = mapper.Map<IEnumerable<PostDTO>>(postsRepository.GetAll());
			return Ok(posts);
		}

		// GET api/posts/5
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var post = mapper.Map<PostDTO>(this.postsRepository.Get(id));
			return post == null ? (IActionResult)NotFound() : Ok(post);
		}

		// POST api/posts
		[HttpPost]
		[Route("")]
		public IActionResult Create([FromBody]PostDTO post)
		{
			post.Author = Context.User.Identity.Name;
			var result = this.postsRepository.Post(mapper.Map<Post>(post));

			return Ok(result);
		}

		// PUT api/posts/5
		[HttpPut("{id}")]
		public IActionResult Edit(int id, [FromBody]PostDTO post)
		{
			this.postsRepository.Put(id, mapper.Map<Post>(post));

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

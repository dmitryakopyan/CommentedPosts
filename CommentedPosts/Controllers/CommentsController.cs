using AutoMapper;
using CommentedPosts.DTO;
using CommentedPosts.Interfaces;
using CommentedPosts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommentedPosts.Controllers
{
	[Route("/[controller]")]
	public class CommentsController : Controller
	{
		private readonly ICommentsRepository commentsRepository;

		private HttpContext context;

		private IMapper mapper;

		public CommentsController(ICommentsRepository commentsRepository, IMapper mapper)
		{
			this.commentsRepository = commentsRepository;
			this.mapper = mapper;
		}

		public HttpContext Context
		{
			get => context ?? HttpContext;
			set => context = value;
		}

		// GET api/comments/5
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var comment = mapper.Map<CommentDTO>(this.commentsRepository.Get(id));
			return comment == null ? (IActionResult)NotFound() : Ok(comment);
		}

		// POST api/comments/5
		[HttpPost]
		[Route("{postId}")]
		public IActionResult Add(int postId, [FromBody]CommentDTO comment)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			comment.Author = Context.User.Identity.Name;
			var result = this.commentsRepository.Post(postId, mapper.Map<Comment>(comment));

			return Ok(result);
		}

		// PUT api/comments/5
		[HttpPut]
		[Route("{id}")]
		public IActionResult Update(int id, [FromBody]CommentDTO comment)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			this.commentsRepository.Put(id, mapper.Map<Comment>(comment));

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

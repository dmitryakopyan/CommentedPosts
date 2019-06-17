﻿using CommentedPosts.Models;
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
		[Route("")]
		public IActionResult GetAll()
		{
			var posts = postsRepository.GetAll();
			return Ok(posts);
		}

		// GET api/posts/5
		[HttpGet]
		[Route("item")]
		public IActionResult Get(int id)
		{
			var post = this.postsRepository.Get(id);
			return post == null ? (IActionResult)NotFound() : Ok(post);
		}

		// POST api/posts
		[HttpPost]
		public IActionResult Create(Post post)
		{
			var result = this.postsRepository.Post(post);

			return Ok(result);
		}

		// PUT api/posts/5
		[HttpPost]
		public IActionResult Edit(int id, Post post)
		{
			this.postsRepository.Put(id, post);

			return Ok();
		}

		// DELETE api/posts/5
		public IActionResult Delete(int id)
		{
			this.postsRepository.Delete(id);

			return Ok();
		}
	}
}

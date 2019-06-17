using System;
using System.Collections.Generic;
using System.Linq;
using CommentedPosts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommentedPosts.Repositories
{
	public class PostsRepository : IPostsRepository
	{
		private readonly CommentedPostsDbContext context;

		public PostsRepository(CommentedPostsDbContext context)
		{
			this.context = context;
		}

		// GET api/students
		public IEnumerable<Post> GetAll()
		{
			return context.Posts.ToList();
		}

		// GET api/students/5
		public Post Get(int id)
		{
			return context.Posts.Include(p => p.Comments).FirstOrDefault(s => s.Id == id);
		}

		// POST api/students
		public int Post([FromBody] Post post)
		{
			post.DateTime = DateTime.Now;
			context.Add(post);
			context.SaveChanges();

			return post.Id;
		}

		// PUT api/students/5
		public void Put(int id, [FromBody] Post post)
		{
			var existing = this.Get(id);

			if (existing == null)
				throw new KeyNotFoundException();

			existing.Comment = post.Comment;
			existing.Title = post.Title;

			context.Update(existing);
			context.SaveChanges();
		}

		// DELETE api/students/5
		public void Delete(int id)
		{
			var existing = this.Get(id);

			if (existing != null)
			{
				context.Remove(existing);
				context.SaveChanges();
			}
		}
	}
}

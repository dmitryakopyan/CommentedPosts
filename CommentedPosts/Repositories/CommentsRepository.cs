using System;
using System.Collections.Generic;
using System.Linq;
using CommentedPosts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommentedPosts.Repositories
{
	public class CommentsRepository : ICommentsRepository
	{
		private readonly CommentedPostsDbContext context;

		public CommentsRepository(CommentedPostsDbContext context)
		{
			this.context = context;
		}

		// GET api/students
		public IEnumerable<Comment> GetAll()
		{
			return context.Comments.ToList();
		}

		// GET api/students/5
		public Comment Get(int id)
		{
			return context.Comments.FirstOrDefault(s => s.ID == id);
		}

		// POST api/students
		public int Post(int postId, [FromBody] Comment comment)
		{
			comment.DateTime = DateTime.Now;
			comment.PostID = postId;
			context.Add(comment);
			context.SaveChanges();

			return comment.ID;
		}

		// PUT api/students/5
		[HttpPut]
		[ActionName("Update")]
		public void Put(int id, [FromBody] Comment comment)
		{
			var existing = this.Get(id);

			if (existing == null)
				throw new KeyNotFoundException();

			existing.Content = comment.Content;

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

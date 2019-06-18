using System;
using System.Linq;
using CommentedPosts.Interfaces;
using CommentedPosts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommentedPosts.Repositories
{
	public class CommentsRepository : ICommentsRepository
	{
		private readonly CommentedPostsDbContext context;

		private readonly IClock clock;

		public CommentsRepository(CommentedPostsDbContext context, IClock clock)
		{
			this.context = context;
			this.clock = clock;
		}

		// GET api/comments/5
		public Comment Get(int id)
		{
			return context.Comments.FirstOrDefault(s => s.ID == id);
		}

		// POST api/comments/5
		public int Post(int postId, [FromBody] Comment comment)
		{
			comment.DateTime = clock.GetTime();
			comment.PostID = postId;
			context.Add(comment);
			context.SaveChanges();

			return comment.ID;
		}

		// PUT api/comments/5
		public void Put(int id, [FromBody] Comment comment)
		{
			var existing = this.Get(id);

			if (existing == null)
				throw new Exception("Not found");

			existing.Content = comment.Content;

			context.Update(existing);
			context.SaveChanges();
		}

		// DELETE api/comments/5
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

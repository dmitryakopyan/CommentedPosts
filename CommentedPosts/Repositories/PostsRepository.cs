using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommentedPosts.Interfaces;
using CommentedPosts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommentedPosts.Repositories
{
	public class PostsRepository : IPostsRepository
	{
		private readonly CommentedPostsDbContext context;

		private readonly IClock clock;

		public PostsRepository(CommentedPostsDbContext context, IClock clock)
		{
			this.context = context;
			this.clock = clock;
		}

		// GET api/posts
		public async Task<IEnumerable<Post>> GetAllAsync()
		{
			return await context.Posts.ToListAsync();
		}

		// GET api/posts/5
		public Post Get(int id)
		{
			return context.Posts.Include(p => p.Comments).FirstOrDefault(s => s.Id == id);
		}

		// POST api/posts
		public async Task<int> PostAsync([FromBody] Post post)
		{
			post.DateTime = clock.GetTime();
			await context.AddAsync(post);
			await context.SaveChangesAsync();

			return post.Id;
		}

		// PUT api/posts/5
		public void Put(int id, [FromBody] Post post)
		{
			var existing = this.Get(id);

			if (existing == null)
				throw new KeyNotFoundException();

			existing.Content = post.Content;
			existing.Title = post.Title;

			context.Update(existing);
			context.SaveChanges();
		}

		// DELETE api/posts/5
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

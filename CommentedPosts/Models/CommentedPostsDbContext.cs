using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CommentedPosts.Models
{
	public class CommentedPostsDbContext : DbContext
	{
		public CommentedPostsDbContext(DbContextOptions options) : base(options)
		{
		}

		public CommentedPostsDbContext()
		{
			
		}

		public virtual DbSet<Post> Posts { get; set; }

		public virtual DbSet<Comment> Comments { get; set; }
	}
}

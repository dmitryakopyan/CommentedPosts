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
			//Database.S(new MigrateDatabaseToLatestVersion<CommentedPostsDbContext, EF6Console.Migrations.Configuration>());
		}

		public DbSet<Post> Posts { get; set; }

		public DbSet<Comment> Comments { get; set; }
	}
}

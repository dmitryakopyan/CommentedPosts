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

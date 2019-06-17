using System;

namespace CommentedPosts.Models
{
	public class Post
	{
		public int Id { get; set; }

		public string Author { get; set; }

		public string Title { get; set; }

		public string Comment { get; set; }

		public DateTime DateTime { get; set; }
	}
}

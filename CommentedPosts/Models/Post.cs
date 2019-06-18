using System;
using System.Collections;
using System.Collections.Generic;

namespace CommentedPosts.Models
{
	public class Post
	{
		public int Id { get; set; }

		public string Author { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public DateTime DateTime { get; set; }

		public IList<Comment> Comments { get; set; }
	}
}

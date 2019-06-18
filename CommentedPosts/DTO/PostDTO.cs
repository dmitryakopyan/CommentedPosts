using System;
using System.Collections.Generic;

namespace CommentedPosts.DTO
{
	public class PostDTO
	{
		public int Id { get; set; }

		public string Author { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public DateTime DateTime { get; set; }

		public IList<CommentDTO> Comments { get; set; }
	}
}

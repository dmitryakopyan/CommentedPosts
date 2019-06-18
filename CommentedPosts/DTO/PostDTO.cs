using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommentedPosts.DTO
{
	public class PostDTO
	{
		public int Id { get; set; }

		public string Author { get; set; }

		[Required]
		[MaxLength(50)]
		public string Title { get; set; }

		[Required]
		[MaxLength(2000)]
		public string Content { get; set; }

		public DateTime DateTime { get; set; }

		public IList<CommentDTO> Comments { get; set; }
	}
}

using System;
using System.ComponentModel.DataAnnotations;

namespace CommentedPostsFront.Models
{
	public class Comment
	{
		public int ID { get; set; }

		public int PostID { get; set; }

		[Required]
		public string Content { get; set; }

		public string Author { get; set; }

		public DateTime DateTime { get; set; }
	}
}

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CommentedPostsFront.ViewModels
{
	public class CommentViewModel
	{
		public int ID { get; set; }

		public int PostID { get; set; }

		[Required]
		[MaxLength(2000)]
		public string Content { get; set; }

		[DisplayName("Posted by")]
		public string Author { get; set; }

		[DisplayName("Posted at")]
		public DateTime DateTime { get; set; }
	}
}

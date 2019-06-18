using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CommentedPostsFront.ViewModels
{
	public class PostViewModel
	{
		public int Id { get; set; }

		[DisplayName("Posted by")]
		public string Author { get; set; }

		[Required]
		[MaxLength(50)]
		public string Title { get; set; }

		[Required]
		[MaxLength(2000)]
		public string Content { get; set; }

		[DisplayName("Posted at")]
		public DateTime DateTime { get; set; }

		public IList<CommentViewModel> Comments { get; set; }

		public string ContentTrimmed
		{
			get { return Content.Length > 50 ? Content.Substring(0, 50) + "..." : Content; }
		}
	}
}

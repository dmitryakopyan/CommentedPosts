using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentedPosts.DTO
{
	public class CommentDTO
	{
		public int ID { get; set; }

		public int PostID { get; set; }

		public string Content { get; set; }

		public string Author { get; set; }

		public DateTime DateTime { get; set; }
	}
}

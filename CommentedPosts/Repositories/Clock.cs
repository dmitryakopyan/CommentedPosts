using System;
using CommentedPosts.Interfaces;

namespace CommentedPosts.Repositories
{
	public class Clock : IClock
	{
		public DateTime GetTime()
		{
			return DateTime.Now;
		}
	}
}
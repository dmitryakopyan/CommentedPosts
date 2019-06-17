using System.Collections.Generic;
using CommentedPosts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommentedPosts.Repositories
{
	public interface IPostsRepository
	{
		IEnumerable<Post> Index();
		Post Get(int id);
		int Post([FromBody]Post post);
		void Put(int id, [FromBody]Post post);
		void Delete(int id);
	}
}
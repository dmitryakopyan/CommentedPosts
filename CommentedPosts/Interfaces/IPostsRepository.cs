using System.Collections.Generic;
using CommentedPosts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommentedPosts.Interfaces
{
	public interface IPostsRepository
	{
		IEnumerable<Post> GetAll();
		Post Get(int id);
		int Post([FromBody]Post post);
		void Put(int id, [FromBody]Post post);
		void Delete(int id);
	}
}
using System.Collections.Generic;
using System.Threading.Tasks;
using CommentedPosts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommentedPosts.Interfaces
{
	public interface IPostsRepository
	{
		Task<IEnumerable<Post>> GetAllAsync();
		Post Get(int id);
		Task<int> PostAsync([FromBody]Post post);
		void Put(int id, [FromBody]Post post);
		void Delete(int id);
	}
}
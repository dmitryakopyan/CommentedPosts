using CommentedPosts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommentedPosts.Interfaces
{
	public interface ICommentsRepository
	{
		Comment Get(int id);
		int Post(int postId, [FromBody]Comment comment);
		void Put(int id, [FromBody]Comment comment);
		void Delete(int id);
	}
}
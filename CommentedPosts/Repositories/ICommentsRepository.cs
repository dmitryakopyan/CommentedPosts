using System.Collections.Generic;
using CommentedPosts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommentedPosts.Repositories
{
	public interface ICommentsRepository
	{
		IEnumerable<Comment> GetAll();
		Comment Get(int id);
		int Post(int postId, [FromBody]Comment comment);
		void Put(int id, [FromBody]Comment comment);
		void Delete(int id);
	}
}
using System.Security.Claims;
using System.Security.Principal;
using CommentedPosts.Controllers;
using CommentedPosts.Models;
using CommentedPosts.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace CommentedPosts.UnitTests
{
	public class CommentsControllerTests
	{
		private const string CurrentUser = "CurrentUser";

		private CommentsController controller;

		private Mock<ICommentsRepository> mockRepository;

		[SetUp]
		public void Setup()
		{
			var context = new Mock<HttpContext>();
			var user = new Mock<ClaimsPrincipal>();
			var identity = new Mock<ClaimsIdentity>();
			identity.Setup(x => x.Name).Returns(CurrentUser);
			user.Setup(x => x.Identity).Returns(identity.Object);
			context.Setup(x => x.User).Returns(user.Object);

			mockRepository = new Mock<ICommentsRepository>();
			controller = new CommentsController(mockRepository.Object);
			controller.Context = context.Object;
		}

		/// <summary>
		/// Add method calls post method of the repository and returns OK result with new comment id value.
		/// </summary>
		[Test]
		public void AddMethodCallsRepositoryPostMethodAndReturnsOkResultWithNewCommentId()
		{
			// arrange
			var postId = 15;
			var commentId = 5;
			mockRepository.Setup(x => x.Post(postId, It.IsAny<Comment>())).Returns(commentId);

			// act
			IActionResult result = controller.Add(postId, new Comment());

			// assert
			mockRepository.VerifyAll();
			Assert.IsInstanceOf<OkObjectResult>(result);
			var value = ((OkObjectResult) result).Value;
			Assert.AreEqual(commentId, value);
		}

		/// <summary>
		/// Update method calls put method of the repository and returns OK result.
		/// </summary>
		[Test]
		public void UpdateMethodCallsRepositoryPutMethodAndReturnsOkResult()
		{
			// arrange
			var commentId = 5;
			mockRepository.Setup(x => x.Put(commentId, It.IsAny<Comment>()));

			// act
			IActionResult result = controller.Update(commentId, new Comment());

			// assert
			mockRepository.VerifyAll();
			Assert.IsInstanceOf<OkResult>(result);
		}

		/// <summary>
		/// Delete method calls delete method of the repository and returns OK result.
		/// </summary>
		[Test]
		public void DeleteMethodCallsRepositoryDeleteMethodAndReturnsOkResult()
		{
			// arrange
			var commentId = 5;
			mockRepository.Setup(x => x.Delete(commentId));

			// act
			IActionResult result = controller.Delete(commentId);

			// assert
			mockRepository.VerifyAll();
			Assert.IsInstanceOf<OkResult>(result);
		}
	}
}
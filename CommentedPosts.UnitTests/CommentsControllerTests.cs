using System.Security.Claims;
using AutoMapper;
using CommentedPosts.Controllers;
using CommentedPosts.DTO;
using CommentedPosts.Interfaces;
using CommentedPosts.Models;
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

		private Mock<IMapper> mapper;

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
			mapper = new Mock<IMapper>();
			controller = new CommentsController(mockRepository.Object, mapper.Object);
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
			IActionResult result = controller.Add(postId, new CommentDTO());

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
			IActionResult result = controller.Update(commentId, new CommentDTO());

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
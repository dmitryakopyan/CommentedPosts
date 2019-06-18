using System.Collections.Generic;
using System.Security.Claims;
using CommentedPosts.Controllers;
using CommentedPosts.Models;
using CommentedPosts.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace CommentedPosts.UnitTests
{
	public class PostsControllerTests
	{
		private const string CurrentUser = "CurrentUser";

		private PostsController controller;

		private Mock<IPostsRepository> mockRepository;

		[SetUp]
		public void Setup()
		{
			var context = new Mock<HttpContext>();
			var user = new Mock<ClaimsPrincipal>();
			var identity = new Mock<ClaimsIdentity>();
			identity.Setup(x => x.Name).Returns(CurrentUser);
			user.Setup(x => x.Identity).Returns(identity.Object);
			context.Setup(x => x.User).Returns(user.Object);

			mockRepository = new Mock<IPostsRepository>();
			controller = new PostsController(mockRepository.Object);
			controller.Context = context.Object;
		}

		/// <summary>
		/// Get all method calls get all method of the repository and returns OK result with post collection.
		/// </summary>
		[Test]
		public void GetAllMethodCallsRepositoryGetAllMethodAndReturnsOkResultWithPostCollection()
		{
			// arrange
			var postId = 15;
			var postCollection = new List<Post>{new Post() {Title = "MyPost"}};
			mockRepository.Setup(x => x.GetAll()).Returns(postCollection);

			// act
			IActionResult result = controller.GetAll();

			// assert
			mockRepository.VerifyAll();
			Assert.IsInstanceOf<OkObjectResult>(result);
			var value = ((OkObjectResult)result).Value;
			Assert.IsInstanceOf<List<Post>>(value);
			Assert.IsTrue(this.AreCollectionsEqual(postCollection, (List<Post>)value));
		}

		/// <summary>
		/// Given post exists
		/// When get method called
		/// Then get method of the repository is called and returns OK result with the post.
		/// </summary>
		[Test]
		public void GivenPostExistsWhenGetMethodCalledThenRepositoryGetMethodCalledAndReturnsOkResultWithThePost()
		{
			// arrange
			var postId = 15;
			var post = new Post() { Title = "MyPost" };
			mockRepository.Setup(x => x.Get(postId)).Returns(post);

			// act
			IActionResult result = controller.Get(postId);

			// assert
			mockRepository.VerifyAll();
			Assert.IsInstanceOf<OkObjectResult>(result);
			var value = ((OkObjectResult)result).Value;
			Assert.AreEqual(post, value);
		}

		/// <summary>
		/// Given post does not exist
		/// When get method called
		/// Then get method of the repository is called and returns not found result.
		/// </summary>
		[Test]
		public void GivenPostDoesNotExistWhenGetMethodCalledThenRepositoryGetMethodCalledAndReturnsNotFoundResult()
		{
			// arrange
			var postId = 15;
			mockRepository.Setup(x => x.Get(postId)).Returns((Post)null);

			// act
			IActionResult result = controller.Get(postId);

			// assert
			mockRepository.VerifyAll();
			Assert.IsInstanceOf<NotFoundResult>(result);
		}

		/// <summary>
		/// Create method calls post method of the repository and returns OK result with new post id value.
		/// </summary>
		[Test]
		public void CreateMethodCallsRepositoryPostMethodAndReturnsOkResultWithNewPostId()
		{
			// arrange
			var postId = 15;
			mockRepository.Setup(x => x.Post(It.IsAny<Post>())).Returns(postId);

			// act
			IActionResult result = controller.Create(new Post());

			// assert
			mockRepository.VerifyAll();
			Assert.IsInstanceOf<OkObjectResult>(result);
			var value = ((OkObjectResult) result).Value;
			Assert.AreEqual(postId, value);
		}

		/// <summary>
		/// Edit method calls put method of the repository and returns OK result.
		/// </summary>
		[Test]
		public void EditMethodCallsRepositoryPutMethodAndReturnsOkResult()
		{
			// arrange
			var postId = 5;
			mockRepository.Setup(x => x.Put(postId, It.IsAny<Post>()));

			// act
			IActionResult result = controller.Edit(postId, new Post());

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
			var postId = 5;
			mockRepository.Setup(x => x.Delete(postId));

			// act
			IActionResult result = controller.Delete(postId);

			// assert
			mockRepository.VerifyAll();
			Assert.IsInstanceOf<OkResult>(result);
		}

		private bool AreCollectionsEqual(IReadOnlyList<Post> expected, IReadOnlyList<Post> actual)
		{
			if (expected.Count != actual.Count)
				return false;

			for (int i =0; i<expected.Count;i++)
			{
				if (!expected[i].Equals(actual[i]))
					return false;
			}

			return true;
		}
	}
}
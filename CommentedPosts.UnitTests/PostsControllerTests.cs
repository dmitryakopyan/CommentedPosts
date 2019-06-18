using System.Collections.Generic;
using System.Linq;
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
	public class PostsControllerTests
	{
		private const string CurrentUser = "CurrentUser";

		private PostsController controller;

		private Mock<IPostsRepository> mockRepository;

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

			mockRepository = new Mock<IPostsRepository>();
			mapper = new Mock<IMapper>();
			mapper.Setup(x => x.Map<Post>(It.IsAny<PostDTO>())).Returns((PostDTO post) => ConvertFromDto(post));
			mapper.Setup(x => x.Map<IEnumerable<Post>>(It.IsAny<IEnumerable<PostDTO>>())).Returns((IEnumerable<PostDTO> posts) => posts.ToList().Select(ConvertFromDto));
			mapper.Setup(x => x.Map<PostDTO>(It.IsAny<Post>())).Returns((Post post) => ConvertToDto(post));
			mapper.Setup(x => x.Map<IEnumerable<PostDTO>>(It.IsAny<IEnumerable<Post>>())).Returns((IEnumerable<Post> posts) => posts.ToList().Select(ConvertToDto));
			controller = new PostsController(mockRepository.Object, mapper.Object);
			controller.Context = context.Object;
		}

		/// <summary>
		/// Get all method calls get all method of the repository and returns OK result with post collection.
		/// </summary>
		[Test]
		public void GetAllMethodCallsRepositoryGetAllMethodAndReturnsOkResultWithPostCollection()
		{
			// arrange
			var postCollection = new List<Post>{new Post() {Title = "MyPost"}};
			mockRepository.Setup(x => x.GetAll()).Returns(postCollection);

			// act
			IActionResult result = controller.GetAll();

			// assert
			mockRepository.VerifyAll();
			Assert.IsInstanceOf<OkObjectResult>(result);
			var value = ((OkObjectResult)result).Value;
			Assert.IsInstanceOf<IEnumerable<PostDTO>>(value);
			Assert.IsTrue(this.AreCollectionsEqual(postCollection, (IEnumerable<PostDTO>)value));
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
			Assert.IsTrue(AreObjectsEqual(post, (PostDTO)value));
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
			IActionResult result = controller.Create(new PostDTO());

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
			IActionResult result = controller.Edit(postId, new PostDTO());

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

		private bool AreCollectionsEqual(IEnumerable<Post> expected, IEnumerable<PostDTO> actual)
		{
			if (expected.Count() != actual.Count())
				return false;

			var expectedEnumr = expected.GetEnumerator();
			var actualEnumr = actual.GetEnumerator();

			while (expectedEnumr.MoveNext())
			{
				actualEnumr.MoveNext();
				if (!AreObjectsEqual(expectedEnumr.Current, actualEnumr.Current))
					return false;
			}

			return true;
		}

		private bool AreObjectsEqual(Post expected, PostDTO actual)
		{
			return expected.Content == actual.Content
			       && expected.Title == actual.Title
			       && expected.Author == actual.Author
			       && expected.Id == actual.Id
			       && expected.DateTime == actual.DateTime;
		}

		private Post ConvertFromDto(PostDTO post)
		{
			if (post == null) return null;

			return new Post()
			{
				Content = post.Content,
				DateTime = post.DateTime,
				Title = post.Title,
				Author = post.Author,
				Id = post.Id
			};
		}

		private PostDTO ConvertToDto(Post post)
		{
			if (post == null) return null;

			return new PostDTO()
			{
				Content = post.Content,
				DateTime = post.DateTime,
				Title = post.Title,
				Author = post.Author,
				Id = post.Id
			};
		}
	}
}
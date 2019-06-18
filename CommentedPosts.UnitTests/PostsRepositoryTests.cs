using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CommentedPosts.Interfaces;
using CommentedPosts.Models;
using CommentedPosts.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace CommentedPosts.UnitTests
{
	public class PostsRepositoryTests
	{
		private const string CurrentUser = "CurrentUser";

		private IPostsRepository controller;

		private Mock<CommentedPostsDbContext> mockContext;

		private DateTime CurrentTime = new DateTime(2013, 5, 6);

		[SetUp]
		public void Setup()
		{
			var context = new Mock<HttpContext>();
			var user = new Mock<ClaimsPrincipal>();
			var identity = new Mock<ClaimsIdentity>();
			identity.Setup(x => x.Name).Returns(CurrentUser);
			user.Setup(x => x.Identity).Returns(identity.Object);
			context.Setup(x => x.User).Returns(user.Object);

			var mockClock = new Mock<IClock>();
			mockClock.Setup(x => x.GetTime()).Returns(CurrentTime);

			mockContext = new Mock<CommentedPostsDbContext>();
			controller = new PostsRepository(mockContext.Object, mockClock.Object);
		}

		/// <summary>
		/// Post method calls add method of the context and returns new post id value.
		/// </summary>
		[Test]
		public void PostMethodCallsContextAddMethodAndReturnsNewPostId()
		{
			// arrange
			var postId = 15;
			var post = new Post() { Id = postId };
			mockContext.Setup(x => x.Add(It.IsAny<Post>()));
			mockContext.Setup(x => x.SaveChanges());

			// act
			int result = controller.Post(post);

			// assert
			mockContext.VerifyAll();
			Assert.AreEqual(CurrentTime, post.DateTime);
			Assert.AreEqual(postId, result);
		}

		/// <summary>
		/// Put method calls update method of the context.
		/// </summary>
		[Test]
		public void PutMethodCallsContextUpdateMethod()
		{
			// arrange
			var postId = 5;

			var existingPost = new Post() { Id = postId, Author = "Author", Content = "Old", Title = "Old"};
			var data = new List<Post> { existingPost }.AsQueryable();
			var mockSet = CreateMockSet(data);
			mockContext.Setup(x => x.Posts).Returns(mockSet.Object);

			mockContext.Setup(x => x.Update(It.IsAny<Post>()));
			mockContext.Setup(x => x.SaveChanges());

			// act
			controller.Put(postId, new Post() { Id = postId, Content = "NewComment", Title = "NewTitle" });

			// assert
			mockContext.VerifyAll();
			Assert.AreEqual("NewComment", existingPost.Content);
			Assert.AreEqual("NewTitle", existingPost.Title);
		}

		/// <summary>
		/// Delete method calls remove method of the context.
		/// </summary>
		[Test]
		public void DeleteMethodCallsContextRemoveMethod()
		{
			// arrange
			var postId = 5;

			var existingPost = new Post() { Id = postId, Author = "Author", Content = "Old", Title = "Old" };
			var data = new List<Post> { existingPost }.AsQueryable();
			var mockSet = CreateMockSet(data);

			mockContext.Setup(x => x.Posts).Returns(mockSet.Object);

			mockContext.Setup(x => x.Remove(It.IsAny<Post>()));
			mockContext.Setup(x => x.SaveChanges());

			// act
			controller.Delete(postId);

			// assert
			mockContext.VerifyAll();
		}

		private static Mock<DbSet<Post>> CreateMockSet(IQueryable<Post> data)
		{
			var mockSet = new Mock<DbSet<Post>>();
			mockSet.As<IQueryable<Post>>().Setup(x => x.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Post>>().Setup(x => x.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Post>>().Setup(x => x.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Post>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());
			return mockSet;
		}
	}
}
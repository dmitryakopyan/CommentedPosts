using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using CommentedPosts.Controllers;
using CommentedPosts.Models;
using CommentedPosts.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace CommentedPosts.UnitTests
{
	public class CommentsRepositoryTests
	{
		private const string CurrentUser = "CurrentUser";

		private CommentsRepository controller;

		private Mock<CommentedPostsDbContext> mockContext;

		private Mock<IClock> mockClock;

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
			controller = new CommentsRepository(mockContext.Object, mockClock.Object);
		}

		/// <summary>
		/// Post method calls add method of the context and returns new comment id value.
		/// </summary>
		[Test]
		public void PostMethodCallsContextAddMethodAndReturnsNewCommentId()
		{
			// arrange
			var postId = 15;
			var commentId = 5;
			var comment = new Comment() { ID = commentId };
			mockContext.Setup(x => x.Add(It.IsAny<Comment>()));
			mockContext.Setup(x => x.SaveChanges());

			// act
			int result = controller.Post(postId, comment);

			// assert
			mockContext.VerifyAll();
			Assert.AreEqual(postId, comment.PostID);
			Assert.AreEqual(CurrentTime, comment.DateTime);
			Assert.AreEqual(commentId, result);
		}

		/// <summary>
		/// Put method calls update method of the context.
		/// </summary>
		[Test]
		public void PutMethodCallsContextUpdateMethod()
		{
			// arrange
			var commentId = 5;

			var existingComment = new Comment() { ID = commentId, Author = "Author", Content = "Old"};
			var data = new List<Comment> { existingComment }.AsQueryable();
			var mockSet = CreateMockSet(data);
			mockContext.Setup(x => x.Comments).Returns(mockSet.Object);

			mockContext.Setup(x => x.Update(It.IsAny<Comment>()));
			mockContext.Setup(x => x.SaveChanges());

			// act
			controller.Put(commentId, new Comment() { ID = commentId, Content = "New"});

			// assert
			mockContext.VerifyAll();
			Assert.AreEqual("New", existingComment.Content);
		}

		/// <summary>
		/// Delete method calls remove method of the context and returns OK result.
		/// </summary>
		[Test]
		public void DeleteMethodCallsContextRemoveMethod()
		{
			// arrange
			var commentId = 5;

			var existingComment = new Comment() { ID = commentId, Author = "Author", Content = "Old" };
			var data = new List<Comment> { existingComment }.AsQueryable();
			var mockSet = CreateMockSet(data);

			mockContext.Setup(x => x.Comments).Returns(mockSet.Object);

			mockContext.Setup(x => x.Remove(It.IsAny<Comment>()));
			mockContext.Setup(x => x.SaveChanges());

			// act
			controller.Delete(commentId);

			// assert
			mockContext.VerifyAll();
		}

		private static Mock<DbSet<Comment>> CreateMockSet(IQueryable<Comment> data)
		{
			var mockSet = new Mock<DbSet<Comment>>();
			mockSet.As<IQueryable<Comment>>().Setup(x => x.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<Comment>>().Setup(x => x.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<Comment>>().Setup(x => x.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<Comment>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());
			return mockSet;
		}
	}
}
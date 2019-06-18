using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using AutoMapper;
using CommentedPostsFront.Models;
using CommentedPostsFront.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CommentedPostsFront.Controllers
{
	public class CommentsController : Controller
	{
		private const string UriString = "https://localhost:44368/";

		private IMapper mapper;

		public CommentsController(IMapper mapper)
		{
			this.mapper = mapper;
		}

		private static HttpClient GetHttpClient()
		{
			return new HttpClient(new HttpClientHandler() {UseDefaultCredentials = true});
		}

		// GET api/posts
		[HttpGet]
		public IActionResult Add(int postId)
		{
			ViewBag.PostId = postId;
			return View();
		}

		// GET api/posts
		[HttpGet]
		public IActionResult Edit(int postId, int id)
		{
			var comment = this.Get(id);
			ViewBag.PostId = postId;
			return View(comment);
		}

		// GET api/posts/5
		[HttpGet]
		public CommentViewModel Get(int id)
		{
			Comment comment;

			using (var client = GetHttpClient())
			{
				client.BaseAddress = new Uri(UriString);

				var responseTask = client.GetAsync("comments/" + id);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsStringAsync();
					readTask.Wait();

					comment = JsonConvert.DeserializeObject<Comment>(readTask.Result);
				}
				else
				{
					comment = null;

					ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
				}
			}
			return mapper.Map<CommentViewModel>(comment);
		}

		// POST api/posts
		[HttpPost]
		public IActionResult Add(int postId, CommentViewModel comment)
		{
			using (var client = GetHttpClient())
			{
				client.BaseAddress = new Uri(UriString);

				var model = mapper.Map<CommentViewModel>(comment);
				var json = JsonConvert.SerializeObject(model);
				var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

				var responseTask = client.PostAsync("comments/" + postId, content);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					return RedirectToAction("Details", "Posts", new { id = postId.ToString() });
				}
			}

			ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

			return View(comment);
		}

		// PUT api/posts/5
		[HttpPost]
		public IActionResult Edit(int id, int postId, CommentViewModel comment)
		{
			using (var client = GetHttpClient())
			{
				client.BaseAddress = new Uri(UriString);

				var model = mapper.Map<CommentViewModel>(comment);
				var json = JsonConvert.SerializeObject(model);
				var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

				var responseTask = client.PutAsync("comments/" + comment.ID, content);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					return RedirectToAction("Details", "Posts", new { id = postId.ToString() });
				}
			}

			ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

			return View(comment);
		}
	
		// DELETE api/posts/5
		public IActionResult Delete(int postId, int id)
		{
			using (var client = GetHttpClient())
			{
				client.BaseAddress = new Uri(UriString);

				var responseTask = client.DeleteAsync("comments/" + id);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					return RedirectToAction("Details", "Posts", new { id = postId.ToString() });
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
					return RedirectToAction("Details", "Posts", new { id = postId.ToString() });
				}
			}
		}
	}
}

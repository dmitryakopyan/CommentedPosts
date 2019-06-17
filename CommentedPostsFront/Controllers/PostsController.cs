using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using CommentedPostsUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CommentedPostsUI.Controllers
{
	public class PostsController : Controller
	{
		private const string UriString = "https://localhost:44368/";

		public PostsController()
		{
		}

		// GET api/posts
		[HttpGet]
		public IActionResult Index()
		{
			IEnumerable<Post> students = null;

			using (var client = GetHttpClient())
			{
				client.BaseAddress = new Uri(UriString);

				var responseTask = client.GetAsync("posts");
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsStringAsync();
					readTask.Wait();

					students = JsonConvert.DeserializeObject<List<Post>>(readTask.Result);
				}
				else
				{
					students = Enumerable.Empty<Post>();

					ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
				}
			}
			return View(students);
		}

		private static HttpClient GetHttpClient()
		{
			return new HttpClient(new HttpClientHandler() {UseDefaultCredentials = true});
		}

		// GET api/posts
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		// GET api/posts
		[HttpGet]
		public IActionResult Details(int id)
		{
			var student = this.Get(id);
			return View(student);
		}

		// GET api/posts
		[HttpGet]
		public IActionResult Edit(int id)
		{
			var student = this.Get(id);
			return View(student);
		}

		// GET api/posts/5
		[HttpGet]
		public Post Get(int id)
		{
			Post post;

			using (var client = GetHttpClient())
			{
				client.BaseAddress = new Uri(UriString);

				var responseTask = client.GetAsync("posts/" + id);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					var readTask = result.Content.ReadAsStringAsync();
					readTask.Wait();

					post = JsonConvert.DeserializeObject<Post>(readTask.Result);
				}
				else
				{
					post = null;

					ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
				}
			}
			return post;
		}

		// POST api/posts
		[HttpPost]
		public IActionResult Create(Post post)
		{
			using (var client = GetHttpClient())
			{
				client.BaseAddress = new Uri(UriString);

				var json = JsonConvert.SerializeObject(post);
				var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

				var responseTask = client.PostAsync("posts", content);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
			}

			ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

			return View(post);
		}

		// PUT api/posts/5
		[HttpPost]
		public IActionResult Edit(int id, Post post)
		{
			using (var client = GetHttpClient())
			{
				client.BaseAddress = new Uri(UriString);

				var json = JsonConvert.SerializeObject(post);
				var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");

				var responseTask = client.PutAsync("posts/" + post.Id, content);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
			}

			ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

			return View(post);
		}
	

		// DELETE api/posts/5
		public IActionResult Delete(int id)
		{
			using (var client = GetHttpClient())
			{
				client.BaseAddress = new Uri(UriString);

				var responseTask = client.DeleteAsync("posts/" + id);
				responseTask.Wait();

				var result = responseTask.Result;
				if (result.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
					return RedirectToAction("Index");
				}
			}
		}
	}
}

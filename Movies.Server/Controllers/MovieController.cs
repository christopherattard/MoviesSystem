using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Server.Controllers
{
	[Route("api/[controller]")]
	public class MovieController : Controller
	{
		private readonly IMovieGrainClient _client;
		public MovieController(IMovieGrainClient client)
		{
			_client = client;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet("{id}")]
		public async Task<SampleDataModel> Get(string id)
		{
			var result = await _client.Get(id).ConfigureAwait(false);
			return result;
		}

		// List Top 5 movies

		// List all movies

		// Search for movie/s

		// Filter by Genre

		// Get selected movie details

		// Create movie

		// Update movie
	}
}

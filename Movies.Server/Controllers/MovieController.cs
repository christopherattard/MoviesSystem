using Microsoft.AspNetCore.Mvc;
using Movies.Contracts;
using Movies.Models;
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

		[HttpPost]
		public async Task<MovieState> CreateMovie([FromBody] MovieState movieState)
		{
			Console.WriteLine("-- POST api/movie: create movie");

			var result = await _client.CreateMovie(movieState).ConfigureAwait(false);
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

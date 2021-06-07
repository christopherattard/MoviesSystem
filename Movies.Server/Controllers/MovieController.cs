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

		[HttpPost]
		public async Task<MovieApiData> CreateMovie([FromBody] MovieApiData movieApiData)
		{
			//Clean the key
			movieApiData.Key = movieApiData.Key.Trim().ToLower();

			var result = await _client.CreateMovie(movieApiData).ConfigureAwait(false);
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

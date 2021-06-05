using Microsoft.AspNetCore.Mvc;
using Movies.Contracts;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Server.Controllers
{
	public class MovieListController : Controller
	{ 
		private readonly IMovieListGrainClient _client;

		[Route("api/[controller]")]
		public MovieListController(IMovieListGrainClient client)
		{
			_client = client;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet("")]
		public async Task<List<MovieInfo>> GetAllMovies()
		{
			//EventApiData[] list = new EventApiData[0]; 

			logger.LogInformation($"-- GET api/movielist: get all movies");

			var movieListGrain = _client.GetGrain<IMovieListGrain>(Guid.Empty);
			EventApiData[] list = await movieListGrain.ListEvents("");

			return list;
		}
	}
}

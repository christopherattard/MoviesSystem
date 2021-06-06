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
	public class MovieListController : Controller
	{ 
		private readonly IMovieListGrainClient _client;
		
		public MovieListController(IMovieListGrainClient client)
		{
			_client = client;
		}

		[HttpGet("")]
		public async Task<List<MovieInfo>> ListMovies()
		{
			Console.WriteLine("-- GET api/movielist: get all movies");

			List<MovieInfo> list = await _client.GetAllMovies();
			return list;
		}
	}
}

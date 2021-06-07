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
		public async Task<List<MovieInfo>> GetAllMovies()
		{			
			List<MovieInfo> list = await _client.GetAllMovies();
			return list;
		}

		[HttpGet("{movieKey}")]
		public async Task<MovieApiData> GetMovieDetails(string movieKey)
		{ 
			var result = await _client.GetMovieDetails(movieKey).ConfigureAwait(false);
			return result;			
		}

		[HttpGet("genre/{genre}")]
		public async Task<List<MovieInfo>> GetMoviesByGenre(string genre)
		{		
			var result = await _client.GetMoviesByGenre(genre).ConfigureAwait(false);
			return result;
		}

		[HttpGet("search/{search}")]
		public async Task<List<MovieInfo>> GetMoviesBySearch(string search)
		{
			var result = await _client.GetMoviesBySearch(search).ConfigureAwait(false);
			return result;
		}

		[HttpGet("top/{topCount}")]
		public async Task<List<MovieInfo>> GetTopMovies(int topCount)
		{
			var result = await _client.GetTopMovies(topCount).ConfigureAwait(false);
			return result;
		}
	}
}

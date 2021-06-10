using Microsoft.Extensions.Configuration;
using Movies.Contracts;
using Movies.Core;
using Movies.Models;
using Orleans;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Movies.GrainClients
{
	public class MovieListGrainClient : IMovieListGrainClient
	{
		private readonly IGrainFactory _grainFactory;
		//private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly string _grainPrimaryKey = "";

		public MovieListGrainClient(IAppInfo appInfo, IGrainFactory grainFactory /*, IHttpContextAccessor httpContextAccessor*/)
		{
			_grainFactory = grainFactory;
			_grainPrimaryKey = appInfo.GrainPrimaryKey;

		}

		public Task AddMovie(MovieInfo movieInfo)
		{
			//string userName = HttpContext.User.Identity.Name;
			var grain = _grainFactory.GetGrain<IMovieListGrain>(_grainPrimaryKey);
			return grain.AddMovie(movieInfo);
		}

		public Task DeleteMovie(string movieId) 
		{
			var grain = _grainFactory.GetGrain<IMovieListGrain>(_grainPrimaryKey);
			return grain.DeleteMovie(movieId);
		}

		public Task<List<MovieInfo>> GetAllMovies()
		{
			var grain = _grainFactory.GetGrain<IMovieListGrain>(_grainPrimaryKey);
			return grain.GetAllMovies();
		}

		public Task<List<MovieInfo>> GetTopMovies(int topCount)
		{
			var grain = _grainFactory.GetGrain<IMovieListGrain>(_grainPrimaryKey);
			return grain.GetTopMovies(topCount);
		}

		public Task<List<MovieInfo>> GetMoviesBySearch(string search)
		{
			var grain = _grainFactory.GetGrain<IMovieListGrain>(_grainPrimaryKey);
			return grain.GetMoviesBySearch(search);
		}

		public Task<List<MovieInfo>> GetMoviesByGenre(string genre)
		{
			var grain = _grainFactory.GetGrain<IMovieListGrain>(_grainPrimaryKey);
			return grain.GetMoviesByGenre(genre);
		}

		public Task<MovieApiData> GetMovieDetails(string movieKey)
		{
			var grain = _grainFactory.GetGrain<IMovieListGrain>(_grainPrimaryKey);
			return grain.GetMovieDetails(movieKey);
		}
	}
}

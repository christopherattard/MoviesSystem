using Movies.Contracts;
using Movies.Models;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movies.GrainClients
{
	public class MovieGrainClient : IMovieGrainClient
	{
		private readonly IGrainFactory _grainFactory;

		public MovieGrainClient(IGrainFactory grainFactory)
		{
			_grainFactory = grainFactory;
		}

		public Task<MovieState> GetAllMovies()
		{
			var grain = _grainFactory.GetGrain<IMovieGrain>(id);
			return grain.GetAllMovies();
		}
		public Task<MovieState> GetTopMovies(int topCount)
		{
			var grain = _grainFactory.GetGrain<IMovieGrain>(id);
			return grain.GetTopMovies(topCount);
		}
	}
}

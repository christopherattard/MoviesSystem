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

		public Task<MovieState> CreateMovie(MovieState movieState) 
		{
			var movieGrain = _grainFactory.GetGrain<IMovieGrain>(movieState.Key);
			return movieGrain.Update(movieState);
		}
	}
}

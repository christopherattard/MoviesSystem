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

		public Task<MovieApiData> CreateMovie(MovieApiData movieApiData) 
		{
			//Clean the key
			movieApiData.Key = movieApiData.Key.Trim().ToLower();

			var movieGrain = _grainFactory.GetGrain<IMovieGrain>(movieApiData.Key);
			return movieGrain.Update(movieApiData);
		}

		public Task<MovieApiData> GetMovieDetails(string movieKey)
		{
			var movieGrain = _grainFactory.GetGrain<IMovieGrain>(movieKey);
			return movieGrain.GetMovieDetails();
		}
	}
}

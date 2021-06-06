using Movies.Contracts;
using Movies.Models;
using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Grains
{
	[StorageProvider(ProviderName = "Default")]
	public class MovieGrain : Grain<MovieState>, IMovieGrain
	{
		public Task Delete(string movieId) => throw new NotImplementedException();

		public async Task<MovieState> Update(MovieState movieState) 
		{
			string key = this.GetPrimaryKeyString();
			Console.WriteLine($"-- MovieGrain Update() for movie with key #{key} and name {movieState.Name} --");

			// update interal grain state

			State.Key = movieState.Key;
			State.Name = movieState.Name;
			State.Description = movieState.Description;
			
			State.Rate = movieState.Rate;
			State.Length = movieState.Length;
			State.Img = movieState.Img;

			// Clean the genres and store them
			List<string> cleanGenres = new List<string>();
			foreach (var genre in movieState.Genres)
			{
				if (string.IsNullOrWhiteSpace(genre))
				{
					cleanGenres.Add(genre.Trim());
				}
			}
			State.Genres = cleanGenres;

			Console.WriteLine($"-- MovieGrain Update() about to write WriteStateAsync --");
			await base.WriteStateAsync();

			// update movie list about this new movie
			MovieInfo movieInfo = new MovieInfo
			{
				Key = movieState.Key,
				Name = movieState.Name,
				Description = movieState.Description,
				Genres = movieState.Genres,
				Rate = movieState.Rate
			};

			var movieListGrain = GrainFactory.GetGrain<IMovieListGrain>("CA");  // the aggregator grain is a singleton - Guid.Empty is convention to indicate this
																				//await aggregator.DeleteAnEvent(id);  
			await movieListGrain.AddMovie(movieInfo);

			return State;
		}
	}
}
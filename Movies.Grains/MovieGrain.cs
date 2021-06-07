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
		public async Task<MovieApiData> Update(MovieApiData movieApiData) 
		{
			string key = this.GetPrimaryKeyString();
			Console.WriteLine($"-- MovieGrain Update() for movie with key #{key} and name {movieApiData.Name} --");

			// update interal grain state

			State.Key = key;
			State.Name = movieApiData.Name;
			State.Description = movieApiData.Description;
			
			State.Rate = movieApiData.Rate;
			State.Length = movieApiData.Length;
			State.Img = movieApiData.Img;

			// Clean the genres and store them
			List<string> cleanGenres = new List<string>();
			foreach (var genre in movieApiData.Genres)
			{
				if (string.IsNullOrWhiteSpace(genre))
				{
					cleanGenres.Add(genre.Trim().ToLower());
				}
			}
			State.Genres = cleanGenres;

			Console.WriteLine($"-- MovieGrain Update() about to write WriteStateAsync --");
			await base.WriteStateAsync();

			// update movie list about this new movie
			MovieInfo movieInfo = new MovieInfo
			{
				Key = movieApiData.Key,
				Name = movieApiData.Name,
				Description = movieApiData.Description,
				Genres = movieApiData.Genres,
				Rate = movieApiData.Rate
			};

			var movieListGrain = GrainFactory.GetGrain<IMovieListGrain>("CA");  // the aggregator grain is a singleton - Guid.Empty is convention to indicate this
																				//await aggregator.DeleteAnEvent(id);  
			await movieListGrain.AddMovie(movieInfo);

			return movieApiData;
		}

		public async Task<MovieApiData> GetMovieDetails() => new MovieApiData
		{
			Key = State.Key,
			Description = State.Description,
			Name = State.Name,				
			Rate = State.Rate,
			Length = State.Length,
			Img = State.Img
		};		
	}
}
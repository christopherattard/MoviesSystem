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
		public async Task<MovieState> CreateOrUpdate(MovieApiData movieApiData)
		{
			string key = this.GetPrimaryKeyString();

			// update interal grain state
			State.Key = key;
			State.Name = movieApiData.Name;
			State.Description = movieApiData.Description;

			State.Rate = movieApiData.Rate;
			State.Length = movieApiData.Length;
			State.Img = movieApiData.Img;
			
			State.Activated = true;

			// Clean the genres and store them
			List<string> cleanGenres = new List<string>();
			foreach (var genre in movieApiData.Genres)
			{
				if (!string.IsNullOrWhiteSpace(genre))
				{
					cleanGenres.Add(genre.Trim().ToLower());
				}
			}
			State.Genres = cleanGenres;

			await base.WriteStateAsync();

			return State;
		}

		public async Task<MovieInfo> GetMovieDetails() => new MovieInfo {
			Key = State.Key,
			Name = State.Name,
			Description = State.Description,
			Rate = State.Rate,
			Genres = State.Genres,
			Activated = State.Activated
		};
	}
}
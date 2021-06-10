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
		public async Task<MovieState> Update(MovieApiData movieApiData) 
		{
			string key = this.GetPrimaryKeyString();			

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
				if (!string.IsNullOrWhiteSpace(genre))
				{
					cleanGenres.Add(genre.Trim().ToLower());
				}
			}
			State.Genres = cleanGenres;
			
			await base.WriteStateAsync();

			return State;			
		}

		public async Task<MovieApiData> GetMovieDetails() => new MovieApiData
			{
				Key = State.Key,
				Description = State.Description,
				Name = State.Name,
				Rate = State.Rate,
				Length = State.Length,
				Img = State.Img,
				Genres = State.Genres				
			};		
	}
}
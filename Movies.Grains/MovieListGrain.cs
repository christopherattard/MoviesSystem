﻿using Movies.Contracts;
using Movies.Models;
using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Grains
{
	[StorageProvider(ProviderName = "Default")]
	public class MovieListGrain : Grain<MovieListState>, IMovieListGrain
	{
		public async Task AddMovie(MovieInfo movieInfo)
		{
			Console.WriteLine($"-- MovieListGrain AddMovie() for movie [{movieInfo.Name}] with key #{movieInfo.Key} --");

			// ensure movie list is not null
			if (State.MovieList == null)
			{
				State.MovieList = new List<MovieInfo>();
			}

			// check to see if movie already exists, and if so, removed, before inserting new movie info
			int index = State.MovieList.FindIndex(movie => movie.Key == movieInfo.Key);
			if (index >= 0)
			{
				State.MovieList.RemoveAt(index);
			}

			// add this event key to our active list
			State.MovieList.Add(movieInfo);

			Console.WriteLine($"-- MovieListGrain.AddMovie() - call WriteStateAsync for new movie with key #{movieInfo.Key} --");
			await base.WriteStateAsync();
			
			return;
		}

		public async Task DeleteMovie(string movieKey)
		{
			Console.WriteLine($"-- MovieListGrain.DeleteMovie() for movie with key #{movieKey} --");

			// check args
			if (string.IsNullOrWhiteSpace(movieKey))
			{
				return;
			}

			if (State.MovieList == null)
			{
				State.MovieList = new List<MovieInfo>();
			}

			// delete this event key from our active list if it exists 
			int index = State.MovieList.FindIndex(movie => movie.Key == movieKey);
			if (index >= 0)
			{
				State.MovieList.RemoveAt(index);  // remove it

				Console.WriteLine($"** -- MovieListGrain.DeleteMovie() - call WriteStateAsync for deleted movie with key #{movieKey} --");
				await base.WriteStateAsync();
			}

			return;
		}

		public async Task<List<MovieInfo>> ListMovies()
		{
			Console.WriteLine($"-- MovieListGrain.ListMovies() --");

			if (State.MovieList == null)
			{
				State.MovieList = new List<MovieInfo>();
			}

			return State.MovieList;			
		}
	}
}
using Movies.Contracts;
using Movies.Core;
using Movies.Models;
using Newtonsoft.Json;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.Server
{
	/// <summary>
	/// This startup class loads the movies.json file contents into the grain.
	/// </summary>
	public class PreloadGrainStartupTask : IStartupTask
	{
		private readonly IGrainFactory _grainFactory;
		private readonly IAppInfo _appInfo;		

		public PreloadGrainStartupTask(IAppInfo appInfo, IGrainFactory grainFactory)
		{
			_appInfo = appInfo;
			_grainFactory = grainFactory;		
		}

		public async Task Execute(CancellationToken cancellationToken)
		{
			// Check that the path is specified and that the file exists
			if (!string.IsNullOrWhiteSpace(_appInfo.MoviesPath) && File.Exists(_appInfo.MoviesPath))
			{
				var fileContents = File.ReadAllText(_appInfo.MoviesPath);
				// Check if the file has any contents
				if (!string.IsNullOrWhiteSpace(fileContents))
				{
					var movieList = JsonConvert.DeserializeObject<Root>(fileContents);

					if (movieList != null && movieList.movies != null)
					{
						foreach (var movieState in movieList.movies)
						{
							var movieStateGenres = new List<string>();
							movieStateGenres.AddRange(movieState.Genres);

							var movieGrain = _grainFactory.GetGrain<IMovieGrain>(movieState.Key);							
							await movieGrain.Update(new MovieApiData
							{
								Key = movieState.Key,
								Name = movieState.Name,
								Description = movieState.Description,
								Genres = movieStateGenres,
								Rate = movieState.Rate,
								Length = movieState.Length,
								Img = movieState.Img
							});

							// update movie list about this new movie
							MovieInfo movieInfo = new MovieInfo
							{
								Key = movieState.Key,
								Name = movieState.Name,
								Description = movieState.Description,
								Genres = movieState.Genres,
								Rate = movieState.Rate
							};

							var movieListGrain = _grainFactory.GetGrain<IMovieListGrain>(_appInfo.GrainPrimaryKey);
							await movieListGrain.AddMovie(movieInfo);
						}
					}
				}
				else
				{
					Console.WriteLine("WARNING - check if movies file has content.");
				}
			}
			else
			{
				Console.WriteLine("WARNING - check if movies file is specified in the moviesPath key in config and check if the file exists.");
			}
		}
	}
}

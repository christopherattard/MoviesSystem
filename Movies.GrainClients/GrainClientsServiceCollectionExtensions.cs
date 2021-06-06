﻿using Microsoft.Extensions.DependencyInjection;
using Movies.Contracts;

namespace Movies.GrainClients
{
	public static class GrainClientsServiceCollectionExtensions
	{
		public static void AddAppClients(this IServiceCollection services)
		{
			services.AddSingleton<ISampleGrainClient, SampleGrainClient>();
			services.AddSingleton<IMovieListGrainClient, MovieListGrainClient>();
			services.AddScoped<IMovieGrainClient, MovieGrainClient>();
		}
	}
}
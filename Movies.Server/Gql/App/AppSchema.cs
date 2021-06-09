using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Movies.Server.Gql.App
{
	public class AppSchema : Schema
	{
		public AppSchema(IServiceProvider provider)
			: base(provider)
		{
			Query = provider.GetRequiredService<AppGraphQuery>();
			Mutation = provider.GetRequiredService<AppGraphMutation>();
		}
	}
}
using GraphQL.Types;
using Movies.Contracts;
using Movies.Models;
using Movies.Server.Gql.Types;

namespace Movies.Server.Gql.App
{
	public class AppGraphMutation : ObjectGraphType
	{
		public AppGraphMutation(IMovieGrainClient client)
		{
			Field<MovieApiDataGraphType>("createmovie",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<MovieInputGraphType>> { Name = "movie" }),
				resolve: context =>
				{
					var movieApiData = (MovieApiData)context.Arguments["movie"];
					return client.CreateMovie(movieApiData);
				});
		}
	}
}
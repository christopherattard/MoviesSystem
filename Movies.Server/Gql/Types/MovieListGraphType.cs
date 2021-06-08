using GraphQL.Types;
using Movies.Contracts;
using Movies.Models;

namespace Movies.Server.Gql.Types
{
	public class MovieListGraphType : ObjectGraphType<MovieListState>
	{
		public MovieListGraphType(IMovieListGrainClient client)
		{
			Field<ListGraphType<MovieInfoGraphType>>("movies",
				resolve: context => client.GetAllMovies());
		}
	}
}
using GraphQL.Types;
using Movies.Contracts;
using Movies.Server.Gql.Types;

namespace Movies.Server.Gql.App
{
	public class AppGraphQuery : ObjectGraphType
	{
		//public AppGraphQuery(ISampleGrainClient sampleClient)
		//{
		//	Name = "AppQueries";

		//	Field<SampleDataGraphType>("sample",
		//		arguments: new QueryArguments(new QueryArgument<StringGraphType>
		//		{
		//			Name = "id"
		//		}),
		//		resolve: ctx => sampleClient.Get(ctx.Arguments["id"].ToString())
		//	);
		//}

		public AppGraphQuery(IMovieListGrainClient client)
		{
			Name = "AppQueries";

			Field<ListGraphType<MovieInfoGraphType>>("getallmovies",
				resolve: ctx => client.GetAllMovies()
			);

			/*{
				getallmovies{
								key,
								name,
								description,
								rate
							}
			}*/

			Field<ListGraphType<MovieInfoGraphType>>("gettopmovies",
				arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "topcount" }),
				resolve: context => client.GetTopMovies((int)context.Arguments["topcount"])
				);

			/*{
				gettopmovies(topcount: 3){
											key,
											name,
											description,
											rate
										}
			}*/
		}



	}
}

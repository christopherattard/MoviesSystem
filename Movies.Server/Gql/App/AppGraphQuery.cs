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

		public AppGraphQuery(IMovieGrainClient movieGrainClient, IMovieListGrainClient movieListGrainClient)
		{
			Name = "AppQueries";

			Field<ListGraphType<MovieInfoGraphType>>("getallmovies",
				resolve: ctx => movieListGrainClient.GetAllMovies()
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
				resolve: context => movieListGrainClient.GetTopMovies((int)context.Arguments["topcount"])
				);

			/*{
				gettopmovies(topcount: 3){
											key,
											name,
											description,
											rate
										}
			}*/			
			
			Field<ListGraphType<MovieInfoGraphType>>("getmoviesbysearch",
				arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "search" }),
				resolve: context => movieListGrainClient.GetMoviesBySearch(context.Arguments["search"].ToString())
				);

			/*{
				getmoviesbysearch(search: "gang,gangster"){
															key,
															name,
															description,
															rate
														}
			}*/

			Field<ListGraphType<MovieInfoGraphType>>("getmoviesbygenre",
				arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "genre" }),
				resolve: context => movieListGrainClient.GetMoviesByGenre(context.Arguments["genre"].ToString())
				);

			/*{
				getmoviesbygenre(genre: "crime,drama"){
														key,
														name,
														description,
														rate
													}
			}*/

			Field<MovieApiDataGraphType>("getmoviedetails",
				arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "moviekey" }),
				resolve: context => movieGrainClient.GetMovieDetails(context.Arguments["moviekey"].ToString())
				);

			/*{
				getmoviedetails(moviekey: "mission-impossible-rogue-nation"){
																				key,
																				name,
																				description,
																				rate,
																				length,
																				img
																			}
			}*/

		}



	}
}

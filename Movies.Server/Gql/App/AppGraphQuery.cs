using GraphQL;
using GraphQL.Types;
using Movies.Contracts;
using Movies.Server.Gql.Types;

namespace Movies.Server.Gql.App
{
	public class AppGraphQuery : ObjectGraphType
	{	
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
								rate,
								genres
							}
			}*/

			Field<ListGraphType<MovieInfoGraphType>>("gettopmovies",
				arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "topcount" }),
				resolve: context => movieListGrainClient.GetTopMovies(context.GetArgument<int>("topcount"))
				);

			/*{
				gettopmovies(topcount: 3){
											key,
											name,
											description,
											rate,
											genres
										}
			}*/			
			
			Field<ListGraphType<MovieInfoGraphType>>("getmoviesbysearch",
				arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "search" }),
				resolve: context => movieListGrainClient.GetMoviesBySearch(context.GetArgument<string>("search"))
				);

			/*{
				getmoviesbysearch(search: "gang,gangster"){
															key,
															name,
															description,
															rate,
															genres
														}
			}*/

			Field<ListGraphType<MovieInfoGraphType>>("getmoviesbygenre",
				arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "genre" }),
				resolve: context => movieListGrainClient.GetMoviesByGenre(context.GetArgument<string>("genre"))
				);

			/*{
				getmoviesbygenre(genre: "crime,drama"){
														key,
														name,
														description,
														rate,
														genres
													}
			}*/

			Field<MovieApiDataGraphType>("getmoviedetails",
				arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "moviekey" }),
				resolve: context => movieGrainClient.GetMovieDetails(context.GetArgument<string>("moviekey"))
				);

			/*{
				getmoviedetails(moviekey: "mission-impossible-rogue-nation"){
																				key,
																				name,
																				description,
																				genres,
																				rate,
																				length,
																				img
																			}
			}*/

		}



	}
}

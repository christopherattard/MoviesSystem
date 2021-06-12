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

			Field<ListGraphType<MovieApiDataGraphType>>("getallmovies",
				resolve: ctx => movieListGrainClient.GetAllMovies()
			);

			/*{
				getallmovies{
								key,
								name,
								description,
								rate,
								genres,
								errors
							}
			}*/

			Field<ListGraphType<MovieApiDataGraphType>>("gettopmovies",
				arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "topcount" }),
				resolve: context => movieListGrainClient.GetTopMovies(context.GetArgument<int>("topcount"))
				);

			/*{
				gettopmovies(topcount: 3){
											key,
											name,
											description,
											rate,
											genres,
											errors
										}
			}*/

			Field<ListGraphType<MovieApiDataGraphType>>("getmoviesbysearch",
				arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "search" }),
				resolve: context => movieListGrainClient.GetMoviesBySearch(context.GetArgument<string>("search"))
				);

			/*{
				getmoviesbysearch(search: "gang,gangster"){
															key,
															name,
															description,
															rate,
															genres,
															errors
														}
			}*/

			Field<ListGraphType<MovieApiDataGraphType>>("getmoviesbygenre",
				arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "genres" }),
				resolve: context => movieListGrainClient.GetMoviesByGenre(context.GetArgument<string>("genres"))
				);

			/*{
				getmoviesbygenre(genres: "crime,drama"){
														key,
														name,
														description,
														rate,
														genres,
														errors
													}
			}*/

			Field<MovieApiDataGraphType>("getmoviedetails",
				arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "moviekey" }),
				resolve: context => movieListGrainClient.GetMovieDetails(context.GetArgument<string>("moviekey"))
				);

			/*{
				getmoviedetails(moviekey: "mission-impossible-rogue-nation"){
																				key,
																				name,
																				description,
																				genres,
																				rate,
																				length,
																				img,
																				errors
																			}
			}*/

		}



	}
}

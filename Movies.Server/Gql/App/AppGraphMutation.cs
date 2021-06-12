using GraphQL;
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
					var movieApiData = context.GetArgument<MovieApiData>("movie");
					return client.CreateMovie(movieApiData);
				});

			/*mutation ($movie : MovieInput!){
					createmovie(movie: $movie){
												key,
												name,
												description,
												genres,
												rate,
												length,
												img,
												errors
											}
				}			
			 
			{
				"movie" : {
  							"key" : "new-film-1",
							"name" :"New Film 1",
							"description" : "The first film",
							"genres" : ["action", "romantic"],
							"rate" : 6.0,
							"length" : "1hr 40mins",
							"img" : "noimage.jpg"
							}  
			}*/


			Field<MovieApiDataGraphType>("updatemovie",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<MovieInputGraphType>> { Name = "movie" }),
				resolve: context =>
				{
					var movieApiData = context.GetArgument<MovieApiData>("movie");
					return client.UpdateMovie(movieApiData);
				});

			/*mutation ($movie : MovieInput!){
					updatemovie(movie: $movie){
												key,
												name,
												description,
												genres,
												rate,
												length,
												img,
												errors
											}
				}			
			 
			{
				"movie" : {
  							"key" : "new-film-1",
							"name" :"New Film 1 (modified)",
							"description" : "The first film (modified)",
							"genres" : ["thriller", "comedy"],
							"rate" : 6.2,
							"length" : "1hr 40mins",
							"img" : "noimage.jpg"
							}  
			}*/
		}
	}
}
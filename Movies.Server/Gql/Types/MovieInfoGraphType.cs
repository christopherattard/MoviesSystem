using GraphQL.Types;
using Movies.Contracts;
using Movies.Models;

namespace Movies.Server.Gql.Types
{
	public class MovieInfoGraphType : ObjectGraphType<MovieInfo>
	{
		public MovieInfoGraphType()
		{
			Field(x => x.Key).Description("Movie key.");
			Field(x => x.Name).Description("Movie name.");
			Field(x => x.Description, nullable: true).Description("Movie description.");
			Field(x => x.Rate).Description("Movie rating.");			
			//Field(x=> x.Genres)
		}
	}
}
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
			Field<ListGraphType<StringGraphType>>("genres", "List of genres.");
			Field(x => x.Description).Description("Movie description.");
			Field(x => x.Rate).Description("Movie rating.");			
		}
	}
}
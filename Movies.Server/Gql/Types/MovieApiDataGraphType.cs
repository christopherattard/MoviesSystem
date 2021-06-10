using GraphQL.Types;
using Movies.Models;

namespace Movies.Server.Gql.Types
{
	public class MovieApiDataGraphType : ObjectGraphType<MovieApiData>
	{
		public MovieApiDataGraphType()
		{
			Field(x => x.Key).Description("Movie key.");
			Field(x => x.Name).Description("Movie name.");
			Field(x => x.Description).Description("Movie description.");
			Field<ListGraphType<StringGraphType>>("genres", "List of genres.");
			Field(x => x.Rate).Description("Movie rating.");
			Field(x => x.Length).Description("Movie length.");
			Field(x => x.Img).Description("Movie image.");

		}
	}
}

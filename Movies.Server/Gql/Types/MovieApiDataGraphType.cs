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
			Field(x => x.Description, nullable: true).Description("Movie description.");
			Field<ListGraphType<StringGraphType>>("genres", "List of genres.");
			Field(x => x.Rate, nullable: true).Description("Movie rating.");
			Field(x => x.Length, nullable: true).Description("Movie length.");
			Field(x => x.Img, nullable: true).Description("Movie image.");
			Field(x => x.ErrorMessage, nullable: true).Description("Errors encountered during API operations.");
		}
	}
}

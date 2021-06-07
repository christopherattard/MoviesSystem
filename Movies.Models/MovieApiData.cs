using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Models
{
	public class MovieApiData
	{
		public string Key { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<string> Genres { get; set; }
		public decimal Rate { get; set; }
		public string Length { get; set; }
		public string Img { get; set; }
	}
}

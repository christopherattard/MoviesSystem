using Movies.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using Xunit;

namespace Movies.Tests
{
    public class PreloadGrainStartupTaskTests
    {
		private const string MOVIES_PATH = @"C:\\Projects\\MoviesSystem\\movies.json";

		[Fact]
        public void MoviesFile_FileExists_True()
        {
			Assert.True(File.Exists(MOVIES_PATH));
        }

		[Fact]
		public void MoviesFile_FileIsNotEmpty_True()
		{
			var fileContents = File.ReadAllText(MOVIES_PATH);

			Assert.True(!string.IsNullOrWhiteSpace(fileContents));
		}

		[Fact]
		public void MoviesFile_FileIsValid_True()
		{
			var fileContents = File.ReadAllText(MOVIES_PATH);
			var movieList = JsonConvert.DeserializeObject<Root>(fileContents);

			Assert.NotNull(movieList);
		}
	}
}

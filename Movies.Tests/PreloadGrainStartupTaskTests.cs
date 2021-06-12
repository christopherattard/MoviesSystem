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
			Assert.True(File.Exists(MOVIES_PATH));

			var fileContents = File.ReadAllText(MOVIES_PATH);

			Assert.True(!string.IsNullOrWhiteSpace(fileContents));
		}
	}
}

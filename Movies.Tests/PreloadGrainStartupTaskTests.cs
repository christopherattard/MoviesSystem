using System;
using System.IO;
using Xunit;

namespace Movies.Tests
{
    public class PreloadGrainStartupTaskTests
    {
        [Fact]
        public void JSONFile_FileExists_True()
        {
			Assert.True(File.Exists(@"C:\\Projects\\MoviesSystem\\movies.json"));
        }
    }
}

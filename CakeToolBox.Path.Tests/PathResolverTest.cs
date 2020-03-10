using Cake.Core;
using Moq;
using Xunit;

namespace CakeToolBox.Path.Tests
{
    public class PathResolverTest
    {
        private readonly ICakeContext _context = new Mock<ICakeContext>().Object;

        [Fact]
        public void ResolvePathTest()
        {
            Assert.Equal("demo\\demo", _context.ResolvePath("demo", "demo"));
        }
    }
}

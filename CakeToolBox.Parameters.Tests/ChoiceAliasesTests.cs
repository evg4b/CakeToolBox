namespace CakeToolBox.Parameters.Tests
{
    using Cake.Core;
    using Exceptions;
    using Moq;
    using System;
    using Xunit;

    public class ChoiceAliasesTests
    {
        private readonly string[] cases = { "param0", "param1", "param2", "param3", "param4" };

        [Theory]
        [InlineData("param", "param")]
        [InlineData("param", "demo", "param", "param44", "666")]
        [InlineData("param1", "param1")]
        [InlineData("param3", "demo4", "demo", "param3")]
        public void ShouldReturnCorrectCase(string argument, params string[] cases)
        {
            var argumentsMock = new Mock<ICakeArguments>(MockBehavior.Strict);
            argumentsMock.Setup(p => p.HasArgument(It.IsAny<string>()))
                .Returns<string>(param => param.Equals(argument, StringComparison.InvariantCultureIgnoreCase));

            var result = GetContext(argumentsMock).Choice(cases);
            
            Assert.Equal(argument, result);
        }

        [Fact]
        public void ShouldThrowErrorWhereCaseNotFound()
        {
            var argumentsMock = new Mock<ICakeArguments>(MockBehavior.Strict);
            argumentsMock.Setup(p => p.HasArgument(It.IsAny<string>()))
                .Returns(false);

            Assert.Throws<CaseNotFoundException>(() => GetContext(argumentsMock).Choice(cases));
        }

        [Fact]
        public void ShouldReturnDefaultValueWhereItSpecifiedAndCaseNotFound()
        {
            const string defaultValue = "defaultValue";

            var argumentsMock = new Mock<ICakeArguments>(MockBehavior.Strict);
            argumentsMock.Setup(p => p.HasArgument(It.IsAny<string>()))
                .Returns(false);

            Assert.Equal(defaultValue, GetContext(argumentsMock).Choice(cases, defaultValue));
        }

        [Fact]
        public void ShouldThrowErrorWhenMoreThanOneCaseSpecified()
        {
            var argumentsMock = new Mock<ICakeArguments>(MockBehavior.Strict);
            argumentsMock.Setup(p => p.HasArgument(It.IsAny<string>()))
                .Returns(true);

            Assert.Throws<MoreThanOneCaseSpecifiedException>(() => GetContext(argumentsMock).Choice(cases));
        }

        [Fact]
        public void ShouldNotThrowErrorWhenDefaultValueIsNull()
        {
            var argumentsMock = new Mock<ICakeArguments>(MockBehavior.Strict);
            argumentsMock.Setup(p => p.HasArgument(It.IsAny<string>()))
                .Returns(false);

            Assert.Null(GetContext(argumentsMock).Choice(cases, null));
        }

        [Fact]
        public void ShouldThrowErrorWhenOptionsAreNotUnique()
        {
            var argument = "case1";
            var cases = new string[] { argument, argument, "case2", "case3" };

            var argumentsMock = new Mock<ICakeArguments>(MockBehavior.Strict);
            argumentsMock.Setup(p => p.HasArgument(It.IsAny<string>()))
                .Returns<string>(p => argument.Equals(p, StringComparison.InvariantCultureIgnoreCase));

            Assert.Throws<NotUniqueCaseException>(() => GetContext(argumentsMock).Choice(cases));
        }

        private ICakeContext GetContext(Mock<ICakeArguments> argumentsMock)
        {
            var contextMock = new Mock<ICakeContext>(MockBehavior.Strict);
            contextMock.Setup(p => p.Arguments)
                .Returns(argumentsMock.Object);

            return contextMock.Object;
        }
    }
}

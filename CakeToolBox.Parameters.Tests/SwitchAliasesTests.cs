namespace CakeToolBox.Parameters.Tests
{
    using Cake.Core;
    using CakeToolBox.Parameters.Exceptions;
    using Moq;
    using System;
    using Xunit;

    public class SwitchTests
    {
        private string[] cases = new string[] { "param0", "param1", "param2", "param3", "param4" };

        [Theory]
        [InlineData("param", "param")]
        [InlineData("param", "demo", "param", "param44", "666")]
        [InlineData("param1", "param1")]
        [InlineData("param3", "demo4", "demo", "param3")]
        public void ShoudReturnCorrectCase(string argument, params string[] cases)
        {
            var argumentsMock = new Mock<ICakeArguments>(MockBehavior.Strict);
            argumentsMock.Setup(p => p.HasArgument(It.IsAny<string>()))
                .Returns<string>(param => param.Equals(argument, System.StringComparison.InvariantCultureIgnoreCase));

            var result = SwitchAliases.Switch(GetContext(argumentsMock), cases);
            
            Assert.Equal(argument, result);
        }

        [Fact]
        public void ShoudThrowErrorWhereCaseNotFound()
        {
            var argumentsMock = new Mock<ICakeArguments>(MockBehavior.Strict);
            argumentsMock.Setup(p => p.HasArgument(It.IsAny<string>()))
                .Returns(false);

            Assert.Throws<CaseNotFoundException>(() => SwitchAliases.Switch(GetContext(argumentsMock), cases));
        }

        [Fact]
        public void ShoudReturnDefaultValueWhereItSpecifiedAndCaseNotFound()
        {
            const string defaultValue = "defaultValue";

            var argumentsMock = new Mock<ICakeArguments>(MockBehavior.Strict);
            argumentsMock.Setup(p => p.HasArgument(It.IsAny<string>()))
                .Returns(false);

            Assert.Equal(defaultValue, SwitchAliases.Switch(GetContext(argumentsMock), cases, defaultValue));
        }

        [Fact]
        public void ShoudThrowErrorWhenMoreThanOneCaseSpecified()
        {
            var argumentsMock = new Mock<ICakeArguments>(MockBehavior.Strict);
            argumentsMock.Setup(p => p.HasArgument(It.IsAny<string>()))
                .Returns(true);

            Assert.Throws<MoreThanOneCaseSpecifiedException>(() => SwitchAliases.Switch(GetContext(argumentsMock), cases));
        }

        [Fact]
        public void ShoudNotThrowErrorWhenDefaultValueIsNull()
        {
            var argumentsMock = new Mock<ICakeArguments>(MockBehavior.Strict);
            argumentsMock.Setup(p => p.HasArgument(It.IsAny<string>()))
                .Returns(false);

            Assert.Null(SwitchAliases.Switch(GetContext(argumentsMock), cases, null));
        }

        [Fact]
        public void ShoudThrowErrorWhenOptionsAreNotUnique()
        {
            var argument = "case1";
            var cases = new string[] { argument, argument, "case2", "case3" };

            var argumentsMock = new Mock<ICakeArguments>(MockBehavior.Strict);
            argumentsMock.Setup(p => p.HasArgument(It.IsAny<string>()))
                .Returns<string>(p => argument.Equals(p, StringComparison.InvariantCultureIgnoreCase));

            Assert.Throws<NotUniqueCaseException>(() => SwitchAliases.Switch(GetContext(argumentsMock), cases));
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

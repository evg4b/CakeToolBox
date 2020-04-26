namespace CakeToolBox.Parameters.Tests
{
    using Cake.Core;
    using Moq;
    using Xunit;
    using Aliases;

    public class ParameterAliasesTests
    {
        [Theory]
        [InlineData("ThisIsParameter", "CAKE_THIS_IS_PARAMETER")]
        [InlineData("Parameter1", "CAKE_PARAMETER1")]
        public void ShouldCallParameterConfiguratorCorrectly(string argument, string envVarName)
        {
            var (context, argumentsMock, environmentMock) = GetContext();

            Assert.Throws<CakeException>(() => context.Parameter<int>(argument));

            argumentsMock.Verify(p => p.HasArgument(argument));
            environmentMock.Verify(p => p.GetEnvironmentVariable(envVarName));

        }

        [Theory]
        [InlineData("ThisIsParameter", "CAKE_THIS_IS_PARAMETER")]
        [InlineData("Parameter1", "CAKE_PARAMETER1")]
        public void ShouldCallParameterConfiguratorCorrectlyWithDefaultValue(string argument, string envVarName)
        {
            var expected = 6788;
            var (context, argumentsMock, environmentMock) = GetContext();

            var result = context.Parameter(argument, expected);

            argumentsMock.Verify(p => p.HasArgument(argument));
            environmentMock.Verify(p => p.GetEnvironmentVariable(envVarName));

            Assert.Equal(expected, result);
        }

        private (ICakeContext, Mock<ICakeArguments>, Mock<ICakeEnvironment>) GetContext()
        {
            var argumentsMock = new Mock<ICakeArguments>(MockBehavior.Strict);
            argumentsMock.Setup(p => p.HasArgument(It.IsAny<string>()))
                .Returns(false);

            var environmentMock = new Mock<ICakeEnvironment>(MockBehavior.Strict);
            environmentMock.Setup(p => p.GetEnvironmentVariable(It.IsAny<string>()))
                .Returns((string)null);

            var contextMock = new Mock<ICakeContext>(MockBehavior.Strict);

            contextMock.Setup(p => p.Arguments)
                .Returns(argumentsMock.Object);

            contextMock.Setup(p => p.Environment)
                .Returns(environmentMock.Object);

            return (contextMock.Object, argumentsMock, environmentMock);
        }
    }
}

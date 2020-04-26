using System;
using System.Collections.Generic;
using Cake.Core;
using CakeToolBox.Parameters.ParameterConfiguration;
using Moq;
using Xunit;

namespace CakeToolBox.Parameters.Tests.ParameterConfiguration
{
    public class ParameterConfiguratorTests
    {
        private const string Prefix = "CAKE";

        [Fact]
        public void ShouldReturnCorrectType()
        {
            var result = ParameterConfigurator.BuildRequired<int>(
                GetCakeArguments(new Dictionary<string, string>()),
                GetCakeEnvironment(new Dictionary<string, string>()),
                Prefix);
            Assert.IsType<Func<string, int>>(result);
        }

        [Fact]
        public void ShouldReturnCorrectValueWithoutConvert()
        {
            var expected = "expected";
            var paramName = "Test";
            var result = ParameterConfigurator.BuildRequired<string>(
                GetCakeArguments(new Dictionary<string, string>
                {
                    {paramName, expected},
                }),
                GetCakeEnvironment(new Dictionary<string, string>()),
                Prefix);

            Assert.Equal(expected, result(paramName));
        }

        [Fact]
        public void ShouldReturnCorrectValueWithConvert()
        {
            var expected = 33213;
            var paramName = "Test";
            var result = ParameterConfigurator.BuildRequired<int>(
                GetCakeArguments(new Dictionary<string, string>
                {
                    {paramName, expected.ToString()},
                }),
                GetCakeEnvironment(new Dictionary<string, string>()),
                Prefix);

            Assert.Equal(expected, result(paramName));
        }

        [Fact]
        public void ShouldReturnValueFromArgumentFirstly()
        {
            var expected = 33213;
            var paramName = "Test";
            var result = ParameterConfigurator.BuildRequired<int>(
                GetCakeArguments(new Dictionary<string, string>
                {
                    {paramName, expected.ToString()},
                }),
                GetCakeEnvironment(new Dictionary<string, string>
                {
                    {$"{Prefix}_{paramName.ToUpperInvariant()}", expected.ToString()},
                }),
                Prefix);

            Assert.Equal(expected, result(paramName));
        }

        [Fact]
        public void ShouldReturnValueFromEnvironmentVariableWhenArgumentNotSet()
        {
            var expected = 33213;
            var paramName = "Test";
            var result = ParameterConfigurator.BuildRequired<int>(
                GetCakeArguments(new Dictionary<string, string>()),
                GetCakeEnvironment(new Dictionary<string, string>
                {
                    {$"{Prefix}_{paramName.ToUpperInvariant()}", expected.ToString()},
                }),
                Prefix);

            Assert.Equal(expected, result(paramName));
        }

        [Theory]
        [InlineData("Test", "CAKE", "CAKE_TEST")]
        [InlineData("TestDemo", "CAKE", "CAKE_TEST_DEMO")]
        [InlineData("Test4", "CAKE", "CAKE_TEST4")]
        [InlineData("Test", "CustomPrefix", "CUSTOM_PREFIX_TEST")]
        public void ShouldFindCorrectEnvironmentVariableNameWithPrefix(string paramName, string prefix, string expectedVarName)
        {
            var expected = 33213;
            var result = ParameterConfigurator.BuildRequired<int>(
                GetCakeArguments(new Dictionary<string, string>()),
                GetCakeEnvironment(new Dictionary<string, string>
                {
                    {expectedVarName, expected.ToString()},
                }),
                prefix);

            Assert.Equal(expected, result(paramName));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void ShouldThrowWherePrefixIsIncorrect(string prefix)
        {
            Assert.ThrowsAny<Exception>(() => ParameterConfigurator.BuildRequired<int>(
                GetCakeArguments(new Dictionary<string, string>()),
                GetCakeEnvironment(new Dictionary<string, string>()),
                prefix));
        }

        private ICakeEnvironment GetCakeEnvironment(IDictionary<string, string> variables)
        {
            var environmentMock = new Mock<ICakeEnvironment>(MockBehavior.Loose);
            environmentMock.Setup(p => p.GetEnvironmentVariable(It.IsAny<string>()))
                .Returns<string>(key => variables.ContainsKey(key)
                    ? variables[key]
                    : null);
            return environmentMock.Object;
        }

        private ICakeArguments GetCakeArguments(IDictionary<string, string> arguments)
        {
            var argumentsMock = new Mock<ICakeArguments>(MockBehavior.Loose);
            argumentsMock.Setup(p => p.HasArgument(It.IsAny<string>()))
                .Returns<string>(arguments.ContainsKey);
            argumentsMock.Setup(p => p.GetArgument(It.IsAny<string>()))
                .Returns<string>(key => arguments.ContainsKey(key)
                    ? arguments[key]
                    : null);
            return argumentsMock.Object;
        }
    }
}
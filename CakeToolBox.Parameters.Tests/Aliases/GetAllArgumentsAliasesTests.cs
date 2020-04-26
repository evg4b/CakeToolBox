namespace CakeToolBox.Parameters.Tests.Aliases
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cake.Core;
    using Cake.Core.Diagnostics;
    using CakeToolBox.Parameters.Aliases;
    using Moq;
    using Xunit;

    class FakeCakeArguments : ICakeArguments
    {
        private readonly Dictionary<string, string> _arguments;

        public FakeCakeArguments(Dictionary<string, string> dictionary)
        {
            _arguments = dictionary;
        }

        public IReadOnlyDictionary<string, string> Arguments => _arguments;

        public bool HasArgument(string name) => _arguments.ContainsKey(name);

        public string GetArgument(string name) => _arguments[name];
    }

    public class GetAllArgumentsAliasesTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void ShouldReturnAllArguments(Dictionary<string, string> arguments)
        {
            var (context, _) = GetContext(arguments);

            var result = context.GetAllArguments();

            Assert.Equal(arguments, result);
        }

        [Fact]
        public void ShouldWarnWhereGetAllArgumentsCalledByDefault()
        {
            var (context, loggerMock) = GetContext();

            context.GetAllArguments();

            VerifyLogCall(loggerMock, Times.Once());
        }

        [Fact]
        public void ShouldNotWarnWhereGetAllArgumentsCalledWithSuppressParameter()
        {
            var (context, loggerMock) = GetContext();

            context.GetAllArguments(true);

            VerifyLogCall(loggerMock, Times.Never());
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void ShouldReturnAllArgumentsNames(Dictionary<string, string> arguments)
        {
            var expected = arguments.Select(p => p.Key).ToList();
            var (context, _) = GetContext(arguments);

            var result = context.GetAllArgumentsNames();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ShouldWarnWhereGetAllArgumentsNamesCalledByDefault()
        {
            var (context, loggerMock) = GetContext();

            context.GetAllArgumentsNames();

            VerifyLogCall(loggerMock, Times.Once());
        }

        [Fact]
        public void ShouldNotWarnWhereGetAllArgumentsNamesCalledWithSuppressParameter()
        {
            var (context, loggerMock) = GetContext();

            context.GetAllArgumentsNames(true);

            VerifyLogCall(loggerMock, Times.Never());
        }

        private static (ICakeContext, Mock<ICakeLog>) GetContext(Dictionary<string, string> dictionary = null)
        {
            var fakeCakeArguments = new FakeCakeArguments(dictionary ?? new Dictionary<string, string>());

            var loggerMock = new Mock<ICakeLog>();
            loggerMock.Setup(p =>
                p.Write(It.IsAny<Verbosity>(), It.IsAny<LogLevel>(), It.IsAny<string>(), It.IsAny<object[]>()));

            var contextMock = new Mock<ICakeContext>();
            contextMock.Setup(p => p.Arguments)
                .Returns(fakeCakeArguments);

            contextMock.Setup(p => p.Log)
                .Returns(loggerMock.Object);

            return (contextMock.Object, loggerMock);
        }

        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                CreateTestItem(new Dictionary<string, string>
                {
                    {"Param1", "Param1Value" },
                    {"Param2", "Param2Value" },
                    {"Param3", "Param3Value" },
                    {"Param4", "Param4Value" },
                }),
                CreateTestItem(new Dictionary<string, string>
                {
                    {Guid.NewGuid().ToString(), Guid.NewGuid().ToString()},
                    {Guid.NewGuid().ToString(), Guid.NewGuid().ToString()},
                    {Guid.NewGuid().ToString(), Guid.NewGuid().ToString()},
                    {Guid.NewGuid().ToString(), Guid.NewGuid().ToString()},
                    {Guid.NewGuid().ToString(), Guid.NewGuid().ToString()},
                }),
                CreateTestItem(new Dictionary<string, string>()),
            };

        private static object[] CreateTestItem(Dictionary<string, string> dictionary)
        {
            return new object[] { dictionary };
        }

        private static void VerifyLogCall(Mock<ICakeLog> loggerMock, Times times)
        {
            loggerMock.Verify(p => p.Write(Verbosity.Normal, LogLevel.Warning, It.IsAny<string>(), It.IsAny<object[]>()),
                times);
        }
    }
}
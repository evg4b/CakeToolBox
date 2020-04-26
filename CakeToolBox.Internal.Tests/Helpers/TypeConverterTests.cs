namespace CakeToolBox.Internal.Tests.Helpers
{
    using System;
    using CakeToolBox.Internal.Helpers;
    using Xunit;

    public class TypeConverterTests
    {
        [Theory]
        [InlineData(typeof(int), "1", 1)]
        [InlineData(typeof(int), "122", 122)]
        [InlineData(typeof(int), "-1223", -1223)]
        [InlineData(typeof(double), "1", 1.0)]
        [InlineData(typeof(double), "1023", 1023.0)]
        [InlineData(typeof(double), "-1", -1.00)]
        [InlineData(typeof(string), "1", "1")]
        [InlineData(typeof(string), "ThisIsParameter", "ThisIsParameter")]
        [InlineData(typeof(float), "1", 1.0f)]
        [InlineData(typeof(float), "122.23", 122.23f)]
        [InlineData(typeof(float), "-1.23", -1.23f)]
        public void ShouldConvertToCorrectType(Type type, string value, object dd)
        {
            var result = TypeConverter.ConvertTo(type, value);
            
            Assert.IsType(type, result);
            Assert.Equal(result, dd);
        }
    }
}
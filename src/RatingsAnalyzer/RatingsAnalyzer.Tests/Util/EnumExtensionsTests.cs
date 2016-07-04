using System;
using System.ComponentModel;
using RatingsAnalyzer.Util;
using Xunit;

namespace RatingsAnalyzer.Tests.Util
{
    public class EnumExtensionsTests
    {
        private enum TestEnum
        {
            [Description("Description")]
            Value
        }

        [Fact]
        public void TestValueConversion()
        {
            Assert.Equal(TestEnum.Value, "Value".ConvertToEnum<TestEnum>());
        }

        [Fact]
        public void TestDescriptionConversion()
        {
            Assert.Equal(TestEnum.Value, "Description".ConvertToEnum<TestEnum>());
        }

        [Fact]
        public void TestNonEnumConversion()
        {
            Assert.Throws<InvalidOperationException>(() => "Value".ConvertToEnum<int>());
        }

        [Fact]
        public void TestInvalidValue()
        {
            Assert.Throws<InvalidOperationException>(() => "OtherValue".ConvertToEnum<TestEnum>());
        }
    }
}

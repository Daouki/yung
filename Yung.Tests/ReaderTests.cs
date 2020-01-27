using Xunit;
using Yung.AST;
using Yung.Exceptions;

namespace Yung.Tests
{
    public class ReaderTests
    {
        [Theory]
        [InlineData("#t")]
        [InlineData("#f")]
        public void Read_Boolean_HasCorrectType(string input)
        {
            var actual = Reader.Read(input);
            Assert.IsType<Boolean>(actual);
        }

        [Theory]
        [InlineData("#t", true)]
        [InlineData("#f", false)]
        public void Read_Boolean_HasCorrectValue(string input, bool expected)
        {
            var actual = Reader.Read(input);
            Assert.Equal(expected, ((Boolean) actual).Value);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("1")]
        [InlineData("2137")]
        [InlineData("-9223372036854775808")]
        [InlineData("9223372036854775807")]
        public void Read_Integer_HasCorrectType(string input)
        {
            var actual = Reader.Read(input);
            Assert.IsType<Integer>(actual);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("-1", -1)]
        [InlineData("1", 1)]
        [InlineData("2137", 2137)]
        [InlineData("-9223372036854775808", long.MinValue)]
        [InlineData("9223372036854775807", long.MaxValue)]
        public void Read_Integer_HasCorrectValue(string input, long expected)
        {
            var actual = (Integer) Reader.Read(input);
            Assert.Equal(expected, actual.Value);
        }

        [Theory]
        [InlineData("0.0")]
        [InlineData("-1.0")]
        [InlineData("1.0")]
        [InlineData("2.137")]
        public void Read_Float_HasCorrectType(string input)
        {
            var actual = Reader.Read(input);
            Assert.IsType<Float>(actual);
        }

        [Theory]
        [InlineData("0.0", 0.0f)]
        [InlineData("-1.0", -1.0f)]
        [InlineData("1.0", 1.0f)]
        [InlineData("2.137", 2.137f)]
        [InlineData("1234.1234567", 1234.1234567f)]
        public void Read_Float_HasCorrectValue(string input, float expected)
        {
            var actual = (Float) Reader.Read(input);
            Assert.Equal(expected, actual.Value);
        }

        [Theory]
        [InlineData(@"a\s")]
        public void Read_InvalidSymbol_ThrowsYungException(string input)
        {
            Assert.Throws<YungException>(() => Reader.Read(input));
        }

        [Theory]
        [InlineData("(")]
        [InlineData("[")]
        //[InlineData("{")]
        //[InlineData("#[")]
        public void Read_UnterminatedCollection_ThrowsYungException(string input)
        {
            Assert.Throws<YungException>(() => Reader.Read(input));
        }

        [Theory]
        [InlineData(")")]
        [InlineData("]")]
        [InlineData("}")]
        public void Read_UnexpectedEndOfCollection_ThrowsYungException(string input)
        {
            Assert.Throws<YungException>(() => Reader.Read(input));
        }

        [Theory]
        [InlineData("asdf")]
        [InlineData("ASDF")]
        [InlineData("AsDf")]
        [InlineData("a123")]
        [InlineData("A123")]
        [InlineData("A?2!")]
        [InlineData("*s+f")]
        [InlineData("is-s?")]
        [InlineData("+")]
        [InlineData("!$%^&*_-+=<>?/")]
        public void Read_Symbol_HasCorrectType(string input)
        {
            var actual = Reader.Read(input);
            Assert.IsType<Symbol>(actual);
        }

        [Theory]
        [InlineData("asdf")]
        [InlineData("ASDF")]
        [InlineData("AsDf")]
        [InlineData("a123")]
        [InlineData("A123")]
        [InlineData("A?2!")]
        [InlineData("*s+f")]
        [InlineData("is-s?")]
        [InlineData("+")]
        [InlineData("!$%^&*_-+=<>?/")]
        public void Read_Symbol_HasCorrectValue(string input)
        {
            var expected = input;
            var actual = Reader.Read(input);
            Assert.Equal(expected, ((Symbol) actual).Value);
        }

        [Fact]
        public void Read_Nil_HasCorrectType()
        {
            var actual = Reader.Read("nil");
            Assert.IsType<Nil>(actual);
        }
    }
}

using Xunit;
using Yung.AST;

namespace Yung.Tests
{
    public class PrinterTests
    {
        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData(long.MinValue, "-9223372036854775808")]
        [InlineData(long.MaxValue, "9223372036854775807")]
        public void Print_Integer(long value, string expected)
        {
            var input = new Integer(value);
            var actual = Printer.Print(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0.0f, "0.0")]
        [InlineData(-1.0f, "-1.0")]
        [InlineData(1.0f, "1.0")]
        [InlineData(-0.8765432f, "-0.8765432")]
        [InlineData(0.1234567f, "0.1234567")]
        public void Print_Float(float value, string expected)
        {
            var input = new Float(value);
            var actual = Printer.Print(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Print_List_Empty()
        {
            const string expected = "()";
            var input = new List();
            var actual = Printer.Print(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Print_Nil()
        {
            const string expected = "nil";
            var input = new Nil();
            var actual = Printer.Print(input);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Print_PrintVector_Empty()
        {
            const string expected = "[]";
            var input = new Vector();
            var actual = Printer.Print(input);
            Assert.Equal(expected, actual);
        }
    }
}

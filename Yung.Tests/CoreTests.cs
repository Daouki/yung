using System.Collections.Generic;
using System.Globalization;
using Xunit;
using Yung.AST;
using Yung.Exceptions;

namespace Yung.Tests
{
    public class CoreFunctionTests
    {
        private Environment _environment;
        
        public CoreFunctionTests()
        {
            _environment = new Environment();
            _environment.Add(new Symbol("+"), Core.Add);
            _environment.Add(new Symbol("-"), Core.Subtract);
            _environment.Add(new Symbol("*"), Core.Multiply);
            _environment.Add(new Symbol("/"), Core.Divide);
        }
        
        [Theory]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("*")]
        [InlineData("/")]
        public void Evaluate_NumberOperation_NoArguments(string function)
        {
            var input = Reader.Read($"({function})");
            var actual = Evaluator.Evaluate(input, _environment);
            Assert.IsType<Nil>(actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void Evaluate_Add_SingleIntegerArgument(long value)
        {
            var expected = value;
            var input = Reader.Read($"(+ {value})");
            var actual = Evaluator.Evaluate(input, _environment);
            Assert.Equal(expected, ((Integer) actual).Value);
        }

        [Theory]
        [InlineData("1 2", 3)]
        [InlineData("1 2 3", 6)]
        [InlineData("1 -2 3", 2)]
        [InlineData("9223372036854775807 1", long.MinValue)]
        [InlineData("-9223372036854775808 -1", long.MaxValue)]
        public void Evaluate_Add_MultipleIntegerArgument(string values, long expected)
        {
            var input = Reader.Read($"(+ {values})");
            var actual = Evaluator.Evaluate(input, _environment);
            Assert.Equal(expected, ((Integer) actual).Value);
        }

        [Theory]
        [InlineData("1.2 2.7", 3.9f)]
        [InlineData("1.0 2.0 3.0", 6.0f)]
        [InlineData("1.5 -2.5 3.5", 2.5f)]
        public void Evaluate_Add_MultipleFloatArgument(string values, float expected)
        {
            var input = Reader.Read($"(+ {values})");
            var actual = Evaluator.Evaluate(input, _environment);
            Assert.Equal(expected, ((Float) actual).Value);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(-1.0)]
        [InlineData(1.0)]
        [InlineData(21.37)]
        public void Evaluate_Add_SingleFloatArgument(float value)
        {
            var expected = value;
            var input =
                Reader.Read(
                    $"(+ {value.ToString("N7", CultureInfo.InvariantCulture.NumberFormat)})");
            var actual = Evaluator.Evaluate(input, _environment);
            Assert.Equal(expected, ((Float) actual).Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void Evaluate_Subtract_SingleIntegerArgument(long value)
        {
            var expected = -value;
            var input = Reader.Read($"(- {value})");
            var actual = Evaluator.Evaluate(input, _environment);
            Assert.Equal(expected, ((Integer) actual).Value);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(-1.0)]
        [InlineData(1.0)]
        [InlineData(21.37)]
        public void Evaluate_Subtract_SingleFloatArgument(float value)
        {
            var expected = -value;
            var input =
                Reader.Read(
                    $"(- {value.ToString("N7", CultureInfo.InvariantCulture.NumberFormat)})");
            var actual = Evaluator.Evaluate(input, _environment);
            Assert.Equal(expected, ((Float) actual).Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void Evaluate_Multiply_SingleIntegerArgument(long value)
        {
            var expected = value;
            var input = Reader.Read($"(* {value})");
            var actual = Evaluator.Evaluate(input, _environment);
            Assert.Equal(expected, ((Integer) actual).Value);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(-1.0)]
        [InlineData(1.0)]
        [InlineData(21.37)]
        public void Evaluate_Multiply_SingleFloatArgument(float value)
        {
            var expected = value;
            var input =
                Reader.Read(
                    $"(* {value.ToString("N7", CultureInfo.InvariantCulture.NumberFormat)})");
            var actual = Evaluator.Evaluate(input, _environment);
            Assert.Equal(expected, ((Float) actual).Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void Evaluate_Divide_SingleIntegerArgument(long value)
        {
            var expected = value;
            var input = Reader.Read($"(/ {value})");
            var actual = Evaluator.Evaluate(input, _environment);
            Assert.Equal(expected, ((Integer) actual).Value);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(-1.0)]
        [InlineData(1.0)]
        [InlineData(21.37)]
        public void Evaluate_Divide_SingleFloatArgument(float value)
        {
            var expected = value;
            var input =
                Reader.Read(
                    $"(/ {value.ToString("N7", CultureInfo.InvariantCulture.NumberFormat)})");
            var actual = Evaluator.Evaluate(input, _environment);
            Assert.Equal(expected, ((Float) actual).Value);
        }

        [Fact]
        public void Evaluate_NumberOperation_CombineIntegersAndFloats_ThrowsTypeMismatch()
        {
            var input = Reader.Read("(- 67 12.396)");
            Assert.Throws<TypeMismatchException>(() => Evaluator.Evaluate(input, _environment));
        }

        [Fact]
        public void Evaluate_NumberOperation_InvalidType_ThrowsTypeMismatch()
        {
            var input = Reader.Read("(+ [] 7 () 2.4)");
            Assert.Throws<TypeMismatchException>(() => Evaluator.Evaluate(input, _environment));
        }
    }
}

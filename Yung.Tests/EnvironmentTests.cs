using Xunit;
using Yung.AST;
using Yung.Exceptions;

namespace Yung.Tests
{
    public class EnvironmentTests
    {
        [Theory]
        [InlineData("a", true)]
        [InlineData("b", false)]
        public void Contains(string binding, bool expected)
        {
            var environment = new Environment();
            environment.Add(new Symbol("a"), new Nil());
            var actual = environment.Contains(new Symbol(binding));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_SymbolRedefinition_ThrowsSymbolRedefinitionException()
        {
            var environment = new Environment();
            environment.Add(new Symbol("x"), new Nil());
            Assert.Throws<SymbolRedefinitionException>(() =>
                environment.Add(new Symbol("x"), new Nil()));
        }

        [Fact]
        public void GetValue_FromOuter()
        {
            var bindingName = new Symbol("x");
            var outerEnvironment = new Environment();
            outerEnvironment.Add(bindingName, new Nil());
            var innerEnvironment = new Environment(outerEnvironment);
            var value = innerEnvironment.GetValue(bindingName);
            Assert.IsType<Nil>(value);
        }

        [Fact]
        public void GetValue_Undefined_ReturnsNull()
        {
            var environment = new Environment();
            var value = environment.GetValue(new Symbol("asdf"));
            Assert.Null(value);
        }
    }
}

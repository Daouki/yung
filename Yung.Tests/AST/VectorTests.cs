using System;
using Xunit;
using Yung.AST;

namespace Yung.Tests.AST
{
    public class VectorTests
    {
        [Fact]
        public void GetElement_PastCount_ThrowsIndexOutOfRangeException()
        {
            var vector = new Vector {new Nil(), new Nil(), new Nil(), new Nil(), new Nil()};
            Assert.Throws<IndexOutOfRangeException>(() => vector[int.MaxValue]);
        }

        [Fact]
        public void SetCapacity_BelowCount_ThrowsArgumentOutOfRangeException()
        {
            var vector = new Vector {new Nil(), new Nil(), new Nil(), new Nil(), new Nil()};
            Assert.Throws<ArgumentOutOfRangeException>(() => vector.Capacity = 3);
        }
    }
}

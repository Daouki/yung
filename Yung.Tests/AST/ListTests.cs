using Xunit;
using Yung.AST;

namespace Yung.Tests.AST
{
    public class ListTests
    {
        [Fact]
        public void Add_ToEmpty_CanAccessHeadValue()
        {
            var list = new List();
            list.Add(new Nil());
            Assert.IsType<Nil>(list.Head.Value);
        }

        [Fact]
        public void Add_ToEmpty_HasCorrectOrder()
        {
            var list = new List();
            list.Add(new Integer(1));
            list.Add(new Integer(2));
            list.Add(new Integer(3));
            Assert.Equal(3, ((Integer) list.Head.Value).Value);
        }

        [Fact]
        public void Add_ToEmpty_HeadIsNotNull()
        {
            var list = new List();
            list.Add(new Nil());
            Assert.NotNull(list.Head);
        }

        [Fact]
        public void Add_ToEmpty_NextIsNull()
        {
            var list = new List();
            list.Add(new Nil());
            Assert.Null(list.Head.Next);
        }
    }
}

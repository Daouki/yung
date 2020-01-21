namespace Yung.AST
{
    public class Integer : IValue
    {
        public Integer(long value)
        {
            Value = value;
        }

        public long Value { get; }
    }
}

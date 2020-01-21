namespace Yung.AST
{
    public class Boolean : IValue
    {
        public Boolean(bool value)
        {
            Value = value;
        }

        public bool Value { get; }
    }
}

namespace Yung.AST
{
    public class Boolean : IValue
    {
        public Boolean(bool value)
        {
            Value = value;
        }

        public bool Value { get; }

        public static explicit operator bool(Boolean boolean)
        {
            return boolean.Value;
        }
    }
}

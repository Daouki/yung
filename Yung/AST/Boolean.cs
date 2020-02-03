namespace Yung.AST
{
    public class Boolean : IValue
    {
        public static readonly Boolean True = new Boolean(true); 
        public static readonly Boolean False = new Boolean(false); 
        
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

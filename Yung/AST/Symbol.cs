namespace Yung.AST
{
    public class Symbol : IValue
    {
        public Symbol(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static bool operator ==(Symbol a, Symbol b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.Value == b.Value;
        }

        public static bool operator !=(Symbol a, Symbol b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Symbol)) return false;
            var symbol = (Symbol) obj;
            return Value == symbol.Value;
        }

        public override int GetHashCode()
        {
            var stringHashCode = Value.GetHashCode();
            return (stringHashCode << 16) | (stringHashCode & 0xFFFF);
        }
    }
}

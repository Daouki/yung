namespace Yung.AST
{
    public class String : IValue
    {
        public string Value { get; }

        public String(string value)
        {
            Value = value;
        }
    }
}

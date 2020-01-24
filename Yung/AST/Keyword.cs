namespace Yung.AST
{
    public class Keyword : IValue

    {
        public Keyword(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}

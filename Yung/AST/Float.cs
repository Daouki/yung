namespace Yung.AST
{
    public class Float : IValue
    {
        public Float(float value)
        {
            Value = value;
        }

        public float Value { get; }
    }
}

namespace Yung.AST
{
    public interface INumber : IValue
    {
        INumber Negate();

        INumber Add(INumber number);

        INumber Subtract(INumber number);

        INumber Multiply(INumber number);

        INumber Divide(INumber number);
        
        Boolean Less(INumber number);
        
        Boolean LessOrEqual(INumber number);

        Boolean Greater(INumber number);

        Boolean GreaterOrEqual(INumber number);
    }
}

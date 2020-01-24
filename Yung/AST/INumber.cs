namespace Yung.AST
{
    public interface INumber : IValue
    {
        INumber Negate();

        INumber Add(INumber number);

        INumber Subtract(INumber number);

        INumber Multiply(INumber number);

        INumber Divide(INumber number);
    }
}

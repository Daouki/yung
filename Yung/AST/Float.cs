using System;
using Yung.Exceptions;

namespace Yung.AST
{
    public class Float : INumber
    {
        public Float(float value)
        {
            Value = value;
        }

        public float Value { get; }

        public INumber Negate()
        {
            return new Float(-Value);
        }

        public INumber Add(INumber number)
        {
            try
            {
                return new Float(Value + ((Float) number).Value);
            }
            catch (InvalidCastException)
            {
                throw new TypeMismatchException();
            }
        }

        public INumber Subtract(INumber number)
        {
            try
            {
                return new Float(Value - ((Float) number).Value);
            }
            catch (InvalidCastException)
            {
                throw new TypeMismatchException();
            }
        }

        public INumber Multiply(INumber number)
        {
            try
            {
                return new Float(Value * ((Float) number).Value);
            }
            catch (InvalidCastException)
            {
                throw new TypeMismatchException();
            }
        }

        public INumber Divide(INumber number)
        {
            try
            {
                return new Float(Value / ((Float) number).Value);
            }
            catch (InvalidCastException)
            {
                throw new TypeMismatchException();
            }
        }
        
        public Boolean Less(INumber number)
        {
            try
            {
                return new Boolean(Value < ((Float) number).Value);
            }
            catch (InvalidCastException)
            {
                throw new TypeMismatchException();
            }
        }

        public Boolean LessOrEqual(INumber number)
        {
            try
            {
                return new Boolean(Value <= ((Float) number).Value);
            }
            catch (InvalidCastException)
            {
                throw new TypeMismatchException();
            }
        }

        public Boolean Greater(INumber number)
        {
            try
            {
                return new Boolean(Value > ((Float) number).Value);
            }
            catch (InvalidCastException)
            {
                throw new TypeMismatchException();
            }
        }

        public Boolean GreaterOrEqual(INumber number)
        {
            try
            {
                return new Boolean(Value >= ((Float) number).Value);
            }
            catch (InvalidCastException)
            {
                throw new TypeMismatchException();
            }
        }
    }
}

﻿using System;
using Yung.Exceptions;

namespace Yung.AST
{
    public class Integer : INumber
    {
        public Integer(long value)
        {
            Value = value;
        }

        public long Value { get; }

        public INumber Negate()
        {
            return new Integer(-Value);
        }

        public INumber Add(INumber number)
        {
            try
            {
                return new Integer(Value + ((Integer) number).Value);
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
                return new Integer(Value - ((Integer) number).Value);
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
                return new Integer(Value * ((Integer) number).Value);
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
                return new Integer(Value / ((Integer) number).Value);
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
                return new Boolean(Value < ((Integer) number).Value);
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
                return new Boolean(Value <= ((Integer) number).Value);
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
                return new Boolean(Value > ((Integer) number).Value);
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
                return new Boolean(Value >= ((Integer) number).Value);
            }
            catch (InvalidCastException)
            {
                throw new TypeMismatchException();
            }
        }
    }
}

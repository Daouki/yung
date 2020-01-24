using System;

namespace Yung.AST
{
    public class Function : IValue
    {
        private readonly Func<List, IValue> _builtIn;

        /// <summary>
        ///     Construct a built-in function.
        /// </summary>
        /// <param name="builtIn">Built-in function definition.</param>
        public Function(Func<List, IValue> builtIn)
        {
            _builtIn = builtIn;
        }

        public IValue Apply(List arguments)
        {
            return _builtIn(arguments);
        }
    }
}

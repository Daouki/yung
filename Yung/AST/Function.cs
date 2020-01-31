using System;

namespace Yung.AST
{
    public class Function : IValue
    {
        private readonly Func<Vector, IValue> _function;
        private readonly Vector _parameters = null;
        private readonly IValue _body = null;
        private readonly Environment _environment = null;

        /// <summary>
        ///     Construct a built-in function.
        /// </summary>
        /// <param name="function">Built-in function definition.</param>
        public Function(Func<Vector, IValue> function)
        {
            _function = function;
        }

        public Function(
            Func<Vector, IValue> function,
            Vector parameters,
            IValue body,
            Environment environment)
            : this(function)
        {
            _parameters = parameters;
            _body = body;
            _environment = environment;
        }

        public IValue Apply(Vector arguments)
        {
            return _function(arguments);
        }
    }
}

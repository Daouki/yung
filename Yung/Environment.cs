using System;
using System.Collections.Generic;
using Yung.AST;
using Yung.Exceptions;

namespace Yung
{
    public class Environment
    {
        private readonly Dictionary<Symbol, IValue> _bindings = new Dictionary<Symbol, IValue>();
        private readonly Environment _outer;

        public Environment(Environment outer = null)
        {
            _outer = outer;
        }

        /// <summary>
        ///     Add a new binding to the current scope.
        /// </summary>
        /// <param name="binding">Name of the binding.</param>
        /// <param name="value">Value of the binding.</param>
        /// <exception cref="SymbolRedefinitionException">Thrown when a symbol with the name 'binding' already exists in the current scope.</exception>
        public void Add(Symbol binding, IValue value)
        {
            if (Contains(binding)) throw new SymbolRedefinitionException(binding);
            try
            {
                _bindings.Add(binding, value);
            }
            catch (ArgumentException)
            {
                throw new SymbolRedefinitionException(binding);
            }
        }

        /// <summary>
        /// Check whether the current scope contains a binding.
        /// </summary>
        /// <param name="binding">Name of the binding.</param>
        /// <returns>If the binding exists, returns true, otherwise, returns false.</returns>
        public bool Contains(Symbol binding)
        {
            return (_bindings.TryGetValue(binding, out var value)
                       ? value
                       : _outer?.GetValue(binding)) != null;
        }

        /// <summary>
        ///     Get a value binded to the symbol 'symbol'.
        /// </summary>
        /// <param name="name">Name of the binding to get the value from.</param>
        /// <returns>Value binded to the binding with the name 'name'.</returns>
        public IValue GetValue(Symbol name)
        {
            return _bindings.TryGetValue(name, out var value) ? value : _outer?.GetValue(name);
        }
    }
}

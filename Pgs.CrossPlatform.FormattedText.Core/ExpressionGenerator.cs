using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Pgs.CrossPlatform.FormattedText.Core
{
    /// <summary>
    /// Very simple class that helps generating expression that execute as switch 
    /// </summary>
    /// <typeparam name="TestT">The type of switch test value.</typeparam>
    /// <typeparam name="T">The type of action invoked in selected case in switch</typeparam>
    public class ExpressionGenerator<TestT, T>
    {
        private readonly List<SwitchCase> _cases = new List<SwitchCase>();

        /// <summary>
        /// Gets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized { get; private set; }
        /// <summary>
        /// Gets the compiled SwitchExpression.
        /// </summary>
        /// <value>
        /// The compiled expression.
        /// </value>
        public Action<TestT, object, T, T> Compiled { get; private set; }
        /// <summary>
        /// Gets the switch expression.
        /// </summary>
        /// <value>
        /// The switch expression.
        /// </value>
        public Expression SwitchExpression { get; private set; }

        ParameterExpression expParameter = Expression.Parameter(typeof(TestT));
        ParameterExpression refObfParameter = Expression.Parameter(typeof(object));
        ParameterExpression invokeParameter1 = Expression.Parameter(typeof(T));
        ParameterExpression invokeParameter2 = Expression.Parameter(typeof(T));

        /// <summary>
        /// Generates the expression and return it's compiled version.
        /// </summary>
        /// <param name="defualtAction">The default action in generated switch.</param>
        /// <param name="forceRegenerate">if set to <c>true</c> force regeneration.</param>
        /// <returns>
        /// Compiled switch expression
        /// </returns>
        public Action<TestT, object, T, T> Generate(Expression<Action> defualtAction = null, bool forceRegenerate = false)
        {
            if (IsInitialized && !forceRegenerate)
                return Compiled;

            if (defualtAction != null)
            {
                SwitchExpression = Expression.Switch(expParameter, Expression.Invoke(defualtAction), _cases.ToArray());
            }
            else
            {
                SwitchExpression = Expression.Switch(expParameter, _cases.ToArray());
            }
            Expression<Action<TestT, object, T,T>> expressionToCompile = Expression.Lambda<Action<TestT, object, T, T>>(SwitchExpression, new List<ParameterExpression>() { expParameter, refObfParameter, invokeParameter1, invokeParameter2 });
            Compiled = expressionToCompile.Compile();
            IsInitialized = true;
            return Compiled;
        }

        /// <summary>
        /// Adds the case.
        /// </summary>
        /// <param name="testValue">The test value.</param>
        /// <param name="methodInfo">The action.</param>
        /// <returns></returns>
        public ExpressionGenerator<TestT, T> AddCase(TestT testValue, MethodInfo methodInfo)
        {
            var expCase = Expression.SwitchCase(
                Expression.Call(null, methodInfo, refObfParameter, invokeParameter1, invokeParameter2),
                Expression.Constant(testValue, typeof(TestT))
            );
            _cases.Add(expCase);
            return this;
        }
    }
}

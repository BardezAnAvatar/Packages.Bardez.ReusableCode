using System;
using System.Linq.Expressions;

namespace Bardez.Projects.ReusableCode
{
    /// <summary>Helper class to get the name of a provided expression</summary>
    /// <remarks>
    ///     Implemented based on:
    ///     http://joelabrahamsson.com/getting-property-and-method-names-using-static-reflection-in-c/
    ///     http://www.jagregory.com/writings/introduction-to-static-reflection/
    /// </remarks>
    public static class StaticReflection
    {
        /// <summary>Gets the name of the parameter provided in the expression</summary>
        /// <param name="expression"><see cref="Expression" /> to get the name for</param>
        /// <returns>The name of the provided <see cref="Expression" /></returns>
        /// <exception cref="ArgumentNullException">Null parameter</exception>
        /// <exception cref="InvalidOperationException">
        ///     Parameter not of type:
        ///     <see cref="MemberExpression" />, <see cref="MethodCallExpression" />,
        ///     <see cref="UnaryExpression" />, or <see cref="ParameterExpression" />
        /// </exception>
        public static String GetExpressionName(Expression expression)
        {
            String name = null;

            if (expression == null)
                throw new ArgumentNullException("expression", "The expression was unexpectedly null.");
            else if (expression is MemberExpression)
                name = StaticReflection.GetMemberName(expression as MemberExpression);
            else if (expression is MethodCallExpression)
                name = StaticReflection.GetMethodName(expression as MethodCallExpression);
            else if (expression is UnaryExpression)
                name = StaticReflection.GetUnaryName(expression as UnaryExpression);
            else if (expression is ParameterExpression)
                name = StaticReflection.GetParameterName(expression as ParameterExpression);
            else
                throw new InvalidOperationException("The provided Expression was not of a valid type to find an appropriate name.");

            return name;
        }

        /// <summary>Gets the name of the parameter provided in the expression</summary>
        /// <typeparam name="T">Type in the expression</typeparam>
        /// <param name="expression"><see cref="Expression" /> to get the name for</param>
        /// <returns>The name of the provided <see cref="Expression" /></returns>
        /// <exception cref="ArgumentNullException">Null parameter</exception>
        public static String GetExpressionName<T>(Expression<Func<T, Object>> expression)
        {
            String name = null;

            if (expression == null)
                throw new ArgumentNullException("expression", "The expression was unexpectedly null.");
            else
                name = StaticReflection.GetExpressionName(expression.Body);

            return name;
        }

        /// <summary>Gets the name of the parameter provided in the expression</summary>
        /// <typeparam name="T">Type in the expression</typeparam>
        /// <param name="expression"><see cref="Expression" /> to get the name for</param>
        /// <returns>The name of the provided <see cref="Expression" /></returns>
        /// <exception cref="ArgumentNullException">Null parameter</exception>
        public static String GetExpressionName<T>(Expression<Action<T>> expression)
        {
            String name = null;

            if (expression == null)
                throw new ArgumentNullException("expression", "The expression was unexpectedly null.");
            else
                name = StaticReflection.GetExpressionName(expression.Body);

            return name;
        }

        /// <summary>Gets the name of the parameter provided in the expression</summary>
        /// <param name="expression"><see cref="UnaryExpression" /> to get the name for</param>
        /// <returns>The name of the provided <see cref="UnaryExpression" /></returns>
        /// <exception cref="ArgumentNullException">Null parameter</exception>
        public static String GetUnaryName(UnaryExpression expression)
        {
            String name = null;

            if (expression == null)
                throw new ArgumentNullException("expression", "The expression was unexpectedly null.");
            else
                name = StaticReflection.GetMemberName(expression.Operand as MemberExpression);

            return name;
        }

        /// <summary>Gets the name of the parameter provided in the expression</summary>
        /// <param name="expression"><see cref="MethodCallExpression" /> to get the name for</param>
        /// <returns>The name of the provided <see cref="MethodCallExpression" /></returns>
        /// <exception cref="ArgumentNullException">Null parameter</exception>
        public static String GetMethodName(MethodCallExpression expression)
        {
            String name = null;

            if (expression == null)
                throw new ArgumentNullException("expression", "The expression was unexpectedly null.");
            else
                name = expression.Method.Name;

            return name;
        }

        /// <summary>Gets the name of the parameter provided in the expression</summary>
        /// <param name="expression"><see cref="MemberExpression" /> to get the name for</param>
        /// <returns>The name of the provided <see cref="MemberExpression" /></returns>
        /// <exception cref="ArgumentNullException">Null parameter</exception>
        public static String GetMemberName(MemberExpression expression)
        {
            String name = null;

            if (expression == null)
                throw new ArgumentNullException("expression", "The expression was unexpectedly null.");
            else
                name = expression.Member.Name;

            return name;
        }

        /// <summary>Gets the name of the parameter provided in the expression</summary>
        /// <param name="expression"><see cref="ParameterExpression" /> to get the name for</param>
        /// <returns>The name of the provided <see cref="ParameterExpression" /></returns>
        /// <exception cref="ArgumentNullException">Null parameter</exception>
        public static String GetParameterName(ParameterExpression expression)
        {
            String name = null;

            if (expression == null)
                throw new ArgumentNullException("expression", "The expression was unexpectedly null.");
            else
                name = expression.Name;

            return name;
        }
    }
}
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Generics.Helpers
{
    public static class Filter
    {
        public static Expression<Func<TModel, bool>>
            FromStringExpression<TModel>(
                string query,
                string parameter = "x")
        {
            try
            {
                var parameterExpression =
                    Expression.Parameter(
                        typeof(TModel),
                        parameter);

                var lambda =
                    DynamicExpressionParser.ParseLambda(
                        new[] { parameterExpression },
                        typeof(bool),
                        query);

                return (Expression<Func<TModel, bool>>)lambda;
            }
            catch (Exception ex)
            {
                throw new ValidationException(
                    $"filter expression invalid: {ex.Message}");
            }
        }
    }
}
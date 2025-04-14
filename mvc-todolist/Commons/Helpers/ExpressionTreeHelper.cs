using mvc_todolist.Models;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace mvc_todolist.Commons.Helpers
{
    public static class ExpressionTreeHelper
    {
        public static Expression<Func<T, bool>> GetFilterExpressionTree<T>(string? condition, string? filterJoin)
        {
            if (condition == null)
            {
                throw new ArgumentException("Condition null");
            }
            var param = Expression.Parameter(typeof(T), $"{nameof(T)[0]}");
            var conditionList = condition.Split(";");
            List<Expression> expressions = new List<Expression>();
            foreach (var it in conditionList)
            {
                if(!string.IsNullOrEmpty(it))
                    expressions.Add(ParseCondition(it, param));
            }
            if (expressions.Count > 0)
            {
                var expression = expressions[0];
                for (int i = 1; i < expressions.Count(); i++)
                {
                    expression = filterJoin == "and" ? Expression.AndAlso(expression, expressions[i]) :
                                                         Expression.OrElse(expression, expressions[i]);
                }
                return Expression.Lambda<Func<T, bool>>(expression, param);
            }

            return default!;

        }
        /*p => p.Age >= 10 && p.Age <= 100 && p.YearBirth==2000*/
        /*Age>=10;Age<=100;YearBirht=2000*/

        private static Expression ParseCondition(string condition, ParameterExpression param)
        {
            var parts = Regex.Split(condition, @"(==|!=|>=|<=|>|<)").Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();

            if (parts.Length != 3) throw new Exception($"Invalid condition: {condition}");

            var property = Expression.Property(param, parts[0].Trim());
            var value = Convert.ChangeType(parts[2].Trim().Trim('\''), property.Type);
            var constant = Expression.Constant(value, property.Type);

            return parts[1] switch
            {
                "==" => Expression.Equal(property, constant),
                ">" => Expression.GreaterThan(property, constant),
                "<" => Expression.LessThan(property, constant),
                "!=" => Expression.NotEqual(property, constant),
                ">=" => Expression.GreaterThanOrEqual(property, constant),
                "<=" => Expression.LessThanOrEqual(property, constant),
                _ => throw new Exception($"Unsupported operator: {parts[1]}")
            };
        }
    }
}

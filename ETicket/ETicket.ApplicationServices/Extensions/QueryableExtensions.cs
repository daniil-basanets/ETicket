using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ETicket.ApplicationServices.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplySortBy<T, TResult>(
            this IQueryable<T> query,
            Expression<Func<T, TResult>> expression,
            string sortDirection
        )
        {
            return (sortDirection == "asc") ? query.OrderBy(expression) : query.OrderByDescending(expression);
        }

        public static Expression<Func<T, bool>> CombineByOrElse<T>(this IEnumerable<Expression<Func<T, bool>>> expressions)
        {
            Expression<Func<T, bool>> result = expressions.FirstOrDefault();

            for (int i = 1; i < expressions.Count(); i++)
            {
                result = result.OrElse(expressions.ElementAt(i));
            }

            return result;
        }

        public static Expression<Func<T, bool>> CombineByAndAlso<T>(this IEnumerable<Expression<Func<T, bool>>> expressions)
        {
            Expression<Func<T, bool>> result = expressions.FirstOrDefault();

            for (int i = 1; i < expressions.Count(); i++)
            {
                result = result.AndAlso(expressions.ElementAt(i));
            }

            return result;
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> firstExpression, Expression<Func<T, bool>> secondExpression)
        {
            var visitor = new ParameterUpdateVisitor(secondExpression.Parameters.First(), firstExpression.Parameters.First());

            secondExpression = visitor.Visit(secondExpression) as Expression<Func<T, bool>>;

            var binExp = Expression.AndAlso(firstExpression.Body, secondExpression.Body);
          
            return Expression.Lambda<Func<T, bool>>(binExp, secondExpression.Parameters);
        }

        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> firstExpression, Expression<Func<T, bool>> secondExpression)
        {
            var visitor = new ParameterUpdateVisitor(secondExpression.Parameters.First(), firstExpression.Parameters.First());

            secondExpression = visitor.Visit(secondExpression) as Expression<Func<T, bool>>;

            var binExp = Expression.OrElse(firstExpression.Body, secondExpression.Body);
          
            return Expression.Lambda<Func<T, bool>>(binExp, secondExpression.Parameters);
        }


        class ParameterUpdateVisitor : ExpressionVisitor
        {
            private ParameterExpression oldParameter;
            private ParameterExpression newParameter;

            public ParameterUpdateVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                this.oldParameter = oldParameter;
                this.newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (object.ReferenceEquals(node, oldParameter))
                    return newParameter;

                return base.VisitParameter(node);
            }
        }
    }
}
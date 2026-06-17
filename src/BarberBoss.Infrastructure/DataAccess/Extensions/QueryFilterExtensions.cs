using System.Linq.Expressions;
using System.Reflection;

namespace BarberBoss.Infrastructure.DataAccess.Extensions;

public static class QueryFilterExtensions
{
    private static readonly HashSet<string> IgnoredProperties = ["Page", "PageSize", "OrderBy", "Descending"];

    public static IQueryable<TEntity> ApplyFilters<TEntity, TRequest>(this IQueryable<TEntity> query, TRequest request)
    {
        var filterProperties = typeof(TRequest)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => !IgnoredProperties.Contains(p.Name) && p.GetValue(request) is not null)
            .ToList();

        foreach (var filterProperty in filterProperties)
        {
            var entityProperty = typeof(TEntity).GetProperty(
                filterProperty.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
            );
            if (entityProperty is null)
                continue;
            
            var value = filterProperty.GetValue(request)!;
            query = query.ApplyCondition(entityProperty, value);
        }
        
        return query;
    }

    private static IQueryable<T> ApplyCondition<T>(this IQueryable<T> query, PropertyInfo entityProp, object value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, entityProp);

        Expression condition;

        if (entityProp.PropertyType == typeof(string))
        {
            var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)])!;
            condition = Expression.Call(propertyAccess, containsMethod, Expression.Constant(value));
        }
        else
        {
            condition = Expression.Equal(propertyAccess, Expression.Constant(value));
        }

        var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
        return query.Where(lambda);
    }
}
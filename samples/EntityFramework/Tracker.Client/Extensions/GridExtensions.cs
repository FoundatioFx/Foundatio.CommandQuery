using System.Data;

using Foundatio.CommandQuery.Queries;

using Grid = LoreSoft.Blazor.Controls;

namespace Tracker.Client.Extensions;

public static class GridExtensions
{
    public static QueryDefinition ToQuery(this Grid.DataRequest request)
    {
        return new QueryDefinition
        {
            Page = request.Page,
            PageSize = request.PageSize,
            Sorts = request.Sorts.ToSort(),
            Filter = request.Query.ToFilter()
        };
    }

    public static List<QuerySort>? ToSort(this IEnumerable<Grid.DataSort>? sorts)
    {
        if (sorts == null)
            return null;

        return sorts
            .Select(s => new QuerySort
            {
                Name = s.Property,
                Descending = s.Descending
            })
            .ToList();
    }

    public static QueryFilter? ToFilter(this Grid.QueryRule? queryRule)
    {
        if (queryRule is Grid.QueryGroup group)
            return group.ToFilter();
        else if (queryRule is Grid.QueryFilter filter)
            return filter.ToFilter();

        return null;
    }

    public static QueryFilter? ToFilter(this Grid.QueryGroup? queryGroup)
    {
        if (queryGroup == null || queryGroup.Filters.Count == 0)
            return null;

        var filter = new QueryFilter();
        filter.Logic = queryGroup.Logic == Grid.QueryLogic.Or ? QueryLogic.Or : QueryLogic.And;

        foreach (var rule in queryGroup.Filters)
        {
            var ruleFilter = rule.ToFilter();
            if (ruleFilter == null)
                continue;

            filter.Filters ??= [];
            filter.Filters.Add(ruleFilter);
        }

        return filter;
    }

    public static QueryFilter? ToFilter(this Grid.QueryFilter? queryFilter)
    {
        if (queryFilter == null)
            return null;

        return new QueryFilter
        {
            Name = queryFilter.Field,
            Operator = TranslateOperator(queryFilter.Operator),
            Value = queryFilter.Value
        };
    }


    public static Grid.DataResult<T> ToResult<T>(this QueryResult<T> pagedResult)
    {
        return new Grid.DataResult<T>(
            items: pagedResult.Data ?? [],
            total: (int)(pagedResult.Total ?? 0),
            continuationToken: pagedResult.ContinuationToken);
    }

    private static QueryOperators? TranslateOperator(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return value switch
        {
            Grid.QueryOperators.Equal => QueryOperators.Equal,
            Grid.QueryOperators.NotEqual => QueryOperators.NotEqual,
            Grid.QueryOperators.Contains => QueryOperators.Contains,
            Grid.QueryOperators.NotContains => QueryOperators.NotContains,
            Grid.QueryOperators.StartsWith => QueryOperators.StartsWith,
            Grid.QueryOperators.NotStartsWith => QueryOperators.NotStartsWith,
            Grid.QueryOperators.EndsWith => QueryOperators.EndsWith,
            Grid.QueryOperators.NotEndsWith => QueryOperators.NotEndsWith,
            Grid.QueryOperators.GreaterThan => QueryOperators.GreaterThan,
            Grid.QueryOperators.GreaterThanOrEqual => QueryOperators.GreaterThanOrEqual,
            Grid.QueryOperators.LessThan => QueryOperators.LessThan,
            Grid.QueryOperators.LessThanOrEqual => QueryOperators.LessThanOrEqual,
            Grid.QueryOperators.IsNull => QueryOperators.IsNull,
            Grid.QueryOperators.IsNotNull => QueryOperators.IsNotNull,
            _ => null,
        };
    }
}

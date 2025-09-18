using Foundatio.CommandQuery.Definitions;

namespace Foundatio.CommandQuery.Queries;

/// <summary>
/// Provides static helper methods for building query filters, groups, and sort expressions.
/// </summary>
/// <remarks>
/// This builder simplifies the creation of <see cref="QueryFilter"/> and <see cref="QuerySort"/> instances
/// for use in data-driven applications.
/// </remarks>
public static class QueryBuilder
{
    /// <summary>
    /// Creates a search query filter for the specified model type.
    /// </summary>
    /// <typeparam name="TModel">The type of the model. Must implement <see cref="ISupportSearch"/>.</typeparam>
    /// <param name="searchText">The text to search for.</param>
    /// <returns>
    /// An instance of <see cref="QueryFilter"/> configured for the search text,
    /// or <see langword="null"/> if the search text is invalid.
    /// </returns>
    public static QueryFilter? Search<TModel>(string searchText)
        where TModel : class, ISupportSearch
    {
        return Search(TModel.SearchFields(), searchText);
    }

    /// <summary>
    /// Creates a search filter group for the specified fields and search text.
    /// </summary>
    /// <param name="fields">The list of fields or property names to search on.</param>
    /// <param name="searchText">The text to search for.</param>
    /// <returns>
    /// An instance of <see cref="QueryFilter"/> configured as a group for the search text,
    /// or <see langword="null"/> if the fields or search text are invalid.
    /// </returns>
    public static QueryFilter? Search(IEnumerable<string> fields, string searchText)
    {
        if (fields is null || string.IsNullOrWhiteSpace(searchText))
            return null;

        var filters = new List<QueryFilter>();

        foreach (var field in fields)
        {
            var filter = new QueryFilter
            {
                Name = field,
                Value = searchText,
                Operator = QueryOperators.Contains,
            };
            filters.Add(filter);
        }

        if (filters.Count == 0)
            return null;

        return new QueryFilter
        {
            Logic = QueryLogic.Or,
            Filters = filters,
        };
    }


    /// <summary>
    /// Creates a sort expression for the specified model type.
    /// </summary>
    /// <typeparam name="TModel">The type of the model. Must implement <see cref="ISupportSearch"/>.</typeparam>
    /// <returns>
    /// An instance of <see cref="QuerySort"/> for the model type.
    /// </returns>
    public static QuerySort? Sort<TModel>()
        where TModel : class, ISupportSearch
    {
        var sortField = TModel.SortField();
        return string.IsNullOrEmpty(sortField) ? null : new QuerySort { Name = sortField };
    }

    /// <summary>
    /// Creates a sort expression for the specified field and direction.
    /// </summary>
    /// <param name="field">The field or property name to sort on.</param>
    /// <param name="descending">Whether to sort in descending order.</param>
    /// <returns>
    /// An instance of <see cref="QuerySort"/> for the specified field and direction.
    /// </returns>
    public static QuerySort Sort(string field, bool descending = false)
        => new() { Name = field, Descending = descending };

    /// <summary>
    /// Creates a sort expression by parsing a sort string.
    /// </summary>
    /// <param name="sortString">The sort string to parse (e.g., "Name desc" or "Name:asc").</param>
    /// <returns>
    /// An instance of <see cref="QuerySort"/> for the parsed sort string,
    /// or <see langword="null"/> if the sort string is invalid.
    /// </returns>
    public static QuerySort? Sort(string? sortString)
        => QuerySort.Parse(sortString);


    /// <summary>
    /// Creates a filter for the specified field, value, and operator.
    /// </summary>
    /// <param name="field">The field or property name to filter on.</param>
    /// <param name="value">The value to filter for.</param>
    /// <param name="operator">The operator to use for the filter.</param>
    /// <returns>
    /// An instance of <see cref="QueryFilter"/> for the specified field, value, and operator.
    /// </returns>
    public static QueryFilter Filter(string field, object? value, QueryOperators @operator = QueryOperators.Equal)
        => new() { Name = field, Value = value, Operator = @operator };


    /// <summary>
    /// Creates a filter group for the specified filters using the "and" logic operator.
    /// </summary>
    /// <param name="filters">The list of filters to group.</param>
    /// <returns>
    /// An instance of <see cref="QueryFilter"/> configured as a group,
    /// or <see langword="null"/> if no valid filters are provided.
    /// </returns>
    /// <remarks>
    /// Any invalid filters will be removed from the group.
    /// </remarks>
    public static QueryFilter? Group(params IEnumerable<QueryFilter?> filters)
        => Group(QueryLogic.And, filters);

    /// <summary>
    /// Creates a filter group for the specified logic and filters.
    /// </summary>
    /// <param name="logic">The group logic operator.</param>
    /// <param name="filters">The list of filters to group.</param>
    /// <returns>
    /// An instance of <see cref="QueryFilter"/> configured as a group,
    /// or <see langword="null"/> if no valid filters are provided.
    /// </returns>
    /// <remarks>
    /// Any invalid filters will be removed from the group.
    /// </remarks>
    public static QueryFilter? Group(QueryLogic logic, params IEnumerable<QueryFilter?> filters)
    {
        // check for any valid filters
        if (!filters.Any(f => f?.IsValid() == true))
            return null;

        var groupFilters = filters
            .Where(f => f?.IsValid() == true)
            .Select(f => f!)
            .ToList();

        // if there's only one filter and it's already a group, return it directly
        if (groupFilters.Count == 1 && groupFilters[0].IsGroup())
            return groupFilters[0];

        return new QueryFilter
        {
            Logic = logic,
            Filters = groupFilters,
        };
    }
}

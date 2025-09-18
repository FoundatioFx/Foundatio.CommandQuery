using System;
using System.Text.Json;
using Foundatio.CommandQuery.Queries;

// Test string array
var stringFilter = new QueryFilter
{
    Name = "categories",
    Value = new[] { "tech", "science", "business" },
    Operator = QueryOperators.In
};

var json = JsonSerializer.Serialize(stringFilter);
Console.WriteLine($"String array JSON: {json}");

var deserializedStringFilter = JsonSerializer.Deserialize<QueryFilter>(json);
var stringArray = deserializedStringFilter?.Value as string[];
Console.WriteLine($"Deserialized as string[]: {stringArray != null}");
if (stringArray != null)
    Console.WriteLine($"String array contents: [{string.Join(", ", stringArray)}]");

Console.WriteLine();

// Test int array
var intFilter = new QueryFilter
{
    Name = "ids",
    Value = new[] { 1, 2, 3, 5, 8 },
    Operator = QueryOperators.In
};

json = JsonSerializer.Serialize(intFilter);
Console.WriteLine($"Int array JSON: {json}");

var deserializedIntFilter = JsonSerializer.Deserialize<QueryFilter>(json);
var intArray = deserializedIntFilter?.Value as int[];
Console.WriteLine($"Deserialized as int[]: {intArray != null}");
if (intArray != null)
    Console.WriteLine($"Int array contents: [{string.Join(", ", intArray)}]");

Console.WriteLine();

// Test mixed array
var mixedFilter = new QueryFilter
{
    Name = "values",
    Value = new object[] { "text", 42, true, 3.14 },
    Operator = QueryOperators.In
};

json = JsonSerializer.Serialize(mixedFilter);
Console.WriteLine($"Mixed array JSON: {json}");

var deserializedMixedFilter = JsonSerializer.Deserialize<QueryFilter>(json);
var objectArray = deserializedMixedFilter?.Value as object[];
Console.WriteLine($"Deserialized as object[]: {objectArray != null}");
if (objectArray != null)
    Console.WriteLine($"Mixed array contents: [{string.Join(", ", objectArray)}]");

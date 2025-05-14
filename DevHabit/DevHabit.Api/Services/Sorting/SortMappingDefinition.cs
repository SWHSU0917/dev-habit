namespace DevHabit.Api.Services.Sorting;

public sealed class SortMappingDefinition<TSource, TDestinatiion> : ISortMappingDefinition
{
    public required SortMapping[] Mappings { get; init; }
}

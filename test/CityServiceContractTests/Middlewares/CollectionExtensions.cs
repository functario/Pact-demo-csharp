namespace CityServiceContractTests.Middlewares;

internal static class CollectionExtensions
{
    internal static ICollection<T> AddRange<T>(this ICollection<T> collection, ICollection<T> items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }

        return collection;
    }
}

using HungerHydra.Enums;

namespace HungerHydra.Models;

internal class RepositoryValue<TValue>
{
    public RepositoryStatuses Status { get; set; }

    public TValue Value { get; set; }

    public string Exception { get; set; }

    public RepositoryValue(TValue value, string exception)
    {
        Value = value;
        Exception = string.Empty;
        Status = RepositoryStatuses.NotFound;
    }

    public RepositoryValue()
    {
        Exception = string.Empty;
        Status = RepositoryStatuses.NotFound;
    }
}
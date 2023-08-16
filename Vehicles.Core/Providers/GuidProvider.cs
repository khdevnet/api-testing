namespace Vehicles.Core.Providers;

internal class GuidProvider : IGuidProvider
{
    public Guid New() => Guid.NewGuid();
}
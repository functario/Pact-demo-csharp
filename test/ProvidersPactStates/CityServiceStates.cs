namespace ProvidersPactStates;

public static class CityServiceStates
{
    public static ProviderStateExtend SomeCitiesExist =>
        new(References.CityService, "SomeCitiesExist", "Some cities exist");
}

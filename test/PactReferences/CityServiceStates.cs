namespace PactReferences;

public static class CityServiceStates
{
    public static ProviderStateExtend SomeCitiesExist =>
        new(Participants.CityService, "SomeCitiesExist", "Some cities exist");
}

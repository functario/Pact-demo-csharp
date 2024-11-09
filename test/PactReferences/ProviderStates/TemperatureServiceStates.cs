namespace PactReferences.ProviderStates;

public static class TemperatureServiceStates
{
    public static ProviderStateExtend SomeTemperaturesExist =>
        new(Participants.TemperatureService, "SomeTemperaturesExist", "Some temperature exist");
}

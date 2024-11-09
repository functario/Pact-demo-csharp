# Reference - Demo environment

The projects have a `.env` to be configured in a state during demo.

## PACTDEMO_DEMOCASE

| Value                     | Definition                                                                          |
| ------------------------- | ----------------------------------------------------------------------------------- |
| HappyPath                 | Makes all tests pass                                                                |
| JsonSerializerInvalidEnum | The Providers and Consumers does not deserialized Enum the same way (int vs string) |


## PACTDEMO_PACTLOGLEVEL

The level of log to display in command line or xunit output.

| Value       |
| ----------- |
| Trace       |
| Debug       |
| Information |
| Warn        |
| Error       |
| None        |


## PACTDEMO_PACTFOLDER

Absolute or relative path to the '[pacts](../../../test/pacts/) folder containing the consumers contracts.
Relative path is based on the bin folder of all contract test projects ()
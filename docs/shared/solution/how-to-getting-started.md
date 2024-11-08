# How-to - Getting started

## Prerequisites

1. [Dotnet 8 SDK installed](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)


## 1. Restore dotnet tools

Execute this command from the root of the solution to install all tools:

```shell
dotnet tool restore
```

## 2. Open solution

Open the solution [PactDemo.sln](../../../) from your C# ide.

## 3. Build the solution

1. Modify the [.env](../../../.env) file to the demo case corresponding (see [ref-demo-environment](./ref-demo-environment.md)).
2. To ensure to update all .env file copies, clean and rebuild the solution.

## 3. Start API projects

1. Select the project "[AppHost](../../../AspireProjects/AppHost/)" as starting project.
2. Start the project.
3. Follow the Aspire instruction to login to the dashboard.

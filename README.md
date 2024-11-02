# PawControl Simulation

A simple .NET client to simulate PawControl devices.

## Archived 2024.11.02.

The simulator is currently archived, as development of the original PawControl product was stopped. Both the product's backend and the simulator was completed to an MVP stage, and are fully functional. If the customer decides to continue development of the product, this will be unarchived and kept up to date.

## Usage

Make sure the [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) is installed (check with `dotnet --list-sdks`).

*Maximize the CMD/PowerShell/Terminal window.*

```shell
cd src
dotnet build
dotnet run --project PawControl
```

It'll instantly start simulating 3 basic tracker devices with a random version.

If the master token doesn't match, edit the source code: `./src/API/PawTracker.cs #10`

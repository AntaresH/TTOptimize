# TTOptimize

A tabletop game simulation and balance-optimization platform built on .NET.

## Configuration & Running

`TTOptimize.ConsoleHost` uses standard .NET layered configuration (`IConfiguration`) in the following order of precedence (later sources override earlier ones):

1. **`TTOptimize.ConsoleHost/appsettings.json`**: Global defaults (database path, default interactive flags).
2. **`TTOptimize.ConsoleHost/Properties/launchSettings.json`**: Visual Studio launch profile environment variables (prefixed with `Simulation__`).
3. **Command-line arguments (`args`)**: CLI overrides passed at startup take highest precedence.

### Key Configuration Settings (`Simulation` section)

| Key | Type | Default | Description |
| :--- | :--- | :--- | :--- |
| `Simulation:SetupInteractive` | `bool` | `true` | If `true`, prompts the user in the console for game and player setup. |
| `Simulation:GameInteractive` | `bool` | `true` | If `true`, renders the game board and accepts human player input. |
| `Simulation:BatchRuns` | `int` | `1` | Number of matches to execute in a single headless batch run. |
| `Simulation:GameId` | `string` | `null` | Identifier of the game to run (e.g., `splendidWonders`). |
| `Simulation:PlayerCount` | `int` | `null` | Number of participants in the match. |
| `Simulation:UseRandomSeed` | `bool` | `null` | Whether to use a random or fixed RNG seed. |

### Example CLI Execution (Headless Batch Run)

```powershell
dotnet run --project ConsoleHost -- --Simulation:SetupInteractive false --Simulation:GameInteractive false --Simulation:GameId splendidWonders --Simulation:PlayerCount 4 --Simulation:BatchRuns 1000
```

## Inspecting the Database

Match records are stored in `TTOptimize.Persistence/ttoptimize.db` (LiteDB v5). To browse collections and run queries outside the application, use **Verdure LiteDB Studio** (Microsoft Store) or the official **LiteDB.Studio** client.

*(Note: Because LiteDB uses file-level locking, stop any running `TTOptimize.ConsoleHost` process before opening the `.db` file).*

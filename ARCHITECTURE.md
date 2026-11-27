# TTOptimize – System Architecture

**TTOptimize** is a game-agnostic tabletop simulation and balance-optimization platform built on .NET. It enables modeling board and card game rulesets, running interactive (human) and automated (AI/bot) agents, executing high-throughput batch simulations, and persisting behavioral and game-balance metrics for statistical analysis.

---

## 1. Project Structure and Dependency Layers

The solution follows a strict, layered Clean Architecture. The inner core (**Core**) has zero dependencies on external frameworks, UI libraries, databases, or concrete game implementations.

```text
TTOptimize/
├── TTOptimize.ConsoleHost/       # Application entry point (Composition Root), DI, configuration, game selection, game setup
├── TTOptimize.ConsoleUI/         # Shared console rendering framework, widgets & UI primitives
├── TTOptimize.Core/              # Game- and infrastructure-agnostic contracts & core models
├── TTOptimize.Engine/            # Simulation engine, lifecycle management, metrics collection
├── TTOptimize.Persistence/       # LiteDB-based document persistence
├── Games/
	└── SplendidWonders/
		├── SplendidWonders.CLI/				# Console rendering (View) and user input handling (Input)
		├── SplendidWonders.Common/				# Game-specific model, setup, action and statistics definitions
		├── SplendidWonders.Gameplay/			# Concrete game ruleset, actions, reducers and statistics extractor
		└── SplendidWonders.Gameplay.Tests/     # Concrete game ruleset, actions, reducers and statistics extractor 
└── Persistence/								# ttoptimize.db
```

### Dependency Rules
* **`TTOptimize.ConsoleHost`** $\rightarrow$ References all TTOptimize.* projects and game-specific modules solely to wire up the Dependency Injection (DI) container.
* **`TTOptimize.ConsoleUI`** $\rightarrow$ `TTOptimize.Core`
* **`TTOptimize.Core`** $\rightarrow$ *No project dependencies.*
* **`TTOptimize.Engine`** $\rightarrow$ `TTOptimize.Core`
* **`TTOptimize.Persistence`** $\rightarrow$ `TTOptimize.Core`, `LiteDB`
* **`SplendidWonders.Common`** $\rightarrow$ `TTOptimize.Core`
* **`SplendidWonders.Gameplay`** $\rightarrow$ `TTOptimize.Core`, `SplendidWonders.Common`
* **`SplendidWonders.CLI`** $\rightarrow$ `TTOptimize.Core`, `SplendidWonders.Common`, `SplendidWonders.Gameplay`

---

## 2. Core Abstractions & Domain Model (`TTOptimize.Core`)

The platform strictly separates **in-game entities** from **external decision-making agents**:

* **`IGameState` & `IPlayer`**: Represents the current, immutable state of a match. In-game participants (`IPlayer`) are identified by their fixed starting seat (`Seat`, `1..N`) and display name.
* **`IAgent`**: An external decision-making entity (`HumanAgent`, `RandomAgent`, or game-specific heuristic bots). Each agent exposes a deterministic `StatisticsSubjectId` (e.g., `"Human:Mate"`, `"HeuristicAgent:RushV1"`) used as the grouping key for statistical aggregation.
* **`IGameRules` & `IGameStateFactory`**: Pure game logic: creating the initial state from an `IGameConfiguration`, enumerating legal actions (`GameAction`), applying state transitions via reducers, and evaluating final standings (`IPlayerResult`).
* **`IGameView` & `IGameInput`**: Rendering and user-input abstractions used exclusively when running in game-interactive mode.

---

## 3. Simulation Engine (`TTOptimize.Engine`)

### Lifecycle & DI Scope Management (`IGameContextFactory` & `GameContext`)
To avoid the *Service Locator* anti-pattern inside business logic, game-specific *Keyed Services* (`IGameStateFactory`, `IGameRules`, `IMatchMetricsExtractor`, and optional `IGameView` / `IGameInput`) are resolved by **`IGameContextFactory`** inside a dedicated `IServiceScope`:
* When `IsGameInteractive == false` (headless / batch mode), the factory returns `null` for `View` and `Input`, eliminating unnecessary UI object allocations during large simulations.
* `GameContext` implements `IDisposable`, ensuring the underlying DI scope is cleanly disposed of at the end of every match.

### Execution Pipeline
1. **`GameSessionManager`**: Provides two primary execution entry points:
   * `RunSingleMatch(...)`: Executes a single match and persists it immediately (`IMatchRepository.SaveMatch`).
   * `RunBatch(...)`: Executes bulk simulations (e.g., 100–10,000+ matches) tagged with a shared `BatchRunId`, writing all records to disk in a single bulk transaction (`IMatchRepository.SaveMatchesBulk`).
2. **`GameOrchestrator`**: The internal state-machine loop of a match. Uses an explicit `IReadOnlyDictionary<int, IAgent>` (`Seat -> IAgent`) mapping to query the active agent for decisions and records every move in `ActionLogEntry`.
3. **`GameSession`**: Coordinates the orchestrator, rules, and optional UI, producing an agent-agnostic `MatchOutcome` wrapped in a `SessionResult` (including execution timing).
4. **`MatchMetricsCollector`**: Combines `MatchOutcome` and the seat-to-agent dictionary to compute game-agnostic metrics (`BaseParticipantMetrics`: rank, victory points, turn count, action counts, and action ratios including `0.0` ratios for unused action types via `KnownActionTypes`). It then invokes the optional `IMatchMetricsExtractor` to append game-specific metrics and returns a persistable **`MatchRecord`**.

---

## 4. Persistence (`TTOptimize.Persistence`)

* Document-oriented storage powered by **LiteDB** (`LiteDbMatchRepository`).
* `MatchRecord` polymorphically stores game configurations (`IGameConfiguration`), participant metrics (`BasePlayerMetrics` subtypes), and match-level metrics (`BaseGameMetrics` subtypes).

---

## 5. Extending the Platform

### Adding a New Game
1. Create a `<GameName>.Common` project referencing `TTOptimize.Core` and implement `IGameState`, `IPlayer`, `IGameConfiguration`, `IGameStateFactory`, `IGameRules`. Define a game-specific `GameAction` set
2. Create a `<GameName>.Gameplay` project referencing `TTOptimize.Core` and `<GameName>.Common` and implement `IGameStateFactory`, `IGameRules`, `IActionExplorer`, `IStateReducer`, along with your game-specific actions and reducers.
3. *(Optional)* Implement `IMatchMetricsExtractor` to capture game-specific end-state metrics and expose `KnownActionTypes`.
4. *(Optional)* Create a `<GameName>.CLI` project implementing `IGameView` and `IGameInput`.
5. Register all services as *Keyed Services* using a unique `gameId` in `ConsoleHost` using a Registration static class with an extension method.

### Adding a New AI / Bot Agent
1. Implement `IAgent` in `<GameName>.Agents`.
2. Provide a unique, deterministic **`StatisticsSubjectId`** (e.g., `"BotName:Version"`) so the statistics module can track its performance across runs.
3. Register the agent type in the game's `SetupService`.

# Repository Agent Skills, Custom Agents & MCP

This repository vendors curated skills, custom agents, and MCP configuration tailored for **WebApplicationRestaurant** (ASP.NET Core 11 MVC, EF Core 11 SQL Server, ASP.NET Core Identity, C# 15).

## MCP Configuration

- [`.mcp.json`](../.mcp.json): Configures the official **Microsoft Learn MCP Server** (`https://learn.microsoft.com/api/mcp`, Streamable HTTP, no authentication required) exposing `microsoft_docs_search`, `microsoft_docs_fetch`, and `microsoft_code_sample_search`.

## Vendored Skills (`.agents/skills/`)

### From [`microsoftdocs/mcp`](https://github.com/microsoftdocs/mcp) (`f0a9478a954c9b3a7c6525a5183466902ca60731`, MIT)

- `microsoft-docs` (`skills/microsoft-docs/SKILL.md`) — Official Microsoft documentation search & fetch.
- `microsoft-code-reference` (`skills/microsoft-code-reference/SKILL.md`) — API signature verification and official Microsoft code samples.
- `microsoft-learn-cli` (`cli/assets/microsoft-learn-cli/SKILL.md`) — CLI fallback via `npx @microsoft/learn-cli` when MCP tools are unavailable.

### From [`dotnet/skills`](https://github.com/dotnet/skills) (`c227688ed14de4467e3e45f95dc69a94e905e219`, MIT)

- `optimizing-ef-core-queries` (`plugins/dotnet-data/skills/optimizing-ef-core-queries/`) — EF Core query performance and correctness review.
- `create-datadriven-aspnetcore` (`plugins/dotnet-data/skills/create-datadriven-aspnetcore/`) — Data-driven ASP.NET Core MVC controllers & Razor Views backed by `DbContext`.
- `csharp-refactoring` (`plugins/dotnet/skills/csharp-refactoring/`) — Behavior-preserving C# refactoring workflow and operation catalog.
- `analyzing-dotnet-performance` (`plugins/dotnet-diag/skills/analyzing-dotnet-performance/`) — .NET performance anti-pattern scan (~50 patterns with reference guides).
- `system-text-json-net11` (`plugins/dotnet11/skills/system-text-json-net11/`) — New `System.Text.Json` APIs in .NET 11.
- `scaffold-dotnet-test-project` (`plugins/dotnet-test/skills/scaffold-dotnet-test-project/`) — Creating and wiring `.NET` test projects into the solution.
- `code-testing` (`plugins/dotnet-test/skills/code-testing/`) — Designing, generating, and fixing tests.
- `run-tests` (`plugins/dotnet-test/skills/run-tests/`) — Running/filtering `dotnet test` and collecting coverage.
- `test-anti-patterns` (`plugins/dotnet-test/skills/test-anti-patterns/`) — Auditing test suites for anti-patterns.

## Custom Agents (`.github/agents/`)

### From [`dotnet/skills`](https://github.com/dotnet/skills) (`c227688ed14de4467e3e45f95dc69a94e905e219`, MIT)

- [`.github/agents/optimizing-dotnet-performance.agent.md`](../.github/agents/optimizing-dotnet-performance.agent.md) — Two-pass .NET performance architect agent paired with `analyzing-dotnet-performance`.

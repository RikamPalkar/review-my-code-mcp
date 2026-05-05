# MCP C# Code Review Server

## MCP Connection JSON

Use one of the following entries in your MCP client configuration.

### Option A: Run from csproj

```json
{
  "mcpServers": {
    "csharp-code-review": {
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "/absolute/path/to/McpCodeReviewServer.csproj"
      ]
    }
  }
}
```

### Option B: Run from built dll

```json
{
  "mcpServers": {
    "csharp-code-review": {
      "command": "dotnet",
      "args": [
        "/absolute/path/to/bin/Debug/net8.0/McpCodeReviewServer.dll"
      ]
    }
  }
}
```

For production use, prefer a Release build path:
- /absolute/path/to/bin/Release/net8.0/McpCodeReviewServer.dll

## What This Project Is

This project is a production-ready MCP server built in C# and .NET. It provides automated code review tools focused on C# quality, security, async correctness, performance, maintainability, naming conventions, and project organization signals.

The server communicates over stdio transport, which makes it easy to plug into MCP-compatible AI clients.

## AI Client Compatibility

This server can be connected to any MCP-compatible AI client app, including:

- Cursor
- Visual Studio Code MCP-capable clients
- Claude Code
- GitHub Copilot environments that support MCP
- Other MCP-compatible tools

If a client supports custom MCP server definitions (command + args), it can connect to this server.

## Main MCP Tools

### review_csharp_code

Reviews C# source input and returns structured JSON with:
- summary
- score
- issues list (severity, category, line, description, fix)

### health_check

Returns server health and timestamp.

### get_rule_backlog

Reads the Add New Rules sections from category markdown files and returns pending proposals as JSON.

## Project Architecture

The project is organized by separation of concerns:

- Tools
  - MCP tool endpoints only
- Services
  - analysis engine, scoring, rule backlog reader, DI registration
- Rules
  - category-based providers and reusable rule abstractions
- Models
  - response contracts
- documentation/rule-catalog
  - rule documentation per category

## Current Rule Categories

- Async
- Security
- Performance
- Maintainability
- Method
- Type Design (class/interface/record)
- File and Folder
- CSharp Modernization

## Rule Documentation Workflow

Each rule category has a dedicated markdown file under documentation/rule-catalog.

Each category file has exactly two sections:

1. Existing Rules
- Rules already implemented in code.

2. Add New Rules
- Backlog entries for future implementation.
- The get_rule_backlog tool reads these entries.

You can extend the project by adding your own entries in the Add New Rules section of each category markdown file. This lets teams propose new checks without changing code first.

## How to Run Locally

### Prerequisites

- .NET SDK installed
- .NET 8 runtime (project targets net8.0)

### Build

dotnet build

### Run

dotnet run --project McpCodeReviewServer.csproj

## How to Connect an AI Client

1. Open your AI client MCP configuration.
2. Add one server entry using either the csproj or dll JSON from the top of this README.
3. Restart the client or reload MCP servers.
4. Verify the server is connected and tools are visible.
5. Call review_csharp_code with C# source content.

## Example review_csharp_code input shape

Send raw C# code as the code argument and optional maxIssues integer.

Example intent:
- code: full C# source
- maxIssues: 50

## Development Notes

- Rules are implemented as pluggable providers.
- Add new executable checks by creating or extending a provider in Rules.
- Register provider in Services/ServiceCollectionExtensions.cs.
- Keep docs in sync by updating the corresponding documentation/rule-catalog category file.

## Extending Rules via Markdown

To extend this project, open the category file in documentation/rule-catalog and add a new bullet under Add New Rules.

Suggested entry format:
- Rule name: <short rule title>; Category: <category>; Severity: <critical|warning|suggestion>; Detection: <pattern/condition>; Fix: <recommended action>

After adding entries, call get_rule_backlog to verify your new items are discoverable by MCP clients.

## Production Guidance

- Use Release build output for client integration.
- Pin package versions in CI.
- Add automated tests for each rule provider.
- Validate new rules against false positive and false negative scenarios.

## Quick File Map

- Program.cs
- Tools/CodeReviewTool.cs
- Services/ReviewAnalyzer.cs
- Services/ReviewScorer.cs
- Services/MarkdownRuleBacklogService.cs
- Rules/Abstractions/*
- Rules/<Category>/*
- documentation/rules.md
- documentation/rule-catalog/*.md

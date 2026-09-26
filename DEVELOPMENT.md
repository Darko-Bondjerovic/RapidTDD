# Development & Technical Stack

This file is for contributors who want to build RapidTDD from source.

### Requirements

- Visual Studio 2019 (for current branch)
- .NET Framework 4.7.2 SDK
- WinForms workload

### NuGet Dependencies

- **FastColoredTextBox** 2.16.24 - code editor
- **DockPanelSuite** 3.0.6 - docking UI
- **Microsoft.CodeAnalysis** 3.9.0 (netstandard2.0)
- **Microsoft.CodeAnalysis.CSharp** 3.9.0 (netstandard2.0)
- **Microsoft.CodeAnalysis.Common** 3.9.0

> Roslyn packages are on netstandard2.0 to keep compatibility with .NET Framework 4.7.2.

### Building

1. Clone repo
2. Open `RapidTDD.sln` in VS2019
3. Restore NuGet packages
4. Build in Release
5. Output is in `/bin/Release`

See [ROADMAP.md](./ROADMAP.md) for planned migration to VS2022 / .NET 4.8.1 / Roslyn 5.9.

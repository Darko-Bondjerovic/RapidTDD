# Roadmap

This document describes the planned technical migration for RapidTDD.

### Current Stack (2024 - stable)

- **IDE:** Visual Studio 2019
- **Framework:** .NET Framework 4.7.2
- **UI:** WinForms
- **Editor:** FastColoredTextBox 2.16.24
- **Docking:** DockPanelSuite 3.0.6
- **Compiler Platform:** Roslyn packages on netstandard2.0
  - Microsoft.CodeAnalysis 3.9.0
  - Microsoft.CodeAnalysis.CSharp 3.9.0

This version is stable and used for all current releases.

### Planned Stack (2026)

```
2024: VS2019 / .NET Framework 4.7.2 / Roslyn 3.9
        ↓
2026: VS2022 / .NET Framework 4.8.1 / Roslyn 5.9
```

#### Goals for 2026 migration:

- [ ] Migrate solution from VS2019 to **VS2022**
- [ ] Upgrade from **.NET Framework 4.7.2 to 4.8.1**
- [ ] Upgrade Roslyn from **3.9.0 to 5.9.0** (latest)
- [ ] Keep WinForms + FastColoredTextBox + DockPanelSuite compatibility
- [ ] Test Code Coverage and in-memory compilation after Roslyn upgrade
- [ ] Prepare ground for future .NET 10 / Roslyn 5.9.0 (2026) and VS extension

### Notes

This migration is not about adding new user features, but about keeping the project maintainable, secure and compatible with modern Roslyn.

If you want to help with the migration, open an issue with title `[Roadmap 2026]`.

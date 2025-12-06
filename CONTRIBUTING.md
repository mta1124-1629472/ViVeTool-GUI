# Contributing to ViVeTool GUI

Thank you for your interest in contributing to ViVeTool GUI! This document provides guidelines and instructions for contributing.

## How to File Issues

### Bug Reports
- Use the [Bug Report template](.github/ISSUE_TEMPLATE/bug_report.md)
- Include your Windows build and ViVeTool-GUI version
- Provide clear steps to reproduce the issue
- Include screenshots or logs if applicable

### Feature Requests
- Use the [Feature Request template](.github/ISSUE_TEMPLATE/feature_request.md)
- Describe the motivation and scope of the feature
- Consider the impact on existing functionality

## How to Submit Pull Requests

1. **Fork the repository** and create your branch from `master`
2. **Make your changes** following the coding standards below
3. **Test your changes** locally using Visual Studio 2022
4. **Commit your changes** with clear, descriptive commit messages
5. **Push to your fork** and open a Pull Request

### Branching Guidelines
- Create feature branches from `master`
- Use descriptive branch names (e.g., `feature/scanner-improvements`, `fix/crash-on-load`)
- Keep branches focused on a single feature or fix

### Commit Expectations
- Write clear, concise commit messages
- Reference related issues when applicable (e.g., "Fixes #123")
- Keep commits atomic — one logical change per commit

## Coding Standards

### Language & Framework
- **Primary language:** Visual Basic .NET
- **Current target:** .NET Framework 4.8 (WinForms)
- **Future target:** .NET 8 (WPF migration in progress)

### Style Guidelines
- Follow existing code patterns and naming conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and reasonably sized

## Testing & Building

### Build Requirements
- Visual Studio 2022
- Windows 10 Version 2004 or later / Windows 11
- .NET Framework 4.8

### Build Steps
1. Open `ViVeTool_GUI.sln` in Visual Studio 2022
2. Restore NuGet packages when prompted
3. Reference the required libraries:
   - `Albacore.ViVe` from `lib` folder or [build it yourself](https://github.com/thebookisclosed/ViVe/tree/master/ViVe)
   - Telerik libraries from `lib\RCWF\2021.3.1109.40` folder
4. Build and run

See [building.md](building.md) for more detailed instructions.

## Licensing

This project is licensed under the **GNU General Public License v3.0 (GPLv3)**. By contributing, you agree that your contributions will be licensed under the same license.

- Keep GPLv3 headers intact in source files
- Do not remove existing licensing text
- See [LICENSE](LICENSE) for the full license text

## Third-Party Dependencies

### Current Dependencies
- **Albacore.ViVe** — Core ViVe API for Windows feature control
- **Telerik UI for WinForms** — UI components (legacy; plan to remove in WPF migration)
- **AutoUpdater.NET** — Application update functionality
- **CrashReporter.Net** — Crash reporting
- **Newtonsoft.JSON** — JSON parsing

### Dependency Guidelines
- Avoid adding new dependencies unless necessary
- Prefer open-source libraries compatible with GPLv3
- The WPF migration path will remove Telerik dependencies

## Security

- **Do not commit secrets or tokens** to the repository
- Report security vulnerabilities privately — see [SECURITY.md](SECURITY.md)
- Be mindful of code that interacts with the Windows Feature Store

## Questions?

If you have questions about contributing, feel free to open a discussion or reach out through the [Discord server](https://discord.gg/8vDFXEucp2).

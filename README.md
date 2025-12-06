# ViVeTool GUI

<div align="center">

![GitHub all releases](https://img.shields.io/github/downloads/peterstrick/vivetool-gui/total?label=downloads)
![GitHub License](https://img.shields.io/github/license/peterstrick/vivetool-gui)


**Windows Feature Control GUI based on ViVeTool**

</div>

## Overview

ViVeTool GUI is a modern, user-friendly graphical interface for managing hidden Windows features in Insider Preview builds. It provides an intuitive way to enable, disable, and search for features without requiring command-line expertise.

**Key Highlights:**
- 🎯 Simple button-based feature management
- 🔍 Powerful search and filtering capabilities
- 🌙 Modern UI with light/dark/system theme support (WPF)
- ⚙️ Advanced feature scanning and discovery tools
- 📋 Community-maintained feature lists

## Getting Started

### System Requirements

- **Windows Version:** Windows 10 Build 18963 (Version 2004) or newer
- **WPF Version (.NET 9):** Recommended for latest features and Windows 11 Fluent design
- **WinForms Version (.NET Framework 4.8):** Legacy support for older systems

### Installation

1. Download the latest release from [GitHub Releases](https://github.com/mta1124-1629472/ViVeTool-GUI/releases)
2. Run the executable (no installation required)
3. Select your Windows build
4. Start managing features

## How to Use

### Method 1: Browse and Select Features

1. Select your Windows build from the dropdown
2. Wait for the feature list to load
3. Expand feature groups by clicking the arrow
4. Select the feature you want to modify
5. Click **"Perform Action"** and choose your desired action

### Method 2: Manually Enter Feature ID

1. Click **"Manually change a Feature"**
2. Enter a Feature ID
3. Click **"Perform Action"** and select your action

## Features

### Core Capabilities

- **Enable/Disable Features** - Easily toggle hidden Windows features on and off
- **Search & Filter** - Quickly find features by name, ID, or description
- **Feature Organization** - Group features by:
  - Always Enabled
  - Always Disabled
  - Enabled by Default
  - Disabled by Default
  - Modifiable
- **Smart Sorting** - Sort by feature name, ID, or current state
- **Clipboard Integration** - Right-click to copy feature names and IDs

### Advanced Features

- **Multi-Build Support** - Load feature lists for different Windows builds
- **Theme Customization** - Dark, light, and system mode support
- **Feature Scanner** - Scan your Windows build to discover hidden features

## Version Comparison: WPF vs Legacy

### WPF Version (.NET 9) - Recommended

**Modern Features:**
- ✅ **Windows 11 Fluent Design** - Native Windows 11 UI with accent color integration
- ✅ **ThemeMode API** - System theme detection and integration
- ✅ **Modern .NET Runtime** - Better performance and security updates
- ✅ **Enhanced Accessibility** - Improved keyboard navigation and screen reader support
- ✅ **Smart Feature Feed Caching** - ETag-based caching with offline fallback
- ✅ **Feature Scanner Integration** - Built-in discovery tools for new features
- ✅ **GitHub Actions Publishing** - Direct feature list publishing for maintainers

**Not Implemented:**
- ❌ Store/A/B Testing repair tools
- ❌ Multi-language localization

**Recommended For:**
- Windows 11 users
- Users wanting the latest UI/UX improvements
- Advanced feature scanning needs
- Active Insider program participants

### Legacy WinForms Version (.NET Framework 4.8)

**Capabilities:**
- ✅ Core feature enable/disable functionality
- ✅ Search and filtering
- ✅ Feature grouping and sorting
- ✅ Basic theme support (Windows default)
- ✅ Multi-build feature list loading
- ✅ Multi-language localization support
- ✅ Store/A/B Testing repair tools
- ✅ LastKnownGood Store fixing capabilities

**Not Implemented:**
- ❌ Windows 11 Fluent Design
- ❌ Accent color integration
- ❌ Smart caching system (ETag-based)
- ❌ GitHub Actions publishing
- ❌ Modern .NET runtime benefits

**Recommended For:**
- Windows 10 users
- Users needing store repair functionality
- Users requiring multi-language support
- Legacy system compatibility
- Minimal resource usage requirements

### Direct Feature Comparison

| Feature | WPF Version | Legacy WinForms |
|---------|------------|----------------|
| **UI Framework** | Windows 11 Fluent | Classic WinForms |
| **Theme Support** | Light/Dark/System | Windows Default |
| **Multi-Language Support** | ❌ | ✅ |
| **Store Repair Tools** | ❌ | ✅ |
| **A/B Testing Fix** | ❌ | ✅ |
| **Feature Scanner** | ✅ Advanced | ⚠️ Limited |
| **Smart Caching** | ✅ ETag-based | ⚠️ Basic |
| **GitHub Publishing** | ✅ Native | ❌ |
| **Keyboard Accessibility** | ✅ Enhanced | ⚠️ Standard |
| **Performance** | ✅ Modern .NET 9 | ⚠️ .NET Framework |
| **Security Updates** | ✅ Current | ⚠️ Legacy support |
| **Windows 10 Support** | ✅ Works | ✅ Optimized |
| **Windows 11 Support** | ✅ Optimized | ✅ Works |

### Migration Path

If you're currently using the **Legacy WinForms version**, see [MIGRATION_NOTES.md](https://github.com/mta1124-1629472/ViVeTool-GUI/blob/master/MIGRATION_NOTES.md) for detailed migration instructions to the new WPF version.

**Note:** Some features from the Legacy version (Store repair, multi-language support) are not yet implemented in the WPF version. This is being tracked for future releases.

## Feature Feed System

ViVeTool GUI uses a GitHub-hosted feature feed to provide current feature lists for supported Windows builds.

### Feed Architecture

- **latest.json** - Metadata about available builds and latest build numbers
- **features/{build}/features.csv** or **features.json** - Per-build feature lists

### Feature Feed Usage

The application automatically fetches feature lists from the feed with:
- **Smart Caching** - Minimizes bandwidth usage
- **Offline Fallback** - Uses cached data when network is unavailable
- **Legacy Support** - Falls back to mach2 format for older builds

### Feature Scanner & Publishing

**For Power Users & Maintainers:**

1. Launch the Feature Scanner from the main application
2. Run a complete scan of your Windows build
3. Click **"Publish via GitHub Actions"** in the publish panel
4. Provide your build number, select format (CSV/JSON), and authenticate
5. Your feature list is automatically committed to the repository

**Note:** Publishing requires maintainer permissions. Contact repository maintainers if you encounter 403 errors.

## Why Use ViVeTool GUI?

Compared to the command-line ViVeTool, ViVeTool GUI offers:

| Feature | CLI ViVeTool | ViVeTool GUI |
|---------|--------------|---------------|
| User-Friendly UI | ❌ | ✅ |
| Feature Search | ⚠️ Limited | ✅ Full-featured |
| One-Click Actions | ❌ | ✅ |
| Modern Theme Support | ❌ | ✅ (WPF) |
| Feature Scanning | ⚠️ Limited | ✅ Advanced |

## Project Structure

```
ViVeTool-GUI/
├── ViVeTool-GUI.Wpf/          # Modern WPF application (recommended)
├── ViVeTool-GUI.FeatureScanner/ # Feature discovery and scanning tools
├── Albacore.ViVe/              # ViVe API wrapper and core functionality
├── vivetool-gui/               # Legacy WinForms version
├── lib/                        # External dependencies
├── images/                     # UI screenshots and icons
├── MIGRATION_NOTES.md          # WPF migration guide
└── building.md                 # Build instructions
```

## Building from Source

For detailed build instructions, see [building.md](https://github.com/mta1124-1629472/ViVeTool-GUI/blob/master/building.md).

**Quick Start:**
```bash
git clone https://github.com/mta1124-1629472/ViVeTool-GUI.git
cd ViVeTool-GUI
dotnet build -c Release
```

## Troubleshooting

### Feature List Won't Load
- Ensure you're connected to the internet (first-time setup)
- Check that your Windows build number is supported
- Try clearing the local cache and restarting

### "Access Denied" Errors
- Run ViVeTool GUI as Administrator
- Ensure your user account has sufficient privileges
- Check Windows Defender or antivirus software isn't blocking the app

## Disclaimer

⚠️ **Important:** Modifying Windows features can affect system stability and performance. Use this tool at your own risk.

No one—including the ViVeTool GUI creators, the [ViVeTool developers](https://github.com/thebookisclosed/ViVe), or the [mach2 creators](https://github.com/riverar/mach2)—is responsible for any damage or unintended side effects resulting from feature modifications. Always create a system restore point before making changes.

## Credits & Attribution

**Built With:**
- [ViVeTool](https://github.com/thebookisclosed/ViVe) - Core feature management API
- [mach2](https://github.com/riverar/mach2) - Feature scanning and legacy data
- [icons8.com](https://icons8.com/) - UI icons

**Special Thanks:**
- [PeterStrick](https://github.com/PeterStrick) - Original ViVe GUI creator
- [The Book Is Closed](https://github.com/thebookisclosed) - ViVeTool & ViVe authors
- [Rivera](https://github.com/riverar) - mach2 developer

## License

ViVeTool GUI is open source. See [LICENSE](https://github.com/mta1124-1629472/ViVeTool-GUI/blob/master/LICENSE) for details.

## Community & Support

- **Discord Server:** [Join our community](https://discord.gg/8vDFXEucp2)
- **Issue Tracker:** [GitHub Issues](https://github.com/mta1124-1629472/ViVeTool-GUI/issues)
- **Discussions:** [GitHub Discussions](https://github.com/mta1124-1629472/ViVeTool-GUI/discussions)
- **Code Quality:** [SonarCloud Analysis](https://sonarcloud.io/summary/new_code?id=PeterStrick_ViVeTool-GUI)

## Contributing

Contributions are welcome! Whether it's:
- 🐛 Bug reports and fixes
- ✨ Feature suggestions and implementation
- 📚 Documentation improvements

See our guidelines in [building.md](https://github.com/mta1124-1629472/ViVeTool-GUI/blob/master/building.md) for more information.

---

<div align="center">

**Made with ❤️ for Windows power users and Insider testers**

[⬆ Back to top](#vivetool-gui)

</div>

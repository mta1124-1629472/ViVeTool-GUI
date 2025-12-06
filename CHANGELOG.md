# Changelog

All notable changes to ViVeTool GUI are documented in this file. This project follows [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Planned
- Multi-language localization support for WPF version
- Store repair functionality for WPF version
- Feature export to CSV/JSON
- Improved feature scanner for recent Windows builds
- Advanced filtering and search operators

## [Current Stable]

### WPF Version (.NET 9)

#### Features
- Windows 11 Fluent Design UI
- Light/Dark/System theme support with accent color integration
- Modern .NET 9 runtime for improved performance and security
- Smart ETag-based feature feed caching
- Offline fallback for cached feature lists
- Advanced feature scanner and discovery tools
- GitHub Actions integration for publishing feature lists
- Enhanced keyboard navigation and accessibility support
- Real-time feature search and filtering
- Feature grouping by state (Always Enabled, Always Disabled, etc.)
- Right-click clipboard integration for feature names/IDs
- Multi-build feature list support

#### Known Limitations
- No multi-language support (planned for future release)
- No store repair tools yet (planned for future release)
- No feature export functionality (planned for future release)
- Theme detection may require restart on theme change

### Legacy WinForms Version (.NET Framework 4.8)

#### Features
- Core feature enable/disable functionality
- Feature search and filtering
- Feature grouping by state
- Smart sorting (by name, ID, or state)
- Multi-build feature list support
- Multi-language localization (10+ languages)
- Store/A/B Testing repair tools
- LastKnownGood Store fixing
- Basic theme support (Windows default)
- Right-click clipboard integration

#### Status
- **No longer actively developed** - Maintained for compatibility only
- Receives critical bug fixes only
- No new features planned
- Recommended for Windows 10 compatibility only

## Migration from Legacy to WPF

See [MIGRATION_NOTES.md](MIGRATION_NOTES.md) for detailed migration information.

### What's New in WPF vs Legacy

✅ **WPF Improvements:**
- Modern Windows 11 UI
- Better performance on modern systems
- Enhanced accessibility
- Active development and updates
- Smart caching system

⚠️ **Note:** Some features from Legacy version are not yet in WPF:
- Multi-language support
- Store repair tools
- Feature export

## Release History

### Previous Releases

For detailed history of previous releases, see [GitHub Releases](https://github.com/mta1124-1629472/ViVeTool-GUI/releases).

## Contributing

Want to contribute? See [CONTRIBUTING.md](building.md) for guidelines on:
- Reporting bugs
- Suggesting features
- Contributing code
- Improving documentation

## Version Scheme

ViVeTool GUI uses Semantic Versioning:

- **MAJOR** version for incompatible API/feature changes
- **MINOR** version for new functionality in backward-compatible manner
- **PATCH** version for bug fixes

Example: `2.1.0`
- 2 = Major version
- 1 = Minor version
- 0 = Patch version

## Support

### Getting Help

- **Discord Community:** [Join our Discord](https://discord.gg/8vDFXEucp2)
- **GitHub Issues:** [Report bugs](https://github.com/mta1124-1629472/ViVeTool-GUI/issues)
- **GitHub Discussions:** [Ask questions](https://github.com/mta1124-1629472/ViVeTool-GUI/discussions)
- **Known Issues:** See [KNOWN_ISSUES.md](KNOWN_ISSUES.md)

### End of Life

- **Legacy WinForms Version:** Maintenance only - use WPF version for active development
- **Old Windows Builds:** Support limited to builds with published feature lists

## Acknowledgments

Thank you to all contributors, translators, and community members who help make ViVeTool GUI better!

- [PeterStrick](https://github.com/PeterStrick) - Original creator
- [The Book Is Closed](https://github.com/thebookisclosed) - ViVeTool & ViVe authors
- [Rivera](https://github.com/riverar) - mach2 developer
- All community contributors

---

**Last Updated:** December 2025

For the latest changes, check the [GitHub Commit History](https://github.com/mta1124-1629472/ViVeTool-GUI/commits/master).

# Frequently Asked Questions (FAQ)

## General Questions

### What is ViVeTool GUI?

ViVeTool GUI is a graphical user interface for managing hidden Windows features in Insider Preview builds. It allows you to enable or disable experimental features without needing to use the command line, making it more accessible to non-technical users.

### Do I need to be in the Insider program to use this?

Yes. ViVeTool GUI is designed specifically for Windows Insider Preview builds. Regular released versions of Windows have very limited hidden features to control.

### Is this safe to use?

ViVeTool GUI itself is safe, but **modifying Windows features can affect system stability**. Always:
- Create a system restore point before making changes
- Test in a VM first if possible
- Understand what each feature does before enabling it
- Only enable features you intend to use

See [KNOWN_ISSUES.md](KNOWN_ISSUES.md) for troubleshooting help if issues occur.

### Who created this?

ViVeTool GUI was originally created by [PeterStrick](https://github.com/PeterStrick). It's based on the [ViVeTool](https://github.com/thebookisclosed/ViVe) project and uses the [mach2](https://github.com/riverar/mach2) feature lists.

## Installation & Setup

### How do I install ViVeTool GUI?

No installation needed:
1. Download the latest release from [GitHub Releases](https://github.com/mta1124-1629472/ViVeTool-GUI/releases)
2. Extract the ZIP file
3. Run the executable (ViVeTool-GUI.exe)

That's it! No installers, no registry changes.

### Should I use WPF or Legacy version?

**Use WPF if:**
- You're on Windows 11
- You want the latest features and UI
- You like modern dark/light theme support
- You want to scan and publish features

**Use Legacy if:**
- You're on Windows 10
- You need multi-language support
- You need store repair tools
- You prefer the classic interface

See [README.md](README.md) for detailed version comparison.

### Do I need Administrator rights?

Yes. Windows feature modifications require Administrator privileges. Run as Administrator (right-click → Run as administrator).

### Can I run this portably (USB drive)?

Yes, both versions run portably with no installation. Just copy the executable to your USB drive and run it.

## Usage Questions

### How do I enable a feature?

**Method 1 - Browse:**
1. Select your Windows build
2. Expand a feature group
3. Click a feature
4. Click "Perform Action" → "Enable"

**Method 2 - Manual:**
1. Click "Manually change a Feature"
2. Enter the Feature ID
3. Click "Perform Action" → "Enable"

### Why won't my changes persist?

Common causes:
- Feature is "Always Enabled" or "Always Disabled" (cannot change)
- You don't have admin rights
- Windows Update reset it
- Need to restart system
- Antivirus is interfering

See [KNOWN_ISSUES.md](KNOWN_ISSUES.md#feature-changes-dont-persist) for solutions.

### What do the feature groups mean?

- **Always Enabled** - Cannot be disabled by users
- **Always Disabled** - Cannot be enabled by users
- **Enabled by Default** - Off by default, but you can disable it
- **Disabled by Default** - Off by default, you can enable it
- **Modifiable** - Can be toggled on/off freely

### Can I search for features by name?

Yes, use the search box to filter features by name, description, or ID. Searches are real-time.

### Can I sort the feature list?

Yes, click the sort button to sort by:
- Feature Name (A-Z)
- Feature ID
- Feature State (enabled/disabled)

### How do I copy a feature ID?

Right-click the feature name → "Copy" (the ID is copied to clipboard).

## Feature Lists & Feed

### What is the "Feature Feed"?

The Feature Feed is a GitHub-hosted list of Windows features organized by build number. ViVeTool GUI automatically downloads and caches these lists so you always have the latest features.

### Why is the feature list empty?

Possible causes:
- Your Windows build has no published features yet
- Internet connection issue
- Feature list hasn't been downloaded (first launch)
- Cache is corrupted

See [KNOWN_ISSUES.md](KNOWN_ISSUES.md#feature-list-wont-load) for solutions.

### Can I load features for a different build?

Yes, use the build selector dropdown to switch between builds. Your Windows build and others with published feature lists are available.

### How often are feature lists updated?

Feature lists are updated when:
- New Windows Insider builds are released
- The community discovers new features
- Maintainers scan new builds

Typically weekly or bi-weekly during active Insider seasons.

## WPF Version Questions

### Can I change the theme?

Yes, the WPF version supports:
- **Light Mode** - Traditional white UI
- **Dark Mode** - Dark UI for low-light environments
- **System Mode** - Follows Windows system theme

Theme option is in Settings or top menu.

### Does it use my Windows accent color?

Yes, the WPF version integrates with your Windows accent color (Settings → Personalization → Colors).

### Can I use WPF on Windows 10?

Yes, WPF version works on Windows 10, but:
- Fluent Design may not render identically
- Some modern theme features unavailable
- Overall experience is optimized for Windows 11

### What is the Feature Scanner?

The Feature Scanner is a tool that scans your Windows build to discover hidden features. It:
- Finds all features available on your system
- Generates a feature list
- Can publish the list to GitHub (for maintainers)

Useful for testing new Insider builds.

### How do I publish features with GitHub Actions?

1. Run Feature Scanner and complete scan
2. Click "Publish via GitHub Actions"
3. Enter build number and select format (CSV/JSON)
4. Authenticate with GitHub token
5. Your feature list is committed to the repository

**Note:** Only repository maintainers can publish. Non-maintainers get a 403 error.

## Legacy Version Questions

### Is the Legacy version still supported?

Legacy is maintenance-only:
- Receives critical bug fixes
- No new features planned
- Recommended for Windows 10 only
- Consider migrating to WPF for continued development

### Can I migrate from Legacy to WPF?

Yes, see [MIGRATION_NOTES.md](MIGRATION_NOTES.md) for detailed instructions.

### Does Legacy have store repair?

Yes, the Legacy version includes store repair tools:
- Fix LastKnownGood Store
- Reset A/B Testing Priorities

(WPF version planned to add this soon)

### How do I use store repair?

Legacy version:
1. Find "Store Repair" or "Fix Store" option
2. Click to run repair
3. Restart Windows

## Troubleshooting

### App won't start

**Try:**
1. Run as Administrator
2. Disable antivirus temporarily
3. Update .NET runtime (WPF) or .NET Framework (Legacy)
4. Check Windows build compatibility

### "Access Denied" error

**Solutions:**
1. Run as Administrator
2. Disable antivirus
3. Check user permissions
4. Try Safe Mode

See [KNOWN_ISSUES.md](KNOWN_ISSUES.md#access-denied-when-enabling-features) for more.

### Feature Scanner doesn't work

**Try:**
1. Run as Administrator
2. Disable antivirus
3. Ensure 1GB+ free disk space
4. Try on fresh Windows install
5. Check build is supported

See [KNOWN_ISSUES.md](KNOWN_ISSUES.md#feature-scanner-crashes-or-hangs) for details.

### GitHub token keeps failing

**Check:**
1. Token has `repo` + `workflow` scopes
2. Token hasn't expired
3. You're a repository maintainer
4. Internet connection is stable

See [KNOWN_ISSUES.md](KNOWN_ISSUES.md#github-actions-publishing-fails) for troubleshooting.

## Advanced Questions

### Can I use this on non-Insider builds?

Technically yes, but:
- Very few hidden features available
- Feature lists may be incomplete
- Not recommended or tested
- Better to use official Windows Insider program

### Can I export features to a file?

**WPF:** Not yet (planned for future release)
**Legacy:** Yes, through the legacy interface

Workaround: Access feature lists directly from [GitHub](https://github.com/mta1124-1629472/ViVeTool-GUI/tree/master/features)

### What's the difference between ViVeTool CLI and ViVeTool GUI?

| Feature | CLI | GUI |
|---------|-----|-----|
| User-Friendly | ❌ | ✅ |
| Search | Limited | Full |
| Modern UI | ❌ | ✅ |
| Feature Scanning | Basic | Advanced |
| Scripting | ✅ | ❌ |

### Can I script this?

No, ViVeTool GUI is not designed for scripting. Use command-line ViVeTool instead:
```bash
vivetool /enable /id:<feature_id>
```

### Can I contribute translations?

**WPF:** Yes, planned for future release (Weblate platform)
**Legacy:** Yes, see [LOCALIZE.md](LOCALIZE.md)

### Can I contribute code?

Yes! See [building.md](building.md) for development setup and contribution guidelines.

## Community & Support

### Where can I get help?

- **Discord:** [Join our community](https://discord.gg/8vDFXEucp2)
- **GitHub Issues:** [Report bugs](https://github.com/mta1124-1629472/ViVeTool-GUI/issues)
- **GitHub Discussions:** [Ask questions](https://github.com/mta1124-1629472/ViVeTool-GUI/discussions)
- **This FAQ:** You're reading it!

### How do I report a bug?

1. Check [KNOWN_ISSUES.md](KNOWN_ISSUES.md) first
2. Search existing [GitHub Issues](https://github.com/mta1124-1629472/ViVeTool-GUI/issues)
3. Open a new issue with:
   - Windows build number
   - ViVeTool GUI version
   - Error message
   - Steps to reproduce

### How do I suggest a feature?

1. Check existing [GitHub Discussions](https://github.com/mta1124-1629472/ViVeTool-GUI/discussions)
2. Open a new discussion
3. Describe the feature and why you need it

### Can I donate?

This is a free, open-source project. The best support is:
- ⭐ Star the repository
- 🐛 Report bugs
- ✨ Suggest features
- 💬 Help other users
- 📝 Contribute code/docs

---

**Can't find your answer here?** [Join our Discord](https://discord.gg/8vDFXEucp2) or [open a discussion](https://github.com/mta1124-1629472/ViVeTool-GUI/discussions)!

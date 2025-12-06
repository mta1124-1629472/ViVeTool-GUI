# Known Issues

This document lists current limitations and known issues with ViVeTool GUI. If you encounter a problem not listed here, please [open an issue](https://github.com/mta1124-1629472/ViVeTool-GUI/issues).

## WPF Version (.NET 9)

### Missing Features (Planned for Future Releases)

- **No Multi-Language Support** - Currently English-only. Localization support planned for a future release.
- **No Store Repair Tools** - Store/A/B Testing repair functionality not yet ported from Legacy version. This is a planned feature.
- **No Feature Export** - Cannot export feature lists to file (WPF version). Available in Legacy version.

### Known Limitations

- **Theme Detection Delay** - System theme changes may require app restart to fully apply
- **Feature Feed Caching** - First load may be slow on slow connections; subsequent loads use cache
- **Limited Build Support** - Only Windows builds with published feature lists are fully supported

### Build-Specific Issues

- **Windows 10 Build Compatibility** - Some WPF UI features may not render identically on Windows 10
- **Non-Insider Builds** - App is designed for Insider Preview builds; general Windows builds have limited feature availability

## Legacy WinForms Version (.NET Framework 4.8)

### End of Life Status

- **Legacy Support Only** - The WinForms version is no longer actively developed
- **No UI Updates** - Will not receive modern theme or design improvements
- **Frozen Feature Set** - No new features will be added

### Known Issues

- **Theme Support Limited** - Only uses Windows system theme; no light/dark mode toggle
- **Performance on Modern Systems** - .NET Framework can be slower than modern .NET on newer hardware
- **No GitHub Integration** - Cannot publish feature lists via GitHub Actions
- **Limited Accessibility** - Older accessibility standards; not fully keyboard-navigable

## Common Issues (Both Versions)

### Feature List Won't Load

**Symptoms:** App shows empty feature list or "Loading..." indefinitely

**Solutions:**
1. Check internet connection (required for first-time feature list download)
2. Verify your Windows build is supported
3. Try clearing cache:
   - WPF: Check AppData\Local\ViVeTool-GUI folder
   - Legacy: Check AppData\Roaming\ViVeTool folder
4. Restart the application
5. Check if GitHub is accessible in your region (feature feed hosted on GitHub)

### "Access Denied" When Enabling Features

**Symptoms:** Error message when trying to enable/disable a feature

**Solutions:**
1. Run as Administrator (right-click → Run as administrator)
2. Disable antivirus/Windows Defender temporarily (they may block writes to Windows feature store)
3. Check User Account Control (UAC) is not blocking the app
4. Ensure user account has Administrator privileges
5. Some features may be unavailable on your build

### Feature Changes Don't Persist

**Symptoms:** Features revert to previous state after restart

**Causes:**
- Feature is locked by Windows ("Always Enabled" or "Always Disabled")
- Windows update reverted changes
- Feature requires system restart to fully apply
- User doesn't have permission to modify that feature

**Solutions:**
1. Check feature state - "Always Enabled/Disabled" features cannot be changed
2. Restart system after enabling/disabling
3. Check Windows Update didn't reset settings
4. Try enabling from Safe Mode (advanced features only)

### Feature Scanner Crashes or Hangs

**Symptoms:** Feature Scanner doesn't respond or crashes during scan

**Solutions:**
1. Ensure sufficient disk space (at least 1GB free)
2. Disable antivirus temporarily
3. Run in Safe Mode (avoids conflicts)
4. Check Windows build is supported (Scanner works best on recent Insider builds)
5. Try on fresh Windows install if issue persists

### GitHub Actions Publishing Fails

**Symptoms:** "403 Forbidden" or "Authentication failed" when publishing

**Solutions:**
1. Verify GitHub token has correct permissions (`repo` + `workflow` scopes)
2. Ensure token has not expired
3. Check you're a repository maintainer
4. For non-maintainers: Contact a project maintainer to publish
5. Verify internet connection is stable

## Platform-Specific Issues

### Windows 11

**WPF Version:**
- ✅ Full support with Fluent Design
- Accent color integration works best with solid accent colors (not gradients)

**Legacy Version:**
- ✅ Works but doesn't utilize Windows 11 features
- May show legacy UI inconsistencies

### Windows 10

**WPF Version:**
- ✅ Works but Fluent Design not fully utilized
- Some UI elements may not render identically

**Legacy Version:**
- ✅ Full compatibility
- Optimized for Windows 10 UI

### Non-English Windows Installations

**Current Status:**
- Legacy version has multi-language support
- WPF version is English-only (localization planned)
- Feature IDs and core functionality work on any language build

## Performance Issues

### App Startup is Slow

**Causes:**
- First-time feature list download
- Antivirus scanning
- System is low on resources

**Solutions:**
1. Disable antivirus scanning for the app executable
2. Close other applications to free resources
3. Upgrade system RAM if consistently slow

### Feature List Scrolling is Laggy

**Causes:**
- Large number of features (1000+)
- Insufficient system RAM
- GPU acceleration disabled

**Solutions:**
1. Use search to filter features (reduces rendered items)
2. Upgrade system RAM
3. Enable GPU acceleration in Windows graphics settings
4. Close other applications

## Reporting Issues

If you encounter a problem not listed here:

1. **Check existing issues:** https://github.com/mta1124-1629472/ViVeTool-GUI/issues
2. **Provide details:**
   - Windows build number (Settings → System → About)
   - ViVeTool GUI version
   - WPF or Legacy version
   - Error message (if any)
   - Steps to reproduce
3. **Attach logs** if available (AppData folder)
4. **Join Discord** for quick community support: https://discord.gg/8vDFXEucp2

## Workarounds

### For Store Repair (WPF Version)

Until store repair is implemented in WPF, you can:
1. Use the Legacy WinForms version for store repair
2. Use command-line ViVeTool: `vivetool /fixStore` (requires admin)
3. Wait for upcoming WPF release with store repair support

### For Multi-Language Support (WPF Version)

Currently:
1. Use Legacy WinForms version for other languages
2. Feature IDs work across all language builds
3. Contribute translations when WPF localization launches

### For Feature Export (WPF Version)

Alternatives:
1. Use Legacy WinForms version to export
2. Access feature lists directly from GitHub feed: https://github.com/mta1124-1629472/ViVeTool-GUI/tree/master/features
3. Use command-line ViVeTool for export functionality

---

**Last Updated:** December 2025

**Have a workaround for an issue?** [Share it with the community!](https://github.com/mta1124-1629472/ViVeTool-GUI/discussions)

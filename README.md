
![GitHub all releases](https://img.shields.io/github/downloads/peterstrick/vivetool-gui/total)
![GitHub License](https://img.shields.io/github/license/peterstrick/vivetool-gui)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/peterstrick/vivetool-gui)
[![](https://dcbadge.vercel.app/api/server/8vDFXEucp2?style=flat)](https://discord.gg/8vDFXEucp2)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=PeterStrick_ViVeTool-GUI&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=PeterStrick_ViVeTool-GUI)
[![Translation](https://weblate.rawrr.dev/widgets/vivetool-gui/-/svg-badge.svg)](https://weblate.rawrr.dev/engage/vivetool-gui)

# ViVeTool GUI <img src="/images/icons8-advertisement-page-96.png" alt="ViVeTool GUI Logo" width="32"/> 
### Windows Feature Control GUI based on ViVeTool

## What is ViVeTool GUI?
ViVeTool GUI lets you easily enable, disable and search for new hidden Features in Windows Insider Builds, with the use of a Button and a pretty UI.

## Disclaimer.
### No one, including me, [the creator of ViVe GUI](https://github.com/PeterStrick/ViVeTool-GUI), [the creators of ViVe and ViVeTool](https://github.com/thebookisclosed/ViVe) or [the creators of mach2](https://github.com/riverar/mach2) are responsible for any damage or unintended side effects, this program might cause to your computer, by changing the Windows Feature Store. Use this program at your own risk.

## How to use it?
Using it is simple, 
Either:

1. Select the Build for which you want to enable or disable features for.
2. Wait for it to load in, open one of the Groups by pressing the Arrow, and select the Feature that you are looking for.
3. Press on Perform Action and perform your desired action for the entered feature ID.

<img width="511" height="355" src="/images/Method1.gif" alt="Image showing you how to perform Method 1" />


---

Or:
1. Press on "Manually change a Feature" (F12)
2. Enter a Feature ID
3. Press on Perform Action and perform your desired action for the selected feature.

<img width="511" height="355" src="/images/Method2.gif" alt="Image showing you how to perform Method 2" />

---

## What are the additional features?
Apart from being able to manage features, ViVeTool GUI let´s you also:
- Load in a Feature List of other Builds
- Search for Features 
- Sort Features by Feature Name, Feature ID or Feature State
- Group Features by: Always Enabled, Always Disabled, Enabled by Default, Disabled by Default and Modifiable
- Copy Feature Names and IDs by right-clicking them
- Switch between Dark, Light and System Mode (WPF version supports Windows 11 Fluent theme with accent colors)
- Automatically load the latest Feature List when starting ViVeTool GUI
- Scan a Windows Build for Hidden Features to create your own Feature List
- Use ViVeTool GUI in multiple translated Languages
- Fix the LastKnownGood Store, as well as the A/B Testing Priorities for ViVeTool Features
- and at last, view the About Box by either pressing on the About Icon, or selecting the "About..." Item in the Application System Menu.

<img width="511" height="175" src="/images/Searching.gif" alt="Image showing you how to search" />

## What are the System Requirements?
Since ViVeTool GUI uses the ViVe API, Windows 10 Build 18963 (Version 2004) and newer is the only OS Requirement.

Apart from that, the only Requirement is .Net Framework 4.8 (WinForms version) or .NET 9 (WPF version).

### WPF Version (.NET 9)
The new WPF version features:
- Modern Windows 11 Fluent theme with light/dark/system mode support
- Windows accent color integration
- ThemeMode experimental API (warning WPF0001 is suppressed in the project)

See [MIGRATION_NOTES.md](MIGRATION_NOTES.md) for more details on the WPF version.

## Why not just use ViVeTool?
Using ViVeTool GUI is more easier and user-friendly, besides it lets you also search for features and enable them with a few clicks.

## Feature Feed

ViVeTool GUI uses a GitHub-hosted feature feed to provide up-to-date feature lists for different Windows builds. The feed consists of:

- `latest.json` - Metadata about available builds and the latest build number
- `features/{build}/features.csv` or `features.json` - Per-build feature lists

### Consuming the Feed

The WPF version automatically fetches feature lists from the feed with:
- **ETag-based caching** - Only downloads when content has changed
- **Offline fallback** - Uses cached data when network is unavailable
- **Legacy support** - Falls back to mach2 format for older builds

### Publishing Feature Lists (Maintainers Only)

After running the Feature Scanner to scan a Windows build for hidden features, you can publish the results to the feed:

1. **Launch Feature Scanner** from the WPF app
2. Complete the scan process to generate a feature list
3. Click **"Publish via GitHub Actions"** in the publish panel
4. Enter the build number, select format (CSV/JSON), and provide your GitHub token
5. The workflow will add your feature list to the repository

**Note:** Publishing is restricted to repository maintainers. If you receive a 403 error, please contact a maintainer to publish your feature list.

### Setting up FEED_PUBLISH_TOKEN (Repository Maintainers)

To enable the publish workflow, maintainers need to set up a GitHub secret:

1. Go to **Settings → Secrets and variables → Actions**
2. Click **New repository secret**
3. Name: `FEED_PUBLISH_TOKEN`
4. Value: A Personal Access Token (PAT) with `repo` and `workflow` permissions
5. Click **Add secret**

The token is used by the GitHub Actions workflow to commit changes to the repository. It is never exposed to client code.

# Licensing
ViVeTool GUI uses Icons from [icons8.com](https://icons8.com/)

ViVeTool GUI is inspired by [ViVeTool](https://github.com/thebookisclosed/ViVe) and uses the [ViVe API](https://github.com/thebookisclosed/ViVe/tree/master/ViVe)

ViVeTool GUI uses [files](https://github.com/riverar/mach2/tree/master/features) from [mach2](https://github.com/riverar/mach2) for the Build Combo Box.

ViVeTool GUI - Feature Scanner uses [mach2](https://github.com/riverar/mach2) to create it's Feature Lists



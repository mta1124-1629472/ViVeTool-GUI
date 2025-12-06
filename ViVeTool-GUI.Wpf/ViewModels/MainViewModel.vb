' ViVeTool-GUI - Windows Feature Control GUI for ViVeTool
' Copyright (C) 2022  Peter Strick / Peters Software Solutions
'
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with this program.  If not, see <https://www.gnu.org/licenses/>.

Option Explicit On
Option Strict On

Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Security.Principal
Imports System.Windows.Data
Imports CommunityToolkit.Mvvm.ComponentModel
Imports CommunityToolkit.Mvvm.Input
Imports ViVeTool_GUI.Wpf.Models
Imports ViVeTool_GUI.Wpf.Services

Namespace ViewModels
    ''' <summary>
    ''' Main view model for the application.
    ''' </summary>
    Partial Public Class MainViewModel
        Inherits ObservableObject

        ' Constants for placeholder/warning detection
        ''' <summary>Placeholder text used when no features could be loaded.</summary>
        Private Const NoFeaturesLoadedText As String = "No features loaded"
        ''' <summary>Maximum allowed group value per Windows RTL Feature Manager constraints.</summary>
        Private Const MaxGroupValueText As String = "14"

        ' Constants - Legacy external scanner EXE is no longer used
        ' The scanner is now integrated into the WPF app via FeatureScannerDialog

        Private ReadOnly _featureService As FeatureService
        Private ReadOnly _gitHubService As GitHubService
        Private ReadOnly _themeService As ThemeService
        Private ReadOnly _publishService As PublishService

        Private _features As ObservableCollection(Of FeatureItem)
        Private _featuresView As ICollectionView
        Private _selectedFeature As FeatureItem
        Private _searchText As String = String.Empty
        Private _selectedBuild As String = String.Empty
        Private _availableBuilds As ObservableCollection(Of String)
        Private _statusMessage As String = "Ready"
        Private _isLoading As Boolean
        Private _currentThemeName As String = "Light"

        ' Publish-related fields
        Private _publishBuildNumber As String = String.Empty
        Private _publishArtifactPath As String = String.Empty
        Private _publishArtifactUrl As String = String.Empty
        Private _publishFormat As String = "csv"
        Private _publishGitHubToken As String = String.Empty
        Private _publishStatus As PublishStatus = PublishStatus.Idle
        Private _publishErrorMessage As String = String.Empty
        Private _isMaintainerOnlyError As Boolean
        Private _isPublishing As Boolean
        Private _canShowPublishPanel As Boolean
        Private _availableFormats As ObservableCollection(Of String)

        ' Admin privilege check fields
        Private _isRunningAsAdmin As Boolean
        Private _adminWarningMessage As String = String.Empty
        Private _localFeatureWarning As String = String.Empty

        ''' <summary>
        ''' Gets whether the application is running with administrator privileges.
        ''' </summary>
        Public Property IsRunningAsAdmin As Boolean
            Get
                Return _isRunningAsAdmin
            End Get
            Private Set(value As Boolean)
                SetProperty(_isRunningAsAdmin, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets the admin warning message to display when not running elevated.
        ''' </summary>
        Public Property AdminWarningMessage As String
            Get
                Return _adminWarningMessage
            End Get
            Private Set(value As String)
                SetProperty(_adminWarningMessage, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets the local feature query warning message (e.g., for group errors).
        ''' </summary>
        Public Property LocalFeatureWarning As String
            Get
                Return _localFeatureWarning
            End Get
            Private Set(value As String)
                SetProperty(_localFeatureWarning, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the collection of features.
        ''' </summary>
        Public Property Features As ObservableCollection(Of FeatureItem)
            Get
                Return _features
            End Get
            Set(value As ObservableCollection(Of FeatureItem))
                SetProperty(_features, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets the filtered view of features.
        ''' </summary>
        Public ReadOnly Property FeaturesView As ICollectionView
            Get
                Return _featuresView
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the currently selected feature.
        ''' </summary>
        Public Property SelectedFeature As FeatureItem
            Get
                Return _selectedFeature
            End Get
            Set(value As FeatureItem)
                If SetProperty(_selectedFeature, value) Then
                    ApplyFeatureCommand.NotifyCanExecuteChanged()
                    RevertFeatureCommand.NotifyCanExecuteChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the search text for filtering features.
        ''' </summary>
        Public Property SearchText As String
            Get
                Return _searchText
            End Get
            Set(value As String)
                If SetProperty(_searchText, value) Then
                    _featuresView?.Refresh()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the selected build number.
        ''' </summary>
        Public Property SelectedBuild As String
            Get
                Return _selectedBuild
            End Get
            Set(value As String)
                SetProperty(_selectedBuild, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the available build numbers.
        ''' </summary>
        Public Property AvailableBuilds As ObservableCollection(Of String)
            Get
                Return _availableBuilds
            End Get
            Set(value As ObservableCollection(Of String))
                SetProperty(_availableBuilds, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the status bar message.
        ''' </summary>
        Public Property StatusMessage As String
            Get
                Return _statusMessage
            End Get
            Set(value As String)
                SetProperty(_statusMessage, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether a loading operation is in progress.
        ''' </summary>
        Public Property IsLoading As Boolean
            Get
                Return _isLoading
            End Get
            Set(value As Boolean)
                If SetProperty(_isLoading, value) Then
                    ApplyFeatureCommand.NotifyCanExecuteChanged()
                    RevertFeatureCommand.NotifyCanExecuteChanged()
                    RefreshBuildsCommand.NotifyCanExecuteChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the current theme name for display.
        ''' </summary>
        Public Property CurrentThemeName As String
            Get
                Return _currentThemeName
            End Get
            Set(value As String)
                SetProperty(_currentThemeName, value)
            End Set
        End Property

#Region "Publish Properties"

        ''' <summary>
        ''' Gets or sets the build number for publishing.
        ''' </summary>
        Public Property PublishBuildNumber As String
            Get
                Return _publishBuildNumber
            End Get
            Set(value As String)
                If SetProperty(_publishBuildNumber, value) Then
                    PublishFeatureListCommand.NotifyCanExecuteChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the artifact path for publishing.
        ''' </summary>
        Public Property PublishArtifactPath As String
            Get
                Return _publishArtifactPath
            End Get
            Set(value As String)
                If SetProperty(_publishArtifactPath, value) Then
                    PublishFeatureListCommand.NotifyCanExecuteChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the artifact URL for publishing.
        ''' </summary>
        Public Property PublishArtifactUrl As String
            Get
                Return _publishArtifactUrl
            End Get
            Set(value As String)
                If SetProperty(_publishArtifactUrl, value) Then
                    PublishFeatureListCommand.NotifyCanExecuteChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the output format for publishing (csv or json).
        ''' </summary>
        Public Property PublishFormat As String
            Get
                Return _publishFormat
            End Get
            Set(value As String)
                SetProperty(_publishFormat, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the GitHub token for workflow dispatch.
        ''' </summary>
        Public Property PublishGitHubToken As String
            Get
                Return _publishGitHubToken
            End Get
            Set(value As String)
                If SetProperty(_publishGitHubToken, value) Then
                    PublishFeatureListCommand.NotifyCanExecuteChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the current publish status.
        ''' </summary>
        Public Property PublishStatus As PublishStatus
            Get
                Return _publishStatus
            End Get
            Set(value As PublishStatus)
                SetProperty(_publishStatus, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the publish error message.
        ''' </summary>
        Public Property PublishErrorMessage As String
            Get
                Return _publishErrorMessage
            End Get
            Set(value As String)
                SetProperty(_publishErrorMessage, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether the error is a maintainer-only access error.
        ''' </summary>
        Public Property IsMaintainerOnlyError As Boolean
            Get
                Return _isMaintainerOnlyError
            End Get
            Set(value As Boolean)
                SetProperty(_isMaintainerOnlyError, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether a publish operation is in progress.
        ''' </summary>
        Public Property IsPublishing As Boolean
            Get
                Return _isPublishing
            End Get
            Set(value As Boolean)
                If SetProperty(_isPublishing, value) Then
                    PublishFeatureListCommand.NotifyCanExecuteChanged()
                    LaunchFeatureScannerCommand.NotifyCanExecuteChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether the publish panel should be shown (after successful scan).
        ''' </summary>
        Public Property CanShowPublishPanel As Boolean
            Get
                Return _canShowPublishPanel
            End Get
            Set(value As Boolean)
                SetProperty(_canShowPublishPanel, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets the available output formats.
        ''' </summary>
        Public Property AvailableFormats As ObservableCollection(Of String)
            Get
                Return _availableFormats
            End Get
            Set(value As ObservableCollection(Of String))
                SetProperty(_availableFormats, value)
            End Set
        End Property

#End Region

        ''' <summary>
        ''' Command to apply/enable a feature.
        ''' </summary>
        Public ReadOnly Property ApplyFeatureCommand As IRelayCommand

        ''' <summary>
        ''' Command to revert a feature to default.
        ''' </summary>
        Public ReadOnly Property RevertFeatureCommand As IRelayCommand

        ''' <summary>
        ''' Command to refresh the list of available builds.
        ''' </summary>
        Public ReadOnly Property RefreshBuildsCommand As IAsyncRelayCommand

        ''' <summary>
        ''' Command to toggle the application theme.
        ''' </summary>
        Public ReadOnly Property ToggleThemeCommand As IRelayCommand

        ''' <summary>
        ''' Command to load features.
        ''' </summary>
        Public ReadOnly Property LoadFeaturesCommand As IAsyncRelayCommand

        ''' <summary>
        ''' Command to launch the Feature Scanner application.
        ''' </summary>
        Public ReadOnly Property LaunchFeatureScannerCommand As IRelayCommand

        ''' <summary>
        ''' Command to publish feature list via GitHub Actions.
        ''' </summary>
        Public ReadOnly Property PublishFeatureListCommand As IAsyncRelayCommand

        ''' <summary>
        ''' Creates a new instance of MainViewModel.
        ''' </summary>
        Public Sub New()
            _featureService = New FeatureService()
            _gitHubService = New GitHubService()
            _themeService = ThemeService.Instance
            _publishService = New PublishService()

            _features = New ObservableCollection(Of FeatureItem)()
            _availableBuilds = New ObservableCollection(Of String)()
            _availableFormats = New ObservableCollection(Of String) From {"csv", "json"}
            _publishFormat = "csv"

            ' Initialize the collection view for filtering
            _featuresView = CollectionViewSource.GetDefaultView(_features)
            _featuresView.Filter = AddressOf FilterFeatures

            ' Initialize commands
            ApplyFeatureCommand = New RelayCommand(AddressOf ExecuteApplyFeature, AddressOf CanExecuteFeatureCommand)
            RevertFeatureCommand = New RelayCommand(AddressOf ExecuteRevertFeature, AddressOf CanExecuteFeatureCommand)
            RefreshBuildsCommand = New AsyncRelayCommand(AddressOf ExecuteRefreshBuildsAsync, AddressOf CanExecuteRefreshBuilds)
            ToggleThemeCommand = New RelayCommand(AddressOf ExecuteToggleTheme)
            LoadFeaturesCommand = New AsyncRelayCommand(AddressOf ExecuteLoadFeaturesAsync)
            LaunchFeatureScannerCommand = New RelayCommand(AddressOf ExecuteLaunchFeatureScanner, AddressOf CanExecuteLaunchFeatureScanner)
            PublishFeatureListCommand = New AsyncRelayCommand(AddressOf ExecutePublishFeatureListAsync, AddressOf CanExecutePublishFeatureList)

            ' Set initial theme name
            _currentThemeName = _themeService.CurrentTheme

            ' Check for administrator privileges
            CheckAdminPrivileges()
        End Sub

        ''' <summary>
        ''' Checks if the application is running with administrator privileges.
        ''' </summary>
        Private Sub CheckAdminPrivileges()
            Try
                Dim identity = WindowsIdentity.GetCurrent()
                Dim principal = New WindowsPrincipal(identity)
                IsRunningAsAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator)

                If Not IsRunningAsAdmin Then
                    AdminWarningMessage = "Running without administrator privileges. Feature queries and changes may be limited."
                    System.Diagnostics.Debug.WriteLine("ViVeTool-GUI: Not running as administrator")
                Else
                    AdminWarningMessage = String.Empty
                    System.Diagnostics.Debug.WriteLine("ViVeTool-GUI: Running as administrator")
                End If
            Catch ex As Exception
                ' If we can't determine admin status, assume not admin and continue
                IsRunningAsAdmin = False
                AdminWarningMessage = "Could not determine administrator status. Some features may be limited."
                System.Diagnostics.Debug.WriteLine($"Admin check failed: {ex.Message}")
            End Try
        End Sub

        ''' <summary>
        ''' Filter predicate for the features collection.
        ''' </summary>
        Private Function FilterFeatures(item As Object) As Boolean
            If item Is Nothing Then
                Return False
            End If

            Dim feature = TryCast(item, FeatureItem)
            If feature Is Nothing Then
                Return False
            End If

            If String.IsNullOrWhiteSpace(SearchText) Then
                Return True
            End If

            ' Filter by name, ID, or notes
            Dim searchLower = SearchText.ToLowerInvariant()
            Return feature.Name.ToLowerInvariant().Contains(searchLower) OrElse
                   feature.Id.ToString().Contains(searchLower) OrElse
                   (Not String.IsNullOrEmpty(feature.Notes) AndAlso feature.Notes.ToLowerInvariant().Contains(searchLower))
        End Function

        ''' <summary>
        ''' Determines if feature commands can execute.
        ''' </summary>
        Private Function CanExecuteFeatureCommand() As Boolean
            Return SelectedFeature IsNot Nothing AndAlso Not IsLoading
        End Function

        ''' <summary>
        ''' Determines if refresh builds command can execute.
        ''' </summary>
        Private Function CanExecuteRefreshBuilds() As Boolean
            Return Not IsLoading
        End Function

        ''' <summary>
        ''' Executes the apply feature command.
        ''' </summary>
        Private Async Sub ExecuteApplyFeature()
            If SelectedFeature Is Nothing Then Return

            Try
                IsLoading = True
                StatusMessage = $"Applying feature {SelectedFeature.Id}..."

                ' TODO: Determine enable/disable based on UI state or show dialog
                Dim result = Await _featureService.ApplyFeatureAsync(SelectedFeature.Id, True)

                If result Then
                    SelectedFeature.IsEnabled = True
                    SelectedFeature.State = "Enabled"
                    StatusMessage = $"Feature {SelectedFeature.Id} applied successfully."
                Else
                    StatusMessage = $"Failed to apply feature {SelectedFeature.Id}."
                End If
            Catch ex As Exception
                StatusMessage = $"Error: {ex.Message}"
            Finally
                IsLoading = False
            End Try
        End Sub

        ''' <summary>
        ''' Executes the revert feature command.
        ''' </summary>
        Private Async Sub ExecuteRevertFeature()
            If SelectedFeature Is Nothing Then Return

            Try
                IsLoading = True
                StatusMessage = $"Reverting feature {SelectedFeature.Id}..."

                Dim result = Await _featureService.RevertFeatureAsync(SelectedFeature.Id)

                If result Then
                    SelectedFeature.IsEnabled = False
                    SelectedFeature.State = "Default"
                    StatusMessage = $"Feature {SelectedFeature.Id} reverted successfully."
                Else
                    StatusMessage = $"Failed to revert feature {SelectedFeature.Id}."
                End If
            Catch ex As Exception
                StatusMessage = $"Error: {ex.Message}"
            Finally
                IsLoading = False
            End Try
        End Sub

        ''' <summary>
        ''' Executes the refresh builds command asynchronously.
        ''' Uses cached/offline feed as fallback if online fetch fails.
        ''' </summary>
        Private Async Function ExecuteRefreshBuildsAsync() As Task
            Try
                IsLoading = True
                StatusMessage = "Fetching available builds..."

                Dim builds = Await _gitHubService.GetAvailableBuildsAsync()

                AvailableBuilds.Clear()
                For Each build In builds
                    AvailableBuilds.Add(build)
                Next

                If AvailableBuilds.Count > 0 Then
                    SelectedBuild = AvailableBuilds(0)
                    StatusMessage = $"Found {AvailableBuilds.Count} builds."
                Else
                    ' No builds available from either online or cache - show non-blocking warning
                    StatusMessage = "No builds available. Check your network connection or try again later."
                    System.Diagnostics.Debug.WriteLine("Warning: No builds available from feed or cache")
                End If
            Catch ex As Exception
                ' Log the error but don't block the app
                StatusMessage = $"Could not fetch builds: {ex.Message}. Local feature browsing still available."
                System.Diagnostics.Debug.WriteLine($"Error fetching builds (non-fatal): {ex}")
            Finally
                IsLoading = False
            End Try
        End Function

        ''' <summary>
        ''' Executes the toggle theme command.
        ''' </summary>
        Private Sub ExecuteToggleTheme()
            _themeService.ToggleTheme()
            CurrentThemeName = _themeService.CurrentTheme
            StatusMessage = $"Theme changed to {CurrentThemeName}."
        End Sub

        ''' <summary>
        ''' Loads features asynchronously.
        ''' Handles group-related errors gracefully without blocking the app.
        ''' </summary>
        Private Async Function ExecuteLoadFeaturesAsync() As Task
            Try
                IsLoading = True
                LocalFeatureWarning = String.Empty
                StatusMessage = "Loading features..."

                Dim loadedFeatures = Await _featureService.GetFeaturesAsync()

                Features.Clear()
                For Each feature In loadedFeatures
                    Features.Add(feature)
                Next

                ' Check if the service returned a warning row (feature ID 0 with placeholder name)
                Dim hasNoFeatures = loadedFeatures.Any(Function(f) f.Id = 0 AndAlso f.Name = NoFeaturesLoadedText)
                
                ' Check for warnings from the service (non-blocking issues)
                Dim warnings = _featureService.Warnings
                Dim hasWarnings = warnings IsNot Nothing AndAlso warnings.Count > 0

                ' Consolidate warnings for display (avoid duplicate messages)
                If hasWarnings Then
                    ' Check for group-related errors using case-insensitive matching
                    Dim groupWarnings = warnings.Where(Function(w) w.IndexOf("Group", StringComparison.OrdinalIgnoreCase) >= 0 OrElse w.Contains(MaxGroupValueText)).ToList()
                    Dim otherWarnings = warnings.Where(Function(w) w.IndexOf("Group", StringComparison.OrdinalIgnoreCase) < 0 AndAlso Not w.Contains(MaxGroupValueText)).ToList()

                    If groupWarnings.Count > 0 Then
                        ' Consolidate group warnings into a single message
                        LocalFeatureWarning = $"Local feature query unavailable: Group value constraints exceeded (max {MaxGroupValueText}). You can still view feature lists from the feed."
                    ElseIf otherWarnings.Count > 0 Then
                        LocalFeatureWarning = $"Local feature query issues: {String.Join("; ", otherWarnings.Take(2))}"
                    End If
                End If
                
                If hasNoFeatures Then
                    ' Set warning and continue - do not block
                    If String.IsNullOrEmpty(LocalFeatureWarning) Then
                        LocalFeatureWarning = _featureService.LastErrorMessage
                        If String.IsNullOrEmpty(LocalFeatureWarning) Then
                            LocalFeatureWarning = "Could not load local system features."
                        End If
                    End If
                    StatusMessage = "Local features unavailable. You can still select a build and view feature lists."
                ElseIf hasWarnings Then
                    ' Features loaded but with some warnings
                    StatusMessage = $"Loaded {Features.Count} features with {warnings.Count} warning(s)."
                Else
                    StatusMessage = $"Loaded {Features.Count} features."
                End If
            Catch ex As Exception
                ' Even on exception, don't block the app - log and continue
                ' Check for group-related exceptions using case-insensitive matching
                If ex.Message.IndexOf("Group", StringComparison.OrdinalIgnoreCase) >= 0 OrElse 
                   ex.Message.Contains(MaxGroupValueText) Then
                    LocalFeatureWarning = $"Local feature query unavailable: {ex.Message}"
                Else
                    LocalFeatureWarning = $"Local feature query failed: {ex.Message}"
                End If
                StatusMessage = "Local features unavailable. Build selection is still available."
                System.Diagnostics.Debug.WriteLine($"Feature loading exception (non-fatal): {ex}")
            Finally
                IsLoading = False
            End Try
        End Function

        ''' <summary>
        ''' Initializes the view model and loads initial data.
        ''' Decouples feed loading from local feature queries - both run independently.
        ''' </summary>
        Public Async Function InitializeAsync() As Task
            ' Run both operations concurrently - feed loading should not be blocked by feature query failures
            Dim feedTask = ExecuteRefreshBuildsAsync()
            Dim featuresTask = ExecuteLoadFeaturesAsync()

            ' Wait for both to complete independently
            Try
                Await Task.WhenAll(feedTask, featuresTask)
            Catch ex As Exception
                ' Individual tasks handle their own exceptions, but WhenAll may aggregate them
                System.Diagnostics.Debug.WriteLine($"InitializeAsync error (non-fatal): {ex.Message}")
            End Try

            ' Update status to reflect overall state
            UpdateOverallStatus()
        End Function

        ''' <summary>
        ''' Updates the status message to reflect the overall application state.
        ''' </summary>
        Private Sub UpdateOverallStatus()
            Dim messages As New List(Of String)()

            ' Check admin status
            If Not IsRunningAsAdmin Then
                messages.Add("Not running as admin")
            End If

            ' Check if builds were loaded
            If AvailableBuilds.Count = 0 Then
                messages.Add("No builds available")
            Else
                messages.Add($"Found {AvailableBuilds.Count} builds")
            End If

            ' Check if features were loaded (ignoring the warning placeholder)
            Dim realFeatureCount = Features.Where(Function(f) f.Id <> 0 OrElse f.Name <> NoFeaturesLoadedText).Count()
            If realFeatureCount > 0 Then
                messages.Add($"{realFeatureCount} features")
            ElseIf LocalFeatureWarning.Length > 0 Then
                messages.Add("Local features unavailable")
            End If

            StatusMessage = String.Join(" | ", messages)
        End Sub

#Region "Publish Methods"

        ''' <summary>
        ''' Determines if the launch feature scanner command can execute.
        ''' </summary>
        Private Function CanExecuteLaunchFeatureScanner() As Boolean
            Return Not IsPublishing AndAlso Not IsLoading
        End Function

        ''' <summary>
        ''' Executes the launch feature scanner command.
        ''' Opens the integrated Feature Scanner dialog instead of launching an external EXE.
        ''' </summary>
        Private Sub ExecuteLaunchFeatureScanner()
            Try
                ' Get the current window as owner for the dialog
                Dim mainWindow = System.Windows.Application.Current?.MainWindow

                ' Create and show the Feature Scanner dialog
                Dim dialog As Views.FeatureScannerDialog
                If mainWindow IsNot Nothing Then
                    dialog = New Views.FeatureScannerDialog(mainWindow)
                Else
                    dialog = New Views.FeatureScannerDialog()
                End If

                StatusMessage = "Feature Scanner opened."

                ' Show the dialog
                Dim result = dialog.ShowDialog()

                ' Check if we have a successful scan result
                If result = True AndAlso dialog.ScanResult IsNot Nothing AndAlso dialog.ScanResult.Success Then
                    ' Populate the publish panel with the scan result
                    SetScannerResult(dialog.ScanResult.OutputFilePath, Services.FeatureScannerService.GetCurrentBuildNumber())
                    StatusMessage = "Feature Scanner completed. Publish panel populated with scan result."
                ElseIf dialog.ScanResult IsNot Nothing AndAlso dialog.ScanResult.IsCancelled Then
                    StatusMessage = "Feature Scanner was cancelled."
                Else
                    ' Still show publish panel for manual entry
                    CanShowPublishPanel = True
                    StatusMessage = "Feature Scanner closed."
                End If

            Catch ex As Exception
                StatusMessage = $"Error opening Feature Scanner: {ex.Message}"
            End Try
        End Sub

        ''' <summary>
        ''' Determines if the publish feature list command can execute.
        ''' </summary>
        Private Function CanExecutePublishFeatureList() As Boolean
            Return Not IsPublishing AndAlso
                   Not String.IsNullOrWhiteSpace(PublishBuildNumber) AndAlso
                   (Not String.IsNullOrWhiteSpace(PublishArtifactPath) OrElse Not String.IsNullOrWhiteSpace(PublishArtifactUrl)) AndAlso
                   Not String.IsNullOrWhiteSpace(PublishGitHubToken)
        End Function

        ''' <summary>
        ''' Executes the publish feature list command asynchronously.
        ''' </summary>
        Private Async Function ExecutePublishFeatureListAsync() As Task
            Try
                IsPublishing = True
                PublishStatus = Models.PublishStatus.Publishing
                PublishErrorMessage = String.Empty
                IsMaintainerOnlyError = False
                StatusMessage = "Publishing feature list via GitHub Actions..."

                Dim result = Await _publishService.DispatchPublishWorkflowAsync(
                    PublishBuildNumber,
                    PublishArtifactPath,
                    PublishArtifactUrl,
                    PublishFormat,
                    PublishGitHubToken)

                If result.Status = Models.PublishStatus.Success Then
                    PublishStatus = Models.PublishStatus.Success
                    StatusMessage = "Feature list publish workflow dispatched successfully. Check GitHub Actions for progress."
                Else
                    PublishStatus = Models.PublishStatus.Failure
                    PublishErrorMessage = result.ErrorMessage
                    IsMaintainerOnlyError = result.IsMaintainerOnly
                    If result.IsMaintainerOnly Then
                        StatusMessage = "Access denied: Publishing feature lists is restricted to repository maintainers only."
                    Else
                        StatusMessage = $"Publish failed: {result.ErrorMessage}"
                    End If
                End If
            Catch ex As Exception
                PublishStatus = Models.PublishStatus.Failure
                PublishErrorMessage = ex.Message
                StatusMessage = $"Error: {ex.Message}"
            Finally
                IsPublishing = False
            End Try
        End Function

        ''' <summary>
        ''' Sets the artifact path from a successful Feature Scanner run.
        ''' </summary>
        ''' <param name="artifactPath">The path to the generated feature list file.</param>
        ''' <param name="buildNumber">The Windows build number.</param>
        Public Sub SetScannerResult(artifactPath As String, buildNumber As String)
            PublishArtifactPath = artifactPath
            PublishBuildNumber = buildNumber
            CanShowPublishPanel = True
            PublishStatus = Models.PublishStatus.Idle
            PublishErrorMessage = String.Empty
        End Sub

        ''' <summary>
        ''' Resets the publish status to idle.
        ''' </summary>
        Public Sub ResetPublishStatus()
            PublishStatus = Models.PublishStatus.Idle
            PublishErrorMessage = String.Empty
            IsMaintainerOnlyError = False
        End Sub

#End Region

    End Class
End Namespace

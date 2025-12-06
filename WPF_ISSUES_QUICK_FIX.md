# WPF Version - Quick Fix Guide

**Status:** WPF is 55% feature complete and mostly broken  
**Estimated Time to Fix Critical Issues:** 5 hours  
**Current Blocker:** Missing store repair, export, manual entry

---

## CRITICAL ISSUES - START HERE

### Issue #1: No Store Repair (30 mins)

**File to Create:** `ViVeTool-GUI.Wpf/Services/StoreRepairService.vb`

```vb
Imports Albacore.ViVe

Namespace Services
    Public Class StoreRepairService
        ''' <summary>Repairs the Windows feature store</summary>
        Public Async Function RepairStoreAsync() As Task(Of Boolean)
            Return Await Task.Run(Function() RepairStoreCore())
        End Function

        Private Function RepairStoreCore() As Boolean
            Try
                ' LastKnownGood Store repair
                Dim config = New FeatureConfiguration With {
                    .FeatureId = &H8CFFFFF4,
                    .EnabledState = FeatureEnabledState.Disabled,
                    .Action = FeatureConfigurationAction.UpdateEnabledState
                }
                
                RtlFeatureManager.SetBootFeatureConfigurations(
                    New List(Of FeatureConfiguration) From {config}
                )
                Return True
            Catch ex As Exception
                Debug.WriteLine($"Store repair failed: {ex.Message}")
                Return False
            End Try
        End Function

        ''' <summary>Fixes A/B Testing Priorities</summary>
        Public Async Function FixABTestingAsync() As Task(Of Boolean)
            Return Await Task.Run(Function() FixABTestingCore())
        End Function

        Private Function FixABTestingCore() As Boolean
            Try
                ' Your A/B testing fix logic here
                Return True
            Catch ex As Exception
                Debug.WriteLine($"A/B fix failed: {ex.Message}")
                Return False
            End Try
        End Function
    End Class
End Namespace
```

**Then Add to MainViewModel:**
```vb
Private ReadOnly _storeRepairService As New StoreRepairService()

Public ReadOnly Property RepairStoreCommand As ICommand
Public ReadOnly Property FixABTestingCommand As ICommand

Private Async Sub RepairStore()
    Dim result = Await _storeRepairService.RepairStoreAsync()
    If result Then
        MessageBox.Show("Store repaired successfully!")
    Else
        MessageBox.Show("Store repair failed. See logs for details.")
    End If
End Sub
```

---

### Issue #2: No Export (1 hour)

**File to Create:** `ViVeTool-GUI.Wpf/Services/ExportService.vb`

```vb
Imports System.IO
Imports System.Text.Json
Imports System.Text.Json.Serialization

Namespace Services
    Public Class ExportService
        ''' <summary>Exports features to CSV format</summary>
        Public Async Function ExportToCSVAsync(
            features As List(Of FeatureItem), 
            filePath As String) As Task(Of Boolean)
            
            Return Await Task.Run(Function() ExportToCsvCore(features, filePath))
        End Function

        Private Function ExportToCsvCore(
            features As List(Of FeatureItem), 
            filePath As String) As Boolean
            
            Try
                Using writer As New StreamWriter(filePath)
                    ' Write header
                    writer.WriteLine("Name,ID,State,Group,Notes")
                    
                    ' Write features
                    For Each feature In features
                        writer.WriteLine(
                            $"\"{feature.Name}\",{feature.Id}," & 
                            $"{feature.State},\"{feature.Group}\"," &
                            $"\"{feature.Notes}\""
                        )
                    Next
                End Using
                Return True
            Catch ex As Exception
                Debug.WriteLine($"CSV export failed: {ex.Message}")
                Return False
            End Try
        End Function

        ''' <summary>Exports features to JSON format</summary>
        Public Async Function ExportToJSONAsync(
            features As List(Of FeatureItem), 
            filePath As String) As Task(Of Boolean)
            
            Return Await Task.Run(Function() ExportToJsonCore(features, filePath))
        End Function

        Private Function ExportToJsonCore(
            features As List(Of FeatureItem), 
            filePath As String) As Boolean
            
            Try
                Dim options = New JsonSerializerOptions With {
                    .WriteIndented = True
                }
                Dim json = JsonSerializer.Serialize(features, options)
                File.WriteAllText(filePath, json)
                Return True
            Catch ex As Exception
                Debug.WriteLine($"JSON export failed: {ex.Message}")
                Return False
            End Try
        End Function
    End Class
End Namespace
```

**Add to MainWindow.xaml:**
```xaml
<Button Content="Export Features" Command="{Binding ExportCommand}" />
```

---

### Issue #3: No Manual Entry (1.5 hours)

**File to Create:** `ViVeTool-GUI.Wpf/Views/ManualFeatureWindow.xaml`

```xaml
<Window x:Class="ViVeTool_GUI.Wpf.ManualFeatureWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Manual Feature Entry" Height="200" Width="400"
        WindowStartupLocation="CenterOwner" ResizeMode="NoResize">
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="120" />
            <ColumnDefinition Width="*" />
        </Grid.ColumnDefinitions>

        <Label Grid.Row="0" Grid.Column="0" Content="Feature ID:" />
        <TextBox x:Name="FeatureIdTextBox" Grid.Row="0" Grid.Column="1" 
                 PreviewTextInput="NumericOnly_PreviewTextInput" />

        <Label Grid.Row="1" Grid.Column="0" Content="State:" Margin="0,10,0,0" />
        <ComboBox Grid.Row="1" Grid.Column="1" Margin="0,10,0,0"
                  x:Name="StateComboBox"
                  ItemsSource="{Binding StateOptions}" 
                  SelectedItem="{Binding SelectedState}" />

        <StackPanel Grid.Row="4" Grid.ColumnSpan="2" Orientation="Horizontal" 
                    HorizontalAlignment="Right" Margin="0,20,0,0">
            <Button Content="OK" Width="80" Margin="0,0,10,0" Click="OK_Click" />
            <Button Content="Cancel" Width="80" Click="Cancel_Click" />
        </StackPanel>
    </Grid>
</Window>
```

**File to Create:** `ViVeTool-GUI.Wpf/Views/ManualFeatureWindow.xaml.vb`

```vb
Public Class ManualFeatureWindow
    Public Property SelectedFeatureId As Integer
    Public Property SelectedState As String
    
    Public Sub New()
        InitializeComponent()
        StateComboBox.Items.Add("Enabled")
        StateComboBox.Items.Add("Disabled")
        StateComboBox.Items.Add("Default")
        StateComboBox.SelectedIndex = 0
    End Sub
    
    Private Sub OK_Click(sender As Object, e As RoutedEventArgs)
        If Not Integer.TryParse(FeatureIdTextBox.Text, SelectedFeatureId) Then
            MessageBox.Show("Invalid Feature ID")
            Return
        End If
        SelectedState = CStr(StateComboBox.SelectedItem)
        Me.DialogResult = True
    End Sub
    
    Private Sub Cancel_Click(sender As Object, e As RoutedEventArgs)
        Me.DialogResult = False
    End Sub
    
    Private Sub NumericOnly_PreviewTextInput(
        sender As Object, e As System.Windows.Input.TextCompositionEventArgs)
        e.Handled = Not Char.IsDigit(e.Text(0))
    End Sub
End Class
```

---

## NEXT PRIORITIES (After Critical)

### High Priority #1: Sorting/Filtering (2.5 hours)

**In MainViewModel.vb:**
```vb
Public ReadOnly Property SortByIDCommand As New RelayCommand(Sub() SortByID())
Public ReadOnly Property SortByNameCommand As New RelayCommand(Sub() SortByName())
Public ReadOnly Property FilterStateProperty As String = "All"

Private Sub SortByID()
    Dim view = CollectionViewSource.GetDefaultView(_features)
    view.SortDescriptions.Clear()
    view.SortDescriptions.Add(New SortDescription("Id", ListSortDirection.Ascending))
End Sub
```

**In MainWindow.xaml:**
```xaml
<StackPanel Orientation="Horizontal" Margin="10">
    <Button Content="Sort by ID" Command="{Binding SortByIDCommand}" />
    <Button Content="Sort by Name" Command="{Binding SortByNameCommand}" />
    <ComboBox ItemsSource="All,Enabled,Disabled" SelectedValuePath="Content"
              SelectedValue="{Binding FilterStateProperty}" />
</StackPanel>
```

### High Priority #2: Feature Grouping (2.5 hours)

**In FeatureService.vb:**
```vb
' Add grouping logic
Public Property GroupedFeatures As ObservableCollection(Of FeatureGroup)

Public Class FeatureGroup
    Public Property GroupName As String
    Public Property Items As List(Of FeatureItem)
    Public Property Count As Integer
End Class
```

---

## QUICK CHECKLIST

### Week 1
- [ ] Add StoreRepairService (30 mins)
- [ ] Add ExportService (1 hr)
- [ ] Add ManualFeatureWindow (1.5 hrs)
- [ ] Test all three together (30 mins)
- [ ] Update README with fixes

**Progress: 60% → 70%**

### Week 2  
- [ ] Add sorting commands (1.5 hrs)
- [ ] Add filtering (1 hr)
- [ ] Add grouping display (2.5 hrs)
- [ ] Add context menus (1 hr)
- [ ] Test integration

**Progress: 70% → 85%**

### Week 3
- [ ] Settings window (3.5 hrs)
- [ ] Error logging (2 hrs)
- [ ] Feature scanner improvements (2.5 hrs)
- [ ] Testing + bug fixes

**Progress: 85% → 95%**

---

## TESTING CHECKLIST

For each feature you fix:
- [ ] Test on Windows 10
- [ ] Test on Windows 11
- [ ] Test happy path (success)
- [ ] Test error path (failures)
- [ ] Test UI responsiveness
- [ ] Check for memory leaks
- [ ] Verify logging works
- [ ] Check against Legacy version

---

## COMMON PATTERNS

### Pattern 1: Async Method
```vb
Public Async Function MethodAsync() As Task(Of Boolean)
    Return Await Task.Run(Function() MethodCore())
End Function

Private Function MethodCore() As Boolean
    Try
        ' Your logic
        Return True
    Catch ex As Exception
        Debug.WriteLine(ex.Message)
        Return False
    End Try
End Function
```

### Pattern 2: Command with Async
```vb
Public ReadOnly Property MyCommand As ICommand

Private Async Sub ExecuteMyCommand()
    Dim result = Await _service.DoSomethingAsync()
    If result Then
        ' Handle success
    Else
        ' Handle failure  
    End If
End Sub
```

### Pattern 3: UI Update from Service
```vb
RaisePropertyChanged(NameOf(MyProperty))
' Or if using ObservableCollection:
_myCollection.Clear()
For Each item In newItems
    _myCollection.Add(item)
Next
```

---

## RESOURCES

- See [CODE_ANALYSIS.md](CODE_ANALYSIS.md) for detailed analysis
- See [DEVELOPMENT.md](DEVELOPMENT.md) for dev setup
- Check legacy `GUI.vb` for reference implementations
- Review [KNOWN_ISSUES.md](KNOWN_ISSUES.md) for user-facing issues

---

**Start with Store Repair. Don't move to next issue until tested.** 🙋

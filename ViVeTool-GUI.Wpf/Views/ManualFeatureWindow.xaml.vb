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

Imports System.Text.RegularExpressions
Imports System.Windows.Input

Namespace Views
    ''' <summary>
    ''' Interaction logic for ManualFeatureWindow.xaml
    ''' Allows users to manually enter a feature ID and desired state.
    ''' </summary>
    Public Partial Class ManualFeatureWindow
        Inherits Window

        ''' <summary>
        ''' Gets or sets the selected feature ID.
        ''' </summary>
        Public Property SelectedFeatureId As Integer = 0

        ''' <summary>
        ''' Gets or sets the selected state.
        ''' </summary>
        Public Property SelectedState As String = "Enabled"

        ''' <summary>
        ''' Creates a new instance of ManualFeatureWindow.
        ''' </summary>
        Public Sub New()
            InitializeComponent()
            
            ' Initialize state combo box
            StateComboBox.Items.Add("Enabled")
            StateComboBox.Items.Add("Disabled")
            StateComboBox.Items.Add("Default")
            StateComboBox.SelectedIndex = 0
            
            ' Set focus to feature ID input
            FeatureIdTextBox.Focus()
        End Sub

        ''' <summary>
        ''' Handles the OK button click event.
        ''' Validates input and closes the dialog with DialogResult = True.
        ''' </summary>
        Private Sub OK_Click(sender As Object, e As RoutedEventArgs)
            ' Clear previous error message
            ErrorMessageBlock.Text = String.Empty
            
            ' Validate feature ID
            If String.IsNullOrWhiteSpace(FeatureIdTextBox.Text) Then
                ErrorMessageBlock.Text = "Please enter a Feature ID"
                FeatureIdTextBox.Focus()
                Return
            End If
            
            ' Try to parse feature ID
            If Not Integer.TryParse(FeatureIdTextBox.Text, SelectedFeatureId) Then
                ErrorMessageBlock.Text = "Invalid Feature ID. Must be a positive whole number."
                FeatureIdTextBox.Focus()
                FeatureIdTextBox.SelectAll()
                Return
            End If
            
            ' Validate feature ID is positive
            If SelectedFeatureId <= 0 Then
                ErrorMessageBlock.Text = "Feature ID must be greater than 0"
                FeatureIdTextBox.Focus()
                FeatureIdTextBox.SelectAll()
                Return
            End If
            
            ' Validate state selection
            If StateComboBox.SelectedIndex < 0 Then
                ErrorMessageBlock.Text = "Please select a Feature State"
                StateComboBox.Focus()
                Return
            End If
            
            ' Get selected state
            SelectedState = CStr(StateComboBox.SelectedItem)
            
            ' Close dialog successfully
            Me.DialogResult = True
            Me.Close()
        End Sub

        ''' <summary>
        ''' Handles the Cancel button click event.
        ''' Closes the dialog with DialogResult = False.
        ''' </summary>
        Private Sub Cancel_Click(sender As Object, e As RoutedEventArgs)
            Me.DialogResult = False
            Me.Close()
        End Sub

        ''' <summary>
        ''' Handles PreviewTextInput event to allow only numeric characters.
        ''' </summary>
        Private Sub NumericOnly_PreviewTextInput(sender As Object, e As TextCompositionEventArgs)
            ' Check if the text is numeric
            If Not Regex.IsMatch(e.Text, "[0-9]") Then
                e.Handled = True
            End If
        End Sub

        ''' <summary>
        ''' Handles window loaded event to set focus and initial state.
        ''' </summary>
        Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            FeatureIdTextBox.Focus()
            FeatureIdTextBox.Clear()
        End Sub
    End Class
End Namespace

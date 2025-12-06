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
Imports System.Diagnostics
Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports ViVeTool_GUI.Wpf.Models

Namespace Services
    ''' <summary>
    ''' Service for exporting features to various file formats.
    ''' Supports CSV, JSON, and legacy TXT formats.
    ''' </summary>
    Public Class ExportService
        ''' <summary>
        ''' Gets the last error message from a failed operation.
        ''' </summary>
        Private _lastErrorMessage As String = String.Empty

        ''' <summary>
        ''' Gets the last error message from a failed operation.
        ''' </summary>
        Public ReadOnly Property LastErrorMessage As String
            Get
                Return _lastErrorMessage
            End Get
        End Property

        ''' <summary>
        ''' Exports features to CSV format.
        ''' </summary>
        ''' <param name="features">Collection of features to export.</param>
        ''' <param name="filePath">Path where the CSV file will be saved.</param>
        ''' <returns>True if successful, false otherwise.</returns>
        Public Async Function ExportToCSVAsync(
            features As ObservableCollection(Of FeatureItem),
            filePath As String) As Task(Of Boolean)
            
            Return Await Task.Run(Function() ExportToCSVCore(features, filePath))
        End Function

        ''' <summary>
        ''' Core method to export to CSV.
        ''' </summary>
        Private Function ExportToCSVCore(
            features As ObservableCollection(Of FeatureItem),
            filePath As String) As Boolean
            
            Try
                ' Validate inputs
                If features Is Nothing OrElse features.Count = 0 Then
                    _lastErrorMessage = "No features to export"
                    Return False
                End If

                ' Create directory if it doesn't exist
                Dim directory = Path.GetDirectoryName(filePath)
                If Not String.IsNullOrEmpty(directory) AndAlso Not Directory.Exists(directory) Then
                    Directory.CreateDirectory(directory)
                End If

                ' Write CSV file
                Using writer As New StreamWriter(filePath, False, Encoding.UTF8)
                    ' Write header
                    writer.WriteLine("Name,ID,State,Enabled,Group,Notes")
                    
                    ' Write feature data
                    For Each feature In features
                        Try
                            ' Escape values for CSV (handle commas and quotes)
                            Dim name = EscapeCSVValue(feature.Name)
                            Dim notes = EscapeCSVValue(feature.Notes)
                            Dim group = EscapeCSVValue(feature.Group)
                            
                            writer.WriteLine($"{name},{feature.Id},{feature.State}," & 
                                $"{If(feature.IsEnabled, "Yes", "No")},{group},{notes}")
                        Catch ex As Exception
                            Debug.WriteLine($"Warning: Failed to export feature {feature.Id}: {ex.Message}")
                        End Try
                    Next
                End Using

                _lastErrorMessage = String.Empty
                Debug.WriteLine($"CSV export completed successfully: {filePath}")
                Return True

            Catch ex As Exception
                _lastErrorMessage = $"CSV export error: {ex.Message}"
                Debug.WriteLine(_lastErrorMessage)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Exports features to JSON format.
        ''' </summary>
        ''' <param name="features">Collection of features to export.</param>
        ''' <param name="filePath">Path where the JSON file will be saved.</param>
        ''' <returns>True if successful, false otherwise.</returns>
        Public Async Function ExportToJSONAsync(
            features As ObservableCollection(Of FeatureItem),
            filePath As String) As Task(Of Boolean)
            
            Return Await Task.Run(Function() ExportToJSONCore(features, filePath))
        End Function

        ''' <summary>
        ''' Core method to export to JSON.
        ''' </summary>
        Private Function ExportToJSONCore(
            features As ObservableCollection(Of FeatureItem),
            filePath As String) As Boolean
            
            Try
                ' Validate inputs
                If features Is Nothing OrElse features.Count = 0 Then
                    _lastErrorMessage = "No features to export"
                    Return False
                End If

                ' Create directory if it doesn't exist
                Dim directory = Path.GetDirectoryName(filePath)
                If Not String.IsNullOrEmpty(directory) AndAlso Not Directory.Exists(directory) Then
                    Directory.CreateDirectory(directory)
                End If

                ' Prepare export data
                Dim exportData As New List(Of Dictionary(Of String, Object))()
                For Each feature In features
                    Try
                        Dim featureData As New Dictionary(Of String, Object) From {
                            {"name", feature.Name},
                            {"id", feature.Id},
                            {"state", feature.State},
                            {"isEnabled", feature.IsEnabled},
                            {"group", feature.Group},
                            {"notes", feature.Notes}
                        }
                        exportData.Add(featureData)
                    Catch ex As Exception
                        Debug.WriteLine($"Warning: Failed to prepare feature {feature.Id}: {ex.Message}")
                    End Try
                Next

                ' Configure JSON serializer options
                Dim options = New JsonSerializerOptions With {
                    .WriteIndented = True,
                    .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }

                ' Serialize to JSON
                Dim json = JsonSerializer.Serialize(exportData, options)
                File.WriteAllText(filePath, json, Encoding.UTF8)

                _lastErrorMessage = String.Empty
                Debug.WriteLine($"JSON export completed successfully: {filePath}")
                Return True

            Catch ex As Exception
                _lastErrorMessage = $"JSON export error: {ex.Message}"
                Debug.WriteLine(_lastErrorMessage)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Exports features to legacy TXT format (mach2 style).
        ''' </summary>
        ''' <param name="features">Collection of features to export.</param>
        ''' <param name="filePath">Path where the TXT file will be saved.</param>
        ''' <returns>True if successful, false otherwise.</returns>
        Public Async Function ExportToTXTAsync(
            features As ObservableCollection(Of FeatureItem),
            filePath As String) As Task(Of Boolean)
            
            Return Await Task.Run(Function() ExportToTXTCore(features, filePath))
        End Function

        ''' <summary>
        ''' Core method to export to legacy TXT format.
        ''' </summary>
        Private Function ExportToTXTCore(
            features As ObservableCollection(Of FeatureItem),
            filePath As String) As Boolean
            
            Try
                ' Validate inputs
                If features Is Nothing OrElse features.Count = 0 Then
                    _lastErrorMessage = "No features to export"
                    Return False
                End If

                ' Create directory if it doesn't exist
                Dim directory = Path.GetDirectoryName(filePath)
                If Not String.IsNullOrEmpty(directory) AndAlso Not Directory.Exists(directory) Then
                    Directory.CreateDirectory(directory)
                End If

                ' Group features by their group
                Dim groupedFeatures = features.GroupBy(Function(f) f.Group).OrderBy(Function(g) g.Key)

                ' Write TXT file in mach2 format
                Using writer As New StreamWriter(filePath, False, Encoding.UTF8)
                    writer.WriteLine("# ViVeTool-GUI Feature Export")
                    writer.WriteLine("# Exported: " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    writer.WriteLine()

                    ' Write features grouped by category
                    For Each group In groupedFeatures
                        Try
                            ' Write group header
                            Select Case group.Key
                                Case "Always Enabled"
                                    writer.WriteLine("## Always Enabled:")
                                Case "Always Disabled"
                                    writer.WriteLine("## Always Disabled:")
                                Case "Enabled by Default"
                                    writer.WriteLine("## Enabled By Default:")
                                Case "Disabled by Default"
                                    writer.WriteLine("## Disabled By Default:")
                                Case Else
                                    writer.WriteLine("## Unknown:")
                            End Select

                            ' Write features in group
                            For Each feature In group.OrderBy(Function(f) f.Id)
                                Try
                                    writer.WriteLine($"{feature.Name}:{feature.Id}")
                                Catch ex As Exception
                                    Debug.WriteLine($"Warning: Failed to write feature {feature.Id}: {ex.Message}")
                                End Try
                            Next

                            writer.WriteLine()
                        Catch ex As Exception
                            Debug.WriteLine($"Warning: Failed to write group {group.Key}: {ex.Message}")
                        End Try
                    Next
                End Using

                _lastErrorMessage = String.Empty
                Debug.WriteLine($"TXT export completed successfully: {filePath}")
                Return True

            Catch ex As Exception
                _lastErrorMessage = $"TXT export error: {ex.Message}"
                Debug.WriteLine(_lastErrorMessage)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Escapes special characters in CSV values.
        ''' </summary>
        Private Function EscapeCSVValue(value As String) As String
            If String.IsNullOrEmpty(value) Then
                Return String.Empty
            End If

            ' If value contains comma, quote, or newline, wrap in quotes and escape internal quotes
            If value.Contains(",") OrElse value.Contains("""") OrElse value.Contains(vbCrLf) Then
                Return """" & value.Replace("""", """""") & """"
            End If

            Return value
        End Function
    End Class
End Namespace

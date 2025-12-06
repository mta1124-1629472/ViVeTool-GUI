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
Imports Albacore.ViVe
Imports ViVeTool_GUI.Wpf.Models

Namespace Services
    ''' <summary>
    ''' Service for managing Windows feature configurations.
    ''' Uses Albacore.ViVe.RtlFeatureManager to query and modify Windows features.
    ''' </summary>
    Public Class FeatureService
        ' Constants for FeatureConfiguration settings
        ' EnabledStateOptions = 1: Standard option for feature state updates
        Private Const EnabledStateOptionsValue As Integer = 1
        ' Group = 4: Standard group value for user-configured features
        Private Const GroupValue As Integer = 4
        ' Variant values (0 = default/no variant)
        Private Const VariantValue As Integer = 0
        Private Const VariantPayloadValue As Integer = 0
        Private Const VariantPayloadKindValue As Integer = 0

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
        ''' Gets all available features from the system by querying RtlFeatureManager.
        ''' </summary>
        ''' <returns>A collection of feature items.</returns>
        Public Async Function GetFeaturesAsync() As Task(Of ObservableCollection(Of FeatureItem))
            Return Await Task.Run(Function() GetFeaturesCore())
        End Function

        ''' <summary>
        ''' Core method to query all feature configurations from the system.
        ''' </summary>
        Private Function GetFeaturesCore() As ObservableCollection(Of FeatureItem)
            Dim result As New ObservableCollection(Of FeatureItem)()

            Try
                ' Query all feature configurations from both Runtime and Boot sections
                Dim runtimeConfigs = RtlFeatureManager.QueryAllFeatureConfigurations(FeatureConfigurationSection.Runtime)
                Dim bootConfigs = RtlFeatureManager.QueryAllFeatureConfigurations(FeatureConfigurationSection.Boot)

                ' Create a dictionary to merge configurations by FeatureId
                Dim featureDict As New Dictionary(Of UInteger, FeatureItem)()

                ' Process runtime configurations first
                If runtimeConfigs IsNot Nothing Then
                    For Each config In runtimeConfigs
                        Dim stateStr As String = config.EnabledState.ToString()
                        Dim isEnabled As Boolean = (config.EnabledState = FeatureEnabledState.Enabled)
                        Dim featureItem As New FeatureItem(
                            CInt(config.FeatureId),
                            $"Feature {config.FeatureId}",
                            stateStr,
                            isEnabled,
                            $"Group: {config.Group}, Variant: {config.Variant}"
                        )
                        featureDict(config.FeatureId) = featureItem
                    Next
                End If

                ' Process boot configurations (may override or add to runtime)
                If bootConfigs IsNot Nothing Then
                    For Each config In bootConfigs
                        If Not featureDict.ContainsKey(config.FeatureId) Then
                            Dim stateStr As String = config.EnabledState.ToString()
                            Dim isEnabled As Boolean = (config.EnabledState = FeatureEnabledState.Enabled)
                            Dim featureItem As New FeatureItem(
                                CInt(config.FeatureId),
                                $"Feature {config.FeatureId}",
                                stateStr,
                                isEnabled,
                                $"Group: {config.Group}, Variant: {config.Variant} (Boot)"
                            )
                            featureDict(config.FeatureId) = featureItem
                        End If
                    Next
                End If

                ' Add all features to the result collection
                For Each kvp In featureDict.OrderBy(Function(x) x.Key)
                    result.Add(kvp.Value)
                Next

            Catch ex As Exception
                ' If querying fails (e.g., on non-Windows or permission issues),
                ' return an empty collection with an error indicator
                _lastErrorMessage = ex.Message
                result.Add(New FeatureItem(
                    0,
                    "Error loading features",
                    "Error",
                    False,
                    $"Failed to query features: {ex.Message}"
                ))
            End Try

            Return result
        End Function

        ''' <summary>
        ''' Enables a specific feature by setting its EnabledState to Enabled.
        ''' </summary>
        ''' <param name="featureId">The feature ID to enable.</param>
        ''' <returns>True if successful, false otherwise.</returns>
        Public Async Function EnableFeatureAsync(featureId As Integer) As Task(Of Boolean)
            Return Await Task.Run(Function() SetFeatureConfiguration(CUInt(featureId), FeatureEnabledState.Enabled))
        End Function

        ''' <summary>
        ''' Disables a specific feature by setting its EnabledState to Disabled.
        ''' </summary>
        ''' <param name="featureId">The feature ID to disable.</param>
        ''' <returns>True if successful, false otherwise.</returns>
        Public Async Function DisableFeatureAsync(featureId As Integer) As Task(Of Boolean)
            Return Await Task.Run(Function() SetFeatureConfiguration(CUInt(featureId), FeatureEnabledState.Disabled))
        End Function

        ''' <summary>
        ''' Reverts a specific feature to its default state.
        ''' </summary>
        ''' <param name="featureId">The feature ID to revert.</param>
        ''' <returns>True if successful, false otherwise.</returns>
        Public Async Function RevertFeatureAsync(featureId As Integer) As Task(Of Boolean)
            Return Await Task.Run(Function() SetFeatureConfiguration(CUInt(featureId), FeatureEnabledState.Default))
        End Function

        ''' <summary>
        ''' Applies a feature configuration (enable or disable).
        ''' </summary>
        ''' <param name="featureId">The feature ID.</param>
        ''' <param name="enable">True to enable, false to disable.</param>
        ''' <returns>True if successful, false otherwise.</returns>
        Public Async Function ApplyFeatureAsync(featureId As Integer, enable As Boolean) As Task(Of Boolean)
            If enable Then
                Return Await EnableFeatureAsync(featureId)
            Else
                Return Await DisableFeatureAsync(featureId)
            End If
        End Function

        ''' <summary>
        ''' Sets a feature configuration using RtlFeatureManager.
        ''' This method sets both boot and live configurations.
        ''' </summary>
        ''' <param name="featureId">The feature ID to configure.</param>
        ''' <param name="enabledState">The desired enabled state.</param>
        ''' <returns>True if successful, false otherwise.</returns>
        Private Function SetFeatureConfiguration(featureId As UInteger, enabledState As FeatureEnabledState) As Boolean
            Try
                ' Create the feature configuration using defined constants
                Dim config As New FeatureConfiguration() With {
                    .FeatureId = featureId,
                    .EnabledState = enabledState,
                    .EnabledStateOptions = EnabledStateOptionsValue,
                    .Group = GroupValue,
                    .Variant = VariantValue,
                    .VariantPayload = VariantPayloadValue,
                    .VariantPayloadKind = VariantPayloadKindValue,
                    .Action = FeatureConfigurationAction.UpdateEnabledState
                }

                Dim configs As New List(Of FeatureConfiguration) From {config}

                ' Set both boot and live configurations
                ' On successful operations:
                ' - SetBootFeatureConfigurations returns True
                ' - SetLiveFeatureConfigurations returns 0
                Dim bootResult As Boolean = RtlFeatureManager.SetBootFeatureConfigurations(configs)
                Dim liveResult As Integer = RtlFeatureManager.SetLiveFeatureConfigurations(configs, FeatureConfigurationSection.Runtime)

                Return bootResult AndAlso (liveResult = 0)
            Catch ex As Exception
                ' Store error message for debugging
                _lastErrorMessage = ex.Message
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Queries a single feature's current configuration.
        ''' </summary>
        ''' <param name="featureId">The feature ID to query.</param>
        ''' <returns>A FeatureItem if found, Nothing otherwise.</returns>
        Public Async Function GetFeatureAsync(featureId As Integer) As Task(Of FeatureItem)
            Return Await Task.Run(Function() GetFeatureCore(CUInt(featureId)))
        End Function

        ''' <summary>
        ''' Core method to query a single feature configuration.
        ''' </summary>
        Private Function GetFeatureCore(featureId As UInteger) As FeatureItem
            Try
                Dim config = RtlFeatureManager.QueryFeatureConfiguration(featureId, FeatureConfigurationSection.Runtime)
                
                ' Check for null explicitly instead of catching NullReferenceException
                If config Is Nothing Then
                    ' Feature not configured, return default state
                    Return New FeatureItem(
                        CInt(featureId),
                        $"Feature {featureId}",
                        "Default",
                        False,
                        "Not configured"
                    )
                End If

                Dim stateStr As String = config.EnabledState.ToString()
                Dim isEnabled As Boolean = (config.EnabledState = FeatureEnabledState.Enabled)
                Return New FeatureItem(
                    CInt(config.FeatureId),
                    $"Feature {config.FeatureId}",
                    stateStr,
                    isEnabled,
                    $"Group: {config.Group}, Variant: {config.Variant}"
                )
            Catch ex As Exception
                _lastErrorMessage = ex.Message
            End Try

            Return Nothing
        End Function
    End Class
End Namespace

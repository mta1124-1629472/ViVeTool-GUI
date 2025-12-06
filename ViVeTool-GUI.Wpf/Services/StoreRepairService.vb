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

Imports System.Diagnostics
Imports Albacore.ViVe

Namespace Services
    ''' <summary>
    ''' Service for repairing Windows feature store and A/B Testing configurations.
    ''' Uses Albacore.ViVe.RtlFeatureManager to apply repairs.
    ''' </summary>
    Public Class StoreRepairService
        ' Feature IDs for store repair
        Private Const LastKnownGoodStoreFeatureId As UInteger = &H8CFFFFF4
        Private Const ABTestingPrioritiesFeatureId As UInteger = &H8CFFFFF5

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
        ''' Repairs the LastKnownGood Store by disabling the repair feature.
        ''' This fixes corrupted Windows feature store states.
        ''' </summary>
        ''' <returns>True if successful, false otherwise.</returns>
        Public Async Function RepairStoreAsync() As Task(Of Boolean)
            Return Await Task.Run(Function() RepairStoreCore())
        End Function

        ''' <summary>
        ''' Core method to repair the store.
        ''' </summary>
        Private Function RepairStoreCore() As Boolean
            Try
                ' Create configuration to disable LastKnownGood Store feature
                Dim config As New FeatureConfiguration() With {
                    .FeatureId = LastKnownGoodStoreFeatureId,
                    .EnabledState = FeatureEnabledState.Disabled,
                    .EnabledStateOptions = 1,
                    .Group = 4,
                    .Variant = 0,
                    .VariantPayload = 0,
                    .VariantPayloadKind = 0,
                    .Action = FeatureConfigurationAction.UpdateEnabledState
                }

                Dim configs As New List(Of FeatureConfiguration) From {config}

                ' Set boot configuration
                Dim bootResult As Boolean = RtlFeatureManager.SetBootFeatureConfigurations(configs)
                
                If bootResult Then
                    _lastErrorMessage = String.Empty
                    Debug.WriteLine("Store repair completed successfully")
                    Return True
                Else
                    _lastErrorMessage = "Store repair failed: SetBootFeatureConfigurations returned false"
                    Debug.WriteLine(_lastErrorMessage)
                    Return False
                End If
            Catch ex As Exception
                _lastErrorMessage = $"Store repair error: {ex.Message}"
                Debug.WriteLine(_lastErrorMessage)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Fixes A/B Testing Priorities by resetting the configuration.
        ''' </summary>
        ''' <returns>True if successful, false otherwise.</returns>
        Public Async Function FixABTestingAsync() As Task(Of Boolean)
            Return Await Task.Run(Function() FixABTestingCore())
        End Function

        ''' <summary>
        ''' Core method to fix A/B testing.
        ''' </summary>
        Private Function FixABTestingCore() As Boolean
            Try
                ' Create configuration to reset A/B Testing Priorities
                Dim config As New FeatureConfiguration() With {
                    .FeatureId = ABTestingPrioritiesFeatureId,
                    .EnabledState = FeatureEnabledState.Default,
                    .EnabledStateOptions = 1,
                    .Group = 4,
                    .Variant = 0,
                    .VariantPayload = 0,
                    .VariantPayloadKind = 0,
                    .Action = FeatureConfigurationAction.UpdateEnabledState
                }

                Dim configs As New List(Of FeatureConfiguration) From {config}

                ' Set boot configuration
                Dim bootResult As Boolean = RtlFeatureManager.SetBootFeatureConfigurations(configs)
                
                If bootResult Then
                    _lastErrorMessage = String.Empty
                    Debug.WriteLine("A/B Testing fix completed successfully")
                    Return True
                Else
                    _lastErrorMessage = "A/B Testing fix failed: SetBootFeatureConfigurations returned false"
                    Debug.WriteLine(_lastErrorMessage)
                    Return False
                End If
            Catch ex As Exception
                _lastErrorMessage = $"A/B Testing fix error: {ex.Message}"
                Debug.WriteLine(_lastErrorMessage)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Performs both store repair and A/B testing fix in sequence.
        ''' </summary>
        ''' <returns>True if both operations successful, false otherwise.</returns>
        Public Async Function RepairAllAsync() As Task(Of Boolean)
            Return Await Task.Run(Function() RepairAllCore())
        End Function

        ''' <summary>
        ''' Core method to perform all repairs.
        ''' </summary>
        Private Function RepairAllCore() As Boolean
            Try
                ' Perform store repair first
                Dim storeRepairSuccess As Boolean = RepairStoreCore()
                If Not storeRepairSuccess Then
                    Debug.WriteLine("Store repair failed, but continuing with A/B testing fix")
                End If

                ' Then perform A/B testing fix
                Dim abTestingSuccess As Boolean = FixABTestingCore()
                If Not abTestingSuccess Then
                    Debug.WriteLine("A/B testing fix failed")
                End If

                ' Return true only if both succeeded
                Return storeRepairSuccess AndAlso abTestingSuccess
            Catch ex As Exception
                _lastErrorMessage = $"Repair all error: {ex.Message}"
                Debug.WriteLine(_lastErrorMessage)
                Return False
            End Try
        End Function
    End Class
End Namespace

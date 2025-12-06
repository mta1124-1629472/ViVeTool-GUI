'ViVeTool-GUI - Windows Feature Control GUI for ViVeTool
'Copyright (C) 2022  Peter Strick / Peters Software Solutions
'
'This program is free software: you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License for more details.
'
'You should have received a copy of the GNU General Public License
'along with this program.  If not, see <https://www.gnu.org/licenses/>.
Imports Telerik.WinControls.UI

''' <summary>
''' Helper class for common dialog operations
''' </summary>
Public NotInheritable Class DialogHelper
    ''' <summary>
    ''' Shows an exception dialog with a "Copy Exception and Close" button
    ''' </summary>
    ''' <param name="caption">Dialog caption</param>
    ''' <param name="heading">Dialog heading</param>
    ''' <param name="exceptionText">Exception text to display in expander</param>
    ''' <param name="additionalText">Optional additional text to display</param>
    Public Shared Sub ShowExceptionDialog(caption As String, heading As String, exceptionText As String, Optional additionalText As String = Nothing)
        'Create a Button that on Click, copies the Exception Text
        Dim CopyExAndClose As New RadTaskDialogButton With {
            .Text = "Copy Exception and Close"
        }
        AddHandler CopyExAndClose.Click, New EventHandler(Sub()
                                                              Try
                                                                  My.Computer.Clipboard.SetText(exceptionText)
                                                              Catch clipex As Exception
                                                                  'Do nothing
                                                              End Try
                                                          End Sub)

        'Fancy Message Box
        Dim RTD As New RadTaskDialogPage With {
                .Caption = caption,
                .Heading = heading,
                .Icon = RadTaskDialogIcon.ShieldErrorRedBar
            }

        'Add additional text if provided
        If additionalText IsNot Nothing Then
            RTD.Text = additionalText
        End If

        'Add the Exception Text to the Expander
        RTD.Expander.Text = exceptionText

        'Set the Text for the "Collapse Info" and "More Info" Buttons
        RTD.Expander.ExpandedButtonText = "Collapse Exception"
        RTD.Expander.CollapsedButtonText = "Show Exception"

        'Add the Button to the Message Box
        RTD.CommandAreaButtons.Add(CopyExAndClose)

        'Show the Message Box
        RadTaskDialog.ShowDialog(RTD)
    End Sub

    ''' <summary>
    ''' Shows an exception dialog for a generic unknown exception
    ''' </summary>
    ''' <param name="ex">The exception to display</param>
    Public Shared Sub ShowUnknownExceptionDialog(ex As Exception)
        ShowExceptionDialog(" An Exception occurred", "An unknown Exception occurred.", ex.ToString)
    End Sub

    ''' <summary>
    ''' Shows a network exception dialog
    ''' </summary>
    ''' <param name="webex">The WebException to display</param>
    Public Shared Sub ShowNetworkExceptionDialog(webex As WebException)
        Dim exceptionText As String
        Try
            exceptionText = "GitHub API Response: " & DirectCast(webex.Response, HttpWebResponse).StatusDescription
        Catch ex As Exception
            exceptionText = webex.ToString
        End Try

        'Create a Button that on Click, copies the Exception Text
        Dim CopyExAndClose As New RadTaskDialogButton With {
            .Text = "Copy Exception and Close"
        }
        AddHandler CopyExAndClose.Click, New EventHandler(Sub()
                                                              Try
                                                                  Dim statusDesc As String
                                                                  Try
                                                                      statusDesc = DirectCast(webex.Response, HttpWebResponse).StatusDescription
                                                                  Catch
                                                                      statusDesc = webex.ToString
                                                                  End Try
                                                                  My.Computer.Clipboard.SetText(statusDesc)
                                                              Catch clipex As Exception
                                                                  'Do nothing
                                                              End Try
                                                          End Sub)

        Dim RTD As New RadTaskDialogPage With {
                .Caption = " A Network Exception occurred",
                .Heading = "A Network Exception occurred. Your IP may have been temporarily rate limited by the GitHub API for an hour.",
                .Icon = RadTaskDialogIcon.ShieldErrorRedBar
            }
        RTD.Expander.Text = exceptionText
        RTD.Expander.ExpandedButtonText = "Collapse Exception"
        RTD.Expander.CollapsedButtonText = "Show Exception"
        RTD.CommandAreaButtons.Add(CopyExAndClose)
        RadTaskDialog.ShowDialog(RTD)
    End Sub

    ''' <summary>
    ''' Shows a simple error dialog without exception details
    ''' </summary>
    ''' <param name="caption">Dialog caption</param>
    ''' <param name="heading">Dialog heading</param>
    ''' <param name="text">Optional additional text</param>
    ''' <param name="icon">Optional icon (defaults to Error)</param>
    Public Shared Sub ShowErrorDialog(caption As String, heading As String, Optional text As String = Nothing, Optional icon As RadTaskDialogIcon = Nothing)
        Dim RTD As New RadTaskDialogPage With {
                .Caption = caption,
                .Heading = heading,
                .Icon = If(icon Is Nothing, RadTaskDialogIcon.Error, icon)
            }
        If text IsNot Nothing Then
            RTD.Text = text
        End If
        RTD.CommandAreaButtons.Add(RadTaskDialogButton.Close)
        RadTaskDialog.ShowDialog(RTD)
    End Sub

    ''' <summary>
    ''' Shows a success dialog
    ''' </summary>
    ''' <param name="caption">Dialog caption</param>
    ''' <param name="heading">Dialog heading</param>
    Public Shared Sub ShowSuccessDialog(caption As String, heading As String)
        Dim RTD As New RadTaskDialogPage With {
                .Caption = caption,
                .Heading = heading,
                .Icon = RadTaskDialogIcon.ShieldSuccessGreenBar
            }
        RTD.CommandAreaButtons.Add(RadTaskDialogButton.Close)
        RadTaskDialog.ShowDialog(RTD)
    End Sub
End Class

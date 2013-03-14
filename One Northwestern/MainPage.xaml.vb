Imports System
Imports System.Threading
Imports System.Windows.Controls
Imports Microsoft.Phone.Controls
Imports Microsoft.Phone.Shell
Imports Coding4Fun.Toolkit.Controls
Partial Public Class MainPage
    Inherits PhoneApplicationPage

    ' Constructor
    Public Sub New()
        InitializeComponent()



        ' Sample code to localize the ApplicationBar
        'BuildLocalizedApplicationBar()

    End Sub

    ' Sample code for building a localized ApplicationBar
    'Private Sub BuildLocalizedApplicationBar()
    '    ' Set the page's ApplicationBar to a new instance of ApplicationBar.
    '    ApplicationBar = New ApplicationBar()

    '    ' Create a new button and set the text value to the localized string from AppResources.
    '    Dim appBarButton As New ApplicationBarIconButton(New Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative))
    '    appBarButton.Text = AppResources.AppBarButtonText
    '    ApplicationBar.Buttons.Add(appBarButton)

    '    ' Create a new menu item with the localized string from AppResources.
    '    Dim appBarMenuItem As New ApplicationBarMenuItem(AppResources.AppBarMenuItemText)
    '    ApplicationBar.MenuItems.Add(appBarMenuItem)
    'End Sub

   



    Private Sub forgotpass_Tap(sender As Object, e As Input.GestureEventArgs) Handles forgotpass.Tap

        Dim input = New InputPrompt
        input.Title = "No Problem"
        input.Message = "Well send you a reset link on your email: "
        input.Show()



    End Sub

    Private Sub login_Tap(sender As Object, e As Input.GestureEventArgs) Handles login.Tap
        If (Username.Text = "mike@u.northwestern.edu") And (Password.Password = "mike") Then
            NavigationService.Navigate(New Uri("/Results.xaml", UriKind.Relative))

        Else
            MessageText.Visibility = System.Windows.Visibility.Visible
            messageicon.Visibility = System.Windows.Visibility.Visible

        End If
    End Sub


    
End Class
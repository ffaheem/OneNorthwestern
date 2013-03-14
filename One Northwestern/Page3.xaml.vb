Partial Public Class Page3
    Inherits PhoneApplicationPage

    Public Sub New()
        InitializeComponent()
    End Sub


    Private Sub card1_Tap(sender As Object, e As Input.GestureEventArgs) Handles card1.Tap
        NavigationService.Navigate(New Uri("/PivotPage2.xaml", UriKind.Relative))
    End Sub

    Private Sub onenorthwestern_Tap(sender As Object, e As Input.GestureEventArgs) Handles onenorthwestern.Tap
        NavigationService.Navigate(New Uri("/Results.xaml", UriKind.Relative))
    End Sub
End Class

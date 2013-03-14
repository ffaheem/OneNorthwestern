Partial Public Class Page6
    Inherits PhoneApplicationPage

    Public Sub New()
        InitializeComponent()
    End Sub



    Private Sub chosenone_Tap(sender As Object, e As Input.GestureEventArgs) Handles chosenone.Tap
        NavigationService.Navigate(New Uri("/PivotPage2.xaml", UriKind.Relative))
    End Sub

    Private Sub onenorthwestern_Tap(sender As Object, e As Input.GestureEventArgs) Handles onenorthwestern.Tap
        NavigationService.Navigate(New Uri("/Results.xaml", UriKind.Relative))
    End Sub
End Class

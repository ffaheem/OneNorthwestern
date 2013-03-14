Partial Public Class Page9
    Inherits PhoneApplicationPage

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub chosenone_Tap(sender As Object, e As Input.GestureEventArgs) Handles chosenone.Tap
        NavigationService.Navigate(New Uri("/PivotPage3.xaml", UriKind.Relative))
    End Sub
End Class

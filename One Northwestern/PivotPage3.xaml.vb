Partial Public Class PivotPage3
    Inherits PhoneApplicationPage

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Pivot_Loaded_1(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub onenorthwestern_Tap(sender As Object, e As Input.GestureEventArgs) Handles onenorthwestern.Tap
        NavigationService.Navigate(New Uri("/Results.xaml", UriKind.Relative))
    End Sub
End Class

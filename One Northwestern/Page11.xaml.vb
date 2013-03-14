Partial Public Class Page11
    Inherits PhoneApplicationPage

    Public Sub New()
        InitializeComponent()
    End Sub


    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        NavigationService.Navigate(New Uri("/accomadation.xaml", UriKind.Relative))
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        NavigationService.Navigate(New Uri("/accomadation.xaml", UriKind.Relative))
    End Sub

    Private Sub onenorthwestern_Tap(sender As Object, e As Input.GestureEventArgs) Handles onenorthwestern.Tap
        NavigationService.Navigate(New Uri("/Results.xaml", UriKind.Relative))
    End Sub
End Class

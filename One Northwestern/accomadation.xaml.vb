Partial Public Class Page2
    Inherits PhoneApplicationPage

    Public Sub New()
        InitializeComponent()
    End Sub


    Private Sub sliderprice_ManipulationDelta(sender As Object, e As ManipulationDeltaEventArgs) Handles sliderprice.ManipulationDelta
        priceValue.Text = "$" + sliderprice.Value.ToString
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        NavigationService.Navigate(New Uri("/Page3.xaml", UriKind.Relative))
    End Sub


    Private Sub advanced_Tap(sender As Object, e As Input.GestureEventArgs) Handles advanced.Tap
        NavigationService.Navigate(New Uri("/Page11.xaml", UriKind.Relative))
    End Sub

    Private Sub onenorthwestern_Tap(sender As Object, e As Input.GestureEventArgs) Handles onenorthwestern.Tap
        NavigationService.Navigate(New Uri("/Results.xaml", UriKind.Relative))
    End Sub

    
End Class

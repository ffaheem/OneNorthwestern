Partial Public Class Page8
    Inherits PhoneApplicationPage

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub slider_radius_ManipulationDelta(sender As Object, e As ManipulationDeltaEventArgs) Handles slider_radius.ManipulationDelta
        radiusvalue.Text = Math.Round(slider_radius.Value).ToString + " m"
    End Sub


    Private Sub search_Click(sender As Object, e As EventArgs) Handles search.Click
        NavigationService.Navigate(New Uri("/Page9.xaml", UriKind.Relative))
    End Sub

    

    Private Sub onenorthwestern_Tap(sender As Object, e As Input.GestureEventArgs) Handles onenorthwestern.Tap
        NavigationService.Navigate(New Uri("/Results.xaml", UriKind.Relative))
    End Sub
End Class

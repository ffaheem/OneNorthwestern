Imports Microsoft.Phone.Maps.Controls

Partial Public Class Page13
    Inherits PhoneApplicationPage
    Dim latt As String
    Dim longg As String


    Dim sender1 As String
    Dim memory1 As String
    Protected Overrides Sub OnNavigatedTo(ByVal e As System.Windows.Navigation.NavigationEventArgs)
        sender1 = "hello"
        memory1 = "hello"
        If (Me.NavigationContext.QueryString.ContainsKey("sender")) Then
            sender1 = Me.NavigationContext.QueryString("sender")
        End If
        If (Me.NavigationContext.QueryString.ContainsKey("memory")) Then
            memory1 = Me.NavigationContext.QueryString("memory")
        End If

    End Sub
    Public Sub New()
        InitializeComponent()
    End Sub
   

    Private Sub map1_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles map1.MouseLeftButtonUp
        map1.Layers.Clear()

        Dim MyGrid As New Grid
        MyGrid.RowDefinitions.Add(New RowDefinition())
        MyGrid.RowDefinitions.Add(New RowDefinition())
        MyGrid.Background = New SolidColorBrush(Colors.Transparent)


        'Creating a Rectangle
        Dim MyRectangle As New Rectangle
        MyRectangle.Fill = New SolidColorBrush(Colors.Black)
        MyRectangle.Height = 20
        MyRectangle.Width = 20
        MyRectangle.SetValue(Grid.RowProperty, 0)
        MyRectangle.SetValue(Grid.ColumnProperty, 0)

        'Adding the Rectangle to the Grid
        MyGrid.Children.Add(MyRectangle)

        'Creating a Polygon
        Dim MyPolygon As New Polygon
        MyPolygon.Points.Add(New Point(2, 0))
        MyPolygon.Points.Add(New Point(22, 0))
        MyPolygon.Points.Add(New Point(2, 40))
        MyPolygon.Stroke = New SolidColorBrush(Colors.Red)
        MyPolygon.Fill = New SolidColorBrush(Colors.Red)
        MyPolygon.SetValue(Grid.RowProperty, 1)
        MyPolygon.SetValue(Grid.ColumnProperty, 0)

        'Adding the Polygon to the Grid
        MyGrid.Children.Add(MyPolygon)



        Dim MyOverlay As New MapOverlay
        MyOverlay.Content = MyGrid

        MyOverlay.GeoCoordinate = map1.ConvertViewportPointToGeoCoordinate(e.GetPosition(map1))
        Dim geocoords(2) As String

        geocoords = MyOverlay.GeoCoordinate.ToString.Split(",")




        latt = geocoords(0).Trim
        longg = geocoords(1).Trim

        MyOverlay.PositionOrigin = New Point(0, 0.5)

        Dim MyLayer As New MapLayer
        MyLayer.Add(MyOverlay)
        map1.Layers.Add(MyLayer)
    End Sub







    Private Sub ApplicationBarIconButton_Click_1(sender As Object, e As EventArgs)
        NavigationService.Navigate(New Uri("/commute.xaml?longg=" + longg + "&latt=" + latt + "&target=" + sender1 + "&memory=" + memory1, UriKind.Relative))
    End Sub

    Private Sub zoomin_Click(sender As Object, e As RoutedEventArgs) Handles zoomin.Click
        If map1.ZoomLevel < 19.5 Then
            map1.ZoomLevel = map1.ZoomLevel + 0.5
        End If
    End Sub


    Private Sub zoomout_Click(sender As Object, e As RoutedEventArgs) Handles zoomout.Click
        If map1.ZoomLevel > 1 Then
            map1.ZoomLevel = map1.ZoomLevel - 0.5
        End If
    End Sub
End Class

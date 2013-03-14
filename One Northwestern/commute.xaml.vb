
Imports Microsoft.Phone.Tasks
Imports System.Device.Location
Imports Microsoft.Phone.Maps
Imports Microsoft.Phone.Maps.Controls
Imports Microsoft.Phone.Maps.Services
Imports System.Windows
Imports System.Windows.MessageBoxButton
Imports System.Windows.Controls
Imports System.Windows.Input
Imports Microsoft.Phone.Controls
Imports Windows.Phone.Speech.Recognition
Imports Windows.Phone.Speech.Synthesis
Imports Windows.Phone.Speech.VoiceCommands
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Xml.Linq
Imports System.Windows.Media.Imaging
Imports System
Imports System.Xml
Imports System.Windows.Threading
Imports System.Threading

Partial Public Class Page5
    Inherits PhoneApplicationPage
    Dim reciever As String
    Dim memory1 As String
    Dim longg As String
    Dim latt As String
    Protected Overrides Sub OnNavigatedTo(ByVal e As System.Windows.Navigation.NavigationEventArgs)
        If (Me.NavigationContext.QueryString.ContainsKey("target")) Then
            reciever = Me.NavigationContext.QueryString("target")
        End If
        If (Me.NavigationContext.QueryString.ContainsKey("memory")) Then
            memory1 = Me.NavigationContext.QueryString("memory")
        End If
        If (Me.NavigationContext.QueryString.ContainsKey("longg")) And (Me.NavigationContext.QueryString.ContainsKey("latt")) Then
            longg = Me.NavigationContext.QueryString("longg")
            latt = Me.NavigationContext.QueryString("latt")
            GetAddress(latt, longg)
            NavigationService.RemoveBackEntry()
        End If

    End Sub

    Public Delegate Sub MethodInvoker()
    Public Sub New()
        InitializeComponent()
    End Sub


    Dim Adress(20) As String
    
   
    Public Sub GetAddress(latt As String, longg As String)
        If reciever = "To" Then
            Totext.Text = "Please Wait.."
            FromText.Text = memory1
        Else
            FromText.Text = "Please Wait.."
            Totext.Text = memory1
        End If


        Dim apilink As String

        apilink = "http://maps.googleapis.com/maps/api/geocode/xml?latlng=" + latt + "," + longg + "&sensor=false"

        ' form the URI
        Dim fullUri As New UriBuilder(apilink)


        ' initialize a new WebRequest
        Dim forecastRequest As HttpWebRequest = CType(WebRequest.Create(fullUri.Uri), HttpWebRequest)

        ' set up the state object for the async request
        Dim forecastState As New ForecastUpdateState()
        forecastState.AsyncRequest = forecastRequest

        ' start the asynchronous request

        forecastRequest.BeginGetResponse(New AsyncCallback(AddressOf HandleGeoCodeResponse), forecastState)

    End Sub

    Private Sub HandleGeoCodeResponse(ByVal asyncResult As IAsyncResult)
        ' get the state information
        Dim forecastState As ForecastUpdateState = CType(asyncResult.AsyncState, ForecastUpdateState)
        Dim forecastRequest As HttpWebRequest = CType(forecastState.AsyncRequest, HttpWebRequest)

        ' end the async request
        forecastState.AsyncResponse = CType(forecastRequest.EndGetResponse(asyncResult), HttpWebResponse)

        Dim streamResult As Stream



        ' get the stream containing the response from the async call
        streamResult = forecastState.AsyncResponse.GetResponseStream()
        'streamResult = New FileStream("query.xml", FileMode.Open, FileAccess.Read)

        Dim xmlWeather As XElement = XElement.Load(streamResult)

        ' find the source element and retrieve the credit information
        'Dim xmlCurrent As XElement = xmlWeather.Descendants("subpod").First()
        'newCredit = CStr(xmlCurrent.Element("plaintext"))

        'answerme = (newCredit)

        '

        Dim count As Integer
        count = 0
        Dim query = From p In xmlWeather.Descendants("result") _
         Select p
        For Each record In query
            Adress(count) = record.Element("formatted_address").Value.ToString
            count = count + 1
        Next




        Me.Dispatcher.BeginInvoke(New MethodInvoker(AddressOf AdressUpdateinText))
    End Sub
    Private Sub AdressUpdateinText()
        If reciever = "To" Then
            Totext.Text = Adress(0)

        Else
            FromText.Text = Adress(0)

        End If




    End Sub
   

    Private Sub ApplicationBarIconButton_Click_1(sender As Object, e As EventArgs)
        NavigationService.Navigate(New Uri("/Page6.xaml", UriKind.Relative))
    End Sub



    


    Private Sub onenorthwestern_Tap(sender As Object, e As Input.GestureEventArgs) Handles onenorthwestern.Tap
        NavigationService.Navigate(New Uri("/Results.xaml", UriKind.Relative))
    End Sub

   
    Private Sub searchfrommap_Tap(sender As Object, e As Input.GestureEventArgs) Handles searchfrommap.Tap
        NavigationService.Navigate(New Uri("/Page13.xaml?sender=From&memory=" + Totext.Text, UriKind.Relative))
    End Sub

    Private Sub searchtomap_Tap(sender As Object, e As Input.GestureEventArgs) Handles searchtomap.Tap

        NavigationService.Navigate(New Uri("/Page13.xaml?sender=To&memory=" + FromText.Text, UriKind.Relative))
    End Sub
End Class

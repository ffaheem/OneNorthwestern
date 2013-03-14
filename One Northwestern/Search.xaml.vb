﻿Imports System.Windows
Imports System.Windows.MessageBoxButton
Imports System.Windows.Controls
Imports System.Windows.Input
Imports Microsoft.Phone.Controls
Imports Windows.Foundation
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


Partial Public Class Page1
    Inherits PhoneApplicationPage

    Public Delegate Sub MethodInvoker()

    Dim answerme(30) As String
    Dim pic(30) As String
    Dim countanswers As Integer
    Dim newRowText As RowDefinition
    Dim newRowPic As RowDefinition
    Dim countpics As Integer
    Dim answerTitle(30) As String
    Dim counttitle As Integer



#Region "Speech objects"
    ' Speech synthesizer for prompting and confirming
    Private speechOutput As SpeechSynthesizer

    ' Speech Recognzier for entering memos using the default dictation grammar
    Private speechInput As SpeechRecognizerUI

    ' Speech Recognizer for getting user commands. Uses mainPageCommands to add a list grammar
    Private commandReco As SpeechRecognizerUI

    ' List of commands to listen for on the mainpage
    Private mainPageCommands As New List(Of String)
#End Region
    Private Sub InitializeSpeech()
        speechOutput = New SpeechSynthesizer

        ' Simple dictation recognizer for filling in memo text
        speechInput = New SpeechRecognizerUI

        ' Set text to display to the user when recognizing
        speechInput.Settings.ExampleText = "Example: ""Burger king near Northwestern university"""
        speechInput.Settings.ListenText = "How can I be of service, Wildcat?"

        ' Recognizer for getting commands on this page
        commandReco = New SpeechRecognizerUI

        ' Set text to display to the user when recognizing
        commandReco.Settings.ExampleText = "Button name, ""Retry"", or ""Quit"""
        commandReco.Settings.ListenText = "Say next command..."

        ' Note: Grammars are intialized in the InitCommandGrammars method
    End Sub

    Private Async Sub GetNewMemoByVoice()
        Try
            ' Prompt the user using speech synthesis

            ' Use the default dictation grammar to get the new memo text
            Dim recoResult = Await speechInput.RecognizeWithUIAsync

            If recoResult.ResultStatus = SpeechRecognitionUIStatus.Succeeded Then
                ' Show new memo text

                Textbox1.Text = recoResult.RecognitionResult.Text

               
            Else
                Me.Dispatcher.BeginInvoke(Sub() MessageBox.Show(String.Format("Oops, something went wrong :(", recoResult.ResultStatus.ToString)))
            End If
        Catch ex As Exception
            Const privacyPolicyHResult As Integer = CInt(&H80045509)

            If ex.HResult = privacyPolicyHResult Then ' User has not accepted the speech privacy policy
                Me.Dispatcher.BeginInvoke(Sub() MessageBox.Show("The user has not accepted the speech privacy policy."))
            Else
                Me.Dispatcher.BeginInvoke(Sub() MessageBox.Show("Exception in dictation recognition" & ex.Message))
            End If
        End Try
    End Sub

    Protected Overrides Sub OnNavigatedTo(ByVal e As System.Windows.Navigation.NavigationEventArgs)
        If (Me.NavigationContext.QueryString.ContainsKey("query")) Then
            Textbox1.Text = Me.NavigationContext.QueryString("query")
            startsearch()
        End If

    End Sub


    Public Sub New()

        InitializeComponent() ' Autogenerated, do not modify
        InitializeSpeech()
       

    End Sub
    Private Async Sub startsearch()
        displaywa.Children.Clear()
        loader.Visibility = System.Windows.Visibility.Visible
        Status.Text = "Loading Answer...Please Wait"
        loader.Value = 20
        If Wolfram.IsChecked = True Then
            GetWolfram()
        Else
            GetYahoo()
        End If
        Await speechOutput.SpeakTextAsync("Please Wait")
        Array.Clear(answerTitle, 0, answerTitle.Length)
        Array.Clear(answerme, 0, answerme.Length)
        Array.Clear(pic, 0, pic.Length)
    End Sub
    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        startsearch()
    End Sub

    Public Sub GetWolfram()
        Dim question = Textbox1.Text
        question = question.Replace(" ", "%20")
        Dim apilink As String

        apilink = "http://api.wolframalpha.com/v2/query?input=" + question + "&appid=X3YH9K-KEE848758J"
        ' form the URI
        Dim fullUri As New UriBuilder(apilink)


        ' initialize a new WebRequest
        Dim forecastRequest As HttpWebRequest = CType(WebRequest.Create(fullUri.Uri), HttpWebRequest)

        ' set up the state object for the async request
        Dim forecastState As New ForecastUpdateState()
        forecastState.AsyncRequest = forecastRequest

        ' start the asynchronous request

        forecastRequest.BeginGetResponse(New AsyncCallback(AddressOf HandleWolframResponse), forecastState)
        loader.Value = 40
    End Sub
    Public Sub GetYahoo()
        Dim question = Textbox1.Text
        question = question.Replace(" ", "%20")
        Dim apilink As String
        apilink = "http://answers.yahooapis.com/AnswersService/V1/questionSearch?appid=rhNlpI72&query=" + question + "&type='resolved'"

        ' form the URI
        Dim fullUri As New UriBuilder(apilink)


        ' initialize a new WebRequest
        Dim forecastRequest As HttpWebRequest = CType(WebRequest.Create(fullUri.Uri), HttpWebRequest)

        ' set up the state object for the async request
        Dim forecastState As New ForecastUpdateState()
        forecastState.AsyncRequest = forecastRequest

        ' start the asynchronous request

        forecastRequest.BeginGetResponse(New AsyncCallback(AddressOf HandleYahooResponse), forecastState)
        loader.Value = 40
    End Sub
    ''' <summary>
    ''' Handle the information returned from the async request
    ''' </summary>
    ''' <param name="asyncResult"></param>
    ''' 


    Private Sub HandleWolframResponse(ByVal asyncResult As IAsyncResult)
        ' get the state information
        Dim forecastState As ForecastUpdateState = CType(asyncResult.AsyncState, ForecastUpdateState)
        Dim forecastRequest As HttpWebRequest = CType(forecastState.AsyncRequest, HttpWebRequest)

        ' end the async request
        forecastState.AsyncResponse = CType(forecastRequest.EndGetResponse(asyncResult), HttpWebResponse)
        Me.Dispatcher.BeginInvoke(New MethodInvoker(AddressOf UpdateLoader))
        Dim streamResult As Stream

  

    ' get the stream containing the response from the async call
        streamResult = forecastState.AsyncResponse.GetResponseStream()
    'streamResult = New FileStream("query.xml", FileMode.Open, FileAccess.Read)
        Me.Dispatcher.BeginInvoke(New MethodInvoker(AddressOf UpdateLoader))
    ' load the XML
    Dim xmlWeather As XElement = XElement.Load(streamResult)

    ' find the source element and retrieve the credit information
    'Dim xmlCurrent As XElement = xmlWeather.Descendants("subpod").First()
    'newCredit = CStr(xmlCurrent.Element("plaintext"))

    'answerme = (newCredit)

    '
        countanswers = 0
        countpics = 0
        counttitle = 0
    Dim query = From p In xmlWeather.Descendants("pod") _
     Select p
        For Each record In query
            answerTitle(counttitle) = record.Attribute("title").Value.ToString
            counttitle = counttitle + 1
        Next



        query = From p In xmlWeather.Descendants("subpod") _
         Select p
        For Each record In query
            answerme(countanswers) = (record.Element("plaintext").Value.ToString)
            countanswers = countanswers + 1
        Next

        query = From p In xmlWeather.Descendants("subpod") _
            Select p
        For Each record In query
            pic(countpics) = record.Element("img").Attribute("src").Value.ToString
            countpics = countpics + 1
        Next



        Me.Dispatcher.BeginInvoke(New MethodInvoker(AddressOf UpdateWolframAnswer))
        Me.Dispatcher.BeginInvoke(New MethodInvoker(AddressOf UpdateLoader))
    End Sub

    Private Sub HandleYahooResponse(ByVal asyncResult As IAsyncResult)
        ' get the state information
        Dim forecastState As ForecastUpdateState = CType(asyncResult.AsyncState, ForecastUpdateState)
        Dim forecastRequest As HttpWebRequest = CType(forecastState.AsyncRequest, HttpWebRequest)

        ' end the async request
        forecastState.AsyncResponse = CType(forecastRequest.EndGetResponse(asyncResult), HttpWebResponse)
        Me.Dispatcher.BeginInvoke(New MethodInvoker(AddressOf UpdateLoader))
        Dim streamResult As Stream



        ' get the stream containing the response from the async call
        streamResult = forecastState.AsyncResponse.GetResponseStream()
        'streamResult = New FileStream("questionSearch.xml", FileMode.Open, FileAccess.Read)
        Me.Dispatcher.BeginInvoke(New MethodInvoker(AddressOf UpdateLoader))
        ' load the XML
        Dim xmlWeather As XElement = XElement.Load(streamResult)



        '
        Dim ns As XNamespace


        ns = "urn:yahoo:answers"

        Dim query = From p In xmlWeather.Descendants(ns + "Question") _
             Select p
        For Each record In query
            answerTitle(countanswers) = (record.Element(ns + "Subject").Value.ToString)
            answerme(countanswers) = (record.Element(ns + "Content").Value.ToString)
            pic(countanswers) = (record.Element(ns + "ChosenAnswer").Value.ToString)
            countanswers = countanswers + 1
        Next





        Me.Dispatcher.BeginInvoke(New MethodInvoker(AddressOf UpdateYahooAnswer))
        Me.Dispatcher.BeginInvoke(New MethodInvoker(AddressOf UpdateLoader))
    End Sub



    Private Async Function UpdateYahooAnswer() As Tasks.Task

        Dim i As Integer

        i = 0

        loader.Value = 100
        loader.Visibility = System.Windows.Visibility.Collapsed


        Do While i < countanswers + countpics

            If answerme(i) <> vbNullString Or answerme(i) <> "" Then
                Dim titleheader As New TextBlock
                titleheader.Name = "newtitleheader" & i
                titleheader.Text = "Question:"
                titleheader.FontWeight = FontWeights.ExtraBold
                titleheader.FontSize = 30
                titleheader.Height = 40
                titleheader.Foreground = New SolidColorBrush(Colors.Cyan)
                titleheader.Margin = New Thickness(1)
                displaywa.Children.Add(titleheader)


                'create TitleQuestion
                Dim titleinfo As New TextBlock
                titleinfo.Name = "newtitle" & i
                titleinfo.Text = answerTitle(i)
                titleinfo.TextWrapping = TextWrapping.Wrap
                titleinfo.FontWeight = FontWeights.Bold
                titleinfo.FontSize = 27
                titleinfo.Height = Double.NaN
                titleinfo.Margin = New Thickness(1)
                displaywa.Children.Add(titleinfo)


                'create Subquestion
                Dim textinfo As New TextBlock
                textinfo.Name = "newinfo" & i
                textinfo.Text = answerme(i)
                textinfo.TextWrapping = TextWrapping.Wrap
                textinfo.FontWeight = FontWeights.SemiBold
                textinfo.FontSize = 20
                textinfo.Height = Double.NaN
                textinfo.Margin = New Thickness(1)
                displaywa.Children.Add(textinfo)


                Dim answerheader As New TextBlock
                answerheader.Name = "newanswerheader" & i
                answerheader.Text = "Answer:"
                answerheader.FontWeight = FontWeights.ExtraBold
                answerheader.FontSize = 30
                answerheader.Height = 40
                answerheader.Foreground = New SolidColorBrush(Colors.Blue)
                answerheader.Margin = New Thickness(1)
                displaywa.Children.Add(answerheader)


                'create answer
                Dim answerinfo As New TextBlock
                answerinfo.Name = "answerinfo" & i
                answerinfo.Text = pic(i)
                answerinfo.TextWrapping = TextWrapping.Wrap
                answerinfo.FontWeight = FontWeights.SemiBold
                answerinfo.FontSize = 20
                answerinfo.Height = Double.NaN
                answerinfo.Margin = New Thickness(1)
                displaywa.Children.Add(answerinfo)

                Dim breaker As New Rectangle
                breaker.Name = "newbreaker" & i
                breaker.Height = 3
                breaker.Fill = New SolidColorBrush(Colors.LightGray)
                breaker.Margin = New Thickness(5)
                displaywa.Children.Add(breaker)
            End If


            i = i + 1
        Loop


        Status.Text = "Loaded"
        If countanswers = 0 Then
            Status.Text = "Sorry, Couldn't find the answer"
            Await speechOutput.SpeakTextAsync("Sorry, Couldn't find the answer")
        End If

        displaywa.Height = Double.NaN

    End Function

    Private Async Function UpdateWolframAnswer() As Tasks.Task

        Dim i As Integer


        i = 0

        loader.Value = 100
        loader.Visibility = System.Windows.Visibility.Collapsed


        Do While i < countanswers

            If answerme(i) <> vbNullString Or answerme(i) <> "" Then
                'create Title
                Dim titleinfo As New TextBlock
                titleinfo.Name = "newtitle" & i

                titleinfo.Text = answerTitle(i)
                titleinfo.TextWrapping = TextWrapping.Wrap
                titleinfo.Height = Double.NaN
                titleinfo.FontWeight = FontWeights.ExtraBold
                titleinfo.FontSize = 30
                titleinfo.Margin = New Thickness(1)
                displaywa.Children.Add(titleinfo)



                'create textbox
                Dim textinfo As New TextBlock
                textinfo.Name = "newinfo" & i
                textinfo.Text = answerme(i)
                textinfo.TextWrapping = TextWrapping.Wrap
                textinfo.Height = Double.NaN
                textinfo.Margin = New Thickness(1)
                displaywa.Children.Add(textinfo)



            ElseIf pic(i) <> vbNullString Then 'check if picture exists for i in arrary
                'Create title
                Dim titleinfo As New TextBlock
                titleinfo.Name = "newtitle" & i
                titleinfo.Text = answerTitle(i)
                titleinfo.TextWrapping = TextWrapping.Wrap
                titleinfo.Height = Double.NaN
                titleinfo.FontWeight = FontWeights.ExtraBold
                titleinfo.FontSize = 30
                titleinfo.Margin = New Thickness(1)
                displaywa.Children.Add(titleinfo)



                'create picture
                Dim picbox As New Image
                Dim bi4 As New BitmapImage
                bi4.UriSource = New Uri(pic(i), UriKind.Absolute)

                picbox.Source = bi4
                picbox.Height = Double.NaN
                picbox.Stretch = Stretch.Uniform
                picbox.Margin = New Thickness(1)
                displaywa.Children.Add(picbox)
                displaywa.Children.IndexOf(picbox)




            End If


            i = i + 1
        Loop


        Status.Text = "Loaded"
        If countanswers = 0 Then
            Status.Text = "Sorry, Couldn't find the answer"
            Await speechOutput.SpeakTextAsync("Sorry, Couldn't find the answer")
        End If

        displaywa.Height = Double.NaN

    End Function

    Private Sub UpdateLoader()
        loader.Value = loader.Value + 20
    End Sub



    Private Sub Textbox1_Tap(sender As Object, e As Input.GestureEventArgs) Handles Textbox1.Tap

    End Sub



    Private Sub Textbox1_TextChanged_1(sender As Object, e As TextChangedEventArgs) Handles Textbox1.TextChanged

    End Sub


    Private Sub Image_Tap_1(sender As Object, e As Input.GestureEventArgs)



        Textbox1.Text = ""
        GetNewMemoByVoice()


    End Sub


    Private Sub Wolfram_Tap(sender As Object, e As Input.GestureEventArgs) Handles Wolfram.Tap
        Button_Click_1(sender, e)
    End Sub

    Private Sub Yahoo_Tap(sender As Object, e As Input.GestureEventArgs) Handles Yahoo.Tap
        Button_Click_1(sender, e)
    End Sub

    Private Sub speakbox_Click(sender As Object, e As EventArgs) Handles speakbox.Click
        Textbox1.Text = ""
        GetNewMemoByVoice()
    End Sub

    Private Sub search_Click(sender As Object, e As EventArgs) Handles search.Click
        startsearch()
    End Sub

    Private Sub onenorthwestern_Tap(sender As Object, e As Input.GestureEventArgs) Handles onenorthwestern.Tap
        NavigationService.Navigate(New Uri("/Results.xaml", UriKind.Relative))
    End Sub

   
End Class

Public Class ForecastUpdateState
    Public Property AsyncRequest() As HttpWebRequest
    Public Property AsyncResponse() As HttpWebResponse
End Class



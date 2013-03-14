Imports Windows.Phone.Speech.Synthesis
Imports Windows.Phone.Speech.Recognition
Imports System.Windows.Threading
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Imaging

Partial Public Class PivotPage1
    Inherits PhoneApplicationPage
    Private WithEvents timer As DispatcherTimer = New DispatcherTimer()
    Private counter As Integer = 0
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
        speechInput.Settings.ExampleText = "Example: ""Carpool from chicago to evanston"""
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
        InitializeTimer()

    End Sub

    Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)


        timer.Stop()

    End Sub

    Private Sub InitializeTimer()
        Dim generator As New Random

        ' Generate random value between 1 and 6.
        Dim value As Integer
        value = generator.Next(1, 6)
        timer.Interval = New TimeSpan(0, 0, value)
        timer.Start()

    End Sub

    Private Sub TimerClick(ByVal sender As System.Object, _
    ByVal e As EventArgs) Handles timer.Tick

        counter += 1

        Dim imagesource As String
        Dim activity As String
        Dim timelapsed As String


        imagesource = "/Assets/amy.png"
        activity = "Amy gave your post a rating"
        timelapsed = "3 Mins ago"

        If (counter Mod 2) = 0 Then
            imagesource = "/Assets/anjali.jpg"
            activity = "Anjali posted a new item"
            timelapsed = "2 Mins ago"
        End If

        If (counter Mod 3) = 0 Then
            imagesource = "/Assets/alex.png"
            activity = "Alex commented on a post"
            timelapsed = "5 Mins ago"
        End If

        Dim titleimage As New Image

        With titleimage

            .Name = "newtitleimage" & counter
            .HorizontalAlignment = System.Windows.HorizontalAlignment.Left
            .Height = 90
            .Margin = New Thickness(9, 10, 0, 0)
            .VerticalAlignment = System.Windows.VerticalAlignment.Top
            .Width = 90
            Dim bi4 As New BitmapImage
            bi4.UriSource = New Uri(imagesource, UriKind.Relative)
            .Source = bi4
            .Opacity = 0.8

        End With









        Dim postlabel As New TextBlock

        With postlabel

            .Name = "newpostlabel" & counter
            .HorizontalAlignment = System.Windows.HorizontalAlignment.Left

            .Margin = New Thickness(115, 10, 0, 0)
            .TextWrapping = TextWrapping.Wrap
            .Text = activity
            .VerticalAlignment = System.Windows.VerticalAlignment.Top
            .FontSize = 24


        End With




        Dim postvalue As New TextBlock

        With postvalue

            .Name = "newpostvalue" & counter
            .HorizontalAlignment = System.Windows.HorizontalAlignment.Left
            .Margin = New Thickness(115, 42, 0, 0)
            .TextWrapping = TextWrapping.Wrap
            .Text = timelapsed
            .VerticalAlignment = System.Windows.VerticalAlignment.Top
            .FontSize = 17


        End With







        Dim updateblock As New Grid

        With updateblock

            .Name = "newupdatebock" & counter
            .Opacity = 0
            .Children.Add(titleimage)
            .Children.Add(postlabel)
            .Children.Add(postvalue)

        End With
        AddHandler updateblock.Tap, AddressOf gotodetails

        foodupdater.Children.Insert(0, updateblock)


        Dim duration As Duration = New Duration(TimeSpan.FromSeconds(2))
        Dim fadeIn As New DoubleAnimation()
        fadeIn.Duration = duration
        Dim fadeOut As New DoubleAnimation()
        fadeOut.Duration = duration

        Dim sb As New Storyboard()

        sb.Duration = duration

        sb.Children.Add(fadeIn)

        Storyboard.SetTarget(fadeIn, updateblock)

        Storyboard.SetTargetProperty(fadeIn, New PropertyPath("(Opacity)"))

        fadeIn.To = 0.8

        foodupdater.Resources.Add("newupdateblock" & counter, sb)


        sb.Begin()




        foodupdater.Height = Double.NaN

        timer.Stop()
        InitializeTimer()

    End Sub

    Public Sub New()
        InitializeComponent()
        InitializeSpeech()
        InitializeTimer()
    End Sub
    Private Sub gotodetails()

        NavigationService.Navigate(New Uri("/PivotPage3.xaml", UriKind.Relative))

    End Sub

    Private Sub living_MouseLeave(sender As Object, e As MouseEventArgs) Handles living.MouseLeave
        living.Opacity = 0.7
    End Sub

    Private Sub living_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles living.MouseLeftButtonDown
        living.Opacity = 1
    End Sub

    Private Sub living_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles living.MouseLeftButtonUp
        living.Opacity = 0.7
    End Sub

    Private Sub living_Tap(sender As Object, e As Input.GestureEventArgs) Handles living.Tap
        NavigationService.Navigate(New Uri("/accomadation.xaml", UriKind.Relative))
    End Sub

    Private Sub commute_MouseLeave(sender As Object, e As MouseEventArgs) Handles commute.MouseLeave
        commute.Opacity = 0.7
    End Sub


    Private Sub commute_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles commute.MouseLeftButtonDown
        commute.Opacity = 1
    End Sub

    Private Sub commute_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles commute.MouseLeftButtonUp
        commute.Opacity = 0.7
    End Sub
    Private Sub commute_Tap(sender As Object, e As Input.GestureEventArgs) Handles commute.Tap
        NavigationService.Navigate(New Uri("/commute.xaml", UriKind.Relative))
    End Sub


    Private Sub food_MouseLeave(sender As Object, e As MouseEventArgs) Handles food.MouseLeave
        food.Opacity = 0.7
    End Sub

    Private Sub food_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles food.MouseLeftButtonDown
        food.Opacity = 1
    End Sub

    Private Sub food_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles food.MouseLeftButtonUp
        food.Opacity = 0.7
    End Sub

    Private Sub food_Tap(sender As Object, e As Input.GestureEventArgs) Handles food.Tap
        NavigationService.Navigate(New Uri("/Food.xaml", UriKind.Relative))
    End Sub







    Private Sub speakbox_Click(sender As Object, e As EventArgs) Handles speakbox.Click
        Textbox1.Text = ""
        GetNewMemoByVoice()
    End Sub

    Private Sub search_Click(sender As Object, e As EventArgs) Handles search.Click
        Dim carpool As String
        carpool = Textbox1.Text
        If carpool.StartsWith("Carpool") Then
            NavigationService.Navigate(New Uri("/Page6.xaml", UriKind.Relative))
        Else
            NavigationService.Navigate(New Uri("/Search.xaml?query=" + Textbox1.Text, UriKind.Relative))

        End If


    End Sub

    Private Sub onenorthwestern_Tap(sender As Object, e As Input.GestureEventArgs) Handles onenorthwestern.Tap
        NavigationService.Navigate(New Uri("/Results.xaml", UriKind.Relative))
    End Sub



End Class
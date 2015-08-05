using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Ktos.SayAnything.Resources;
using Ktos.SayAnything.Models;
using Windows.Phone.Speech.Synthesis;
using System.Threading.Tasks;
using Ktos.WindowsPhone;

namespace Ktos.SayAnything
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ApplicationBarIconButton btnPlay;
        private ApplicationBarIconButton btnPlayAndRecord;
        private XnaFakeLoop xfl;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            BuildLocalizedApplicationBar();

            xfl = new XnaFakeLoop();            
        }

        /// <summary>
        /// Budowanie zlokalizowanego ApplicationBar
        /// </summary>
        private void BuildLocalizedApplicationBar()
        {
            this.ApplicationBar = new ApplicationBar();

            btnPlay = new ApplicationBarIconButton(new Uri("/Assets/appbar.speaker.rest.png", UriKind.Relative));
            btnPlay.Click += appBarButton_Click;
            btnPlay.Text = AppResources.Say;
            ApplicationBar.Buttons.Add(btnPlay);

            btnPlayAndRecord = new ApplicationBarIconButton(new Uri("/Assets/appbar.speaker.rest.png", UriKind.Relative));
            btnPlayAndRecord.Click += btnPlayAndRecord_Click;
            btnPlayAndRecord.Text = AppResources.SayAndRecord;
            ApplicationBar.Buttons.Add(btnPlayAndRecord);

            var miSettings = new ApplicationBarMenuItem(AppResources.Settings);
            miSettings.Click += (s, ev) => { NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative)); };

            var miAbout = new ApplicationBarMenuItem(AppResources.About);
            miAbout.Click += (s, ev) => { NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative)); };

            ApplicationBar.MenuItems.Add(miSettings);
            ApplicationBar.MenuItems.Add(miAbout);

        }        

        async void btnPlayAndRecord_Click(object sender, EventArgs e)
        {
            xfl.Start();

            var fileName = "record.wav";
            Microphone microphone = new Microphone();

            btnPlay.IsEnabled = false;
            btnPlayAndRecord.IsEnabled = false;
            
            microphone.Start();
            await sayText();
            microphone.Stop();

            await microphone.SaveToIsolatedStorageAsync(fileName);

            btnPlayAndRecord.IsEnabled = true;
            btnPlay.IsEnabled = true;            
            
            microphone.SaveToMediaLibrary(fileName, AppResources.AppName, tbUserText.Text.Substring(0, 10));

            xfl.Stop();
        }

        async void appBarButton_Click(object sender, EventArgs e)
        {
            btnPlay.IsEnabled = false;
            btnPlayAndRecord.IsEnabled = false;
            await sayText();
            btnPlayAndRecord.IsEnabled = true;
            btnPlay.IsEnabled = true;            
        }

        async Task sayText()
        {
            try
            {
                VoiceInformation v = (VoiceInformation)PhoneApplicationService.Current.State["voice"];
                var text = tbUserText.Text;
                text = System.Text.RegularExpressions.Regex.Replace(text, "<[^>]*(>|$)", "");
                await Speech.Say(text, v.Language, v.Gender);
            }
            catch (Exception ex)
            {
                MessageBox.Show(AppResources.msgPlayError + "\n" + ex.Message);
            }
        }

        private void tbUserText_TextInputStart(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            this.svText.UpdateLayout();
            this.svText.ScrollToVerticalOffset(this.tbUserText.ActualHeight);            
        }
    }
}
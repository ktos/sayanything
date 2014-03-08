using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Ktos.SayAnything.Resources;
using Ktos.SayAnything.Models;
using Windows.Phone.Speech.Synthesis;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Threading;
using Microsoft.Xna.Framework;

namespace Ktos.SayAnything
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ApplicationBarIconButton btnPlay;
        private ApplicationBarIconButton btnPlayAndRecord;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            BuildLocalizedApplicationBar();

            // Timer to simulate the XNA Game Studio game loop (Microphone is from XNA Game Studio)
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(50);
            dt.Tick += delegate { try { FrameworkDispatcher.Update(); } catch { } };
            dt.Start();
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

        private MemoryStream ms;

        async void btnPlayAndRecord_Click(object sender, EventArgs e)
        {
            Microphone microphone = new Microphone();

            btnPlay.IsEnabled = false;
            btnPlayAndRecord.IsEnabled = false;
            microphone.Start();
            await sayText();
            microphone.Stop();

            await microphone.SaveToIsolatedStorage("record.wav");
            btnPlayAndRecord.IsEnabled = true;
            btnPlay.IsEnabled = true;
        }

        async void appBarButton_Click(object sender, EventArgs e)
        {
            /*btnPlay.IsEnabled = false;
            btnPlayAndRecord.IsEnabled = false;
            await sayText();
            btnPlayAndRecord.IsEnabled = true;
            btnPlay.IsEnabled = true;*/
            Microphone m = new Microphone();
            m.SaveToMediaLibrary();
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
                MessageBox.Show(AppResources.msgPlayError);
            }
        }

        private void tbUserText_TextInputStart(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            this.svText.UpdateLayout();
            this.svText.ScrollToVerticalOffset(this.tbUserText.ActualHeight);
        }
    }
}
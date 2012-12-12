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

namespace Ktos.SayAnything
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ApplicationBarIconButton btnPlay;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// Budowanie zlokalizowanego ApplicationBar
        /// </summary>
        private void BuildLocalizedApplicationBar()
        {
            this.ApplicationBar = new ApplicationBar();

            btnPlay = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.play.rest.png", UriKind.Relative));
            btnPlay.Click += appBarButton_Click;
            btnPlay.Text = AppResources.Play;
            ApplicationBar.Buttons.Add(btnPlay);

            var miSettings = new ApplicationBarMenuItem(AppResources.Settings);
            miSettings.Click += (s, ev) => { NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative)); };

            var miAbout = new ApplicationBarMenuItem(AppResources.About);
            miAbout.Click += (s, ev) => { NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative)); };

            ApplicationBar.MenuItems.Add(miSettings);
            ApplicationBar.MenuItems.Add(miAbout);

        }

        async void appBarButton_Click(object sender, EventArgs e)
        {
            try
            {
                VoiceInformation v = (VoiceInformation)PhoneApplicationService.Current.State["voice"];
                var text = tbUserText.Text;
                text = System.Text.RegularExpressions.Regex.Replace(text, "<[^>]*(>|$)", "");
                btnPlay.IsEnabled = false;
                await Speech.Say(text, v.Language, v.Gender);
                btnPlay.IsEnabled = true;
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
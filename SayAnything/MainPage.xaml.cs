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

            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.play.rest.png", UriKind.Relative));
            appBarButton.Click += appBarButton_Click;
            appBarButton.Text = AppResources.Play;
            ApplicationBar.Buttons.Add(appBarButton);

            var miSettings = new ApplicationBarMenuItem(AppResources.Settings);
            miSettings.Click += (s, ev) => { NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative)); };

            var miAbout = new ApplicationBarMenuItem(AppResources.About);
            miAbout.Click += (s, ev) => { NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative)); };

            ApplicationBar.MenuItems.Add(miSettings);
            ApplicationBar.MenuItems.Add(miAbout);

        }

        void appBarButton_Click(object sender, EventArgs e)
        {
            try
            {
                VoiceInformation v = (VoiceInformation)PhoneApplicationService.Current.State["voice"];
                Speech.Say(tbUserText.Text, v.Language, v.Gender);
            }
            catch (Exception ex)
            {
                MessageBox.Show(AppResources.msgPlayError);
            }
        }
    }
}
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
using Windows.Phone.Speech.Synthesis;

namespace Ktos.SayAnything
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.play.rest.png", UriKind.Relative));
            appBarButton.Click += appBarButton_Click;
            //appBarButton.Text = AppResources.AppBarButtonText;
            ApplicationBar.Buttons.Add(appBarButton);

            // Create a new menu item with the localized string from AppResources.
            //ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            //ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        async void appBarButton_Click(object sender, EventArgs e)
        {
            var text2speech = new SpeechSynthesizer();
            await text2speech.SpeakSsmlAsync(@"<speak version=""1.0""
                xmlns=""http://www.w3.org/2001/10/synthesis"" xml:lang=""pl-PL"">
                <voice gender=""male"">" + tbUserText.Text + "</voice></speak>");
        }
    }
}
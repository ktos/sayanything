using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Ktos.SayAnything.Models;
using Windows.Phone.Speech.Synthesis;

namespace Ktos.SayAnything
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        private bool selectable;

        public SettingsPage()
        {
            selectable = false;
            InitializeComponent();

            lpVoice.ItemsSource = Speech.GetVoices();                        
            try
            {
                lpVoice.SelectedIndex = (int)PhoneApplicationService.Current.State["index"];
            }
            catch (Exception ex)
            {
                lpVoice.SelectedIndex = 0;
            }
            selectable = true;
        }

        private void cbVoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectable)
            {
                var v = (VoiceInformation)lpVoice.SelectedItem;
                PhoneApplicationService.Current.State["voice"] = v;
                PhoneApplicationService.Current.State["index"] = lpVoice.SelectedIndex;
                App.VoiceId = v.Id;
            }
        }
    }
}
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
        public SettingsPage()
        {
            InitializeComponent();

            ContentPanel.DataContext = Speech.GetVoices();
            lpVoice.SelectedItem = Speech.GetVoices().Select(x => x.Id == App.VoiceId).First();
        }

        private void cbVoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            var v = (VoiceInformation)e.AddedItems[0];
            PhoneApplicationService.Current.State["voice"] = v;
            App.VoiceId = v.Id;
        }
    }
}
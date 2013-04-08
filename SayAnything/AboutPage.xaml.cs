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

namespace Ktos.SayAnything
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void bSupport_Click(object sender, RoutedEventArgs e)
        {
            var n = new Microsoft.Phone.Tasks.EmailComposeTask();
            n.To = "wp7@ktos.info";
            n.Subject = "[" + AppResources.AppName + "] ";
            n.Show();
        }
    }
}
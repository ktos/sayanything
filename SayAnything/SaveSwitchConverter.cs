using System;
using System.Windows.Data;
using Ktos.SayAnything.Resources;

namespace Ktos.SayAnything
{
    class SaveSwitchConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
                return (bool)value ? AppResources.switchSave : AppResources.switchSaveOff;

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

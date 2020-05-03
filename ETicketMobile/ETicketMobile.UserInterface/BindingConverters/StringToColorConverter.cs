using System;
using System.Globalization;
using Xamarin.Forms;

namespace ETicketMobile.UserInterface.BindingConverters
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;

            if (!string.IsNullOrEmpty(str))
                return Color.Red;

            return Color.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
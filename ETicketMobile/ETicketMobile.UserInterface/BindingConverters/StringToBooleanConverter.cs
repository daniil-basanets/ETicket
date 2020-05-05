using System;
using System.Globalization;
using Xamarin.Forms;

namespace ETicketMobile.UserInterface.BindingConverters
{
    public class StringToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;

            return !string.IsNullOrEmpty(str);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
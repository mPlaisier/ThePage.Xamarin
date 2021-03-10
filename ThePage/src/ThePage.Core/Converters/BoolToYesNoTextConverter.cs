using System;
using System.Globalization;
using MvvmCross.Converters;

namespace ThePage.Core
{
    public class BoolToYesNoTextConverter : MvxValueConverter<bool?, string>
    {
        protected override string Convert(bool? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == true)
                return "Yes";

            return value == false
                ? "No"
                : "";
        }
    }
}
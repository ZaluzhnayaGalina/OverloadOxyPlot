using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace OverloadOxyPlot.Converters
{
    internal static class EnumHelper
    {
        public static string Description(Enum eValue)
        {
            var attributes = eValue.GetType()
                .GetField(eValue.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length != 0)
            {
                var descriptionAttribute = attributes[0] as DescriptionAttribute;
                if (descriptionAttribute != null)
                    return descriptionAttribute.Description;
            }
            return "";
        }

        public static IEnumerable<ValueDescription> GetValuesAndDescriptions(Type T)
        {
            if (!T.IsEnum)
                throw new Exception("T must be EnumType");
            return Enum.GetValues(T)
                .Cast<Enum>()
                .Select(e => new ValueDescription { Value = e, Description = Description(e) })
                .ToList();
        }
    }

    [ValueConversion(typeof(Enum), typeof(IEnumerable<ValueDescription>))]
    internal class EnumToCollectionConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? EnumHelper.GetValuesAndDescriptions(value.GetType()) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    internal class EnumToStringConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? EnumHelper.Description((Enum)value) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    internal class ValueDescription
    {
        public object Value { get; set; }
        public string Description { get; set; }
    }
}

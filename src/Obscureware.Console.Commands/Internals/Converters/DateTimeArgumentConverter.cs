namespace Obscureware.Console.Commands.Internals.Converters
{
    using System;
    using System.Globalization;

    [ArgumentConverterTargetType(typeof(DateTime))]
    internal class DateTimeArgumentConverter : ArgumentConverter
    {
        /// <inheritdoc />
        public override object TryConvert(string argumentText, CultureInfo culture)
        {
            return DateTime.Parse(argumentText, culture);
        }
    }
}
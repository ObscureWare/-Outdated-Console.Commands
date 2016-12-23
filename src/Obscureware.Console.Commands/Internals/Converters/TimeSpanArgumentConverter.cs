namespace Obscureware.Console.Commands.Internals.Converters
{
    using System;
    using System.Globalization;

    [ArgumentConverterTargetType(typeof(TimeSpan))]
    internal class TimeSpanArgumentConverter : ArgumentConverter
    {
        /// <inheritdoc />
        public override object TryConvert(string argumentText, CultureInfo culture)
        {
            return TimeSpan.Parse(argumentText, culture);
        }
    }
}
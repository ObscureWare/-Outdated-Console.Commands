namespace Obscureware.Console.Commands.Internals.Converters
{
    using System;
    using System.Globalization;

    [ArgumentConverterTargetType(typeof(Double))]
    internal class DoubleArgumentConverter : ArgumentConverter
    {
        /// <inheritdoc />
        public override object TryConvert(string argumentText, CultureInfo culture)
        {
            return Double.Parse(argumentText, culture);
        }
    }
}
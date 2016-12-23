namespace Obscureware.Console.Commands.Internals.Converters
{
    using System;
    using System.Globalization;

    [ArgumentConverterTargetType(typeof(Decimal))]
    internal class DecimalArgumentConverter : ArgumentConverter
    {
        /// <inheritdoc />
        public override object TryConvert(string argumentText, CultureInfo culture)
        {
            return Decimal.Parse(argumentText, culture);
        }
    }
}
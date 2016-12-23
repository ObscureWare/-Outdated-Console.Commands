namespace Obscureware.Console.Commands
{
    using System.Globalization;

    public interface IValueParsingOptions
    {
        CultureInfo UiCulture { get; }
    }
}
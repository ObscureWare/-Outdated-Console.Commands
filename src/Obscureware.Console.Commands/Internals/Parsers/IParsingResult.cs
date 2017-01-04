namespace ObscureWare.Console.Commands.Internals.Parsers
{
    internal interface IParsingResult
    {
        bool IsFine { get; }

        string Message { get; }
    }
}
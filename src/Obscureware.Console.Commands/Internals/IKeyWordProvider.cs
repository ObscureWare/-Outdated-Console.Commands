namespace Obscureware.Console.Commands.Internals
{
    using System.Collections.Generic;

    /// <summary>
    /// Class that implements this interface provides list of key-words that shall be reserved and forbidden as user commands / options because have (or could have global meaning)
    /// </summary>
    internal interface IKeyWordProvider
    {
        IEnumerable<string> GetCommandKeyWords();

        IEnumerable<string> GetOptionKeyWords();
    }
}
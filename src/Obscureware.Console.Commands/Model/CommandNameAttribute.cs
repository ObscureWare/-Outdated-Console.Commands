namespace Obscureware.Console.Commands.Model
{
    using System;

    /// <summary>
    /// Specifies command name
    /// </summary>
    public class CommandNameAttribute : Attribute
    {
        public string CommandName { get; private set; }

        public CommandNameAttribute(string commandName)
        {
            this.CommandName = commandName;
        }
    }
}
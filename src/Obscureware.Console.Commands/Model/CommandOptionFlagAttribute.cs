namespace Obscureware.Console.Commands.Model
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CommandOptionFlagAttribute : Attribute
    {
        /// <summary>
        /// Gets strings / letters that will enable this flag.
        /// </summary>
        public string[] CommandLiterals { get; private set; }

        public CommandOptionFlagAttribute(params string[] commandLiterals)
        {
            this.CommandLiterals = commandLiterals;
        }
    }
}

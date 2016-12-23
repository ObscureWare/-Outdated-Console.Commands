namespace Obscureware.Console.Commands.Model
{
    using System;

    public class CommandOptionSwitchlessAttribute : Attribute
    {
        public int ArgumentIndex { get; set; }

        public CommandOptionSwitchlessAttribute(int argumentIndex)
        {
            this.ArgumentIndex = argumentIndex;
        }
    }
}
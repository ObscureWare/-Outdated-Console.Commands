namespace Obscureware.Console.Commands.Model
{
    using System;

    public class CommandOptionSwitchAttribute : Attribute
    {
        public Type SwitchBaseType { get; private set; }

        public string[] CommandLiterals { get; private set; }

        public object DefaultValue { get; set; }

        public CommandOptionSwitchAttribute(Type switchBaseType, params string[] commandLiterals)
        {
            if (switchBaseType == null) throw new ArgumentNullException(nameof(switchBaseType));
            if (commandLiterals == null) throw new ArgumentNullException(nameof(commandLiterals));
            if (!switchBaseType.IsEnum) throw new ArgumentException("Value must be an Enumeration type.", nameof(switchBaseType));
            if (commandLiterals.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(commandLiterals));

            this.SwitchBaseType = switchBaseType;
            this.CommandLiterals = commandLiterals;
        }
    }
}
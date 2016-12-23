namespace Obscureware.Console.Commands.Internals
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal class SyntaxInfo
    {
        private readonly PropertyInfo _propertyInfo;

        public SyntaxInfo(PropertyInfo propertyInfo, string optionName)
        {
            this.OptionName = optionName;
            this._propertyInfo = propertyInfo;
        }

        public bool IsMandatory { get; internal set; }

        public string[] Literals { get; internal set; }

        public string OptionName { get; private set; }

        public SyntaxOptionType OptionType { get; internal set; }

        public string[] SwitchValues { get; set; }

        public string Description { get; set; }

        public string GetSyntaxString(ICommandParserOptions options)
        {
            var wrapper = (this.IsMandatory) ? "<{0}{1}>" : "[{0}{1}]";
            return string.Format(wrapper, this.GetInnerSyntaxSelector(options), this.GetInnerSyntaxString(options));
        }

        private object GetInnerSyntaxSelector(ICommandParserOptions options)
        {
            switch (this.OptionType)
            {
                case SyntaxOptionType.Flag:
                    return options.FlagCharacters.First();
                case SyntaxOptionType.Switch:
                    return options.SwitchCharacters.First();
                case SyntaxOptionType.CustomValueSwitch:
                    return options.SwitchCharacters.First();
                case SyntaxOptionType.Switchless:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException(nameof(SyntaxOptionType));
            }
        }

        private string GetInnerSyntaxString(ICommandParserOptions options)
        {
            switch (this.OptionType)
            {
                case SyntaxOptionType.Flag:
                    return this.Literals.First();
                case SyntaxOptionType.Switch:
                    return this.GetSwitchSyntax(options);
                case SyntaxOptionType.CustomValueSwitch:
                    return this.GetCustomSwitchSyntax(options);
                case SyntaxOptionType.Switchless:
                    return $"\"{this.OptionName}\"";
                default:
                    throw new ArgumentOutOfRangeException(nameof(SyntaxOptionType));
            }
        }

        private string GetSwitchSyntax(ICommandParserOptions options)
        {
            string literal = this.Literals.First();
            string value = string.Join("|", this.SwitchValues);

            switch (options.OptionArgumentMode)
            {
                case CommandOptionArgumentMode.Separated:
                    return $"{literal} {value}";
                case CommandOptionArgumentMode.Merged:
                    return $"{literal}{value}";
                case CommandOptionArgumentMode.Joined:
                    return $"{literal}{options.OptionArgumentJoinCharacater}{value}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(CommandOptionArgumentMode));
            }
        }

        private string GetCustomSwitchSyntax(ICommandParserOptions options)
        {
            string literal = this.Literals.First();
            string value = this.OptionName;

            switch (options.OptionArgumentMode)
            {
                case CommandOptionArgumentMode.Separated:
                    return $"{literal} {value}";
                case CommandOptionArgumentMode.Merged:
                    return $"{literal}{value}";
                case CommandOptionArgumentMode.Joined:
                    return $"{literal}{options.OptionArgumentJoinCharacater}{value}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(CommandOptionArgumentMode));
            }
        }
    }

    internal enum SyntaxOptionType
    {
        Flag,

        Switch,

        CustomValueSwitch,

        Switchless
    }
}
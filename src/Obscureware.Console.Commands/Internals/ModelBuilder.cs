namespace Obscureware.Console.Commands.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Converters;
    using Model;

    using Obscureware.Console.Commands.Styles;

    using Parsers;

    internal class ModelBuilder
    {
        private readonly Dictionary<string, FlagPropertyParser> _flagParsers = new Dictionary<string, FlagPropertyParser>();
        private readonly Dictionary<string, BaseSwitchPropertyParser> _switchParsers = new Dictionary<string, BaseSwitchPropertyParser>();
        private readonly List<SwitchlessPropertyParser> _switchlessParsers = new List<SwitchlessPropertyParser>();

        private readonly Type _modelType;
        private readonly ConvertersManager _convertersManager = new ConvertersManager();
        //private readonly List<PropertyInfo> _mandatoryProperties = new List<PropertyInfo>();
        private readonly List<SyntaxInfo> _syntax = new List<SyntaxInfo>();

        /// <summary>
        /// Gets name of the command
        /// </summary>
        public string CommandName { get; private set; }

        /// <summary>
        /// Gets description of the command.
        /// </summary>
        public string CommandDescription { get; private set; }

        /// <summary>
        /// Obtains "precompiled" syntax info
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SyntaxInfo> GetSyntax()
        {
            return this._syntax;
        }

        public ModelBuilder(Type modelType, string commandName)
        {
            this.CommandName = commandName;
            if (modelType == null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            this._modelType = modelType;
            this.ValidateModel(modelType);

            this.ReadCoreHelpInformation();
            this.BuildParsingProperties();
        }

        private void BuildSyntaxInfo()
        {
            // syntax attribute

            // other help attribute

            // check is mandatory
        }

        private void BuildParsingProperties()
        {
            var properties = this._modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.Name.Equals(nameof(CommandModel.RawParameters)))
                {
                    continue;
                }

                var attributes = propertyInfo.GetCustomAttributes(inherit: true);

                var optionNameAtt = attributes.SingleOrDefault(att => att is CommandOptionNameAttribute) as CommandOptionNameAttribute;
                if (optionNameAtt == null)
                {
                    throw new BadImplementationException($"Model property \"{propertyInfo.Name}\" misses mandatory {nameof(CommandOptionNameAttribute)}.", this._modelType);
                }

                var syntaxInfo = new SyntaxInfo(propertyInfo, optionNameAtt.Name);
                this._syntax.Add(syntaxInfo);

                // mandatory attribute
                var mandatoryAtt = attributes.SingleOrDefault(att => att is MandatoryAttribute) as MandatoryAttribute;
                if (mandatoryAtt != null && mandatoryAtt.IsParameterMandatory)
                {
                    //this._mandatoryProperties.Add(propertyInfo);
                    syntaxInfo.IsMandatory = true;
                }

                // description attribute
                var descriptionAtt = attributes.SingleOrDefault(att => att is CommandDescriptionAttribute) as CommandDescriptionAttribute;
                syntaxInfo.Description = descriptionAtt != null ? descriptionAtt.Description : "*** description not available ***";

                // Is Flag
                CommandOptionFlagAttribute optionFlagAtt = attributes.SingleOrDefault(att => att is CommandOptionFlagAttribute) as CommandOptionFlagAttribute;
                if (optionFlagAtt != null)
                {
                    var parser = new FlagPropertyParser(propertyInfo);
                    syntaxInfo.Literals = optionFlagAtt.CommandLiterals;
                    syntaxInfo.OptionType = SyntaxOptionType.Flag;

                    foreach (var literal in optionFlagAtt.CommandLiterals)
                    {
                        var aLiteral = literal.Trim();

                        if (this._flagParsers.ContainsKey(aLiteral))
                        {
                            throw new BadImplementationException($"Flag argument literal \"{literal}\" has been declared more than once in the {this._modelType.FullName}.", this._modelType);
                        }

                        this._flagParsers.Add(aLiteral, parser);
                    }
                }

                // Is Switch
                CommandOptionSwitchAttribute optionSwitchAttribute = attributes.SingleOrDefault(att => att is CommandOptionSwitchAttribute) as CommandOptionSwitchAttribute;
                if (optionSwitchAttribute != null)
                {
                    var parser = this.BuildSwitchParser(propertyInfo, optionSwitchAttribute, attributes);
                    if (parser == null)
                    {
                        throw new BadImplementationException($"Could not find proper SwitchParser for property \"{propertyInfo.Name}\"", this._modelType);
                    }

                    syntaxInfo.Literals = optionSwitchAttribute.CommandLiterals;
                    syntaxInfo.OptionType = SyntaxOptionType.Switch;
                    syntaxInfo.SwitchValues = parser.GetValidValues().ToArray();

                    foreach (var literal in optionSwitchAttribute.CommandLiterals)
                    {
                        var aLiteral = literal.Trim();

                        if (this._switchParsers.ContainsKey(aLiteral))
                        {
                            throw new BadImplementationException($"Flag argument literal \"{literal}\" has been declared more than once in the {this._modelType.FullName}.", this._modelType);
                        }

                        this._switchParsers.Add(aLiteral, parser);
                    }
                }

                // Is Valued Flag - named, free value
                CommandOptionCustomValueSwitchAttribute optionCustomValueAtt = attributes.SingleOrDefault(att => att is CommandOptionCustomValueSwitchAttribute) as CommandOptionCustomValueSwitchAttribute;
                if (optionCustomValueAtt != null)
                {
                    var converter = this._convertersManager.GetConverterFor(propertyInfo.PropertyType);
                    if (converter == null)
                    {
                        throw new BadImplementationException($"Could not find required ArgumentConverter for type \"{propertyInfo.PropertyType.FullName}\" for ValueArgument \"{propertyInfo.Name}\".", this._modelType);
                    }

                    var parser = this.BuildValueOptionParser(propertyInfo, optionCustomValueAtt, attributes, converter);
                    if (parser == null)
                    {
                        throw new BadImplementationException($"Could not find proper SwitchParser for property \"{propertyInfo.Name}\"", this._modelType);
                    }

                    syntaxInfo.Literals = optionCustomValueAtt.CommandLiterals;
                    syntaxInfo.OptionType = SyntaxOptionType.CustomValueSwitch;

                    // value parser is mainly compatible with switch-one
                    foreach (var literal in optionCustomValueAtt.CommandLiterals)
                    {
                        var aLiteral = literal.Trim();

                        if (this._switchParsers.ContainsKey(aLiteral))
                        {
                            throw new BadImplementationException($"Flag argument literal \"{literal}\" has been declared more than once in the {this._modelType.FullName}.", this._modelType);
                        }

                        this._switchParsers.Add(aLiteral, parser);
                    }
                }

                // Is unnamed argument
                CommandOptionSwitchlessAttribute nonPosAtt = attributes.SingleOrDefault(att => att is CommandOptionSwitchlessAttribute) as CommandOptionSwitchlessAttribute;
                if (nonPosAtt != null)
                {
                    syntaxInfo.OptionType = SyntaxOptionType.Switchless;

                    var converter = this._convertersManager.GetConverterFor(propertyInfo.PropertyType);
                    if (converter == null)
                    {
                        throw new BadImplementationException($"Could not find required ArgumentConverter for type \"{propertyInfo.PropertyType.FullName}\" for unnamed Argument at index [{nonPosAtt.ArgumentIndex}].", this._modelType);
                    }

                    this._switchlessParsers.Add(new SwitchlessPropertyParser(nonPosAtt.ArgumentIndex, propertyInfo, converter));
                }
            }
        }

        private BaseSwitchPropertyParser BuildValueOptionParser(PropertyInfo propertyInfo, CommandOptionCustomValueSwitchAttribute optionCustomValueAtt, object[] attributes, ArgumentConverter converter)
        {
            return new CustomValueSwitchParser(propertyInfo, optionCustomValueAtt, converter);
        }

        private BaseSwitchPropertyParser BuildSwitchParser(PropertyInfo propertyInfo, CommandOptionSwitchAttribute optionSwitchAttribute, object[] otherAttributes)
        {
            if (optionSwitchAttribute.SwitchBaseType.IsEnum)
            {
                return new EnumSwitchParser(propertyInfo, optionSwitchAttribute);
            }

            return null;
        }

        private void ReadCoreHelpInformation()
        {
            CommandDescriptionAttribute att = this._modelType.GetCustomAttribute<CommandDescriptionAttribute>();
            this.CommandDescription = att?.Description ?? "* Description not available *";
        }

        private void ValidateModel(Type modelType)
        {
            var publicCtor = modelType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.Any, new Type[0], null);
            if (publicCtor == null)
            {
                throw new ArgumentException($"{modelType.FullName} does not expose public, parameterless constructor.");
            }



            // TODO: validate model type deeper - conflicting switches, missing attributes etc.
        }

        public CommandModel BuildModel(IEnumerable<string> arguments, ICommandParserOptions options)
        {
            var model = Activator.CreateInstance(this._modelType) as CommandModel;
            string[] args = arguments.ToArray();
            int argIndex = 0;
            int unnamedIndex = 0;
            string[] flagSwitchPrefixes = options.SwitchCharacters.Concat(options.FlagCharacters).Distinct().ToArray();

            while (argIndex < args.Length)
            {
                string arg = args[argIndex].Trim();

                // need to skip unnamed parameters there are just exactly like one of prefixes (i.e. - single slash or backslash)
                if (flagSwitchPrefixes.All(p => p != arg) && flagSwitchPrefixes.Any(p => arg.StartsWith(p)))
                {
                    var propertyParser = this.FindProperty(options, arg);
                    if (propertyParser == null)
                    {
                        // TODO: write error about invalid switch
                        return null;
                    }

                    propertyParser.Apply(options, model, args, ref argIndex);
                }
                else
                {
                    var propertyParser = this._switchlessParsers.SingleOrDefault(p => p.ArgumentIndex == unnamedIndex++);
                    if (propertyParser == null)
                    {
                        // TODO: write error about too many switch-less arguments
                        return null;
                    }

                    propertyParser.Apply(options, model, args, ref argIndex);
                }

                argIndex++;
            }

            // TODO: validate that all mandatory options were provided and switch-less arguments too
            // TODO: validate mixed / non mixed mode for switch-less parameters

            return model;
        }

        private BasePropertyParser FindProperty(ICommandParserOptions options, string argSyntax)
        {
            // Flags
            if (!options.AllowFlagsAsOneArgument)
            {
                foreach (var flagPrefix in options.FlagCharacters)
                {
                    string cleanFlag = argSyntax.CutLeftFirst(flagPrefix);

                    FlagPropertyParser parser;
                    if (this._flagParsers.TryGetValue(cleanFlag, out parser))
                    {
                        return parser;
                    }
                }
            }
            else
            {
                if (this._flagParsers.Keys.Any(flag => flag.Length > 1))
                {
                    throw new BadImplementationException($"Cannot use {nameof(options.AllowFlagsAsOneArgument)} mode with flags longer than one character.", this._modelType);
                }

                // TODO: implement, with above exception this will be easier...

                // TODO: add multi-flag parser here
                throw new NotImplementedException();
            }


            // Switches
            foreach (var switchPrefix in options.SwitchCharacters)
            {
                string cleanFlag = argSyntax.CutLeftFirst(switchPrefix);
                BaseSwitchPropertyParser parser = this._switchParsers.FirstOrDefault(p => p.Key.Equals(cleanFlag)).Value;
                return parser;
            }

            return null;
        }
    }
}
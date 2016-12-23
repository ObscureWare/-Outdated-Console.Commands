namespace Obscureware.Console.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Internals;

    using Obscureware.Console.Commands.Styles;

    using ObscureWare.Console;

    /// <summary>
    /// Fluent-syntax based CommandEngine builder
    /// </summary>
    public class CommandEngineBuilder
    {
        private CommandParserOptions _options;

        private CommandEngineStyles _styles;

        private readonly List<Type> _commands;

        private CommandEngineBuilder()
        {
            this._commands = new List<Type>();
            this._styles = CommandEngineStyles.DefaultStyles;
            // TODO: this._options = CommandParserOptions.Default;

            this.AddStandardCommands();
        }

        private void AddStandardCommands()
        {
            // ...
        }

        public CommandEngineBuilder UsingStyles(CommandEngineStyles styles)
        {
            if (styles == null)
            {
                throw new ArgumentNullException(nameof(styles));
            }

            this._styles = styles;

            return this;
        }

        public CommandEngineBuilder UsingOptions(CommandParserOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this._options = options;

            return this;
        }

        public CommandEngineBuilder WithCommandsFromAssembly(Assembly asm)
        {
            if (asm == null)
            {
                throw new ArgumentNullException(nameof(asm));
            }

            var commandTypes = asm.GetTypes().Where(t => typeof(IConsoleCommand).IsAssignableFrom(t));
            this._commands.AddRange(commandTypes);

            return this;
        }

        public CommandEngineBuilder WithCommands(params Type[] commandTypes)
        {
            if (commandTypes == null)
            {
                throw new ArgumentNullException(nameof(commandTypes));
            }

            this._commands.AddRange(commandTypes);


            return this;
        }

        /// <summary>
        /// Finally construct Engine instance when all items are ready
        /// </summary>
        /// <param name="console"></param>
        /// <returns></returns>
        public ICommandEngine ConstructForConsole(IConsole console)
        {
            if (console == null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (this._options == null)
            {
                throw new InvalidOperationException("Could not construct engine without providing Options object.");
            }

            if (this._styles == null)
            {
                throw new InvalidOperationException("Could not construct engine without providing Styles object.");
            }

            var printHelper = new HelpPrinter(this._options, this._styles.HelpStyles, console);
            var commandManager = new CommandManager(this._commands.Distinct().ToArray());

            var keywords = printHelper.GetCommandKeyWords();
            // TODO: here eventually construct other subsystems with keywords, and then merge them. Also validate different keywords from different subsystems...

            ValidateKeywords(keywords, commandManager);
            // TODO: switches too?

            return new CommandEngine(commandManager, this._options, this._styles, printHelper, console);
        }

        /// <summary>
        /// Verifies if any command name is in conflict with built-in commands (mostly help-related)
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="commandManager"></param>
        private static void ValidateKeywords(IEnumerable<string> keywords, CommandManager commandManager)
        {
            var conflictKeywords = keywords.Select(commandManager.FindCommand).Where(cmd => cmd != null).ToArray();
            if (conflictKeywords.Any())
            {
                throw new BadImplementationException($"Following commands are in conflict with keywords: {string.Join(", ", conflictKeywords.GetType().Name)}", typeof(CommandManager));
            }
        }

        /// <summary>
        /// Start fluent syntax
        /// </summary>
        /// <returns></returns>
        public static CommandEngineBuilder Build()
        {
            return new CommandEngineBuilder();
        }
    }
}

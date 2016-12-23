namespace Obscureware.Console.Commands.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class CommandManager
    {
        private readonly Dictionary<string, CommandInfo> _commands;
        private StringComparison _selectedComparison = StringComparison.InvariantCulture;

        private CommandCaseSensitivenes _commandsSensitivenes;

        /// <summary>
        /// Gets or sets whether command names are case sensitive or not.
        /// </summary>
        public CommandCaseSensitivenes CommandsSensitivenes
        {
            get
            {
                return this._commandsSensitivenes;
            }
            set
            {
                this._commandsSensitivenes = value;
                this._selectedComparison = (value == CommandCaseSensitivenes.Sensitive) ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
            }
        }

        public CommandManager(Type[] commands)
        {
            this._commands = this.CheckCommands(commands);
        }

        public CommandInfo FindCommand(string cmdName)
        {

            return this._commands
                .SingleOrDefault(pair =>
                    pair.Key.Equals(cmdName, this._selectedComparison))
                .Value;
        }

        public IEnumerable<CommandInfo> GetAll()
        {
            return this._commands.Values.ToArray();
        }

        private Dictionary<string, CommandInfo> CheckCommands(Type[] commands)
        {
            Dictionary<string, CommandInfo> result = new Dictionary<string, CommandInfo>();

            ConsoleCommandBuilder builder = new ConsoleCommandBuilder();
            foreach (var commandType in commands)
            {
                Tuple<ModelBuilder, IConsoleCommand> cmd = builder.ValidateAndBuildCommand(commandType);
                result.Add(cmd.Item1.CommandName, new CommandInfo(cmd.Item2, cmd.Item1));
            }

            return result;
        }


    }
}
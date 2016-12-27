// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandManager.cs" company="Obscureware Solutions">
// MIT License
//
// Copyright(c) 2016 Sebastian Gruchacz
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>
// <summary>
//   Defines the CommandManager internal class responsible for holding collection of known commands.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
                Tuple<CommandModelBuilder, IConsoleCommand> cmd = builder.ValidateAndBuildCommand(commandType);
                result.Add(cmd.Item1.CommandName, new CommandInfo(cmd.Item2, cmd.Item1));
            }

            return result;
        }
    }
}
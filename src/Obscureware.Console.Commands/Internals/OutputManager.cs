// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OutputManager.cs" company="Obscureware Solutions">
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
//   Defines OutputManager class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Obscureware.Console.Commands.Internals
{
    using System.Collections.Generic;
    using System.Globalization;

    using Obscureware.Console.Commands.Styles;

    using ObscureWare.Console;
    using Operations.Styles;
    using Operations.TablePrinters;
    using Operations.Tables;

    public class OutputManager : ICommandOutput
    {
        // TODO: add everywhere console / buffer boundaries checking

        private readonly IConsole _consoleInstance;
        private readonly CommandEngineStyles _engineStyles;

        private readonly CultureInfo _uiCulture;

        /// <summary>
        /// Default table printer for all commands
        /// </summary>
        private readonly DataTablePrinter _tablePrinter;

        public OutputManager(IConsole consoleInstance, CommandEngineStyles engineStyles, CultureInfo uiCulture)
        {
            this._consoleInstance = consoleInstance;
            this._engineStyles = engineStyles;
            this._uiCulture = uiCulture;

            this._tablePrinter = new SimpleTablePrinter(
                consoleInstance, 
                new SimpleTableStyle(engineStyles.HelpStyles.HelpHeader, engineStyles.OddRowColor)
                {
                    EvenRowColor = engineStyles.EvenRowColor,
                    AtomicPrinting = true,
                    ShowHeader = true,
                    OverflowBehaviour = TableOverflowContentBehavior.Wrap
                });
        }

        public void PrintResultLines(IEnumerable<string> results)
        {
            // TODO: improve!

            foreach (var result in results)
            {
                this._consoleInstance.WriteLine(result);
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            this._consoleInstance.Clear();
        }

        /// <inheritdoc />
        public void PrintSimpleTable<T>(DataTable<T> filesTable)
        {
            this._tablePrinter.PrintTable(filesTable);
        }

        public void PrintWarning(string message)
        {
            this._consoleInstance.WriteLine(this._engineStyles.Error, message);
        }

        public CultureInfo UiCulture
        {
            get
            {
                return this._uiCulture;
            }
        }

        public IOutputLineManager ReserveNewLine()
        {
            // TODO: returned line manager must be managed in case it needs to be repositioned if buffer boundaries change...

            throw new System.NotImplementedException();
        }
    }
}
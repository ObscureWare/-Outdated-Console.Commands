namespace Obscureware.Console.Commands.Internals
{
    using System.Collections.Generic;
    using System.Globalization;

    using Obscureware.Console.Commands.Styles;

    using ObscureWare.Console;
    using Operations.Tables;

    public class OutputManager : ICommandOutput
    {
        private readonly IConsole _consoleInstance;
        private readonly CommandEngineStyles _engineStyles;

        private readonly CultureInfo _uiCulture;

        private readonly DataTablePrinter _tablePrinter;

        public OutputManager(IConsole consoleInstance, CommandEngineStyles engineStyles, CultureInfo uiCulture)
        {
            this._consoleInstance = consoleInstance;
            this._engineStyles = engineStyles;
            this._uiCulture = uiCulture;
            this._tablePrinter = new DataTablePrinter(consoleInstance);
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
            this._tablePrinter.PrintAsSimpleTable(filesTable, this._engineStyles.HelpStyles.HelpHeader, this._engineStyles.HelpStyles.HelpDefinition);
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
    }
}
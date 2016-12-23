namespace ConsoleTests
{
    using System;
    using System.Drawing;
    using Obscureware.Console.Operations;
    using ObscureWare.Console;

    internal static class Program
    {
        private static void Main(string[] args)
        {
            ConsoleManager helper = new ConsoleManager();
            //helper.ReplaceConsoleColor(ConsoleColor.DarkCyan, Color.Salmon);
            helper.ReplaceConsoleColors(
                new Tuple<ConsoleColor, Color>(ConsoleColor.DarkCyan, Color.Chocolate),
                new Tuple<ConsoleColor, Color>(ConsoleColor.Blue, Color.DodgerBlue),
                new Tuple<ConsoleColor, Color>(ConsoleColor.Yellow, Color.Gold),
                new Tuple<ConsoleColor, Color>(ConsoleColor.DarkBlue, Color.MidnightBlue));

            IConsole console = new SystemConsole(helper, isFullScreen: false);
            ConsoleOperations ops = new ConsoleOperations(console);

            TestCommands test = new TestCommands(console);

            console.ReadLine();
        }
    }
}
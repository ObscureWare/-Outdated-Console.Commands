namespace ConsoleApplication1.Commands
{
    using System;
    using System.IO;

    using Obscureware.Console.Commands;
    using Obscureware.Console.Commands.Model;

    [CommandModel(typeof(ChangeDirUpCommandModel))]
    public class ChangeDirUpCommand : IConsoleCommand
    {
        /// <inheritdoc />
        public void Execute(object contextObject, ICommandOutput output, object runtimeModel)
        {
            DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
            if (dir.FullName != dir.Root.FullName)
            {
                Environment.CurrentDirectory = dir.Parent.FullName;
                //(contextObject as ConsoleContext).
            }
        }
    }
}
namespace ConsoleApplication1.Commands
{
    using ConsoleTests;
    using Obscureware.Console.Commands;
    using Obscureware.Console.Commands.Model;

    [CommandModel(typeof(ExitCommandModel))]
    public class ExitCommand : IConsoleCommand
    {
        /// <inheritdoc />
        public void Execute(object contextObject, ICommandOutput output, object runtimeModel)
        {
            output.PrintWarning("Temrinating application...");
            (contextObject as ConsoleContext).ShallFinishInteracativeSession = true; // let it throw on null
        }
    }
}
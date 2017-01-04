namespace ConsoleApplication1.Commands
{
    using ObscureWare.Console.Commands;
    using ObscureWare.Console.Commands.Model;

    [CommandModel(typeof(ClsCommandModel))]
    public class ClsCommand : IConsoleCommand
    {
        /// <inheritdoc />
        public void Execute(object contextObject, ICommandOutput output, object runtimeModel)
        {
            output.Clear();
        }
    }
}
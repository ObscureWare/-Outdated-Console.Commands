namespace ConsoleApplication1.Commands
{
    using Obscureware.Console.Commands;
    using Obscureware.Console.Commands.Model;

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
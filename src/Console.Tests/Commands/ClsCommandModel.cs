namespace ConsoleApplication1.Commands
{
    using Obscureware.Console.Commands.Model;

    [CommandDescriptorFor(typeof(ClsCommand))]
    [CommandName("cls")]
    [CommandDescription(@"Clears / Resets screen of the console.")]
    public class ClsCommandModel : CommandModel
    {
    }
}
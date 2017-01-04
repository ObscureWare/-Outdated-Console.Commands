namespace ConsoleApplication1.Commands
{
    using ObscureWare.Console.Commands.Model;

    [CommandModelFor(typeof(ClsCommand))]
    [CommandName("cls")]
    [CommandDescription(@"Clears / Resets screen of the console.")]
    public class ClsCommandModel : CommandModel
    {
    }
}
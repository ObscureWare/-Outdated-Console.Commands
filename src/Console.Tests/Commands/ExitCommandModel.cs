namespace ConsoleApplication1.Commands
{
    using ObscureWare.Console.Commands.Model;

    [CommandModelFor(typeof(ExitCommand))]
    [CommandName("exit")]
    [CommandDescription(@"Immediately terminates the application.")]
    public class ExitCommandModel : CommandModel
    {
    }
}
namespace ConsoleApplication1.Commands
{
    using Obscureware.Console.Commands.Model;

    [CommandDescriptorFor(typeof(ExitCommand))]
    [CommandName("exit")]
    [CommandDescription(@"Immediately terminates the application.")]
    public class ExitCommandModel : CommandModel
    {
    }
}
namespace ConsoleApplication1.Commands
{
    using Obscureware.Console.Commands.Model;

    [CommandDescriptorFor(typeof(ChangeDirUpCommand))]
    [CommandName("cd..")]
    [CommandDescription(@"Moves Current Directory one level up.")]
    public class ChangeDirUpCommandModel : CommandModel
    {

    }
}
namespace ConsoleApplication1.Commands
{
    using Obscureware.Console.Commands;
    using Obscureware.Console.Commands.Model;

    /// <summary>
    /// The change dir command model.
    /// </summary>
    [CommandDescriptorFor(typeof(ChangeDirCommand))]
    [CommandName("cd")]
    [CommandDescription(@"Moves Current Directory specific way.")]
    public class ChangeDirCommandModel : CommandModel
    {
        [CommandOptionName(@"target")]
        [Mandatory(false)]
        [CommandOptionSwitchless(0)]
        [CommandDescription("Specifies how directory shall be changed. Nothing or '.' will remain in current folder. '..' Will go one level up. '\\' will immediately jump to the root. Anything else means subdirectory or exact location - if has rooted format..")]
        // TODO: add multi-line descriptions / multiple attributes
        // TODO: perhaps differentiate command and option descriptions
        public string Target { get; set; }
    }
}
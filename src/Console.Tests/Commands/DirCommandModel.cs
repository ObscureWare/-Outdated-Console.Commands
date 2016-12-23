namespace ConsoleApplication1.Commands
{
    using Obscureware.Console.Commands;
    using Obscureware.Console.Commands.Model;

    [CommandDescriptorFor(typeof(DirCommand))]
    [CommandName("dir")]
    [CommandDescription(@"Lists files withing current folder or repository state, depending on selected options.")]
    public class DirCommandModel : CommandModel
    {
        [CommandOptionName(@"includeFolders")]
        [Mandatory(false)]
        [CommandOptionFlag("d", "D")]
        // TODO: Name Attribute? Or just use activation letters for help/syntax display?
        [CommandDescription("When set, specifies whether directories shall be listed too.")]
        public bool IncludeFolders { get; set; }

        [CommandOptionName(@"mode")]
        [Mandatory]
        [CommandOptionSwitch(typeof(DirectoryListMode), "m", DefaultValue = DirectoryListMode.CurrentDir)]
        [CommandDescription("Specifies which predefined directory location shall be listed. Defaults to 'CurrentDir'")]
        // TODO: list help for switches. Get from enumeration itself? Allow coloring syntax? Somehow...
        // TODO: more switch types?
        // TODO: runtime support switch auto-complete. Sourced through ModelBuilder & Parser
        public DirectoryListMode Mode { get; set; }

        [CommandOptionName(@"filter")]
        [Mandatory(false)]
        [CommandOptionCustomValueSwitch("f", "F")]
        [CommandDescription("Specifies filter for enumerated files. Does not apply to folders listing.")]
        // TODO: runtime support for some values / unnamed values auto-completion? sourced through command itself...
        public string Filter { get; set; }
    }

    // TODO: add sorting
}
namespace Obscureware.Console.Commands.Styles
{
    using ObscureWare.Console;

    public interface IHelpStyles
    {
        ICommonStyles CommonStyles { get; }

        ConsoleFontColor HelpSyntax { get; set; }

        ConsoleFontColor HelpDescription { get; set; }

        ConsoleFontColor HelpDefinition { get; set; }

        ConsoleFontColor HelpBody { get; set; }

        ConsoleFontColor HelpHeader { get; set; }
    }
}
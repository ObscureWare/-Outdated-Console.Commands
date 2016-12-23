namespace Obscureware.Console.Commands.Styles
{
    using ObscureWare.Console;

    public class HelpStyles : IHelpStyles
    {
        public HelpStyles(ICommonStyles commonStyles)
        {
            this.CommonStyles = commonStyles;
        }

        public ICommonStyles CommonStyles { get; private set; }

        public ConsoleFontColor HelpSyntax { get; set; }

        public ConsoleFontColor HelpDescription { get; set; }

        public ConsoleFontColor HelpDefinition { get; set; }

        public ConsoleFontColor HelpBody { get; set; }

        public ConsoleFontColor HelpHeader { get; set; }
    }
}
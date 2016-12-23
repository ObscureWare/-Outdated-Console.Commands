namespace Obscureware.Console.Commands.Styles
{
    using System.Drawing;

    using ObscureWare.Console;

    /// <summary>
    /// Contains various color scheme settings used by CommandEngine
    /// </summary>
    public class CommandEngineStyles : ICommonStyles
    {
        public static CommandEngineStyles DefaultStyles
        {
            get
            {
                var engineStyles = new CommandEngineStyles
                {
                    Default = new ConsoleFontColor(Color.DarkGray, Color.Black),
                    Error = new ConsoleFontColor(Color.DarkRed, Color.Black),
                    Warning = new ConsoleFontColor(Color.Orange, Color.Black),
                    

                    Prompt = new ConsoleFontColor(Color.Yellow, Color.DarkBlue)
                };

                engineStyles.HelpStyles = new HelpStyles(engineStyles)
                                           {
                                               HelpHeader = new ConsoleFontColor(Color.Black, Color.DarkGray),
                                               HelpBody = new ConsoleFontColor(Color.LightGray, Color.Black),
                                               HelpDefinition = new ConsoleFontColor(Color.White, Color.Black),
                                               HelpDescription = new ConsoleFontColor(Color.LightGray, Color.Black),
                                               HelpSyntax = new ConsoleFontColor(Color.White, Color.DarkBlue)
                                           };

                return engineStyles;
            }
        }

        // TODO: split this class into specialized classes / interfaces (help styles, messaging styles, table styles etc.)

        public ConsoleFontColor Warning { get; set; }

        public ConsoleFontColor Error { get; set; }

        public ConsoleFontColor Default { get; set; }

        /// <summary>
        /// Style for displaying command prompt
        /// </summary>
        public ConsoleFontColor Prompt { get;  set; }

        /// <summary>
        /// Styling Help provider
        /// </summary>
        public IHelpStyles HelpStyles { get; set; }
    }
}
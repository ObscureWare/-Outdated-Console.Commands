namespace Obscureware.Console.Commands.Internals
{
    internal class CommandInfo
    {
        public IConsoleCommand Command { get; private set; }

        public ModelBuilder ModelBuilder { get; private set; }


        public CommandInfo(IConsoleCommand commandInstance, ModelBuilder modelBuilder)
        {
            this.Command = commandInstance;
            this.ModelBuilder = modelBuilder;
        }
    }
}
namespace ObscureWare.Console.Commands.Model
{
    public abstract class CommandModel
    {
        public string[] RawParameters { get; internal set; }
    }
}
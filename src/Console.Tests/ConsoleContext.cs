namespace ConsoleTests
{
    using System;
    using Obscureware.Console.Commands;

    public class ConsoleContext : ICommandEngineContext
    {
        public bool ShallFinishInteracativeSession { get; set; }

        public string GetCurrentPrompt()
        {
            string dir = Environment.CurrentDirectory;
            if (!dir.EndsWith("\\"))
            {
                dir += "\\";
            }
            return dir;
        }
    }
}
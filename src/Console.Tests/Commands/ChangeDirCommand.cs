namespace ConsoleApplication1.Commands
{
    using System;
    using System.IO;

    using Obscureware.Console.Commands;
    using Obscureware.Console.Commands.Model;

    [CommandModel(typeof(ChangeDirCommandModel))]
    public class ChangeDirCommand : IConsoleCommand
    {
        /// <inheritdoc />
        public void Execute(object contextObject, ICommandOutput output, object runtimeModel)
        {
            ChangeDirCommandModel model = runtimeModel as ChangeDirCommandModel;
            switch (model?.Target.Trim())
            {
                case "":
                case ".":
                {
                    break; // remain;
                }
                case "..":
                {
                    DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
                    if (dir.FullName != dir.Root.FullName)
                    {
                        Environment.CurrentDirectory = dir.Parent?.FullName ?? dir.FullName; // stay
                    }
                    break;
                }
                case "\\":
                {
                    DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
                    Environment.CurrentDirectory = dir.Root.FullName;
                    break;
                }
                default:
                {
                    string path = model.Target.Trim();
                    if (!Path.IsPathRooted(path))
                    {
                        Environment.CurrentDirectory = new DirectoryInfo(path).FullName;
                    }
                    else
                    {
                        path = Path.Combine(Environment.CurrentDirectory, path);
                        Environment.CurrentDirectory = new DirectoryInfo(path).FullName;
                    }
                    break;
                }

                //(contextObject as ConsoleContext).
            }
        }
    }
}
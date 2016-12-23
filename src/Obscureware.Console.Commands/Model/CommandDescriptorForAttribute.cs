namespace Obscureware.Console.Commands.Model
{
    using System;

    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandDescriptorForAttribute : Attribute
    {
        public Type ModelledCommandType { get; private set; }

        public CommandDescriptorForAttribute(Type modelledCommandType)
        {
            this.ModelledCommandType = modelledCommandType;
        }
    }

    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandModelAttribute : Attribute
    {
        public Type ModelType { get; private set; }

        public CommandModelAttribute(Type modelType)
        {
            this.ModelType = modelType;
        }
    }
}
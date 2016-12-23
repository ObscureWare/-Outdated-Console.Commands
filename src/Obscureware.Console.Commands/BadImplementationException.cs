namespace Obscureware.Console.Commands
{
    using System;

    [Serializable]
    internal class BadImplementationException : Exception
    {
        /// <summary>
        /// The type that has been improperly implemented or configured.
        /// </summary>
        public Type TargetType { get; private set; }

        public BadImplementationException(string message, Type targetType) : base (message)
        {
            this.TargetType = targetType;
        }
    }
}
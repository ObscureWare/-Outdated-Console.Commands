namespace Obscureware.Console.Commands
{
    public enum CommandOptionArgumentMode
    {
        /// <summary>
        /// Switch and argument are separated with white-chars
        /// </summary>
        Separated,

        /// <summary>
        /// Switch and argument are concatenated together.
        /// </summary>
        Merged,

        /// <summary>
        /// Switch and argument are being joined by dedicated character, like : =
        /// </summary>
        Joined
    }
}
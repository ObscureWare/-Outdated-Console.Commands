namespace Obscureware.Console.Commands.Internals
{
    using System;

    /// <summary>
    /// Case-insensitive, invariant comparer
    /// </summary>
    internal class InsensitiveStringComparer : StringComparer
    {
        /// <inheritdoc />
        public override int Compare(string x, string y)
        {
            return string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <inheritdoc />
        public override bool Equals(string x, string y)
        {
            return string.Equals(x, y, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <inheritdoc />
        public override int GetHashCode(string obj)
        {
            return obj.ToUpper().GetHashCode();
        }
    }
}
namespace Obscureware.Console.Commands.Internals
{
    using System;

    /// <summary>
    /// Case-sensitive, invariant comparer
    /// </summary>
    internal class SensitiveStringComparer : StringComparer
    {
        /// <inheritdoc />
        public override int Compare(string x, string y)
        {
            return string.Compare(x, y, StringComparison.InvariantCulture);
        }

        /// <inheritdoc />
        public override bool Equals(string x, string y)
        {
            return string.Equals(x, y, StringComparison.InvariantCulture);
        }

        /// <inheritdoc />
        public override int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
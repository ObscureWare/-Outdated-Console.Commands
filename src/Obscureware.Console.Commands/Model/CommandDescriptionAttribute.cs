namespace Obscureware.Console.Commands.Model
{
    using System;

    /// <summary>Specifies a description for a command model or any of fragment of that model.</summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class CommandDescriptionAttribute : Attribute
    {
        /// <summary>Specifies the default value for the <see cref="CommandDescriptionAttribute"/> which is an empty string (""). This static field is read-only.</summary>
        public static readonly CommandDescriptionAttribute Default = new CommandDescriptionAttribute(string.Empty);

        /// <summary>Initializes a new instance of the <see cref="CommandDescriptionAttribute"/> class with no parameters.</summary>
        public CommandDescriptionAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDescriptionAttribute"/> class with a description.
        /// </summary>
        /// <param name="description">The description text.</param>
        public CommandDescriptionAttribute(string description)
        {
            this.Description = description;
        }

        /// <summary>
        /// Gets the description stored in this attribute.
        /// </summary>
        /// <returns>The description stored in this attribute.</returns>
        public string Description { get; }

        /// <summary>
        /// Returns whether the value of the given object is equal to the current <see cref="CommandDescriptionAttribute"/>
        /// </summary>
        /// <param name="obj">he object to test the value equality of.</param>
        /// <returns>true if the value of the given object is equal to that of the current; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (Object.Equals(this, obj))
            {
                return true;
            }

            CommandDescriptionAttribute cast = obj as CommandDescriptionAttribute;
            if (cast == null || !cast.Description.Equals(this.Description))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return this.Description?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// Returns a value indicating whether this is the default <see cref="CommandDescriptionAttribute"/> instance.
        /// </summary>
        /// <returns>true, if this is the default <see cref="CommandDescriptionAttribute"/> instance; otherwise, false.</returns>
        public override bool IsDefaultAttribute()
        {
            return ReferenceEquals(this, Default);
        }
    }
}
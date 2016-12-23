namespace Obscureware.Console.Commands.Model
{
    using System;

    public class MandatoryAttribute : Attribute
    {
        public bool IsParameterMandatory { get; private set; }

        public MandatoryAttribute(bool isParameterMandatory = true)
        {
            this.IsParameterMandatory = isParameterMandatory;
        }
    }
}
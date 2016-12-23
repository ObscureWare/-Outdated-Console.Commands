namespace Obscureware.Console.Commands.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Converters;

    internal class ConvertersManager
    {
        private readonly Dictionary<Type, ArgumentConverter> _knownConverters = new Dictionary<Type, ArgumentConverter>();

        public ConvertersManager() // TODO: add more Assemblies to scan for custom converters
        {
            Assembly asm = this.GetType().Assembly;
            this.LoadConvertersFromAssembly(asm);
        }

        private void LoadConvertersFromAssembly(Assembly asm)
        {
            // TODO: what to do with conflicts? Now exception... Replace? Then what hierarchy... Let Attributes decide? CustomArgumentConverterAttribute? Sounds fine. Not have to scan them.

            var converterTypes = asm.GetTypes().Where(t => typeof(ArgumentConverter).IsAssignableFrom(t));
            foreach (var converterType in converterTypes)
            {
                if (converterType.IsAbstract)
                {
                    continue; // Do not need these
                }

                var att = converterType.GetCustomAttribute<ArgumentConverterTargetTypeAttribute>();
                if (att == null)
                {
                    throw new BadImplementationException($"[{converterType.FullName}] misses required {nameof(ArgumentConverterTargetTypeAttribute)}.", converterType);
                }

                var publicCtor = converterType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null,
                    new Type[0], null);
                if (publicCtor == null)
                {
                    throw new BadImplementationException($"[{converterType.FullName}] does not expose public, parameterless constructor.", converterType);
                }

                var converterInstance = (ArgumentConverter)Activator.CreateInstance(converterType);
                this._knownConverters.Add(att.TargetType, converterInstance);
            }
        }

        /// <summary>
        /// Tries to find Argument Converter dedicated to converting string into given type.
        /// </summary>
        /// <param name="conversionTarget"></param>
        /// <returns></returns>
        public ArgumentConverter GetConverterFor(Type conversionTarget)
        {
            ArgumentConverter converter;
            if (this._knownConverters.TryGetValue(conversionTarget, out converter))
            {
                return converter;
            }

            return null;
        }
    }
}

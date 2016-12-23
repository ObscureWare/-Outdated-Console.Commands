// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Obscureware Solutions">
// MIT License
//
// Copyright(c) 2016 Sebastian Gruchacz
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>
// <summary>
//   Defines various public extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Obscureware.Console.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Extensions
    {
        /// <summary>
        /// Returns base text without first matched prefix. Matching on the left-most end of the text.
        /// </summary>
        /// <param name="baseText"></param>
        /// <param name="prefixes"></param>
        /// <returns></returns>
        public static string CutLeftFirst(this string baseText, params string[] prefixes)
        {
            foreach (string prefix in prefixes)
            {
                if (baseText.StartsWith(prefix))
                {
                    return baseText.Substring(prefix.Length);
                }
            }

            return baseText;
        }

        private static readonly Random Rnd = new Random();

        /// <summary>
        /// Picks one element from collection randomly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Must not be empty!</param>
        /// <returns></returns>
        public static T SelectRandom<T>(this ICollection<T> collection)
        {
            if (collection.Count > 1)
            {
                int index = Rnd.Next(0, collection.Count);
                return collection.Skip(index).First();
            }
            else
            {
                return collection.First(); // or crash
            }
        }
    }
}

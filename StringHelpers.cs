using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    public static class StringHelpers
    {
        /// <summary>
        /// Return first string (from parameters) that is not null and not empty
        /// Example usage: return FirstNonEmpty(CustomColor, Teacher?.Color, "#6F3948");
        /// </summary>
        public static string FirstNonEmpty(params string[] strings)
        {
            return strings.FirstOrDefault(s => !string.IsNullOrEmpty(s));
        }
    }
}

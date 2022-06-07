using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace linqPlusPlus
{
    public static class GeneralExtensions
    {
        public static bool HasContent(this string str) => !(string.IsNullOrEmpty(str) && string.IsNullOrWhiteSpace(str));
        /// <summary>
        /// expression friendly boolean null propagation
        /// </summary>
        public static bool IfNull(this object obj, Func<bool> nullPart, Func<bool> notNullPart) => obj is null ? nullPart.Invoke() : notNullPart.Invoke();

    }
}

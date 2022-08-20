using System;
using System.Collections.Generic;
using System.Linq;
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

            public static TDestination Map<TDestination>(this object source)
            {
                try
                {
                    //return AutoMapper.Mapper.Map<TDestination>(source);
                    var json = JsonExtensions.ToJson(source);
                    return JsonExtensions.DeserializeJson<TDestination>(json);
                }
                catch (Exception)
                {
                    return default(TDestination);
                }
            }
    }

}

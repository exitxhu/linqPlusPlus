using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace linqPlusPlus
{
    public static class GeneralExtensions
    {
        public static bool HasContent(this string str) => !(string.IsNullOrEmpty(str) && string.IsNullOrWhiteSpace(str));
        public static bool IsEmpty(this string str) => !str.HasContent();
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
        public static bool In(this object obj, params object[] source)
        {
            return source.Contains(obj);
        }
        public static bool NotIn(this object obj, params object[] source)
        {
            return !source.Contains(obj);
        }
        public static bool In(this object[] obj, params object[] source)
        {
            return obj.Any(a => source.Contains(a));
        }
        public static bool NotIn(this object[] obj, params object[] source)
        {
            return !obj.Any(a => !source.Contains(a));
        }
        public static bool IsSameSite(this Uri first, Uri second)
        {
            var fSegs = first.Host.Split(".");
            if (fSegs.Length < 2)
                return false;
            var sSegs = second.Host.Split(".");
            if (sSegs.Length < 2)
                return false;
            var fSite = $"{fSegs[^2]}.{fSegs[^1]}";
            var sSite = $"{sSegs[^2]}.{sSegs[^1]}";
            return string.Equals(fSite, sSite, StringComparison.OrdinalIgnoreCase);
        }
        public static bool IsSameSite(this Uri first, string second)
        {
            if (Uri.TryCreate(second, new UriCreationOptions(), out var sec))
                return first.IsSameSite(sec);
            return false;
        }
    }

}

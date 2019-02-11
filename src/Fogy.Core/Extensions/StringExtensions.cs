using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 判断字符串是否为空。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 判断字符串是否为空。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptySpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static T ToEnum<T>(this string value)
            where T : struct
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return (T)Enum.Parse(typeof(T), value);
        }

        public static T ToEnum<T>(this string value, bool ignoreCase)
            where T : struct
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
    }
}

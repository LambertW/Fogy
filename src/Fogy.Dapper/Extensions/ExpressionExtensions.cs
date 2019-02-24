using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Dapper
{
    public static class ExpressionExtensions
    {
        public static bool In<T>(this int obj, T[] array)
        {
            return true;
        }

        public static bool In<T>(this string obj, T[] array)
        {
            return true;
        }

        public static bool In<T>(this Guid obj, T[] array)
        {
            return true;
        }
    }
}

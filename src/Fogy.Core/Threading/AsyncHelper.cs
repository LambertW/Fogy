using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Threading
{
    public static class AsyncHelper
    {
        public static bool IsAsync(this MethodInfo method)
        {
            return (method.ReturnType == typeof(Task) ||
                (method.ReturnType.GetTypeInfo().IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)));
        }

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncContext.Run(func);
        }

        public static void RuncSync(Func<Task> action)
        {
            AsyncContext.Run(action);
        }
    }
}

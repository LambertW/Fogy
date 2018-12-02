using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.ObjectMapper
{
    public interface IObjectMapper
    {
        TDestincation Map<TDestincation>(object source);

        TDestincation Map<TSource, TDestincation>(TSource source, TDestincation destincation);
    }
}

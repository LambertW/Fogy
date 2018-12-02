using Fogy.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Reflection
{
    public class AssemblyFinder : IAssemblyFinder, ISingletonDependency
    {
        public List<Assembly> GetAllAssemblies()
        {
            var assemblies = new List<Assembly>();

            return AppDomain.CurrentDomain.GetAssemblies().ToList();
        }
    }
}

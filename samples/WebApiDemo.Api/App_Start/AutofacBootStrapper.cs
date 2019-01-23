using Autofac;
using Autofac.Integration.WebApi;
using Fogy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;

namespace WebApiDemo.Api
{
    public class AutofacBootStrapper
    {
        public static void CoreAutofactInit()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            // Solve IIS recycle AppDomain problem.
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>();
            var reorderAssemblies = MakeSureCoreModuleIsFirstModule(assemblies.ToList());
            builder.RegisterAssemblyModules(reorderAssemblies.ToArray());

            builder.RegisterApiControllers(assemblies.ToArray());
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterWebApiModelBinderProvider();

            SetupWebApiFilter(builder);

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void SetupWebApiFilter(ContainerBuilder builder)
        {
            //builder.Register(c => new ApiExceptionFilter)
        }

        private static List<Assembly> MakeSureCoreModuleIsFirstModule(List<Assembly> assemblies)
        {
            var coreModule = typeof(CoreInstaller);
            var coreAssembly = assemblies.First(t => t.GetTypes().Contains(coreModule));

            assemblies.Remove(coreAssembly);
            assemblies.Insert(0, coreAssembly);

            return assemblies;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Fogy.Core.Dependency;
using Fogy.Core.Domain.Repositories;
using Fogy.Core.Logging;
using Fogy.Core.Reflection;

namespace Fogy.Core
{
    public class CoreInstaller : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(NullLogger<>))
                .As(typeof(ILogger<>))
                .IfNotRegistered(typeof(ILogger<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<AssemblyFinder>().As<IAssemblyFinder>().SingleInstance();

            builder.RegisterType<ConnectionStringProvider>().As<IConnectionStringProvider>().IfNotRegistered(typeof(IConnectionStringProvider)).SingleInstance();

            var assemblyFinder = new AssemblyFinder();
            var assemblies = assemblyFinder.GetAllAssemblies();
            SetupResolveRules(builder, assemblies.ToArray());
        }

        private void SetupResolveRules(ContainerBuilder builder, Assembly[] assembly)
        {
            var transientType = typeof(ITransientDependency);
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => transientType.IsAssignableFrom(t) && t != transientType)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired();

            var singletonType = typeof(ISingletonDependency);
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => singletonType.IsAssignableFrom(t) && t != singletonType)
                .AsImplementedInterfaces()
                .SingleInstance()
                .PropertiesAutowired();
        }
    }
}

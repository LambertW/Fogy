using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Fogy.Core.Reflection;
using IObjectMapper = Fogy.Core.ObjectMapper.IObjectMapper;

namespace Fogy.AutoMapper
{
    public class MapperInstaller : Autofac.Module
    {
        //private IAssemblyFinder _assemblyFinder;

        public MapperInstaller()
        {
            //_assemblyFinder = assemblyFinder;
        }


        protected override void Load(ContainerBuilder builder)
        {
            Action<IMapperConfigurationExpression> configurer = configuration =>
            {
                FindAndAutoMapTypes(configuration);
            };

            Mapper.Initialize(configurer);

            builder.RegisterInstance(Mapper.Configuration).As<IConfigurationProvider>().SingleInstance();
            builder.RegisterInstance(Mapper.Instance).As<IMapper>().SingleInstance();

            builder.RegisterType<AutoMapperObjectMapper>().As<IObjectMapper>().PropertiesAutowired().SingleInstance();
        }

        private void FindAndAutoMapTypes(IMapperConfigurationExpression configuration)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            var types = assemblies.SelectMany(t => t.GetTypes()).Where(t =>
            {
                var typeInfo = t.GetTypeInfo();
                return typeInfo.IsDefined(typeof(AutoMapAttribute))
                    || typeInfo.IsDefined(typeof(AutoMapFromAttribute))
                    || typeInfo.IsDefined(typeof(AutoMapToAttribute));
            });

            foreach (var type in types)
            {
                configuration.CreateAutoAttributeMaps(type);
            }
        }
    }
}

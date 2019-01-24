using Autofac;
using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Dapper
{
    public class DapperInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var classMapper = typeof(IClassMapper);
            var assembiles = AppDomain.CurrentDomain.GetAssemblies()
                .Where(t =>
                    t.GetTypes().Any(inner => classMapper.IsAssignableFrom(inner) && inner != classMapper)).ToArray();

            DapperExtensions.DapperExtensions.SetMappingAssemblies(assembiles);
            DapperExtensions.DapperAsyncExtensions.SetMappingAssemblies(assembiles);
        }
    }
}

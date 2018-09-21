using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.WebApi;
using Lottery.Service;
using Lottery.Service.IoC.Autofac;
using System.Reflection;
using System.Web.Http;

namespace Lottery.Web.App_Start
{
    public class IocConfig
    {
        public static IContainer Container;

        public static void Initialize(HttpConfiguration configuration)
        {
            Initialize(configuration, RegisterDependencies(new ContainerBuilder()));
        }

        private static void Initialize(HttpConfiguration configuration, IContainer container)
        {
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterDependencies(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<LotteryManager>().As<ILotteryManager>().InstancePerRequest();

            builder.RegisterModule(new ServiceModule());

            return builder.Build();
        }
    }
}
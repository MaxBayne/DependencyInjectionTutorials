using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Ninject;
using DependencyInjection.Interfaces;
using DependencyInjection.Application;
using DependencyInjection.Services;


namespace DependencyInjection
{
    class Program
    {
        public static IContainer Container;

        static void Main(string[] args)
        {
            #region Ninject

            /////////////////////////////////////
            //Dependency Injection using Ninject
            /////////////////////////////////////

            /*

            //Declare IOC Container
            using var kernel = new StandardKernel();

            //Register Components
            kernel.Bind<IApp>().To<App>().InSingletonScope();
            kernel.Bind<IServices>().To<Application.Services>().InSingletonScope();
            kernel.Bind<IConfigService>().To<ConfigService>();

            //Resolve the Components
            var app = kernel.Get<IApp>();
            app.Run();
            

            //Constructor Injection [IApp]
            //var services = kernel.Get<IServices>();

            //Property Injection [IApp]
            //var configs = kernel.Get<IConfigService>();

            */


            #endregion


            /////////////////////////////////////
            //Dependency Injection using Autofac
            /////////////////////////////////////

            //Build Container IOC
            var builder = new ContainerBuilder();
            builder.RegisterModule<AppModule>();
            Container = builder.Build();


            //Take Scope From Container
            using (var scope = Container.BeginLifetimeScope())
            {
                //Resolve
                //var app = scope.Resolve<IApp>();
                var config = scope.Resolve<IConfigService>();

                
                config.Print();
                config.Print();
                config.Print();

                //app.Run();

            }


            Console.ReadLine();
        }

        public class AppModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                //Register Components

                //Register Instance
                //builder.RegisterInstance(new App(50)).As<IApp>();

                //Register Type as Self
                //builder.RegisterType<App>().AsSelf();

                //Register Type of Class
                builder.RegisterType<App>().As<IApp>().SingleInstance();
                builder.RegisterType<Application.Services>().As<IServices>();

                //Register Type with its implemented Interfaces
                //builder.RegisterType<App>().AsImplementedInterfaces().SingleInstance();

                //To Allow Properties Injection use AutoWired
                builder.RegisterType<ConfigService>().As<IConfigService>().PropertiesAutowired();

                //Register Delegate
                //builder.Register((c) =>new ConfigService()).As<IConfigService>().PropertiesAutowired();


            }
        }
    }
}

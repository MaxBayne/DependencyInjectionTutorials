using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Ninject;
using DependencyInjection.Interfaces;
using DependencyInjection.Application;
using DependencyInjection.Services;
using Module = Autofac.Module;


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

            var builder = new ContainerBuilder();
            
            //Register Components Declared inside Module
            //builder.RegisterModule<AppModule>();
            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
            
            //Register Components inside Assembly
            //var currentAssembly = Assembly.GetExecutingAssembly();
            //builder.RegisterAssemblyTypes(currentAssembly)
            //       .AsImplementedInterfaces();

            //Build Container IOC
            Container = builder.Build();


            //Take Scope From Container
            using (var scope = Container.BeginLifetimeScope("scoped"))
            {
                //Resolve
                var app = scope.Resolve<IApp>();

                var log1 = scope.Resolve<ILogService>(new NamedParameter("logName", "outerScope"));
                var log2 = scope.Resolve<ILogService>(new NamedParameter("logName", "outerScope"));


                Console.WriteLine($"Log1: {log1.GetLogName()}");
                Console.WriteLine($"Log2: {log2.GetLogName()}");


                using (var scope2 = scope.BeginLifetimeScope())
                {
                    var log3 = scope2.Resolve<ILogService>(new NamedParameter("logName", "nestedScope"));
                    Console.WriteLine($"Log3: {log3.GetLogName()}");
                }



                //app.Run();

                //Dispose Scope and its Resolved Components
                scope.Dispose();
            }


            using (var scope3 = Container.BeginLifetimeScope("scoped"))
            {
                var log4 = scope3.Resolve<ILogService>(new NamedParameter("logName", "OtherScope"));
                Console.WriteLine($"Log4: {log4.GetLogName()}");
            }





            Console.ReadLine();
        }

        public class AppModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                //Register Components

                //-----------------------------------------------------------
                //Register Instance
                //builder.RegisterInstance(new App(50)).As<IApp>();
                //-----------------------------------------------------------

                //Register Type as Self
                //builder.RegisterType<App>().AsSelf();

                //-----------------------------------------------------------

                //Register Type of Class

                builder.RegisterType<App>().As<IApp>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).SingleInstance();
                
                builder.RegisterType<Application.Services>().As<IServices>().PropertiesAutowired();

                //-----------------------------------------------------------

                //Register Type with its implemented Interfaces
                //builder.RegisterType<App>().AsImplementedInterfaces().SingleInstance();

                //-----------------------------------------------------------

                //Properties Injection For Reflection
                //inject all properties that are public and writable
                //builder.RegisterType<ConfigService>().As<IConfigService>().PropertiesAutowired();
                //builder.RegisterType<App>().As<IApp>().WithProperty("PropertyName", "PropertyValue");

                //To Allow  circular dependencies
                builder.RegisterType<ConfigService>().As<IConfigService>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

                //-----------------------------------------------------------

                //Properties Injection For Lambda Expression
                //inject all properties that are public and writable
                //builder.Register(c=>new App{Services=c.ResolveOptional<IServices>()}).As<IApp>();

                //To Allow  circular dependencies
                //builder.Register(c => new ConfigService())
                //       .OnActivated(e => e.Instance.App = e.Context.Resolve<IApp>())
                //       .As<IConfigService>();

                //-----------------------------------------------------------

                //Register Type with Specific Constructor , default if will choose the most constructor with registered components
                //builder.RegisterType<SecurityService>().UsingConstructor(typeof(IApp),typeof(IConfigService)).As<ISecurityService>();
                //-----------------------------------------------------------
                //Register Type and control which implemented type will be used by using parameters
                /*builder.Register<IAppService>((context, parameters) =>
                {
                    var p = parameters.Named<string>("ServiceName");

                    if (p == "cache")
                    {
                        return new CacheService();
                    }
                    else if (p=="migration")
                    {
                        return new MigrationService();
                    }
                    else
                    {
                        return new SecurityService();
                    }
                });
                var cacheService = scope.Resolve<IAppService>(new NamedParameter("ServiceName", "cache"));
                var migrationService = scope.Resolve<IAppService>(new NamedParameter("ServiceName", "migration"));
                */

                //-----------------------------------------------------------
                //when register different components with same service[interface] so the default behavior that the last registration will be used mean [CacheService]
                //if u want to select the first registration so use [PreserveExistingDefaults]
                //builder.RegisterType<MigrationService>().As<IAppService>();
                //builder.RegisterType<CacheService>().As<IAppService>().PreserveExistingDefaults();

                //-----------------------------------------------------------

                //Passing Parameters With Registration

                //builder.Register(c => new SecurityService(securityType: "type", securityDirection: "diretion")).As<ISecurityService>();

                //builder.RegisterType<SecurityService>().As<ISecurityService>().WithParameter("securityType", "TestType");

                //-----------------------------------------------------------

                //Passing Parameters with Resolving

                //builder.Register((context, parameters) => new SecurityService(securityType:parameters.Named<string>("securityType"))).As<ISecurityService>();
                //var sec = scope.Resolve<ISecurityService>(new NamedParameter("securityType", "testType"));


                //-----------------------------------------------------------

                //Scope For Instance

                //[InstancePerDependency] mean every resolve will create new instance of component , its default 
                //builder.RegisterType<LogService>().As<ILogService>().InstancePerDependency();

                //[SingleInstance] mean once Instance Over All Scopes , its global shared
                //builder.RegisterType<LogService>().As<ILogService>().SingleInstance();

                //[InstancePerLifetimeScope] mean every resolve inside scope will be use one instance only , other scope cant access this shared over one scope , nested scope will get new instance
                //builder.RegisterType<LogService>().As<ILogService>().InstancePerLifetimeScope();

                //[InstancePerMatchingLifetimeScope] mean every resolve inside multi scope that use [scoped] tag will share only one instance over multi scopes
                //so its like singleton but inside tagged scope only not global over the application
                //builder.RegisterType<LogService>().As<ILogService>().InstancePerMatchingLifetimeScope("scoped");

                //-----------------------------------------------------------

                //Release Instance after scope end

                //Auto Dispose
                //need component type implement  [IDisposable] or [IAsyncDisposable] to can autofac dispose it automatically

                //Manual Dispose
                //if you need dispose manula to call dispose by OnRelease 
                //builder.RegisterType<LogService>().As<ILogService>().OnRelease(e=>e.Dispose());

                //Disable Dispose
                //so container will not dispose that components
                //builder.RegisterType<LogService>().As<ILogService>().ExternallyOwned();

                //-----------------------------------------------------------

                //Events

                //[OnPreparing]  : event will fire each time try to resolve the component , used to change the parameters that component need on create instance
                //builder.RegisterType<LogService>().As<ILogService>().OnPreparing(e => Console.WriteLine("OnPreparing LogService"));

                //[OnActivating] : event will fire before using the component so here you can replace the instance with another or make property or method injection
                //builder.RegisterType<LogService>().As<ILogService>().OnActivating((e) => Console.WriteLine("OnActivating LogService"));

                //[OnActivated] : event will fire after component constructed mean after creating instance from component , u can use it to perform some action after component created
                //builder.RegisterType<LogService>().As<ILogService>().OnActivated((e) => Console.WriteLine("OnActivated LogService"));

                //[OnRelease] : event will fire after release the component when scope end
                //builder.RegisterType<LogService>().As<ILogService>().OnRelease(e => Console.WriteLine("OnRelease LogService"));

                //builder.RegisterType<LogService>()
                //       .As<ILogService>()
                //       .OnPreparing(e => Console.WriteLine("OnPreparing LogService"))
                //       .OnActivating(e => Console.WriteLine("OnActivating LogService"))
                //       .OnActivated(e => Console.WriteLine("OnActivated LogService"))
                //       .OnRelease(e => Console.WriteLine("OnRelease LogService"));

                //-----------------------------------------------------------

            }
        }
    }
}

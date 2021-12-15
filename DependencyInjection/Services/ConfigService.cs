using DependencyInjection.Application;
using DependencyInjection.Interfaces;
using Ninject;

namespace DependencyInjection.Services
{
    public interface IConfigService
    {
        void Print();
    }

    public class ConfigService: IConfigService
    {
        //[Inject]
        //public IApp app{ get; set; }

        public IApp app { get; set; }

        public ConfigService()
        {
            //app.PrintNumber();
        }

        public void Print()
        {
            app.PrintNumber();
        }
    }
    
}
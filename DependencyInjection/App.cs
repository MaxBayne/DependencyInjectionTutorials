using System;
using System.Threading.Tasks;
using Ninject;

namespace DependencyInjection.Application
{
    public interface IApp
    {
        void Run();
        Task PrintNumber();

        void Increment();
    }

    public class App : IApp
    {
        //private readonly IServices _services;
        private int _counter;

        //Property Injection By Ninject
        //[Inject]
        //public IServices Services { get; set; }


        public IServices Services{ get; set; }
        


        public App(int counter=0)
        {
            _counter = counter;
        }

        public void Run()
        {
            Increment();
            Services.RunAsync();
        }

        public void Increment()
        {
            ++_counter;
        }

        public async Task PrintNumber()
        {
            await Task.Run(() =>
            {
                Console.WriteLine(_counter);
            });
            
        }
    }
}
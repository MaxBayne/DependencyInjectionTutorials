using System.Threading.Tasks;
using DependencyInjection.Services;

// ReSharper disable once CheckNamespace
namespace DependencyInjection.Application
{
    public interface IServices
    {
        Task RunAsync();

    }

    public class Services : IServices
    {
        private IApp _app;
        public Services(IApp app)
        {
            _app = app;

            //RunAsync();
            _app.PrintNumber();
        }

        public ISecurityService SecurityService { get; set; }

        public async Task RunAsync()
        {
           await _app.PrintNumber();
        }
    }
}
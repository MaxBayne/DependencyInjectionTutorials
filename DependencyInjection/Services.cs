using System.Threading.Tasks;

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


        public async Task RunAsync()
        {
           await _app.PrintNumber();
        }
    }
}
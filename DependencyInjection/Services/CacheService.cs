using DependencyInjection.Interfaces;

namespace DependencyInjection.Services
{
    public interface ICacheService
    {

    }

    public class CacheService:ICacheService, IAppService
    {
        public CacheService()
        {
            
        }
    }
}
using DependencyInjection.Application;
using DependencyInjection.Interfaces;

namespace DependencyInjection.Services
{
    public interface ISecurityService
    {

    }

    public class SecurityService:ISecurityService,IAppService
    {

        public SecurityService(string securityType)
        {

        }

        public SecurityService(IApp app)
        {
            
        }

        public SecurityService(IApp app,IConfigService configService)
        {

        }

    }
}
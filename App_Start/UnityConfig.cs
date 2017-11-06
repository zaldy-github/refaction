using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using RefactorMe.Services;

using RefactorMe.Models.UnitOfWork;
namespace RefactorMe
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IProductServices, ProductServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IProductOptionServices, ProductOptionServices>().RegisterType<UnitOfWork>(new HierarchicalLifetimeManager());

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
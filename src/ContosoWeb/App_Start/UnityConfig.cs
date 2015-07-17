using Microsoft.Practices.Unity;
using Contoso.Models;
using ContosoWeb.ProductSearch;
using ContosoWeb.Recommendations;
using ContosoWeb.Utils;

namespace ContosoWeb
{
    public class UnityConfig
    {
        public static UnityContainer BuildContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IContosoWebContext, ContosoWebContext>(new InjectionConstructor(StaticConfig.DbContext.WebConnectionStringName));
            container.RegisterType<IOrdersQuery, OrdersQuery>();
            container.RegisterType<IRaincheckQuery, RaincheckQuery>();
            container.RegisterType<IRecommendationEngine, AzureMLFrequentlyBoughtTogetherRecommendationEngine>();
            container.RegisterType<ITelemetryProvider, TelemetryProvider>();
            container.RegisterType<IProductSearch, StringContainsProductSearch>();

            container.RegisterInstance<IHttpClient>(container.Resolve<HttpClientWrapper>());

            return container;
        }
    }
}
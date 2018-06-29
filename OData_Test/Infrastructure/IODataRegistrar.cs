using System;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Routing;
using OData_Test.Data.Domain;

namespace OData_Test.Infrastructure
{
    public interface IODataRegistrar
    {
        void Register(IRouteBuilder routes, IServiceProvider services);
    }

    public class ODataRegistrar : IODataRegistrar
    {
        public void Register(IRouteBuilder routes, IServiceProvider services)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder(services);

            builder.EntitySet<MessageTemplate>("MessageTemplateApi");
            builder.EntitySet<MessageTemplateVersion>("MessageTemplateVersionApi");
            builder.EntitySet<PageType>("PageTypeApi");
            builder.EntitySet<PageType>("PageTypeApi");
            builder.EntitySet<Region>("RegionApi");
            //builder.EntitySet<PublicUserInfo>("PublicUserApi");

            var getCurrentVersionFunction = builder.EntityType<MessageTemplateVersion>().Collection.Function("GetCurrentVersion");
            getCurrentVersionFunction.Parameter<int>("templateId");
            getCurrentVersionFunction.Parameter<string>("cultureCode");
            getCurrentVersionFunction.ReturnsFromEntitySet<MessageTemplateVersion>("MessageTemplateVersionApi");

            routes.MapODataServiceRoute("OData", "odata", builder.GetEdmModel());
        }
    }
}
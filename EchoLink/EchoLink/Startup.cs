using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(EchoLink.Startup))]

namespace EchoLink
{
    using System.Web.Http;

    using JetBrains.Annotations;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class Startup
    {
        [UsedImplicitly]
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            var formatters = config.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            app.UseWebApi(config);
        }
    }
}

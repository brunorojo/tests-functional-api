using Microsoft.Extensions.Configuration;
using System.IO;

namespace TestesFuncionais.Configurations
{
    public class ApiConfigurations
    {
        public string BaseUrl { get; set; }
        public HeadersConfig Headers { get; set; }
        public RoutesConfig Routes { get; set; }
    }

    public class HeadersConfig
    {
        public string Authorization { get; set; }
        public string CustomHeader { get; set; }
    }

    public class RoutesConfig
    {
        public string CreateItem { get; set; }
        public string GetAllItems { get; set; }
        public string UpdateItem { get; set; }
        public string DeleteItem { get; set; }
    }

    public class ConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configurations"))
                .AddJsonFile("config.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public ApiConfigurations GetApiConfiguration()
        {
            var environment = _configuration.GetValue<string>("Environment");

            var environmentConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "Configurations", $"{environment}.json");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(environmentConfigPath, optional: false, reloadOnChange: true);

            var config = builder.Build();
            
            var apiConfigSection = config.GetSection("ApiConfigurations");
            var apiConfigurations = new ApiConfigurations
            {
                BaseUrl = apiConfigSection.GetValue<string>("BaseUrl"),
                Headers = new HeadersConfig
                {
                    Authorization = apiConfigSection.GetSection("Headers").GetValue<string>("Authorization"),
                    CustomHeader = apiConfigSection.GetSection("Headers").GetValue<string>("CustomHeader")
                },
                Routes = new RoutesConfig
                {
                    CreateItem = apiConfigSection.GetSection("Routes").GetValue<string>("CreateItem"),
                    GetAllItems = apiConfigSection.GetSection("Routes").GetValue<string>("GetAllItems"),
                    UpdateItem = apiConfigSection.GetSection("Routes").GetValue<string>("UpdateItem"),
                    DeleteItem = apiConfigSection.GetSection("Routes").GetValue<string>("DeleteItem")
                }
            };

            return apiConfigurations;
        }
    }
}

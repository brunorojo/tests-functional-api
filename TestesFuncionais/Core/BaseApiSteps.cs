using RestSharp;
using TestesFuncionais.Configurations;

namespace TestesFuncionais.Core;

public abstract class BaseApiSteps : XunitContextBase
{
    protected readonly ITestOutputHelper OutputHelper;
    protected readonly RestClient Client;

    protected BaseApiSteps(ITestOutputHelper outputHelper, ConfigurationService configService) : base(outputHelper)
    {
        OutputHelper = outputHelper;

        var apiConfig = configService.GetApiConfiguration();
        Client = new RestClient(apiConfig.BaseUrl); // Agora usa a URL configurável

        OutputHelper.WriteLine($"### {Context.Test.DisplayName} ###");
    }

    protected async Task<RestResponse> ExecuteWithRetryAsync(RestRequest request, int maxRetries = 3)
    {
        int attempt = 0;
        RestResponse? response = null;

        while (attempt < maxRetries)
        {
            attempt++;
            OutputHelper.WriteLine($"Tentativa {attempt} de {maxRetries}");

            response = await Client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return response;
            }

            OutputHelper.WriteLine($"Falha na tentativa {attempt}: {response.StatusCode} - {response.ErrorMessage}");
            await Task.Delay(2000);
        }

        throw new Xunit.Sdk.XunitException($"Todas as tentativas falharam. Último erro: {response?.StatusCode} - {response?.ErrorMessage}");
    }
}
using RestSharp;
using Xunit;

namespace TestesFuncionais;

public abstract class BaseApiSteps : XunitContextBase
{
    protected readonly ITestOutputHelper _output;
    protected readonly RestClient _client;

    protected BaseApiSteps(ITestOutputHelper output) : base(output)
    {
        _output = output;
        _client = new RestClient("https://jsonplaceholder.typicode.com");
        _output.WriteLine($"### {Context.Test.DisplayName} ###");
    }

    protected async Task<RestResponse> ExecuteWithRetryAsync(RestRequest request, int maxRetries = 3)
    {
        int attempt = 0;
        RestResponse? response = null;

        while (attempt < maxRetries)
        {
            attempt++;
            _output.WriteLine($"Tentativa {attempt} de {maxRetries}");

            response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return response;
            }

            _output.WriteLine($"Falha na tentativa {attempt}: {response.StatusCode} - {response.ErrorMessage}");
            await Task.Delay(2000);
        }

        throw new Xunit.Sdk.XunitException($"Todas as tentativas falharam. Último erro: {response?.StatusCode} - {response?.ErrorMessage}");
    }
}
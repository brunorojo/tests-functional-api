using RestSharp;
using TestesFuncionais.Configurations;
using TestesFuncionais.Core;

namespace TestesFuncionais;

public class ApiTests : BaseApiSteps
{
    private readonly ConfigurationService _configService;
    private readonly ApiConfigurations _apiConfig;

    public ApiTests(ITestOutputHelper outputHelper) : base(outputHelper, new ConfigurationService())
    {
        _configService = new ConfigurationService();
        _apiConfig = _configService.GetApiConfiguration();
    }

    private RestClient CreateClient()
    {
        return new RestClient(_apiConfig.BaseUrl); // Usa a BaseUrl configurada
    }

    [Fact]
    public async Task CriarItemComSucesso()
    {
        var request = BaseApiArrangers.CreateRequest("posts", Method.Post);
        request.AddJsonBody(BaseApiArrangers.CreatePostBody());

        var response = await ExecuteWithRetryAsync(request);

        OutputHelper.WriteLine("## RESPONSE:");
        OutputHelper.WriteLine($"Status Code: {response.StatusCode}");
        OutputHelper.WriteLine($"Content: {response.Content ?? "Resposta vazia"}");

        Assert.True(response.IsSuccessful, $"Erro na requisição: {response.StatusCode} - {response.ErrorMessage}");
        Assert.False(string.IsNullOrWhiteSpace(response.Content), "Resposta vazia ou inválida.");
    }

    [Fact]
    public async Task AtualizarItemComSucesso()
    {
        var request = BaseApiArrangers.CreateRequest("posts/1", Method.Patch);
        request.AddJsonBody(BaseApiArrangers.CreatePatchBody());

        var response = await ExecuteWithRetryAsync(request);

        OutputHelper.WriteLine("## RESPONSE:");
        OutputHelper.WriteLine($"Status Code: {response.StatusCode}");
        OutputHelper.WriteLine($"Content: {response.Content ?? "Response vazio"}");

        Assert.True(response.IsSuccessful, $"Erro na requisição: {response.StatusCode} - {response.ErrorMessage}");
        Assert.False(string.IsNullOrWhiteSpace(response.Content), "Response inválido.");
    }

    [Fact]
    public async Task ConsultarTodosItensComSucesso()
    {
        var request = BaseApiArrangers.CreateRequest("posts", Method.Get);

        var response = await ExecuteWithRetryAsync(request);

        OutputHelper.WriteLine("## RESPONSE:");
        OutputHelper.WriteLine($"Status Code: {response.StatusCode}");
        OutputHelper.WriteLine($"Content: {response.Content ?? "Resposta vazia"}");

        Assert.True(response.IsSuccessful, $"Erro na requisição: {response.StatusCode} - {response.ErrorMessage}");
        Assert.False(string.IsNullOrWhiteSpace(response.Content), "Response inválido.");
    }
}
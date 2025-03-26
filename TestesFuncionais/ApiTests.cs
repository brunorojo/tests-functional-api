using RestSharp;
using TestesFuncionais;

public class ApiTests : BaseApiSteps
{
    public ApiTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task CriarItemComSucesso()
    {
        var request = BaseApiArrangers.CreateRequest("posts", Method.Post);
        request.AddJsonBody(BaseApiArrangers.CreatePostBody());

        var response = await ExecuteWithRetryAsync(request);

        _output.WriteLine("## RESPONSE:");
        _output.WriteLine($"Status Code: {response.StatusCode}");
        _output.WriteLine($"Content: {response.Content ?? "Resposta vazia"}");

        Assert.True(response.IsSuccessful, $"Erro na requisição: {response.StatusCode} - {response.ErrorMessage}");
        Assert.False(string.IsNullOrWhiteSpace(response.Content), "Resposta vazia ou inválida.");
    }

    [Fact]
    public async Task AtualizarItemComSucesso()
    {
        var request = BaseApiArrangers.CreateRequest("posts/1", Method.Patch);
        request.AddJsonBody(BaseApiArrangers.CreatePatchBody());

        var response = await ExecuteWithRetryAsync(request);

        _output.WriteLine("## RESPONSE:");
        _output.WriteLine($"Status Code: {response.StatusCode}");
        _output.WriteLine($"Content: {response.Content ?? "Response vazio"}");

        Assert.True(response.IsSuccessful, $"Erro na requisição: {response.StatusCode} - {response.ErrorMessage}");
        Assert.False(string.IsNullOrWhiteSpace(response.Content), "Response invalido.");
    }

    [Fact]
    public async Task ConsultarTodosItensComSucesso()
    {
        var request = BaseApiArrangers.CreateRequest("posts", Method.Get);

        var response = await ExecuteWithRetryAsync(request);

        _output.WriteLine("## RESPONSE:");
        _output.WriteLine($"Status Code: {response.StatusCode}");
        _output.WriteLine($"Content: {response.Content ?? "Resposta vazia"}");

        Assert.True(response.IsSuccessful, $"Erro na requisição: {response.StatusCode} - {response.ErrorMessage}");
        Assert.False(string.IsNullOrWhiteSpace(response.Content), "Response inválido.");
    }
}

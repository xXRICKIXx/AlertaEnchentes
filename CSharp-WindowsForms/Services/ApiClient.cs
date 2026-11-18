using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AlertaEnchentes.Desktop.Models;

namespace AlertaEnchentes.Desktop.Services;

/// <summary>
/// Consome a API REST (SOA) via HTTP/JSON.
/// A URL base pode ser alterada pela variável de ambiente ALERTA_ENCHENTES_API_URL.
/// </summary>
public class ApiClient : IDisposable
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _http;

    public ApiClient()
    {
        var url = Environment.GetEnvironmentVariable("AQUAGUARD_API_URL") ?? "http://localhost:5080/";
        _http = new HttpClient
        {
            BaseAddress = new Uri(url),
            Timeout = TimeSpan.FromSeconds(10)
        };
    }

    // ---------- Ocorrências ----------
    public async Task<List<Ocorrencia>> ListarOcorrenciasAsync() =>
        await GetAsync<List<Ocorrencia>>("api/ocorrencias") ?? new();

    public Task<Ocorrencia?> ObterOcorrenciaAsync(int id) =>
        GetAsync<Ocorrencia>($"api/ocorrencias/{id}");

    public Task<Ocorrencia?> CriarOcorrenciaAsync(NovaOcorrencia nova) =>
        SendAsync<Ocorrencia>(HttpMethod.Post, "api/ocorrencias", nova);

    public Task<Ocorrencia?> AtualizarStatusAsync(int id, string status) =>
        SendAsync<Ocorrencia>(HttpMethod.Patch, $"api/ocorrencias/{id}/status", new { status });

    // ---------- Alertas ----------
    public async Task<List<Alerta>> ListarAlertasAsync() =>
        await GetAsync<List<Alerta>>("api/alertas") ?? new();

    public Task<Alerta?> CriarAlertaAsync(NovoAlerta novo) =>
        SendAsync<Alerta>(HttpMethod.Post, "api/alertas", novo);

    // ---------- Infra HTTP ----------
    private async Task<T?> GetAsync<T>(string url)
    {
        using var resposta = await _http.GetAsync(url);
        await GarantirSucessoAsync(resposta);
        return await resposta.Content.ReadFromJsonAsync<T>(Json);
    }

    private async Task<T?> SendAsync<T>(HttpMethod metodo, string url, object corpo)
    {
        using var requisicao = new HttpRequestMessage(metodo, url)
        {
            Content = JsonContent.Create(corpo, options: Json)
        };
        using var resposta = await _http.SendAsync(requisicao);
        await GarantirSucessoAsync(resposta);
        return await resposta.Content.ReadFromJsonAsync<T>(Json);
    }

    private static async Task GarantirSucessoAsync(HttpResponseMessage resposta)
    {
        if (resposta.IsSuccessStatusCode) return;

        var corpo = await resposta.Content.ReadAsStringAsync();
        throw new ApiException((int)resposta.StatusCode, ExtrairMensagem(corpo, resposta.ReasonPhrase));
    }

    /// <summary>Lê "detail" ou a lista "errors" (ValidationProblemDetails) do corpo da resposta.</summary>
    private static string ExtrairMensagem(string corpo, string? padrao)
    {
        try
        {
            using var doc = JsonDocument.Parse(corpo);
            var raiz = doc.RootElement;

            if (raiz.TryGetProperty("errors", out var erros) && erros.ValueKind == JsonValueKind.Object)
            {
                var sb = new StringBuilder();
                foreach (var campo in erros.EnumerateObject())
                    foreach (var msg in campo.Value.EnumerateArray())
                        sb.AppendLine("• " + msg.GetString());
                if (sb.Length > 0) return sb.ToString().TrimEnd();
            }

            if (raiz.TryGetProperty("detail", out var detalhe) && detalhe.ValueKind == JsonValueKind.String)
                return detalhe.GetString() ?? padrao ?? "Erro na API.";
        }
        catch (JsonException)
        {
            // corpo não é JSON: usa a mensagem padrão
        }

        return padrao ?? "Erro na API.";
    }

    public void Dispose() => _http.Dispose();
}

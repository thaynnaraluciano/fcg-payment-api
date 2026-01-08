using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;

namespace Domain;

public class ValidacaoServicosExternos
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ValidacaoServicosExternos> _logger;

    public ValidacaoServicosExternos(IHttpClientFactory httpClientFactory, ILogger<ValidacaoServicosExternos> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<bool> UsuarioExisteAsync(Guid userId)
    {
        var client = _httpClientFactory.CreateClient("UserApi");

        var url = $"/api/v1/usuario/BuscarUsuarioPorId/{userId}";

        _logger.LogInformation("Validando usuário no UserAPI: GET {Url}", url);

        var resp = await client.GetAsync(url);

        if (resp.StatusCode == HttpStatusCode.NotFound)
            return false;

        resp.EnsureSuccessStatusCode(); // se erro 401/500 etc, vai aparecer no log
        return true;
    }

    public async Task<bool> JogoExisteAsync(Guid gameId)
    {
        var client = _httpClientFactory.CreateClient("GameApi");

        // ⚠️ Ajuste aqui conforme a rota real do seu swagger:
        var url = $"api/v1/jogo/{gameId}";

        _logger.LogInformation("Validando jogo no GameAPI: GET {Url}", url);

        var resp = await client.GetAsync(url);

        if (resp.StatusCode == HttpStatusCode.NotFound)
            return false;

        resp.EnsureSuccessStatusCode(); // se erro 401/500 etc, vai aparecer no log
        return true;
    }
}

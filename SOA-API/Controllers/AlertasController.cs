using AlertaEnchentes.Api.Models;
using AlertaEnchentes.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlertaEnchentes.Api.Controllers;

[ApiController]
[Route("api/alertas")]
public class AlertasController : ControllerBase
{
    private readonly IAlertaService _service;

    public AlertasController(IAlertaService service) => _service = service;

    /// <summary>Consulta os alertas disponíveis.</summary>
    [HttpGet]
    public async Task<ActionResult<List<Alerta>>> Get() =>
        Ok(await _service.ListarAsync());

    /// <summary>Cria um novo alerta relacionado a uma situação de risco.</summary>
    [HttpPost]
    public async Task<ActionResult<Alerta>> Post([FromBody] AlertaCreateDto dto)
    {
        try
        {
            var criado = await _service.CriarAsync(dto);
            return Created($"/api/alertas/{criado.Id}", criado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { detail = ex.Message });
        }
    }
}

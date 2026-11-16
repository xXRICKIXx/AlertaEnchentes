using AlertaEnchentes.Api.Models;
using AlertaEnchentes.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlertaEnchentes.Api.Controllers;

[ApiController]
[Route("api/ocorrencias")]
public class OcorrenciasController : ControllerBase
{
    private readonly IOcorrenciaService _service;

    public OcorrenciasController(IOcorrenciaService service) => _service = service;

    /// <summary>Retorna todas as ocorrências registradas.</summary>
    [HttpGet]
    public async Task<ActionResult<List<Ocorrencia>>> Get() =>
        Ok(await _service.ListarAsync());

    /// <summary>Retorna uma ocorrência específica.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Ocorrencia>> GetById(int id)
    {
        var ocorrencia = await _service.ObterAsync(id);
        return ocorrencia is null ? NotFound(new { detail = $"Ocorrência {id} não encontrada." }) : Ok(ocorrencia);
    }

    /// <summary>Registra uma nova ocorrência.</summary>
    [HttpPost]
    public async Task<ActionResult<Ocorrencia>> Post([FromBody] OcorrenciaCreateDto dto)
    {
        try
        {
            var criada = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = criada.Id }, criada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { detail = ex.Message });
        }
    }

    /// <summary>Atualiza o status (Aberta, Em acompanhamento, Resolvida).</summary>
    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<Ocorrencia>> PatchStatus(int id, [FromBody] StatusUpdateDto dto)
    {
        try
        {
            var atualizada = await _service.AtualizarStatusAsync(id, dto.Status);
            return atualizada is null ? NotFound(new { detail = $"Ocorrência {id} não encontrada." }) : Ok(atualizada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { detail = ex.Message });
        }
    }
}

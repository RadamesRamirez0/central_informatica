using central_informatica.src.Server.Services;
using central_informatica.src.Shared.DTOs;
using central_informatica.src.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace central_informatica.src.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonasController : ControllerBase
{
    private readonly PersonasService _personasService;

    public PersonasController(PersonasService personasService)
    {
        _personasService = personasService;
    }

    [HttpGet]
    public async Task<ActionResult<Persona>> GetAllPersonas([FromQuery] string? busqueda)
    {
        return Ok(await _personasService.GetAll(busqueda));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Persona>> GetPersonaByID([FromRoute] int id)
    {
        return Ok(await _personasService.GetById(id));
    }

    [HttpPost]
    public async Task<ActionResult<Persona>> PostPersona([FromBody] PersonaDTO dto)
    {
        return Ok(await _personasService.Add(dto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Persona>> PutUpdatePersona(
        [FromRoute] int id,
        [FromBody] PersonaDTO dto
    )
    {
        try
        {
            return Ok(await _personasService.Update(id, dto));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Persona>> DeletePersona([FromRoute] int id)
    {
        try
        {
            return Ok(await _personasService.Delete(id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

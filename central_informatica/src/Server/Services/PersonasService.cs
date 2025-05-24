using central_informatica.src.Server.Interfaces;
using central_informatica.src.Server.Repository;
using central_informatica.src.Shared.DTOs;
using central_informatica.src.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace central_informatica.src.Server.Services;

public class PersonasService : IService<Persona, PersonaDTO>
{
    private readonly ProjectContext _ctx;

    public PersonasService(ProjectContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<Persona>> GetAll(string? busqueda)
    {
        IQueryable<Persona> query = _ctx.Personas;

        if (!busqueda.IsNullOrEmpty())
        {
            query = query.Where(x =>
                x.Nombre.Contains(busqueda)
                || x.ApellidoPaterno.Contains(busqueda)
                || x.ApellidoMaterno.Contains(busqueda)
            );
        }

        return await query.ToListAsync();
    }

    public async Task<Persona?> GetById(int id)
    {
        return await _ctx.Personas.FindAsync(id);
    }

    public async Task<Persona> Add(PersonaDTO dto)
    {
        Persona newPersona = new(dto);

        _ctx.Personas.Add(newPersona);

        await _ctx.SaveChangesAsync();

        return newPersona;
    }

    public async Task<Persona> Update(int id, PersonaDTO dto)
    {
        var persona = await _ctx.Personas.FindAsync(id);

        if (persona == null)
        {
            throw new Exception("Persona no encontrada");
        }

        persona.UpdateFromDTO(dto);

        _ctx.Personas.Update(persona);

        await _ctx.SaveChangesAsync();

        return persona;
    }

    public async Task<Persona> Delete(int id)
    {
        var persona = await _ctx.Personas.FindAsync(id);

        if (persona == null)
        {
            throw new Exception("Persona no encontrada");
        }

        _ctx.Personas.Remove(persona);

        await _ctx.SaveChangesAsync();

        return persona;
    }
}

using central_informatica.src.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace central_informatica.src.Server.Repository;

public class ProjectContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Persona> Personas { get; set; }
}

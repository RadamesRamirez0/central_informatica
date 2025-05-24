using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace central_informatica.src.Shared.DTOs;

public class PersonaDTO
{
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string ApellidoPaterno { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string ApellidoMaterno { get; set; } = string.Empty;

    [Required]
    public DateTime FechaNacimiento { get; set; }

    private char _sexo;

    [Required]
    [Column(TypeName = "char(1)")]
    public char Sexo
    {
        get => _sexo;
        set
        {
            char[] validGenres = ['F', 'M'];
            if (!validGenres.Contains(value))
            {
                throw new ArgumentException("Sexo solo puede ser M o F");
            }

            _sexo = value;
        }
    }
}

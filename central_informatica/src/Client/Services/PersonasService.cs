using System.Text;
using System.Text.Json;
using central_informatica.src.Shared.DTOs;
using central_informatica.src.Shared.Models;
using Microsoft.IdentityModel.Tokens;

namespace central_informatica.src.Client.Services;

public class PersonasService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public PersonasService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<List<Persona>> GetAllAsync(string? busqueda)
    {
        try
        {
            var uri = "api/personas";
            if (busqueda != null)
            {
                uri += "?busqueda=" + busqueda;
            }

            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var personas = JsonSerializer.Deserialize<List<Persona>>(jsonContent, _jsonOptions);
                return personas ?? new List<Persona>();
            }
            else
            {
                throw new HttpRequestException(
                    $"Error al obtener personas: {response.StatusCode} - {response.ReasonPhrase}"
                );
            }
        }
        catch (HttpRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error inesperado al obtener personas: {ex.Message}", ex);
        }
    }

    public async Task<Persona?> GetByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/personas/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var persona = JsonSerializer.Deserialize<Persona>(jsonContent, _jsonOptions);
                return persona;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw new HttpRequestException(
                    $"Error al obtener persona: {response.StatusCode} - {response.ReasonPhrase}"
                );
            }
        }
        catch (HttpRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error inesperado al obtener persona: {ex.Message}", ex);
        }
    }

    public async Task<Persona> CreateAsync(PersonaDTO personaDto)
    {
        try
        {
            var jsonContent = JsonSerializer.Serialize(personaDto, _jsonOptions);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/personas", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var persona = JsonSerializer.Deserialize<Persona>(responseContent, _jsonOptions);
                return persona ?? throw new Exception("Error al deserializar la respuesta");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Error al crear persona: {response.StatusCode} - {response.ReasonPhrase}. Detalle: {errorContent}"
                );
            }
        }
        catch (HttpRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error inesperado al crear persona: {ex.Message}", ex);
        }
    }

    public async Task<Persona> UpdateAsync(int id, PersonaDTO personaDto)
    {
        try
        {
            var jsonContent = JsonSerializer.Serialize(personaDto, _jsonOptions);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/personas/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var persona = JsonSerializer.Deserialize<Persona>(responseContent, _jsonOptions);
                return persona ?? throw new Exception("Error al deserializar la respuesta");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Error al actualizar persona: {response.StatusCode} - {response.ReasonPhrase}. Detalle: {errorContent}"
                );
            }
        }
        catch (HttpRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error inesperado al actualizar persona: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/personas/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Error al eliminar persona: {response.StatusCode} - {response.ReasonPhrase}. Detalle: {errorContent}"
                );
            }
        }
        catch (HttpRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error inesperado al eliminar persona: {ex.Message}", ex);
        }
    }
}

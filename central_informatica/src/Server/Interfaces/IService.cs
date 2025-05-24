using System.Text;

namespace central_informatica.src.Server.Interfaces;

public interface IService<T, D>
    where T : class
    where D : class
{
    Task<List<T>> GetAll(string busqueda);
    Task<T?> GetById(int id);
    Task<T> Add(D dto);
    Task<T> Update(int id, D dto);
    Task<T> Delete(int id);
}

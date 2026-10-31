using ProductosApi.Models;

namespace ProductosApi.Data;

public interface IProductoBusqueda : IProductoRepository
{
    List<Producto> Buscar(string texto);
}
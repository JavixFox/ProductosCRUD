using Microsoft.AspNetCore.Mvc;
using ProductosApi.Data;
using ProductosApi.Models;

namespace ProductosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoBusqueda _repo;

    public ProductosController(IProductoBusqueda repo) => _repo = repo;

    // GET api/productos
    [HttpGet]
    public ActionResult<List<Producto>> Get() => Ok(_repo.ObtenerTodos());

    // GET api/productos/5
    [HttpGet("{id:int}")]
    public ActionResult<Producto> GetById(int id)
    {
        var producto = _repo.ObtenerPorId(id);
        return producto is null
            ? NotFound(new { mensaje = $"No existe el producto con Id {id}" })
            : Ok(producto);
    }

    // GET api/productos/buscar?texto=mouse
    [HttpGet("buscar")]
    public ActionResult<List<Producto>> Buscar([FromQuery] string texto)
        => Ok(_repo.Buscar(texto ?? string.Empty));

    // POST api/productos
    [HttpPost]
    public ActionResult<Producto> Post([FromBody] Producto producto)
    {
        producto.Id = _repo.Insertar(producto);
        return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
    }

    // PUT api/productos/5
    [HttpPut("{id:int}")]
    public IActionResult Put(int id, [FromBody] Producto producto)
    {
        producto.Id = id;
        return _repo.Actualizar(producto)
            ? NoContent()
            : NotFound(new { mensaje = $"No existe el producto con Id {id}" });
    }

    // DELETE api/productos/5
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
        => _repo.Eliminar(id)
            ? NoContent()
            : NotFound(new { mensaje = $"No existe el producto con Id {id}" });
}
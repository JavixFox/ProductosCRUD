using MySqlConnector;
using ProductosApi.Models;

namespace ProductosApi.Data;

public class ProductoRepository : IProductoBusqueda
{
    private readonly string _connectionString;

    public ProductoRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Falta la cadena de conexión 'Default'.");
    }

    // ---------- CONEXIÓN ----------
    private MySqlConnection CrearConexion() => new MySqlConnection(_connectionString);

    // ---------- CONSULTAS ----------
    public List<Producto> ObtenerTodos()
    {
        var lista = new List<Producto>();
        const string sql = @"SELECT Id, Nombre, Descripcion, Precio, Stock, Categoria
                            FROM Productos ORDER BY Id";

        using var cn = CrearConexion();
        using var cmd = new MySqlCommand(sql, cn);
        cn.Open();
        using var dr = cmd.ExecuteReader();
        while (dr.Read()) lista.Add(Mapear(dr));
        return lista;
    }

    public Producto? ObtenerPorId(int id)
    {
        const string sql = @"SELECT Id, Nombre, Descripcion, Precio, Stock, Categoria
                            FROM Productos WHERE Id = @Id";

        using var cn = CrearConexion();
        using var cmd = new MySqlCommand(sql, cn);
        cmd.Parameters.AddWithValue("@Id", id);
        cn.Open();
        using var dr = cmd.ExecuteReader();
        return dr.Read() ? Mapear(dr) : null;
    }

    // ---------- INSERT ----------
    public int Insertar(Producto p)
    {
        const string sql = @"
            INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, Categoria)
            VALUES (@Nombre, @Descripcion, @Precio, @Stock, @Categoria);
            SELECT LAST_INSERT_ID();";

        using var cn = CrearConexion();
        using var cmd = new MySqlCommand(sql, cn);
        AgregarParametros(cmd, p);
        cn.Open();
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    // ---------- ACTUALIZAR ----------
    public bool Actualizar(Producto p)
    {
        const string sql = @"
            UPDATE Productos
            SET Nombre = @Nombre, Descripcion = @Descripcion, Precio = @Precio,
                Stock = @Stock, Categoria = @Categoria
            WHERE Id = @Id";

        using var cn = CrearConexion();
        using var cmd = new MySqlCommand(sql, cn);
        AgregarParametros(cmd, p);
        cmd.Parameters.AddWithValue("@Id", p.Id);
        cn.Open();
        return cmd.ExecuteNonQuery() > 0;
    }

    // ---------- ELIMINAR ----------
    public bool Eliminar(int id)
    {
        const string sql = "DELETE FROM Productos WHERE Id = @Id";

        using var cn = CrearConexion();
        using var cmd = new MySqlCommand(sql, cn);
        cmd.Parameters.AddWithValue("@Id", id);
        cn.Open();
        return cmd.ExecuteNonQuery() > 0;
    }

    // ---------- BUSCAR ----------
    public List<Producto> Buscar(string texto)
    {
        var lista = new List<Producto>();
        const string sql = @"
            SELECT Id, Nombre, Descripcion, Precio, Stock, Categoria
            FROM Productos
            WHERE Nombre LIKE @Texto OR Descripcion LIKE @Texto OR Categoria LIKE @Texto
            ORDER BY Nombre";

        using var cn = CrearConexion();
        using var cmd = new MySqlCommand(sql, cn);
        cmd.Parameters.AddWithValue("@Texto", $"%{texto}%");
        cn.Open();
        using var dr = cmd.ExecuteReader();
        while (dr.Read()) lista.Add(Mapear(dr));
        return lista;
    }

    // ---------- AUXILIARES ----------
    private static Producto Mapear(MySqlDataReader dr) => new()
    {
        Id          = dr.GetInt32(0),
        Nombre      = dr.GetString(1),
        Descripcion = dr.IsDBNull(2) ? null : dr.GetString(2),
        Precio      = dr.GetDecimal(3),
        Stock       = dr.GetInt32(4),
        Categoria   = dr.IsDBNull(5) ? null : dr.GetString(5)
    };

    private static void AgregarParametros(MySqlCommand cmd, Producto p)
    {
        cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@Descripcion", (object?)p.Descripcion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Precio", p.Precio);
        cmd.Parameters.AddWithValue("@Stock", p.Stock);
        cmd.Parameters.AddWithValue("@Categoria", (object?)p.Categoria ?? DBNull.Value);
    }
}
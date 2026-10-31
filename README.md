# Práctica: Acceso a base de datos con ADO.NET

Backend CRUD de productos con .NET 8, ADO.NET y MySQL, probado con Swagger.

**Alumno:** José Javier Vizcaino Márquez
**Matrícula:** UP230159
**Grupo:** ISC09A

## Requisitos
- .NET 8 SDK
- MySQL 8 

## Cómo ejecutarlo
1. Ejecutar `script.sql` en MySQL.
2. Ajustar la contraseña en `ProductosApi/appsettings.json`.
3. `cd ProductosApi && dotnet run`
4. Abrir `/swagger` en la URL que muestre la consola.

## Puntos cubiertos
| Punto | Implementación |
|---|---|
| Conexión | `CrearConexion()` en `ProductoRepository` |
| Consultas | `ObtenerTodos()`, `ObtenerPorId()` |
| Insert | `Insertar()` → POST /api/productos |
| Actualizar | `Actualizar()` → PUT /api/productos/{id} |
| Eliminar | `Eliminar()` → DELETE /api/productos/{id} |
| Buscar | `Buscar()` → GET /api/productos/buscar?texto= |

using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTiposCuntas
    {
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Borrar(int Id);
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string Nombre, int UsuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int UsuarioId);
        Task<TipoCuenta> ObtenerporId(int id, int UsuarioId);
    }

    public class RepositorioTiposCuentas : IRepositorioTiposCuntas
    {
        private readonly string cs;

        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            cs = configuration.GetConnectionString("DB");
        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var cn = new SqlConnection(cs);

            var id = await cn.QuerySingleAsync<int>($@"INSERT INTO TiposCuentas(Nombre, UsuarioId, Orden)
                                                       VALUES (@Nombre, @UsuarioId, 0);
                                                       SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;
        }

        public async Task<bool> Existe(string Nombre, int UsuarioId)
        {
            using var cn = new SqlConnection(cs);

            var existe = await cn.QueryFirstOrDefaultAsync<int>(@"SELECT 1
                                                                  FROM TiposCuentas
                                                                  WHERE Nombre = @Nombre and UsuarioId = @UsuarioId;", 
                                                                  new {Nombre,UsuarioId});
            return existe == 1;
        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(int UsuarioId)
        {
            using var cn = new SqlConnection(cs);

            return await cn.QueryAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden
                                                     FROM TiposCuentas
                                                     WHERE UsuarioId = @UsuarioId", new {UsuarioId});
        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var cn = new SqlConnection(cs);

            await cn.ExecuteAsync(@"UPDATE TiposCuentas
                                    SET Nombre = @Nombre
                                    WHERE Id = @Id", tipoCuenta);
        }

        public async Task<TipoCuenta> ObtenerporId(int id, int UsuarioId)
        {
            using var cn = new SqlConnection(cs);

            return await cn.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden
                                                                   FROM TiposCuentas
                                                                   WHERE Id = @Id AND UsuarioId = @UsuarioId", new { id, UsuarioId });
        }

        public async Task Borrar(int Id)
        {
            using var cn = new SqlConnection(cs);

            await cn.ExecuteAsync(@"DELETE TiposCuentas WHERE Id = @Id", new { Id }); 
        }
    }
}

using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioCuentas
    {
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
        Task Crear(Cuenta cuenta);
    }
    public class RepositorioCuentas: IRepositorioCuentas
    {
        private readonly string _connectionString;
        public RepositorioCuentas(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Cuenta cuenta)
        {
            using var conn = new SqlConnection(_connectionString);
            var id = await conn.QuerySingleAsync<int>(
                @"INSERT INTO Cuentas (Nombre, TipoCuentaId,Descripcion,Balance) VALUES
                (@Nombre,@TipoCuentaId,@Descripcion,@Balance);", 6);

               // SELECT SCOPE_INDENTITY();", cuenta);
            cuenta.Id = id;
        }

        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<Cuenta>(@"SELECT Cuentas.Id, Cuentas.Nombre, Balance, tc.Nombre as TipoCuenta
                         FROM Cuentas INNER JOIN TiposCuentas tc ON tc.Id = Cuentas.TipoCuentaId
                         WHERE tc.UsuarioId = @UsuarioId
                         ORDER BY tc.Orden", new { usuarioId});
        }
    }
}

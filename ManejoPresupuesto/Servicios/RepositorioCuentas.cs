using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioCuentas
    {
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
                (@Nombre,@TipoCuentaId,@Descripcion,@Balance
                SELECT SCOPE_INDENTITY();", cuenta);
            cuenta.Id = id;
        }
    }
}

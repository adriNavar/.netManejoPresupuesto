using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace ManejoPresupuesto.Servicios
{

    public interface IRepositorioTiposCuentas
    {
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Borrar(int id);
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
        Task Ordenar(IEnumerable<TipoCuenta> tiposCuentasOrdenados);
    }
    public class RepositorioTipoCuentas: IRepositorioTiposCuentas
    {
        private readonly string connectionString;
        public RepositorioTipoCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
         }


       

        public async Task Crear (TipoCuenta tipoCuenta) // para hacer esto asincrono se usa async
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                ("TiposCuentas_Insertar",
                new {usuarioId=tipoCuenta.usuarioId,
                nombre=tipoCuenta.nombre},
                commandType:System.Data.CommandType.StoredProcedure);// commandType aclara que lo que estoy usando es un procedimento almacenado
            tipoCuenta.id = id;
                                                        
        }


        public async Task<Boolean> Existe(string nombre, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>
                ("select 1 from tiposCuentas where nombre=@nombre and usuarioId=@usuarioId;", // Siempre va el @ cuando igualo campos de la db con los parametros
                new { nombre, usuarioId });
            return existe == 1;
        }

        public async Task <IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>
                (@"select id,nombre,orden from tiposCuentas where usuarioId=@usuarioId order by orden",
                new {usuarioId});
        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE tiposCuentas 
                                          set nombre = @nombre
                                            where id= @id",tipoCuenta); // Execute sirve cuando no necesitamos retornar nada en este caso solo deseeo modificar
        }

        public async Task<TipoCuenta> ObtenerPorId (int id , int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"select id,nombre,orden 
                                                                        from tiposCuentas
                                                                        where id=@id and usuarioId=@usuarioId",
                                                                        new { id, usuarioId });
        }


        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"delete tiposCuentas 
                                          where id= @id", new { id }); // Execute sirve cuando no necesitamos retornar nada en este caso solo deseeo modificar
        }
        public async Task Ordenar (IEnumerable<TipoCuenta> tiposCuentasOrdenados)
        {
            var query = "UPDATE TiposCuentas SET orden=@Orden where id=@id";
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query,tiposCuentasOrdenados);
        }
    }
}

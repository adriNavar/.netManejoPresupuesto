using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioCuentas{
        Task Crear(Cuenta cuenta);
       Task<IEnumerable<Cuenta>>Buscar(int usuarioId);
       Task<Cuenta> ObtenerPorId(int id, int usuarioId);
       Task Actualizar(CuentaCreacionViewModel cuenta);
       Task Borrar(int id);
    }
    
    public class RepositorioCuentas:IRepositorioCuentas
    {
        private readonly string connectionString;


        public RepositorioCuentas(IConfiguration configuration) // el Iconfiguration me sirver para extrar el connection string
        {
            connectionString=configuration.GetConnectionString("DefaultConnection");
        }

         public async Task Crear (Cuenta cuenta) // para hacer esto asincrono se usa async
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"insert into cuentas (nombre,tipoCuentaId,descripcion,balance)
                                                       values(@nombre,@tipoCuentaId,@descripcion,@balance); 
                                                       select scope_identity();",cuenta);
              cuenta.id = id;
                                                        
        }

        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId) {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cuenta>(@"select cuentas.id,cuentas.nombre,balance,tc.nombre AS tipoCuenta
                                                       from cuentas
                                                       inner join  tiposCuentas tc
                                                       on tc.id= cuentas.tipoCuentaId
                                                       where tc.usuarioId=@usuarioId
                                                       order by tc.orden", new {usuarioId});

        }


        public async Task<Cuenta> ObtenerPorId(int id, int usuarioId)  {
        
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cuenta>(@"select cuentas.id,cuentas.nombre,balance,descripcion,tc.id
                                                       from cuentas
                                                       inner join  tiposCuentas tc
                                                       on tc.id= cuentas.tipoCuentaId
                                                       where tc.usuarioId=@usuarioId and cuentas.id=@id",
                                                        new { id, usuarioId });
         }


        public async Task Actualizar(CuentaCreacionViewModel cuenta){
             using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"update cuentas set nombre=@nombre,
                    balance=@balance,descripcion=@descripcion,tipoCuentaId=@tipoCuentaId
                     where id=@id;",cuenta);      
                                               
        }

        public async Task Borrar(int id){
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"delete cuentas where id=@id;",new {id});                                                     
        }
    }

}
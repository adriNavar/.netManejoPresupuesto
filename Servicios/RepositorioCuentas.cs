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
            return await connection.QueryAsync<Cuenta>(@"select c.id,c.nombre,c.balance,tc.nombre 
                                                       from cuentas c
                                                       inner join  tiposCuentas tc
                                                       on tc.id=c.tipoCuentaId
                                                       where tc.usuarioId=@usuarioId
                                                       order by tc.orden",new {usuarioId});

        }
        
    }
}
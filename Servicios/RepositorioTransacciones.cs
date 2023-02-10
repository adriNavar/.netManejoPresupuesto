using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{

     public interface IRepositorioTransacciones
    {
    Task Crear (Transaccion transaccion);
    }
    public class RepositorioTransacciones : IRepositorioTransacciones

    {   
        private readonly string connectionString;

        public RepositorioTransacciones(IConfiguration configuration){
           connectionString = configuration.GetConnectionString("DefaultConnection");
        }

           public async Task Crear (Transaccion transaccion) // para hacer esto asincrono se usa async
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>("Transacciones_Insertar", 
            new{transaccion.usuarioId, transaccion.fechaTransaccion, transaccion.monto,
            transaccion.categoriaId,transaccion.cuentaId,transaccion.nota},
            commandType:System.Data.CommandType.StoredProcedure);
              transaccion.id = id;
                                                        
        }
        
    }
}
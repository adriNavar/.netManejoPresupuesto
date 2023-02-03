using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{   
    public interface IRepositorioCategorias
    {
        Task Crear (Categoria categoria);
    }
    public class RepositorioCategorias:IRepositorioCategorias
    {
          private readonly string connectionString;
        public RepositorioCategorias(IConfiguration configuration)
        {
            connectionString=configuration.GetConnectionString("DefaultConnexion");
        }
        
        public async Task Crear (Categoria categoria){
              using var connection = new SqlConnection(connectionString);
              var id = await connection.QuerySingleAsync<int>(@"insert into categorias 
              (nombre,tipoOperacionId,usuarioId)values(@nombre,@tipoOperacionId,@usuarioId); 
              select scope_identity();",categoria);
              categoria.id=id;
        }
      
    }
}
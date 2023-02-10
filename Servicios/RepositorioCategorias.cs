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
        Task<IEnumerable<Categoria>> Obtener(int usuarioId);
        Task<Categoria> ObtenerPorId(int id, int usuarioId);
        Task Actualizar(Categoria categoria);
        Task Borrar(int id);
    }
    public class RepositorioCategorias:IRepositorioCategorias
    {
          private readonly string connectionString;
        public RepositorioCategorias(IConfiguration configuration)
        {
           connectionString=configuration.GetConnectionString("DefaultConnection");;
        }
        
        public async Task Crear (Categoria categoria){
              using var connection = new SqlConnection(connectionString);
              var id = await connection.QuerySingleAsync<int>(@"insert into categorias 
              (nombre,tipoOperacionId,usuarioId)values(@nombre,@tipoOperacionId,@usuarioId); 
              select scope_identity();",categoria);
              categoria.id=id;
        }

      public async Task<IEnumerable<Categoria>> Obtener(int usuarioId){
        using var connection = new SqlConnection(connectionString);
        return await connection.QueryAsync<Categoria>("select * from categorias where usuarioId=@usuarioId", new {usuarioId});
        }

            public async Task<Categoria> ObtenerPorId(int id, int usuarioId)  {
        
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Categoria>(@"select * from categorias
                                                                       where id=@id and usuarioId=@usuarioId", new {id, usuarioId});
         }
           public async Task Actualizar(Categoria categoria){
             using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"update categorias set nombre=@nombre,
                                        tipoOperacionId=@tipoOperacionId
                                        where id=@id;",categoria); 
                                   
        }

            public async Task Borrar(int id){
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"delete categorias where id=@id;",new {id});                                                     
        }
    }

}
    

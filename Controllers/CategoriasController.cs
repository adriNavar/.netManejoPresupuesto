using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly IRepositorioCategorias repositorioCategorias;
        public CategoriasController(IRepositorioCategorias repositorioCategorias,
        IServicioUsuarios servicioUsuarios)
        {
            this.repositorioCategorias = repositorioCategorias;
             this.servicioUsuarios=servicioUsuarios;
        }
        
        [HttpGet]
        public IActionResult Crear(){
           
             return View();
            
        }

        [HttpPost]
      public async Task<IActionResult> Crear(Categoria categoria)
      {     
        if (!ModelState.IsValid){
                return View(categoria);
                
            }
         var usuarioId = servicioUsuarios.ObtenerUsuarioId();
        categoria.usuarioId = usuarioId;   
         await repositorioCategorias.Crear(categoria);
         return RedirectToAction("Index");  
       }
    }
}
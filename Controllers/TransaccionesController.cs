using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Controllers
{
    public class TransaccionesController: Controller
    {
        private readonly IServicioUsuarios servicioUsuarios;
         private readonly IRepositorioCuentas repositorioCuentas;
        public TransaccionesController(IServicioUsuarios servicioUsuarios,
        
       IRepositorioCuentas repositorioCuentas)
        {
            this.repositorioCuentas = repositorioCuentas;
            this.servicioUsuarios = servicioUsuarios;
            
        }

        public async Task <IActionResult> Crear(){
            var usuarioId= servicioUsuarios.ObtenerUsuarioId();
            var modelo= new TransaccionCreacionViewModel();
            modelo.cuentas= await ObtenerCuenta(usuarioId);
            return View(modelo);
            
        }
        
        private async Task <IEnumerable<SelectListItem>> ObtenerCuenta(int usuarioId){
            var  cuentas = await repositorioCuentas.Buscar(usuarioId);
            return cuentas.Select(x => new SelectListItem (x.nombre,x.id.ToString()));
        }
    }
}
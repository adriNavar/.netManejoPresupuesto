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
    public class CuentasController:Controller
    {
        
    
        private readonly IServicioUsuarios servicioUsuarios;
        private readonly  IRepositorioTiposCuentas repositorioTiposCuentas ;
        private readonly  IRepositorioCuentas repositorioCuentas ;
        public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas,
        IServicioUsuarios servicioUsuarios, IRepositorioCuentas repositorioCuentas)
        {        
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.servicioUsuarios=servicioUsuarios;
            this.repositorioCuentas = repositorioCuentas;
        }

        public async Task<IActionResult> Index(){
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var cuentasConTiposCuentas=await repositorioCuentas.Buscar(usuarioId);
            var modelo=cuentasConTiposCuentas
            .GroupBy(x=> x.tipoCuenta)
            .Select(grupo => new IndiceCuentasViewModel{
                tipoCuenta=grupo.Key,
                cuentas=grupo.AsEnumerable()
            }).ToList() ;
            return View(modelo);
            
        }
        
        [HttpGet]
        public async Task <IActionResult> Crear(){
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var modelo= new CuentaCreacionViewModel();
            modelo.TiposCuentas = await ObternerTiposCuentas(usuarioId);

             return View(modelo);
            
        }

        [HttpPost]
               public async Task<IActionResult> Crear(CuentaCreacionViewModel cuenta)
        {
        
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(cuenta.tipoCuentaId,usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado","Home");
            }
            if (!ModelState.IsValid){
                cuenta.TiposCuentas = await ObternerTiposCuentas(usuarioId);
                return View(cuenta);
                
            }
                      
            await repositorioCuentas.Crear(cuenta);
            return RedirectToAction("Index");
            
        }

        private async Task<IEnumerable<SelectListItem>> ObternerTiposCuentas(int usuarioId){
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
            return tiposCuentas.Select(x => new SelectListItem(x.nombre, x.id.ToString()));
           
        }


    }
}
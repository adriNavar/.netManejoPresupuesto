using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        private readonly IMapper mapper;
        public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas,
        IServicioUsuarios servicioUsuarios, IRepositorioCuentas repositorioCuentas,IMapper mapper)

        {        
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.servicioUsuarios=servicioUsuarios;
            this.repositorioCuentas = repositorioCuentas;
            this.mapper = mapper;
            
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
            modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);

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
                cuenta.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
                return View(cuenta);
                
            }
                      
            await repositorioCuentas.Crear(cuenta);
            return RedirectToAction("Index");
            
        }

        public async Task<IActionResult> Editar(int id){
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var cuenta =await repositorioCuentas.ObtenerPorId(id,usuarioId);

            if(cuenta is null){
                return RedirectToAction("NoEncontrado","Home");
                
            }

            var modelo= mapper.Map<CuentaCreacionViewModel>(cuenta);

            
           modelo.TiposCuentas=await ObtenerTiposCuentas(usuarioId);
           return View(modelo);         

        }

        
    [HttpPost]
    public async Task<IActionResult> Editar(CuentaCreacionViewModel cuentaEditar){
         var usuarioId = servicioUsuarios.ObtenerUsuarioId();
         var cuenta=await repositorioCuentas.ObtenerPorId(cuentaEditar.id,usuarioId);
         if(cuenta is null){
            return RedirectToAction("NoEncontrado", "Home");
         }
         var tipoCuenta= await repositorioTiposCuentas.ObtenerPorId(cuentaEditar.tipoCuentaId,usuarioId);
         if (tipoCuenta is null){
            return RedirectToAction("NoEncontrado", "Home");
         }
            await repositorioCuentas.Actualizar(cuentaEditar);
            return RedirectToAction("Index");

    }


        [HttpGet]
        public async Task<IActionResult> Borrar(int id){
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var cuenta=await repositorioCuentas.ObtenerPorId(id,usuarioId);
            if(cuenta is null){
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(cuenta);          
        }

        [HttpPost]
         public async Task<IActionResult> BorrarCuenta(int id){
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var cuenta=await repositorioCuentas.ObtenerPorId(id,usuarioId);
            if(cuenta is null){
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioCuentas.Borrar(id);
            return RedirectToAction("Index");
                     
         }
        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId){
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
            return tiposCuentas.Select(x => new SelectListItem(x.nombre, x.id.ToString()));
           
        }


    }
}
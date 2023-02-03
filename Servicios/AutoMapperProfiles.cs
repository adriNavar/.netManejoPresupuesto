using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Servicios
{
    public class AutoMapperProfiles : Profile

    {
        public AutoMapperProfiles(){
            CreateMap<Cuenta,CuentaCreacionViewModel>();
        }  
    }
}
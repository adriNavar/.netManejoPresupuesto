using System;
using System.Collections.Generic;
using System.Linq;
using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Models
{
    public class IndiceCuentasViewModel
    {
        public string  tipoCuenta { get; set; }
        
        public IEnumerable<Cuenta> cuentas { get; set; }
        
        public decimal balance => cuentas.Sum(x=> x.balance);

    }
}
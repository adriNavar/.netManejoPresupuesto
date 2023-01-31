using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ManejoPresupuesto.Validaciones;
using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Models
{
    public class Cuenta
    {
        public int id { get; set; }
        [Display(Name = "Nombre")]
         [Required(ErrorMessage = "El campo {0} nombre es requerido")]
         [StringLength(maximumLength:50)]
         [PrimeraLetraMayuscula]
        public string nombre { get; set; }
        [Display(Name = "Tipo de cuenta")]
        public int tipoCuentaId { get; set;}
        [Display(Name = "Balance")]
        public decimal balance { get; set; }
        [StringLength(maximumLength:1000)]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }

        public string tipoCuenta { get; set; }
        
        
    }
}
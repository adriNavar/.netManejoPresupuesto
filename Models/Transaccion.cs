using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ManejoPresupuesto.Models
{
    public class Transaccion
    {       
        [Display(Name = "Id")]
        public int id { get; set; }
        [Display(Name = "Usuario")]
        public int usuarioId { get; set; }
        [Display(Name = "Fecha Transaccion")]
        [DataType(DataType.Date)]

        public DateTime fechaTransaccion { get; set; } = DateTime.Today;
        
        [Display(Name = "Monto")]     
        public decimal monto { get; set; } 
        [Range(1,maximum:int.MaxValue, ErrorMessage = "Debe seleccionar una categoria")]
        [Display(Name = "Categoria")]
        public int categoriaId { get; set; }
        [StringLength(maximumLength:1000, ErrorMessage ="La nota no debe pasar de {1} caracteres")]
        [Display(Name = "Nota")]
        public string nota { get; set; }
        [Range(1,maximum:int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta")]
        [Display(Name = "Cuenta")]
        public int cuentaId { get; set; }
    }
}
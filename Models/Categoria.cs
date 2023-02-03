using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ManejoPresupuesto.Models
{
    public class Categoria
    {
        public int id { get; set; }
         [Display(Name = "Nombre")]
         [Required (ErrorMessage = "El campo {0} nombre es requerido")]
         [StringLength(maximumLength:50,ErrorMessage ="No puede ser mayor a {1} caracteres")]
        public string nombre { get; set; }
        [Display(Name = "Tipo de Operacion")]
        public  TipoOperacion tipoOperacionId { get; set; }

        public int usuarioId { get; set; }
      }
}
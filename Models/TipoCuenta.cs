using ManejoPresupuesto.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta : IValidatableObject
    {
        public int id { get; set; }
        [Display(Name = "Nombre del tipo de cuenta")]
        [Required(ErrorMessage = "El campo {0} nombre es requerido")]
        //[PrimeraLetraMayuscula]
        [Remote(action: "VerificarExisteTipoCuenta", controller: "TiposCuentas")]
        public string nombre { get; set; }
        [Required]
        
        public int usuarioId { get; set; }
        [Required]
        public int orden { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            {
                if (nombre != null && nombre.Length > 0)
                {
                    var primeraLetra = nombre[0].ToString();

                    if (primeraLetra != primeraLetra.ToUpper())
                    {
                        yield return new ValidationResult("La primera letra debe ser en mayusucla", new[]
                        {nameof(nombre)});
                    }
                }
            }

        }
    }
 }


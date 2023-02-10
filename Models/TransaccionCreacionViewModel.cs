using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Models
{
    public class TransaccionCreacionViewModel: Transaccion
    {
        public IEnumerable<SelectListItem> cuentas{ get; set;}
        public IEnumerable<SelectListItem>categorias { get; set; }

        public TipoOperacion tipoOperacionId{ get; set; }
    }
}
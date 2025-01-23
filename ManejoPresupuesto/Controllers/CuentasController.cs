using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Controllers
{
    public class CuentasController: Controller
    {
        private readonly IServicioUsuarios _servicioUsuarios;
        private readonly IRepositorioTiposCuentas _repositorioTiposCuentas;
        public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas,
            IServicioUsuarios servicioUsuarios)
        {
            _servicioUsuarios = servicioUsuarios;
            _repositorioTiposCuentas = repositorioTiposCuentas;
        }

       

        [HttpGet]
        public async IActionResult Crear()
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await _repositorioTiposCuentas.Obtener(usuarioId);
            var modelo = new CuentaCreacionViewModel();
            modelo.TiposCuentas = tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
            return View(modelo);
        }
    }
}

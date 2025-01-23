using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        //private readonly string connectionString;
        private readonly IRepositorioTiposCuentas _repositorioTiposCuentas;
        private readonly IServicioUsuarios _servicioUsuarios;

        public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas,
             IServicioUsuarios servicioUsuarios)
        //public TiposCuentasController(IConfiguration configuration)
        {
            //connectionString = configuration.GetConnectionString("DefaultConnection");
            _repositorioTiposCuentas = repositorioTiposCuentas;
            _servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await _repositorioTiposCuentas.Obtener(usuarioId);
            return View(tiposCuentas);
        }

        public IActionResult Crear()
        {
            //using (var connection = new SqlConnection(connectionString))
            //{
            //    var query = connection.Query("SELECT 1").FirstOrDefault();
            //}
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            tipoCuenta.UsuarioId = _servicioUsuarios.ObtenerUsuarioId(); ;

            var yaExisteTipoCuent = await _repositorioTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);
            if (yaExisteTipoCuent)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre),
                    $"El nombre {tipoCuenta.Nombre} ya existe.");
                return View(tipoCuenta);
            }
            await _repositorioTiposCuentas.Crear(tipoCuenta);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> Editar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await _repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("No Encontrado", "Home");
            }
            await _repositorioTiposCuentas.Actualizar(tipoCuenta);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<ActionResult> Editar(TipoCuenta tipoCuenta)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuentaExiste = await _repositorioTiposCuentas.ObtenerPorId(tipoCuenta.Id, usuarioId);
            if (tipoCuenta is null)
            {
                return RedirectToAction("No Encontrado", "Home");
            }
            return View(tipoCuenta);
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId(); ;
            var yaExisteTipoCuenta = await _repositorioTiposCuentas.Existe(nombre, usuarioId);

            if (yaExisteTipoCuenta)
            {
                return Json($"El Nombre {nombre} ya existe");
            }
            return Json(true);
        }

        //[HttpPost]
        public async Task<ActionResult> Borrar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await _repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("No Encontrado", "Home");
            }
            return View(tipoCuenta);
            //await _repositorioTiposCuentas.Actualizar(tipoCuenta);
            //return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> BorrarTipoCuenta(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await _repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("No Encontrado", "Home");
            }

            await _repositorioTiposCuentas.Borrar(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
            return Ok();

        }
    }
}

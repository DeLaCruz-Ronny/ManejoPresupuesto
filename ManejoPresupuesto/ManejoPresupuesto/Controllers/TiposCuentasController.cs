using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly IRepositorioTiposCuntas repositorioTiposCuntas;
        private readonly IServicioUsuarios servicioUsuarios;

        public TiposCuentasController(IRepositorioTiposCuntas repositorioTiposCuntas,
                IServicioUsuarios servicioUsuarios)
        {
            this.repositorioTiposCuntas = repositorioTiposCuntas;
            this.servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            var UsuarioId = servicioUsuarios.ObtenerUsuariosId();
            var tiposCuentas = await repositorioTiposCuntas.Obtener(UsuarioId);
            return View(tiposCuentas);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            tipoCuenta.UsuarioId = servicioUsuarios.ObtenerUsuariosId();

            var YaExiste = await repositorioTiposCuntas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (YaExiste)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre),
                    $"El nombre {tipoCuenta.Nombre} ya existe.");
                return View(tipoCuenta);
            }

            await repositorioTiposCuntas.Crear(tipoCuenta);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int Id)
        {
            var UsuarioId = servicioUsuarios.ObtenerUsuariosId();
            var tiposCuentas = await repositorioTiposCuntas.ObtenerporId(Id, UsuarioId);

            if (tiposCuentas is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tiposCuentas);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(TipoCuenta tipoCuenta)
        {
            var UsuarioId = servicioUsuarios.ObtenerUsuariosId();
            var existeTipoCuenta = await repositorioTiposCuntas.ObtenerporId(tipoCuenta.Id, UsuarioId);

            if (existeTipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioTiposCuntas.Actualizar(tipoCuenta);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Borrar(int Id)
        {
            var UsuarioId = servicioUsuarios.ObtenerUsuariosId();
            var tiposCuentas = await repositorioTiposCuntas.ObtenerporId(Id, UsuarioId);

            if (tiposCuentas is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(tiposCuentas);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarTipoCuenta(int Id)
        {
            var UsuarioId = servicioUsuarios.ObtenerUsuariosId();
            var tiposCuentas = await repositorioTiposCuntas.ObtenerporId(Id, UsuarioId);

            if (tiposCuentas is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioTiposCuntas.Borrar(Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
            return Ok();
        }
    }
}

using EFood.AccesoDatos.Repositorio;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin + "," + DS.Rol_Mantemiento + "," + DS.Rol_Consulta)]
    public class TipoPrecioController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public TipoPrecioController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }


        public IActionResult Index()
        {
            return View();
        }

        //Es un get por defecto
        public async Task<IActionResult> Upsert(int? id)
        {
            TipoPrecio tipoPrecio = new TipoPrecio();

            if (id == null)
            {
                //Crear nueva Linea de Comida

                return View(tipoPrecio);
            }
            //Actualizar Linea de Comida
            tipoPrecio = await _unidadTrabajo.TipoPrecio.Obtener(id.GetValueOrDefault());
            if (tipoPrecio == null)
            {
                return NotFound();
            }
            return View(tipoPrecio);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TipoPrecio tipoPrecio)
        {

            if (ModelState.IsValid && -100 <= tipoPrecio.Cambio && tipoPrecio.Cambio <= 100)
            {
                // Obtiene el nombre del usuario actualmente logueado
                var usuarioNombre = User.Identity.Name;

                if (tipoPrecio.Id == 0)
                {
                    await _unidadTrabajo.TipoPrecio.Agregar(tipoPrecio);
                    await _unidadTrabajo.Guardar();

                    // Ahora que la línea de comida se ha agregado a la base de datos, obtenemos su ID real
                    var idRegistro = tipoPrecio.Id;

                    TempData[DS.Exitosa] = "Tipo Precio creado exitosamente";

                    // Registra en la bitácora
                    await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, idRegistro.ToString(), $"Se insertó el tipo de precio '{tipoPrecio.Descripcion}' con ID: {idRegistro}");
                }
                else
                {
                    _unidadTrabajo.TipoPrecio.Actualizar(tipoPrecio);
                    TempData[DS.Exitosa] = "Tipo Precio actualizado exitosamente";

                    // Registra en la bitácora
                    await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, tipoPrecio.Id.ToString(), $"Se actualizó el tipo precio '{tipoPrecio.Descripcion}' con ID: {tipoPrecio.Id}");
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            if (tipoPrecio.Cambio < -100 || tipoPrecio.Cambio > 100)
            {
                TempData[DS.Error] = "El cambio porcentual debe estar entre -100% y 100%";
                return View(tipoPrecio);
            }
            TempData[DS.Error] = "Error al guardar el Tipo Precio";
            return View(tipoPrecio);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.TipoPrecio.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuarioNombre = User.Identity.Name;

            var TipoPrecioBD = await _unidadTrabajo.TipoPrecio.Obtener(id);
            if (TipoPrecioBD == null)
            {
                return Json(new { success = false, message = "Error al borrar el Tipo Precio" });
            }
            _unidadTrabajo.TipoPrecio.Remover(TipoPrecioBD);
            await _unidadTrabajo.Guardar();

            await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, TipoPrecioBD.Id.ToString(), $"Se eliminó el tipo precio '{TipoPrecioBD.Descripcion}' con ID: {TipoPrecioBD.Id}");

            return Json(new { success = true, message = "Tipo Precio borrado correctamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.TipoPrecio.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(c => c.Descripcion.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(c => c.Descripcion.ToLower().Trim() == nombre.ToLower().Trim() && c.Id != id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });

        }



        #endregion


    }
}

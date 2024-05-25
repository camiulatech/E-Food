﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos.ViewModels;
using EFood.Modelos;
using EFood.Utilidades;
using EFood.Modelos.CarritoCompras;


namespace EFoodCommerce.Areas.Commerce.Controllers
{
    [Area("Commerce")]
    public class CarritoCompraController : Controller
    {

        private const string SessionKeyCarrito = "Carrito";
        private readonly IUnidadTrabajo _unidadTrabajo;

        public CarritoCompraController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            var carrito = ObtenerCarritoDeSesion();
            return View(carrito);
        }

        public IActionResult Agregar(Producto producto, int cantidad, TipoPrecio tipoPrecio)
        {
            var carrito = ObtenerCarritoDeSesion();
            carrito.AgregarItem(producto, cantidad, tipoPrecio);
            GuardarCarritoEnSesion(carrito);
            return RedirectToAction("Index");
        }

        public IActionResult Remover(Producto producto)
        {
            var carrito = ObtenerCarritoDeSesion();
            carrito.EliminarItem(producto);
            GuardarCarritoEnSesion(carrito);
            return RedirectToAction("Index");
        }

        private CarritoCompra ObtenerCarritoDeSesion()
        {
            var carritoJson = HttpContext.Session.GetString(SessionKeyCarrito);
            return carritoJson == null ? new CarritoCompra() : JsonConvert.DeserializeObject<CarritoCompra>(carritoJson);
        }

        private void GuardarCarritoEnSesion(Carrito carrito)
        {
            var carritoJson = JsonConvert.SerializeObject(carrito);
            HttpContext.Session.SetString(SessionKeyCarrito, carritoJson);
        }

        private Producto ObtenerProductoPorId(int productoId)
        {
            // Implementa este método para obtener el producto de tu base de datos
            return new Producto { Id = productoId, Nombre = "Producto de ejemplo", Precio = 10.0m };
        }


        #region API

        #endregion
    }
}

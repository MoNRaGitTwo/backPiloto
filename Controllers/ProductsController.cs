using DemoPilotoV1.BDD;
using DemoPilotoV1.Clases;
using DemoPilotoV1.DTOS;
using DemoPilotoV1.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DemoPilotoV1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly BaseDeDatos _context;
        private readonly RepoProductos _repoProducto;

        public ProductsController(BaseDeDatos context)
        {
            _context = context;
            _repoProducto = new RepoProductos(context);
        }

        







        [HttpPost("crear")]
        [SwaggerOperation("Crea un nuevo producto")]
        [SwaggerResponse(StatusCodes.Status200OK, "Producto creado exitosamente")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos del producto no válidos")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor")]
        public async Task<IActionResult> CrearProducto([FromForm] ProductoDto nuevoProductoDto)
        {
            try
            {
                if (nuevoProductoDto == null || nuevoProductoDto.ImagenData == null)
                {
                    return BadRequest("Datos del producto o imagen no válidos");
                }

                byte[] imageData;
                using (var memoryStream = new MemoryStream())
                {
                    await nuevoProductoDto.ImagenData.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();
                }

                // Llamar al método GuardarProducto del repositorio
                _repoProducto.GuardarProducto(
                    nuevoProductoDto.Name,
                    nuevoProductoDto.Price,
                    nuevoProductoDto.Stock,
                    imageData,
                    nuevoProductoDto.ImageFileName,
                    nuevoProductoDto.CodigoQR
                );

                return Ok(new { Message = "Producto creado exitosamente" });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }


        [HttpGet("TodoProductos")]
        [SwaggerOperation("Obtiene todos los productos")]
        [SwaggerResponse(StatusCodes.Status200OK, "Productos obtenidos exitosamente")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor")]
        public async Task<IActionResult> ObtenerProductos()
        {
            try
            {
                var productos = await _context.ProductsDos.ToListAsync();

                var productosConImagenes = productos.Select(producto =>
                {
                    string imageData = string.Empty;

                    if (producto.ImageData != null && producto.ImageData.Length > 0)
                    {
                        // Convertir los datos de la imagen a una cadena Base64
                        imageData = Convert.ToBase64String(producto.ImageData);
                    }

                    return new
                    {
                        producto.Id,
                        producto.Name,
                        producto.Price,
                        producto.Stock,
                        ImageData = imageData,
                        producto.ImageFileName,
                        producto.CodigoQR,
                        Proveedores = producto.ProveedorProductos.Select(pp => new
                        {
                            pp.Proveedor.Id,
                            pp.Proveedor.Nombre
                        })
                    };
                }).ToList();

                return Ok(productosConImagenes);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpDelete("Eliminar/{id}")]
        [SwaggerOperation("Elimina un producto por ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Producto eliminado exitosamente")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Producto no encontrado")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor")]
        public IActionResult EliminarProducto(int id)
        {
            try
            {
                var producto = _repoProducto.ObtenerProductoPorId(id);
                if (producto == null)
                {
                    return NotFound("Producto no encontrado");
                }

                _repoProducto.EliminarProducto(id);
                return Ok(new { Message = "Producto eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        [HttpPut("Editar/{id}")]
        [SwaggerOperation("Edita un producto existente")]
        [SwaggerResponse(StatusCodes.Status200OK, "Producto editado exitosamente")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Producto no encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos del producto no válidos")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor")]
        public IActionResult EditarProducto(int id, [FromForm] ProductoDto productoEditadoDto)
        {
            try
            {
                if (productoEditadoDto == null)
                {
                    return BadRequest("Datos del producto no válidos");
                }

                var productoExistente = _repoProducto.ObtenerProductoPorId(id);
                if (productoExistente == null)
                {
                    return NotFound("Producto no encontrado");
                }

                byte[] imageData = null;
                if (productoEditadoDto.ImagenData != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        productoEditadoDto.ImagenData.CopyTo(memoryStream);
                        imageData = memoryStream.ToArray();
                    }
                }

                // Editar el producto en el repositorio
                _repoProducto.EditarProducto(id, productoEditadoDto.Name, productoEditadoDto.Price, productoEditadoDto.Stock, imageData, productoEditadoDto.ImageFileName);

                // Obtener los datos actualizados del producto
                var productoActualizado = _repoProducto.ObtenerProductoPorId(id);

                // Devolver los datos actualizados junto con el mensaje de éxito
                return Ok(new { Message = "Producto editado exitosamente", Product = productoActualizado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("ActualizarStock/{id}")]
        [SwaggerOperation("Edita stock existente")]
        [SwaggerResponse(StatusCodes.Status200OK, "Stock editado exitosamente")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Producto no encontrado")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos del producto no válidos")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor")]
        public IActionResult ActualizarStock(int id, [FromBody] int nuevoStock)
        {
            try
            {
                var productoExistente = _repoProducto.ObtenerProductoPorId(id);
                if (productoExistente == null)
                {
                    return NotFound("Producto no encontrado");
                }

                _repoProducto.ActualizarStock(id, nuevoStock); // Llama a ActualizarStock con los dos parámetros

                productoExistente.Stock = nuevoStock; // Actualiza el stock en el objeto existente

                return Ok(new { Message = "Stock actualizado exitosamente", Product = productoExistente });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // Nuevos métodos

        [HttpPost("AsociarProductoAProveedor")]
        [SwaggerOperation("Asocia un producto a un proveedor")]
        [SwaggerResponse(StatusCodes.Status200OK, "Producto asociado exitosamente")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos no válidos")]
        public async Task<IActionResult> AsociarProductoAProveedor(int proveedorId, int productoId)
        {
            try
            {
                await _repoProducto.AsociarProductoAProveedor(proveedorId, productoId);
                return Ok(new { Message = "Producto asociado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("ReducirStock/{productoId}")]
        [SwaggerOperation("resta el stock del producto")]
        public IActionResult ReducirStock(int productoId, int cantidadComprada)
        {
            try
            {
                var producto = _repoProducto.ObtenerProductoPorId(productoId);
                if (producto == null || producto.Stock < cantidadComprada)
                {
                    return NotFound("Producto no encontrado o stock insuficiente");
                }

                _repoProducto.ActualizarStock(productoId, producto.Stock - cantidadComprada);
                return Ok(new { Message = "Stock actualizado", StockRestante = producto.Stock - cantidadComprada });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("ObtenerProductosPorProveedor/{proveedorId}")]
        [SwaggerOperation("Obtiene productos por proveedor")]
        [SwaggerResponse(StatusCodes.Status200OK, "Productos obtenidos exitosamente")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos no válidos")]
        public async Task<IActionResult> ObtenerProductosPorProveedor(int proveedorId)
        {
            try
            {
                var productos = await _repoProducto.ObtenerProductosPorProveedor(proveedorId);
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}

using DemoPilotoV1.BDD;
using DemoPilotoV1.Clases;
using DemoPilotoV1.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace DemoPilotoV1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientesController : Controller
    {
        private readonly BaseDeDatos _context;
        private readonly RepoClientes _repoClientes;

        public ClientesController(BaseDeDatos context)
        {
            _context = context;
            _repoClientes = new RepoClientes(context);
        }

        [HttpGet("TodosClientes")]
        [SwaggerOperation("Obtiene todos los clientes")]
        [SwaggerResponse(StatusCodes.Status200OK, "Clientes obtenidos exitosamente")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor")]
        public IActionResult ObtenerClientes()
        {
            var clientes = _repoClientes.ObtenerTodosLosClientes();
            return Ok(clientes);
        }


        [HttpPost("ActualizarDeudaCliente")]
        [SwaggerOperation("Actualiza la deuda de un cliente")]
        [SwaggerResponse(StatusCodes.Status200OK, "Deuda del cliente actualizada correctamente")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Error en los datos recibidos")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor")]
        public IActionResult ActualizarDeudaCliente([FromBody] ActualizarDeudaRequest request)
        {
            if (request == null)
            {
                return BadRequest("Datos de actualización de deuda no proporcionados");
            }

            try
            {
                _repoClientes.ActualizarDeudaCliente(request.ClienteId, request.NuevaDeuda);
                return Ok("Deuda del cliente actualizada correctamente -> Soy el controaldorCliente(back end)");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la deuda del cliente: {ex.Message}");
            }
        }



        [HttpPost("RegistrarCompra")]
        [SwaggerOperation("Registra una nueva compra a crédito")]
        [SwaggerResponse(StatusCodes.Status200OK, "Compra registrada correctamente")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Error en los datos recibidos")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor")]
        public IActionResult RegistrarCompra([FromBody] Compras request)
        {
            if (request == null)
            {
                return BadRequest("Datos de compra no proporcionados");
            }

            try
            {
                var compra = new Compras
                {
                    ClienteId = request.ClienteId,
                    Fecha = DateTime.Now,
                    Detalle = request.Detalle,
                    Total = request.Total
                };

                _repoClientes.GuardarCompra(compra);
                return Ok("Compra registrada correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar la compra: {ex.Message}");
            }
        }

        [HttpGet("ObtenerComprasCliente/{clienteId}")]
        [SwaggerOperation("Obtiene las compras de un cliente")]
        [SwaggerResponse(StatusCodes.Status200OK, "Compras obtenidas exitosamente")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor")]
        public IActionResult ObtenerComprasCliente(int clienteId)
        {
            try
            {
                var compras = _context.Compras.Where(c => c.ClienteId == clienteId).ToList();
                return Ok(compras);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las compras del cliente: {ex.Message}");
            }
        }

       
    }
}

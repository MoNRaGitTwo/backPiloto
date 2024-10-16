using DemoPilotoV1.Clases;
using DemoPilotoV1.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DemoPilotoV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CajaController : ControllerBase
    {
        private readonly RepoCaja _repoCaja;

        public CajaController(RepoCaja repoCaja)
        {
            _repoCaja = repoCaja;
        }

        [HttpPost]
        public IActionResult GuardarCaja([FromBody] Caja caja)
        {
            if (caja == null)
            {
                return BadRequest("La información de la caja es requerida.");
            }

            try
            {
                _repoCaja.GuardarCaja(caja);
                return Ok("Datos de la caja guardados exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar la información de la caja: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult ObtenerUltimaCaja()
        {
            var caja = _repoCaja.ObtenerUltimaCaja();
            if (caja != null)
            {
                return Ok(caja);
            }

            return NotFound("No se encontró información de la caja.");
        }

        [HttpPut]
        public IActionResult ActualizarCaja([FromBody] Caja caja)
        {
            if (caja == null)
            {
                return BadRequest("La información de la caja es requerida.");
            }

            try
            {
                _repoCaja.ActualizarCaja(caja);
                return Ok("Datos de la caja actualizados exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la información de la caja: {ex.Message}");
            }
        }
    }
}

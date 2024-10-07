using DemoPilotoV1.Clases;
using DemoPilotoV1.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DemoPilotoV1.Controllers
{
    public class ReservasController : Controller
    {
        private readonly RepoReservas _repo;

        public ReservasController(RepoReservas repo)
        {
            _repo = repo;
        }

        
        [HttpGet("TodaslasReservas")]
        [SwaggerOperation("todo las reservas ")]
        public async Task<IActionResult> Get()
        {
            var reservas = await _repo.GetAllReservas();
            return Ok(reservas);
        }


        [HttpPost("CrearReserva")]
        [SwaggerOperation("CrearReserva ")]
        public async Task<IActionResult> Create([FromBody] Reservas reserva)
        {
            await _repo.CreateReserva(reserva);
            return CreatedAtAction(nameof(Get), new { id = reserva.ReservaId }, reserva);
        }
    }
}

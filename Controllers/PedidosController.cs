using DemoPilotoV1.Clases;
using DemoPilotoV1.DTOS;
using DemoPilotoV1.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : ControllerBase
{
    private readonly RepoPedidos _repoPedidos;

    public PedidosController(RepoPedidos repoPedidos)
    {
        _repoPedidos = repoPedidos;
    }

    [HttpGet]

    public ActionResult<List<PedidoDTO>> GetPedidos()
    {
        var pedidos = _repoPedidos.ObtenerTodosLosPedidos()
                                  .Select(p => new PedidoDTO
                                  {
                                      Id = p.Id,
                                      ClienteId = p.ClienteId,
                                      FechaPedido = p.FechaPedido,
                                      Total = p.Total,
                                      Estado = p.Estado,
                                      DetallesPedidos = p.DetallesPedidos.Select(d => new DetallePedidoDTO
                                      {
                                          Id = d.Id,
                                          PedidoId = d.PedidoId,
                                          ProductoId = d.ProductoId,
                                          Cantidad = d.Cantidad,
                                          Precio = d.Precio
                                      }).ToList()
                                  }).ToList();

        return Ok(pedidos);
    }

    [HttpGet("{id}")]
    public ActionResult<PedidoDTO> GetPedido(int id)
    {
        var pedido = _repoPedidos.ObtenerPedidoPorId(id);
        if (pedido == null)
        {
            return NotFound();
        }

        var pedidoDTO = new PedidoDTO
        {
            Id = pedido.Id,
            ClienteId = pedido.ClienteId,
            FechaPedido = pedido.FechaPedido,
            Total = pedido.Total,
            Estado = pedido.Estado,
            DetallesPedidos = pedido.DetallesPedidos.Select(d => new DetallePedidoDTO
            {
                Id = d.Id,
                PedidoId = d.PedidoId,
                ProductoId = d.ProductoId,
                Cantidad = d.Cantidad,
                Precio = d.Precio
            }).ToList()
        };

        return Ok(pedidoDTO);
    }

    
    [HttpPost("GuardarPedidos")]
    [SwaggerOperation("Guarda los pedidos")]
    public ActionResult<PedidoDTO> GuardarPedido([FromBody] PedidoDTO pedidoDTO)
    {
        if (pedidoDTO.DetallesPedidos == null || pedidoDTO.DetallesPedidos.Count == 0)
        {
            return BadRequest("El pedido debe tener al menos un detalle.");
        }

        var pedido = new Pedidos
        {
            ClienteId = pedidoDTO.ClienteId,
            FechaPedido = pedidoDTO.FechaPedido,
            Total = pedidoDTO.Total,
            Estado = pedidoDTO.Estado,
            DetallesPedidos = pedidoDTO.DetallesPedidos.Select(d => new DetallePedidos
            {
                PedidoId = d.PedidoId,
                ProductoId = d.ProductoId,
                Cantidad = d.Cantidad,
                Precio = d.Precio
            }).ToList()
        };

        _repoPedidos.GuardarPedido(pedido);
        pedidoDTO.Id = pedido.Id;

        return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedidoDTO);
    }

    [HttpPut("{id}")]
    public ActionResult ActualizarPedido(int id, [FromBody] PedidoDTO pedidoDTO)
    {
        var existingPedido = _repoPedidos.ObtenerPedidoPorId(id);
        if (existingPedido == null)
        {
            return NotFound();
        }

        existingPedido.ClienteId = pedidoDTO.ClienteId;
        existingPedido.FechaPedido = pedidoDTO.FechaPedido;
        existingPedido.Total = pedidoDTO.Total;
        existingPedido.Estado = pedidoDTO.Estado;

        _repoPedidos.EliminarDetallesPedido(existingPedido.Id);
        existingPedido.DetallesPedidos = pedidoDTO.DetallesPedidos.Select(d => new DetallePedidos
        {
            PedidoId = d.PedidoId,
            ProductoId = d.ProductoId,
            Cantidad = d.Cantidad,
            Precio = d.Precio
        }).ToList();

        _repoPedidos.ActualizarPedido(existingPedido);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult EliminarPedido(int id)
    {
        var existingPedido = _repoPedidos.ObtenerPedidoPorId(id);
        if (existingPedido == null)
        {
            return NotFound();
        }

        _repoPedidos.EliminarPedido(id);
        return NoContent();
    }
}

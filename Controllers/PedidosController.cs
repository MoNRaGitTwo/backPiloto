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
                                      Id = p.Id, // Añadir esta línea
                                      UserId = p.UserId,
                                      FechaPedido = p.FechaPedido,
                                      Total = p.Total,
                                      Estado = p.Estado,
                                      DetallesPedidos = p.DetallesPedidos.Select(d => new DetallePedidoDTO
                                      {
                                          Id = d.Id,
                                          PedidoId = d.PedidoId,
                                          ProductoId = d.ProductoId,
                                          Nombre = d.Nombre,
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
            Id = pedido.Id, // Añadir esta línea
            UserId = pedido.UserId,
            FechaPedido = pedido.FechaPedido,
            Total = pedido.Total,
            Estado = pedido.Estado,
            DetallesPedidos = pedido.DetallesPedidos.Select(d => new DetallePedidoDTO
            {
                Id = d.Id,
                PedidoId = d.PedidoId,
                ProductoId = d.ProductoId,
                Nombre = d.Nombre,
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
        // Verificar si hay detalles en el pedido
        if (pedidoDTO.DetallesPedidos == null || pedidoDTO.DetallesPedidos.Count == 0)
        {
            return BadRequest("El pedido debe tener al menos un detalle.");
        }

        // Crear la entidad Pedido con los detalles del pedido
        var pedido = new Pedidos
        {
            UserId = pedidoDTO.UserId,
            FechaPedido = pedidoDTO.FechaPedido,
            Total = pedidoDTO.Total,
            Estado = pedidoDTO.Estado,
            DetallesPedidos = pedidoDTO.DetallesPedidos.Select(d => new DetallePedidos
            {
                ProductoId = d.ProductoId,
                Nombre = d.Nombre,
                Cantidad = d.Cantidad,
                Precio = d.Precio
            }).ToList()
        };

        // Guardar el pedido y sus detalles en la base de datos
        _repoPedidos.GuardarPedido(pedido);

        // Asignar el PedidoId y los Ids de los detalles después de guardar
        foreach (var detalle in pedido.DetallesPedidos)
        {
            detalle.PedidoId = pedido.Id;  // Asignar PedidoId al detalle
        }

        // Actualizar el pedido en la base de datos para reflejar los cambios
        _repoPedidos.ActualizarPedido(pedido);

        // Actualizar el pedidoDTO con el Id del pedido generado por la base de datos
        pedidoDTO.Id = pedido.Id;

        // Actualizar los PedidoId y Id en cada DetallePedidoDTO
        for (int i = 0; i < pedido.DetallesPedidos.Count; i++)
        {
            pedidoDTO.DetallesPedidos[i].PedidoId = pedido.Id;
            pedidoDTO.DetallesPedidos[i].Id = pedido.DetallesPedidos[i].Id;  // Asignar el Id generado
        }

        // Retornar el pedidoDTO actualizado con los Ids correctos
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

       // existingPedido.ClienteId = pedidoDTO.ClienteId;
        existingPedido.FechaPedido = pedidoDTO.FechaPedido;
        existingPedido.Total = pedidoDTO.Total;
        existingPedido.Estado = pedidoDTO.Estado;

        _repoPedidos.EliminarDetallesPedido(existingPedido.Id);
        existingPedido.DetallesPedidos = pedidoDTO.DetallesPedidos.Select(d => new DetallePedidos
        {
            PedidoId = d.PedidoId,
            ProductoId = d.ProductoId,
            Nombre = d.Nombre,  // Agregar el campo Nombre aquí
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

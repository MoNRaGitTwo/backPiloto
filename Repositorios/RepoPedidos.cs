using DemoPilotoV1.BDD;
using DemoPilotoV1.Clases;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DemoPilotoV1.Repositorios
{
    public class RepoPedidos
    {
        private readonly BaseDeDatos _baseDeDatos;

        public RepoPedidos(BaseDeDatos baseDeDatos)
        {
            _baseDeDatos = baseDeDatos;
        }

        public List<Pedidos> ObtenerTodosLosPedidos()
        {
            return _baseDeDatos.Pedidos
                .Include(p => p.DetallesPedidos)
                .ToList();
        }

        public Pedidos ObtenerPedidoPorId(int id)
        {
            return _baseDeDatos.Pedidos
                .Include(p => p.DetallesPedidos)
                .FirstOrDefault(p => p.Id == id);
        }

        public void GuardarPedido(Pedidos pedido)
        {
            _baseDeDatos.Pedidos.Add(pedido);
            _baseDeDatos.SaveChanges();
        }

        public void ActualizarPedido(Pedidos pedido)
        {
            _baseDeDatos.Entry(pedido).State = EntityState.Modified;
            _baseDeDatos.SaveChanges();
        }

        public void EliminarPedido(int id)
        {
            var pedido = ObtenerPedidoPorId(id);
            if (pedido != null)
            {
                _baseDeDatos.DetallesPedidos.RemoveRange(pedido.DetallesPedidos);
                _baseDeDatos.Pedidos.Remove(pedido);
                _baseDeDatos.SaveChanges();
            }
        }

        public void EliminarDetallesPedido(int pedidoId)
        {
            var detalles = _baseDeDatos.DetallesPedidos.Where(d => d.PedidoId == pedidoId).ToList();
            _baseDeDatos.DetallesPedidos.RemoveRange(detalles);
            _baseDeDatos.SaveChanges();
        }
        public List<Pedidos> ObtenerPedidosPorUsuarioId(int userId)
        {
            return _baseDeDatos.Pedidos
                .Include(p => p.DetallesPedidos)
                .Where(p => p.UserId == userId)
                .ToList();
        }
    }
}

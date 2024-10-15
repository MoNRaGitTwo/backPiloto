using DemoPilotoV1.BDD;
using DemoPilotoV1.Clases;
using System.Collections.Generic;
using System.Linq;

namespace DemoPilotoV1.Repositorios
{
    public class RepoClientes
    {
        private readonly BaseDeDatos _baseDeDatos;

        public RepoClientes(BaseDeDatos baseDeDatos)
        {
            _baseDeDatos = baseDeDatos;
        }

        public List<Clientes> ObtenerTodosLosClientes()
        {
            return _baseDeDatos.Clientes.ToList();
        }

        public Clientes ObtenerClientePorId(int id)
        {
            return _baseDeDatos.Clientes.FirstOrDefault(c => c.Id == id);
        }

        public void GuardarCliente(string nombre, decimal deuda)
        {
            var cliente = new Clientes
            {
                Nombre = nombre,
                Deuda = deuda
            };
            _baseDeDatos.Clientes.Add(cliente);
            _baseDeDatos.SaveChanges();
        }

        public void ActualizarDeudaCliente(int id, decimal nuevaDeuda)
        {
            var cliente = ObtenerClientePorId(id);
            if (cliente != null)
            {
                cliente.Deuda = nuevaDeuda;
                _baseDeDatos.SaveChanges();
            }
        }

        public void GuardarCompra(Compras compra)
        {
            _baseDeDatos.Compras.Add(compra);
            _baseDeDatos.SaveChanges();
        }
        /*
        public List<Compras> ObtenerComprasCliente(int clienteId)
        {
            return _baseDeDatos.Compras.Where(c => c.ClienteId == clienteId).ToList();
        }*/


    }
}

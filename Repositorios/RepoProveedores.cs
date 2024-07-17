using DemoPilotoV1.BDD;
using DemoPilotoV1.Clases;

namespace DemoPilotoV1.Repositorios
{
    public class RepoProveedores
    {
        private readonly BaseDeDatos _context;

        public RepoProveedores(BaseDeDatos context)
        {
            _context = context;
        }

        public List<Proveedores> ObtenerTodosLosProveedores()
        {
            return _context.Proveedores.ToList();
        }

        public void AgregarProveedor(Proveedores proveedor)
        {
            _context.Proveedores.Add(proveedor);
            _context.SaveChanges();
        }

        public Proveedores ObtenerProveedorPorId(int id)
        {
            return _context.Proveedores.Find(id);
        }

        public void ActualizarProveedor(Proveedores proveedor)
        {
            _context.Proveedores.Update(proveedor);
            _context.SaveChanges();
        }

        public void EliminarProveedor(int id)
        {
            var proveedor = _context.Proveedores.Find(id);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
                _context.SaveChanges();
            }
        }
    }

}

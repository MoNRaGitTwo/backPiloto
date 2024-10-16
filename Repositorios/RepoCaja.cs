using DemoPilotoV1.BDD;
using DemoPilotoV1.Clases;
using System;
using System.Linq;



namespace DemoPilotoV1.Repositorios
{
    public class RepoCaja
    {
        private readonly BaseDeDatos _baseDeDatos;

        public RepoCaja(BaseDeDatos baseDeDatos)
        {
            _baseDeDatos = baseDeDatos;
        }

        public void GuardarCaja(Caja caja)
        {
            caja.FechaActualizacion = DateTime.Now;
            _baseDeDatos.Caja.Add(caja);
            _baseDeDatos.SaveChanges();
        }

        public Caja ObtenerUltimaCaja()
        {
            return _baseDeDatos.Caja
                .OrderByDescending(c => c.FechaActualizacion)
                .FirstOrDefault();
        }

        public void ActualizarCaja(Caja caja)
        {
            var cajaExistente = _baseDeDatos.Caja
                .OrderByDescending(c => c.FechaActualizacion)
                .FirstOrDefault();

            if (cajaExistente != null)
            {
                cajaExistente.MontoEfectivo = caja.MontoEfectivo;
                cajaExistente.MontoCredito = caja.MontoCredito;
                cajaExistente.Gastos = caja.Gastos;
                cajaExistente.Extra = caja.Extra;
                cajaExistente.FechaActualizacion = DateTime.Now;

                _baseDeDatos.SaveChanges();
            }
        }
    }
}

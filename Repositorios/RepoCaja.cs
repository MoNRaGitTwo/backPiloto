using DemoPilotoV1.BDD;
using DemoPilotoV1.Clases;

public class RepoCaja
{
    private readonly BaseDeDatos _baseDeDatos;

    public RepoCaja(BaseDeDatos baseDeDatos)
    {
        _baseDeDatos = baseDeDatos;
    }

    public void GuardarCaja(Caja caja)
    {
        _baseDeDatos.Caja.Add(caja);
        _baseDeDatos.SaveChanges();
    }

    public Caja ObtenerUltimaCaja()
    {
        // Ajustar si ya no necesitas ordenar por FechaActualizacion
        return _baseDeDatos.Caja
            .OrderByDescending(c => c.Id) // Ordenar por ID para obtener la última caja agregada.
            .FirstOrDefault();
    }

    public void ActualizarCaja(Caja caja)
    {
        var cajaExistente = _baseDeDatos.Caja
            .OrderByDescending(c => c.Id) // Ordenar por ID para obtener la última caja agregada.
            .FirstOrDefault();

        if (cajaExistente != null)
        {
            cajaExistente.MontoEfectivo += caja.MontoEfectivo;
            cajaExistente.MontoCredito += caja.MontoCredito;
            cajaExistente.Gastos += caja.Gastos;
            cajaExistente.Extra += caja.Extra;

            _baseDeDatos.SaveChanges();
        }
    }
}

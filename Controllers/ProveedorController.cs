using DemoPilotoV1.BDD;
using DemoPilotoV1.Clases;
using DemoPilotoV1.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

public class ProveedoresController : ControllerBase
{
    private readonly BaseDeDatos _context;
    private readonly RepoProveedores _repoProveedores;

    public ProveedoresController(BaseDeDatos context)
    {
        _context = context;
        _repoProveedores = new RepoProveedores(context);
    }

    [HttpGet("TodosProveedores")]
    [SwaggerOperation("Obtiene todos los proveedores")]
    [SwaggerResponse(StatusCodes.Status200OK, "Proveedores obtenidos exitosamente")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor")]
    public IActionResult ObtenerProveedores()
    {
        try
        {
            var proveedores = _repoProveedores.ObtenerTodosLosProveedores();
            return Ok(proveedores);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
        }
    }

    [HttpPost("AgregarProveedor")]
    [SwaggerOperation("Agrega un nuevo proveedor")]
    [SwaggerResponse(StatusCodes.Status201Created, "Proveedor agregado exitosamente")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Solicitud incorrecta")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor")]
    public IActionResult AgregarProveedor([FromBody] Proveedores proveedor)
    {
        if (proveedor == null)
        {
            return BadRequest("Proveedor es nulo");
        }

        try
        {
            _repoProveedores.AgregarProveedor(proveedor);
            return CreatedAtAction(nameof(ObtenerProveedores), new { id = proveedor.Id }, proveedor);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
        }
    }

    [HttpPut("ActualizarProveedor/{id}")]
    [SwaggerOperation("Actualiza un proveedor existente")]
    [SwaggerResponse(StatusCodes.Status200OK, "Proveedor actualizado exitosamente")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Solicitud incorrecta")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Proveedor no encontrado")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor")]
    public IActionResult ActualizarProveedor(int id, [FromBody] Proveedores proveedor)
    {
        if (proveedor == null || proveedor.Id != id)
        {
            return BadRequest("Proveedor es nulo o el ID no coincide");
        }

        try
        {
            var proveedorExistente = _repoProveedores.ObtenerProveedorPorId(id);
            if (proveedorExistente == null)
            {
                return NotFound("Proveedor no encontrado");
            }

            _repoProveedores.ActualizarProveedor(proveedor);
            return Ok(proveedor);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
        }
    }

    [HttpDelete("EliminarProveedor/{id}")]
    [SwaggerOperation("Elimina un proveedor existente")]
    [SwaggerResponse(StatusCodes.Status200OK, "Proveedor eliminado exitosamente")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Proveedor no encontrado")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor")]
    public IActionResult EliminarProveedor(int id)
    {
        try
        {
            var proveedorExistente = _repoProveedores.ObtenerProveedorPorId(id);
            if (proveedorExistente == null)
            {
                return NotFound("Proveedor no encontrado");
            }

            _repoProveedores.EliminarProveedor(id);
            return Ok("Proveedor eliminado exitosamente");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
        }
    }

    [HttpGet("ProveedoresPorDia/{dia}")]
    [SwaggerOperation("Obtiene los proveedores que pasan en un día específico")]
    [SwaggerResponse(StatusCodes.Status200OK, "Proveedores obtenidos exitosamente")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor")]
    public IActionResult ObtenerProveedoresPorDia(string dia)
    {
        try
        {
            var proveedores = _repoProveedores.ObtenerProveedoresPorDia(dia);
            return Ok(proveedores);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor: " + ex.Message);
        }
    }
}

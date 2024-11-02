using Microsoft.AspNetCore.Mvc;
using SERVIDOR_MAUI.Models;
using System.Collections.Generic;
using System.Linq;

namespace SERVIDOR_MAUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatoController : ControllerBase
    {
        private static List<Plato> platos = new List<Plato>
        {
        };

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(platos.OrderBy(_ => System.Guid.NewGuid()).ToList());
        }

        [HttpPost]
        public IActionResult Post([FromBody] Plato nuevoPlato)
        {
            nuevoPlato.Id = platos.Count + 1;
            platos.Add(nuevoPlato);
            return CreatedAtAction(nameof(Get), new { id = nuevoPlato.Id }, nuevoPlato);
        }


        [HttpDelete("{id}")]
        public IActionResult DeletePlato(int id)
        {
            var plato = platos.FirstOrDefault(p => p.Id == id);
            if (plato == null)
            {
                return NotFound();
            }
            platos.Remove(plato);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Plato updatedPlato)
        {
            var existingPlato = platos.FirstOrDefault(p => p.Id == id);
            if (existingPlato == null) return NotFound();

            existingPlato.Nombre = updatedPlato.Nombre;
            existingPlato.Costo = updatedPlato.Costo;
            existingPlato.Ingredientes = updatedPlato.Ingredientes;

            return NoContent();
        }

    }
}
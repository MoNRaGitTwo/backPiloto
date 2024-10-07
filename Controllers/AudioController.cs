using DemoPilotoV1.Clases;
using DemoPilotoV1.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DemoPilotoV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AudioController : ControllerBase
    {
        private readonly RepoAudio _repoAudio;

        public AudioController(RepoAudio repoAudio)
        {
            _repoAudio = repoAudio;
        }

        [HttpPost("upload")]
        [SwaggerOperation(Summary = "Sube un archivo de audio")]
        [SwaggerResponse(StatusCodes.Status200OK, "Archivo de audio subido exitosamente")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos no válidos")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Error interno del servidor")]
        public async Task<IActionResult> UploadAudio([FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No se ha enviado ningún archivo.");
                }

                if (!file.ContentType.StartsWith("audio/"))
                {
                    return BadRequest("El archivo no es un audio válido.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var fileData = memoryStream.ToArray();

                    var audioFile = new AudioFile
                    {
                        FileName = file.FileName,
                        FileType = file.ContentType,
                        FileData = fileData,
                        UploadedDate = DateTime.UtcNow
                    };

                    _repoAudio.GuardarAudio(audioFile.FileName, audioFile.FileType, audioFile.FileData);

                    return Ok(new { Message = "Archivo de audio subido exitosamente" });
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }


        [HttpGet("download/{id}")]
        public IActionResult DownloadAudio(int id)
        {
            try
            {
                var audio = _repoAudio.ObtenerAudioPorId(id);
                if (audio == null)
                {
                    return NotFound("Archivo de audio no encontrado.");
                }

                return File(audio.FileData, audio.FileType, audio.FileName);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}

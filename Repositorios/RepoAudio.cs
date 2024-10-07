using DemoPilotoV1.BDD;
using DemoPilotoV1.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoPilotoV1.Repositorios
{
    public class RepoAudio
    {
        private readonly BaseDeDatos _baseDeDatos;

        public RepoAudio(BaseDeDatos baseDeDatos)
        {
            _baseDeDatos = baseDeDatos;
        }

        public List<AudioFile> ObtenerTodosLosAudios()
        {
            try
            {
                return _baseDeDatos.AudioFiles
                    .Select(a => new AudioFile
                    {
                        Id = a.Id,
                        FileName = a.FileName,
                        FileType = a.FileType,
                        UploadedDate = a.UploadedDate
                    }).ToList();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al obtener todos los audios: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        public AudioFile ObtenerAudioPorId(int id)
        {
            return _baseDeDatos.AudioFiles
                .FirstOrDefault(a => a.Id == id);
        }

        public void GuardarAudio(string fileName, string fileType, byte[] fileData)
        {
            var audio = new AudioFile
            {
                FileName = fileName,
                FileType = fileType,
                FileData = fileData,
                UploadedDate = DateTime.UtcNow
            };

            _baseDeDatos.AudioFiles.Add(audio);
            _baseDeDatos.SaveChanges();
        }

        public void EliminarAudio(int id)
        {
            var audio = ObtenerAudioPorId(id);
            if (audio != null)
            {
                _baseDeDatos.AudioFiles.Remove(audio);
                _baseDeDatos.SaveChanges();
            }
        }

        public AudioFile EditarAudio(int id, string fileName, string fileType, byte[] fileData)
        {
            var audio = _baseDeDatos.AudioFiles
                .FirstOrDefault(a => a.Id == id);

            if (audio != null)
            {
                audio.FileName = fileName;
                audio.FileType = fileType;

                if (fileData != null && fileData.Length > 0)
                {
                    audio.FileData = fileData;
                }

                _baseDeDatos.SaveChanges();

                return audio;
            }

            return null;
        }
    }
}

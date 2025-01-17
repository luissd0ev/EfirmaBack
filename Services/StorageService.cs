using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace APIEfirma.Services
{
    public class StorageService
    {
        private readonly string _savePath;

        public StorageService(IConfiguration configuration)
        {
            // Lee la configuración de "SavePath" desde appsettings.json
            _savePath = configuration["StorageSettings:SavePath"]
                        ?? throw new ArgumentNullException("SavePath is not configured.");
        }

        public async Task SaveFileAsync(byte[] fileContent, string fileName)
        {
            // Asegúrate de que el directorio existe
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }

            // Construye la ruta completa del archivo
            string fullPath = Path.Combine(_savePath, fileName);

            // Guarda el archivo de forma asíncrona
            await File.WriteAllBytesAsync(fullPath, fileContent);
        }
    }
}

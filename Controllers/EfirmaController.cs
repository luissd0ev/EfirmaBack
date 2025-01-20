using APIEfirma.Models;
using APIEfirma.Utility;
using EFirmaProyecto.Utility;
using FirmaDocumento.Areas.FEA.Models;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Tls;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;
using iTextSharp.text;
using iTextSharp.text.pdf;
using QRCoder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Security.Cryptography;
using System.Data;

using Dapper;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Ocsp;
using System.Net;
using System.Collections;
using System.Drawing;
using System.Net.Http;
using Abp.Threading;
using System.Drawing.Imaging;
using APIEfirma.Interfaces;

using iTextSharp.text.pdf;

//using iText.Kernel.Pdf.Canvas.Parser.Listener;
//using iText.Kernel.Pdf.Canvas.Parser;


//using iTextSharp.text.pdf; // Alias: PdfSharp
//using iText.Kernel.Pdf; // Alias: PdfKernel

//using iText.Layout;
//using iText.Layout.Element;
using System.Text.RegularExpressions;
using System;
using System.IO;
using UglyToad.PdfPig;
using APIEfirma.Services;
using APIEfirma.Repositories;


namespace APIEfirma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EfirmaController : Controller
    {
        private readonly ICertificadoService _certificadoService;
        private readonly IFirmaService _firmaService;
        private readonly StorageService _storageService;
        private readonly IDocumento<Documento> _documento;  

        public EfirmaController(ICertificadoService certificadoService, IFirmaService firmaService, StorageService storageService, IDocumento<Documento> documento)
        {
            _certificadoService = certificadoService;
            _firmaService = firmaService;
            _storageService = storageService;
            _documento = documento;
        }

        private string firmante = "";
        public string serie_firmante { get; set; }

        public string fecha_revo { get; set; }
        public string fechacdmx { get; set; }
        public string fecha_utc { get; set; }
        public string hash256 { get; set; }
        public string firma { get; set; }


        [HttpPost("Firmar")]
        public async Task<IActionResult> Firmar(
       IFormFile key,
       IFormFile cer,
       IFormFile documento,
       [FromForm] string contrasena
         )
        {
            if (key == null || cer == null || documento == null || string.IsNullOrWhiteSpace(contrasena))
            {
                return BadRequest(new { message = "Todos los archivos y la contraseña son obligatorios." });
            }

            try
            {
                // Leer los archivos en memoria
                using var keyMemoryStream = new MemoryStream();
                 await key.OpenReadStream().CopyToAsync(keyMemoryStream);

                using var cerMemoryStream = new MemoryStream();
                await cer.OpenReadStream().CopyToAsync(cerMemoryStream);


                var documentoMemoryStream = new MemoryStream();
                await documento.OpenReadStream().CopyToAsync(documentoMemoryStream);

                // Validar el certificado antes de proceder
                var validacionCertificado = _certificadoService.ValidaCertSAT(cerMemoryStream);

                if (!validacionCertificado.Item1)
                {
                    return BadRequest(new { message = validacionCertificado.Item2 });
                }
                // Construir el objeto DatosCofirma
                var datosBaseDoc = new DatosCofirmaB
                {
                    CER = cerMemoryStream.ToArray(),
                    KEY = keyMemoryStream.ToArray(),
                    Pass = contrasena,
                    NombreArchivo = documento.FileName,
                    CadenaOriginal = documentoMemoryStream.ToArray()
                };

                //Generar la firma
                var resultadoFirma = _firmaService.GeneraCadenaFirma(datosBaseDoc, documentoMemoryStream);



                string fechacdmx = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                string fecha_revo = DateTime.Now.ToString("yyyy-MM-dd");
                string fecha_utc = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                firmante = resultadoFirma.firmanteDocumento;

                MemoryStream pdfFirmado = _firmaService.EscribeFirma(
                    resultadoFirma.firma,
                    resultadoFirma.hash256,
                    datosBaseDoc,
                    firmante,
                    fechacdmx,
                    1,
                    1,
                    false
                );

                pdfFirmado.Position = 0;
                byte[] pdfFirmadoBytes = pdfFirmado.ToArray();
                string nombreArchivo = $"{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid()}.pdf";
                // Guardar el documento firmado en la ubicación configurada
                // Guardar el documento en la base de datos
                var nuevoDocumento = new Documento
                {
                    DocRfc = "RFC GENERIC", // Asume que `firmante` tiene un RFC
                    DocNombredocumento = nombreArchivo,
                    DocFirma = resultadoFirma.firma,
                    DocHashcode = resultadoFirma.hash256
                };

                _documento.Insert(nuevoDocumento);
                await _storageService.SaveFileAsync(pdfFirmadoBytes, nombreArchivo);
                return File(pdfFirmado, "application/pdf", "documento_firmado.pdf");


            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error durante el proceso de firma.", error = ex.Message });
            }
        }

        [HttpPost("VerificarFirma")]
        public async Task<IActionResult> VerificarFirma(
         IFormFile documento
        )
        {
            try
            {
                if (documento == null || documento.Length == 0)
                {
                    return BadRequest("El documento no puede estar vacío.");
                }

                // Leer el documento en memoria
                using var documentoMemoryStream = new MemoryStream();
                await documento.OpenReadStream().CopyToAsync(documentoMemoryStream);

                // Convertir el documento en un arreglo de bytes
                byte[] documentoBytes = documentoMemoryStream.ToArray();

                // Extraer la firma del documento
                string firma = _firmaService.ExtraerFirmaDesdePdf(documentoBytes);

                if (string.IsNullOrEmpty(firma))
                {
                    return NotFound("No se encontró una firma válida en el documento.");
                }

                // Validar la firma (aquí podrías agregar la lógica adicional según tus necesidades)
                return Ok(new { Mensaje = "Firma válida encontrada", Firma = firma });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error procesando el PDF: {ex.Message}");
            }
        }

        // Método para obtener los documentos
        //[HttpGet("GetDocuments")]
        //public async Task<IActionResult> GetDocuments()
        //{
        //    try
        //    {
        //        var documents = await _efirmaRepository.getDocuments();

        //        if (documents == null )
        //        {
        //            return NotFound("No se encontraron documentos.");
        //        }

        //        return Ok(documents);  // Devuelves los documentos obtenidos
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error al obtener los documentos: {ex.Message}");
        //    }
        //}

    }



}

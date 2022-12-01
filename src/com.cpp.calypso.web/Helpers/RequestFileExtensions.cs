using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Web;

namespace com.cpp.calypso.web
{
    public static class RequestFileExtensions
    {

        /// <summary>
        /// Generar un object Archivo desde el request del controller.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="nameFieldWithFile"></param>
        /// <returns></returns>
        public static ArchivoDto GenerateFileFromRequest(this HttpRequestBase request, string nameFieldWithFile)
        {

            //uploadFile
            HttpPostedFileBase file = request.Files[nameFieldWithFile];
            if (file == null)
                return null;


            string fileName = file.FileName;
            string fileContentType = file.ContentType;
            byte[] fileBytes = new byte[file.ContentLength];
            var data = file.InputStream.Read(fileBytes, 0,
                Convert.ToInt32(file.ContentLength));

            var random = new Random();
            var archivo = new ArchivoDto
            {
                Id = 0,
                codigo = "Codigo_" + random.Next(1, 99999),
                nombre = fileName,
                vigente = true,
                fecha_registro = DateTime.Now,
                hash = fileBytes,
                tipo_contenido = fileContentType,
            };

            return archivo;
        }

    }


}
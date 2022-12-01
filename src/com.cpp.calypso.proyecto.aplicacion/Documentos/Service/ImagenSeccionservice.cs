using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Dto;
using com.cpp.calypso.proyecto.aplicacion.Documentos.Interface;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Documentos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Documentos.Service
{
    public class ImagenSeccionAsyncBaseCrudAppService : AsyncBaseCrudAppService<ImagenSeccion, ImagenSeccionDto, PagedAndFilteredResultRequestDto>, IImagenSeccionAsyncBaseCrudAppService
    {

        const string AES_IV = "00f74597de203655";//16 bits  
        public ImagenSeccionAsyncBaseCrudAppService(
            IBaseRepository<ImagenSeccion> repository
        ) : base(repository)
        {
        }

        public async Task<bool> CrearImagenSeccionAsync(ImagenSeccionDto dto)
        {

            var entity = Mapper.Map<ImagenSeccion>(dto);
            await Repository.InsertAsync(entity);
            return true;
        }

        public string EncryptByAES(string input, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV.Substring(0, 16));

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                        byte[] bytes = msEncrypt.ToArray();
                        return ByteArrayToHexString(bytes);
                    }
                }
            }
        }
        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                //hexadecimal number
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
                //16 digits separated by spaces
                //sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            }
            return sb.ToString().ToUpper();
        }

        public async Task<string> CrearImagenesSeccionAsync(List<ImagenSeccionDto> imagenes)
        {
            foreach (var imagen in imagenes)
            {
                var partes = this.GetChunks(this.EncryptByAES(imagen.ImagenBase64, CatalogosCodigos.keycryt), 32000);
                if (partes.Count > 20)
                {
                    return "ENCRYPTADO_MAYOR_20";
                }

                await CrearImagenSeccionAsync(imagen);
            }
            return "OK";
        }

        public List<string> GetChunks(string value, int chunkSize)
        {
            List<string> triplets = new List<string>();
            while (value.Length > chunkSize)
            {
                triplets.Add(value.Substring(0, chunkSize));
                value = value.Substring(chunkSize);
            }

            if (value != "")
                triplets.Add(value);
            return triplets;
        }

        public ResultadoEliminacionResponse EliminarImagen(int id)
        {
            Repository.Delete(id);
            return new ResultadoEliminacionResponse
            {
                Eliminado = true,
                Error = ""
            };
        }

        public List<ImagenSeccionDto> ObtenerImagensPorSeccion(int seccionId)
        {
            var imagenes = Repository.GetAll()
                .Where(o => o.SeccionId == seccionId)
                .ToList();

            return Mapper.Map<List<ImagenSeccionDto>>(imagenes);
        }
    }
}

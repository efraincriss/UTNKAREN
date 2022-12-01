using Abp.Application.Services.Dto;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ColaboradoresFotografiaAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradoresFotografia, ColaboradoresFotografiaDto, PagedAndFilteredResultRequestDto>, IColaboradoresFotografiaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Archivo> _archivoRepository;

        public ColaboradoresFotografiaAsyncBaseCrudAppService(
            IBaseRepository<ColaboradoresFotografia> repository,
            IBaseRepository<Archivo> archivoRepository
            ) : base(repository)
        {
            _archivoRepository = archivoRepository;
        }

        public async Task<string> CrearActualizarFotografiaPorColaboradorAsync(ColaboradoresFotografiaDto colaboradoresFotografia, HttpPostedFileBase[] UploadedFile)
        {
            /* buscamos si tiene alguna fotografia 
             * registrada el colaborador para actualizarla o registrarla */
            ColaboradoresFotografia foto = Repository
               .GetAll()
               .Where(e => e.colaborador_id == colaboradoresFotografia.colaborador_id
                && e.origen == colaboradoresFotografia.origen)
               .FirstOrDefault();

            if (foto != null)
            {
                foto.vigente = true;
                foto.fecha_registro = DateTime.Now;

                /* actualizamos el archivo */
                if (UploadedFile != null && UploadedFile.Length > 0)
                {
                    foreach (var archivo in UploadedFile)
                    {
                        string fileName = archivo.FileName;
                        string fileContentType = archivo.ContentType;
                        byte[] fileBytes = new byte[archivo.ContentLength];
                        var data = archivo.InputStream.Read(fileBytes, 0,
                            Convert.ToInt32(archivo.ContentLength));

                        Archivo n = new Archivo
                        {
                            Id = foto.archivo_id,
                            codigo = "CFOTO" + colaboradoresFotografia.colaborador_id,
                            nombre = fileName,
                            vigente = true,
                            fecha_registro = DateTime.Now,
                            hash = fileBytes,
                            tipo_contenido = fileContentType,
                        };

                        /* Actualiza el file */
                        var archivoid = _archivoRepository.InsertOrUpdateAndGetId(n);
                        foto.archivo_id = archivoid;

                        /* actualiza el registro */
                        var result = Repository.Update(foto);
                        return result.ToString();
                    }
                }

            }
            else
            {
                /* cargamos el archivo */
                if (UploadedFile != null && UploadedFile.Length > 0)
                {
                    foreach (var archivo in UploadedFile)
                    {

                        string fileName = archivo.FileName;
                        string fileContentType = archivo.ContentType;
                        byte[] fileBytes = new byte[archivo.ContentLength];
                        var data = archivo.InputStream.Read(fileBytes, 0,
                            Convert.ToInt32(archivo.ContentLength));

                        Archivo n = new Archivo
                        {
                            Id = 0,
                            codigo = "CFOTO" + colaboradoresFotografia.colaborador_id,
                            nombre = fileName,
                            vigente = true,
                            fecha_registro = DateTime.Now,
                            hash = fileBytes,
                            tipo_contenido = fileContentType,
                        };
                        var archivoid = _archivoRepository.InsertAndGetId(n);

                        /* Registramos la fotografia al colaborador */
                        colaboradoresFotografia.archivo_id = archivoid;
                        colaboradoresFotografia.vigente = true;

                        var result = Repository.InsertAndGetId(MapToEntity(colaboradoresFotografia));
                        return result.ToString();
                    }
                }
            }
            return "NO";
        }

        public bool EliminarFotografiaPorOrigen(int Idcolaborador, string origen)
        {
            var foto = Repository.GetAll()
                                 .Where(e => e.colaborador_id == Idcolaborador && e.origen == origen)
                                 .FirstOrDefault();

            if (foto != null)
            {
                foto.vigente = false;
                Repository.Update(foto);

                /* se cambia el vigente a false el archivo registrado */
                var archivo = _archivoRepository.Get(foto.archivo_id);
                archivo.vigente = false;
                _archivoRepository.Update(archivo);
                return true;
            }

            return false;
        }

        public Archivo GetArchivoFotografia(int Idcolaborador, string origen)
        {
            ColaboradoresFotografia foto = Repository
                                            .GetAll()
                                            .Where(e => e.colaborador_id == Idcolaborador 
                                            && e.origen == origen && e.vigente == true)
                                            .FirstOrDefault();

            if (foto != null)
            {
                var archivo = _archivoRepository.GetAll().Where(e => e.Id == foto.archivo_id).FirstOrDefault();
                return archivo;
            }

            return null;
        }

        public ColaboradoresFotografia GetFotografia(int Id)
        {
            ColaboradoresFotografia foto = Repository.Get(Id);
            return foto;
        }

        public List<ColaboradoresFotografia> GetFotografiaPorOrigen(int Idcolaborador, string origen)
        {
            List<ColaboradoresFotografia> fotos = Repository
                .GetAll()
                .Where(e => e.colaborador_id == Idcolaborador && e.origen == origen && e.vigente == true)
                .ToList();
            return fotos;
        }

    }
}

using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ColaboradoresAusentismoRequisitosAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradoresAusentismoRequisitos, ColaboradoresAusentismoRequisitosDto, PagedAndFilteredResultRequestDto>, IColaboradoresAusentismoRequisitosAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Archivo> _archivoRepository;

        public ColaboradoresAusentismoRequisitosAsyncBaseCrudAppService(
            IBaseRepository<ColaboradoresAusentismoRequisitos> repository,
            IBaseRepository<Archivo> archivoRepository
            ) : base(repository)
        {
            _archivoRepository = archivoRepository;
        }

        public async Task<string> CrearAusentismoRequisitoAsync(ColaboradoresAusentismoRequisitosDto colaboradoresAusentismoRequisito, HttpPostedFileBase[] UploadedFile)
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
                        codigo = "ARCHIVO_" + colaboradoresAusentismoRequisito.requisito_id,
                        nombre = fileName,
                        vigente = true,
                        fecha_registro = DateTime.Now,
                        hash = fileBytes,
                        tipo_contenido = fileContentType,
                    };
                    var archivoid = _archivoRepository.InsertAndGetId(n);

                    /* Registramos la fotografia al colaborador */
                    colaboradoresAusentismoRequisito.archivo_id = archivoid;

                    var result = Repository.InsertAndGetId(MapToEntity(colaboradoresAusentismoRequisito));
                    return "OK";
                }
            }

            return "OK";
        }

        public async Task<string> EditarAusentismoRequisitoAsync(ColaboradoresAusentismoRequisitosDto colaboradoresAusentismoRequisito, HttpPostedFileBase[] UploadedFile)
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

                    var n = _archivoRepository.Get(colaboradoresAusentismoRequisito.archivo_id.Value);
                    n.fecha_registro = DateTime.Now;
                    n.nombre = fileName;
                    n.hash = fileBytes;
                    n.tipo_contenido = fileContentType;                   

                    await _archivoRepository.InsertOrUpdateAsync(n);

                    //var result = Repository.Update(MapToEntity(colaboradoresAusentismoRequisito));

                    return "OK";
                }
            }

            return "OK";
        }

        public List<ColaboradoresAusentismoRequisitosDto> GetAusentismoRequisito(int id) {

            var query = Repository.GetAll().Where(c => c.colaborador_ausentismo_id == id);

            var requisitos = (from d in query
                              select new ColaboradoresAusentismoRequisitosDto
                              {
                                  Id = d.Id,
                                  colaborador_ausentismo_id = d.colaborador_ausentismo_id,
                                  requisito_id = d.requisito_id,
                                  archivo_id = d.archivo_id,
                                  cumple = d.cumple,
                                  Archivo = d.Archivo
                              }).ToList();

            return requisitos;
        }

        public ColaboradoresAusentismoRequisitosDto ObtenerArchivos(int Id)
        {
            var query = Repository.GetAllIncluding(c => c.Archivo).Where(c => c.colaborador_ausentismo_id == Id).FirstOrDefault();
            if (query != null && query.Id > 0)
            {
                ColaboradoresAusentismoRequisitosDto e = new ColaboradoresAusentismoRequisitosDto();
                e.Id = query.Id;
                e.Archivo = query.Archivo;
                e.archivo_id = query.archivo_id;
                e.colaborador_ausentismo_id = query.colaborador_ausentismo_id;

                return e;
            }

            else {
                return new ColaboradoresAusentismoRequisitosDto();
            }
            
        }
    }
}

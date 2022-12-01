using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
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
    public class ColaboradoresAusentismoReintegrosAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradoresAusentismoReintegros, ColaboradoresAusentismoReintegrosDto, PagedAndFilteredResultRequestDto>, IColaboradoresAusentismoReintegrosAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Archivo> _archivoRepository;
        private readonly IBaseRepository<ColaboradoresAusentismo> _colaboradoresAusentismo;
        private readonly IBaseRepository<Colaboradores> _colaboradoresRepository;

        public ColaboradoresAusentismoReintegrosAsyncBaseCrudAppService(
            IBaseRepository<ColaboradoresAusentismoReintegros> repository,
            IBaseRepository<Colaboradores> colaboradoresRepository,
            IBaseRepository<Archivo> archivoRepository,
            IBaseRepository<ColaboradoresAusentismo> colaboradoresAusentismo
            ) : base(repository)
        {
            _archivoRepository = archivoRepository;
            _colaboradoresRepository = colaboradoresRepository;
            _colaboradoresAusentismo = colaboradoresAusentismo;
        }

        public async Task<string> CrearReintegrosAsync(ColaboradoresAusentismoReintegrosDto colaboradoresAusentismoReintegrosDto, HttpPostedFileBase[] UploadedFile)
        {
            /* Buscamos el ausentismo */
            var ca = _colaboradoresAusentismo.Get(colaboradoresAusentismoReintegrosDto.colaborador_ausentismo_id); 

            /* Actualizamos el estado del ausentismo de colaborador */
            Colaboradores co = _colaboradoresRepository.Get(ca.colaborador_id);
            _colaboradoresRepository.Update(co);

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
                            codigo = "DOCRES" + colaboradoresAusentismoReintegrosDto.colaborador_ausentismo_id,
                            nombre = fileName,
                            vigente = true,
                            fecha_registro = DateTime.Now,
                            hash = fileBytes,
                            tipo_contenido = fileContentType,
                        };
                        var archivoid = _archivoRepository.InsertAndGetId(n);

                        /* Registramos el archivo al reintegro  */
                        colaboradoresAusentismoReintegrosDto.archivo_id = archivoid;
                        colaboradoresAusentismoReintegrosDto.vigente = true;

                        var result = Repository.InsertAndGetId(MapToEntity(colaboradoresAusentismoReintegrosDto));
                        return result.ToString();
                    }
                }
            return "NO";
        }
    }
}

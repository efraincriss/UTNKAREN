using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion
{


    public class AdendaServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<Adenda, AdendaDto, PagedAndFilteredResultRequestDto>, IAdendaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Archivo> _archivoRepository;
        private readonly IBaseRepository<ArchivosContrato> _archivoContratoRepository;
        public AdendaServiceAsyncBaseCrudAppService(IBaseRepository<Adenda> repository,
            IBaseRepository<Archivo> archivoRepository,
            IBaseRepository<ArchivosContrato> archivoContratoRepository
            ) : base(repository)
        {
            _archivoRepository = archivoRepository;
            _archivoContratoRepository = archivoContratoRepository;
        }

        public bool comprobarfechaadenda(DateTime fa, DateTime fcontrato)
        {
            if (fa > fcontrato)
            {
                return true;
            }
            else {

                return false;
            }
        }

        public AdendaDto EliminarVigencia(int AdendaId)
        {
            throw new NotImplementedException();
        }

        public AdendaDto GetDetalle(int AdendaId)
        {
            var adendaQuery = Repository.GetAllIncluding(
                c => c.Contrato, c=>c.ArchivosContrato).Where(c => c.vigente == true);
            AdendaDto item = (from c in adendaQuery

                                                 where c.Id == AdendaId
                                                 select new AdendaDto
                                                 {
                                                     Id = c.Id,
                                                     ContratoId = c.ContratoId,
                                                     fecha=c.fecha,
                                                    codigo=c.codigo,
                                                    descripcion=c.descripcion,
                                                     vigente = c.vigente,
                                                     Contrato =c.Contrato,
                                                     ArchivosContratoId=c.ArchivosContratoId,
                                                     ArchivosContrato = c.ArchivosContrato
                                                 }).FirstOrDefault();

            return item;
        }

        public int GuardarArchivo(int ContratoId, HttpPostedFileBase UploadedFile)
        {
            if (UploadedFile != null)
            {
                string fileName = UploadedFile.FileName;
                string fileContentType = UploadedFile.ContentType;
                byte[] fileBytes = new byte[UploadedFile.ContentLength];
                var data = UploadedFile.InputStream.Read(fileBytes, 0,
                    Convert.ToInt32(UploadedFile.ContentLength));

                Archivo n = new Archivo
                {
                    Id = 0,
                    codigo = "AC000",
                    nombre = fileName,
                    vigente = true,
                    fecha_registro = DateTime.Now,
                    hash = fileBytes,
                    tipo_contenido = fileContentType,
                };
                var archivoid = _archivoRepository.InsertAndGetId(n);

                if (archivoid > 0)
                {
                    ArchivosContrato ac = new ArchivosContrato()
                    {
                        Id = 0,
                        ContratoId = ContratoId,
                        ArchivoId = archivoid,
                        vigente = true
                    };
                    var Archivocontratoid = _archivoContratoRepository.InsertAndGetId(ac);
                    if (Archivocontratoid > 0)
                    {
                        return Archivocontratoid;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;

                }



            }
            return 0;
        }
    }
}
    


using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion;
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
    public class ContratoDocumentoBancarioServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<ContratoDocumentoBancario, ContratoDocumentoBancarioDto, PagedAndFilteredResultRequestDto>, IContratoDocumentoBancarioAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Archivo> _archivoRepository;
        private readonly IBaseRepository<ArchivosContrato> _archivoContratoRepository;

        public ContratoDocumentoBancarioServiceAsyncBaseCrudAppService(IBaseRepository<ContratoDocumentoBancario> repository,
       IBaseRepository<Archivo> archivoRepository,
      IBaseRepository<ArchivosContrato> archivoContratoRepository
            
            ) : base(repository)
        {
            _archivoRepository =archivoRepository;
            _archivoContratoRepository = archivoContratoRepository;
        }


        public ContratoDocumentoBancarioDto GetDetalle(int ContratoDocumentoBancarioId)
        {
            var contratodocumentobancarioQuery = Repository.GetAllIncluding(c=>c.Contrato,c=>c.InstitucionFinanciera,c=>c.ArchivosContrato, c=>c.ArchivosContrato.Archivos).Where(c=>c.vigente==true);
            var item = (from c in contratodocumentobancarioQuery

                                where c.Id == ContratoDocumentoBancarioId
                                select new ContratoDocumentoBancarioDto
                                {
                                    Id = c.Id,
                                    ContratoId = c.ContratoId,
                                    InstitucionFinancieraId = c.InstitucionFinancieraId,
                                    tipo_documento = c.tipo_documento,
                                    codigo = c.codigo,
                                    fecha_emision = c.fecha_emision,
                                    fecha_vencimiento = c.fecha_vencimiento,
                                    notificado_cliente = c.notificado_cliente,
                                    estado = c.estado,
                                    fecha_notificacion = c.fecha_notificacion,
                                    vigente = c.vigente,
                                     Contrato = c.Contrato,
                                    concepto = c.concepto,
                                    InstitucionFinanciera = c.InstitucionFinanciera,
                                    ArchivosContratoId = c.ArchivosContratoId,
                                    ArchivosContrato = c.ArchivosContrato
                                }).FirstOrDefault();

            return item;
        }

        public ContratoDocumentoBancarioDto EliminarVigencia(int ContratoDocumentoBancarioId)
        {
            var contratrodocbancario = this.GetDetalle(ContratoDocumentoBancarioId);
            if (contratrodocbancario != null)
            {
                    contratrodocbancario.vigente = false;
                    var reqActualizado = Repository.Update(MapToEntity(contratrodocbancario));
                    return MapToEntityDto(reqActualizado);
            }
            return new ContratoDocumentoBancarioDto();
        }

        public bool comprobarfecha(DateTime fi, DateTime fv)
        {
            if (fv > fi)
            {
                return true;
            }
            else
            {

                return false;
            }
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

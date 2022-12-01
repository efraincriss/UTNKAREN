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
	public class ColaboradorRequisitoAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradorRequisito, ColaboradorRequisitoDto, PagedAndFilteredResultRequestDto>, IColaboradorRequisitoAsyncBaseCrudAppService
	{
        private readonly IBaseRepository<Archivo> _archivoRepository;
        public ColaboradorRequisitoAsyncBaseCrudAppService(
			IBaseRepository<ColaboradorRequisito> repository,
            IBaseRepository<Archivo> archivoRepository
			) : base(repository)
		{
            _archivoRepository = archivoRepository;

        }

		public List<ColaboradorRequisitoDto> GetList(int Id)
		{
			var query = Repository.GetAllIncluding(c=> c.Requisitos).Where(c=> c.ColaboradoresId == Id && c.vigente == true).OrderBy(c => c.Requisitos.nombre);

			var requisitos = (from d in query
							  select new ColaboradorRequisitoDto
							  {
								  Id = d.Id,
								  ColaboradoresId = d.ColaboradoresId,
                                  Requisitos = d.Requisitos,
								  RequisitosId = d.RequisitosId,
                                  Archivo = d.Archivo,
								  ArchivoId = d.ArchivoId,
								  cumple = d.cumple,
								  fecha_caducidad = d.fecha_caducidad,
                                  fecha_emision = d.fecha_emision,
								  observacion = d.observacion,
								  vigente = d.vigente,
                                  /*catalogo_accion_id = d.catalogo_accion_id,
                                  nombre_accion = d.catalogo_accion_id > 0 ? d.Accion.nombre : "",*/
							  }).ToList();

            return requisitos;
		}

		public ColaboradorRequisitoDto GetRequisito(int Id)
		{
			var d = Repository.Get(Id);

			ColaboradorRequisitoDto requisito = new ColaboradorRequisitoDto()
			{
				Id = d.Id,
				ColaboradoresId = d.ColaboradoresId,
				RequisitosId = d.RequisitosId,
                ArchivoId = d.ArchivoId,
				cumple = d.cumple,
				fecha_caducidad = d.fecha_caducidad,
                fecha_emision = d.fecha_emision,
                observacion = d.observacion,
				vigente = d.vigente
			};


			return requisito;
		}

        public async Task<int> CargarArchivoRequisito(ColaboradorRequisitoDto requisito, HttpPostedFileBase[] UploadedFile)
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
                        codigo = "COL_REQ_" + requisito.ColaboradoresId,
                        nombre = fileName,
                        vigente = true,
                        fecha_registro = DateTime.Now,
                        hash = fileBytes,
                        tipo_contenido = fileContentType,
                    };
                    var archivoid = _archivoRepository.InsertAndGetId(n);

                    ///* Registramos la archivo al colaborador */
                    //requisito.ArchivoId = archivoid;
                    
                    //var result = Repository.InsertAndGetId(MapToEntity(requisito));
                    return archivoid;
                }
            }
            return 0;
        }

        public async Task<Archivo> ActualizaArchivoRequisito(ColaboradorRequisitoDto requisito, HttpPostedFileBase[] UploadedFile)
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

                    var n = _archivoRepository.Get(requisito.ArchivoId.Value);
                    n.fecha_registro = DateTime.Now;
                    n.nombre = fileName;
                    n.hash = fileBytes;
                    n.tipo_contenido = fileContentType;

                    var result = await _archivoRepository.UpdateAsync(n);

                    return result;
                }
            }
            return null;
        }
    }
}

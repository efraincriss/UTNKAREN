using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using Abp.Extensions;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;
using Xceed.Words.NET;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ConsultaPublicaAsyncBaseCrudAppService : AsyncBaseCrudAppService<ConsultaPublica, ConsultaPublicaDto, PagedAndFilteredResultRequestDto>, IConsultaPublicaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Ciudad> _repositoryCiudad;
        private readonly IBaseRepository<Archivo> _archivoRepository;


        public ConsultaPublicaAsyncBaseCrudAppService(
            IBaseRepository<ConsultaPublica> repository,
            IBaseRepository<Ciudad> repositoryCiudad,
            IBaseRepository<Archivo> archivoRepository
            ) : base(repository)
        {
            _repositoryCiudad = repositoryCiudad;
            _archivoRepository = archivoRepository;
        }


        public List<ConsultaPublicaDto> BuscarPorIdentificacionNombre(string identificacion = "", string nombre = "")
        {
            var query = Repository.GetAll()
                .Include(o => o.TipoIdentificacion)
                ;

            if (!identificacion.IsNullOrEmpty())
            {
                query = query.Where(o => o.identificacion.StartsWith(identificacion));
            }

            if (!nombre.IsNullOrEmpty())
            {
                query = query.Where(o =>
                    o.nombres_completos.Contains(nombre));
            }

            var entities = query.ToList();
            var dto = Mapper.Map<List<ConsultaPublica>, List<ConsultaPublicaDto>>(entities);
            return dto;
        }

        public string GenerarWord(int consultaPublicaId)
        {

            var consulta = Repository.Get(consultaPublicaId);
            var ciudad = _repositoryCiudad.Get(consulta.CiudadTrabajoId);

            // Path Plantilla
            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/Accesos/Anexo10.docx");

            if (File.Exists((string)filename))
            {
                Random a = new Random();
                var valor = a.Next(1, 100000);
                string salida = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/Accesos/DocumentosGenerados/"+ DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + "_"+consulta.nombres_completos + ".docx");
             
                using (var plantilla = DocX.Load(filename))
                {
                        var document = DocX.Create(salida);
                        document.InsertDocument(plantilla);
                        document.ReplaceText("{fecha_generacion}",consulta.fecha_consulta.GetValueOrDefault().ToShortDateString());
                        document.ReplaceText("{ciudad}", ciudad.nombre);
                        document.ReplaceText("{nombre_completo}", consulta.nombres_completos);
                        document.ReplaceText("{identificacion}", consulta.identificacion);
                        document.ReplaceText("{condicion}", consulta.condicion_cedulado);

                    document.Save();
                    return salida;
                 }      
                    
            }
            else
            {
                return null;
            }
        }


        public void SubirPdf(int consultaPublicaId, ArchivoDto archivo)
        {
            var consulta = Repository.Get(consultaPublicaId);
            if (consulta.ArchivoPdfId > 0)
            {
                var archivoId = consulta.ArchivoPdfId;
                var file = consulta.ArchivoPdf;

                consulta.ArchivoPdfId = null;
                Repository.Update(consulta);
                _archivoRepository.Delete(file);

            }
            var entity = _archivoRepository.Insert(Mapper.Map<ArchivoDto, Archivo>(archivo));

            
            consulta.ArchivoPdf = entity;
            Repository.Update(consulta);
        }
    }
}


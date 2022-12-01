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

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ColaboradoresHuellaDigitalAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradoresHuellaDigital, ColaboradoresHuellaDigitalDto, PagedAndFilteredResultRequestDto>, IColaboradoresHuellaDigitalAsyncBaseCrudAppService
    {

        private readonly ICatalogoAsyncBaseCrudAppService _catalogoRepository;

        public ColaboradoresHuellaDigitalAsyncBaseCrudAppService(
            IBaseRepository<ColaboradoresHuellaDigital> repository,
            ICatalogoAsyncBaseCrudAppService catalogoRepository) 
            : base(repository)
        {
            _catalogoRepository = catalogoRepository;
        }

        public async Task<string> ActualizarHuellasPorColaboradorAsync(ColaboradoresHuellaDigitalDto colaboradoresHuellaDigital)
        {
            colaboradoresHuellaDigital.fecha_registro = DateTime.Now;
            colaboradoresHuellaDigital.vigente = true;
            var result = Repository.Update(MapToEntity(colaboradoresHuellaDigital));

            return result.ToString();
        }

        public async Task<string> CrearHuellasPorColaboradorAsync(ColaboradoresHuellaDigitalDto colaboradoresHuellaDigital)
        {
            
           colaboradoresHuellaDigital.fecha_registro = DateTime.Now;
           colaboradoresHuellaDigital.vigente = true;
           var result = Repository.InsertAndGetId(MapToEntity(colaboradoresHuellaDigital));

           return result.ToString();
        }

        public bool EliminarHuella(int Id)
        {
            var huella = Repository.Get(Id);

            if (huella != null)
            {
                huella.vigente = false;
                huella.principal = false;
                huella.IsDeleted = true;
                Repository.Update(huella);
                return true;
            }

            return false;
        }

        public ColaboradoresHuellaDigital GetHuellaDigital(int Id)
        {
            ColaboradoresHuellaDigital huella = Repository.Get(Id);
            return huella;
        }

        public ColaboradoresHuellaDigital GetHuellaPorDedoColaborador(int IdColoaborador, int IdDedo)
        {
            ColaboradoresHuellaDigital huella = Repository.GetAll()
                .Where(a => a.colaborador_id == IdColoaborador &&  a.catalogo_dedo_id == IdDedo )
                .FirstOrDefault();
            return huella;
        }

        public List<ColaboradoresHuellaDigitalDto> GetHuellasPorColaborador(int Id)
        {
            var e = 1;
            var nr = 1;
            var query = Repository.GetAll();

            var huellaColaborador = (from d in query 
                               where d.vigente == true && d.colaborador_id == Id
                                     select new ColaboradoresHuellaDigitalDto
                                     {
                                       Id = d.Id,
                                       fecha_registro = d.fecha_registro,
                                       catalogo_dedo_id = d.catalogo_dedo_id,
                                       colaborador_id = d.colaborador_id,
                                       principal = d.principal,
                                       huella = d.huella,
                                       plantilla_base64 = d.plantilla_base64,
                                       vigente = d.vigente,
                               }).ToList();

            foreach (var h in huellaColaborador)
            {
                var catalogoIdentificacion = _catalogoRepository.GetCatalogo(h.catalogo_dedo_id);
                h.dedo = catalogoIdentificacion.nombre;

                h.nro_huella = nr;

                if (h.principal == true)
                {
                    h.esPrincipal = "SI";
                }else
                {
                    h.esPrincipal = "NO";
                }
                nr++;
            }

            return huellaColaborador;
        }
        
    }
}

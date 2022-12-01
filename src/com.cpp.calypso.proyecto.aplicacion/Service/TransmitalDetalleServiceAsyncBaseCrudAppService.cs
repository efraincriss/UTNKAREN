using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class TransmitalDetalleServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<TransmitalDetalle, TransmitalDetalleDto, PagedAndFilteredResultRequestDto>, ITransmitalDetalleAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<TransmitalCabecera> _rtransmitalcabecera;
        public TransmitalDetalleServiceAsyncBaseCrudAppService(IBaseRepository<TransmitalDetalle> repository,
            IBaseRepository<TransmitalCabecera> rtransmitalcabecera) : base(repository)
        {
            _rtransmitalcabecera = rtransmitalcabecera;
        }

        public TransmitalDetalleDto GetDetalle(int TransmitalDetalleId)
        {
            var Query = Repository.GetAllIncluding(c => c.Transmital, c => c.Archivo).Where(e => e.vigente == true).ToList();

            var item = (from r in Query
                        where r.Id == TransmitalDetalleId
                        where r.vigente == true
                        select new TransmitalDetalleDto()
                        {
                            Id = r.Id,
                            
                            TransmitalId = r.TransmitalId,
                            Transmital = r.Transmital,
                            descripcion = r.descripcion,
                            vigente = r.vigente,
                            codigo_detalle = r.codigo_detalle,
                            nro_copias = r.nro_copias,
                            nro_hojas = r.nro_hojas,
                            ArchivoId = r.ArchivoId,
                            es_oferta = r.es_oferta
                        }).FirstOrDefault(); // Tercera
            return item;
        }

        public List<TransmitalDetalleDto> GetTransmitalDetalles(int TransmitalId)
        {
            var Query = Repository.GetAllIncluding(c => c.Transmital, c => c.Transmital.OfertaComercial, c => c.Archivo)
                                                   .Where(c=>c.TransmitalId== TransmitalId)
                                                .OrderByDescending(c=>c.es_oferta);
            var items = (from r in Query
                         where r.vigente == true
                         where r.TransmitalId == TransmitalId
                         select new TransmitalDetalleDto()
                         {
                             Id = r.Id,
                             TransmitalId = r.TransmitalId,
                             descripcion = r.descripcion,
                             vigente = r.vigente,
                             codigo_detalle = r.codigo_detalle,
                             nro_copias = r.nro_copias,
                             nro_hojas = r.nro_hojas,
                             ArchivoId = r.ArchivoId,
                             es_oferta = r.es_oferta,
                             version=r.version,
                             

                            estado_es_oferta=r.es_oferta?"SI":"NO",
                            nombre_archivo=r.Archivo.nombre,

                         }).ToList();
            return items;
        }

        public bool EliminarVigencia(int Transmitadetalleid)
        {

            var oferta = Repository.Get(Transmitadetalleid);

            if (oferta != null)
            {
                oferta.vigente = false;
                Repository.Update(oferta);
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool existe_esoferta(int id)
        {
            var existe = Repository.GetAll().Where(c => c.vigente).Where(c => c.es_oferta).Where(c => c.TransmitalId == id).ToList();
            if (existe.Count > 0) {
                return true;
            }
            return false;

        }
    }
}

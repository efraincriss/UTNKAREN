using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class CentrocostosContratoServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<CentrodecostosContrato, CentrocostosContratoDto, PagedAndFilteredResultRequestDto>, ICentrocostoContratoAsyncBaseCrudAppService
    {
        public CentrocostosContratoServiceAsyncBaseCrudAppService(IBaseRepository<CentrodecostosContrato> repository) : base(repository)
        {

        }

        public async Task<CentrocostosContratoDto> GetDetalle(int CentrodecostosContratoId)
        {
            var centrocostosQuery = Repository.GetAllIncluding(c=>c.Contrato).Where(c=>c.vigente==true);
            CentrocostosContratoDto item = (from c in centrocostosQuery

                                            where c.Id == CentrodecostosContratoId
                                            select new CentrocostosContratoDto
                                            {
                                                Id = c.Id,
                                                ContratoId = c.ContratoId,
                                                id_centrocostos = c.id_centrocostos,
                                                observaciones = c.observaciones,
                                                estado = c.estado,
                                                vigente = c.vigente,
                                                Contrato = c.Contrato
                                            }).FirstOrDefault();

            return item;
        }
    }
}


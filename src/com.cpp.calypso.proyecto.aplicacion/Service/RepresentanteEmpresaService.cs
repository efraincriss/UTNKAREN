using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class RepresentanteEmpresaAsyncBaseCrudAppService : AsyncBaseCrudAppService<RepresentanteEmpresa, RepresentanteEmpresaDto, PagedAndFilteredResultRequestDto>, IRepresentanteEmpresaAsyncBaseCrudAppService
    {
        public RepresentanteEmpresaAsyncBaseCrudAppService(IBaseRepository<RepresentanteEmpresa> repository) : base(repository)
        {

        }

        public RepresentanteEmpresaDto GetDetalles(int representanteId)
        {
            var representanteQuery = Repository.GetAll();
            var item = (from r in representanteQuery
                        where r.Id == representanteId
                        select new RepresentanteEmpresaDto()
                {
                    Id = r.Id,
                    tipo_identificacion = r.tipo_identificacion,
                    identificacion = r.identificacion,
                    nombre = r.nombre,
                    fecha_inicio = r.fecha_inicio,
                    fecha_fin = r.fecha_fin,
                    vigente = r.vigente,
                    tipo_representante = r.tipo_representante,
                    estado_representante = r.estado_representante,
                    EmpresaId = r.EmpresaId
                }).SingleOrDefault();
            return item;
        }

        public int EliminarVigencia(int representanteId)
        {
            var representanteQuery = Repository.GetAll();
            var item = (from r in representanteQuery
                where r.Id == representanteId
                select new RepresentanteEmpresaDto()
                {
                    Id = r.Id,
                    tipo_identificacion = r.tipo_identificacion,
                    identificacion = r.identificacion,
                    nombre = r.nombre,
                    fecha_inicio = r.fecha_inicio,
                    fecha_fin = r.fecha_fin,
                    vigente = r.vigente,
                    tipo_representante = r.tipo_representante,
                    estado_representante = r.estado_representante,
                    EmpresaId = r.EmpresaId
                }).SingleOrDefault();
            item.vigente = false;
            Repository.Update(MapToEntity(item));
                            //Mapper.Map<RepresentanteEmpresa>(item)
            return item.EmpresaId;

        }

        public string CrearRepresentante(RepresentanteEmpresaDto representanteEmpresa)
        {
            representanteEmpresa.vigente = true;
            if (representanteEmpresa.tipo_identificacion.Equals("Cedula"))
            {
                var cedulaValida = ValidacionIdentificacion.VerificarCedula(representanteEmpresa.identificacion);
                if (!cedulaValida)
                {
                    return "cedula";
                }
            }
            else if (representanteEmpresa.tipo_identificacion.Equals("Ruc"))
            {
                if (representanteEmpresa.identificacion.Length != 13)
                {
                    return "ruc";
                }
            }

            var result = Repository.InsertAndGetId(MapToEntity(representanteEmpresa));
            return result.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class CuentaEmpresaAsyncBaseCrudAppService : AsyncBaseCrudAppService<CuentaEmpresa, CuentaEmpresaDto, PagedAndFilteredResultRequestDto>, ICuentaEmpresaAsyncBaseCrudAppService
    {
        public CuentaEmpresaAsyncBaseCrudAppService(IBaseRepository<CuentaEmpresa> repository) : base(repository)
        {
        }

        public CuentaEmpresaDto GetDetalles(int cuentaId)
        {
            var cuentaQuery = Repository.GetAllIncluding(c => c.InstitucionFinanciera, c => c.Empresa);
            var item = (from c in cuentaQuery
                where c.Id == cuentaId
                select new CuentaEmpresaDto()
                {
                    Id = c.Id,
                    EmpresaId = c.EmpresaId,
                    InstitucionFinanciera = c.InstitucionFinanciera,
                    estado = c.estado,
                    numero_cuenta = c.numero_cuenta,
                    tipo_cuenta = c.tipo_cuenta,
                    vigente = c.vigente,
                    InstitucionFinancieraId = c.InstitucionFinancieraId,
                    Empresa = c.Empresa
                }).SingleOrDefault();
            return item;
        }

        public int EliminarVigencia(int cuentaId)
        {
            var cuenta = this.GetDetalles(cuentaId);
            cuenta.vigente = false;
            cuenta.InstitucionFinancieraId = cuenta.InstitucionFinanciera.Id;
            Repository.Update(Mapper.Map<CuentaEmpresa>(cuenta));
            return cuenta.EmpresaId;
        }
    }
}

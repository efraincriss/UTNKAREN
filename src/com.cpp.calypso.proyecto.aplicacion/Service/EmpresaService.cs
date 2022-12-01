using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.aplicacion;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.proyecto.aplicacion;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class EmpresaServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<Empresa, EmpresaDto, PagedAndFilteredResultRequestDto>, IEmpresaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<RepresentanteEmpresa> repositoryRepresentante;
        private readonly IBaseRepository<CuentaEmpresa> repositoryCuentaEmpresa;
        private readonly IBaseRepository<Contrato> _contratoRepository;

        public EmpresaServiceAsyncBaseCrudAppService(
            IBaseRepository<Empresa> repository, 
            IBaseRepository<RepresentanteEmpresa> repositoryRepresentante,
            IBaseRepository<CuentaEmpresa> repositoryCuentaEmpresa,
            IBaseRepository<Contrato> contratoRepository
            ) : base(repository)
        {
            this.repositoryRepresentante = repositoryRepresentante;
            this.repositoryCuentaEmpresa = repositoryCuentaEmpresa;
            _contratoRepository = contratoRepository;
        }

        public Empresa CrearEmpresa (Empresa empresa)
        {
            var empresa_new = Repository.Insert(empresa);
            return empresa_new;
        }


        public async Task<EmpresaDto> GetDetalle(int empresaId)
        {
            var empresaQuery = Repository.GetAll();
            // .GetAll();
            //.GetAllIncluding(d => d.)
            var item = (from empresa in empresaQuery                        
                where empresa.Id == empresaId
                select  new EmpresaDto
                {
                    Id = empresa.Id,
                    tipo_identificacion = empresa.tipo_identificacion,
                    identificacion = empresa.identificacion,
                    razon_social = empresa.razon_social,
                    direccion = empresa.direccion,
                    correo = empresa.correo,
                    estado = empresa.estado,
                    telefono = empresa.telefono,
                    tipo_sociedad = empresa.tipo_sociedad,
                    observaciones = empresa.observaciones,
                    es_principal = empresa.es_principal,
                    tipo_contribuyente = empresa.tipo_contribuyente, 
                    vigente = empresa.vigente,     
                    lider_operaciones=empresa.lider_operaciones
                }).SingleOrDefault();    
            return item;
        }


        public List<RepresentanteEmpresaDto> GetRepresentanteEmpresa(int empresaId)
        {
            var representantesQuery = repositoryRepresentante.GetAll();

            var representantes = (from r in representantesQuery
                where r.EmpresaId == empresaId
                where r.vigente == true
                select new RepresentanteEmpresaDto
                {
                    Id = r.Id,
                    nombre = r.nombre,
                    fecha_inicio = r.fecha_inicio,
                    fecha_fin = r.fecha_fin,
                    estado_representante = r.estado_representante
                }).ToList();
            return representantes;
        }

        public List<CuentaEmpresaDto> GetCuentasEmpresa(int empresaId)
        {
            var cuentasQuery = repositoryCuentaEmpresa.GetAllIncluding(e => e.InstitucionFinanciera);
            var cuentas_empresa = (from c in cuentasQuery
                where c.EmpresaId == empresaId
                where c.vigente == true
                select new CuentaEmpresaDto
                {
                    Id = c.Id,
                    tipo_cuenta = c.tipo_cuenta,
                    numero_cuenta = c.numero_cuenta,
                    estado = c.estado,
                    vigente = c.vigente,
                    nombre_banco = c.InstitucionFinanciera.nombre
                }).ToList();

            return cuentas_empresa;
        }

        public async Task CancelarVigencia(int empresaId)
        {
            var empresaQuery = Repository.GetAll();
            var item = (from empresa in empresaQuery
                        where empresa.Id == empresaId
                        select new Empresa
                        {
                            Id = empresa.Id,
                            tipo_identificacion = empresa.tipo_identificacion,
                            identificacion = empresa.identificacion,
                            razon_social = empresa.razon_social,
                            direccion = empresa.direccion,
                            correo = empresa.correo,
                            estado = empresa.estado,
                            telefono = empresa.telefono,
                            tipo_sociedad = empresa.tipo_sociedad,
                            observaciones = empresa.observaciones,
                            es_principal = empresa.es_principal,
                            tipo_contribuyente = empresa.tipo_contribuyente,
                            vigente = empresa.vigente
                        }
                        ).FirstOrDefault();
            item.vigente = false;
            Repository.InsertOrUpdate(item);
            
        }

        public List<Empresa> GetEmpresas()
        {
            var empresaQuery = Repository.GetAll().Where(e => e.vigente == true).ToList();
            return empresaQuery;
        }

        public List<EmpresaDto> GetEmpresasApi()
        {
            var empresaQuery = Repository.GetAll();
            var empresas = (from empresa in empresaQuery
                            select new EmpresaDto
                {
                    Id = empresa.Id,
                    tipo_identificacion = empresa.tipo_identificacion,
                    identificacion = empresa.identificacion,
                    razon_social = empresa.razon_social,
                    direccion = empresa.direccion,
                    correo = empresa.correo,
                    estado = empresa.estado,
                    telefono = empresa.telefono,
                    tipo_sociedad = empresa.tipo_sociedad,
                    observaciones = empresa.observaciones,
                    es_principal = empresa.es_principal,
                    tipo_contribuyente = empresa.tipo_contribuyente,
                    vigente = empresa.vigente
                }
            ).ToList();
            return empresas;
        }

        public bool ComprobarYBorrarEmpresa(int empresaId)
        {
            bool res = true;
            var empresaQuery = Repository.GetAll();
            var iteEmpresaDto = (from empresa in empresaQuery
                where empresa.Id == empresaId
                select new EmpresaDto
                {
                    Id = empresa.Id,
                    tipo_identificacion = empresa.tipo_identificacion,
                    identificacion = empresa.identificacion,
                    razon_social = empresa.razon_social,
                    direccion = empresa.direccion,
                    correo = empresa.correo,
                    estado = empresa.estado,
                    telefono = empresa.telefono,
                    tipo_sociedad = empresa.tipo_sociedad,
                    observaciones = empresa.observaciones,
                    es_principal = empresa.es_principal,
                    tipo_contribuyente = empresa.tipo_contribuyente,
                    vigente = empresa.vigente,                                     
                }).SingleOrDefault();

            
            var hijosActivos = false;

            var count = 0;

            var cuentas =
            this.GetCuentasEmpresa(empresaId).Count > 0 ? count++ : count;

            var representantes =
                this.GetRepresentanteEmpresa(empresaId).Count > 0 ? count++ : count;

            var contratosQuery = _contratoRepository
                .GetAll()
                .Where(o
                    => o.vigente == true).Count(o => o.EmpresaId == iteEmpresaDto.Id);

            var contratos = contratosQuery > 0 ? count++ : count;

            var b = count > 0 ? hijosActivos = true : hijosActivos = false;

            if (hijosActivos)
            {                
                return false;
            }
            else
            {
                iteEmpresaDto.vigente = false;
                Repository.Update(Mapper.Map<Empresa>(iteEmpresaDto));
                return true;
            }          
        }



        public async Task<string> CrearEmpresaAsync(EmpresaDto empresa)
        {
            empresa.vigente = true;
            if (empresa.tipo_identificacion == TipoDeIdentificacion.Cedula)
            {
                var cedulaValida = ValidacionIdentificacion.VerificarCedula(empresa.identificacion);
                if (!cedulaValida)
                {
                    return "cedula";
                }
            }
            else if (empresa.tipo_identificacion == TipoDeIdentificacion.RUC)
            {
                if (empresa.identificacion.Length != 13)
                {
                    return "ruc";
                }
            }

            var result = Repository.InsertAndGetId(MapToEntity(empresa));
            return result.ToString();
        }


    }
}

using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ClienteServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<Cliente, ClienteDto, PagedAndFilteredResultRequestDto>, IClienteAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Contrato>_repositorycontrato;
        public ClienteServiceAsyncBaseCrudAppService(IBaseRepository<Cliente> repository, IBaseRepository<Contrato> repositorycontrato) : base(repository)
        {
            _repositorycontrato = repositorycontrato;
        }

        public List<Cliente> GetClientes()
        {
            var clienteQuery = Repository.GetAll().Where(e => e.vigente == true).ToList();
            return clienteQuery;
        }

        public List<ClienteDto> GetClientesApi()
        {
            var clientequery = Repository.GetAll().Where(e => e.vigente == true).ToList();
            List<ClienteDto> item = (from c in clientequery
                select new ClienteDto
                {
                    Id = c.Id,
                    identificacion = c.identificacion,
                    tipo_identificacion = c.tipo_identificacion,
                    correo = c.correo,
                    direccion = c.direccion,
                    es_grupo = c.es_grupo,
                    fecha_fin = c.fecha_fin,
                    fecha_registro = c.fecha_registro,
                    razon_social = c.razon_social,
                    telefono = c.telefono,
                    tiene_contrato = c.tiene_contrato,
                    codigoasignado = c.codigoasignado,
                    tipo_contribuyente = c.tipo_contribuyente,
                    estado = c.estado,
                    vigente = c.vigente,
                    representate_legal = c.representate_legal,
                    projectmanager = c.projectmanager,
                    lider_operaciones=c.lider_operaciones
                }).ToList();

            return item;
        }

        public async Task<ClienteDto> GetDetalle(int ClienteId)
        {
            var clientequery = Repository.GetAll().Where(e => e.vigente == true).ToList();
            ClienteDto item = (from c in clientequery

                where c.Id == ClienteId
                select new ClienteDto
                {
                    Id = c.Id,
                   identificacion = c.identificacion,
                   tipo_identificacion = c.tipo_identificacion,
                   correo = c.correo,
                    direccion = c.direccion,
                    es_grupo = c.es_grupo,
                    fecha_fin = c.fecha_fin,
                    fecha_registro =c.fecha_registro,
                    razon_social = c.razon_social,
                    telefono = c.telefono,
                    tiene_contrato = c.tiene_contrato,
                    codigoasignado = c.codigoasignado,
                   tipo_contribuyente = c.tipo_contribuyente,
                    estado = c.estado,
                   vigente = c.vigente,
                     representate_legal = c.representate_legal,
                    projectmanager = c.projectmanager,
                    lider_operaciones=c.lider_operaciones
                }).FirstOrDefault();

            return item;
        }
        public async Task<string> CrearClienteAsync(ClienteDto cliente)
        {
            cliente.vigente = true;
            List<Cliente> todos = Repository.GetAll().Where(e => e.vigente == true).ToList();

            foreach (var clientes in todos)
            {
                if (cliente.identificacion == clientes.identificacion)
                {
                    return "unica";
                }                
            }
            if (cliente.tipo_identificacion == TipoDeIdentificacion.Cedula)
            {
                var cedulaValida = ValidacionIdentificacion.VerificarCedula(cliente.identificacion);
                if (!cedulaValida)
                {
                    return "cedula";
                }
            }
            else if (cliente.tipo_identificacion == TipoDeIdentificacion.RUC)
            {
                if (cliente.identificacion.Length != 13)
                {
                    return "ruc";
                }
            }

            var result = Repository.InsertAndGetId(MapToEntity(cliente));
            return result.ToString();
        }
        public async Task<string> ActualizarClienteAsync(ClienteDto cliente)
        {
            int idcliente = cliente.Id;
            cliente.vigente = true;
            List<Cliente> todos = Repository.GetAll().Where(e => e.vigente == true).Where(e=>e.Id!=cliente.Id).ToList();
            foreach (var clientes in todos)
            {
                if (cliente.identificacion == clientes.identificacion )
                {
                    return "unica";
                }
            }
            if (cliente.tipo_identificacion == TipoDeIdentificacion.Cedula)
            {
                var cedulaValida = ValidacionIdentificacion.VerificarCedula(cliente.identificacion);
                if (!cedulaValida)
                {
                    return "cedula";
                }
            }
            else if (cliente.tipo_identificacion == TipoDeIdentificacion.RUC)
            {
                if (cliente.identificacion.Length != 13)
                {
                    return "ruc";
                }
            }

            var result = Repository.InsertOrUpdateAndGetId(MapToEntity(cliente));
            return result.ToString();
        }

        public List<ContratoDto> GetContratosporCliente(int ClienteId)
        {
            var contratosquery = _repositorycontrato.GetAllIncluding(c=>c.Cliente).Where(c=>c.vigente==true);
            var contratos = (from c in contratosquery
                where c.ClienteId ==ClienteId && c.vigente == true
                select new ContratoDto
                {
                    Id = c.Id,
                    EmpresaId = c.EmpresaId,
                    ClienteId = c.ClienteId,
                    Codigo = c.Codigo,
                    id_modalidad = c.id_modalidad,
                    objeto = c.objeto,
                    descripcion = c.descripcion,
                    fecha_firma = c.fecha_firma,
                    fecha_inicio = c.fecha_inicio,
                    fecha_fin = c.fecha_fin,
                    dias_emision_oferta = c.dias_emision_oferta,
                    dias_plazo_oferta_requerimiento = c.dias_plazo_oferta_requerimiento,
                    dias_plazo_factura = c.dias_plazo_factura,
                    documento_factura_plazo = c.documento_factura_plazo,
                    dias_plazo_certificacion_mensual = c.dias_plazo_certificacion_mensual,
                    dias_garantia_ingenieria = c.dias_garantia_ingenieria,
                    dias_garantia_suministros = c.dias_garantia_suministros,
                    dias_garantia_construccion = c.dias_garantia_construccion,
                    estado_contrato = c.estado_contrato,
                    fecha_acta_entrega_recepcion = c.fecha_acta_entrega_recepcion,
                    vigente = c.vigente
                }).ToList();

            return contratos;
        }

        public bool EliminarCliente(int Id)
        {
            var r = Repository.Get(Id);
            r.vigente = false;
            var s = Repository.Update(r);
            if (s.Id >= 0)
            {
                return true;

            }
            else
            {
                return false;
            }
            
        }
    }
    }


using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Transporte;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Service
{

    public class ChoferAsyncBaseCrudAppService : AsyncBaseCrudAppService<Chofer, ChoferDto, PagedAndFilteredResultRequestDto>, IChoferAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<dominio.Proveedor.Proveedor> _proveedorRepository;
        private readonly IBaseRepository<ServicioProveedor> _servicioproveedor;
        private readonly IBaseRepository<Catalogo> _catalogoRespository;
        private readonly IdentityEmailMessageService _correoservice;


        public ChoferAsyncBaseCrudAppService(
            IBaseRepository<Chofer> repository,
            IBaseRepository<dominio.Proveedor.Proveedor> proveedorRepository,
            IBaseRepository<ServicioProveedor> servicioproveedor,
            IBaseRepository<Catalogo> catalogoRespository,

        //EMail
            IdentityEmailMessageService correoservice

            ) : base(repository)
        {
            _proveedorRepository = proveedorRepository;
            _servicioproveedor = servicioproveedor;
            _catalogoRespository = catalogoRespository;
            _correoservice = correoservice;
        }

        public Chofer BuscarChofer(string NumeroIdentificacion)
        {
            var ChoferEncontrado = Repository.GetAll().Where(c => c.NumeroIdentificacion == NumeroIdentificacion).FirstOrDefault();
            return ChoferEncontrado;
        }
        public Chofer BuscarChoferProveedor(string NumeroIdentificacion, int proveedorid)
        {
            var ChoferEncontrado = Repository.GetAll().Where(c => c.NumeroIdentificacion == NumeroIdentificacion)
                                                    .Where(c => c.TipoIdentificacionId == proveedorid).FirstOrDefault();
            return ChoferEncontrado;
        }
        public Chofer BuscarChoferProveedorEditar(string NumeroIdentificacion, int proveedorid, int id)
        {
            var ChoferEncontrado = Repository.GetAll().Where(c => c.NumeroIdentificacion == NumeroIdentificacion)
                                                    .Where(c => c.TipoIdentificacionId == proveedorid)
                                                    .Where(c => c.Id != id)
                                                    .FirstOrDefault();
            return ChoferEncontrado;
        }


        public int EditarChofer(Chofer chofer)

        {
            var E = Repository.Get(chofer.Id);
            E.FechaEstado = DateTime.Now;
            E.Estado = chofer.Estado;
            E.ProveedorId = chofer.ProveedorId;
            E.GeneroId = chofer.GeneroId;
            //E.EstadoCivilId = chofer.EstadoCivilId;
            E.Nombres = chofer.Nombres;
            E.Apellidos = chofer.Apellidos;
            E.ApellidosNombres = chofer.ApellidosNombres;
            E.Celular = chofer.Celular;
            E.Mail = chofer.Mail;
            E.FechaNacimiento = chofer.FechaNacimiento;
            var Chofer = Repository.Update(E);
            return E.Id;
        }

        public int EliminarChofer(int id)
        {
            var chofer = Repository.Get(id);
            Repository.Delete(chofer);
            return chofer.Id;
        }

        public Chofer GetDetalles(int id)
        {
            var chofer = Repository.GetAll().Where(c => c.Id == id).FirstOrDefault();
            return chofer;
        }

        public int IngresarChofer(Chofer chofer)
        {
            chofer.Estado = ChoferEstado.Activo;
            chofer.FechaEstado = DateTime.Now;
            var id = Repository.InsertAndGetId(chofer);
            return id;
        }

        public List<ProveedorDto> ListaProveedoresTransporte()
        {

            var proveedores_transporte = _servicioproveedor.GetAll().Where(c => c.Servicio.codigo == CatalogosCodigos.SERVICIO_TRANSPORTE)
                                                                    .Where(c => c.estado == ServicioEstado.Activo)
                                                                    .Where(c => c.Proveedor.estado == dominio.Proveedor.ProveedorEstado.Activo)
                                                                    .Select(c => c.Proveedor)
                                                                    .ToList();
            var proveedorDto = (from p in proveedores_transporte
                                select new ProveedorDto()
                                {
                                    Id = p.Id,
                                    razon_social = p.razon_social

                                }
                              ).ToList();

            return proveedorDto;
        }

        public List<ChoferDto> Listar()
        {
            var query = Repository.GetAll().Include(c => c.TipoIdentificacion).Include(c => c.Proveedor).ToList();
            var choferes = (from c in query
                            select new ChoferDto()
                            {
                                Id = c.Id,
                                NombreTipoIdentificacion = c.TipoIdentificacion.nombre,
                                NumeroIdentificacion = c.NumeroIdentificacion,
                                Apellidos = c.Apellidos,
                                Nombres = c.Nombres,
                                Estado = c.Estado,
                                NombreEstado = Enum.GetName(typeof(ChoferEstado), c.Estado).ToUpper(),
                                NombreProveedor = c.Proveedor.razon_social,
                                ApellidosNombres = c.ApellidosNombres

                            }).ToList();
            return choferes;

        }

        public async Task<List<ChoferDto>> GetLookupAll()
        {
            var query = Repository.GetAll();

            var lookupDtos = await (from c in query
                                    where c.Estado == ChoferEstado.Activo
                                    select new ChoferDto
                                    {
                                        Id = c.Id,
                                        NumeroIdentificacion = c.NumeroIdentificacion,
                                        ApellidosNombres = c.ApellidosNombres
                                    }
                               ).ToListAsync();
            return lookupDtos;
        }

        public async Task<bool> EnviarCorreo()
        {
            try
            {
                IdentityMessage nuevo = new IdentityMessage() {
                    Subject = "Hola q hace!!!",
                    Body="Hola q hace !!!",
                    Destination="efrain@atikasoft.com"
                };
                 await _correoservice.SendAsync(nuevo);
                return true;

            }
            catch (Exception ex)
            {
                ElmahExtension.LogToElmah(ex);
                return false;
            }
        }
    }
}

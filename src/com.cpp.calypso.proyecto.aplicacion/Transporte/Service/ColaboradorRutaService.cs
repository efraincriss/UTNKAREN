using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Models;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Transporte;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Service
{
    public class ColaboradorRutaAsyncBaseCrudAppService : comun.aplicacion.AsyncBaseCrudAppService<ColaboradorRuta, ColaboradorRutaDto, PagedAndFilteredResultRequestDto>, IColaboradorRutaAsyncBaseCrudAppService
    {

        private readonly IBaseRepository<Vehiculo> _vehiculoRepository;
        private readonly IBaseRepository<Ruta> _rutaRepository;
        private readonly IBaseRepository<RutaHorario> _horarioRepository;
        private readonly IBaseRepository<Parada> _paradaRepository;
        private readonly IBaseRepository<Colaboradores> _colaboradoresRepository;
        private readonly IBaseRepository<ColaboradorRuta> _colaboradorrutaRepository;
        private readonly IBaseRepository<Contacto> _contactoRepository;
        private readonly IBaseRepository<RutaParada> _rutaparadaRepository;

        //Reportes

        private readonly IBaseRepository<ConsumoTransporte> _consumoRepository;
        private readonly IdentityEmailMessageService _correoservice;
        public ColaboradorRutaAsyncBaseCrudAppService(
            IBaseRepository<ColaboradorRuta> repository,
             IBaseRepository<Vehiculo> vehiculoRepository,
            IBaseRepository<Ruta> rutaRepository,
            IBaseRepository<RutaHorario> horarioRepository,
            IBaseRepository<Parada> paradaRepository,
            IBaseRepository<Colaboradores> colaboradoresRepository,
             IBaseRepository<Contacto> contactoRepository,
             IBaseRepository<RutaParada> rutaparadaRepository,
             IBaseRepository<ColaboradorRuta> colaboradorrutaRepository,
             IdentityEmailMessageService correoservice
            ) : base(repository)
        {
            _vehiculoRepository = vehiculoRepository;
            _rutaRepository = rutaRepository;
            _horarioRepository = horarioRepository;
            _paradaRepository = paradaRepository;
            _colaboradoresRepository = colaboradoresRepository;
            _contactoRepository = contactoRepository;
            _rutaparadaRepository = rutaparadaRepository;
            _colaboradorrutaRepository = colaboradorrutaRepository;
            _correoservice = correoservice;
        }

        public ColaboradoresDetallesDto BuscarColaborador(string NumeroIdentificacion)
        {
            var colaboradorbusqueda = _colaboradoresRepository.GetAll().Where(c => c.numero_identificacion == NumeroIdentificacion).FirstOrDefault();

            if (colaboradorbusqueda != null)
            {
                var colaborador = Mapper.Map<Colaboradores, ColaboradoresDetallesDto>(colaboradorbusqueda);
                return colaborador;
            }
            return new ColaboradoresDetallesDto();
        }

        public ColaboradorRuta BuscarColaboradoRuta(int ColaboradorId, int RutaHorarioId, int Id = 0)
        {
            var encontrado = Repository.GetAll().Where(c => c.ColaboradorId == ColaboradorId)
                                              .Where(c => c.RutaHorarioId == RutaHorarioId)
                                              .Where(c => c.Id != Id)
                                              .FirstOrDefault();
            return encontrado != null ? encontrado : new ColaboradorRuta();

        }

        public int Editar(ColaboradorRuta ruta)
        {
            var r = Repository.Get(ruta.Id);
            r.RutaHorarioId = ruta.RutaHorarioId;
            r.Observacion = ruta.Observacion;

            var actualizado = Repository.Update(r);
            return actualizado.Id;
        }

        public int Eliminar(int id)
        {
            var ruta = Repository.Get(id);
            Repository.Delete(ruta);
            return ruta.Id;
        }

        public async Task EnviarMensajeAsync(int id)
        {  var rutaAsignada = Repository.GetAllIncluding(c=>c.RutaHorario, c=>c.RutaHorario.Ruta.Origen,c=>c.RutaHorario.Ruta.Destino).Where(c=>c.Id==id).FirstOrDefault();
            var Colaboraborador = _colaboradoresRepository.Get(rutaAsignada.ColaboradorId);

            string parada = "";

            if (Colaboraborador.ContactoId.HasValue && Colaboraborador.ContactoId.Value > 0) {

                var contactoemail = _contactoRepository.Get(Colaboraborador.ContactoId.Value);

                if (contactoemail != null && contactoemail.correo_electronico.Length > 0) {

                    var paradas = _rutaparadaRepository.GetAll().Where(c => c.RutaId == rutaAsignada.RutaHorario.RutaId).Select(c=>c.Parada.Nombre).ToList();
                    if (paradas.Count > 0) {
                        parada = String.Join("<br/>", paradas);
                    }


                try
                {
                        //Usuario

                        //Configuración del Mensaje
                        IdentityMessage cuerpo = new IdentityMessage();
                        cuerpo.Subject = "PMDIS: Asignación de Ruta de Transportes";
                        //Aquí ponemos el mensaje que incluirá el correo
                        cuerpo.Body =" Estimado "+Colaboraborador.nombres_apellidos+":<br/>Se informa que ha sido asignada la ruta "+ rutaAsignada.RutaHorario.Ruta.Nombre+ " para su utilización: <br/>Horario: "+rutaAsignada.RutaHorario.Horario+".<br/> Lugar Origen: "+rutaAsignada.RutaHorario.Ruta.Origen.Nombre+ "<br/>Lugar Destino: " + rutaAsignada.RutaHorario.Ruta.Destino.Nombre+ " <br/>Paradas:<br/> " + parada+ "<br/> <br/>" +
                    "Puede hacer uso del servicio de transporte";
                      //Especificamos a quien enviaremos el Email, no es necesario que sea Gmail, puede ser cualquier otro proveedor
                  
                    cuerpo.Destination=contactoemail.correo_electronico;
                    //Si queremos enviar archivos adjuntos tenemos que especificar la ruta en donde se encuentran

                    await _correoservice.SendAsync(cuerpo);

                    }
                catch (Exception ex)
                {
                    Logger.Debug(ex.Message);
                    Logger.Error(ex.Message);
                    Logger.Warn(ex.Message);
                    ElmahExtension.LogToElmah(ex);

                }
                }

            }


        }

        public ColaboradorRuta GetDetalles(int id)
        {
            var ruta = Repository.GetAll().Where(c => c.Id == id).Include(c => c.RutaHorario).Where(c => !c.RutaHorario.IsDeleted).FirstOrDefault();
            return ruta;
        }

        public Ruta GetDetallesRuta(int id)
        {
            var ruta = _horarioRepository.GetAll().Where(c => c.Id == id).Select(c=>c.Ruta)            
                
                .Include(c => c.Origen).Include(c => c.Destino).Include(c => c.Sector).FirstOrDefault();
            return ruta;
        }

        public int Ingresar(ColaboradorRuta ruta)
        {
            ruta.FechaAsignacion = DateTime.Now;
            ruta.UsuarioAsignacion = System.Web.HttpContext.Current.User.Identity.Name.ToString();
            ruta.Estado = ColaboradorRutaAsignada.Asignado;
            var nuevo = Repository.InsertAndGetId(ruta);

            return nuevo;
        }

        public List<ColaboradorRutaDto> Listar()
        {
            var query = Repository.GetAll().Include(c => c.RutaHorario.Ruta.Origen)
                                           .Where(c => !c.RutaHorario.IsDeleted)
                                           .Where(c => !c.RutaHorario.Ruta.IsDeleted)
                                           .ToList();
            var choferes = (from c in query
                            select new ColaboradorRutaDto()
                            {
                                Id = c.Id,
                                NombreOrigen = c.RutaHorario.Ruta.Origen.Nombre,
                                NombreDestino = c.RutaHorario.Ruta.Destino.Nombre,
                                Sector = c.RutaHorario.Ruta.Descripcion,
                                ColaboradorId = c.ColaboradorId,
                                RutaHorarioId = c.RutaHorarioId,
                                Colaborador = Mapper.Map<Colaboradores, ColaboradoresDetallesDto>(_colaboradoresRepository.Get(c.ColaboradorId)),
                                Observacion = c.Observacion
                            }).ToList();
            return choferes;
        }

        public List<ColaboradorRutaDto> ListarbyColaborador(int id)
        {
            var query = Repository.GetAll().Include(c => c.RutaHorario.Ruta.Origen)
                                           .Include(c => c.RutaHorario.Ruta.Destino)
                                           .Include(c => c.RutaHorario.Ruta.Sector)
                                           .Where(c=>c.ColaboradorId==id)
                                           .Where(c=>!c.RutaHorario.IsDeleted)
                                           .Where(c=>!c.RutaHorario.Ruta.IsDeleted)
                                           .ToList();
            var choferes = (from c in query
                            select new ColaboradorRutaDto()
                            {
                                Id = c.Id,
                                NombreOrigen = c.RutaHorario.Ruta.Origen.Nombre,
                                NombreDestino = c.RutaHorario.Ruta.Destino.Nombre,
                                Sector = c.RutaHorario.Ruta.Sector.nombre,
                                ColaboradorId = c.ColaboradorId,
                                RutaHorarioId = c.RutaHorarioId,
                                Colaborador = Mapper.Map<Colaboradores, ColaboradoresDetallesDto>(_colaboradoresRepository.Get(c.ColaboradorId)),
                                Observacion = c.Observacion,
                                NombreRuta=c.RutaHorario.Ruta.Nombre,
                                Horario=c.RutaHorario.Horario.ToString()
                            }).ToList();
            return choferes;
        }

        public List<RutaHorarioDto> ListaRutasHorario(int id)
        {
            var query = _horarioRepository.GetAll().Include(c => c.Ruta).Where(c=>!c.Ruta.IsDeleted).ToList();
            

            var colaboradores_rutas = _colaboradorrutaRepository.GetAll().Where(c => c.ColaboradorId == id).Select(c=>c.RutaHorarioId).ToList();


            var queryNotInResults = (from x in query
                                     where !colaboradores_rutas.Contains(x.Id)
                                     select x).ToList();

            var rutahorario = (from r in queryNotInResults
                               select new RutaHorarioDto
                               {
                                   Id = r.Id,
                                   Horario = r.Horario,
                                   NombreRuta = r.Ruta.Nombre,
                                   RutaId = r.RutaId
                               }).OrderBy(c => c.Horario).ToList();

         
            return rutahorario;
        }

        public List<TrabajoDiarioDto> ReporteDiarioTrabajo(InputReporteTransporte i)
        {
            throw new NotImplementedException();
        }

        public List<DiarioViajeVehiculoDto> ReporteDiarioViajesProveedor(InputReporteTransporte i)
        {
            throw new NotImplementedException();
        }

        public List<PersonasTransportadasDto> ReportePersonasTransportadas(InputReporteTransporte i)
        {
            throw new NotImplementedException();
        }
    }
}
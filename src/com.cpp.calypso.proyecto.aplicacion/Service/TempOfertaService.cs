using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.dominio;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class TempOfertaAsyncBaseCrudAppService : AsyncBaseCrudAppService<TempOferta, TempOfertaDto, PagedAndFilteredResultRequestDto>, ITempOfertaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<TempRequerimiento> _tempRequerimientoRepository;
        private readonly IBaseRepository<Requerimiento> _requerimientoRepository;
        private readonly IBaseRepository<Oferta> _ofertaRepository;
        private readonly IBaseRepository<OfertaComercial> _ofertaComercialRepository;
        private readonly IdentityEmailMessageService _correoservice;

        public TempOfertaAsyncBaseCrudAppService(
            IBaseRepository<TempOferta> repository,
            IBaseRepository<TempRequerimiento> tempRequerimientoRepository,
            IBaseRepository<Requerimiento> requerimientoRepository,
            IBaseRepository<Oferta> ofertaRepository,
            IBaseRepository<OfertaComercial> ofertaComercialRepository,
            IdentityEmailMessageService correoservice
            ) : base(repository)
        {
            _tempRequerimientoRepository = tempRequerimientoRepository;
            _requerimientoRepository = requerimientoRepository;
            _ofertaRepository = ofertaRepository;
            _ofertaComercialRepository = ofertaComercialRepository;
            _correoservice = correoservice;
        }

        public void CargarOfertas()
        {
            var datos = Repository.GetAll().ToList();
            List<TempOferta> listado = new List<TempOferta>();
            for (var i = 56; i < 420; i++)
            {
                var temp = datos[i];

                var requerimientoTemp = _tempRequerimientoRepository
                    .GetAll().FirstOrDefault(o => o.CodigoOfertaAsiciada == temp.codigo);

                if (requerimientoTemp != null)
                {
                    var requerimiento = _requerimientoRepository.Get(requerimientoTemp.RequerimeintoId.Value);
                    var oferta = new OfertaComercial()
                    {
                        estado_oferta = this.GetEstadoOferta(temp.EstadoOferta),
                        service_request = this.GetServiceRequest(temp.ServiceRequest),
                        monto_so_referencia_total = temp.monto_so_referencial_total,
                        monto_ofertado = temp.monto_ofertado,
                        monto_so_aprobado = temp.monto_so_aprobado,
                        monto_ofertado_pendiente_aprobacion = temp.monto_ofertado_pendiente_aprobacion,
                        monto_certificado_aprobado_acumulado = temp.monto_certificado_acumulado,
                        fecha_pliego = temp.fecha_pliego,
                        fecha_primer_envio = temp.fecha_primer_envio,
                        fecha_ultimo_envio = temp.fecha_ultimo_envio,
                        dias_emision_oferta = temp.dias_emisión_oferta,
                        porcentaje_avance = temp.porcentaje_avance,
                        fecha_ultima_modificacion = temp.fecha_ultima_modificacion,
                        version = temp.version,
                        codigo = temp.codigo,
                        alcance = this.GetAlcance(temp.alcance),
                        estatus_de_Ejecucion = this.GetStatusEjecucion(temp.status_ejecucion),
                        vigente = true,
                        tipo_Trabajo_Id = this.GetTipoTrabajo(temp.tipo_Trabajo_Id),
                        centro_de_Costos_Id = this.GetCentroCostos(temp.centro_de_Costos_Id),
                        codigo_shaya = temp.codigo_shaya,
                        acta_cierre = this.GetActaCierre(temp.acta_cierre),
                        revision_Oferta = temp.version,
                        descripcion = temp.descripcion,
                        fecha_oferta = temp.fecha_ultimo_envio,
                        forma_contratacion = this.GetFormaContratacion(temp.forma_contratacion),
                        service_order = this.GetServiceOrder(temp.ServiceOrder),
                        ContratoId = 1,
                    };

                    try
                    {
                        _ofertaComercialRepository.Insert(Mapper.Map<OfertaComercial>(oferta));
                    }
                    catch (Exception e)
                    {
                        var msg = e.Message;

                    }
                }
                else
                {

                    listado.Add(temp);
                }
            }

            var x = listado;
        }

        public async Task<string> CargarOfertas2Async(int desde, int hasta)
        {
            var datos = Repository.GetAll()
                                  .Where(c => c.Id >= desde)
                                  .Where(c => c.Id <= hasta)
                                  .ToList();
            List<String> migrados = new List<string>();
            foreach (var temp in datos)
            {
                /*var requerimientoTemp = _tempRequerimientoRepository
                    .GetAll().FirstOrDefault(o => o.CodigoOfertaAsiciada == temp.codigo);

                if (requerimientoTemp != null)
                {
                    var requerimiento = _requerimientoRepository.Get(requerimientoTemp.RequerimeintoId);*/

                var en = _ofertaComercialRepository.GetAll().Where(c => c.codigo == temp.codigo).FirstOrDefault();
                if (en != null && en.Id > 0)
                {

                    en.estado_oferta = this.GetEstadoOferta(temp.EstadoOferta);
                    en.service_request = this.GetServiceRequest(temp.ServiceRequest);
                    en.monto_so_referencia_total = temp.monto_so_referencial_total;
                    en.monto_ofertado = temp.monto_ofertado;
                    en.monto_so_aprobado = temp.monto_so_aprobado;
                    en.monto_ofertado_pendiente_aprobacion = temp.monto_ofertado_pendiente_aprobacion;
                    en.monto_certificado_aprobado_acumulado = temp.monto_certificado_acumulado;
                    en.fecha_pliego = temp.fecha_pliego;
                    en.fecha_primer_envio = temp.fecha_primer_envio;
                    en.fecha_ultimo_envio = temp.fecha_ultimo_envio;
                    en.dias_emision_oferta = temp.dias_emisión_oferta;
                    en.porcentaje_avance = temp.porcentaje_avance;
                    en.fecha_ultima_modificacion = temp.fecha_ultima_modificacion;
                    en.version = temp.version;
                    en.codigo = temp.codigo;
                    en.alcance = this.GetAlcance(temp.alcance);
                    en.estatus_de_Ejecucion = this.GetStatusEjecucion(temp.status_ejecucion);
                    en.vigente = true;
                    en.tipo_Trabajo_Id = this.GetTipoTrabajo(temp.tipo_Trabajo_Id);
                    en.centro_de_Costos_Id = this.GetCentroCostos(temp.centro_de_Costos_Id);
                    en.codigo_shaya = temp.codigo_shaya;
                    en.acta_cierre = this.GetActaCierre(temp.acta_cierre);
                    en.revision_Oferta = temp.version;
                    en.descripcion = temp.descripcion;
                    en.fecha_oferta = temp.fecha_ultimo_envio;
                    en.forma_contratacion = this.GetFormaContratacion(temp.forma_contratacion);
                    en.service_order = this.GetServiceOrder(temp.ServiceOrder);
                    en.ContratoId = 1;


                    try
                    {
                        _ofertaComercialRepository.Update(en);
                        string data = "ACTUALIZADA-" + en.codigo +"-"+ en.descripcion + "-" + DateTime.Now.Date.ToShortDateString();
                        migrados.Add(data);
                    }
                    catch (Exception e)
                    {
                        var msg = e.Message;
                        string data = "ERROR- ACTUALIZACION-" + en.codigo + "-" + en.descripcion + "-" + DateTime.Now.Date.ToShortDateString() + "-" + e.Message;
                        migrados.Add(data);
                    }


                }
                else
                {




                    var oferta = new OfertaComercial()
                    {
                        estado_oferta = this.GetEstadoOferta(temp.EstadoOferta),
                        service_request = this.GetServiceRequest(temp.ServiceRequest),
                        monto_so_referencia_total = temp.monto_so_referencial_total,
                        monto_ofertado = temp.monto_ofertado,
                        monto_so_aprobado = temp.monto_so_aprobado,
                        monto_ofertado_pendiente_aprobacion = temp.monto_ofertado_pendiente_aprobacion,
                        monto_certificado_aprobado_acumulado = temp.monto_certificado_acumulado,
                        fecha_pliego = temp.fecha_pliego,
                        fecha_primer_envio = temp.fecha_primer_envio,
                        fecha_ultimo_envio = temp.fecha_ultimo_envio,
                        dias_emision_oferta = temp.dias_emisión_oferta,
                        porcentaje_avance = temp.porcentaje_avance,
                        fecha_ultima_modificacion = temp.fecha_ultima_modificacion,
                        version = temp.version,
                        codigo = temp.codigo,
                        alcance = this.GetAlcance(temp.alcance),
                        estatus_de_Ejecucion = this.GetStatusEjecucion(temp.status_ejecucion),
                        vigente = true,
                        tipo_Trabajo_Id = this.GetTipoTrabajo(temp.tipo_Trabajo_Id),
                        centro_de_Costos_Id = this.GetCentroCostos(temp.centro_de_Costos_Id),
                        codigo_shaya = temp.codigo_shaya,
                        acta_cierre = this.GetActaCierre(temp.acta_cierre),
                        revision_Oferta = temp.version,
                        descripcion = temp.descripcion,
                        fecha_oferta = temp.fecha_ultimo_envio,
                        forma_contratacion = this.GetFormaContratacion(temp.forma_contratacion),
                        service_order = this.GetServiceOrder(temp.ServiceOrder),
                        ContratoId = 1,
                    };

                    try
                    {
                        _ofertaComercialRepository.Insert(Mapper.Map<OfertaComercial>(oferta));
                        string data = "CREADA-" + oferta.codigo + oferta.descripcion + DateTime.Now.Date.ToShortDateString();
                        migrados.Add(data);
                    }
                    catch (Exception e)
                    {
                        var msg = e.Message;
                        string data = "ERROR-" + oferta.codigo + oferta.descripcion + DateTime.Now.Date.ToShortDateString() + e.Message;
                        migrados.Add(data);
                    }
                }
            }
            await this.EnviarArchivos(migrados, "OFERTAS");
            return "OK";
        }

        public async Task<string> EnviarArchivos(List<String> datos, string tipo)
        {
            Random a = new Random();
            var valor = a.Next(1, 1000);
            string fileName = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosMigraciones/" + tipo + "" + DateTime.Now.Date.Year + "" + valor + ".txt");

            using (StreamWriter sw = File.CreateText(fileName))
            {

                foreach (var b in datos)
                {
                    sw.WriteLine(b);
                }
            }

            MailMessage message = new MailMessage();
            message.Subject = "PMDIS: MIGRACIONES";

            message.To.Add("efrain.saransig@atikasoft.com");
            //message.To.Add("paola.soria@atikasoft.com");
            if (File.Exists((string)fileName))
            {
                message.Attachments.Add(new Attachment(fileName));
            }
            /*********/
            try
            {
                await _correoservice.SendWithFilesAsync(message);
                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;

            }





        }

        public Oferta.ClaseOferta GetClase(string nombre)
        {
            switch (nombre)
            {
                case "Budgetario":
                    return Oferta.ClaseOferta.Budgetario;
                case "Clase 1":
                    return Oferta.ClaseOferta.Clase1;
                case "Clase 2":
                    return Oferta.ClaseOferta.Clase2;
                case "Clase 3":
                    return Oferta.ClaseOferta.Clase3;
                case "Clase 4":
                    return Oferta.ClaseOferta.Clase4;
                case "Clase 5":
                    return Oferta.ClaseOferta.Clase5;
                default:
                    return Oferta.ClaseOferta.Clase1;

            }
        }
        public int GetStatusEjecucion(string nombre)
        {
            switch (nombre)
            {
                case "Ejecución SHAYA":
                    return 2037;
                case "En Ejecución Ingeniería":
                    return 2036;
                case "N/A":
                    return 2038;
                case "Pendiente Ejecución":
                    return 2033;
                case "":
                    return 0;

                default:
                    return 0;

            }
        }

        public int GetTipoTrabajo(string nombre)
        {
            switch (nombre)
            {
                case "Adicional":
                    return 2016;
                case "Aumento Capacidad":
                    return 2008;
                case "Emergentes":
                    return 2010;
                case "Facilidades":
                    return 2006;
                case "Inyección":
                    return 2012;
                case "Línea de Flujo":
                    return 2027;
                case "Línea Eléctrica":
                    return 2009;
                case "Logística":
                    return 2011;
                case "Mejora Estaciones":
                    return 2007;
                case "Notificación de Cambio Ingeniería":
                    return 4037;
                case "Plataforma existente":
                    return 2005;
                case "Plataforma nueva":
                    return 2015;
                case "Pulling":
                    return 2017;
                case "Servicios":
                    return 2004;
                case "Taladro":
                    return 2019;
                case "Tratamiento de Agua":
                    return 2014;
                case "Workover":
                    return 2018;
                default:
                    return 4060;

            }
        }

        public int GetEstadoOferta(string estado)
        {
            switch (estado)
            {
                case "Cancelado":
                    return 2024;
                case "Aprobado":
                    return 2029;
                case "Anulado":
                    return 2031;
                case "Ejecución SHAYA":
                    return 2030;
                case "Eval SHAYA":
                    return 2028;
                case "Eval TECHINT":
                    return 2027;
                case "Sin SR":
                    return 2026;
                default:
                    return 2032;

            }
        }

        public int GetCentroCostos(string estado)
        {
            switch (estado)
            {
                case "CAPEX":
                    return 2021;
                case "CAPEX FISCAL":
                    return 2022;
                case "EXTRAORDINARIO":
                    return 2020;
                case "N/A":
                    return 4061;
                case "OPEX":
                    return 2023;
                default:
                    return 4062;

            }
        }

        public int GetActaCierre(string acta)
        {
            switch (acta)
            {
                case "SI":
                    return 1;
                case "NO":
                    return 2;
                case "N/A":
                    return 3;
                default:
                    return 3;
            }
        }

        public bool GetServiceRequest(string request)
        {
            switch (request)
            {
                case "SI":
                    return true;
                case "NO":
                    return false;
                default:
                    return false;

            }
        }

        public bool GetServiceOrder(string request)
        {
            switch (request)
            {
                case "SO":
                    return true;
                case "CO":
                    return true;
                default:
                    return false;

            }
        }

        public int GetAlcance(string alcance)
        {
            switch (alcance)
            {
                case "EPC":
                    return 4165;
                case "C":
                    return 4166;
                case "E":
                    return 4167;
                case "EC":
                    return 4168;
                case "EP":
                    return 4169;
                case "P":
                    return 4170;
                case "PC":
                    return 4171;
                case "N/A":
                    return 4172;         
                default:
                    return 0;

            }
        }

        public int GetFormaContratacion(string forma)
        {
            switch (forma)
            {
                case "Cost + Fee":
                    return 5188;
                case "Lump Sum":
                    return 3021;
                case "Lump Sum + Cost + Fee":
                    return 3021;
                case "Reembolsable":
                    return 5189;
                case "Reembolsables":
                    return 5189;
                case "Reembolsables + Unit Prices":
                    return 4076;
                case "Tarifas":
                    return 3020;
                case "Tarifas (Construccion)":
                    return 3020;
                case "Unit Price":
                    return 3022;
                case "Unit Price + Reembolsable":
                    return 4076;
                case "Unit Prices":
                    return 3022;
                case "Unit Prices + Reembolsable":
                    return 4076;
                case "Unit Prices + Reembolsables":
                    return 4076;
                case "Unit Prices+ Reembolsable":
                    return 4076;
                case "Unit Pricess":
                    return 3022;
                case "Unit Pricess + Reembolsable":
                    return 4076;
                case "Unite Prices + Reembolsable":
                    return 4076;
                default:
                    return 0;

            }
        }
    }


}

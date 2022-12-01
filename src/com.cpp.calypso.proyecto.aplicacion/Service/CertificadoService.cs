using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.comun.entityframework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class CertificadoAsyncBaseCrudAppService : AsyncBaseCrudAppService<Certificado, CertificadoDto, PagedAndFilteredResultRequestDto>, ICertificadoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Computo> _computorepository;
        private readonly IBaseRepository<Proyecto> _proyectorepository;
        private readonly IBaseRepository<Oferta> _ofertarepository;
        private readonly IBaseRepository<OfertaComercialPresupuesto> _ofertacomercialrepository;
        private readonly IBaseRepository<DetalleCertificado> _detalleCertificadoRepository;
        private readonly IItemAsyncBaseCrudAppService _itemservice;

        private readonly IBaseRepository<Item> _repositoryitem;
        private readonly IBaseRepository<Catalogo> _repositorycatalogo;
        private readonly IBaseRepository<Preciario> _repositorypreciario;
        private readonly IBaseRepository<DetallePreciario> _repositorydetallepreciario;

        private readonly IBaseRepository<DetalleAvanceObra> _dobrarepository;
        private readonly IBaseRepository<DetalleAvanceIngenieria> _dingenieriarepository;
        private readonly IBaseRepository<DetalleAvanceProcura> _dprocurarepository;

        private readonly IBaseRepository<RdoDetalleEac> _drdoeacrepository;
        private readonly IBaseRepository<Contrato> _contratorepository;
        private readonly IBaseRepository<DetalleOrdenServicio> _detallosRepository;
        private readonly IBaseRepository<Secuencial> _secuenciaRepository;
        private readonly IBaseRepository<DetalleAvanceObra> _detalleavancebrarepository;
        private readonly IBaseRepository<ComputoComercial> _computocomercialrepository;

        private readonly IBaseRepository<Empresa> _empresarepository;
        private readonly IBaseRepository<Cliente> _clienterepository;
        private readonly IBaseRepository<RdoCabecera> _rdo_cabecera;
        private readonly IBaseRepository<RdoDetalleEac> _rdo_detalle_EAC;
        private readonly IBaseRepository<ComputoPresupuesto> _computoPresupuestoRepository;
        private readonly IBaseRepository<Presupuesto> _presupuesto;
        private readonly IBaseRepository<ParametroSistema> _parametrorepository;
        public CertificadoAsyncBaseCrudAppService(

            IBaseRepository<Certificado> repository,
            IBaseRepository<Computo> computorepository,
            IBaseRepository<Oferta> ofertarepository,
             IBaseRepository<OfertaComercialPresupuesto> ofertacomercialrepository,
        IBaseRepository<Proyecto> proyectorepository,
            IBaseRepository<DetalleCertificado> detalleCertificadoRepository,
            IBaseRepository<Item> repositoryitem,
            IBaseRepository<Catalogo> repositorycatalogo,
            IBaseRepository<DetallePreciario> repositorydetallepreciario,
            IBaseRepository<Preciario> repositorypreciario,
            IBaseRepository<DetalleAvanceObra> dobrarepository,
            IBaseRepository<DetalleAvanceIngenieria> dingenieriarepository,
            IBaseRepository<DetalleAvanceProcura> dprocurarepository,
            IBaseRepository<RdoDetalleEac> drdoeacrepository,
            IBaseRepository<Contrato> contratorepository,
            IBaseRepository<DetalleOrdenServicio> detallosRepository,
            IBaseRepository<Secuencial> secuenciaRepository,
           IBaseRepository<DetalleAvanceObra> detalleavancebrarepository,
            IBaseRepository<ComputoComercial> computocomercialrepository,
           IBaseRepository<Empresa> empresarepository,
       IBaseRepository<Cliente> clienterepository,
       IBaseRepository<RdoCabecera> rdo_cabecera,
       IBaseRepository<Presupuesto> presupuesto,
        IBaseRepository<ParametroSistema> parametrorepository,
        IBaseRepository<ComputoPresupuesto> computoPresupuestoRepository,
        IBaseRepository<RdoDetalleEac> rdo_detalle_EAC

        ) : base(repository)
        {
            _computorepository = computorepository;
            _ofertarepository = ofertarepository;
            _detalleCertificadoRepository = detalleCertificadoRepository;
            _proyectorepository = proyectorepository;
            _repositorydetallepreciario = repositorydetallepreciario;
            _repositorycatalogo = repositorycatalogo;
            _repositorypreciario = repositorypreciario;
            _repositoryitem = repositoryitem;
            _detalleavancebrarepository = detalleavancebrarepository;
            _computocomercialrepository = computocomercialrepository;
            _computoPresupuestoRepository = computoPresupuestoRepository;
            _itemservice = new ItemServiceAsyncBaseCrudAppService(
                                                 repositoryitem,
                                                 repositorydetallepreciario,
                                                 repositorypreciario,
                                                 repositorycatalogo, _detalleavancebrarepository, _computocomercialrepository, _computoPresupuestoRepository);
            _dobrarepository = dobrarepository;
            _dingenieriarepository = dingenieriarepository;
            _dprocurarepository = dprocurarepository;
            _drdoeacrepository = drdoeacrepository;
            _contratorepository = contratorepository;
            _detallosRepository = detallosRepository;
            _ofertacomercialrepository = ofertacomercialrepository;
            _secuenciaRepository = secuenciaRepository;
            _empresarepository = empresarepository;
            _clienterepository = clienterepository;
            _rdo_cabecera = rdo_cabecera;
            _presupuesto = presupuesto;
            _parametrorepository = parametrorepository;
            _rdo_detalle_EAC = rdo_detalle_EAC;
        }

        public bool Eliminar(int Id)
        {
            var porcentaje = Repository.Get(Id);
            var hijo = _detalleCertificadoRepository.GetAll().Where(c => c.vigente == true)
                .Where(c => c.CertificadoId == Id).ToList();

            porcentaje.vigente = false;
            foreach (var item in hijo)
            {
                var avanceobra = _detalleavancebrarepository.Get(Convert.ToInt32(item.avanceid_referencia));
                avanceobra.estacertificado = false;
                _detalleavancebrarepository.Update(avanceobra);

                item.vigente = false;
                _detalleCertificadoRepository.Update(item);

            }

            return true;
        }

        public CertificadoDto getdetalle(int Id)
        {
            var query = Repository.GetAllIncluding(c => c.Proyecto, c => c.Proyecto.Contrato, c => c.Proyecto.Contrato.Cliente).Where(e => e.vigente == true);
            var detalle = (from d in query
                           where d.Id == Id
                           where d.vigente == true
                           select new CertificadoDto
                           {
                               Id = d.Id,
                               vigente = d.vigente,
                               ProyectoId = d.ProyectoId,
                               estado_actual = d.estado_actual,
                               fecha_corte = d.fecha_corte,
                               fecha_emision = d.fecha_emision,
                               monto_certificado = d.monto_certificado,
                               monto_pendiente = d.monto_pendiente,
                               numero_certificado = d.numero_certificado,
                               porcentaje_avance = d.porcentaje_avance,
                               presupuesto_inicial = d.presupuesto_inicial,
                               tiene_GR = d.tiene_GR,
                               Proyecto = d.Proyecto,
                               tipo_certificado = d.tipo_certificado

                           }

            ).FirstOrDefault();
            return detalle;

        }

        public List<CertificadoDto> Listar()
        {
            var query = Repository.GetAllIncluding(c => c.Proyecto.Contrato.Cliente);
            var detalle = (from d in query
                           where d.vigente == true
                           select new CertificadoDto
                           {
                               Id = d.Id,
                               vigente = d.vigente,

                               estado_actual = d.estado_actual,
                               fecha_corte = d.fecha_corte,
                               fecha_emision = d.fecha_emision,
                               monto_certificado = d.monto_certificado,
                               monto_pendiente = d.monto_pendiente,
                               numero_certificado = d.numero_certificado,
                               porcentaje_avance = d.porcentaje_avance,
                               presupuesto_inicial = d.presupuesto_inicial,
                               tiene_GR = d.tiene_GR,
                               ProyectoId = d.ProyectoId,
                               Proyecto = d.Proyecto,
                               tipo_certificado = d.tipo_certificado

                           }
            ).OrderByDescending(c => c.fecha_corte).ToList();
            return detalle;
        }

        public List<CertificadoDto> GetCretificadosGr(int ProyectoId)
        {
            var query = Repository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.tiene_GR == false)
                .Where(o => o.ProyectoId == ProyectoId);

            var items = (from c in query
                         select new CertificadoDto()
                         {
                             Id = c.Id,
                             ProyectoId = c.ProyectoId,
                             Proyecto = c.Proyecto,
                             fecha_emision = c.fecha_emision,
                             estado_actual = c.estado_actual,
                             monto_certificado = c.monto_certificado,
                             numero_certificado = c.numero_certificado,
                             tipo_certificado = c.tipo_certificado,
                         }).ToList();
            return items;
        }
        public decimal MontoPresupuestoIngenieria(int ProyectoId)
        {

            decimal montoingenieria = 0;
            var ofertad = this.OfertaDefinitiva(ProyectoId);

            if (ofertad != null && ofertad.Id > 0)
            {
                var listacomputos = _computorepository.GetAllIncluding(c => c.Wbs.Oferta.Proyecto, c => c.Item)
                    .Where(c => c.vigente == true).Where(c => c.Wbs.OfertaId == ofertad.Id)
                    .Where(c => c.Item.GrupoId == 1).ToList();

                if (listacomputos != null && listacomputos.Count >= 0)
                {
                    montoingenieria = (from x in listacomputos select x.costo_total).Sum(); //Presupuesto Total

                }
            }

            return montoingenieria;
        }

        public decimal MontoPresupuestoConstruccion(int ProyectoId)
        {
            decimal montoingenieria = 0;
            var ofertad = this.OfertaDefinitiva(ProyectoId);

            if (ofertad != null && ofertad.Id > 0)
            {


                var listacomputos = _computorepository.GetAllIncluding(c => c.Wbs.Oferta.Proyecto, c => c.Item)
                    .Where(c => c.vigente == true).Where(c => c.Wbs.OfertaId == ofertad.Id)
                    .Where(c => c.Item.GrupoId == 2).ToList();

                if (listacomputos != null && listacomputos.Count >= 0)
                {
                    montoingenieria = (from x in listacomputos select x.costo_total).Sum(); //Presupuesto Total

                }
            }

            return montoingenieria;
        }

        public decimal MontoPresupuestoProcura(int ProyectoId)

        {
            decimal montoingenieria = 0;
            var ofertad = this.OfertaDefinitiva(ProyectoId);

            if (ofertad != null && ofertad.Id > 0)
            {

                var listacomputos = _computorepository.GetAllIncluding(c => c.Wbs.Oferta.Proyecto, c => c.Item)
                    .Where(c => c.vigente == true).Where(c => c.Wbs.OfertaId == ofertad.Id).Where(c => c.Item.GrupoId == 3).ToList();

                if (listacomputos != null && listacomputos.Count >= 0)
                {
                    montoingenieria = (from x in listacomputos select x.costo_total).Sum(); //Presupuesto Total

                }

            }



            return montoingenieria;
        }
        public decimal MontoPresupuestoSubcontratos(int ProyectoId)

        {
            decimal montoingenieria = 0;
            var ofertad = this.OfertaDefinitiva(ProyectoId);

            if (ofertad != null && ofertad.Id > 0)
            {

                var listacomputos = _computorepository.GetAllIncluding(c => c.Wbs.Oferta.Proyecto, c => c.Item.Grupo)
                    .Where(c => c.vigente == true).Where(c => c.Wbs.OfertaId == ofertad.Id).Where(c => c.Item.GrupoId == 4).ToList();

                if (listacomputos != null && listacomputos.Count >= 0)
                {
                    montoingenieria = (from x in listacomputos select x.costo_total).Sum(); //Presupuesto Total

                }

            }



            return montoingenieria;
        }

        public Oferta OfertaDefinitiva(int ProyectoId)
        {
            var oferta = _ofertarepository.GetAllIncluding(c => c.Proyecto)
                .Where(c => c.ProyectoId == ProyectoId)
                .Where(c => c.es_final == true)
                .Where(c => c.vigente == true)
                .FirstOrDefault();

            return oferta;
        }

        public decimal MontoCertificadoIngenieria(int ProyectoId)
        {
            decimal montocertificadoingenieria = 0;
            var detalles = _detalleCertificadoRepository.GetAllIncluding(c => c.Computo, c => c.Computo.Item, c => c.Computo.Wbs.Oferta)
                .Where(c => c.Computo.Wbs.Oferta.ProyectoId == ProyectoId).

                Where(c => c.Computo.Item.GrupoId == 1)
                .Where(c => c.vigente == true).ToList();

            if (detalles != null && detalles.Count >= 0)
            {
                montocertificadoingenieria = (from x in detalles select x.monto_a_certificar).Sum(); //Presupuesto Total

            }

            var r = _proyectorepository.Get(ProyectoId);


            return montocertificadoingenieria;
        }

        public decimal MontoCertificadoConstruccion(int ProyectoId)
        {
            decimal montocertificadoingenieria = 0;
            var detalles = _detalleCertificadoRepository.GetAllIncluding(c => c.Computo, c => c.Computo.Item, c => c.Computo.Wbs.Oferta)
                .Where(c => c.Computo.Wbs.Oferta.ProyectoId == ProyectoId).
                Where(c => c.Computo.Item.GrupoId == 2)
                .Where(c => c.vigente == true).ToList();

            if (detalles != null && detalles.Count >= 0)
            {
                montocertificadoingenieria = (from x in detalles select x.monto_a_certificar).Sum(); //Presupuesto Total

            }

            return montocertificadoingenieria;
        }

        public decimal MontoCertificadoProcura(int ProyectoId)
        {
            decimal montocertificadoingenieria = 0;
            var detalles = _detalleCertificadoRepository.GetAllIncluding(c => c.Computo, c => c.Computo.Item, c => c.Computo.Wbs.Oferta)
                .Where(c => c.Computo.Wbs.Oferta.ProyectoId == ProyectoId).
                Where(c => c.Computo.Item.GrupoId == 3)
                .Where(c => c.vigente == true).ToList();

            if (detalles != null && detalles.Count >= 0)
            {
                montocertificadoingenieria = (from x in detalles select x.monto_a_certificar).Sum(); //Presupuesto Total

            }

            return montocertificadoingenieria;
        }

        public bool cambiarestadocertificado(int id)
        {
            var certificado = Repository.Get(id);
            certificado.estado_actual = 1;
            var resultado = Repository.Update(certificado);

            if (resultado.Id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool cancelarestadocertificado(int id)
        {
            var certificado = Repository.Get(id);
            certificado.estado_actual = 0;
            var resultado = Repository.Update(certificado);

            if (resultado.Id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public ExcelPackage GenerarExcelCertificado(int Id, int proyectoid)
        {

            bool esSegundoFormato = false;
            var cabecera = Repository.GetAll().Where(x => x.Id == Id).FirstOrDefault();
            var cert = Repository.Get(Id);


            ExcelPackage excel = new ExcelPackage();

            var hoja = excel.Workbook.Worksheets.Add("Certificado");
            excel.Workbook.CalcMode = ExcelCalcMode.Automatic;

            hoja.TabColor = System.Drawing.Color.Azure;
            //hoja.DefaultRowHeight = 13;


            RdoCabecera ultimoRdo = null;

            //Lineas Cabercera
            // ALTURA CABECERAS 42
            hoja.View.ZoomScale = 50;
            hoja.Row(1).Height = 42;
            hoja.Row(2).Height = 42;
            hoja.Row(3).Height = 42;
            hoja.Row(4).Height = 42;
            hoja.Row(5).Height = 42;
            hoja.Row(6).Height = 43;
            hoja.Row(7).Height = 42;
            hoja.Row(8).Height = 42;
            hoja.Row(9).Height = 42;



            //hoja.Cells["C3:Q3"].Style.Border.Top.Style = ExcelBorderStyle.Medium;
            //hoja.Cells["C3:Q3"].Style.Border.Top.Color.SetColor(Color.Black);



            //* hoja.View.FreezePanes(9, 1);
            hoja.View.FreezePanes(12, 1);

            #region OBTENCION DE DATOS
            //DATOS CABECERA
            var proyecto = _proyectorepository.Get(proyectoid);
            var contrato = _contratorepository.Get(proyecto.contratoId);
            var cliente = _clienterepository.Get(contrato.ClienteId);
            var empresa = _empresarepository.Get(contrato.EmpresaId);

            esSegundoFormato = contrato != null && contrato.Formato == FormatoContrato.Contrato_2019 ? true : false;


            decimal montopo = 0;
            var listamontopo = _detallosRepository.GetAll().Where(x => x.ProyectoId == proyectoid)
                                                    .Where(x => x.vigente)
                                                    .Where(x => x.OrdenServicio.vigente)
                                                    .Where(x => x.OrdenServicio.anio == cabecera.fecha_corte.Year)
                                                    .ToList();

            if (listamontopo.Count > 0)
            {
                montopo = (from mp in listamontopo select mp.valor_os).Sum();
            }


            var ofertacomercial = _ofertacomercialrepository.GetAll()
                                  .Where(x => x.vigente).Where(x => x.Requerimiento.ProyectoId == proyectoid)
                                   .Where(x => x.Requerimiento.tipo_requerimiento == TipoRequerimiento.Principal)
                                   .Select(x => x.OfertaComercial.codigo).Distinct().ToArray();

            string resultado = "";
            if (ofertacomercial.Length > 0)
            {
                resultado = string.Join(" ; ", ofertacomercial);

            }
            int planilla = Repository.GetAll().Where(x => x.ProyectoId == cabecera.ProyectoId)
                                            .Where(x => x.estado_actual == 1)
                                            .Where(x => x.vigente)
                                            .Where(x => x.fecha_corte < cabecera.fecha_corte)
                                            .ToList().Count + 1;



            #endregion

            var BordeExcelMediano = ExcelBorderStyle.Thin;

            #region CABECERA CERTIFICADO
            //FILAS CABECERA
            string celdaCabecera = "C1:Q1";
            hoja.Cells[celdaCabecera].Merge = true;
            hoja.Cells[celdaCabecera].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celdaCabecera].Style.Fill.BackgroundColor.SetColor(Color.White);

            hoja.Cells[celdaCabecera].Style.Border.Top.Style = ExcelBorderStyle.Medium;
            hoja.Cells[celdaCabecera].Style.Border.Left.Style = ExcelBorderStyle.Medium;
            hoja.Cells[celdaCabecera].Style.Border.Right.Style = ExcelBorderStyle.Medium;



            celdaCabecera = "C2:Q2";
            hoja.Cells[celdaCabecera].Merge = true;
            hoja.Cells[celdaCabecera].Value = "PROYECTO CAMPO AUCA - BLOQUE 61";
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(new Font("Arial", 26, FontStyle.Bold));
            hoja.Cells[celdaCabecera].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celdaCabecera].Style.Fill.BackgroundColor.SetColor(Color.White);

            celdaCabecera = "C3:Q3";
            hoja.Cells[celdaCabecera].Merge = true;
            hoja.Cells[celdaCabecera].Value = "CERTIFICADO DE CONSTRUCCIÓN";
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(new Font("Arial", 26, FontStyle.Bold));
            hoja.Cells[celdaCabecera].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celdaCabecera].Style.Fill.BackgroundColor.SetColor(Color.White);

            celdaCabecera = "C4:Q4";
            hoja.Cells[celdaCabecera].Merge = true;
            hoja.Cells[celdaCabecera].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celdaCabecera].Style.Fill.BackgroundColor.SetColor(Color.White);




            celdaCabecera = "C5:Q5";
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells[celdaCabecera].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celdaCabecera].Style.Fill.BackgroundColor.SetColor(Color.White);
            celdaCabecera = "C6:Q8";
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells[celdaCabecera].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celdaCabecera].Style.Fill.BackgroundColor.SetColor(Color.White);


            //FILA 6
            var fuente = new Font("Arial", 16, FontStyle.Bold);
            var fuenteR = new Font("Arial", 16, FontStyle.Regular);
            celdaCabecera = "C6";
            hoja.Cells[celdaCabecera].Value = "CÓDIGO DE PROYECTO:";
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(fuente);
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            celdaCabecera = "C6:E6";
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            celdaCabecera = "F6:J6";
            hoja.Cells[celdaCabecera].Merge = true;
            hoja.Cells[celdaCabecera].Value = proyecto.codigo.ToUpper() + " - " + proyecto.nombre_proyecto.ToUpper();
            hoja.Cells[celdaCabecera].Style.WrapText = true;
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(fuenteR);
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);



            celdaCabecera = "L6";
            hoja.Cells[celdaCabecera].Value = "CERTIFICADO N°:";
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(fuente);
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            celdaCabecera = "K6:L6";
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            celdaCabecera = "M6:N6";
            hoja.Cells[celdaCabecera].Merge = true;
            hoja.Cells[celdaCabecera].Value = this.NombreCertificado(cabecera.Id, proyecto.Id);


            //hoja.Cells[celdaCabecera].Value = proyecto.codigo_reporte_certificacion + "-CRT-" + (planilla<10?"0"+planilla:""+planilla)+"-"+cabecera.fecha_corte.ToString("yy") + "" + cabecera.fecha_corte.ToString("MM") + "" + cabecera.fecha_corte.ToString("dd");


            hoja.Cells[celdaCabecera].Style.WrapText = true;
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(fuenteR);
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            celdaCabecera = "P6";
            hoja.Cells[celdaCabecera].Value = "RDO";
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(fuente);
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            celdaCabecera = "O6:P6";
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            celdaCabecera = "Q6";

            hoja.Cells[celdaCabecera].Value = "√";
            hoja.Cells[celdaCabecera].Style.WrapText = true;
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(fuenteR);
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);



            celdaCabecera = "C7";
            hoja.Cells[celdaCabecera].Value = "OFERTA N°";
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(fuente);
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            celdaCabecera = "C7:E7";
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            celdaCabecera = "F7:J7";
            hoja.Cells[celdaCabecera].Merge = true;
            hoja.Cells[celdaCabecera].Value = resultado;
            hoja.Cells[celdaCabecera].Style.WrapText = true;
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(fuenteR);
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            celdaCabecera = "L7";
            hoja.Cells[celdaCabecera].Value = "MES DE CERTIFICACIÓN:";
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(fuente);
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            celdaCabecera = "K7:L7";
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            celdaCabecera = "M7:N7";
            hoja.Cells[celdaCabecera].Merge = true;
            hoja.Cells[celdaCabecera].Value = cabecera.fecha_corte.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-Es")).ToUpper() + " " + cabecera.fecha_corte.Year; ;
            hoja.Cells[celdaCabecera].Style.WrapText = true;
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(fuenteR);
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            celdaCabecera = "P7";
            hoja.Cells[celdaCabecera].Value = "SOPORTE DE CANTIDADES";
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(fuente);
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            celdaCabecera = "O7:P7";
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            celdaCabecera = "Q7";

            hoja.Cells[celdaCabecera].Value = "-";
            hoja.Cells[celdaCabecera].Style.WrapText = true;
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(fuenteR);
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);



            celdaCabecera = "C8";
            hoja.Cells[celdaCabecera].Value = "LOCACIÓN:";
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(fuente);
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



            celdaCabecera = "F8";
            hoja.Cells[celdaCabecera].Value = proyecto.locacion;
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(fuenteR);
            // hoja.Cells[celdaCabecera].Style.WrapText = true;
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            celdaCabecera = "C8:E8";
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            celdaCabecera = "F8:Q8";
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);



            /*celdaCabecera = "F8:J8";
            hoja.Cells[celdaCabecera].Merge = true;
            hoja.Cells[celdaCabecera].Value = "";
            hoja.Cells[celdaCabecera].Style.WrapText = true;
            hoja.Cells[celdaCabecera].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells[celdaCabecera].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celdaCabecera].Style.Font.SetFromFont(fuente);
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Thin);*/





            hoja.Cells["C9:C10"].Merge = true;
            hoja.Cells["C9:C10"].Value = "ITEM";
            hoja.Cells["C9:C10"].AutoFilter = true;
            hoja.Cells["C9:C10"].Style.WrapText = true;
            hoja.Cells["C9:C10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["C9:C10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            hoja.Cells["C9:C10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["C9:C10"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            hoja.Cells["C9:C10"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells["C9:C10"].Style.Font.Bold = true;


            hoja.Cells[9, 4, 10, 7].Merge = true;
            hoja.Cells[9, 4, 10, 7].Value = "DESCRIPCIÓN";
            hoja.Cells[9, 4, 10, 7].AutoFilter = true;

            hoja.Cells[9, 4, 10, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[9, 4, 10, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            hoja.Cells[9, 4, 10, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells[9, 4, 10, 7].Style.WrapText = true;
            hoja.Cells[9, 4, 10, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[9, 4, 10, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[9, 4, 10, 7].Style.Font.Bold = true;



            hoja.Cells["H10"].Value = "UNIDAD";
            hoja.Cells["H10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["H10"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            hoja.Cells["H10"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["H10"].Style.WrapText = true;
            hoja.Cells["H10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["H10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["H10"].Style.Font.Bold = true;

            hoja.Cells["I10"].Value = "CANTIDAD ESTIMADA";
            hoja.Cells["I10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["I10"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            hoja.Cells["I10"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["I10"].Style.WrapText = true;
            hoja.Cells["I10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["I10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["I10"].Style.Font.Bold = true;

            hoja.Cells["J10"].Value = "PRECIO UNITARIO";
            hoja.Cells["J10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["J10"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            hoja.Cells["J10"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells["J10"].Style.Font.Bold = true;

            hoja.Cells["J10"].Style.WrapText = true;
            hoja.Cells["J10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["J10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            hoja.Cells["K10"].Value = "COSTO ESTIMADO TOTAL";
            hoja.Cells["K10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["K10"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            hoja.Cells["K10"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["K10"].Style.WrapText = true;
            hoja.Cells["K10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["K10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["K10"].Style.Font.Bold = true;

            hoja.Column(8).Width = 18;
            hoja.Column(9).Width = 18;
            hoja.Column(10).Width = 25;
            hoja.Column(11).Width = 23;


            string celda = "H9:N9";
            hoja.Cells[celda].Merge = true;
            hoja.Cells[celda].Value = "CANTIDAD REAL";
            hoja.Cells[celda].AutoFilter = true;
            hoja.Cells[celda].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells[celda].Style.WrapText = true;
            hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celda].Style.Font.Bold = true;

            hoja.Cells["L10"].Value = "PERIODO ANTERIOR";
            hoja.Cells["L10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["L10"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            hoja.Cells["L10"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["L10"].Style.WrapText = true;
            hoja.Cells["L10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["L10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["L10"].Style.Font.Bold = true;

            hoja.Cells["M10"].Value = "CANTIDAD ESTIMADA";
            hoja.Cells["M10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["M10"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            hoja.Cells["M10"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["M10"].Style.WrapText = true;
            hoja.Cells["M10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["M10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["M10"].Style.Font.Bold = true;

            hoja.Cells["N10"].Value = "ACUMULADO";
            hoja.Cells["N10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["N10"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            hoja.Cells["N10"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells["N10"].Style.Font.Bold = true;

            hoja.Cells["N10"].Style.WrapText = true;
            hoja.Cells["N10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["N10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            hoja.Column(12).Width = 18;
            hoja.Column(13).Width = 18;
            hoja.Column(14).Width = 25;
            hoja.Column(15).Width = 23;




            hoja.Cells["O9:Q9"].Merge = true;
            hoja.Cells["O9:Q9"].Value = "COSTOS REAL";
            hoja.Cells["O9:Q9"].AutoFilter = true;
            hoja.Cells["O9:Q9"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["O9:Q9"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            hoja.Cells["O9:Q9"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["O9:Q9"].Style.WrapText = true;
            hoja.Cells["O9:Q9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["O9:Q9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["O9:Q9"].Style.Font.Bold = true;

            hoja.Cells["O10"].Value = "PERIODO ANTERIOR";
            hoja.Cells["O10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["O10"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            hoja.Cells["O10"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["O10"].Style.WrapText = true;
            hoja.Cells["O10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["O10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["O10"].Style.Font.Bold = true;

            hoja.Cells["P10"].Value = "ESTE PERIODO";
            hoja.Cells["P10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["P10"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            hoja.Cells["P10"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["P10"].Style.WrapText = true;
            hoja.Cells["P10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["P10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["P10"].Style.Font.Bold = true;

            hoja.Cells["Q10"].Value = "ACUMULADO";
            hoja.Cells["Q10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["Q10"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(153, 204, 255));
            hoja.Cells["Q10"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells["Q10"].Style.Font.Bold = true;

            hoja.Cells["Q10"].Style.WrapText = true;
            hoja.Cells["Q10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["Q10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Column(16).Width = 24;
            hoja.Column(17).Width = 24;

            var fechaCorte = cabecera.fecha_corte.Date;

       
            var montoPresupuestoTotal = Convert.ToDecimal(0);
            var presupuestos = _presupuesto.GetAll().Where(y => y.vigente)
                                                  .Where(y => y.es_final)
                                                  .Where(y => y.estado_aprobacion == Presupuesto.EstadoAprobacion.Aprobado)
                                                  .Where(y => y.ProyectoId == proyectoid).ToList();

            if (presupuestos != null && presupuestos.Count > 0)
            {
                montoPresupuestoTotal = Decimal.Round(
                                               (from z in presupuestos
                                                select Decimal.Round(z.monto_total, 8)).Sum()
                                                 , 8);
            }


            #endregion



            /*Ultimo RDO Generado a la fecha de Corte*/
            var rdo = _rdo_cabecera.GetAll().Where(r => r.es_definitivo)
                                       .Where(r => r.estado)
                                       .Where(r => r.vigente)
                                       .Where(r => r.fecha_rdo <= fechaCorte)
                                       .Where(r => r.ProyectoId == proyectoid)
                                       .OrderByDescending(r => r.fecha_rdo).ToList()
                                       .FirstOrDefault();


            var detallesConstruccionUltimoRdo = new List<RdoDetalleEac>();
            var detallesConstruccionAnteriorRdoCertificado = new List<RdoDetalleEac>();

            if (rdo != null && rdo.Id > 0)
            {
                detallesConstruccionUltimoRdo = _rdo_detalle_EAC.GetAllIncluding(d => d.Computo.Item)
                                                 .Where(d => d.RdoCabeceraId == rdo.Id)
                                                 .ToList();
            }


            /* Avances de Obra Actuales hasta la fecha de generacion del Ultimo RDO */

            /* CODIGO ACTUAL*/
           /* var contruccion = _dobrarepository.GetAllIncluding(x => x.AvanceObra.Oferta, x => x.Computo.Item)
                                              .Where(x => x.vigente)
                                              .Where(x => x.Computo.vigente)
                                              .Where(x => x.AvanceObra.vigente)
                                              .Where(x => x.AvanceObra.aprobado)
                                              .Where(x => x.AvanceObra.Oferta.es_final)
                                              .Where(x => x.AvanceObra.Oferta.vigente)
                                              .Where(x => x.AvanceObra.Oferta.ProyectoId == proyectoid)
                                              .Where(x => x.AvanceObra.fecha_presentacion <= fechaCorte)
                                              .ToList();
                                              */

            /* Valores del Certificado Anterior */
            var certificado_anterior = Repository.GetAll().Where(x => x.ProyectoId == proyectoid)
                                                        .Where(x => x.estado_actual == 1)//Aprobado
                                                        .Where(x => x.vigente)
                                                        .Where(x => x.fecha_corte < cabecera.fecha_corte)
                                                        .Where(x => x.Id != cabecera.Id)
                                                        .OrderByDescending(x => x.fecha_corte)
                                                        .FirstOrDefault();

            if (certificado_anterior != null) {
            var rdoAnterior = _rdo_cabecera.GetAll().Where(r => r.es_definitivo)
                             .Where(r => r.estado)
                             .Where(r => r.vigente)
                             .Where(r => r.fecha_rdo <= certificado_anterior.fecha_corte)
                             .Where(r => r.ProyectoId == proyectoid)
                             .OrderByDescending(r => r.fecha_rdo).ToList()
                             .FirstOrDefault();


                if (rdoAnterior != null && rdoAnterior.Id > 0)
                {
                    detallesConstruccionAnteriorRdoCertificado = _rdo_detalle_EAC.GetAllIncluding(d => d.Computo.Item)
                                                     .Where(d => d.RdoCabeceraId == rdoAnterior.Id)
                                                     .ToList();
                }

            }


            hoja.Row(9).Height = 21;

            int c = 12;
            var cantidadsinaui = Convert.ToDecimal(0);

            //var items = _itemservice.ArbolItemsCertificadoSinPendientes(proyectoid, fechaCorte); //items contrato y fecha Sin Pendientes de AProbacion
            //  var itemsPendientes = _itemservice.ArbolItemsCertificadoPendientesAprobacion(proyectoid, fechaCorte); //items contrato y fecha Sin Pendientes de AProbacion


            var items = _itemservice.ArbolItemsCertificadoSinPendientesUltimoRdo(proyectoid, fechaCorte,detallesConstruccionUltimoRdo); //items contrato y fecha Sin Pendientes de AProbacion
              var itemsPendientes = _itemservice.ArbolItemsCertificadoPendientesAprobacionUltimoRdo(proyectoid, fechaCorte, detallesConstruccionUltimoRdo); //items contrato y fecha Sin Pendientes de AProbacion



            #region CuerpoItems
            foreach (var pitem in items)
            {
                var range = "C" + c;
                Color colorcell = Color.FromArgb(217, 217, 217);
                hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                range = "D" + c;
                hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                range = "E" + c;
                hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                range = "F" + c;
                hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                range = "G" + c;
                hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                range = "H" + c;
                hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                range = "I" + c;
                hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                range = "J" + c;
                hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                range = "K" + c;
                hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                range = "L" + c;
                hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                range = "M" + c;
                hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                range = "N" + c;
                hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                range = "O" + c;
                hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                range = "P" + c;
                hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                range = "Q" + c;
                hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);

                var especialidadItem = _repositoryitem.GetAllIncluding(i => i.Especialidad, i => i.Grupo).Where(i => i.Id == pitem.Id).FirstOrDefault();
                hoja.Cells[c, 18].Value = especialidadItem != null ? especialidadItem.Especialidad.codigo : "";
                hoja.Cells[c, 18].Style.Font.Color.SetColor(Color.FromArgb(159, 159, 159));

                hoja.Row(c).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Regular));
                hoja.Cells[c, 1].Value = pitem.GrupoId;


                hoja.Cells[c, 2].Value = pitem.Id;
                hoja.Cells[c, 3].Value = pitem.codigo;

                hoja.Cells[c, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                hoja.Cells[c, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                hoja.Cells[c, 4, c, 7].Merge = true;
                hoja.Cells[c, 4, c, 7].Value = pitem.nombre;


                hoja.Cells[c, 4, c, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                hoja.Cells[c, 4, c, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells[c, 4, c, 7].Style.WrapText = true;

                if (pitem.nombre.Length < 110)
                {
                    hoja.Row(c).Height = 22;
                }
                else
                if (pitem.nombre.Length < 220)
                {
                    hoja.Row(c).Height = 79;
                }
                else
                {
                    hoja.Row(c).Height = 101;
                }



                hoja.Cells[c, 1].Style.Font.Color.SetColor(Color.White);
                hoja.Cells[c, 2].Style.Font.Color.SetColor(Color.White);
                hoja.Cells[c, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells[c, 1].Style.Fill.BackgroundColor.SetColor(Color.White);
                hoja.Cells[c, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells[c, 2].Style.Fill.BackgroundColor.SetColor(Color.White);
                hoja.Cells[c, 1].Style.Border.BorderAround(ExcelBorderStyle.None);
                hoja.Cells[c, 2].Style.Border.BorderAround(ExcelBorderStyle.None);


                if (pitem.UnidadId != 0)
                {
                    hoja.Cells[c, 8].Value = this.nombrecatalogo(pitem.UnidadId);
                    hoja.Cells[c, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells[c, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                var precio_unitario = this.obtenerprecio_unitario(pitem.Id);
                if (precio_unitario > 0)
                {

                    hoja.Cells[c, 10].Value = precio_unitario;
                    hoja.Cells[c, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    hoja.Cells[c, 10].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                    string precio_unitario_d = hoja.Cells[c, 10].Address;

                    string cantidad_ant = hoja.Cells[c, 12].Address;

                    string costo_cantidad_ant = hoja.Cells[c, 15].Address;
                    var formula_cantidad_anterior = "$" + precio_unitario_d + "*$" + cantidad_ant;
                    hoja.Cells[c, 15].FormulaR1C1 = formula_cantidad_anterior;
                    hoja.Cells[c, 15].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells[c, 15].Style.Numberformat.Format =
                        "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                    hoja.Cells[c, 15].Style.WrapText = true;


                    string cantidad_acum = hoja.Cells[c, 14].Address;

                    string costo_cantidad_acum = hoja.Cells[c, 17].Address;
                    var formula_cantidad_acum = "$" + precio_unitario_d + "*$" + cantidad_acum;
                    hoja.Cells[c, 17].FormulaR1C1 = formula_cantidad_acum;
                    hoja.Cells[c, 17].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells[c, 17].Style.Numberformat.Format =
                        "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                    hoja.Cells[c, 17].Style.WrapText = true;

                    string cantidad_es = hoja.Cells[c, 13].Address;

                    //var formula_cantidad = "$" + precio_unitario_d + "*$" + cantidad_es;
                    var formula_cantidad = "$" + costo_cantidad_acum + "-$" + costo_cantidad_ant;
                    hoja.Cells[c, 16].FormulaR1C1 = formula_cantidad;
                    hoja.Cells[c, 16].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells[c, 16].Style.Numberformat.Format =
                        "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                    hoja.Cells[c, 16].Style.WrapText = true;

                }

                if (rdo != null && rdo.Id > 0)
                {
                    ultimoRdo = rdo;

                    //if (detallesConstruccionAnteriorRdoCertificado.Count > 0)
                    //{

                        int itemid = Convert.ToInt32(hoja.Cells[c, 2].Value != null ? hoja.Cells[c, 2].Value : "0");

                        var cantidadanterior = Convert.ToDecimal(0);

                        if (certificado_anterior != null && certificado_anterior.Id > 0)
                        {
                            cantidadanterior = Decimal.Round((from e in detallesConstruccionAnteriorRdoCertificado
                                                              where e.Computo.Item.GrupoId == 2
                                                              where e.Computo.Item.Id == itemid
                                                              //where e.Computo.Item.PendienteAprobacion == false
                                                              //where e.AvanceObra.fecha_presentacion < rdo.fecha_rdo



                                                         //     where e.AvanceObra.fecha_presentacion <= certificado_anterior.fecha_corte // CODIGO ACTUAL


                                                              select Decimal.Round(e.cantidad_acumulada, 8)
                                ).Sum(), 8);
                        }

                        /* CODIGO ACTUAL 
                         * var tempanterior = Decimal.Round((from e in contruccion
                                                           where e.Computo.Item.GrupoId == 2
                                                           where e.Computo.Item.Id == itemid
                                                           //where e.Computo.Item.PendienteAprobacion == false


                                                             where e.AvanceObra.fecha_presentacion < rdo.fecha_rdo // CODIGO ACTUAL


                                                           select Decimal.Round(e.cantidad_diaria, 8)
                             ).Sum(), 8);
                         var tempactual = Decimal.Round((from e in contruccion
                                                         where e.Computo.Item.GrupoId == 2
                                                         where e.Computo.Item.Id == itemid
                                                         //where e.Computo.Item.PendienteAprobacion == false


                                                           where e.AvanceObra.fecha_presentacion == rdo.fecha_rdo// CODIGO ACTUAL



                                                         select Decimal.Round(e.cantidad_diaria, 8)
                                   ).Sum(), 8);
                         var totalacumulada = tempanterior + tempactual;*/


                        /*Actualizacion Datos Ultimo Rdo*/
                        var totalacumulada = Decimal.Round((from e in detallesConstruccionUltimoRdo
                                                            where e.Computo.Item.GrupoId == 2
                                                            where e.Computo.Item.Id == itemid
                                                            //where e.Computo.Item.PendienteAprobacion == false
                                                            select Decimal.Round(e.cantidad_acumulada, 8)
                               ).Sum(), 8);

                        decimal periodoactual = Decimal.Round(totalacumulada - cantidadanterior, 8);

                        cantidadsinaui = periodoactual + cantidadsinaui;

                        if (cantidadanterior > 0)
                        {
                            hoja.Cells[c, 12].Value = cantidadanterior;
                            hoja.Cells[c, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            hoja.Cells[c, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            hoja.Cells[c, 12].Style.WrapText = true;
                        }
                        else
                        {
                            hoja.Cells[c, 12].Value = cantidadanterior;
                            hoja.Cells[c, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            hoja.Cells[c, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        hoja.Cells[c, 12].Style.Numberformat.Format = "#,##0.00";

                        if (totalacumulada > 0)
                        {
                            hoja.Cells[c, 14].Value = totalacumulada;
                            hoja.Cells[c, 14].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            hoja.Cells[c, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            hoja.Cells[c, 14].Style.WrapText = true;
                        }
                        else
                        {
                            hoja.Cells[c, 14].Value = totalacumulada;
                            hoja.Cells[c, 14].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            hoja.Cells[c, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            hoja.Cells[c, 14].Style.WrapText = true;
                        }
                        hoja.Cells[c, 14].Style.Numberformat.Format = "#,##0.00";


                        string cantidad_ant = hoja.Cells[c, 12].Address;
                        string cantidad_acum = hoja.Cells[c, 14].Address;

                        //var formula_cantidad = "$" + precio_unitario_d + "*$" + cantidad_es;
                        var formula_cantidad = "$" + cantidad_acum + "-$" + cantidad_ant;
                        hoja.Cells[c, 13].FormulaR1C1 = formula_cantidad;
                        hoja.Cells[c, 13].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja.Cells[c, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja.Cells[c, 13].Style.Numberformat.Format = "#,##0.00";
                        hoja.Cells[c, 13].Style.WrapText = true;







                    //}
                }


                if (pitem.item_padre == ".")
                {
                    hoja.Cells[c, 3, c, 17].Style.Font.Bold = true;
                    hoja.Cells[c, 3, c, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja.Cells[c, 3, c, 17].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    hoja.Cells[c, 3, c, 17].Style.Border.Top.Style =
                    hoja.Cells[c, 3, c, 17].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    hoja.Cells[c, 12].Value = "";
                    hoja.Cells[c, 13].Value = "";
                    hoja.Cells[c, 14].Value = "";
                }
                if (pitem.item_padre != "." && pitem.para_oferta == false)
                {
                    hoja.Cells[c, 3, c, 17].Style.Font.Bold = true;
                    hoja.Cells[c, 3, c, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja.Cells[c, 3, c, 17].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    hoja.Cells[c, 3, c, 17].Style.Border.Top.Style =
                    hoja.Cells[c, 3, c, 17].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    hoja.Cells[c, 12].Value = "";
                    hoja.Cells[c, 13].Value = "";
                    hoja.Cells[c, 14].Value = "";
                }


                hoja.Cells[c, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                hoja.Cells[c, 3].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                hoja.Cells[c, hoja.Dimension.End.Column].Style.Border.Right.Style = ExcelBorderStyle.Medium;




                c = c + 1;
            }

            c = c + 1;
            if (itemsPendientes.Count > 0)
            {

                hoja.Cells[c, 3, c, 17].Style.Font.Bold = true;
                hoja.Cells[c, 3, c, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells[c, 3, c, 17].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                hoja.Cells[c, 3, c, 17].Style.Border.Top.Style =
                hoja.Cells[c, 3, c, 17].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                hoja.Cells[c, 4, c, 7].Merge = true;
                hoja.Cells[c, 4, c, 7].Value = "OBRAS COMPLEMENTARIAS - EN PROCESO DE APROBACIÓN";
                hoja.Row(c).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                c = c + 1;

                foreach (var pitem in itemsPendientes)
                {
                    var range = "C" + c;
                    Color colorcell = Color.FromArgb(217, 217, 217);
                    hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                    range = "D" + c;
                    hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                    range = "E" + c;
                    hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                    range = "F" + c;
                    hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                    range = "G" + c;
                    hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                    range = "H" + c;
                    hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                    range = "I" + c;
                    hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                    range = "J" + c;
                    hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                    range = "K" + c;
                    hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                    range = "L" + c;
                    hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                    range = "M" + c;
                    hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                    range = "N" + c;
                    hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                    range = "O" + c;
                    hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                    range = "P" + c;
                    hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);
                    range = "Q" + c;
                    hoja.Cells[range].Style.Border.BorderAround(ExcelBorderStyle.Thin, colorcell);



                    var especialidadItem = _repositoryitem.GetAllIncluding(i => i.Especialidad, i => i.Grupo).Where(i => i.Id == pitem.Id).FirstOrDefault();
                    hoja.Cells[c, 18].Value = especialidadItem != null ? especialidadItem.Especialidad.codigo : "";
                    hoja.Cells[c, 18].Style.Font.Color.SetColor(Color.FromArgb(159, 159, 159));


                    hoja.Row(c).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Regular));

                    hoja.Cells[c, 1].Value = pitem.GrupoId;

                    /* if (pitem.codigo.Length < 110)
                     {
                         hoja.Row(c).Height = 21;
                     }*/
                    hoja.Cells[c, 2].Value = pitem.Id;
                    hoja.Cells[c, 3].Value = pitem.codigo;
                    hoja.Cells[c, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells[c, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    hoja.Cells[c, 4, c, 7].Merge = true;
                    hoja.Cells[c, 4, c, 7].Value = pitem.nombre;
                    hoja.Cells[c, 4, c, 7].Style.WrapText = true;
                    hoja.Cells[c, 4, c, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    hoja.Cells[c, 4, c, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells[c, 4, c, 7].Style.WrapText = true;

                    if (pitem.nombre.Length < 110)
                    {
                        hoja.Row(c).Height = 22;
                    }
                    else
                    if (pitem.nombre.Length < 220)
                    {
                        hoja.Row(c).Height = 79;
                    }
                    else
                    {
                        hoja.Row(c).Height = 115;
                    }




                    hoja.Cells[c, 1].Style.Font.Color.SetColor(Color.White);
                    hoja.Cells[c, 2].Style.Font.Color.SetColor(Color.White);
                    hoja.Cells[c, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja.Cells[c, 1].Style.Fill.BackgroundColor.SetColor(Color.White);
                    hoja.Cells[c, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja.Cells[c, 2].Style.Fill.BackgroundColor.SetColor(Color.White);
                    hoja.Cells[c, 1].Style.Border.BorderAround(ExcelBorderStyle.None);
                    hoja.Cells[c, 2].Style.Border.BorderAround(ExcelBorderStyle.None);


                    if (pitem.UnidadId != 0)
                    {
                        hoja.Cells[c, 8].Value = this.nombrecatalogo(pitem.UnidadId);
                        hoja.Cells[c, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja.Cells[c, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    var precio_unitario = this.obtenerprecio_unitario(pitem.Id);
                    if (precio_unitario > 0)
                    {

                        hoja.Cells[c, 10].Value = precio_unitario;
                        hoja.Cells[c, 10].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja.Cells[c, 10].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                        string precio_unitario_d = hoja.Cells[c, 10].Address;

                        string cantidad_ant = hoja.Cells[c, 12].Address;

                        string costo_cantidad_ant = hoja.Cells[c, 15].Address;
                        var formula_cantidad_anterior = "$" + precio_unitario_d + "*$" + cantidad_ant;
                        hoja.Cells[c, 15].FormulaR1C1 = formula_cantidad_anterior;
                        hoja.Cells[c, 15].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja.Cells[c, 15].Style.Numberformat.Format =
                            "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                        hoja.Cells[c, 15].Style.WrapText = true;







                        string cantidad_acum = hoja.Cells[c, 14].Address;

                        string costo_cantidad_acum = hoja.Cells[c, 17].Address;
                        var formula_cantidad_acum = "$" + precio_unitario_d + "*$" + cantidad_acum;
                        hoja.Cells[c, 17].FormulaR1C1 = formula_cantidad_acum;
                        hoja.Cells[c, 17].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja.Cells[c, 17].Style.Numberformat.Format =
                            "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                        hoja.Cells[c, 17].Style.WrapText = true;

                        string cantidad_es = hoja.Cells[c, 13].Address;

                        //var formula_cantidad = "$" + precio_unitario_d + "*$" + cantidad_es;
                        var formula_cantidad = "$" + costo_cantidad_acum + "-$" + costo_cantidad_ant;
                        hoja.Cells[c, 16].FormulaR1C1 = formula_cantidad;
                        hoja.Cells[c, 16].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja.Cells[c, 16].Style.Numberformat.Format =
                            "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                        hoja.Cells[c, 16].Style.WrapText = true;

                    }


                    if (ultimoRdo != null && ultimoRdo.Id > 0)
                    {


                        

                            int itemid = Convert.ToInt32(hoja.Cells[c, 2].Value != null ? hoja.Cells[c, 2].Value : "0");

                            var cantidadanterior = Convert.ToDecimal(0);

                  
                            if (certificado_anterior != null && certificado_anterior.Id > 0)
                            {
                                cantidadanterior = Decimal.Round((from e in detallesConstruccionAnteriorRdoCertificado
                                                                  where e.Computo.Item.GrupoId == 2
                                                                  where e.Computo.Item.Id == itemid
                                                                  //where e.Computo.Item.PendienteAprobacion == false
                                                                  //where e.AvanceObra.fecha_presentacion < rdo.fecha_rdo


                                                                  //  where e.AvanceObra.fecha_presentacion <= certificado_anterior.fecha_corte// CODIGO ACTUAL


                                                                  select Decimal.Round(e.cantidad_acumulada, 8)
                                    ).Sum(), 8);
                            }

                            /*var tempanterior = Decimal.Round((from e in contruccion
                                                              where e.Computo.Item.GrupoId == 2
                                                              where e.Computo.Item.Id == itemid
                                                              //where e.Computo.Item.PendienteAprobacion == false

                                                              where e.AvanceObra.fecha_presentacion < ultimoRdo.fecha_rdo //CODIGO ACTUAL

                                                              select Decimal.Round(e.cantidad_diaria, 8)
                                ).Sum(), 8);
                            var tempactual = Decimal.Round((from e in contruccion
                                                            where e.Computo.Item.GrupoId == 2
                                                            where e.Computo.Item.Id == itemid
                                                            //where e.Computo.Item.PendienteAprobacion == false


                                                            where e.AvanceObra.fecha_presentacion == ultimoRdo.fecha_rdo// CODIGO ACTUAL 


                                                            select Decimal.Round(e.cantidad_diaria, 8)
                                      ).Sum(), 8);
                            var totalacumulada = tempanterior + tempactual;*/

                        
                            /*Actualizacion Datos Ultimo Rdo Pendientes*/
                            var totalacumulada = Decimal.Round((from e in detallesConstruccionUltimoRdo
                                                                where e.Computo.Item.GrupoId == 2
                                                                where e.Computo.Item.Id == itemid
                                                                //where e.Computo.Item.PendienteAprobacion == false
                                                                select Decimal.Round(e.cantidad_acumulada, 8)
                                   ).Sum(), 8);

                            decimal periodoactual = Decimal.Round(totalacumulada - cantidadanterior, 8);
                            cantidadsinaui = periodoactual + cantidadsinaui;


                            if (cantidadanterior > 0)
                            {
                                hoja.Cells[c, 12].Value = cantidadanterior;
                                hoja.Cells[c, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[c, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[c, 12].Style.WrapText = true;
                            }
                            else
                            {
                                hoja.Cells[c, 12].Value = cantidadanterior;
                                hoja.Cells[c, 12].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[c, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[c, 12].Style.WrapText = true;
                            }
                            hoja.Cells[c, 12].Style.Numberformat.Format =
              "#,##0.00";

                            if (totalacumulada > 0)
                            {
                                hoja.Cells[c, 14].Value = totalacumulada;
                                hoja.Cells[c, 14].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[c, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[c, 14].Style.WrapText = true;
                            }
                            else
                            {
                                hoja.Cells[c, 14].Value = totalacumulada;
                                hoja.Cells[c, 14].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                hoja.Cells[c, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                hoja.Cells[c, 14].Style.WrapText = true;
                            }

                            hoja.Cells[c, 14].Style.Numberformat.Format =
                 "#,##0.00";
                            string cantidad_ant = hoja.Cells[c, 12].Address;
                            string cantidad_acum = hoja.Cells[c, 14].Address;

                            //var formula_cantidad = "$" + precio_unitario_d + "*$" + cantidad_es;
                            var formula_cantidad = "$" + cantidad_acum + "-$" + cantidad_ant;
                            hoja.Cells[c, 13].FormulaR1C1 = formula_cantidad;
                            hoja.Cells[c, 13].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            hoja.Cells[c, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            hoja.Cells[c, 13].Style.Numberformat.Format =
                            "#,##0.00";
                            hoja.Cells[c, 13].Style.WrapText = true;







                        
                    }



                    if (pitem.item_padre == ".")
                    {
                        hoja.Cells[c, 3, c, 17].Style.Font.Bold = true;
                        hoja.Cells[c, 3, c, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja.Cells[c, 3, c, 17].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        hoja.Cells[c, 3, c, 17].Style.Border.Top.Style =
                        hoja.Cells[c, 3, c, 17].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        hoja.Cells[c, 3, c, 17].Style.Border.Top.Style =
   hoja.Cells[c, 3, c, 17].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        hoja.Cells[c, 3, c, 17].Style.Border.Top.Color.SetColor(Color.Black);
                        hoja.Cells[c, 3, c, 17].Style.Border.Bottom.Color.SetColor(Color.Black);
                        hoja.Cells[c, 12].Value = "";
                        hoja.Cells[c, 13].Value = "";
                        hoja.Cells[c, 14].Value = "";
                    }
                    if (pitem.item_padre != "." && pitem.para_oferta == false)
                    {
                        hoja.Cells[c, 3, c, 17].Style.Font.Bold = true;
                        hoja.Cells[c, 3, c, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja.Cells[c, 3, c, 17].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                        hoja.Cells[c, 3, c, 17].Style.Border.Top.Style =
                        hoja.Cells[c, 3, c, 17].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        hoja.Cells[c, 3, c, 17].Style.Border.Top.Color.SetColor(Color.Black);
                        hoja.Cells[c, 3, c, 17].Style.Border.Bottom.Color.SetColor(Color.Black);
                        hoja.Cells[c, 12].Value = "";
                        hoja.Cells[c, 13].Value = "";
                        hoja.Cells[c, 14].Value = "";
                    }


                    hoja.Cells[c, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    hoja.Cells[c, 3].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    hoja.Cells[c, hoja.Dimension.End.Column].Style.Border.Right.Style = ExcelBorderStyle.Medium;


                    c = c + 1;
                }


            }
            c = c + 1;


            hoja.Cells["C" + hoja.Dimension.End.Row + ":" + "Q" + hoja.Dimension.End.Row].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

            hoja.Cells[11, 3, c, 15].AutoFilter = true;

            #endregion

            //rangos formatos

            hoja.Cells["Q12" + ":" + "Q" + hoja.Dimension.End.Row].Style.Numberformat.Format =
       "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";






            var noOfCol = 16; //columna de los Calculos
            var noOfRow = hoja.Dimension.End.Row;


            // rango total
            string costoi = hoja.Cells[12, 16].Address;
            string costoif = hoja.Cells[noOfRow, 16].Address;
            string rangovalortotal = "$" + costoi + ":$" + costoif;

            string rangovalortotali = "$" + hoja.Cells[12, 15].Address + ":$" + hoja.Cells[noOfRow, 15].Address;
            string rangovalortotald = "$" + hoja.Cells[12, 17].Address + ":$" + hoja.Cells[noOfRow, 17].Address;
            int bfila = noOfRow;

            bfila = bfila + 2;
            //CALCULOS BASE
            //Ingenieria


            ExcelWorksheet n = hoja;

            #region SubtotalesAnteriores
            n.Cells[bfila, 4, bfila, 7].Merge = true;
            n.Cells[bfila, 4, bfila, 7].Value = "Sub - total Ingeniería";
            n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
            n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));

            n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));


            n.Row(bfila).Height = 28;

            n.Cells[bfila, 4, bfila, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            //suma ingenieria

            string dinicio = n.Cells[12, 1].Address;
            string dfinal = n.Cells[noOfRow, 1].Address;
            string rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            string grupo = "" + 1 + "";

            var formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            n.Cells[bfila, noOfCol].FormulaR1C1 = formula;
            n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol].Style.WrapText = true;

            string sumaingenieria = "$" + n.Cells[bfila, noOfCol].Address;


            n.Cells[bfila, noOfCol].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol].Style.Font.Bold = true;

            n.Cells[bfila, noOfCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            n.Cells[bfila, noOfCol].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
            n.Cells[bfila, noOfCol].Style.Font.Color.SetColor(Color.White);
            n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            //I

            n.Cells[bfila, noOfCol - 1].Value = 0.0;
            n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

            string SII = "$" + n.Cells[bfila, noOfCol - 1].Address;


            n.Cells[bfila, noOfCol - 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;




            var formulai = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotali + ")";
            n.Cells[bfila, noOfCol - 1].FormulaR1C1 = formulai;
            n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

            string sumaingenieriai = "$" + n.Cells[bfila, noOfCol - 1].Address;

            //D


            n.Cells[bfila, noOfCol + 1].Value = 0.0;
            n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

            string SID = "$" + n.Cells[bfila, noOfCol + 1].Address;


            n.Cells[bfila, noOfCol + 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;


            formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotald + ")";
            n.Cells[bfila, noOfCol + 1].FormulaR1C1 = formula;
            n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol].Style.WrapText = true;

            string sumaingenieriad = "$" + n.Cells[bfila, noOfCol + 1].Address;


            bfila = bfila + 2;
            //Procura

            n.Cells[bfila, 4, bfila, 7].Merge = true;
            n.Cells[bfila, 4, bfila, 7].Value = "Sub-total Procura";
            n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
            n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Row(bfila).Height = 28;
            n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //suma procura

            dinicio = n.Cells[12, 1].Address;
            dfinal = n.Cells[noOfRow, 1].Address;
            rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            grupo = "" + 3 + "";

            formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            n.Cells[bfila, noOfCol].FormulaR1C1 = formula;
            n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol].Style.WrapText = true;


            string suma_procura = "$" + n.Cells[bfila, noOfCol].Address;
            n.Cells[bfila, noOfCol].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol].Style.Font.Bold = true;

            n.Cells[bfila, noOfCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            n.Cells[bfila, noOfCol].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
            n.Cells[bfila, noOfCol].Style.Font.Color.SetColor(Color.White);
            n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            //I

            n.Cells[bfila, noOfCol - 1].Value = 0.0;
            n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

            string SPI = "$" + n.Cells[bfila, noOfCol - 1].Address;


            n.Cells[bfila, noOfCol - 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;

            formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotali + ")";
            n.Cells[bfila, noOfCol - 1].FormulaR1C1 = formula;
            n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

            string sumaprocurai = "$" + n.Cells[bfila, noOfCol - 1].Address;

            //D


            n.Cells[bfila, noOfCol + 1].Value = 0.0;
            n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

            string SPD = "$" + n.Cells[bfila, noOfCol + 1].Address;


            n.Cells[bfila, noOfCol + 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;

            formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotald + ")";
            n.Cells[bfila, noOfCol + 1].FormulaR1C1 = formula;
            n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

            string sumaprocurad = "$" + n.Cells[bfila, noOfCol + 1].Address;


            //CALCULOS BASE
            //Reembolsables
            bfila = bfila + 2;

            n.Cells[bfila, 4, bfila, 7].Merge = true;
            n.Cells[bfila, 4, bfila, 7].Value = "Sub-total Reembolsables";
            n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
            n.Row(bfila).Height = 28;
            n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            n.Cells[bfila, noOfCol].Value = 0;
            n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol].Style.WrapText = true;

            string sumareembolsables = "$" + n.Cells[bfila, noOfCol].Address;

            n.Cells[bfila, noOfCol].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol].Style.Font.Bold = true;

            n.Cells[bfila, noOfCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            n.Cells[bfila, noOfCol].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
            n.Cells[bfila, noOfCol].Style.Font.Color.SetColor(Color.White);
            n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            //I

            n.Cells[bfila, noOfCol - 1].Value = 0.0;
            n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

            string SRRI = "$" + n.Cells[bfila, noOfCol - 1].Address;


            n.Cells[bfila, noOfCol - 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;



            //D


            n.Cells[bfila, noOfCol + 1].Value = 0.0;
            n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

            string SRRD = "$" + n.Cells[bfila, noOfCol + 1].Address;


            n.Cells[bfila, noOfCol + 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;


            #endregion


            string formulaConstruccionDescuentos = "";
            string formulaConstruccionDescuentosI = "";
            string formulaConstruccionDescuentosD = "";
            if (esSegundoFormato)
            {

                bfila = bfila + 2;
                //Sub Total Civil

                n.Cells[bfila, 4, bfila, 7].Merge = true;
                n.Cells[bfila, 4, bfila, 7].Value = "Subtotal Civil";
                n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
                n.Row(bfila).Height = 28;
                n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                n.Cells[bfila, 4, bfila, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                dinicio = n.Cells[12, 18].Address;
                dfinal = n.Cells[noOfRow, 18].Address;
                rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
                grupo = '"' + ProyectoCodigos.OBRAS_CIVILES + '"';

                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
                n.Cells[bfila, noOfCol].FormulaR1C1 = formula;

                string sumaObrasCiviles = "$" + n.Cells[bfila, noOfCol].Address;
                //n.Cells["A1"].FormulaR1C1 = formula;


                n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol].Style.WrapText = true;


                n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol].Style.Font.Bold = true;

                n.Cells[bfila, noOfCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                n.Cells[bfila, noOfCol].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
                n.Cells[bfila, noOfCol].Style.Font.Color.SetColor(Color.White);
                n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                // VALOR IZQUIERDO
                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotali + ")";
                n.Cells[bfila, noOfCol - 1].FormulaR1C1 = formula;
                n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

                string sumaObraCivili = "$" + n.Cells[bfila, noOfCol - 1].Address;


                n.Cells[bfila, noOfCol - 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;

                //DERECCHO OBRA CIVIL
                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotald + ")";
                n.Cells[bfila, noOfCol + 1].FormulaR1C1 = formula;

                n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

                string sumaObraCivild = "$" + n.Cells[bfila, noOfCol + 1].Address;

                n.Cells[bfila, noOfCol + 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;



                bfila = bfila + 2;

                //SUBTOTAL MECANICA

                n.Cells[bfila, 4, bfila, 7].Merge = true;
                n.Cells[bfila, 4, bfila, 7].Value = "Subtotal Mecánica";
                n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
                n.Row(bfila).Height = 28;
                n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                n.Cells[bfila, 4, bfila, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                dinicio = n.Cells[12, 18].Address;
                dfinal = n.Cells[noOfRow, 18].Address;
                rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
                grupo = '"' + ProyectoCodigos.OBRAS_MECANICAS + '"';

                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
                n.Cells[bfila, noOfCol].FormulaR1C1 = formula;

                string sumaObrasMecanicas = "$" + n.Cells[bfila, noOfCol].Address;
                //n.Cells["A1"].FormulaR1C1 = formula;


                n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol].Style.WrapText = true;


                n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol].Style.Font.Bold = true;

                n.Cells[bfila, noOfCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                n.Cells[bfila, noOfCol].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
                n.Cells[bfila, noOfCol].Style.Font.Color.SetColor(Color.White);
                n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                // VALOR IZQUIERDO
                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotali + ")";
                n.Cells[bfila, noOfCol - 1].FormulaR1C1 = formula;
                n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

                string sumaObraMecanicai = "$" + n.Cells[bfila, noOfCol - 1].Address;


                n.Cells[bfila, noOfCol - 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;

                //DERECCHO OBRA CIVIL
                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotald + ")";
                n.Cells[bfila, noOfCol + 1].FormulaR1C1 = formula;

                n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

                string sumaObraMecanicad = "$" + n.Cells[bfila, noOfCol + 1].Address;

                n.Cells[bfila, noOfCol + 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;


                bfila = bfila + 2;

                //SUBTOTAL ELECTRICA

                n.Cells[bfila, 4, bfila, 7].Merge = true;
                n.Cells[bfila, 4, bfila, 7].Value = "Subtotal Eléctrica";
                n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
                n.Row(bfila).Height = 28;
                n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                n.Cells[bfila, 4, bfila, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                dinicio = n.Cells[12, 18].Address;
                dfinal = n.Cells[noOfRow, 18].Address;
                rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
                grupo = '"' + ProyectoCodigos.OBRAS_ELECTRICAS + '"';

                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
                n.Cells[bfila, noOfCol].FormulaR1C1 = formula;

                string sumaObrasElectricas = "$" + n.Cells[bfila, noOfCol].Address;
                //n.Cells["A1"].FormulaR1C1 = formula;


                n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol].Style.WrapText = true;


                n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol].Style.Font.Bold = true;

                n.Cells[bfila, noOfCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                n.Cells[bfila, noOfCol].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
                n.Cells[bfila, noOfCol].Style.Font.Color.SetColor(Color.White);
                n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                // VALOR IZQUIERDO
                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotali + ")";
                n.Cells[bfila, noOfCol - 1].FormulaR1C1 = formula;
                n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

                string sumaObraElectricasi = "$" + n.Cells[bfila, noOfCol - 1].Address;


                n.Cells[bfila, noOfCol - 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;

                //DERECCHO 
                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotald + ")";
                n.Cells[bfila, noOfCol + 1].FormulaR1C1 = formula;

                n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

                string sumaObraElectricasd = "$" + n.Cells[bfila, noOfCol + 1].Address;

                n.Cells[bfila, noOfCol + 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;



                bfila = bfila + 2;

                //SUBTOTAL INSTRUMENTACION

                n.Cells[bfila, 4, bfila, 7].Merge = true;
                n.Cells[bfila, 4, bfila, 7].Value = "Subtotal Instrumentacion";
                n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
                n.Row(bfila).Height = 28;
                n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                n.Cells[bfila, 4, bfila, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                dinicio = n.Cells[12, 18].Address;
                dfinal = n.Cells[noOfRow, 18].Address;
                rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
                grupo = '"' + ProyectoCodigos.OBRAS_INSTRUMENTOS_CONTROL + '"';

                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
                n.Cells[bfila, noOfCol].FormulaR1C1 = formula;

                string sumaInstrumentacionyControl = "$" + n.Cells[bfila, noOfCol].Address;
                //n.Cells["A1"].FormulaR1C1 = formula;


                n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol].Style.WrapText = true;


                n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol].Style.Font.Bold = true;

                n.Cells[bfila, noOfCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                n.Cells[bfila, noOfCol].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
                n.Cells[bfila, noOfCol].Style.Font.Color.SetColor(Color.White);
                n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                // VALOR IZQUIERDO
                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotali + ")";
                n.Cells[bfila, noOfCol - 1].FormulaR1C1 = formula;
                n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

                string sumaInstrumentacionyControli = "$" + n.Cells[bfila, noOfCol - 1].Address;


                n.Cells[bfila, noOfCol - 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;

                //DERECCHO 
                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotald + ")";
                n.Cells[bfila, noOfCol + 1].FormulaR1C1 = formula;

                n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

                string sumaInstrumentacionyControld = "$" + n.Cells[bfila, noOfCol + 1].Address;

                n.Cells[bfila, noOfCol + 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;




                bfila = bfila + 2;

                //SUBTOTAL SERVICIOS ESPECIALES

                n.Cells[bfila, 4, bfila, 7].Merge = true;
                n.Cells[bfila, 4, bfila, 7].Value = "Subtotal Servicios Especiales";
                n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
                n.Row(bfila).Height = 28;
                n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                n.Cells[bfila, 4, bfila, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                dinicio = n.Cells[12, 18].Address;
                dfinal = n.Cells[noOfRow, 18].Address;
                rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
                grupo = '"' + ProyectoCodigos.SERVICIOS_EPECIALES + '"';

                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
                n.Cells[bfila, noOfCol].FormulaR1C1 = formula;

                string sumaServiciosEspeciales = "$" + n.Cells[bfila, noOfCol].Address;
                //n.Cells["A1"].FormulaR1C1 = formula;


                n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol].Style.WrapText = true;


                n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol].Style.Font.Bold = true;

                n.Cells[bfila, noOfCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                n.Cells[bfila, noOfCol].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
                n.Cells[bfila, noOfCol].Style.Font.Color.SetColor(Color.White);
                n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                // VALOR IZQUIERDO
                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotali + ")";
                n.Cells[bfila, noOfCol - 1].FormulaR1C1 = formula;
                n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

                string sumaServiciosEspecialesi = "$" + n.Cells[bfila, noOfCol - 1].Address;


                n.Cells[bfila, noOfCol - 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;

                //DERECCHO 
                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotald + ")";
                n.Cells[bfila, noOfCol + 1].FormulaR1C1 = formula;

                n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

                string sumaServiciosEspecialesd = "$" + n.Cells[bfila, noOfCol + 1].Address;

                n.Cells[bfila, noOfCol + 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;



                //Descuento
                bfila = bfila + 2;


                n.Cells[bfila, 4, bfila, 7].Merge = true;
                n.Cells[bfila, 4, bfila, 7].Value = "Descuento 1% ítems Mecánicos, Eléctricos e Instrumentación";
                n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
                n.Row(bfila).Height = 28;
                n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                n.Cells[bfila, 4, bfila, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                n.Cells[bfila, 8].Value = -0.01;
                n.Cells[bfila, 8].Style.WrapText = true;
                n.Cells[bfila, 8].Style.Font.Color.SetColor(Color.White);

                n.Cells[bfila, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                n.Cells[bfila, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                n.Cells[bfila, 8].Style.Font.Bold = true;
                n.Cells[bfila, 8].Style.Numberformat.Format = "0.00%";


                //caculo
                string formulaDescuento = "=$" + n.Cells[bfila, 8].Address + "*(" + sumaObrasMecanicas + "+" + sumaObrasElectricas + "+" + sumaInstrumentacionyControl + ")";
                n.Cells[bfila, noOfCol].FormulaR1C1 = formulaDescuento;
                n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol].Style.WrapText = true;


                string valorSumaDescuento = n.Cells[bfila, noOfCol].Address;
                n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                n.Cells[bfila, noOfCol].Style.Font.Bold = true;
                n.Cells[bfila, noOfCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
                n.Cells[bfila, noOfCol].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
                n.Cells[bfila, noOfCol].Style.Font.Color.SetColor(Color.White);
                n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                //

                //I
                formulaDescuento = "=$" + n.Cells[bfila, 8].Address + "*(" + sumaObraMecanicai + "+" + sumaObraElectricasi + "+" + sumaInstrumentacionyControli + ")";
                n.Cells[bfila, noOfCol - 1].FormulaR1C1 = formulaDescuento;
                n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                    "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

                string sumaDescuentoI = "$" + n.Cells[bfila, noOfCol - 1].Address;


                n.Cells[bfila, noOfCol - 1].Style.Border.Top.Style =
                n.Cells[bfila, noOfCol - 1].Style.Border.Left.Style =
                n.Cells[bfila, noOfCol - 1].Style.Border.Right.Style =
                n.Cells[bfila, noOfCol - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;

                //D

                formulaDescuento = "=$" + n.Cells[bfila, 8].Address + "*(" + sumaObraMecanicad + "+" + sumaObraElectricasd + "+" + sumaInstrumentacionyControld + ")";
                n.Cells[bfila, noOfCol + 1].FormulaR1C1 = formulaDescuento;
                n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                      "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

                string sumaDescuentoD = "$" + n.Cells[bfila, noOfCol + 1].Address;


                n.Cells[bfila, noOfCol + 1].Style.Border.Top.Style =
                n.Cells[bfila, noOfCol + 1].Style.Border.Left.Style =
                n.Cells[bfila, noOfCol + 1].Style.Border.Right.Style =
                n.Cells[bfila, noOfCol + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;


                formulaConstruccionDescuentos = "=" + sumaObrasCiviles + "+" + sumaObrasMecanicas + "+" + sumaObrasElectricas + "+" + sumaInstrumentacionyControl + "+" + sumaServiciosEspeciales + "+" + valorSumaDescuento;
                formulaConstruccionDescuentosI = "=" + sumaObraCivili + "+" + sumaObraMecanicai + "+" + sumaObraElectricasi + "+" + sumaInstrumentacionyControli + "+" + sumaServiciosEspecialesi + "+" + sumaDescuentoI;
                formulaConstruccionDescuentosD = "=" + sumaObraCivild + "+" + sumaObraMecanicad + "+" + sumaObraElectricasd + "+" + sumaInstrumentacionyControld + "+" + sumaServiciosEspecialesd + "+" + sumaDescuentoD;

            }



            //Consturccion
            bfila = bfila + 2;



            n.Cells[bfila, 4, bfila, 7].Merge = true;
            n.Cells[bfila, 4, bfila, 7].Value = "Sub-total Construcción";
            n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
            n.Row(bfila).Height = 28;
            n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            dinicio = n.Cells[12, 1].Address;
            dfinal = n.Cells[noOfRow, 1].Address;
            rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            grupo = "" + 2 + "";

            if (esSegundoFormato)
            {
                formula = formulaConstruccionDescuentos;
            }
            else
            {
                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            }

            n.Cells[bfila, noOfCol].FormulaR1C1 = formula;


            string suma_contruccion = "$" + n.Cells[bfila, noOfCol].Address;
            n.Cells["A1"].FormulaR1C1 = formula;






            n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol].Style.WrapText = true;


            n.Cells[bfila, noOfCol].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol].Style.Font.Bold = true;

            n.Cells[bfila, noOfCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            n.Cells[bfila, noOfCol].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
            n.Cells[bfila, noOfCol].Style.Font.Color.SetColor(Color.White);
            n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);



            //I
            if (esSegundoFormato)
            {
                formula = formulaConstruccionDescuentosI;
            }
            else
            {
                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotali + ")";
            }



            n.Cells[bfila, noOfCol - 1].FormulaR1C1 = formula;
            n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

            string suma_contruccioni = "$" + n.Cells[bfila, noOfCol - 1].Address;


            n.Cells[bfila, noOfCol - 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;
            //D

            if (esSegundoFormato)
            {
                formula = formulaConstruccionDescuentosD;
            }
            else
            {
                formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotald + ")";
            }


            n.Cells[bfila, noOfCol + 1].FormulaR1C1 = formula;

            n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

            string suma_contrucciond = "$" + n.Cells[bfila, noOfCol + 1].Address;


            n.Cells[bfila, noOfCol + 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;

            //adminitracion
            bfila = bfila + 2;



            n.Cells[bfila, 4, bfila, 7].Merge = true;
            n.Cells[bfila, 4, bfila, 7].Value = "Administración";
            n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
            n.Row(bfila).Height = 28;
            n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            string administracion = "$" + n.Cells[bfila, noOfCol].Address;
            n.Cells[bfila, noOfCol].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol].Style.Font.Bold = true;



            n.Cells[bfila, noOfCol].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol].Style.Font.Bold = true;

            n.Cells[bfila, noOfCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            n.Cells[bfila, noOfCol].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
            n.Cells[bfila, noOfCol].Style.Font.Color.SetColor(Color.White);
            n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            //I

            n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

            string administracioni = "$" + n.Cells[bfila, noOfCol - 1].Address;


            n.Cells[bfila, noOfCol - 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;

            //D


            n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

            string administracionid = "$" + n.Cells[bfila, noOfCol + 1].Address;


            n.Cells[bfila, noOfCol + 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;




            //
            bfila++;

            n.Cells[bfila, 4, bfila, 7].Merge = true;
            n.Cells[bfila, 4, bfila, 7].Value = "Administración sobre Obra (%)";
            // n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 12, FontStyle.Regular));
            n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
            n.Row(bfila).Height = 21;
            n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.Border.Top.Style =
            n.Cells[bfila, 4, bfila, 7].Style.Border.Left.Style =
            n.Cells[bfila, 4, bfila, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.Font.Bold = true;



            n.Cells[bfila, 8].Value = 0.4119;
            n.Cells[bfila, 8].Style.WrapText = true;

            n.Cells[bfila, 8].Style.Border.Top.Style =
            n.Cells[bfila, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, 8].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            n.Cells[bfila, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 8].Style.Font.Bold = true;
            n.Cells[bfila, 8].Style.Numberformat.Format = "0.0%";

            //contruccion

            string formula_calculo = "=" + n.Cells[bfila, 8].Address + "*" + suma_contruccion;
            n.Cells[bfila, noOfCol].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol].Style.WrapText = true;

            string valoradministracion_obra = n.Cells[bfila, noOfCol].Address;

            n.Cells[bfila, noOfCol].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol].Style.Font.Bold = true;


            //I
            formula_calculo = "=" + n.Cells[bfila, 8].Address + "*" + suma_contruccioni;
            n.Cells[bfila, noOfCol - 1].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

            string ADOI = "$" + n.Cells[bfila, noOfCol - 1].Address;


            n.Cells[bfila, noOfCol - 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;

            //D

            formula_calculo = "=" + n.Cells[bfila, 8].Address + "*" + suma_contrucciond;
            n.Cells[bfila, noOfCol + 1].FormulaR1C1 = formula_calculo;

            n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

            string ADOD = "$" + n.Cells[bfila, noOfCol + 1].Address;


            n.Cells[bfila, noOfCol + 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;



            //
            bfila++;

            n.Cells[bfila, 4, bfila, 7].Merge = true;
            n.Cells[bfila, 4, bfila, 7].Value = "Imprevistos sobre Obra (%)";
            //n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 12, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
            n.Row(bfila).Height = 21;
            n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.Border.Top.Style =
            n.Cells[bfila, 4, bfila, 7].Style.Border.Left.Style =
            n.Cells[bfila, 4, bfila, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.Font.Bold = true;



            n.Cells[bfila, 8].Value = 0.03;
            n.Cells[bfila, 8].Style.WrapText = true;

            n.Cells[bfila, 8].Style.Border.Top.Style =
            n.Cells[bfila, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, 8].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            n.Cells[bfila, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 8].Style.Font.Bold = true;
            n.Cells[bfila, 8].Style.Numberformat.Format = "0.0%";

            //caculo
            formula_calculo = "=" + n.Cells[bfila, 8].Address + "*" + suma_contruccion;
            n.Cells[bfila, noOfCol].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol].Style.WrapText = true;


            string valorimprevistos_obra = n.Cells[bfila, noOfCol].Address;
            n.Cells[bfila, noOfCol].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol].Style.Font.Bold = true;
            //

            //I
            formula_calculo = "=" + n.Cells[bfila, 8].Address + "*" + suma_contruccioni;
            n.Cells[bfila, noOfCol - 1].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

            string IMOI = "$" + n.Cells[bfila, noOfCol - 1].Address;


            n.Cells[bfila, noOfCol - 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;

            //D

            formula_calculo = "=" + n.Cells[bfila, 8].Address + "*" + suma_contrucciond;
            n.Cells[bfila, noOfCol + 1].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                  "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

            string IMOD = "$" + n.Cells[bfila, noOfCol + 1].Address;


            n.Cells[bfila, noOfCol + 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;


            bfila++;

            n.Cells[bfila, 4, bfila, 7].Merge = true;
            n.Cells[bfila, 4, bfila, 7].Value = "Utilidad sobre Obra (%)";
            //n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 12, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
            n.Row(bfila).Height = 21;
            n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.Border.Top.Style =
            n.Cells[bfila, 4, bfila, 7].Style.Border.Left.Style =
            n.Cells[bfila, 4, bfila, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.Font.Bold = true;


            n.Cells[bfila, 8].Value = 0.12;
            n.Cells[bfila, 8].Style.WrapText = true;

            n.Cells[bfila, 8].Style.Border.Top.Style =
            n.Cells[bfila, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, 8].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            n.Cells[bfila, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 8].Style.Font.Bold = true;
            n.Cells[bfila, 8].Style.Numberformat.Format = "0.0%";


            //caculo
            formula_calculo = "=" + n.Cells[bfila, 8].Address + "*" + suma_contruccion;
            n.Cells[bfila, noOfCol].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol].Style.WrapText = true;



            string valor_utilidadObra = n.Cells[bfila, noOfCol].Address;
            n.Cells[bfila, noOfCol].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol].Style.Font.Bold = true;
            //


            //I
            formula_calculo = "=" + n.Cells[bfila, 8].Address + "*" + suma_contruccioni;
            n.Cells[bfila, noOfCol - 1].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

            string UTI = "$" + n.Cells[bfila, noOfCol - 1].Address;


            n.Cells[bfila, noOfCol - 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;

            //D

            formula_calculo = "=" + n.Cells[bfila, 8].Address + "*" + suma_contrucciond;
            n.Cells[bfila, noOfCol + 1].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

            string UTD = "$" + n.Cells[bfila, noOfCol + 1].Address;


            n.Cells[bfila, noOfCol + 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;


            //

            bfila++;

            n.Cells[bfila, 4, bfila, 7].Merge = true;
            n.Cells[bfila, 4, bfila, 7].Value = "Administracion sobre Procura Contratista (%)";
            //n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 12, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
            n.Row(bfila).Height = 21;
            n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.Border.Top.Style =
            n.Cells[bfila, 4, bfila, 7].Style.Border.Left.Style =
            n.Cells[bfila, 4, bfila, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.Font.Bold = true;


            n.Cells[bfila, 8].Value = 0.1;
            n.Cells[bfila, 8].Style.WrapText = true;

            n.Cells[bfila, 8].Style.Border.Top.Style =
            n.Cells[bfila, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, 8].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            n.Cells[bfila, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 8].Style.Font.Bold = true;
            n.Cells[bfila, 8].Style.Numberformat.Format = "0.0%";




            //caculo
            formula_calculo = "=" + n.Cells[bfila, 8].Address + "*" + suma_procura;
            n.Cells[bfila, noOfCol].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol].Style.WrapText = true;


            string valor_procura = n.Cells[bfila, noOfCol].Address;
            n.Cells[bfila, noOfCol].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol].Style.Font.Bold = true;

            //I
            formula_calculo = "=" + n.Cells[bfila, 8].Address + "*" + sumaprocurai;
            n.Cells[bfila, noOfCol - 1].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

            string APCI = "$" + n.Cells[bfila, noOfCol - 1].Address;


            n.Cells[bfila, noOfCol - 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;

            //D
            formula_calculo = "=" + n.Cells[bfila, 8].Address + "*" + sumaprocurad;
            n.Cells[bfila, noOfCol + 1].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

            string APCD = "$" + n.Cells[bfila, noOfCol + 1].Address;


            n.Cells[bfila, noOfCol + 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;

            //
            bfila++;


            n.Cells[bfila, 4, bfila, 7].Merge = true;
            n.Cells[bfila, 4, bfila, 7].Value = "Administracion sobre Reembolsables (%)";
            //n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 12, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
            n.Row(bfila).Height = 21;
            n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.Border.Top.Style =
            n.Cells[bfila, 4, bfila, 7].Style.Border.Left.Style =
            n.Cells[bfila, 4, bfila, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.Font.Bold = true;

            n.Cells[bfila, 8].Value = 0.1;
            n.Cells[bfila, 8].Style.WrapText = true;

            n.Cells[bfila, 8].Style.Border.Top.Style =
            n.Cells[bfila, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, 8].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            n.Cells[bfila, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 8].Style.Font.Bold = true;
            n.Cells[bfila, 8].Style.Numberformat.Format = "0.0%";


            //caculo
            formula_calculo = "=" + n.Cells[bfila, 8].Address + "*" + sumareembolsables;
            n.Cells[bfila, noOfCol].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol].Style.WrapText = true;


            string valor_reembolsables = n.Cells[bfila, noOfCol].Address;
            n.Cells[bfila, noOfCol].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol].Style.Font.Bold = true;

            //I

            formula_calculo = "=" + n.Cells[bfila, 8].Address + "*" + sumareembolsables;
            n.Cells[bfila, noOfCol - 1].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol - 1].Style.WrapText = true;


            string valor_reembolsablesi = n.Cells[bfila, noOfCol - 1].Address;
            n.Cells[bfila, noOfCol - 1].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol - 1].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol - 1].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;


            formula_calculo = "=" + n.Cells[bfila, 8].Address + "*" + sumareembolsables;
            n.Cells[bfila, noOfCol + 1].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 1].Style.WrapText = true;


            string valor_reembolsablesd = n.Cells[bfila, noOfCol + 1].Address;
            n.Cells[bfila, noOfCol + 1].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol + 1].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol + 1].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;
            //



            //envio de datos a administracion

            formula_calculo = "=SUM(" + valoradministracion_obra + ":" + valor_reembolsables + ")";
            n.Cells[administracion].Formula = formula_calculo;
            n.Cells[administracion].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[administracion].Style.WrapText = true;


            formula_calculo = "=SUM(" + ADOI + ":" + valor_reembolsablesi + ")";
            n.Cells[administracioni].Formula = formula_calculo;
            n.Cells[administracioni].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[administracioni].Style.WrapText = true;


            n.Cells[bfila, noOfCol - 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;

            formula_calculo = "=SUM(" + ADOD + ":" + valor_reembolsablesd + ")";
            n.Cells[administracionid].Formula = formula_calculo;
            n.Cells[administracionid].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[administracionid].Style.WrapText = true;



            n.Cells[bfila, noOfCol + 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;


            for (var i = 0; i <= 4; i++)
            {
                n.Row(bfila - i).OutlineLevel = 1;
                n.Row(bfila - i).Collapsed = true;
            }



            bfila = bfila + 2;




            n.Cells[bfila, 4, bfila, 7].Merge = true;
            n.Cells[bfila, 4, bfila, 7].Value = "TOTAL";
            n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
            n.Row(bfila).Height = 28;
            n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



            //TOTAL

            formula_calculo = "=TRUNC(SUM(" + sumaingenieria + "+" + suma_procura + "+" + suma_contruccion + "+" + sumareembolsables + "+" + administracion + "),3)";
            n.Cells[bfila, noOfCol].Formula = formula_calculo;
            n.Cells[bfila, noOfCol].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol].Style.WrapText = true;
            string valortotal = n.Cells[bfila, noOfCol].Address;
            n.Cells["A2"].FormulaR1C1 = formula_calculo;


            n.Cells[bfila, noOfCol].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol].Style.Font.Bold = true;
            n.Cells[bfila, noOfCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            n.Cells[bfila, noOfCol].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
            n.Cells[bfila, noOfCol].Style.Font.Color.SetColor(Color.White);
            n.Cells[bfila, noOfCol].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            //I
            formula_calculo = "=TRUNC(SUM(" + sumaingenieriai + "+" + sumaprocurai + "+" + suma_contruccioni + "+" + SRRI + "+" + administracioni + "),3)";
            n.Cells[bfila, noOfCol - 1].Formula = formula_calculo;
            n.Cells[bfila, noOfCol - 1].Style.Numberformat.Format =
             "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol - 1].Style.WrapText = true;

            string TI = "$" + n.Cells[bfila, noOfCol - 1].Address;


            n.Cells[bfila, noOfCol - 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol - 1].Style.Font.Bold = true;

            //D

            formula_calculo = "=TRUNC(SUM(" + sumaingenieriad + "+" + sumaprocurad + "+" + suma_contrucciond + "+" + SRRD + "+" + administracionid + "),3)";
            n.Cells[bfila, noOfCol + 1].Formula = formula_calculo;
            n.Cells[bfila, noOfCol + 1].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 1].Style.WrapText = true;

            string TD = n.Cells[bfila, noOfCol + 1].Address;


            n.Cells[bfila, noOfCol + 1].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            n.Cells[bfila, noOfCol + 1].Style.Font.Bold = true;




            bfila = bfila + 3;



            MontosTotalesRDO m = new MontosTotalesRDO();

            if (ultimoRdo != null && ultimoRdo.Id > 0)
            {
                var rdoDetalles = _drdoeacrepository.GetAll()
                                       .Where(x => x.vigente)
                                       .Where(x => x.RdoCabeceraId == ultimoRdo.Id)
                                       .ToList();



                if (rdoDetalles.Count > 0)
                {
                    m.costoBudget = Decimal.Round(
                                               (from x in rdoDetalles
                                                select Decimal.Round(x.costo_presupuesto, 8)).Sum()
                                                 , 8);

                    m.costoEAC = Decimal.Round(
                                              (from x in rdoDetalles

                                               select Decimal.Round(x.costo_eac, 8)).Sum()
                                               , 8);
                }
            }

            n.Cells[bfila, 4, bfila, 7].Merge = true;
            n.Cells[bfila, 4, bfila, 7].Value = "MONTO PRESUPUESTADO"; //"VALOR ACTUAL FACTURADO (VER DETALLE)";
            n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Row(bfila).Height = 28;
            n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;

            n.Cells[bfila, 4, bfila, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.Font.Bold = true;



            //VALOR

            n.Cells[bfila, 15, bfila, 17].Merge = true;


            n.Cells[bfila, 15, bfila, 17].Value = montoPresupuestoTotal;//"";
            n.Cells[bfila, 15, bfila, 17].Style.Font.Bold = true;
            n.Cells[bfila, 15, bfila, 17].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 15, bfila, 17].Style.Font.Color.SetColor(Color.White);
            n.Cells[bfila, 15, bfila, 17].Style.Numberformat.Format =
         "#,##0.00";



            n.Cells[bfila, 15, bfila, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
            n.Cells[bfila, 15, bfila, 17].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
            n.Cells[bfila, 15, bfila, 17].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            n.Cells[bfila, 15, bfila, 17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 15, bfila, 17].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            var montopresupuestado_rev = n.Cells[bfila, 15, bfila, 17].Address;

            bfila = bfila + 1;


            var fechaSegunRdo = " FECHA " + cabecera.fecha_corte.ToString("dd", CultureInfo.CreateSpecificCulture("es-Es")).ToUpper() + "/" + cabecera.fecha_corte.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-Es")).ToUpper() + "/" + cabecera.fecha_corte.Year;
            n.Cells[bfila, 4, bfila, 7].Merge = true;
            n.Cells[bfila, 4, bfila, 7].Value = "MONTO ESTIMADO A FINALIZACIÓN SEGÚN RDO" + fechaSegunRdo;//"AJUSTE CON SOPORTE DE CANTIDADES DE OBRA ";
            n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Row(bfila).Height = 28;
            n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;

            n.Cells[bfila, 4, bfila, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.Font.Bold = true;


            //VALOR


            n.Cells[bfila, 15, bfila, 17].Merge = true;
            n.Cells[bfila, 15, bfila, 17].Value = m.costoEAC;// "";
            n.Cells[bfila, 15, bfila, 17].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 15, bfila, 17].Style.Font.Color.SetColor(Color.White);
            n.Cells[bfila, 15, bfila, 17].Style.Font.Bold = true;
            n.Cells[bfila, 15, bfila, 17].Style.Numberformat.Format =
    "#,##0.00";
            n.Cells[bfila, 15, bfila, 17].Style.WrapText = true;

            n.Cells[bfila, 15, bfila, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
            n.Cells[bfila, 15, bfila, 17].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
            n.Cells[bfila, 15, bfila, 17].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            n.Cells[bfila, 15, bfila, 17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 15, bfila, 17].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            bfila = bfila + 1;


            n.Cells[bfila, 4, bfila, 7].Merge = true;

            n.Cells[bfila, 4, bfila, 7].Value = "MONTO PO EMITIDA POR SHAYA";//"TOTAL A CERTIFICAR";
            n.Cells[bfila, 4, bfila, 7].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 4, bfila, 7].Style.WrapText = true;
            n.Row(bfila).Height = 28;
            n.Row(bfila).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));

            n.Cells[bfila, 4, bfila, 7].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            n.Cells[bfila, 4, bfila, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4, bfila, 7].Style.Font.Bold = true;


            //VALOR


            n.Cells[bfila, 15, bfila, 17].Merge = true;
            n.Cells[bfila, 15, bfila, 17].Value = montopo;// "";
            n.Cells[bfila, 15, bfila, 17].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            n.Cells[bfila, 15, bfila, 17].Style.Font.Color.SetColor(Color.White);
            n.Cells[bfila, 15, bfila, 17].Style.Font.Bold = true;
            n.Cells[bfila, 15, bfila, 17].Style.Numberformat.Format =
            "#,##0.00";
            n.Cells[bfila, 15, bfila, 17].Style.WrapText = true;

            n.Cells[bfila, 15, bfila, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
            n.Cells[bfila, 15, bfila, 17].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
            n.Cells[bfila, 15, bfila, 17].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            n.Cells[bfila, 15, bfila, 17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 15, bfila, 17].Style.VerticalAlignment = ExcelVerticalAlignment.Center;




            var foot = hoja.Dimension.End.Row + 2;
            string celdaVariacion = "C" + foot + ":G" + foot;
            hoja.Cells[celdaVariacion].Merge = true;
            hoja.Cells[celdaVariacion].Value = "VARIACIONES AL CERTIFICADO MENSUAL";
            hoja.Cells[celdaVariacion].Style.WrapText = true;
            hoja.Cells[celdaVariacion].Style.Font.SetFromFont(new Font("Arial", 26, FontStyle.Bold));
            hoja.Cells[celdaVariacion].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[celdaVariacion].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celdaVariacion].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells[celdaVariacion].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celdaVariacion].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
            hoja.Cells[celdaVariacion].Style.Font.Color.SetColor(Color.White);
            hoja.Row(foot).Height = 30;
            foot = foot + 2;
            int rangoinicialtotales = foot;
            string celldetalle = "C" + foot + ":G" + foot;
            hoja.Row(foot).Height = 27;
            hoja.Cells["C" + foot + ":D" + foot].Merge = true;
            hoja.Cells["C" + foot + ":D" + foot].Value = "ITEM";
            hoja.Cells["C" + foot + ":D" + foot].Style.WrapText = true;
            hoja.Cells["C" + foot + ":D" + foot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["C" + foot + ":D" + foot].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            hoja.Cells["E" + foot + ":F" + foot].Merge = true;
            hoja.Cells["E" + foot + ":F" + foot].Value = "DESCRIPCIÓN";
            hoja.Cells["E" + foot + ":F" + foot].Style.WrapText = true;
            hoja.Cells["E" + foot + ":F" + foot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["E" + foot + ":F" + foot].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));

            hoja.Cells["G" + foot].Style.WrapText = true;
            hoja.Cells["G" + foot].Value = "MONTO TOTAL";
            hoja.Cells["G" + foot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["G" + foot].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));

            for (int i = foot; i <= foot + 5; i++)
            {
                hoja.Row(i).Height = 27;
                hoja.Cells["C" + i + ":D" + i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                hoja.Cells["E" + i + ":F" + i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                hoja.Cells["G" + i].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            }
            foot = foot + 7;

            string cell = "E" + foot + ":F" + foot;


            hoja.Row(foot).Height = 42;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "MONTO TOTAL DE VARIACIONES AL CERTIFICADO MENSUAL";
            hoja.Cells[cell].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            hoja.Cells[cell].Style.Font.Italic = true;
            hoja.Cells[cell].Style.WrapText = true;
            //hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            hoja.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            hoja.Cells["G" + foot].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            foot = foot + 1;
            hoja.Row(foot).Height = 10;
            foot = foot + 1;

            cell = "E" + foot + ":F" + foot;
            string celldetotal = "E" + foot + ":F" + foot;
            hoja.Row(foot).Height = 42;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "MONTO TOTAL A CERTIFICAR";
            hoja.Cells[cell].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            hoja.Cells[cell].Style.Font.Italic = true;
            hoja.Cells[cell].Style.WrapText = true;
            //hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            hoja.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["G" + foot].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            /*
                        hoja.Cells[celdaVariacion].Merge = true;
                        hoja.Cells[celdaVariacion].Value = "DETALLE VALOR FACTURADO";
                        hoja.Cells[celdaVariacion].Style.WrapText = true;
                        hoja.Cells[celdaVariacion].Style.Font.SetFromFont(new Font("Arial", 26, FontStyle.Bold));
                        hoja.Cells[celdaVariacion].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja.Cells[celdaVariacion].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja.Cells[celdaVariacion].Style.Border.BorderAround(ExcelBorderStyle.Medium);
                        hoja.Cells[celdaVariacion].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja.Cells[celdaVariacion].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 112, 192));
                        hoja.Cells[celdaVariacion].Style.Font.Color.SetColor(Color.White);
                        hoja.Row(foot).Height = 30;
                        foot = foot + 2;
                        int rangoinicialtotales = foot;
                        string celldetalle = "C" + foot + ":G" + foot;
                        hoja.Row(foot).Height = 27;
                        hoja.Cells["C" + foot + ":D" + foot].Merge = true;
                        hoja.Cells["C" + foot + ":D" + foot].Value = "AÑO";
                        hoja.Cells["C" + foot + ":D" + foot].Style.WrapText = true;
                        hoja.Cells["C" + foot + ":D" + foot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja.Cells["C" + foot + ":D" + foot].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                        hoja.Cells["C" + foot + ":D" + foot].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja.Cells["C" + foot + ":D" + foot].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217,217,217));

                        hoja.Cells["E" + foot].Merge = true;
                        hoja.Cells["E" + foot].Value = "MES";
                        hoja.Cells["E" + foot].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja.Cells["E" + foot].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));

                        hoja.Cells["E" + foot + ":F" + foot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja.Cells["E" + foot + ":F" + foot].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));

                        hoja.Cells["F" + foot].Value = "# FACTURA";
                        hoja.Cells["F" + foot].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja.Cells["F" + foot].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));

                        hoja.Cells["E" + foot + ":F" + foot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja.Cells["E" + foot + ":F" + foot].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));

                        hoja.Cells["G" + foot].Style.WrapText = true;
                        hoja.Cells["G" + foot].Value = "MONTO FACTURADO";
                        hoja.Cells["G" + foot].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja.Cells["G" + foot].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));

                        hoja.Cells["G" + foot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja.Cells["G" + foot].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));

                        for (int i = foot; i <= foot + 5; i++)
                        {
                            hoja.Row(i).Height = 27;
                            hoja.Cells["C" + i + ":D" + i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            hoja.Cells["E" + i ].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            hoja.Cells["F" + i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            hoja.Cells["G" + i].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        }
                        foot = foot + 7;

                        string cell = "E" + foot + ":F" + foot;

                        string celldetotal = "E" + foot + ":F" + foot;
                        hoja.Row(foot).Height = 42;
                        hoja.Cells[cell].Merge = true;
                        hoja.Cells[cell].Value = "TOTAL FACTURADO";
                        hoja.Cells[cell].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));

                        hoja.Cells[cell].Style.WrapText = true;
                        hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.None);
                        hoja.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        hoja.Cells["G" + foot].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        */

            foot = foot + 2;
            var linefoot = foot;

            foot = foot + 1;
            hoja.Row(foot).Height = 38;
            foot = foot + 1;
            hoja.Row(foot).Height = 42;
            foot = foot + 1;
            hoja.Row(foot).Height = 110;
            foot = foot + 1;
            hoja.Row(foot).Height = 42;
            var finalfoot = foot;






            string cellfoot = "C" + linefoot + ":Q" + finalfoot;
            hoja.Cells[cellfoot].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            /*cellfoot = "C" + rangoinicialtotales + ":Q" + finalfoot;
           hoja.Cells[cellfoot].Style.Border.BorderAround(ExcelBorderStyle.Thin);
           hoja.Cells[cellfoot].Style.Fill.PatternType = ExcelFillStyle.Solid;
           hoja.Cells[cellfoot].Style.Fill.BackgroundColor.SetColor(Color.White);*/

            /*hoja.Cells[celldetalle].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celldetalle].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
            hoja.Cells[celldetotal].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celldetotal].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));*/

            foot = foot + 1;
            hoja.Row(foot).Height = 42;


            string celdaFirma = "C" + foot + ":J" + foot;
            hoja.Cells[celdaFirma].Merge = true;
            hoja.Cells[celdaFirma].Value = "CPP";
            hoja.Cells[celdaFirma].Style.WrapText = true;
            hoja.Cells[celdaFirma].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[celdaFirma].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celdaFirma].Style.Font.SetFromFont(fuenteR);
            hoja.Cells[celdaFirma].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[celdaFirma].Style.Font.SetFromFont(new Font("Arial", 26, FontStyle.Bold));

            celdaFirma = "K" + foot + ":Q" + foot;
            hoja.Cells[celdaFirma].Merge = true;
            hoja.Cells[celdaFirma].Value = "SHAYA";
            hoja.Cells[celdaFirma].Style.WrapText = true;
            hoja.Cells[celdaFirma].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[celdaFirma].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celdaFirma].Style.Font.SetFromFont(fuenteR);
            hoja.Cells[celdaFirma].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[celdaFirma].Style.Font.SetFromFont(new Font("Arial", 26, FontStyle.Bold));




            celdaCabecera = "C1:Q1";
            hoja.Cells[celdaCabecera].Style.Border.Left.Style = ExcelBorderStyle.Medium;
            hoja.Cells[celdaCabecera].Style.Border.Left.Color.SetColor(Color.Black);
            hoja.Cells[celdaCabecera].Style.Border.Top.Style = ExcelBorderStyle.Medium;
            hoja.Cells[celdaCabecera].Style.Border.Left.Color.SetColor(Color.Black);
            hoja.Cells[celdaCabecera].Style.Border.Right.Style = ExcelBorderStyle.Medium;
            hoja.Cells[celdaCabecera].Style.Border.Left.Color.SetColor(Color.Black);

            hoja.Cells["C2" + ":" + "C" + hoja.Dimension.End.Row].Style.Border.Left.Style = ExcelBorderStyle.Medium;

            hoja.Cells["C2" + ":" + "C" + hoja.Dimension.End.Row].Style.Border.Left.Color.SetColor(Color.Black);

            hoja.Cells["Q2" + ":" + "Q" + hoja.Dimension.End.Row].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            hoja.Cells["Q2" + ":" + "Q" + hoja.Dimension.End.Row].Style.Border.Right.Color.SetColor(Color.Black);


            hoja.Cells["C" + hoja.Dimension.End.Row + ":" + "Q" + hoja.Dimension.End.Row].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

            hoja.Cells["C" + hoja.Dimension.End.Row + ":" + "Q" + hoja.Dimension.End.Row].Style.Border.Bottom.Color.SetColor(Color.Black);




            //FORMATO A UNA PAGINA
            hoja.View.PageBreakView = true;
            hoja.PrinterSettings.PrintArea = hoja.Cells[1, 3, hoja.Dimension.End.Row, hoja.Dimension.End.Column - 1];

            hoja.PrinterSettings.Orientation = eOrientation.Landscape;

            /*hoja.PrinterSettings.RepeatRows = hoja.Cells["3:4"];
            hoja.PrinterSettings.RepeatColumns = hoja.Cells["C:O"];*/

            hoja.PrinterSettings.FitToPage = true;
            hoja.PrinterSettings.FitToWidth = 1;
            hoja.PrinterSettings.FitToHeight = 0;
            hoja.Cells[1, 1, hoja.Dimension.End.Row, 2].Style.Font.Color.SetColor(Color.FromArgb(159, 159, 159));



            string pathpetroamazonas = System.Web.HttpContext.Current.Server.MapPath("~/Views/LogosCPP/_petroamazonas.png");
            string patharbolecuador = System.Web.HttpContext.Current.Server.MapPath("~/Views/LogosCPP/_arbolecuador.png");
            string pathcpp = System.Web.HttpContext.Current.Server.MapPath("~/Views/LogosCPP/_cpp.png");
            //string pathpetroEcuador = System.Web.HttpContext.Current.Server.MapPath("~/Views/LogosCPP/_petroecuador2.png");



            /*if (proyecto.usar_logo_prederminado)
            {
                if (File.Exists((string)pathpetroamazonas))
                {
                    Image _logopretro = Image.FromFile(pathpetroamazonas);
                    var picture = hoja.Drawings.AddPicture("pretroamazonas", _logopretro);
                    picture.SetPosition(3, 0, 3, 0);
                }
            }
            else
            {
                if (File.Exists((string)pathpetroEcuador))
                {
                    Image _logopetroEcuador = Image.FromFile(pathpetroEcuador);
                    var picture = hoja.Drawings.AddPicture("pretroecuador", _logopetroEcuador);
                    picture.SetSize(400, 135);
                    picture.SetPosition(3, 8, 2, 50);

                }
            }
            */
            if (File.Exists((string)patharbolecuador))
            {
                Image _logoarbol = Image.FromFile(patharbolecuador);
                var picture = hoja.Drawings.AddPicture("arbolecuad", _logoarbol);
                picture.SetPosition(0, 20, 2, 70);
            }
            if (File.Exists((string)pathcpp))
            {
                Image _logocpp = Image.FromFile(pathcpp);
                var picture = hoja.Drawings.AddPicture("cpp", _logocpp);
                picture.SetPosition(0, 20, 13, 60);

            }


            //cert.monto_certificado = MontoTotalSinAIU;
            //cert.monto_pendiente = MontoTotalAIU;
            //Repository.Update(cert);


            // 


            //Es Ultimas modificaciones
            hoja.DeleteColumn(9);
            hoja.DeleteColumn(10);


            hoja.Column(3).Width = 16;//15.29
            hoja.Column(4).Width = 5.46;//4.71
            hoja.Column(5).Width = 47.7;// 47
            hoja.Column(6).Width = 47.7;// 47
            hoja.Column(7).Width = 47.7;// 47
            hoja.Column(8).Width = 18;// 17.29
            hoja.Column(9).Width = 25;// 24.29
            hoja.Column(10).Width = 38;//28
            hoja.Column(11).Width = 28.7;//28
            hoja.Column(12).Width = 28.7;//28
            hoja.Column(13).Width = 25;// 22.29
            hoja.Column(14).Width = 25;// 23.29
            hoja.Column(15).Width = 25;// 23.29

            hoja.Column(1).Hidden = true;

            hoja.Row(9).Height = 26.25;// 23.29
            hoja.Row(9).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            hoja.Row(10).Height = 47;// 23.29
            hoja.Row(10).Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));

            hoja.Row(11).Height = 12;// 23.29

            hoja.InsertRow(9, 1);
            celdaCabecera = "C9:O9";
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells[celdaCabecera].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celdaCabecera].Style.Fill.BackgroundColor.SetColor(Color.White);
            hoja.Row(9).Height = 42;// 23.29

            //Border Cabecera C1:D4

            celdaCabecera = "C1:D4";
            hoja.Cells[celdaCabecera].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            hoja.Column(13).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Column(14).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Column(15).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            return excel;
        }


        public string nombrecatalogo(int tipocatagoid)
        {
            var a = _repositorycatalogo.Get(tipocatagoid);
            if (a != null && a.Id > 0)
            {
                return a.nombre;
            }
            else
            {
                return "";
            }

        }

        public decimal obtenerprecio_unitario(int item_id)
        {
            var a = _repositorydetallepreciario.GetAll().Where(c => c.vigente)
                                                        .Where(c => c.ItemId == item_id)
                                                        .FirstOrDefault();
            if (a != null && a.Id > 0)
            {
                return a.precio_unitario;
            }
            else
            {
                return 0;
            }

        }




        public List<Contrato> GetListContratos()
        {
            var query = _contratorepository.GetAll().Where(c => c.vigente).ToList();
            return query;
        }

        public List<Proyecto> GetListProyecto(int ContratoId)
        {
            var query = _proyectorepository.GetAll().Where(c => c.vigente).Where(c => c.contratoId == ContratoId).ToList();
            return query;
        }

        public Certificado GetDetalleCertificado(int Id)
        {
            return Repository.GetAllIncluding(c => c.Proyecto.Contrato).Where(c => c.Id == Id).FirstOrDefault();
        }

        public bool desaprobar(int id, string pass)
        {
            var pass_param = _parametrorepository.GetAll().Where(c => c.Codigo == "BAJA.CERT").Select(c => c.Valor).FirstOrDefault();

            if (pass_param == pass)
            {
                var entity = Repository.Get(id);
                entity.estado_actual = 0;
                Repository.Update(entity);
                return true;

            }
            else
            {
                return false;
            }
        }

        public MontosCabecerasCertificado ObtenerMontosCertificadosCabeceras(int Id, int proyectoid)
        {
            var montosTotales = new MontosCabecerasCertificado();
            var cabecera = Repository.GetAll().Where(x => x.Id == Id).FirstOrDefault();
            var cert = Repository.Get(Id);
            RdoCabecera ultimoRdo = null;
            var fechaCorte = cabecera.fecha_corte.Date;

            decimal montoTotalSinAIU = Convert.ToDecimal(0);

            var items = _itemservice.ArbolItemsCertificadoSinPendientes(proyectoid, fechaCorte).Where(c => c.para_oferta).Where(c => c.GrupoId == 2).ToList(); //items contrato y fecha Sin Pendientes de AProbacion
            var itemsPendientes = _itemservice.ArbolItemsCertificadoPendientesAprobacion(proyectoid, fechaCorte).Where(c => c.para_oferta).Where(c => c.GrupoId == 2).ToList(); //items contrato y fecha Sin Pendientes de AProbacion



            var contruccion = _dobrarepository.GetAllIncluding(x => x.AvanceObra.Oferta, x => x.Computo.Item.Grupo)
                                              .Where(x => x.vigente)
                                              .Where(x => x.Computo.vigente)
                                              .Where(x => x.AvanceObra.vigente)
                                              .Where(x => x.AvanceObra.aprobado)
                                              .Where(x => x.AvanceObra.Oferta.es_final)
                                              .Where(x => x.AvanceObra.Oferta.vigente)
                                              .Where(x => x.AvanceObra.Oferta.ProyectoId == proyectoid)
                                              .Where(x => x.AvanceObra.fecha_presentacion <= fechaCorte)
                                              .ToList();


            var certificado_anterior = Repository.GetAll().Where(x => x.ProyectoId == proyectoid)
                                                        .Where(x => x.estado_actual == 1)//Aprobado
                                                        .Where(x => x.vigente)
                                                        .Where(x => x.fecha_corte < cabecera.fecha_corte)
                                                        .Where(x => x.Id != cabecera.Id)
                                                        .OrderByDescending(x => x.fecha_corte)
                                                        .FirstOrDefault();


            var rdo = _rdo_cabecera.GetAll().Where(r => r.es_definitivo)
                                           .Where(r => r.estado)
                                           .Where(r => r.vigente)
                                           .Where(r => r.fecha_rdo <= fechaCorte)
                                           .Where(r => r.ProyectoId == proyectoid)
                                           .OrderByDescending(r => r.fecha_rdo).ToList()
                                           .FirstOrDefault();

            var detallesConstruccionUltimoRdo = new List<RdoDetalleEac>();
            if (rdo != null && rdo.Id > 0)
            {
                detallesConstruccionUltimoRdo = _rdo_detalle_EAC.GetAllIncluding(d => d.Computo.Item)
                                                 .Where(d => d.RdoCabeceraId == rdo.Id)
                                                 .ToList();
            }

            var cantidadsinaui = Convert.ToDecimal(0);
            foreach (var pitem in items)
            {

                if (rdo != null && rdo.Id > 0)
                {
                    ultimoRdo = rdo;

                    if (contruccion.Count > 0)
                    {

                        int itemid = pitem.Id;

                        var cantidadanterior = Convert.ToDecimal(0);

                        if (certificado_anterior != null && certificado_anterior.Id > 0)
                        {
                            cantidadanterior = Decimal.Round((from e in contruccion
                                                              where e.Computo.Item.GrupoId == 2
                                                              where e.Computo.Item.Id == itemid
                                                              where e.AvanceObra.fecha_presentacion <= certificado_anterior.fecha_corte
                                                              select Decimal.Round(e.cantidad_diaria * e.precio_unitario, 8)).Sum(), 8);


                        }

                        /* ACUMULADO SEGUN AVANCE DE OBRA
                        var tempanterior = Decimal.Round((from e in contruccion
                                                          where e.Computo.Item.GrupoId == 2
                                                          where e.Computo.Item.Id == itemid
                                                          where e.AvanceObra.fecha_presentacion < rdo.fecha_rdo
                                                          select Decimal.Round(e.cantidad_diaria * e.precio_unitario, 8)
                            ).Sum(), 8);
                        var tempactual = Decimal.Round((from e in contruccion
                                                        where e.Computo.Item.GrupoId == 2
                                                        where e.Computo.Item.Id == itemid
                                                        //where e.Computo.Item.PendienteAprobacion == false
                                                        where e.AvanceObra.fecha_presentacion == rdo.fecha_rdo
                                                        select Decimal.Round(e.cantidad_diaria * e.precio_unitario, 8)
                                  ).Sum(), 8);
                         
                        var totalacumulada = tempanterior + tempactual;         
                         */

                        /*ACUMULADO SEGUN ULTIMO RDO TODOS LOS DETALLES*/
                        var totalacumulada = Decimal.Round((from e in detallesConstruccionUltimoRdo
                                                            where e.Computo.Item.GrupoId == 2
                                                            where e.Computo.Item.Id == itemid
                                                            //where e.Computo.Item.PendienteAprobacion == false
                                                            select Decimal.Round(e.cantidad_acumulada * e.precio_unitario, 8)
                 ).Sum(), 8);


                        decimal periodoactual = Decimal.Round(totalacumulada - cantidadanterior, 8);

                        cantidadsinaui = periodoactual + cantidadsinaui;




                    }
                }

            }


            if (itemsPendientes.Count > 0)
            {
                foreach (var pitem in itemsPendientes)
                {
                    if (ultimoRdo != null && ultimoRdo.Id > 0)
                    {
                        if (contruccion.Count > 0)
                        {
                            int itemid = pitem.Id;

                            var cantidadanterior = Convert.ToDecimal(0);

                            if (certificado_anterior != null && certificado_anterior.Id > 0)
                            {
                                cantidadanterior = Decimal.Round((from e in contruccion
                                                                  where e.Computo.Item.GrupoId == 2
                                                                  where e.Computo.Item.Id == itemid
                                                                  where e.AvanceObra.fecha_presentacion <= certificado_anterior.fecha_corte
                                                                  select Decimal.Round(e.cantidad_diaria * e.precio_unitario, 8)).Sum(), 8);
                            }

                            var tempanterior = Decimal.Round((from e in contruccion
                                                              where e.Computo.Item.GrupoId == 2
                                                              where e.Computo.Item.Id == itemid
                                                              where e.AvanceObra.fecha_presentacion < ultimoRdo.fecha_rdo
                                                              select Decimal.Round(e.cantidad_diaria * e.precio_unitario, 8)).Sum(), 8);
                            var tempactual = Decimal.Round((from e in contruccion
                                                            where e.Computo.Item.GrupoId == 2
                                                            where e.Computo.Item.Id == itemid
                                                            where e.AvanceObra.fecha_presentacion == ultimoRdo.fecha_rdo
                                                            select Decimal.Round(e.cantidad_diaria * e.precio_unitario, 8)
                                      ).Sum(), 8);
                            var totalacumulada = tempanterior + tempactual;

                            decimal periodoactual = Decimal.Round(totalacumulada - cantidadanterior, 8);
                            cantidadsinaui = periodoactual + cantidadsinaui;


                        }
                    }

                }


            }


            montosTotales.montoTotalsinAIU = cantidadsinaui;

            decimal Administracion_sobre_Obra = Decimal.Round(cantidadsinaui * Convert.ToDecimal(0.4119), 8);

            decimal Imprevistos_sobre_Obra = Decimal.Round(cantidadsinaui * Convert.ToDecimal(0.03), 8);

            decimal Utilidad_sobre_Obra = Decimal.Round(cantidadsinaui * Convert.ToDecimal(0.12), 8);

            montosTotales.montoTotalAIU = Decimal.Round(cantidadsinaui + Administracion_sobre_Obra + Imprevistos_sobre_Obra + Utilidad_sobre_Obra, 8);

            return montosTotales;
        }

        public string NombreCertificado(int Id, int proyectoId)
        {
            var cabecera = Repository.GetAll().Where(x => x.Id == Id).FirstOrDefault();
            var proyecto = _proyectorepository.GetAllIncluding(c => c.Contrato).Where(c => c.Id == proyectoId).FirstOrDefault();
            int planilla = Repository.GetAll().Where(x => x.ProyectoId == cabecera.ProyectoId)
                                          .Where(x => x.estado_actual == 1)
                                          .Where(x => x.vigente)
                                          .Where(x => x.fecha_corte < cabecera.fecha_corte)
                                          .ToList().Count + 1;

            string numCertificado = proyecto.codigo_reporte_certificacion + "-CRT" + (proyecto.Contrato.Formato == FormatoContrato.Contrato_2016 ? "-16-" : "-") + cabecera.fecha_corte.ToString("yy") + "" + cabecera.fecha_corte.ToString("MM") + "" + cabecera.fecha_corte.ToString("dd");
            return numCertificado;
        }
    }
}


using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using OfficeOpenXml.ConditionalFormatting;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class RsoCabeceraAsyncBaseCrudAppService : AsyncBaseCrudAppService<RsoCabecera, Dto.RsoCabeceraDto, PagedAndFilteredResultRequestDto>, IRsoCabeceraAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<RsoDetalleEac> _detalleRepository;
        private readonly IBaseRepository<Wbs> _wbsRepository;
        private readonly IBaseRepository<Computo> _compuRepository;
        private readonly IBaseRepository<Oferta> _ofertaRepository;
        private readonly IBaseRepository<RsoDetalleEac> _eacRepository;
        private readonly IBaseRepository<AvanceObra> _avanceRepository;
        private readonly IBaseRepository<DetalleAvanceObra> _davanceRepository;
        private readonly IBaseRepository<Proyecto> _proyectoRepository;
        private readonly IBaseRepository<ProyectoObservacion> _proyectoObservacionRepository;
        private readonly IBaseRepository<ObraDisruptivo> _obraDisruptivoRepository;
        private readonly IBaseRepository<Requerimiento> _requerimientoRepository;
        private readonly IBaseRepository<CurvasProyecto> _curvaRepository;
        private readonly IBaseRepository<CurvaProyectoRSO> _rsocurvaRepository;
        private readonly IBaseRepository<Precipitacion> _precipitacionRepository;
        private readonly IBaseRepository<Contrato> _contratoRepository;
        private readonly IBaseRepository<RdoCabecera> _rdocabeceraRepository;
        private readonly IBaseRepository<RdoDetalleEac> _eacRDORepository;
        //Archivos avance obra
        private readonly IBaseRepository<Archivo> _archivoRepository;
        private readonly IBaseRepository<ArchivosAvanceObra> _archivoAvanceObraRepository;
        private readonly IdentityEmailMessageService _correoservice;
        private readonly IBaseRepository<CorreoLista> _correslistarepository;
        private readonly IBaseRepository<Catalogo> _catalogorepository;

        public RsoCabeceraAsyncBaseCrudAppService(
            IBaseRepository<RsoCabecera> repository,
            IBaseRepository<RsoDetalleEac> detalleRepository,
            IBaseRepository<Wbs> wbsRepository,
            IBaseRepository<Computo> compuRepository,
            IBaseRepository<Oferta> ofertaRepository,
            IBaseRepository<RsoDetalleEac> eacRepository,
            IBaseRepository<AvanceObra> avanceRepository,
            IBaseRepository<DetalleAvanceObra> davanceRepository,
        IBaseRepository<Proyecto> proyectoRepository,
            IBaseRepository<ProyectoObservacion> proyectoObservacionRepository,
            IBaseRepository<ObraDisruptivo> obraDisruptivoRepository,
              IBaseRepository<CurvasProyecto> curvaRepository,
                          IBaseRepository<Archivo> archivoRepository,
            IBaseRepository<ArchivosAvanceObra> archivoAvanceObraRepository,
            IBaseRepository<Precipitacion> precipitacionRepository,
            IdentityEmailMessageService correoservice,
            IBaseRepository<CorreoLista> correslistarepository,
            IBaseRepository<Contrato> contratoRepository,
            IBaseRepository<Catalogo> catalogorepository,        //Curva

        IBaseRepository<Requerimiento> requerimientoRepository,
        IBaseRepository<RdoCabecera> rdocabeceraRepository,
        IBaseRepository<CurvaProyectoRSO> rsocurvaRepository,
        IBaseRepository<RdoDetalleEac> eacRDORepository
        ) : base(repository)
        {
            _detalleRepository = detalleRepository;
            _wbsRepository = wbsRepository;
            _compuRepository = compuRepository;
            _ofertaRepository = ofertaRepository;
            _eacRepository = eacRepository;
            _avanceRepository = avanceRepository;
            _proyectoRepository = proyectoRepository;
            _proyectoObservacionRepository = proyectoObservacionRepository;
            _obraDisruptivoRepository = obraDisruptivoRepository;
            _requerimientoRepository = requerimientoRepository;
            _curvaRepository = curvaRepository;
            _davanceRepository = davanceRepository;
            _archivoRepository = archivoRepository;
            _archivoAvanceObraRepository = archivoAvanceObraRepository;
            _precipitacionRepository = precipitacionRepository;
            _correoservice = correoservice;
            _correslistarepository = correslistarepository;
            _contratoRepository = contratoRepository;
            _catalogorepository = catalogorepository;
            _rdocabeceraRepository = rdocabeceraRepository;
            _rsocurvaRepository = rsocurvaRepository;
            _eacRDORepository = eacRDORepository;
        }

        public RsoCabecera GetDetalles(int RdoCabeceraId)
        {
            var rdo = Repository.Get(RdoCabeceraId);
            return rdo;
        }

        public List<RsoCabecera> GetRdoCabeceras(int ProyectoId)
        {
            var rdoQuery = Repository.GetAllIncluding(c => c.Proyecto, c => c.Proyecto.Contrato, c => c.Proyecto.Contrato.Cliente)
                .Where(e => e.vigente == true)
                .Where(c => c.ProyectoId == ProyectoId)
                .OrderByDescending(o => o.es_definitivo)
                .ThenByDescending(o => o.fecha_rdo)
                .ToList();
            return rdoQuery;
        }

        public List<RsoCabeceraDto> GetRdoCabecerasTable(int ProyectoId)
        {
            var rdoQuery = Repository.GetAllIncluding(c => c.Proyecto, c => c.Proyecto.Contrato, c => c.Proyecto.Contrato.Cliente)
                .Where(e => e.vigente == true)
                .Where(c => c.ProyectoId == ProyectoId)
                .OrderByDescending(o => o.es_definitivo)
                .ThenByDescending(o => o.fecha_rdo)
                .ToList();

            var lista = (from r in rdoQuery
                         select new RsoCabeceraDto
                         {
                             Id = r.Id,
                             es_definitivo = r.es_definitivo,
                             codigo_rdo = r.codigo_rdo,
                             estado = r.estado,
                             emitido = r.emitido,
                             fecha_envio = r.fecha_envio,
                             fecha_rdo = r.fecha_rdo,
                             ProyectoId = r.ProyectoId,
                             observacion = r.observacion,
                             version = r.version,
                             vigente = r.vigente,
                             FormatEstado = r.es_definitivo ? "SI" : "NO",
                             Proyecto = r.Proyecto

                         }).ToList();

            return lista;
        }

        public async Task EmitirRdoAsync(int RdoCabeceraId)
        {
            var cabecera = Repository.Get(RdoCabeceraId);
            cabecera.emitido = true;
            cabecera.fecha_envio = DateTime.Now;
            Repository.Update(cabecera);

            var avances = _avanceRepository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.Oferta.ProyectoId == cabecera.ProyectoId)
                .Where(o => o.fecha_presentacion == cabecera.fecha_rdo).ToList();

            foreach (var a in avances)
            {
                a.emitido = true;
                _avanceRepository.Update(a);
            }


            var correos = _correslistarepository.GetAll().Where(c => c.vigente)
                                                        .Where(c => c.ListaDistribucion.vigente)
                                                        .Where(c => c.ListaDistribucion.nombre == CatalogosCodigos.DEFAULT_LISTADISTRIBUCION_RDO).ToList();
            if (correos.Count > 0)
            {



                /* ES: Envio de Excel al Correo*/
                MailMessage message = new MailMessage();
                message.Subject = "PMDIS: Emisión de RDO " + cabecera.Proyecto.codigo + "-" + cabecera.fecha_rdo.Date;


                foreach (var item in correos)
                {
                    message.To.Add(item.correo);
                    ElmahExtension.LogToElmah(new Exception("Send Emited RDO to: " + item.correo));
                }
                Random a = new Random();
                var valor = a.Next(1, 1000);
                ExcelPackage excel_rdo = this.MontoTotales(cabecera.Id, "EAC");

                excel_rdo.SaveAs(new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosProyectos/RdoEmitidos/RDO-" + cabecera.Proyecto.codigo + "-" + cabecera.fecha_rdo.Day + "_" + cabecera.fecha_rdo.Month + "_" + cabecera.fecha_rdo.Year + "_" + valor + ".xlsx")));

                string url = System.Web.HttpContext.Current.Server.MapPath("~/Views/ArchivosProyectos/RdoEmitidos/RDO-" + cabecera.Proyecto.codigo + "-" + cabecera.fecha_rdo.Day + "_" + cabecera.fecha_rdo.Month + "_" + cabecera.fecha_rdo.Year + "_" + valor + ".xlsx");
                if (File.Exists((string)url))
                {
                    message.Attachments.Add(new Attachment(url));
                }
                await _correoservice.SendWithFilesAsync(message);
                /*********/



            }
        }
        public int CreateRdoCabecera(int ProyectoId, DateTime fecha, int Id)
        {

            var rdoAntiguo = Repository
                .GetAll()
                .Where(o => o.vigente)
                .Where(o => o.ProyectoId == ProyectoId)
                .Where(o => o.fecha_rdo == fecha)
                .FirstOrDefault(o => o.es_definitivo);

            if (rdoAntiguo != null)
            {
                var version = rdoAntiguo.version.ToCharArray()[0];
                version++;

                rdoAntiguo.es_definitivo = false;
                Repository.Update(rdoAntiguo);

                var nuevoRdo = new RsoCabecera()
                {
                    vigente = true,
                    ProyectoId = ProyectoId,
                    es_definitivo = true,
                    codigo_rdo = "Pendiente",
                    estado = true,
                    fecha_rdo = fecha,
                    version = version + "",

                };
                if (Id > 0)
                {
                    var rdoAnterior = this.GetDetalles(Id);
                    if (rdoAnterior != null)
                    {
                        nuevoRdo.fecha_inicio = rdoAnterior.fecha_rdo.AddDays(1);
                    }
                }
                var id = Repository.InsertAndGetId(nuevoRdo);
                return id;
            }
            else
            {
                var nuevoRdo = new RsoCabecera()
                {
                    vigente = true,
                    ProyectoId = ProyectoId,
                    es_definitivo = true,
                    codigo_rdo = "Pendiente",
                    estado = true,
                    fecha_rdo = fecha,
                    version = "A",
                };
                if (Id > 0)
                {
                    var rdoAnterior = this.GetDetalles(Id);
                    if (rdoAnterior != null)
                    {
                        nuevoRdo.fecha_inicio = rdoAnterior.fecha_rdo.AddDays(1);
                    }
                }
                var id = Repository.InsertAndGetId(nuevoRdo);
                return id;
            }
        }

        public ExcelPackage GenerarExcelCabecera(int RdoCabeceraId)
        {
            var query = _eacRepository.GetAllIncluding(o => o.Computo, o => o.RsoCabecera.Proyecto)
               .Where(o => o.vigente)
               .Where(o => o.RsoCabeceraId == RdoCabeceraId).ToList();

            var cabecera = Repository.Get(RdoCabeceraId);

            /*Variables Contrato 2 */

            bool second_format = false;


            var proyecto = _proyectoRepository.GetAll().Where(c => c.Id == cabecera.ProyectoId).Where(c => c.vigente).FirstOrDefault();
            if (proyecto != null && proyecto.Id > 0)
            {
                var contrato = _contratoRepository.GetAll().Where(c => c.Id == proyecto.contratoId).Where(c => c.vigente).FirstOrDefault();

                if (contrato != null && contrato.Formato.HasValue && contrato.Formato.Value == FormatoContrato.Contrato_2019)
                {
                    second_format = true;
                }
            }

            #region Formato 1
            // Datos básicos del documento
            ExcelPackage excelPackage = new ExcelPackage();
            excelPackage.Workbook.Properties.Author = "CPP";
            excelPackage.Workbook.Properties.Title = "RSO";
            excelPackage.Workbook.Properties.Subject = "Generación de RSO";
            excelPackage.Workbook.Properties.Created = DateTime.Now;


            //Crea una hoja de trabajo
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("RSO");

            // Lineas de divición
            worksheet.View.ShowGridLines = false;

            // Cabecera de nombres 
            worksheet.Cells["B2:AI6"].Style.Font.SetFromFont(new Font("Arial", 25, FontStyle.Bold));

            worksheet.Cells["C3:AG3"].Merge = true;
            worksheet.Cells["C3:AG3"].Value = "PROYECTO CAMPO AUCA";
            worksheet.Cells["C3:AG3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["C3:AG3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            worksheet.Cells["C4:AG4"].Merge = true;
            worksheet.Cells["C4:AG4"].Value = proyecto.es_RSO ? "REPORTE SEMANAL OBRA" : "REPORTE SEMANAL OBRA";
            worksheet.Cells["C4:AG4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["C4:AG4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            worksheet.Cells["C5:AG5"].Merge = true;
            worksheet.Cells["C5:AG5"].Value = "(" + query[0].RsoCabecera.Proyecto.codigo + ") " + query[0].RsoCabecera.Proyecto.descripcion_proyecto;
            worksheet.Cells["C5:AG5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["C5:AG5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            /* string fechaperiodo = query[0].RsoCabecera.fecha_inicio.HasValue ? query[0].RsoCabecera.fecha_inicio.Value.ToString("dd-mmm-yyyy") + " - " + query[0].RsoCabecera.fecha_rdo.ToString("dd-mmm-yyyy") :
                  query[0].RsoCabecera.fecha_rdo.ToString("dd-mmm-yyyy");*/
            worksheet.Cells["C6:AG6"].Merge = true;
            worksheet.Cells["C6:AG6"].Value = query[0].RsoCabecera.fecha_rdo;
            worksheet.Cells["C6:AG6"].Style.Numberformat.Format = "dd-mmm-yyyy";
            worksheet.Cells["C6:AG6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["C6:AG6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            //worksheet.Cells["C6:AG6"].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;

            // Estilos de fuente
            var headerCells = worksheet.Cells["C8:AH14"];
            var headerFont = headerCells.Style.Font;
            headerFont.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            worksheet.Cells["C8:AG9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["C8:AG9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            worksheet.Cells["C11:AG11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["C11:AG11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            worksheet.Cells["C13:AJ14"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["C13:AJ14"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //Colores
            worksheet.Cells["C8:AJ8"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells["C8:AJ8"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

            worksheet.Cells["C11:AJ11"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells["C11:AJ11"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(166, 166, 166));

            worksheet.Cells["C9:AJ9"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells["C9:AJ9"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));

            worksheet.Cells["C13:AJ14"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells["C13:AJ14"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));

            worksheet.Cells["M13:O14"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells["M13:O14"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(48, 84, 150));

            worksheet.Cells["S13:U14"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells["S13:U14"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(48, 84, 150));

            worksheet.Cells["P13:R14"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells["P13:R14"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(142, 169, 219));

            worksheet.Cells["V13:V14"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells["V13:V14"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(142, 169, 219));

            worksheet.Cells["C8:AJ14"].Style.Font.Color.SetColor(Color.White);


            //Bordes
            worksheet.Cells["C8:AJ9"].Style.Border.BorderAround(ExcelBorderStyle.Medium, System.Drawing.Color.Black);
            worksheet.Cells["C11:AJ11"].Style.Border.BorderAround(ExcelBorderStyle.Medium, System.Drawing.Color.Black);

            worksheet.Cells["C8:AG8"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            worksheet.Cells["C8:AG8"].Style.Border.Bottom.Color.SetColor(Color.White);
            worksheet.Cells["C8:F9"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
            worksheet.Cells["C8:F9"].Style.Border.Right.Color.SetColor(Color.White);
            worksheet.Cells["I8:L9"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
            worksheet.Cells["I8:L9"].Style.Border.Right.Color.SetColor(Color.White);
            worksheet.Cells["I8:L9"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
            worksheet.Cells["I8:L9"].Style.Border.Left.Color.SetColor(Color.White);
            worksheet.Cells["W8:Y9"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
            worksheet.Cells["W8:Y9"].Style.Border.Left.Color.SetColor(Color.White);
            worksheet.Cells["W8:Y9"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
            worksheet.Cells["W8:Y9"].Style.Border.Right.Color.SetColor(Color.White);


            worksheet.Cells["C13:AJ14"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["C13:AJ14"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["C13:AJ14"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["C13:AJ14"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            worksheet.Cells["C13:AJ14"].Style.Border.BorderAround(ExcelBorderStyle.Medium, System.Drawing.Color.Black);





            // Header
            worksheet.Cells["C8:F8"].Merge = true;
            worksheet.Cells["C8:F8"].Value = "FECHA INICIO PROYECTO";
            worksheet.Cells["C9:F9"].Merge = true;
            worksheet.Cells["C9:F9"].Value = cabecera.Proyecto.fecha_estimada_inicio;




            worksheet.Cells["G8:H8"].Merge = true;
            worksheet.Cells["G8:H8"].Value = "FECHA DE FIN DE PROYECTO";
            worksheet.Cells["G9:H9"].Merge = true;
            worksheet.Cells["G9:H9"].Value = cabecera.Proyecto.fecha_estimada_fin;



            /*worksheet.Cells["M8:V8"].Merge = true;*/
            /*worksheet.Cells["M8:V8"].Value = "";*/

            worksheet.Cells["I8:V8"].Merge = true;
            worksheet.Cells["I8:V8"].Value = "FECHA DE INICIO REAL PROYECTO";
            // worksheet.Cells["I8:v8"].Style.WrapText = true;


            worksheet.Cells["W8:Y8"].Merge = true;
            worksheet.Cells["W8:Y8"].Value = "FECHA DE FIN REAL PROYECTO";

            // Plazo Previsto
            var plazo_previsto = (cabecera.Proyecto.fecha_estimada_fin.GetValueOrDefault().Day - cabecera.Proyecto.fecha_estimada_inicio.GetValueOrDefault().Day);
            worksheet.Cells["Z8:AA8"].Merge = true;
            worksheet.Cells["Z8:AA8"].Value = "PLAZO PREVISTO";
            worksheet.Cells["Z9:AA9"].Merge = true;
            worksheet.Cells["Z9:AA9"].Value = plazo_previsto;

            // Disruptivos
            var disruptivo = this.SumatoriaDisruptivo(cabecera.ProyectoId);
            worksheet.Cells["AB8:AC8"].Merge = true;
            worksheet.Cells["AB8:AC8"].Value = "DISRUPTIVOS";
            worksheet.Cells["AB9:AC9"].Merge = true;
            worksheet.Cells["AB9:AC9"].Value = disruptivo;

            // Plazo Ajustado
            worksheet.Cells["AD8:AE8"].Merge = true;
            worksheet.Cells["AD8:AE8"].Value = "PLAZO AJUSTADO";
            worksheet.Cells["AD9:AE9"].Merge = true;
            worksheet.Cells["AD9:AE9"].Value = plazo_previsto - disruptivo;

            worksheet.Cells["AF8"].Value = "SPI";
            worksheet.Cells["AG8"].Value = "CPI";

            worksheet.Cells["C11:AJ11"].Merge = true;
            worksheet.Cells["C11:AJ11"].Value = "SEGUIMIENTO DE PROYECTO";

            // ============================================= //
            worksheet.Cells["C13:C14"].Merge = true;
            worksheet.Cells["C13:C14"].Value = "Código Preciario";
            worksheet.Cells["C13:C14"].Style.WrapText = true;

            worksheet.Cells["D13:D14"].Merge = true;
            worksheet.Cells["D13:D14"].Value = "ID";

            worksheet.Cells["E13:G14"].Merge = true;
            worksheet.Cells["E13:G14"].Value = "Nombre Actividad";

            worksheet.Cells["E13:G14"].Merge = true;
            worksheet.Cells["E13:G14"].Value = "Nombre Actividad";

            worksheet.Cells["H13:H14"].Merge = true;
            worksheet.Cells["H13:H14"].Value = "% Buget";
            worksheet.Cells["H13:H14"].Style.WrapText = true;

            worksheet.Cells["I13:I14"].Merge = true;
            worksheet.Cells["I13:I14"].Value = "% EAC";
            worksheet.Cells["I13:I14"].Style.WrapText = true;

            worksheet.Cells["J13:J14"].Merge = true;
            worksheet.Cells["J13:J14"].Value = "Cantidad Budget";
            worksheet.Cells["J13:J14"].Style.WrapText = true;

            worksheet.Cells["K13:k14"].Merge = true;
            worksheet.Cells["K13:k14"].Value = "Cantidad EAC";
            worksheet.Cells["K13:K14"].Style.WrapText = true;

            worksheet.Cells["L13:L14"].Merge = true;
            worksheet.Cells["L13:L14"].Value = "UM";

            worksheet.Cells["M13:M14"].Merge = true;
            worksheet.Cells["M13:M14"].Value = "P.U";
            worksheet.Cells["M13:M14"].Style.WrapText = true;

            worksheet.Cells["N13:N14"].Merge = true;
            worksheet.Cells["N13:N14"].Value = "Costo Budget";
            worksheet.Cells["N13:N14"].Style.WrapText = true;

            worksheet.Cells["O13:O14"].Merge = true;
            worksheet.Cells["O13:O14"].Value = "Costo EAC";
            worksheet.Cells["O13:O14"].Style.WrapText = true;

            worksheet.Cells["P13:P14"].Merge = true;
            worksheet.Cells["P13:P14"].Value = "AC Anterior";
            worksheet.Cells["P13:P14"].Style.WrapText = true;

            worksheet.Cells["Q13:Q14"].Merge = true;
            worksheet.Cells["Q13:Q14"].Value = "AC Semanal";
            worksheet.Cells["Q13:Q14"].Style.WrapText = true;

            worksheet.Cells["R13:R14"].Merge = true;
            worksheet.Cells["R13:R14"].Value = "AC Actual";
            worksheet.Cells["R13:R14"].Style.WrapText = true;

            worksheet.Cells["S13:S14"].Merge = true;
            worksheet.Cells["S13:s14"].Value = "EV Anterior";
            worksheet.Cells["S13:S14"].Style.WrapText = true;

            worksheet.Cells["T13:T14"].Merge = true;
            worksheet.Cells["T13:T14"].Value = "EV Semanal";
            worksheet.Cells["T13:T14"].Style.WrapText = true;

            worksheet.Cells["U13:U14"].Merge = true;
            worksheet.Cells["U13:U14"].Value = "EV Actual";
            worksheet.Cells["U13:U14"].Style.WrapText = true;

            worksheet.Cells["V13:V14"].Merge = true;
            worksheet.Cells["V13:V14"].Value = "PV Costo Planificado";
            worksheet.Cells["V13:V14"].Style.WrapText = true;

            worksheet.Cells["K13:k14"].Merge = true;
            worksheet.Cells["K13:k14"].Value = "Cantidad EAC";
            worksheet.Cells["K13:K14"].Style.WrapText = true;

            worksheet.Cells["W13:X13"].Merge = true;
            worksheet.Cells["W13:X13"].Value = "Previsto";

            worksheet.Cells["W14"].Value = "Inicio";
            worksheet.Cells["X14"].Value = "Fin";

            worksheet.Cells["Y13:Z13"].Merge = true;
            worksheet.Cells["Y13:Z13"].Value = "Real";

            worksheet.Cells["Y14"].Value = "Inicio";
            worksheet.Cells["Z14"].Value = "Fin";

            worksheet.Cells["AA13:AC13"].Merge = true;
            worksheet.Cells["AA13:AC13"].Value = "Cantidad";

            worksheet.Cells["AA14"].Value = "Anterior";
            worksheet.Cells["AB14"].Value = "Semanal";
            worksheet.Cells["AC14"].Value = "Acumulada";

            worksheet.Cells["AD13:AD14"].Merge = true;
            worksheet.Cells["AD13:AD14"].Value = "% Avance Acumulado Anterior";
            worksheet.Cells["AD13:AD14"].Style.WrapText = true;

            worksheet.Cells["AE13:AE14"].Merge = true;
            worksheet.Cells["AE13:AE14"].Value = "% Avance Semanal";
            worksheet.Cells["AE13:AE14"].Style.WrapText = true;

            worksheet.Cells["AF13:AF14"].Merge = true;
            worksheet.Cells["AF13:AF14"].Value = "% AvanceActual Acumulado";
            worksheet.Cells["AF13:AF14"].Style.WrapText = true;

            worksheet.Cells["AG13:AJ14"].Merge = true;
            worksheet.Cells["AG13:AJ14"].Value = "% Avance Previsto Acumulado";
            worksheet.Cells["AG13:AJ14"].Style.WrapText = true;
            /*
            worksheet.Cells["AI13:AI14"].Merge = true;
            worksheet.Cells["AI13:AI14"].Value = "Earn Value";
            worksheet.Cells["AI13:AI14"].Style.WrapText = true;
            worksheet.Cells["AI13:AI14"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["AI13:AI14"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            worksheet.Cells["AI13:AI14"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(48, 84, 150));
            worksheet.Cells["AI13:AI14"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["AI13:AI14"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["AI13:AI14"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["AI13:AI14"].Style.Font.Size = 18;
            worksheet.Cells["AI13:AI14"].Style.Font.Bold = true;
            */

            #endregion
            // Width de las columnas
            worksheet.Column(3).Width = 20;
            worksheet.Column(4).Width = 17;
            worksheet.Column(5).Width = 25;
            worksheet.Column(6).Width = 34;
            worksheet.Column(7).Width = 70;
            worksheet.Column(7).Width = 70;
            worksheet.Column(8).Width = 17;
            worksheet.Column(9).Width = 17;
            worksheet.Column(10).Width = 21;
            worksheet.Column(11).Width = 22;
            worksheet.Column(12).Width = 18;
            worksheet.Column(13).Width = 14;
            worksheet.Column(14).Width = 29;
            worksheet.Column(15).Width = 29;
            worksheet.Column(16).Width = 27;
            worksheet.Column(17).Width = 21;
            worksheet.Column(18).Width = 28;
            worksheet.Column(19).Width = 28;
            worksheet.Column(20).Width = 21;
            worksheet.Column(21).Width = 27;
            worksheet.Column(22).Width = 28;
            worksheet.Column(23).Width = 28;
            worksheet.Column(24).Width = 62;
            worksheet.Column(25).Width = 29;
            worksheet.Column(26).Width = 29;
            worksheet.Column(27).Width = 19;
            worksheet.Column(28).Width = 19;
            worksheet.Column(29).Width = 19;
            worksheet.Column(30).Width = 23;
            worksheet.Column(31).Width = 23;
            worksheet.Column(32).Width = 23;
            worksheet.Column(33).Width = 23;
            worksheet.Column(35).Width = 5;
            worksheet.Column(36).Width = 5;
            // Heigh
            worksheet.Row(7).Height = 37;
            worksheet.Row(8).Height = 51;
            worksheet.Row(9).Height = 44;
            worksheet.Row(13).Height = 54;
            worksheet.Row(14).Height = 54;
            worksheet.Row(4).Height = 47;
            worksheet.Row(5).Height = 37;
            worksheet.Row(6).Height = 37;
            worksheet.Column(35).Width = 5;
            worksheet.Column(36).Width = 5;


            // Zoom
            worksheet.View.ZoomScale = 40;

            if (second_format)
            {



                /*Totales con Descuentos APlicados*/

                var rangot = "C" + 15 + ":AJ" + 15;
                var cellt = "F" + 15;
                worksheet.Cells[cellt].Value = "TOTAL CON DESCUENTO APLICADO";
                worksheet.Cells[rangot].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[rangot].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));
                worksheet.Cells[rangot].Style.Border.BorderAround(ExcelBorderStyle.Medium);

                worksheet.Cells[rangot].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[rangot].Style.Font.Size = 17;
                worksheet.Cells[rangot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[rangot].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Row(15).Height = 50;

            }

            return excelPackage;
        }


        public ExcelPackage GenerarExcelRdo(int RdoCabeceraId, string TipoReporte)
        {
            /*Variables Contrato 2 */

            bool second_format = false;
            int filaAdicionales = 0;
            bool tieneItemsPendientes = false;
            var excelPackage = this.GenerarExcelCabecera(RdoCabeceraId);

            var worksheet = excelPackage.Workbook.Worksheets[1];

            var cabecera = Repository.Get(RdoCabeceraId);

            var proyecto = _proyectoRepository.GetAll().Where(c => c.Id == cabecera.ProyectoId).Where(c => c.vigente).FirstOrDefault();
            if (proyecto != null && proyecto.Id > 0)
            {
                var contrato = _contratoRepository.GetAll().Where(c => c.Id == proyecto.contratoId).Where(c => c.vigente).FirstOrDefault();

                if (contrato != null && contrato.Formato.HasValue && contrato.Formato.Value == FormatoContrato.Contrato_2019)
                {
                    second_format = true;
                }
            }

            /*  Solo Observciones a la Fecha del RDO*/
            var observaciones = this.ListarPorProyectoTipo(cabecera.ProyectoId, TipoComentario.Observacion).Where(c => c.FechaObservacion <= cabecera.fecha_rdo).ToList();
            var actividadesrealizadas = this.ListarPorProyectoTipo(cabecera.ProyectoId, TipoComentario.ActividadRealizada).Where(c => c.FechaObservacion <= cabecera.fecha_rdo).ToList(); ;
            var actividadesprogramadas = this.ListarPorProyectoTipo(cabecera.ProyectoId, TipoComentario.ActividadProgramada).Where(c => c.FechaObservacion <= cabecera.fecha_rdo).ToList();


            var RSOAnterior = Repository.GetAll().Where(c => c.fecha_rdo < cabecera.fecha_rdo)
                                      .Where(c => c.vigente)
                                      .Where(c => c.es_definitivo)
                                      .OrderByDescending(c => c.fecha_rdo)
                                      .FirstOrDefault();

            if (RSOAnterior != null)
            {
                actividadesrealizadas = actividadesrealizadas.Where(c => c.FechaObservacion > RSOAnterior.fecha_rdo).ToList();
                actividadesprogramadas = actividadesprogramadas.Where(c => c.FechaObservacion > RSOAnterior.fecha_rdo).ToList();
            }




            //var actividades = this.GetActividades(cabecera.ProyectoId, cabecera.fecha_rdo);

            var datos = this.GetRdo(RdoCabeceraId, TipoReporte);

            MontosTotalesRDO m = new MontosTotalesRDO();
            if (second_format) //Si es Segundo Formato
            {
                var rdoDetalles = _eacRepository.GetAll()
                                                            .Where(c => c.vigente)
                                                           .Where(c => c.RsoCabeceraId == cabecera.Id)
                                                            .ToList();



                if (rdoDetalles.Count > 0)
                {
                    m.costoBudget = Decimal.Round(
                                               (from c in rdoDetalles
                                                select Decimal.Round(c.costo_presupuesto, 8)).Sum()
                                                 , 8);

                    m.costoEAC = Decimal.Round(
                                              (from c in rdoDetalles

                                               select Decimal.Round(c.costo_eac, 8)).Sum()
                                               , 8);

                    m.ac_anterior = Decimal.Round(
                                                 (from c in rdoDetalles

                                                  select Decimal.Round(c.ac_anterior, 8)).Sum()
                                               , 8);
                    m.ac_diario = Decimal.Round(
                                                (from c in rdoDetalles

                                                 select Decimal.Round(c.ac_diario, 8)).Sum()
                                                 , 8);
                    m.ac_actual = Decimal.Round(
                                               (from c in rdoDetalles

                                                select Decimal.Round(c.ac_actual, 8)).Sum()
                                                , 8);

                    m.ev_anterior = Decimal.Round(
                                            (from c in rdoDetalles

                                             select Decimal.Round(c.ev_anterior, 8)).Sum()
                                             , 8);
                    m.ev_diario = Decimal.Round(
                                                 (from c in rdoDetalles

                                                  select Decimal.Round(c.ev_diario, 8)).Sum()
                                                  , 8);

                    m.ev_actual = Decimal.Round(
                                               (from c in rdoDetalles

                                                select Decimal.Round(c.ev_actual, 8)).Sum()
                                                , 8);

                    m.ern_value = Decimal.Round(
                                                  (from c in rdoDetalles

                                                   select Decimal.Round(c.ern_value, 8)).Sum()
                                                   , 8);



                    m.pv_costo_planificado = Decimal.Round(
                                                 (from c in rdoDetalles

                                                  select Decimal.Round(c.pv_costo_planificado, 8)).Sum()
                                                 , 8);


                }
            }

            var hojaAdicionales = excelPackage.Workbook.Worksheets.Add("ADICIONALES"); //Hoja Adicionales



            hojaAdicionales.Cells["C13:C14"].Merge = true;
            hojaAdicionales.Cells["C13:C14"].Value = "Costo EAC";
            hojaAdicionales.Cells["C13:C14"].Style.WrapText = true;

            hojaAdicionales.Cells["D13:D14"].Merge = true;
            hojaAdicionales.Cells["D13:D14"].Value = "AC Actual";
            hojaAdicionales.Cells["D13:D14"].Style.WrapText = true;

            hojaAdicionales.Cells["C13:C14"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            hojaAdicionales.Cells["C13:C14"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));

            hojaAdicionales.Cells["D13:D14"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            hojaAdicionales.Cells["D13:D14"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(142, 169, 219));

            hojaAdicionales.Cells["C2:D9"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            hojaAdicionales.Cells["C2:D9"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(237, 125, 49));

            var datosAdicionales = this.GetRdoAdicionales(RdoCabeceraId, TipoReporte);
            var rdoDetallesAdicionales = _eacRepository.GetAll()
                                                   .Where(c => c.vigente)
                                                   .Where(c => c.RsoCabeceraId == RdoCabeceraId)
                                                   .Where(c => c.PendienteAprobacion || c.es_temporal)
                                                   .ToList();
            if (rdoDetallesAdicionales.Count > 0)
            {
                tieneItemsPendientes = true;
            }
            MontosTotalesRDO ma = new MontosTotalesRDO();

            if (rdoDetallesAdicionales.Count > 0)
            {
                int precision = 20;
                ma.porcentajeBudget = Decimal.Round((from c in rdoDetallesAdicionales select Decimal.Round(c.porcentaje_presupuesto_total, precision)).Sum()
                                           , precision);
                ma.porcentajeEAC = Decimal.Round((from c in rdoDetallesAdicionales select Decimal.Round(c.porcentaje_costo_eac_total, precision)).Sum(), precision);
                ma.costoBudget = Decimal.Round((from c in rdoDetallesAdicionales select Decimal.Round(c.costo_presupuesto, 8)).Sum()
                                             , 8);
                ma.costoEAC = Decimal.Round((from c in rdoDetallesAdicionales select Decimal.Round(c.costo_eac, 8)).Sum()
                                           , 8);

                ma.ac_anterior = Decimal.Round((from c in rdoDetallesAdicionales select Decimal.Round(c.ac_anterior, 8)).Sum()
                                           , 8);
                ma.ac_diario = Decimal.Round((from c in rdoDetallesAdicionales select Decimal.Round(c.ac_diario, 8)).Sum()
                                             , 8);
                ma.ac_actual = Decimal.Round((from c in rdoDetallesAdicionales select Decimal.Round(c.ac_actual, 8)).Sum()
                                            , 8);

                ma.ev_anterior = Decimal.Round((from c in rdoDetallesAdicionales select Decimal.Round(c.ev_anterior, 8)).Sum()
                                         , 8);
                ma.ev_diario = Decimal.Round(
                                             (from c in rdoDetallesAdicionales select Decimal.Round(c.ev_diario, 8)).Sum()
                                              , 8);

                ma.ev_actual = Decimal.Round(
                                           (from c in rdoDetallesAdicionales select Decimal.Round(c.ev_actual, 8)).Sum()
                                            , 8);

                ma.ern_value = Decimal.Round(
                                              (from c in rdoDetallesAdicionales select Decimal.Round(c.ern_value, 8)).Sum()
                                               , 8);



                ma.pv_costo_planificado = Decimal.Round(
                                             (from c in rdoDetallesAdicionales select Decimal.Round(c.pv_costo_planificado, 8)).Sum()
                                             , 8);

                ma.fecha_inicio_prevista = (from c in rdoDetallesAdicionales
                                            where c.fecha_inicio_prevista != null
                                            select c.fecha_inicio_prevista).Min(c => c.Value);
                ma.fecha_fin_prevista = (from c in rdoDetallesAdicionales
                                         where c.fecha_fin_prevista != null
                                         select c.fecha_fin_prevista).Max(c => c.Value);

                ma.fecha_inicio_real = (from c in rdoDetallesAdicionales
                                        where c.fecha_inicio_real != null
                                        select c.fecha_inicio_real).Count() > 0 ?
                                       (from c in rdoDetallesAdicionales
                                        where c.fecha_inicio_real != null
                                        select c.fecha_inicio_real).Min(c => c.Value) : new DateTime(1999, 01, 01);


                var fecha_reales = (from c in rdoDetallesAdicionales
                                    where c.fecha_fin_real != null
                                    select c.fecha_fin_real).ToList();
                if (fecha_reales.Count > 0)
                {
                    ma.fecha_fin_real = (from c in rdoDetallesAdicionales
                                         where c.fecha_fin_real != null
                                         select c.fecha_fin_real).Max(c => c.Value);
                }
                else
                {
                    ma.fecha_fin_real = new DateTime(1999, 01, 01);
                }

                decimal costo_presupuesto = Decimal.Round((from c in rdoDetallesAdicionales
                                                           select Decimal.Round(c.costo_presupuesto, precision)).Sum(), precision);

                if (costo_presupuesto > 0)
                {
                    ma.avance_Acumulado_Anterior = Decimal.Round(
                                                        Decimal.Round((from c in rdoDetallesAdicionales
                                                                       select Decimal.Round(c.ev_anterior, precision)).Sum(), precision) / costo_presupuesto, precision);

                    ma.avance_Diario = Decimal.Round(
                                                     Decimal.Round((from c in rdoDetallesAdicionales
                                                                    select Decimal.Round(c.ev_diario, precision)).Sum(), precision) /
                                                    costo_presupuesto
                                                    , precision);


                    ma.avance_Actual_Acumulado = Decimal.Round(
                                                      Decimal.Round((from c in rdoDetallesAdicionales
                                                                     select Decimal.Round(c.ev_actual, precision)).Sum(), precision) /
                                                     costo_presupuesto, precision);

                    ma.Avance_Previsto_Acumulado = Decimal.Round(
                                                    Decimal.Round((from c in rdoDetallesAdicionales
                                                                   select Decimal.Round(c.pv_costo_planificado, precision)).Sum(), precision) /
                                                  costo_presupuesto
                                                            , precision);


                }
            }


            var count = 0;
            var fistline = 17;

            worksheet.Cells[1, 2].Value = fistline; //Value Start Data

            for (int i = fistline; i < datos.Count + fistline; i++)
            {

                if (datos[count].tipo == "Padre")
                {

                    var cell = "E" + i + ":G" + i;
                    worksheet.Cells[cell].Merge = true;
                    worksheet.Cells[cell].Value = datos[count].nombre_actividad;

                    worksheet.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    var range = "C" + i + ":AJ" + i;
                    worksheet.Cells[range].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    hojaAdicionales.Cells[range].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    if (datos[count].color.Length > 0)
                    {
                        worksheet.Cells[range].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(datos[count].color));

                        hojaAdicionales.Cells[range].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(datos[count].color));
                    }
                    else
                    {

                        worksheet.Cells[range].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                        hojaAdicionales.Cells[range].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));

                    }
                    // worksheet.Cells[range].Style.Font.Color.SetColor(Color.Black);
                    if (datos[count].principal)
                    {
                        worksheet.Cells[range].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[range].Style.Font.Bold = true;
                        worksheet.Cells[cell].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));

                    }
                    //worksheet.Cells[cell].Style.Font.Bold = true;
                    worksheet.Row(i).Height = 37;
                    // worksheet.Row(i).Style.Font.Bold = true;

                    /*cell = "D" + i + "";
                    worksheet.Cells[cell].Value = datos[count].costo_budget_version_anterior; // CostoBugetAnterior
                    */

                    cell = "H" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_presupuesto_total; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "I" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_costo_eac_total; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "J" + i + "";
                    //worksheet.Cells[cell].Value = datos[count].presupuesto_total;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "K" + i + "";
                    //worksheet.Cells[cell].Value = datos[count].cantidad_eac;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "M" + i + "";
                    //worksheet.Cells[cell].Value = datos[count].precio_unitario; //Todo

                    cell = "N" + i + "";
                    worksheet.Cells[cell].Value = datos[count].costo_presupuesto;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "O" + i + "";
                    worksheet.Cells[cell].Value = datos[count].costo_eac;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "P" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ac_anterior;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "Q" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ac_diario;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "R" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ac_actual;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "S" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ev_anterior;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "T" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ev_diario;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "U" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ev_actual;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "V" + i + "";
                    worksheet.Cells[cell].Value = datos[count].pv_costo_planificado;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "W" + i + "";
                    worksheet.Cells[cell].Value = datos[count].fecha_inicio_prevista; // Fecha
                    worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                    cell = "X" + i + "";
                    worksheet.Cells[cell].Value = datos[count].fecha_fin_prevista; // fecha
                    worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                    cell = "Y" + i + "";

                    worksheet.Cells[cell].Value = datos[count].fecha_inicio_real; //fecha

                    worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                    cell = "Z" + i + "";
                    if (datos[count].porcentaje_avance_actual_acumulado == 1)
                    {
                        worksheet.Cells[cell].Value = datos[count].fecha_fin_real; // fecha
                    }
                    worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                    cell = "AA" + i + "";
                    //worksheet.Cells[cell].Value = datos[count].cantidad_anterior;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "AB" + i + "";
                    //worksheet.Cells[cell].Value = datos[count].cantidad_diaria;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "AC" + i + "";
                    //worksheet.Cells[cell].Value = datos[count].cantidad_acumulada;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "AD" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_avance_anterior; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";//
                    cell = "AE" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_avance_diario; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "AF" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_avance_actual_acumulado; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "AG" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_avance_previsto_acumulado; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";

                    cell = "AI" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ern_value;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "AJ" + i + "";


                    decimal number = Decimal.Round(datos[count].porcentaje_avance_actual_acumulado - datos[count].porcentaje_avance_previsto_acumulado, 1);
                    double datavalue = decimal.ToDouble(number);

                    worksheet.Cells[cell].Value = number; // porcentaje

                    if (datavalue >= 0.0)
                    {
                        ExcelRange rng = worksheet.Cells[cell];
                        ExcelAddress address = new ExcelAddress(rng.Address);
                        var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                        v.ShowValue = false;
                        v.Icon2.Value = 1;
                        v.Icon3.Value = 2;
                        //  v.Icon2.Value = -0.05;
                        // v.Icon3.Value = 0.0;
                        v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;


                        //string formular = "=IF($AH$" + i + "= 0,0,ROUND(" + "$AG$" + i + "-$AH$" + i + ",1))";
                        //worksheet.Cells[indicador].Formula = formular;  // porcentaje
                        worksheet.Cells[cell].Value = 3;


                    }
                    else if (datavalue < 0.0 && datavalue >= -0.05)
                    {
                        ExcelRange rng = worksheet.Cells[cell];
                        ExcelAddress address = new ExcelAddress(rng.Address);
                        var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                        v.ShowValue = false;
                        v.Icon2.Value = 1;
                        v.Icon3.Value = 2;
                        //  v.Icon2.Value = -0.05;
                        // v.Icon3.Value = 0.0;
                        v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;


                        //string formular = "=IF($AH$" + i + "= 0,0,ROUND(" + "$AG$" + i + "-$AH$" + i + ",1))";
                        //worksheet.Cells[indicador].Formula = formular;  // porcentaje
                        worksheet.Cells[cell].Value = 1;

                    }
                    else
                    {
                        ExcelRange rng = worksheet.Cells[cell];
                        ExcelAddress address = new ExcelAddress(rng.Address);
                        var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                        v.ShowValue = false;
                        v.Icon2.Value = 1;
                        v.Icon3.Value = 2;
                        //  v.Icon2.Value = -0.05;
                        // v.Icon3.Value = 0.0;
                        v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;


                        //string formular = "=IF($AH$" + i + "= 0,0,ROUND(" + "$AG$" + i + "-$AH$" + i + ",1))";
                        //worksheet.Cells[indicador].Formula = formular;  // porcentaje
                        worksheet.Cells[cell].Value = 0;
                    }

                    /*COLUMNAS ADICIONALES PADRES*/

                    /*var apu = new List<RdoDatos>();
                    apu.Add(datos[count]);
                    var costo_eac_descuento = this.ObtenerTotalesEspecialidad(apu, "COSTO_EAC", m.costoEAC);
                    var ac_actual_descuento = this.ObtenerTotalesEspecialidad(apu, "AC_ACTUAL", m.ac_actual);*/

                    cell = "C" + i + "";

                    // hojaAdicionales.Cells[cell].Value = datos[count].costo_eac - this.ObtenerDescuentoEspecialidadAPU(costo_eac_descuento, datos[count]);
                    hojaAdicionales.Cells[cell].Value = datos[count].costo_eac_descuento;

                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    if (datos[count].principal)
                    {
                        hojaAdicionales.Cells[cell].Style.Font.Color.SetColor(Color.White);
                        hojaAdicionales.Cells[cell].Style.Font.Bold = true;
                        hojaAdicionales.Cells[cell].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));

                    }

                    cell = "D" + i + "";
                    // hojaAdicionales.Cells[cell].Value = datos[count].ac_actual - this.ObtenerDescuentoEspecialidadAPU(ac_actual_descuento, datos[count]);
                    hojaAdicionales.Cells[cell].Value = datos[count].ac_actual_descuento;

                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    if (datos[count].principal)
                    {
                        hojaAdicionales.Cells[cell].Style.Font.Color.SetColor(Color.White);
                        hojaAdicionales.Cells[cell].Style.Font.Bold = true;
                        hojaAdicionales.Cells[cell].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));

                    }

                }
                else if (datos[count].tipo == "Actividad")
                {

                    var cell = "E" + i + ":G" + i;
                    worksheet.Cells[cell].Merge = true;
                    worksheet.Cells[cell].Value = datos[count].nombre_actividad;
                    worksheet.Cells[cell].Style.WrapText = true;
                    worksheet.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    var range = "C" + i + ":AJ" + i;
                    worksheet.Cells[range].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    hojaAdicionales.Cells[range].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    if (datos[count].color.Length > 0)
                    {
                        worksheet.Cells[range].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(datos[count].color));
                        hojaAdicionales.Cells[range].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(datos[count].color));

                    }
                    else
                    {
                        worksheet.Cells[range].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                        hojaAdicionales.Cells[range].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));

                    }
                    worksheet.Cells[range].Style.Font.Color.SetColor(Color.Black);
                    //worksheet.Cells[cell].Style.Font.Bold = true;
                    worksheet.Row(i).Height = 32;
                    //worksheet.Row(i).Style.Font.Bold = true;

                    /* cell = "D" + i + "";
                     worksheet.Cells[cell].Value = datos[count].costo_budget_version_anterior; // CostoBugetAnterior*/

                    cell = "H" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_presupuesto_total; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "I" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_costo_eac_total; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "J" + i + "";
                    //worksheet.Cells[cell].Value = datos[count].presupuesto_total;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "K" + i + "";
                    // worksheet.Cells[cell].Value = datos[count].cantidad_eac;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "M" + i + "";
                    //worksheet.Cells[cell].Value = datos[count].precio_unitario; //Todo
                    cell = "N" + i + "";
                    worksheet.Cells[cell].Value = datos[count].costo_presupuesto;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "O" + i + "";
                    worksheet.Cells[cell].Value = datos[count].costo_eac;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "P" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ac_anterior;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "Q" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ac_diario;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "R" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ac_actual;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "S" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ev_anterior;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "T" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ev_diario;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "U" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ev_actual;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "V" + i + "";
                    worksheet.Cells[cell].Value = datos[count].pv_costo_planificado;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "W" + i + "";
                    worksheet.Cells[cell].Value = datos[count].fecha_inicio_prevista; // Fecha
                    worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                    cell = "X" + i + "";
                    worksheet.Cells[cell].Value = datos[count].fecha_fin_prevista; // fecha
                    worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                    cell = "Y" + i + "";
                    worksheet.Cells[cell].Value = datos[count].fecha_inicio_real; //fecha
                    worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                    cell = "Z" + i + "";
                    if (datos[count].porcentaje_avance_actual_acumulado == 1)
                    {
                        worksheet.Cells[cell].Value = datos[count].fecha_fin_real; // fecha
                    }
                    worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                    cell = "AA" + i + "";
                    //worksheet.Cells[cell].Value = datos[count].cantidad_anterior;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "AB" + i + "";
                    //worksheet.Cells[cell].Value = datos[count].cantidad_diaria;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "AC" + i + "";
                    //worksheet.Cells[cell].Value = datos[count].cantidad_acumulada;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "AD" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_avance_anterior; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "AE" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_avance_diario; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "AF" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_avance_actual_acumulado; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "AG" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_avance_previsto_acumulado; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "AI" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ern_value; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "AJ" + i + "";


                    decimal number = Decimal.Round(datos[count].porcentaje_avance_actual_acumulado - datos[count].porcentaje_avance_previsto_acumulado, 1);

                    worksheet.Cells[cell].Value = number; // porcentaje

                    double datavalue = decimal.ToDouble(number);

                    if (datavalue >= 0.0)
                    {
                        ExcelRange rng = worksheet.Cells[cell];
                        ExcelAddress address = new ExcelAddress(rng.Address);
                        var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                        v.ShowValue = false;
                        v.Icon2.Value = 1;
                        v.Icon3.Value = 2;
                        //  v.Icon2.Value = -0.05;
                        // v.Icon3.Value = 0.0;
                        v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;


                        //string formular = "=IF($AH$" + i + "= 0,0,ROUND(" + "$AG$" + i + "-$AH$" + i + ",1))";
                        //worksheet.Cells[indicador].Formula = formular;  // porcentaje
                        worksheet.Cells[cell].Value = 3;


                    }
                    else if (datavalue < 0.0 && datavalue >= -0.05)
                    {
                        ExcelRange rng = worksheet.Cells[cell];
                        ExcelAddress address = new ExcelAddress(rng.Address);
                        var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                        v.ShowValue = false;
                        v.Icon2.Value = 1;
                        v.Icon3.Value = 2;
                        //  v.Icon2.Value = -0.05;
                        // v.Icon3.Value = 0.0;
                        v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;


                        //string formular = "=IF($AH$" + i + "= 0,0,ROUND(" + "$AG$" + i + "-$AH$" + i + ",1))";
                        //worksheet.Cells[indicador].Formula = formular;  // porcentaje
                        worksheet.Cells[cell].Value = 1;

                    }
                    else
                    {
                        ExcelRange rng = worksheet.Cells[cell];
                        ExcelAddress address = new ExcelAddress(rng.Address);
                        var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                        v.ShowValue = false;
                        v.Icon2.Value = 1;
                        v.Icon3.Value = 2;
                        //  v.Icon2.Value = -0.05;
                        // v.Icon3.Value = 0.0;
                        v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;


                        //string formular = "=IF($AH$" + i + "= 0,0,ROUND(" + "$AG$" + i + "-$AH$" + i + ",1))";
                        //worksheet.Cells[indicador].Formula = formular;  // porcentaje
                        worksheet.Cells[cell].Value = 0;
                    }


                    /*COLUMNAS ADICIONALES ACTIVIDADES*/

                    /*var apu = new List<RdoDatos>();
                    apu.Add(datos[count]);
                    var costo_eac_descuento = this.ObtenerTotalesEspecialidad(apu, "COSTO_EAC", m.costoEAC);
                    var ac_actual_descuento = this.ObtenerTotalesEspecialidad(apu, "AC_ACTUAL", m.ac_actual);
                    */
                    cell = "C" + i + "";

                    //hojaAdicionales.Cells[cell].Value = datos[count].costo_eac - this.ObtenerDescuentoEspecialidadAPU(costo_eac_descuento, datos[count]);
                    hojaAdicionales.Cells[cell].Value = datos[count].costo_eac_descuento;

                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "D" + i + "";
                    //hojaAdicionales.Cells[cell].Value = datos[count].ac_actual - this.ObtenerDescuentoEspecialidadAPU(ac_actual_descuento, datos[count]);
                    hojaAdicionales.Cells[cell].Value = datos[count].ac_actual_descuento;

                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                }
                else
                {

                    var cell = "C" + i + "";
                    worksheet.Cells[cell].Value = datos[count].codigo_preciario;

                    cell = "D" + i + "";
                    worksheet.Cells[cell].Value = datos[count].id_rubro; // CostoBugetAnterior


                    cell = "E" + i + ":G" + i;
                    worksheet.Cells[cell].Merge = true;
                    worksheet.Cells[cell].Value = datos[count].nombre_actividad;

                    worksheet.Row(i).Height = 40;
                    worksheet.Cells[cell].Style.WrapText = true;
                    worksheet.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    /*cell = "D" + i + "";
                    worksheet.Cells[cell].Value = datos[count].costo_budget_version_anterior; // CostoBugetAnterior*/

                    cell = "H" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_presupuesto_total; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "I" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_costo_eac_total;// porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "J" + i + "";
                    worksheet.Cells[cell].Value = datos[count].presupuesto_total;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "K" + i + "";
                    worksheet.Cells[cell].Value = datos[count].cantidad_eac;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "L" + i + "";
                    worksheet.Cells[cell].Value = datos[count].UM; //Todo
                    cell = "M" + i + "";
                    worksheet.Cells[cell].Value = datos[count].precio_unitario; //Todo
                    cell = "N" + i + "";
                    worksheet.Cells[cell].Value = datos[count].costo_presupuesto;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "O" + i + "";
                    worksheet.Cells[cell].Value = datos[count].costo_eac;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "P" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ac_anterior;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "Q" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ac_diario;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "R" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ac_actual;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "S" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ev_anterior;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "T" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ev_diario;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "U" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ev_actual;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "V" + i + "";
                    worksheet.Cells[cell].Value = datos[count].pv_costo_planificado;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "W" + i + "";
                    worksheet.Cells[cell].Value = datos[count].fecha_inicio_prevista; // Fecha
                    worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                    cell = "X" + i + "";
                    worksheet.Cells[cell].Value = datos[count].fecha_fin_prevista; // fecha
                    worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                    cell = "Y" + i + "";

                    worksheet.Cells[cell].Value = datos[count].fecha_inicio_real;
                    //fecha
                    worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                    cell = "Z" + i + "";
                    if (datos[count].porcentaje_avance_actual_acumulado == 1)
                    {
                        worksheet.Cells[cell].Value = datos[count].fecha_fin_real; // fecha
                    }
                    worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                    cell = "AA" + i + "";
                    worksheet.Cells[cell].Value = datos[count].cantidad_anterior;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "AB" + i + "";
                    worksheet.Cells[cell].Value = datos[count].cantidad_diaria;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "AC" + i + "";
                    worksheet.Cells[cell].Value = datos[count].cantidad_acumulada;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "AD" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_avance_anterior; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "AE" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_avance_diario; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "AF" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_avance_actual_acumulado; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "AG" + i + "";
                    worksheet.Cells[cell].Value = datos[count].porcentaje_avance_previsto_acumulado; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                    cell = "AI" + i + "";
                    worksheet.Cells[cell].Value = datos[count].ern_value; // porcentaje
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "AJ" + i + "";


                    decimal number = Decimal.Round(datos[count].porcentaje_avance_actual_acumulado - datos[count].porcentaje_avance_previsto_acumulado, 1);


                    double datavalue = decimal.ToDouble(number);


                    if (datavalue >= 0.0)
                    {
                        ExcelRange rng = worksheet.Cells[cell];
                        ExcelAddress address = new ExcelAddress(rng.Address);
                        var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                        v.ShowValue = false;
                        v.Icon2.Value = 1;
                        v.Icon3.Value = 2;
                        //  v.Icon2.Value = -0.05;
                        // v.Icon3.Value = 0.0;
                        v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;


                        //string formular = "=IF($AH$" + i + "= 0,0,ROUND(" + "$AG$" + i + "-$AH$" + i + ",1))";
                        //worksheet.Cells[indicador].Formula = formular;  // porcentaje
                        worksheet.Cells[cell].Value = 3;


                    }
                    else if (datavalue < 0.0 && datavalue >= -0.05)
                    {
                        ExcelRange rng = worksheet.Cells[cell];
                        ExcelAddress address = new ExcelAddress(rng.Address);
                        var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                        v.ShowValue = false;
                        v.Icon2.Value = 1;
                        v.Icon3.Value = 2;
                        //  v.Icon2.Value = -0.05;
                        // v.Icon3.Value = 0.0;
                        v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;


                        //string formular = "=IF($AH$" + i + "= 0,0,ROUND(" + "$AG$" + i + "-$AH$" + i + ",1))";
                        //worksheet.Cells[indicador].Formula = formular;  // porcentaje
                        worksheet.Cells[cell].Value = 1;

                    }
                    else
                    {
                        ExcelRange rng = worksheet.Cells[cell];
                        ExcelAddress address = new ExcelAddress(rng.Address);
                        var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                        v.ShowValue = false;
                        v.Icon2.Value = 1;
                        v.Icon3.Value = 2;
                        //  v.Icon2.Value = -0.05;
                        // v.Icon3.Value = 0.0;
                        v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;


                        //string formular = "=IF($AH$" + i + "= 0,0,ROUND(" + "$AG$" + i + "-$AH$" + i + ",1))";
                        //worksheet.Cells[indicador].Formula = formular;  // porcentaje
                        worksheet.Cells[cell].Value = 0;
                    }

                    /*COLUMNAS ADICIONALES AUXILIARES APUS*/

                    var apu = new List<RdoDatos>();
                    apu.Add(datos[count]);
                    var costo_eac_descuento = this.ObtenerTotalesEspecialidad(apu, "COSTO_EAC", m.costoEAC);
                    var ac_actual_descuento = this.ObtenerTotalesEspecialidad(apu, "AC_ACTUAL", m.ac_actual);

                    cell = "C" + i + "";

                    hojaAdicionales.Cells[cell].Value = datos[count].costo_eac - this.ObtenerDescuentoEspecialidadAPU(costo_eac_descuento, datos[count]);
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "D" + i + "";
                    hojaAdicionales.Cells[cell].Value = datos[count].ac_actual - this.ObtenerDescuentoEspecialidadAPU(ac_actual_descuento, datos[count]);
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                }

                count++;
            }

            count += 19; // 17 de la cabecera + 2 de la sepracion de
            worksheet.Cells[1, 3].Value = count; //Value Start Data



            //var datosAdicionales = new List<RdoDatos>();
            if (datosAdicionales.Count > 1)
            {


                var cellItem = "C" + count + ":F" + count;
                worksheet.Cells[cellItem].Merge = true;
                worksheet.Cells[cellItem].Value = "ITEMS PENDIENTES";
                worksheet.Cells[cellItem].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                worksheet.Cells[cellItem].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[cellItem].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                worksheet.Cells[cellItem].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[cellItem].Style.Fill.BackgroundColor.SetColor(Color.Black);
                worksheet.Cells[cellItem].Style.Font.Color.SetColor(Color.White);
                worksheet.Row(count).Height = 38;

                hojaAdicionales.Cells[cellItem].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hojaAdicionales.Cells[cellItem].Style.Fill.BackgroundColor.SetColor(Color.Black);
                hojaAdicionales.Cells[cellItem].Style.Font.Color.SetColor(Color.White);
                hojaAdicionales.Row(count).Height = 38;


                filaAdicionales = count;

                count = count + 2;
                worksheet.Cells[1, 4].Value = count; //Value Start Data

                var countAdicional = 0;

                var iterar_hasta = datosAdicionales.Count + count;
                for (int i = count; i < iterar_hasta; i++)
                {
                    if (datosAdicionales[countAdicional].tipo == "Padre")
                    {
                        var cell = "E" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].nombre_actividad;
                        var range = "C" + i + ":AJ" + i;
                        worksheet.Cells[range].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[range].Style.Fill.BackgroundColor.SetColor(Color.Black);
                        worksheet.Cells[range].Style.Font.Color.SetColor(Color.White);
                        worksheet.Row(i).Height = 37;

                        hojaAdicionales.Cells[range].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        hojaAdicionales.Cells[range].Style.Fill.BackgroundColor.SetColor(Color.Black);
                        hojaAdicionales.Cells[range].Style.Font.Color.SetColor(Color.White);
                        hojaAdicionales.Row(i).Height = 37;


                    }
                    else if (datosAdicionales[countAdicional].tipo == "Actividad")
                    {
                        var cell = "E" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].nombre_actividad;
                        var range = "C" + i + ":AJ" + i;
                        worksheet.Cells[range].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[range].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        worksheet.Cells[range].Style.Font.Color.SetColor(Color.White);
                        worksheet.Row(i).Height = 32;


                        hojaAdicionales.Cells[range].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        hojaAdicionales.Cells[range].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        hojaAdicionales.Cells[range].Style.Font.Color.SetColor(Color.White);
                        hojaAdicionales.Row(i).Height = 32;


                    }
                    else
                    {
                        var cell = "C" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].codigo_preciario;
                        cell = "E" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].nombre_actividad;
                        worksheet.Row(i).Height = 31;
                        cell = "H" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].porcentaje_presupuesto_total; // porcentaje
                        worksheet.Cells[cell].Style.Numberformat.Format = "#0.000%";
                        cell = "I" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].porcentaje_costo_eac_total; // porcentaje
                        worksheet.Cells[cell].Style.Numberformat.Format = "#0.000%";
                        cell = "J" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].presupuesto_total;
                        worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        cell = "K" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].cantidad_eac;
                        worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        cell = "L" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].UM; //Todo
                        cell = "M" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].precio_unitario; //Todo
                        cell = "N" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].costo_presupuesto;
                        worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        cell = "O" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].costo_eac;
                        worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        cell = "P" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].ac_anterior;
                        worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        cell = "Q" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].ac_diario;
                        worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        cell = "R" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].ac_actual;
                        worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        cell = "S" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].ev_anterior;
                        worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        cell = "T" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].ev_diario;
                        worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        cell = "U" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].ev_actual;
                        worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        cell = "V" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].pv_costo_planificado;
                        worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        cell = "W" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].fecha_inicio_prevista; // Fecha
                        worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                        cell = "X" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].fecha_fin_prevista; // fecha
                        worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                        cell = "Y" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].fecha_inicio_real; //fecha
                        worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                        cell = "Z" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].fecha_fin_real; // fecha
                        worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                        cell = "AA" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].cantidad_anterior;
                        worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.000";
                        cell = "AB" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].cantidad_diaria;
                        worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.000";
                        cell = "AC" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].cantidad_acumulada;
                        worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.000";
                        cell = "AD" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].porcentaje_avance_anterior; // porcentaje
                        worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                        cell = "AE" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].porcentaje_avance_diario; // porcentaje
                        worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                        cell = "AF" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].porcentaje_avance_actual_acumulado; // porcentaje
                        worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                        cell = "AG" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].porcentaje_avance_previsto_acumulado; // porcentaje
                        worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                        cell = "AI" + i + "";
                        worksheet.Cells[cell].Value = datosAdicionales[countAdicional].ern_value; // porcentaje
                        worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.000";

                        cell = "AJ" + i + "";
                        decimal number = Decimal.Round(datos[countAdicional].porcentaje_avance_actual_acumulado - datos[countAdicional].porcentaje_avance_previsto_acumulado, 1);

                        worksheet.Cells[cell].Value = number; // porcentaje\\
                        double datavalue = decimal.ToDouble(number);

                        if (datavalue >= 0.0)
                        {
                            ExcelRange rng = worksheet.Cells[cell];
                            ExcelAddress address = new ExcelAddress(rng.Address);
                            var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                            v.ShowValue = false;
                            v.Icon2.Value = 1;
                            v.Icon3.Value = 2;
                            //  v.Icon2.Value = -0.05;
                            // v.Icon3.Value = 0.0;
                            v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                            v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                            v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;


                            //string formular = "=IF($AH$" + i + "= 0,0,ROUND(" + "$AG$" + i + "-$AH$" + i + ",1))";
                            //worksheet.Cells[indicador].Formula = formular;  // porcentaje
                            worksheet.Cells[cell].Value = 3;


                        }
                        else if (datavalue < 0.0 && datavalue >= -0.05)
                        {
                            ExcelRange rng = worksheet.Cells[cell];
                            ExcelAddress address = new ExcelAddress(rng.Address);
                            var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                            v.ShowValue = false;
                            v.Icon2.Value = 1;
                            v.Icon3.Value = 2;
                            //  v.Icon2.Value = -0.05;
                            // v.Icon3.Value = 0.0;
                            v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                            v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                            v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;


                            //string formular = "=IF($AH$" + i + "= 0,0,ROUND(" + "$AG$" + i + "-$AH$" + i + ",1))";
                            //worksheet.Cells[indicador].Formula = formular;  // porcentaje
                            worksheet.Cells[cell].Value = 1;

                        }
                        else
                        {
                            ExcelRange rng = worksheet.Cells[cell];
                            ExcelAddress address = new ExcelAddress(rng.Address);
                            var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                            v.ShowValue = false;
                            v.Icon2.Value = 1;
                            v.Icon3.Value = 2;
                            //  v.Icon2.Value = -0.05;
                            // v.Icon3.Value = 0.0;
                            v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                            v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                            v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;


                            //string formular = "=IF($AH$" + i + "= 0,0,ROUND(" + "$AG$" + i + "-$AH$" + i + ",1))";
                            //worksheet.Cells[indicador].Formula = formular;  // porcentaje
                            worksheet.Cells[cell].Value = 0;
                        }



                        /*COLUMNAS ADICIONALES AUXILIARES*/


                        /*var apu = new List<RdoDatos>();
                        apu.Add(datosAdicionales[countAdicional]);
                        var costo_eac_descuento = this.ObtenerTotalesEspecialidad(apu, "COSTO_EAC", ma.costoEAC);
                        var ac_actual_descuento = this.ObtenerTotalesEspecialidad(apu, "AC_ACTUAL", ma.ac_actual);
                        */
                        cell = "C" + i + "";

                        // hojaAdicionales.Cells[cell].Value = datosAdicionales[countAdicional].costo_eac - this.ObtenerDescuentoEspecialidadAPU(costo_eac_descuento, datosAdicionales[countAdicional]);
                        hojaAdicionales.Cells[cell].Value = datosAdicionales[countAdicional].costo_eac_descuento;

                        hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        cell = "D" + i + "";
                        hojaAdicionales.Cells[cell].Value = datosAdicionales[countAdicional].ac_actual_descuento;

                        //hojaAdicionales.Cells[cell].Value = datosAdicionales[countAdicional].ac_actual - this.ObtenerDescuentoEspecialidadAPU(ac_actual_descuento, datosAdicionales[countAdicional]);
                        hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    }



                    count++;
                    countAdicional++;
                }
                worksheet.Cells[1, 5].Value = count + 2; //ValueFin Adicionales
            }

            var ultimo_count = count;

            var filter_num_inicial = 15;
            var rango = "C" + filter_num_inicial + ":AJ" + ultimo_count;
            var dataCell = worksheet.Cells[rango];

            // Borde negro a la tabla de los datos
            worksheet.Cells[rango].Style.Border.BorderAround(ExcelBorderStyle.Medium, System.Drawing.Color.Black);
            if (second_format)
            {
                filter_num_inicial = filter_num_inicial + 2;
                rango = "C" + filter_num_inicial + ":AJ" + ultimo_count;
            }
            worksheet.Cells[rango].AutoFilter = true;



            /* Formato Contrato 2*/

            if (second_format) //Si es Segundo Formato
            {



                var lista = (from l in datos  // Valor Total de Descuentos solo aplican a items sin pendientes de aprobacion
                             where l.tipo != "Actividad"
                             where l.tipo != "Padre"
                             select l).ToList();

                var listaAdcionales = (from l in datosAdicionales
                                       where l.tipo != "Actividad"
                                       where l.tipo != "Padre"
                                       select l).ToList();
                lista.AddRange(listaAdcionales);

                if (lista.Count > 0)
                {

                    var costo_buget_e = this.ObtenerTotalesEspecialidad(lista, "COSTO_BUGET", m.costoBudget);
                    var costo_eac_e = this.ObtenerTotalesEspecialidad(lista, "COSTO_EAC", m.costoEAC);
                    var ac_anterior_e = this.ObtenerTotalesEspecialidad(lista, "AC_ANTERIOR", m.ac_anterior);
                    var ac_diario_e = this.ObtenerTotalesEspecialidad(lista, "AC_DIARIO", m.ac_diario);
                    var ac_actual_e = this.ObtenerTotalesEspecialidad(lista, "AC_ACTUAL", m.ac_actual);
                    var ev_anterior_e = this.ObtenerTotalesEspecialidad(lista, "EV_ANTERIOR", m.ev_anterior);
                    var ev_diario_e = this.ObtenerTotalesEspecialidad(lista, "EV_DIARIO", m.ev_diario);
                    var ev_actual_e = this.ObtenerTotalesEspecialidad(lista, "EV_ACTUAL", m.ev_actual);
                    var earn_value_e = this.ObtenerTotalesEspecialidad(lista, "EARN_VALUE", m.ern_value);
                    var pv_costo_panificado_e = this.ObtenerTotalesEspecialidad(lista, "PV_COSTO_PLANIFICADO", m.pv_costo_planificado);




                    count += 2;

                    var cell = "N" + count + "";
                    int rangoInicialValorDescuentos = count;
                    worksheet.Cells["L1"].Value = rangoInicialValorDescuentos;
                    worksheet.Cells["L1"].Style.Font.Color.SetColor(Color.White);

                    var rangocolor = "C" + count + ":AJ" + count;
                    var celltext = "E" + count;
                    worksheet.Cells[celltext].Value = "VALOR TOTAL COSTO DIRECTO OBRAS CIVILES";
                    worksheet.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(244, 176, 132));

                    worksheet.Cells[cell].Value = costo_buget_e.VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "O" + count + "";
                    worksheet.Cells[cell].Value = costo_eac_e.VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "C" + count + "";
                    hojaAdicionales.Cells[cell].Value = costo_eac_e.VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "P" + count + "";
                    worksheet.Cells[cell].Value = ac_anterior_e.VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "Q" + count + "";
                    worksheet.Cells[cell].Value = ac_diario_e.VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "R" + count + "";
                    worksheet.Cells[cell].Value = ac_actual_e.VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "D" + count + "";
                    hojaAdicionales.Cells[cell].Value = ac_actual_e.VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "S" + count + "";
                    worksheet.Cells[cell].Value = ev_anterior_e.VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "T" + count + "";
                    worksheet.Cells[cell].Value = ev_diario_e.VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "U" + count + "";
                    worksheet.Cells[cell].Value = ev_actual_e.VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "V" + count + "";
                    worksheet.Cells[cell].Value = pv_costo_panificado_e.VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "X" + count + "";
                    worksheet.Cells[cell].Value = earn_value_e.VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    int value = 15;


                    cell = "N" + value + "";
                    worksheet.Cells[cell].Value = costo_buget_e.TOTAL_DESCUENTO_APLICADO;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "O" + value + "";
                    worksheet.Cells[cell].Value = costo_eac_e.TOTAL_DESCUENTO_APLICADO;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "C" + value + "";
                    hojaAdicionales.Cells[cell].Value = costo_eac_e.TOTAL_DESCUENTO_APLICADO;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "P" + value + "";
                    worksheet.Cells[cell].Value = ac_anterior_e.TOTAL_DESCUENTO_APLICADO;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "Q" + value + "";
                    worksheet.Cells[cell].Value = ac_diario_e.TOTAL_DESCUENTO_APLICADO;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "R" + value + "";
                    worksheet.Cells[cell].Value = ac_actual_e.TOTAL_DESCUENTO_APLICADO;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "D" + value + "";
                    hojaAdicionales.Cells[cell].Value = ac_actual_e.TOTAL_DESCUENTO_APLICADO;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "S" + value + "";
                    worksheet.Cells[cell].Value = ev_anterior_e.TOTAL_DESCUENTO_APLICADO;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "T" + value + "";
                    worksheet.Cells[cell].Value = ev_diario_e.TOTAL_DESCUENTO_APLICADO;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "U" + value + "";
                    worksheet.Cells[cell].Value = ev_actual_e.TOTAL_DESCUENTO_APLICADO;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "V" + value + "";
                    worksheet.Cells[cell].Value = pv_costo_panificado_e.TOTAL_DESCUENTO_APLICADO;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    //cell = "AI" + count + "";
                    cell = "K" + 1 + ""; //CABECERA PRINCIPAL
                    worksheet.Cells[cell].Value = earn_value_e.TOTAL_DESCUENTO_APLICADO;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[cell].Style.Font.Color.SetColor(Color.White);


                    cell = "C" + value + ":D" + value;
                    hojaAdicionales.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    hojaAdicionales.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));
                    hojaAdicionales.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Medium);



                    count++;
                    rangocolor = "C" + count + ":AJ" + count;
                    celltext = "E" + count;
                    worksheet.Cells[celltext].Value = "DESCUENTO 0% ITEMS";
                    worksheet.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 228, 214));

                    cell = "N" + count + "";
                    worksheet.Cells[cell].Value = costo_buget_e.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "O" + count + "";
                    worksheet.Cells[cell].Value = costo_eac_e.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "C" + count + "";
                    hojaAdicionales.Cells[cell].Value = costo_eac_e.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";



                    cell = "P" + count + "";
                    worksheet.Cells[cell].Value = ac_anterior_e.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "Q" + count + "";
                    worksheet.Cells[cell].Value = ac_diario_e.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "R" + count + "";
                    worksheet.Cells[cell].Value = ac_actual_e.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "D" + count + "";
                    hojaAdicionales.Cells[cell].Value = ac_actual_e.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "S" + count + "";
                    worksheet.Cells[cell].Value = ev_anterior_e.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "T" + count + "";
                    worksheet.Cells[cell].Value = ev_diario_e.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "U" + count + "";
                    worksheet.Cells[cell].Value = ev_actual_e.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "V" + count + "";
                    worksheet.Cells[cell].Value = pv_costo_panificado_e.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "X" + count + "";
                    worksheet.Cells[cell].Value = earn_value_e.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    count++;
                    rangocolor = "C" + count + ":AJ" + count;
                    celltext = "E" + count;
                    worksheet.Cells[celltext].Value = "VALOR TOTAL COSTO DIRECTO OBRAS MECÁNICAS";
                    worksheet.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(112, 173, 71));

                    cell = "N" + count + "";
                    worksheet.Cells[cell].Value = costo_buget_e.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "O" + count + "";
                    worksheet.Cells[cell].Value = costo_eac_e.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "C" + count + "";
                    hojaAdicionales.Cells[cell].Value = costo_eac_e.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "P" + count + "";
                    worksheet.Cells[cell].Value = ac_anterior_e.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "Q" + count + "";
                    worksheet.Cells[cell].Value = ac_diario_e.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "R" + count + "";
                    worksheet.Cells[cell].Value = ac_actual_e.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "D" + count + "";
                    hojaAdicionales.Cells[cell].Value = ac_actual_e.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "S" + count + "";
                    worksheet.Cells[cell].Value = ev_anterior_e.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "T" + count + "";
                    worksheet.Cells[cell].Value = ev_diario_e.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "U" + count + "";
                    worksheet.Cells[cell].Value = ev_actual_e.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "V" + count + "";
                    worksheet.Cells[cell].Value = pv_costo_panificado_e.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "X" + count + "";
                    //cell = "E" + 1 + "";

                    worksheet.Cells[cell].Value = earn_value_e.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";



                    count++;
                    rangocolor = "C" + count + ":AJ" + count;
                    celltext = "E" + count;
                    worksheet.Cells[celltext].Value = "DESCUENTO 1% ITEMS";
                    worksheet.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(169, 208, 142));

                    cell = "N" + count + "";
                    worksheet.Cells[cell].Value = costo_buget_e.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "O" + count + "";
                    worksheet.Cells[cell].Value = costo_eac_e.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "C" + count + "";
                    hojaAdicionales.Cells[cell].Value = costo_eac_e.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * -1;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "P" + count + "";
                    worksheet.Cells[cell].Value = ac_anterior_e.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "Q" + count + "";
                    worksheet.Cells[cell].Value = ac_diario_e.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "R" + count + "";
                    worksheet.Cells[cell].Value = ac_actual_e.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "D" + count + "";
                    hojaAdicionales.Cells[cell].Value = ac_actual_e.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * -1;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "S" + count + "";
                    worksheet.Cells[cell].Value = ev_anterior_e.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "T" + count + "";
                    worksheet.Cells[cell].Value = ev_diario_e.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "U" + count + "";
                    worksheet.Cells[cell].Value = ev_actual_e.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "V" + count + "";
                    worksheet.Cells[cell].Value = pv_costo_panificado_e.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    /*EARN VALUE SAVE CABECERA*/
                    cell = "X" + count + "";
                    //cell = "E" + 1 + "";
                    worksheet.Cells[cell].Value = earn_value_e.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    count++;
                    rangocolor = "C" + count + ":AJ" + count;
                    celltext = "E" + count;
                    worksheet.Cells[celltext].Value = "VALOR TOTAL COSTO DIRECTO OBRAS ELÉCTRICAS";
                    worksheet.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(91, 155, 213));

                    cell = "N" + count + "";
                    worksheet.Cells[cell].Value = costo_buget_e.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "O" + count + "";
                    worksheet.Cells[cell].Value = costo_eac_e.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "C" + count + "";

                    hojaAdicionales.Cells[cell].Value = costo_eac_e.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "P" + count + "";
                    worksheet.Cells[cell].Value = ac_anterior_e.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "Q" + count + "";
                    worksheet.Cells[cell].Value = ac_diario_e.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "R" + count + "";
                    worksheet.Cells[cell].Value = ac_actual_e.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "D" + count + "";
                    hojaAdicionales.Cells[cell].Value = ac_actual_e.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "S" + count + "";
                    worksheet.Cells[cell].Value = ev_anterior_e.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "T" + count + "";
                    worksheet.Cells[cell].Value = ev_diario_e.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "U" + count + "";
                    worksheet.Cells[cell].Value = ev_actual_e.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "V" + count + "";
                    worksheet.Cells[cell].Value = pv_costo_panificado_e.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "X" + count + "";
                    worksheet.Cells[cell].Value = earn_value_e.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    count++;
                    rangocolor = "C" + count + ":AJ" + count;
                    celltext = "E" + count;
                    worksheet.Cells[celltext].Value = "DESCUENTO 1% ITEMS";
                    worksheet.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(189, 215, 238));

                    cell = "N" + count + "";
                    worksheet.Cells[cell].Value = costo_buget_e.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "O" + count + "";
                    worksheet.Cells[cell].Value = costo_eac_e.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "C" + count + "";
                    hojaAdicionales.Cells[cell].Value = costo_eac_e.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * -1;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "P" + count + "";
                    worksheet.Cells[cell].Value = ac_anterior_e.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "Q" + count + "";
                    worksheet.Cells[cell].Value = ac_diario_e.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "R" + count + "";
                    worksheet.Cells[cell].Value = ac_actual_e.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "D" + count + "";
                    hojaAdicionales.Cells[cell].Value = ac_actual_e.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * -1;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "S" + count + "";
                    worksheet.Cells[cell].Value = ev_anterior_e.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "T" + count + "";
                    worksheet.Cells[cell].Value = ev_diario_e.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "U" + count + "";
                    worksheet.Cells[cell].Value = ev_actual_e.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "V" + count + "";
                    worksheet.Cells[cell].Value = pv_costo_panificado_e.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "X" + count + "";
                    worksheet.Cells[cell].Value = earn_value_e.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    count++;
                    rangocolor = "C" + count + ":AJ" + count;
                    celltext = "E" + count;
                    worksheet.Cells[celltext].Value = "VALOR TOTAL COSTO DIRECTO OBRAS INSTRUMENTOS & CONTROL";
                    worksheet.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));

                    cell = "N" + count + "";
                    worksheet.Cells[cell].Value = costo_buget_e.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "O" + count + "";
                    worksheet.Cells[cell].Value = costo_eac_e.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "C" + count + "";
                    hojaAdicionales.Cells[cell].Value = costo_eac_e.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "P" + count + "";
                    worksheet.Cells[cell].Value = ac_anterior_e.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "Q" + count + "";
                    worksheet.Cells[cell].Value = ac_diario_e.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "R" + count + "";
                    worksheet.Cells[cell].Value = ac_actual_e.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "D" + count + "";
                    hojaAdicionales.Cells[cell].Value = ac_actual_e.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";



                    hojaAdicionales.Cells[cell].Value = ac_actual_e.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "S" + count + "";
                    worksheet.Cells[cell].Value = ev_anterior_e.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "T" + count + "";
                    worksheet.Cells[cell].Value = ev_diario_e.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "U" + count + "";
                    worksheet.Cells[cell].Value = ev_actual_e.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "V" + count + "";
                    worksheet.Cells[cell].Value = pv_costo_panificado_e.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "X" + count + "";
                    worksheet.Cells[cell].Value = earn_value_e.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    count++;
                    rangocolor = "C" + count + ":AJ" + count;
                    celltext = "E" + count;
                    worksheet.Cells[celltext].Value = "DESCUENTO 1% ITEMS";
                    worksheet.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 230, 153));
                    cell = "N" + count + "";
                    worksheet.Cells[cell].Value = costo_buget_e.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "O" + count + "";
                    worksheet.Cells[cell].Value = costo_eac_e.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "C" + count + "";
                    hojaAdicionales.Cells[cell].Value = costo_eac_e.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * -1;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "P" + count + "";
                    worksheet.Cells[cell].Value = ac_anterior_e.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "Q" + count + "";
                    worksheet.Cells[cell].Value = ac_diario_e.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "R" + count + "";
                    worksheet.Cells[cell].Value = ac_actual_e.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    cell = "D" + count + "";
                    hojaAdicionales.Cells[cell].Value = ac_actual_e.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * -1;
                    hojaAdicionales.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "S" + count + "";
                    worksheet.Cells[cell].Value = ev_anterior_e.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "T" + count + "";
                    worksheet.Cells[cell].Value = ev_diario_e.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "U" + count + "";
                    worksheet.Cells[cell].Value = ev_actual_e.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "V" + count + "";
                    worksheet.Cells[cell].Value = pv_costo_panificado_e.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                    cell = "X" + count + "";
                    worksheet.Cells[cell].Value = earn_value_e.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * -1;
                    worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";


                    int rangoFinalValorDescuentos = count;
                    worksheet.Cells["M1"].Value = rangoFinalValorDescuentos;
                    worksheet.Cells["M1"].Style.Font.Color.SetColor(Color.White);
                    for (var i = 0; i <= 7; i++)
                    {
                        worksheet.Row(rangoFinalValorDescuentos - i).OutlineLevel = 1;
                        worksheet.Row(rangoFinalValorDescuentos - i).Collapsed = true;
                    }

                }


            }



            var celdacurva = "A1";
            worksheet.Cells[celdacurva].Value = (count + 4);
            worksheet.Cells[celdacurva].Style.Font.Color.SetColor(Color.White);


            count = count + 33;

            // Actividades Realizadas
            count += 5;
            var celda = "C" + count + ":L" + count;
            worksheet.Cells[celda].Merge = true;
            worksheet.Cells[celda].Value = "REGISTRO DE CAMBIOS";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium, System.Drawing.Color.Black);
            worksheet.Row(count).Height = 38;
            var headerCells = worksheet.Cells[celda];
            var headerFont = headerCells.Style.Font;
            headerFont.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            celda = "Q" + count + ":AI" + count;
            worksheet.Cells[celda].Merge = true;
            worksheet.Cells[celda].Value = "REGISTRO DE HITOS DISRUPTIVOS";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            celda = "L" + count + ":AI" + count;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium, System.Drawing.Color.Black);
            count++;

            celda = "C" + count;
            worksheet.Cells[celda].Value = "N";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.Black);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Font.Bold = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

            celda = "D" + count + ":G" + count;
            worksheet.Cells[celda].Merge = true;
            worksheet.Cells[celda].Value = "DESCRIPCIÓN";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.Black);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Font.Bold = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

            celda = "H" + count + "";
            worksheet.Cells[celda].Merge = true;
            worksheet.Cells[celda].Value = "FECHA INICIO";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.Black);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Font.Bold = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

            celda = "I" + count + "";
            worksheet.Cells[celda].Merge = true;
            worksheet.Cells[celda].Value = "FECHA FIN";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.Black);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Font.Bold = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);


            celda = "J" + count;
            worksheet.Cells[celda].Value = "UNIDAD";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.Black);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Font.Bold = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

            celda = "K" + count;
            worksheet.Cells[celda].Value = "CANTIDAD ORIGINAL";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.Black);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Font.Bold = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

            celda = "L" + count;
            worksheet.Cells[celda].Value = "CANTIDAD ACTUAL";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.Black);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Font.Bold = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

            celda = "M" + count + ":X" + count;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);


            //DISRUPTIVOS

            celda = "W" + count + "";
            worksheet.Cells[celda].Merge = true;
            worksheet.Cells[celda].Value = "N";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.Black);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Font.Bold = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

            celda = "X" + count + ":Z" + count;
            worksheet.Cells[celda].Merge = true;
            worksheet.Cells[celda].Value = "DESCRIPCIÓN";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.Black);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Font.Bold = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);


            celda = "AA" + count + ":AC" + count;
            worksheet.Cells[celda].Merge = true;
            worksheet.Cells[celda].Value = "FECHA INICIO";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.Black);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Font.Bold = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);


            celda = "AD" + count + ":AF" + count;
            worksheet.Cells[celda].Merge = true;
            worksheet.Cells[celda].Value = "FECHA FIN";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.Black);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Font.Bold = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

            celda = "AG" + count + ":AI" + count;
            worksheet.Cells[celda].Merge = true;
            worksheet.Cells[celda].Value = "DÍAS TOTALES";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.Black);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Font.Bold = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

            worksheet.Row(count).Height = 70;
            worksheet.Row(count).Style.Font.Bold = true;
            count++;







            var lista_disruptivos = _obraDisruptivoRepository.GetAll()
                                                .Where(c => c.vigente)
                                                .Where(c => c.ProyectoId == cabecera.ProyectoId)
                                                              .Where(c => c.Proyecto.vigente).ToList();


            var precipitaciones = _precipitacionRepository.GetAll()
                                                .Where(c => c.vigente)
                                                .Where(c => c.ProyectoId == cabecera.ProyectoId).ToList();

            if (lista_disruptivos.Count > 0)
            {
                var cont = 1;
                foreach (var dis in lista_disruptivos)
                {

                    celda = "W" + count + "";
                    worksheet.Cells[celda].Merge = true;
                    worksheet.Cells[celda].Value = cont;
                    worksheet.Cells[celda].Style.WrapText = true;
                    worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                    celda = "X" + count + ":Z" + count;
                    worksheet.Cells[celda].Merge = true;
                    worksheet.Cells[celda].Value = dis.observaciones;
                    worksheet.Cells[celda].Style.WrapText = true;
                    worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                    celda = "AA" + count + ":AC" + count;
                    worksheet.Cells[celda].Merge = true;
                    worksheet.Cells[celda].Value = dis.fecha_inicio.HasValue ? dis.fecha_inicio.Value.ToShortDateString() : "";
                    worksheet.Cells[celda].Style.WrapText = true;
                    worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                    celda = "AD" + count + ":AF" + count;
                    worksheet.Cells[celda].Merge = true;
                    worksheet.Cells[celda].Value = dis.fecha_fin.HasValue ? dis.fecha_fin.Value.ToShortDateString() : cabecera.fecha_rdo.ToShortDateString();
                    worksheet.Cells[celda].Style.WrapText = true;
                    worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);


                    celda = "AG" + count + ":AI" + count;
                    worksheet.Cells[celda].Merge = true;
                    // worksheet.Cells[celda].Value = dis.fecha_inicio.HasValue &&dis.fecha_fin.HasValue?(dis.fecha_fin.Value-dis.fecha_inicio.Value).Days:0;
                    int dias = 0;
                    /*
                     Numero de Horas
                     * if (dis.numero_horas_hombres > 0)
                    {
                        TimeSpan timespan = TimeSpan.FromHours(Convert.ToDouble((dis.numero_horas_hombres / 10) * 24));
                        dias = timespan.Days;
                    }*/

                    if (dis.fecha_fin.HasValue)
                    {
                        dias = (dis.fecha_fin.Value - dis.fecha_inicio.Value).Days + 1;
                    }
                    else
                    {
                        dias = (cabecera.fecha_rdo - dis.fecha_inicio.Value).Days + 1;
                    }


                    worksheet.Cells[celda].Value = dias;// dis.numero_horas_hombres;
                    worksheet.Cells[celda].Style.WrapText = true;
                    worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);


                    celda = "C" + count + ":L" + count;
                    worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                    celda = "M" + count + ":X" + count;
                    worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                    count++;
                    cont++;
                }

            }

            if (precipitaciones.Count > 0)
            {
                celda = "W" + count + "";
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = "";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                worksheet.Cells[celda].Style.Font.Bold = true;
                worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                celda = "X" + count + ":Z" + count;
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = "PRECIPITACIONES REGISTRADAS: " + cabecera.Proyecto != null ? cabecera.Proyecto.nombre_proyecto : "";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                worksheet.Cells[celda].Style.Font.Bold = true;
                worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                celda = "AA" + count + ":AC" + count;
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = "PREVIO";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                worksheet.Cells[celda].Style.Font.Bold = true;
                worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                celda = "AD" + count + ":AF" + count;
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = "HOY";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                worksheet.Cells[celda].Style.Font.Bold = true;
                worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                celda = "AG" + count + ":AI" + count;
                worksheet.Cells[celda].Merge = true;
                // worksheet.Cells[celda].Value = dis.fecha_inicio.HasValue &&dis.fecha_fin.HasValue?(dis.fecha_fin.Value-dis.fecha_inicio.Value).Days:0;
                worksheet.Cells[celda].Value = "ACUMULADO";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                worksheet.Cells[celda].Style.Font.Bold = true;
                worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                celda = "C" + count + ":L" + count;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                celda = "M" + count + ":X" + count;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                count++;



                celda = "W" + count + "";
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = "";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                worksheet.Cells[celda].Style.Font.Bold = true;
                worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                celda = "X" + count + ":Z" + count;
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = "Total Precipitaciones";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                worksheet.Cells[celda].Style.Font.Bold = true;
                worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                celda = "AA" + count + ":AC" + count;
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = (from p in precipitaciones where p.Fecha.Date < cabecera.fecha_rdo.Date select p.CantidadDiaria).Sum() + " mm";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                worksheet.Cells[celda].Style.Font.Bold = true;
                worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                celda = "AD" + count + ":AF" + count;
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = (from p in precipitaciones where p.Fecha.Date == cabecera.fecha_rdo.Date select p.CantidadDiaria).Sum() + " mm";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                worksheet.Cells[celda].Style.Font.Bold = true;
                worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                celda = "AG" + count + ":AI" + count;
                worksheet.Cells[celda].Merge = true;
                // worksheet.Cells[celda].Value = dis.fecha_inicio.HasValue &&dis.fecha_fin.HasValue?(dis.fecha_fin.Value-dis.fecha_inicio.Value).Days:0;
                worksheet.Cells[celda].Value = (from p in precipitaciones where p.Fecha.Date <= cabecera.fecha_rdo.Date select p.CantidadDiaria).Sum() + " mm";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                worksheet.Cells[celda].Style.Font.Bold = true;
                worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                celda = "C" + count + ":L" + count;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                celda = "M" + count + ":X" + count;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                count++;

                celda = "W" + count + "";
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = "1";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                celda = "X" + count + ":Z" + count;
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = "Precipitaciones Diurnas ";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                celda = "AA" + count + ":AC" + count;
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = (from p in precipitaciones where p.Tipo == TipoPrecipitacion.Diurna where p.Fecha.Date < cabecera.fecha_rdo.Date select p.CantidadDiaria).Sum() + " mm";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                celda = "AD" + count + ":AF" + count;
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = (from p in precipitaciones where p.Tipo == TipoPrecipitacion.Diurna where p.Fecha.Date == cabecera.fecha_rdo.Date select p.CantidadDiaria).Sum() + " mm";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);


                celda = "AG" + count + ":AI" + count;
                worksheet.Cells[celda].Merge = true;
                // worksheet.Cells[celda].Value = dis.fecha_inicio.HasValue &&dis.fecha_fin.HasValue?(dis.fecha_fin.Value-dis.fecha_inicio.Value).Days:0;
                worksheet.Cells[celda].Value = (from p in precipitaciones where p.Tipo == TipoPrecipitacion.Diurna where p.Fecha.Date <= cabecera.fecha_rdo.Date select p.CantidadDiaria).Sum() + " mm";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);


                celda = "C" + count + ":L" + count;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                celda = "M" + count + ":X" + count;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                count++;
                celda = "W" + count + "";
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = "2";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                celda = "X" + count + ":Z" + count;
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = "Precipitaciones Nocturnas ";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                celda = "AA" + count + ":AC" + count;
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = (from p in precipitaciones where p.Tipo == TipoPrecipitacion.Norturna where p.Fecha.Date < cabecera.fecha_rdo.Date select p.CantidadDiaria).Sum() + " mm";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                celda = "AD" + count + ":AF" + count;
                worksheet.Cells[celda].Merge = true;
                worksheet.Cells[celda].Value = (from p in precipitaciones where p.Tipo == TipoPrecipitacion.Norturna where p.Fecha.Date == cabecera.fecha_rdo.Date select p.CantidadDiaria).Sum() + " mm";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);


                celda = "AG" + count + ":AI" + count;
                worksheet.Cells[celda].Merge = true;
                // worksheet.Cells[celda].Value = dis.fecha_inicio.HasValue &&dis.fecha_fin.HasValue?(dis.fecha_fin.Value-dis.fecha_inicio.Value).Days:0;
                worksheet.Cells[celda].Value = (from p in precipitaciones where p.Tipo == TipoPrecipitacion.Norturna where p.Fecha.Date <= cabecera.fecha_rdo.Date select p.CantidadDiaria).Sum() + " mm";
                worksheet.Cells[celda].Style.WrapText = true;
                worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);


                celda = "C" + count + ":L" + count;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                celda = "M" + count + ":X" + count;
                worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                count++;


            }




            celda = "C" + count + ":L" + count;
            worksheet.Cells[celda].Merge = true;
            worksheet.Cells[celda].Value = "ACTIVIDADES REALIZADAS";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium, System.Drawing.Color.Black);
            worksheet.Row(count).Height = 38;
            headerCells = worksheet.Cells[celda];
            headerFont = headerCells.Style.Font;
            headerFont.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            celda = "Q" + count + ":AI" + count;
            worksheet.Cells[celda].Merge = true;
            worksheet.Cells[celda].Value = "ACTIVIDADES PROGRAMADAS";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            celda = "L" + count + ":AI" + count;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium, System.Drawing.Color.Black);
            count++;

            var contadoractrealizadas = actividadesrealizadas.Count;
            var contadoractprogramadas = actividadesprogramadas.Count;

            if (contadoractrealizadas >= contadoractprogramadas)
            {
                var fila = count;
                foreach (var actividad in actividadesrealizadas)
                {
                    celda = "C" + count + ":L" + count;
                    worksheet.Cells[celda].Merge = true;
                    worksheet.Cells[celda].Style.WrapText = true;
                    worksheet.Cells[celda].Value = actividad.Observacion;
                    worksheet.Row(count).Height = 38;
                    worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    celda = "L" + count + ":AI" + count;
                    worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    count++;
                }
                foreach (var actividad in actividadesprogramadas)
                {
                    celda = "X" + fila + ":AI" + fila;
                    worksheet.Cells[celda].Merge = true;
                    worksheet.Cells[celda].Style.WrapText = true;
                    worksheet.Cells[celda].Value = actividad.Observacion;
                    fila++;
                }
            }
            else
            {
                var fila = count;
                foreach (var actividad in actividadesprogramadas)
                {
                    celda = "X" + count + ":AI" + count;
                    worksheet.Cells[celda].Merge = true;
                    worksheet.Cells[celda].Style.WrapText = true;
                    worksheet.Cells[celda].Value = actividad.Observacion;
                    worksheet.Row(count).Height = 38;
                    celda = "M" + count + ":AI" + count;
                    worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                    celda = "C" + count + ":L" + count;
                    worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    count++;
                }
                foreach (var actividad in actividadesrealizadas)
                {
                    celda = "C" + fila + ":L" + fila;
                    worksheet.Cells[celda].Merge = true;
                    worksheet.Cells[celda].Style.WrapText = true;
                    worksheet.Cells[celda].Value = actividad.Observacion;
                    fila++;
                }
            }

            celda = "C" + count + ":L" + count;
            worksheet.Cells[celda].Merge = true;
            worksheet.Cells[celda].Value = "OBSERVACIONES CPP ";// + cabecera.Proyecto.Contrato.Cliente.razon_social;
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium, System.Drawing.Color.Black);
            worksheet.Row(count).Height = 38;
            headerCells = worksheet.Cells[celda];
            headerFont = headerCells.Style.Font;
            headerFont.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            celda = "Q" + count + ":AI" + count;
            worksheet.Cells[celda].Merge = true;
            worksheet.Cells[celda].Value = "OBSERVACIONES SHAYA S.A";
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            celda = "L" + count + ":AI" + count;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium, System.Drawing.Color.Black);
            count++;

            var ObservacionesInternas = observaciones.Where(c => c.Tipo == TipoComentario.Observacion).Where(c => c.Codigo == CatalogosCodigos.EMPRESAINTERNA).ToList();
            var ObservacionesExternas = observaciones.Where(c => c.Tipo == TipoComentario.Observacion).Where(c => c.Codigo == CatalogosCodigos.EMPRESAEXTERNA).ToList();
            var contadorobservacionesin = ObservacionesInternas.Count;
            var contadorobservacionesext = ObservacionesExternas.Count;

            if (contadorobservacionesin >= contadorobservacionesext)
            {
                var fila = count;
                foreach (var observacion in ObservacionesInternas)
                {
                    celda = "C" + count + ":L" + count;
                    worksheet.Cells[celda].Merge = true;
                    worksheet.Cells[celda].Style.WrapText = true;
                    worksheet.Cells[celda].Value = observacion.FechaObservacion.ToShortDateString() + "-" + observacion.Observacion;
                    worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Row(count).Height = 45;

                    celda = "L" + count + ":AI" + count;
                    worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    count++;
                }
                foreach (var observacion in ObservacionesExternas)
                {
                    celda = "X" + fila + ":AI" + fila;
                    worksheet.Cells[celda].Merge = true;
                    worksheet.Cells[celda].Style.WrapText = true;
                    worksheet.Cells[celda].Value = observacion.FechaObservacion.ToShortDateString() + "-" + observacion.Observacion;
                    fila++;
                }
            }
            else
            {
                var fila = count;
                foreach (var observacion in ObservacionesExternas)
                {
                    celda = "X" + count + ":AI" + count;
                    worksheet.Cells[celda].Merge = true;
                    worksheet.Cells[celda].Style.WrapText = true;
                    worksheet.Cells[celda].Value = observacion.FechaObservacion.ToShortDateString() + "-" + observacion.Observacion;
                    worksheet.Row(count).Height = 45;
                    worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    celda = "M" + count + ":AI" + count;
                    worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);


                    celda = "C" + count + ":L" + count;
                    worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    count++;
                }
                foreach (var observacion in ObservacionesInternas)
                {
                    celda = "C" + fila + ":L" + fila;
                    worksheet.Cells[celda].Merge = true;
                    worksheet.Cells[celda].Style.WrapText = true;
                    worksheet.Cells[celda].Value = observacion.FechaObservacion.ToShortDateString() + "-" + observacion.Observacion;
                    fila++;
                }
            }
            count++;
            //FOTOGRAFICO
            celda = "C" + count + ":AJ" + count;
            worksheet.Cells[celda].Merge = true;
            worksheet.Cells[celda].Value = "REGISTRO FOTOGRÁFICO";// + cabecera.Proyecto.Contrato.Cliente.razon_social;
            worksheet.Cells[celda].Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));
            worksheet.Cells[celda].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[celda].Style.WrapText = true;
            worksheet.Cells[celda].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(146, 208, 80));
            worksheet.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium, System.Drawing.Color.Black);
            worksheet.Row(count).Height = 38;
            count++;


            ultimo_count = count;
            rango = "C17" + ":AJ" + ultimo_count;
            dataCell = worksheet.Cells[rango];

            // Fuente regular a la tabla de datos
            var dataCellFont = dataCell.Style.Font;
            dataCellFont.SetFromFont(new Font("Arial", 18, FontStyle.Regular));

            // Estilo a la columna de las actividades
            //worksheet.Column(5).Style.Font.SetFromFont(new Font("Arial", 18, FontStyle.Bold));


            var ultimafila = worksheet.Dimension.End.Row + 2;



            var fotos_rdo = _archivoAvanceObraRepository.GetAllIncluding(c => c.Archivo, c => c.AvanceObra.Oferta)
                                                              .Where(c => c.AvanceObra.fecha_presentacion == cabecera.fecha_rdo)
                                                              .Where(c => c.AvanceObra.aprobado)
                                                              .Where(c => c.AvanceObra.vigente)
                                                              .Where(c => c.Archivo.vigente)
                                                              .Where(c => c.AvanceObra.Oferta.es_final)
                                                              .Where(c => c.AvanceObra.Oferta.vigente)
                                                              .Where(c => c.AvanceObra.Oferta.ProyectoId == cabecera.ProyectoId)
                                                                .Where(c => c.vigente).ToList();


            if (fotos_rdo.Count > 0)
            {
                string descripcion = "";
                var numberfotos = 1;

                var totalfotos = fotos_rdo.Count;

                if (totalfotos == 1)
                {

                    foreach (var foto in fotos_rdo.OrderByDescending(c => c.AvanceObra.fecha_presentacion))
                    {
                        if (numberfotos <= 1)
                        {
                            MemoryStream ms = new MemoryStream(foto.Archivo.hash);
                            System.Drawing.Image Imagen = System.Drawing.Image.FromStream(ms);

                            using (MemoryStream imgStream = new MemoryStream())
                            {

                                Imagen.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);

                                imgStream.Seek(0, SeekOrigin.Begin);

                                var picture = worksheet.Drawings.AddPicture("_AVCF" + numberfotos, Imagen);
                                if (numberfotos == 1)
                                {
                                    picture.SetSize(800, 700);
                                    picture.SetPosition(ultimafila, 0, 16, 0);
                                    string param = "Q" + (ultimafila + 40) + ":U" + (ultimafila + 40);
                                    worksheet.Cells[param].Merge = true;
                                    worksheet.Cells[param].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[param].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    worksheet.Cells[param].Value = foto.descripcion;
                                    worksheet.Cells[param].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                                }
                                else
                                {
                                    picture.SetSize(800, 700);
                                    picture.SetPosition(ultimafila, 0, 18, 0);

                                    string param = "S" + (ultimafila + 40) + ":V" + (ultimafila + 40);
                                    worksheet.Cells[param].Merge = true;
                                    worksheet.Cells[param].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[param].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    worksheet.Cells[param].Value = foto.descripcion;
                                    worksheet.Cells[param].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                                }
                            }
                        }
                        numberfotos = numberfotos + 1;
                        descripcion = foto.descripcion;
                    }




                }
                if (totalfotos == 2)
                {

                    foreach (var foto in fotos_rdo.OrderByDescending(c => c.AvanceObra.fecha_presentacion))
                    {
                        if (numberfotos <= 2)
                        {
                            MemoryStream ms = new MemoryStream(foto.Archivo.hash);
                            System.Drawing.Image Imagen = System.Drawing.Image.FromStream(ms);

                            using (MemoryStream imgStream = new MemoryStream())
                            {

                                Imagen.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);

                                imgStream.Seek(0, SeekOrigin.Begin);

                                var picture = worksheet.Drawings.AddPicture("_AVCF" + numberfotos, Imagen);
                                if (numberfotos == 1)
                                {
                                    picture.SetSize(800, 700);
                                    picture.SetPosition(ultimafila, 0, 6, 0);
                                    string param = "G" + (ultimafila + 40) + ":J" + (ultimafila + 40);
                                    worksheet.Cells[param].Merge = true;
                                    worksheet.Cells[param].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[param].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    worksheet.Cells[param].Value = foto.descripcion;
                                    worksheet.Cells[param].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                                }
                                else
                                {
                                    picture.SetSize(800, 700);
                                    picture.SetPosition(ultimafila, 0, 18, 0);

                                    string param = "S" + (ultimafila + 40) + ":Y" + (ultimafila + 40);
                                    worksheet.Cells[param].Merge = true;
                                    worksheet.Cells[param].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[param].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    worksheet.Cells[param].Value = foto.descripcion;
                                    worksheet.Cells[param].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                                }
                            }
                        }
                        numberfotos = numberfotos + 1;
                        descripcion = foto.descripcion;
                    }


                }
                if (totalfotos == 3)
                {

                    foreach (var foto in fotos_rdo.OrderByDescending(c => c.AvanceObra.fecha_presentacion))
                    {
                        if (numberfotos <= 3)
                        {
                            MemoryStream ms = new MemoryStream(foto.Archivo.hash);
                            System.Drawing.Image Imagen = System.Drawing.Image.FromStream(ms);

                            using (MemoryStream imgStream = new MemoryStream())
                            {

                                Imagen.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);

                                imgStream.Seek(0, SeekOrigin.Begin);

                                var picture = worksheet.Drawings.AddPicture("_AVCF" + numberfotos, Imagen);
                                if (numberfotos == 1)
                                {
                                    picture.SetSize(800, 700);
                                    picture.SetPosition(ultimafila, 0, 6, 0);
                                    string param = "G" + (ultimafila + 40) + ":J" + (ultimafila + 40);
                                    worksheet.Cells[param].Merge = true;
                                    worksheet.Cells[param].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[param].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    worksheet.Cells[param].Value = foto.descripcion;
                                    worksheet.Cells[param].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                                }
                                if (numberfotos == 2)
                                {
                                    picture.SetSize(800, 700);
                                    picture.SetPosition(ultimafila, 0, 16, 0);
                                    string param = "Q" + (ultimafila + 40) + ":T" + (ultimafila + 40);
                                    worksheet.Cells[param].Merge = true;
                                    worksheet.Cells[param].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[param].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    worksheet.Cells[param].Value = foto.descripcion;
                                    worksheet.Cells[param].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                                }
                                if (numberfotos == 3)
                                {
                                    picture.SetSize(800, 700);
                                    picture.SetPosition(ultimafila, 0, 24, 0);

                                    string param = "Y" + (ultimafila + 40) + ":AC" + (ultimafila + 40);
                                    worksheet.Cells[param].Merge = true;
                                    worksheet.Cells[param].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[param].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    worksheet.Cells[param].Value = foto.descripcion;
                                    worksheet.Cells[param].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                                }
                            }
                        }
                        numberfotos = numberfotos + 1;
                        descripcion = foto.descripcion;
                    }


                }
                if (totalfotos >= 4)
                {

                    foreach (var foto in fotos_rdo.OrderByDescending(c => c.AvanceObra.fecha_presentacion))
                    {
                        if (numberfotos <= 4)
                        {
                            MemoryStream ms = new MemoryStream(foto.Archivo.hash);
                            System.Drawing.Image Imagen = System.Drawing.Image.FromStream(ms);

                            using (MemoryStream imgStream = new MemoryStream())
                            {

                                Imagen.Save(imgStream, System.Drawing.Imaging.ImageFormat.Bmp);

                                imgStream.Seek(0, SeekOrigin.Begin);

                                var picture = worksheet.Drawings.AddPicture("_AVCF" + numberfotos, Imagen);
                                if (numberfotos == 1)
                                {
                                    picture.SetSize(800, 700);
                                    picture.SetPosition(ultimafila, 0, 6, 0);
                                    string param = "G" + (ultimafila + 40) + ":J" + (ultimafila + 40);
                                    worksheet.Cells[param].Merge = true;
                                    worksheet.Cells[param].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[param].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    worksheet.Cells[param].Value = foto.descripcion;
                                    worksheet.Cells[param].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                                }
                                if (numberfotos == 2)
                                {
                                    picture.SetSize(800, 700);
                                    picture.SetPosition(ultimafila, 0, 13, 0);
                                    string param = "N" + (ultimafila + 40) + ":Q" + (ultimafila + 40);
                                    worksheet.Cells[param].Merge = true;
                                    worksheet.Cells[param].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[param].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    worksheet.Cells[param].Value = foto.descripcion;
                                    worksheet.Cells[param].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                                }
                                if (numberfotos == 3)
                                {
                                    picture.SetSize(800, 700);
                                    picture.SetPosition(ultimafila, 0, 19, 0);

                                    string param = "T" + (ultimafila + 40) + ":W" + (ultimafila + 40);
                                    worksheet.Cells[param].Merge = true;
                                    worksheet.Cells[param].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[param].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    worksheet.Cells[param].Value = foto.descripcion;
                                    worksheet.Cells[param].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                                }
                                if (numberfotos == 4)
                                {
                                    picture.SetSize(800, 700);
                                    picture.SetPosition(ultimafila, 0, 25, 0);

                                    string param = "Z" + (ultimafila + 40) + ":AC" + (ultimafila + 40);
                                    worksheet.Cells[param].Merge = true;
                                    worksheet.Cells[param].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[param].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    worksheet.Cells[param].Value = foto.descripcion;
                                    worksheet.Cells[param].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
                                }
                            }
                        }
                        numberfotos = numberfotos + 1;
                        descripcion = foto.descripcion;
                    }


                }

                ultimafila = ultimafila + 43;

            }

            var inicial = ultimafila - 2;
            ultimafila = ultimafila + 15;

            worksheet.Cells["C" + ultimafila].Value = "FIRMAS DE RESPONSABILIDAD";
            worksheet.Cells["C" + ultimafila].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));

            worksheet.Cells["C" + ultimafila + ":AJ" + ultimafila].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells["C" + ultimafila + ":AJ" + ultimafila].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            worksheet.Cells["C" + ultimafila + ":AJ" + ultimafila].Style.Border.BorderAround(ExcelBorderStyle.Medium, System.Drawing.Color.Black);

            ultimafila = ultimafila + 3;
            worksheet.Cells["C" + ultimafila].Value = "FIRMA";
            worksheet.Row(ultimafila).Height = 40;
            worksheet.Cells["C" + ultimafila].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            worksheet.Cells["Y" + ultimafila].Value = "FIRMA";
            worksheet.Cells["Y" + ultimafila].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            ultimafila = ultimafila + 1;
            worksheet.Cells["C" + ultimafila].Value = "NOMBRE";
            worksheet.Cells["C" + ultimafila].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            worksheet.Cells["Y" + ultimafila].Value = "NOMBRE";
            worksheet.Cells["Y" + ultimafila].Style.Font.SetFromFont(new Font("Arial", 16, FontStyle.Bold));
            worksheet.Row(ultimafila).Height = 40;
            ultimafila = ultimafila + 2;
            worksheet.Row(ultimafila).Height = 40;
            for (int i = inicial; i < ultimafila; i++)
            {
                worksheet.Row(i).Height = 40;
            }


            worksheet.Cells["G" + ultimafila].Value = "CPP";
            worksheet.Cells["G" + ultimafila].Style.Font.SetFromFont(new Font("Arial", 20, FontStyle.Bold));

            worksheet.Cells["AC" + ultimafila].Value = "SHAYA ECUADOR S.A.";
            worksheet.Cells["AC" + ultimafila].Style.Font.SetFromFont(new Font("Arial", 20, FontStyle.Bold));

            worksheet.Cells["C" + ultimafila + ":AJ" + ultimafila].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

            // Borde contorno del documento
            rango = "B2:AJ" + (ultimafila + 2);
            worksheet.Cells[rango].Style.Border.BorderAround(ExcelBorderStyle.Medium, System.Drawing.Color.Black);


            ///////////////EAR VALUE SOLUTION ////////////////////

            string ultimo = worksheet.Cells[1, worksheet.Dimension.End.Column].Address;

            worksheet.Cells[17, worksheet.Dimension.End.Column - 2, datos.Count + 17, worksheet.Dimension.End.Column - 2]
            .Style.Border.Right.Style = ExcelBorderStyle.Medium;
            worksheet.InsertColumn(22, 1, 21);

            worksheet.Column(22).Width = 27;
            worksheet.Cells["V13:V14"].Merge = true;
            worksheet.Cells["V13:V14"].Value = "Earn Value (EAC)";
            worksheet.Cells["V13:V14"].Style.WrapText = true;
            worksheet.Cells["V13:V14"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["V13:V14"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            worksheet.Cells["V13:V14"].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            worksheet.Cells["V13:V14"].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells["V13:V14"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["V13:V14"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["V13:V14"].Style.Font.Size = 18;
            worksheet.Cells["V13:V14"].Style.Font.Bold = true;

            if (filaAdicionales > 0)
            {
                /*ADICIONALES*/
                #region Montos TotalesAdicionales
                string cellAdicional = "H" + filaAdicionales;
                /*Porcentaje Budget
                worksheet.Cells[cellAdicional].Value = ma.porcentajeBudget;
                worksheet.Cells[cellAdicional].Style.Numberformat.Format = "#0.00%";
                worksheet.Cells[cellAdicional].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[cellAdicional].Style.Font.Size = 17;*/

                cellAdicional = "I" + filaAdicionales;
                /*Porcentaje EAc
                worksheet.Cells[cellAdicional].Value = ma.porcentajeEAC;
                worksheet.Cells[cellAdicional].Style.Numberformat.Format = "#0.00%";
                worksheet.Cells[cellAdicional].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[cellAdicional].Style.Font.Size = 17;*/

                cellAdicional = "N" + filaAdicionales;
                //Costo Budget
                worksheet.Cells[cellAdicional].Value = ma.costoBudget;

                cellAdicional = "O" + filaAdicionales;
                //Costo EAc
                worksheet.Cells[cellAdicional].Value = ma.costoEAC;

                cellAdicional = "P" + filaAdicionales;
                //Ac Anterior
                worksheet.Cells[cellAdicional].Value = ma.ac_anterior;

                //Ac diario
                cellAdicional = "Q" + filaAdicionales;
                worksheet.Cells[cellAdicional].Value = ma.ac_diario;

                //Ac Actual
                cellAdicional = "R" + filaAdicionales;
                worksheet.Cells[cellAdicional].Value = ma.ac_actual;

                //ev anterior
                cellAdicional = "S" + filaAdicionales;
                worksheet.Cells[cellAdicional].Value = ma.ev_anterior;

                //ev diario
                cellAdicional = "T" + filaAdicionales;
                worksheet.Cells[cellAdicional].Value = ma.ev_diario;
                //ev actual
                cellAdicional = "U" + filaAdicionales;
                worksheet.Cells[cellAdicional].Value = ma.ev_actual;

                cellAdicional = "V" + filaAdicionales;
                //worksheet.Cells[cellAdicional].Value = ma.ern_value;

                // pv costos planificado
                cellAdicional = "W" + filaAdicionales;
                worksheet.Cells[cellAdicional].Value = ma.pv_costo_planificado;

                for (int i = 14; i <= 23; i++)
                {
                    worksheet.Cells[filaAdicionales, i].Style.WrapText = true;
                    worksheet.Cells[filaAdicionales, i].Style.Font.Size = 17;
                    worksheet.Cells[filaAdicionales, i].Style.Numberformat.Format = "#,##0.00";
                }
                for (int i = 31; i <= 34; i++)
                {
                    worksheet.Cells[filaAdicionales, i].Style.WrapText = true;
                    worksheet.Cells[filaAdicionales, i].Style.Font.Size = 17;
                    worksheet.Cells[filaAdicionales, i].Style.Numberformat.Format = "#0.00%";

                }


                // fecha inicio previsto
                cellAdicional = "X" + filaAdicionales;
                worksheet.Cells[cellAdicional].Value = ma.fecha_inicio_prevista;
                worksheet.Cells[cellAdicional].Style.WrapText = true;
                worksheet.Cells[cellAdicional].Style.Font.Size = 17;
                worksheet.Cells[cellAdicional].Style.Numberformat.Format = "dd-mmm-yy";

                //fecha fin previsto
                cellAdicional = "Y" + filaAdicionales;
                worksheet.Cells[cellAdicional].Value = ma.fecha_fin_prevista;
                worksheet.Cells[cellAdicional].Style.WrapText = true;
                worksheet.Cells[cellAdicional].Style.Font.Size = 17;
                worksheet.Cells[cellAdicional].Style.Numberformat.Format = "dd-mmm-yy";



                // fecha inicio previsto
                cellAdicional = "Z" + filaAdicionales;
                var fechatemp = ma.fecha_inicio_real != new DateTime(1999, 01, 01) ? "" + ma.fecha_inicio_real.Value.ToString("dd-MMM-yy") : "";
                worksheet.Cells[cellAdicional].Value = fechatemp;
                worksheet.Cells[cellAdicional].Style.WrapText = true;
                worksheet.Cells[cellAdicional].Style.Font.Size = 17;
                worksheet.Cells[cellAdicional].Style.Numberformat.Format = "dd-mmm-yy";


                if (ma.avance_Actual_Acumulado == 1)
                {
                    cellAdicional = "AA" + filaAdicionales;
                    //fecha fin previsto
                    if (ma.fecha_fin_real == new DateTime(1999, 01, 01))
                    {

                        worksheet.Cells[cellAdicional].Value = "";
                    }
                    else
                    {
                        worksheet.Cells[cellAdicional].Value = ma.fecha_fin_real;
                    }

                    worksheet.Cells[cellAdicional].Style.WrapText = true;
                    worksheet.Cells[cellAdicional].Style.Font.Size = 17;
                    worksheet.Cells[cellAdicional].Style.Numberformat.Format = "dd-mmm-yy";

                }

                // Avance Acumulado Anterior 
                cellAdicional = "AE" + filaAdicionales;
                worksheet.Cells[cellAdicional].Value = ma.avance_Acumulado_Anterior;

                // Avance Diario
                cellAdicional = "AF" + filaAdicionales;
                worksheet.Cells[cellAdicional].Value = ma.avance_Diario;

                // Avance Actual Acumulado
                cellAdicional = "AG" + filaAdicionales;
                worksheet.Cells[cellAdicional].Value = ma.avance_Actual_Acumulado;


                // Avance Previsto Acumulado
                cellAdicional = "AH" + filaAdicionales;
                worksheet.Cells[cellAdicional].Value = ma.Avance_Previsto_Acumulado;


                cellAdicional = "C" + filaAdicionales + ":AJ" + filaAdicionales;
                worksheet.Cells[cellAdicional].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[cellAdicional].Style.Fill.BackgroundColor.SetColor(Color.Black);
                worksheet.Cells[cellAdicional].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[cellAdicional].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[cellAdicional].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                #endregion
            }


            worksheet.Cells[17, worksheet.Dimension.End.Column - 1, datos.Count + 17, worksheet.Dimension.End.Column - 1]
            .Copy(worksheet.Cells[17, 22, datos.Count + 17, 22]);
            worksheet.Cells[17, 22, datos.Count + 17, 22].Style.Border.Right.Style = ExcelBorderStyle.None;

            worksheet.DeleteColumn(worksheet.Dimension.End.Column - 1);

            string celldainicial = "B1";
            string celldafinal = "C1";
            string celldaiadc = "D1";
            string celldafadc = "E1";
            int filaInicial = Convert.ToInt32("0" + worksheet.Cells[celldainicial].Value);
            int filaFinal = Convert.ToInt32("0" + worksheet.Cells[celldafinal].Value);
            string indicador = "";
            for (int i = filaInicial; i <= filaFinal; i++)
            {
                indicador = "$AJ$" + i;

                decimal number = Convert.ToDecimal("0" + worksheet.Cells[indicador].Value);

                double datavalue = decimal.ToDouble(number);


                if (datavalue >= 0.0)
                {
                    ExcelRange rng = worksheet.Cells[indicador];
                    ExcelAddress address = new ExcelAddress(rng.Address);
                    var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                    v.ShowValue = false;
                    v.Icon2.Value = 1;
                    v.Icon3.Value = 2;
                    //  v.Icon2.Value = -0.05;
                    // v.Icon3.Value = 0.0;
                    v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                    v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                    v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;


                }
                else if (datavalue < 0.0 && datavalue >= -0.05)
                {
                    ExcelRange rng = worksheet.Cells[indicador];
                    ExcelAddress address = new ExcelAddress(rng.Address);
                    var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                    v.ShowValue = false;
                    v.Icon2.Value = 1;
                    v.Icon3.Value = 2;
                    //  v.Icon2.Value = -0.05;
                    // v.Icon3.Value = 0.0;
                    v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                    v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                    v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;

                }
                else
                {
                    ExcelRange rng = worksheet.Cells[indicador];
                    ExcelAddress address = new ExcelAddress(rng.Address);
                    var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                    v.ShowValue = false;
                    v.Icon2.Value = 1;
                    v.Icon3.Value = 2;
                    //  v.Icon2.Value = -0.05;
                    // v.Icon3.Value = 0.0;
                    v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                    v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                    v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;
                }

            }

            if (datosAdicionales.Count > 0)
            {
                filaInicial = Convert.ToInt32("0" + worksheet.Cells[celldaiadc].Value);
                filaFinal = Convert.ToInt32("0" + worksheet.Cells[celldafadc].Value);
                indicador = "";
                for (int i = filaInicial; i <= filaFinal; i++)
                {
                    indicador = "$AJ$" + i;

                    decimal number = Convert.ToDecimal("0" + worksheet.Cells[indicador].Value);

                    double datavalue = decimal.ToDouble(number);


                    if (datavalue >= 0.0)
                    {
                        ExcelRange rng = worksheet.Cells[indicador];
                        ExcelAddress address = new ExcelAddress(rng.Address);
                        var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                        v.ShowValue = false;
                        v.Icon2.Value = 1;
                        v.Icon3.Value = 2;
                        //  v.Icon2.Value = -0.05;
                        // v.Icon3.Value = 0.0;
                        v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;


                    }
                    else if (datavalue < 0.0 && datavalue >= -0.05)
                    {
                        ExcelRange rng = worksheet.Cells[indicador];
                        ExcelAddress address = new ExcelAddress(rng.Address);
                        var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                        v.ShowValue = false;
                        v.Icon2.Value = 1;
                        v.Icon3.Value = 2;
                        //  v.Icon2.Value = -0.05;
                        // v.Icon3.Value = 0.0;
                        v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;

                    }
                    else
                    {
                        ExcelRange rng = worksheet.Cells[indicador];
                        ExcelAddress address = new ExcelAddress(rng.Address);
                        var v = worksheet.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                        v.ShowValue = false;
                        v.Icon2.Value = 1;
                        v.Icon3.Value = 2;
                        //  v.Icon2.Value = -0.05;
                        // v.Icon3.Value = 0.0;
                        v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                        v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;
                    }

                }


            }

            // worksheet.DeleteColumn(35);
            if (tieneItemsPendientes && !second_format)
            {
                worksheet.InsertRow(16, 1);
                hojaAdicionales.InsertRow(16, 1);

                worksheet.Cells[filaAdicionales + 1, 3, filaAdicionales + 1, 35].Copy(worksheet.Cells[17, 3, 17, 35]);
                worksheet.Cells[17, 3, 17, 36].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[17, 3, 17, 36].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(89, 89, 89));
                worksheet.Cells[17, 3, 17, 36].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[17, 3, 17, 36].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[17, 3, 17, 36].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Row(17).Height = 34;
                worksheet.Cells["C17:F17"].Merge = false;
                worksheet.Cells["V17"].Value = ma.ern_value;
                worksheet.Cells["V17"].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells["C17"].Value = "";
                worksheet.Cells["D17"].Value = "";
                worksheet.Cells["E17:G17"].Merge = true;
                worksheet.Cells["E17:G17"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                worksheet.Cells["H17"].Value = ma.porcentajeBudget;
                worksheet.Cells["H17"].Style.Numberformat.Format = "#0.00%";
                worksheet.Cells["H17"].Style.Font.Size = 17;
                worksheet.Cells["H17"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["I17"].Value = ma.porcentajeEAC;
                worksheet.Cells["I17"].Style.Numberformat.Format = "#0.00%";
                worksheet.Cells["I17"].Style.Font.Size = 17;
                worksheet.Cells["I17"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells["A16"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                worksheet.InsertRow(18, 1);
                hojaAdicionales.InsertRow(18, 1);

                worksheet.Row(18).Height = 34;
                worksheet.Row(18).Style.Font.Size = 17;
                worksheet.Cells[18, 3, 18, 36].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[18, 3, 18, 36].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(89, 89, 89));
                worksheet.Cells[18, 3, 18, 36].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells[18, 3, 18, 36].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[18, 3, 18, 36].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                worksheet.Cells["E18:G18"].Merge = true;
                worksheet.Cells["E18:G18"].Value = "CONSTRUCCIÓN";
                worksheet.Cells["E18:G18"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells["A18"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                worksheet.Cells["B18"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                worksheet.Cells["AJ18"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                int filatotal = 16;
                int filaadicional = 17;
                int filaactual = 18;
                string cellt = "H" + filatotal;
                string cella = "H" + filaadicional;
                string cell = "H" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt + "-" + "$" + cella;
                worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                cellt = "I" + filatotal;
                cella = "I" + filaadicional;
                cell = "I" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt + "-" + "$" + cella;
                worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                cellt = "N" + filatotal;
                cella = "N" + filaadicional;
                cell = "N" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt + "-" + "$" + cella;
                worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                cellt = "O" + filatotal;
                cella = "O" + filaadicional;
                cell = "O" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt + "-" + "$" + cella;
                worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                cellt = "P" + filatotal;
                cella = "P" + filaadicional;
                cell = "P" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt + "-" + "$" + cella;
                worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                cellt = "Q" + filatotal;
                cella = "Q" + filaadicional;
                cell = "Q" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt + "-" + "$" + cella;
                worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                cellt = "R" + filatotal;
                cella = "R" + filaadicional;
                cell = "R" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt + "-" + "$" + cella;
                worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                cellt = "S" + filatotal;
                cella = "S" + filaadicional;
                cell = "S" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt + "-" + "$" + cella;
                worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                cellt = "T" + filatotal;
                cella = "T" + filaadicional;
                cell = "T" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt + "-" + "$" + cella;
                worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                cellt = "U" + filatotal;
                cella = "U" + filaadicional;
                cell = "U" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt + "-" + "$" + cella;
                worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                cellt = "V" + filatotal;
                cella = "V" + filaadicional;
                cell = "V" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt + "-" + "$" + cella;
                worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                cellt = "W" + filatotal;
                cella = "W" + filaadicional;
                cell = "W" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt + "-" + "$" + cella;
                worksheet.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                cellt = "X" + filatotal;
                cella = "X" + filaadicional;
                cell = "X" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt;
                worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                cellt = "Y" + filatotal;
                cella = "Y" + filaadicional;
                cell = "Y" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt;
                worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                cellt = "Z" + filatotal;
                cella = "Z" + filaadicional;
                cell = "Z" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt;
                worksheet.Cells[cell].Style.Numberformat.Format = "dd-mmm-yy";
                cellt = "AE" + filatotal;
                cella = "AE" + filaadicional;
                cell = "AE" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt;
                worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                cellt = "AF" + filatotal;
                cella = "AF" + filaadicional;
                cell = "AF" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt;
                worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                cellt = "AG" + filatotal;
                cella = "AG" + filaadicional;
                cell = "AG" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt;
                worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";
                cellt = "AH" + filatotal;
                cella = "AH" + filaadicional;
                cell = "AH" + filaactual;
                worksheet.Cells[cell].FormulaR1C1 = "=" + "$" + cellt;
                worksheet.Cells[cell].Style.Numberformat.Format = "#0.00%";

                worksheet.Row(15).Height = 34;




            }
            worksheet.View.PageBreakView = true;
            worksheet.PrinterSettings.PrintArea = worksheet.Cells[2, 2, worksheet.Dimension.End.Row, 36];
            worksheet.PrinterSettings.FitToPage = true;

            hojaAdicionales.Cells["C16"].Value = m.costoEAC;
            hojaAdicionales.Cells["C16"].Style.Numberformat.Format = "#,##0.00";
            hojaAdicionales.Cells["D16"].Value = m.ac_actual;
            hojaAdicionales.Cells["D16"].Style.Numberformat.Format = "#,##0.00";
            hojaAdicionales.Cells["C16:D16"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hojaAdicionales.Cells["C16:D16"].Style.Fill.BackgroundColor.SetColor(Color.Black);
            hojaAdicionales.Cells["C16:D16"].Style.Font.Color.SetColor(Color.White);

            if (second_format)
            {
                hojaAdicionales.Cells[1, 3, hojaAdicionales.Dimension.End.Row, 3]
                .Copy(worksheet.Cells[1, 45, worksheet.Dimension.End.Row, 45]);

                worksheet.Column(45).Width = 30;
                worksheet.Column(45).Style.Font.Size = 20;



                hojaAdicionales.Cells[1, 4, hojaAdicionales.Dimension.End.Row, 4]
        .Copy(worksheet.Cells[1, 46, worksheet.Dimension.End.Row, 46]);
                worksheet.Column(46).Width = 30;
                worksheet.Column(46).Style.Font.Size = 20;
                hojaAdicionales.Hidden = eWorkSheetHidden.Hidden;

                worksheet.Cells["AS1:AT16"].Style.Font.Color.SetColor(Color.White);
                worksheet.Cells["AS1:AT16"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["AS1:AT16"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                for (var i = 44; i <= 46; i++)
                {
                    worksheet.Column(i).OutlineLevel = 1;
                    worksheet.Column(i).Collapsed = true;
                }
            }
            return excelPackage;
        }

        public List<RdoDatos> GetRdo(int RdoCabeceraId, string TipoReporte)
        {

            var cabera = Repository.Get(RdoCabeceraId);

            var ofertas = _ofertaRepository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.es_final)
                .Where(o => o.ProyectoId == cabera.ProyectoId).ToList();

            var lista = new List<RdoDatos>();
            foreach (var oferta in ofertas)
            {

                var nodo = this.GenerarArbol(oferta.Id, cabera.Id, TipoReporte);
                lista.AddRange(nodo);
            }

            return lista;
        }

        public List<RdoDatos> GenerarArbol(int ofertaId, int cabeceraId, string TipoReporte)
        {
            var i = _wbsRepository.GetAll().Where(c => c.id_nivel_padre_codigo == ".")
                .Where(o => o.OfertaId == ofertaId)
                .Where(o => o.vigente).ToList();


            var Lista = new List<RdoDatos>();

            var maxlevelwbs = this.nivel_mas_alto(ofertaId);
            var catalogoscolor = _catalogorepository.GetAllIncluding(c => c.TipoCatalogo).Where(c => c.TipoCatalogo.codigo == ProyectoCodigos.WBSLEVELCOLOR).Where(c => c.ordinal <= maxlevelwbs).OrderBy(c => c.ordinal).ToList();

            int ord = 0;
            foreach (var x in i)
            {

                var color = catalogoscolor[ord].valor_texto;
                var item = GenerarNodos(x, cabeceraId, TipoReporte, ord, color, catalogoscolor);
                Lista.AddRange(item);

            }

            return Lista;
        }

        public List<RdoDatos> GenerarNodos(Wbs wbs, int cabeceraId, string TipoReporte, int ord, string color, List<Catalogo> colores)
        {

            List<Wbs> hijos = GetWbsHijos(wbs.id_nivel_codigo, wbs.OfertaId);
            if (hijos.Count > 0)
            {
                var ordinal = ord + 1;
                var nuevo_color = colores[ordinal].valor_texto;
                var lista_hijos = new List<RdoDatos>();


                foreach (var h in hijos)
                {
                    var lhijos = GenerarNodos(h, cabeceraId, TipoReporte, ordinal, nuevo_color, colores);
                    if (lhijos.Count > 0)
                    {
                        lista_hijos.AddRange(lhijos);
                    }
                }

                List<RdoDatos> list = new List<RdoDatos>();
                var actividadesDelWbs = this.RdoDatosConTotales(wbs, TipoReporte, cabeceraId);
                var padre = this.CalcularTotales(actividadesDelWbs);
                padre.tipo = "Padre";
                padre.nombre_actividad = wbs.nivel_nombre;
                padre.color = color;
                if (ord == 0)
                {
                    padre.principal = true;
                }
                if (lista_hijos.Count > 0)
                {
                    list.Add(padre);
                    list.AddRange(lista_hijos);
                }
                return list;
            }
            else
            {
                List<RdoDatos> list = new List<RdoDatos>();

                var itemsDelWbs = this.RdoDatosConTotales(wbs, TipoReporte, cabeceraId);
                var actividad = this.CalcularTotales(itemsDelWbs);
                actividad.tipo = "Actividad";
                actividad.color = color;
                actividad.nombre_actividad = wbs.nivel_nombre;


                if (TipoReporte == "BUDGET")
                {

                    var items = this.GetRdoDetalles(wbs.Id, cabeceraId);
                    if (items.Count > 0)
                    {
                        list.Add(actividad);
                        list.AddRange(items);
                    }
                }
                else
                {
                    var items = this.GetRdoDetallesEAC(wbs.Id, cabeceraId);
                    if (items.Count > 0)
                    {
                        list.Add(actividad);
                        list.AddRange(items);
                    }
                }


                return list;
            }
        }

        public List<RdoDatos> GetRdoDetalles(int WbsId, int cabeceraId)
        {

            var query = _detalleRepository.GetAllIncluding(o => o.Computo)
                .Where(o => o.vigente == true)
                .Where(o => o.RsoCabeceraId == cabeceraId)
                .Where(o => o.WbsId == WbsId)
                .Where(o => o.Computo.presupuestado);


            var list = (from d in query
                        select new RdoDatos()
                        {
                            cantidad_eac = d.cantidad_eac,
                            precio_unitario = d.precio_unitario,
                            cantidad_acumulada = d.cantidad_acumulada,
                            cantidad_planificada = d.cantidad_planificada,
                            cantidad_diaria = d.cantidad_diaria,
                            tipo = "item",
                            nombre_actividad = d.nombre_actividad,
                            porcentaje_costo_eac_total = d.porcentaje_costo_eac_total,
                            porcentaje_avance_actual_acumulado = d.porcentaje_avance_actual_acumulado,
                            costo_eac = d.costo_eac,
                            cantidad_anterior = d.cantidad_anterior,
                            porcentaje_avance_previsto_acumulado = d.porcentaje_avance_previsto_acumulado,
                            ac_diario = d.ac_diario,
                            porcentaje_avance_anterior = d.porcentaje_avance_anterior,
                            ac_actual = d.ac_actual,
                            porcentaje_avance_diario = d.porcentaje_avance_diario,
                            porcentaje_presupuesto_total = d.porcentaje_presupuesto_total,
                            ev_anterior = d.ev_anterior,
                            ac_anterior = d.ac_anterior,
                            ev_diario = d.ev_diario,
                            fecha_fin_prevista = d.fecha_fin_prevista,
                            costo_presupuesto = d.costo_presupuesto,
                            presupuesto_total = d.presupuesto_total,
                            fecha_inicio_prevista = d.fecha_inicio_prevista,
                            fecha_inicio_real = d.fecha_inicio_real,
                            fecha_fin_real = d.fecha_fin_real,
                            codigo_preciario = d.codigo_preciario,
                            ev_actual = d.ev_actual,
                            pv_costo_planificado = d.pv_costo_planificado,
                            UM = d.UM,
                            ern_value = d.ern_value
                        }).ToList();
            return list;
        }

        public List<RdoDatos> GetRdoDetallesEAC(int WbsId, int cabeceraId)
        {

            var query = _eacRepository.GetAllIncluding(o => o.Computo.Item)
                .Where(o => o.vigente)
                .Where(o => o.Computo.Item.vigente)
                .Where(o => o.Computo.vigente)
                .Where(o => o.Computo.Item.GrupoId == 2)
                .Where(o => o.RsoCabeceraId == cabeceraId)
                .Where(o => o.vigente)
                .Where(o => o.WbsId == WbsId)
                .Where(o => !o.Computo.Item.PendienteAprobacion)
                .Where(o => !o.es_temporal).ToList() //items temporales para aprobacion
                ;

            var data = (from d in query
                        select new RdoDatos()
                        {
                            computoId = d.ComputoId,
                            cantidad_eac = d.cantidad_eac,
                            precio_unitario = d.precio_unitario,
                            cantidad_acumulada = d.cantidad_acumulada,
                            cantidad_planificada = d.cantidad_planificada,
                            cantidad_diaria = d.cantidad_diaria,
                            tipo = "item",
                            nombre_actividad = d.nombre_actividad,
                            porcentaje_costo_eac_total = d.porcentaje_costo_eac_total,
                            porcentaje_avance_actual_acumulado = d.porcentaje_avance_actual_acumulado,
                            costo_eac = d.costo_eac,
                            cantidad_anterior = d.cantidad_anterior,
                            porcentaje_avance_previsto_acumulado = d.porcentaje_avance_previsto_acumulado,
                            ac_diario = d.ac_diario,
                            porcentaje_avance_anterior = d.porcentaje_avance_anterior,
                            ac_actual = d.ac_actual,
                            porcentaje_avance_diario = d.porcentaje_avance_diario,
                            porcentaje_presupuesto_total = d.porcentaje_presupuesto_total,
                            ev_anterior = d.ev_anterior,
                            ac_anterior = d.ac_anterior,
                            ev_diario = d.ev_diario,
                            fecha_fin_prevista = d.fecha_fin_prevista,
                            costo_presupuesto = d.costo_presupuesto,
                            presupuesto_total = d.presupuesto_total,
                            fecha_inicio_prevista = d.fecha_inicio_prevista,
                            fecha_inicio_real = d.fecha_inicio_real,
                            fecha_fin_real = d.fecha_fin_real,
                            codigo_preciario = d.codigo_preciario,
                            ev_actual = d.ev_actual,
                            pv_costo_planificado = d.pv_costo_planificado,
                            UM = d.UM,
                            ern_value = d.ern_value,
                            codigo_especialidad = d.codigo_especialidad,
                            codigo_grupo = d.codigo_grupo,
                            costo_budget_version_anterior = d.costo_budget_version_anterior,
                            ev_actual_version_anterior = d.ev_actual_version_anterior,
                            id_rubro = d.id_rubro,
                            codigo_ordenamiento = this.QuitarLetras(d.codigo_preciario),

                        }).ToList();

            foreach (var nodoRdoDatos in data)
            {
                var apu = new List<RdoDatos>();
                apu.Add(nodoRdoDatos);
                var costo_eac_desc = this.ObtenerTotalesEspecialidad(apu, "COSTO_EAC", 0);
                var ac_actual_desc = this.ObtenerTotalesEspecialidad(apu, "AC_ACTUAL", 0);

                nodoRdoDatos.costo_eac_descuento = nodoRdoDatos.costo_eac - this.ObtenerDescuentoEspecialidadAPU(costo_eac_desc, nodoRdoDatos);
                nodoRdoDatos.ac_actual_descuento = nodoRdoDatos.ac_actual - this.ObtenerDescuentoEspecialidadAPU(ac_actual_desc, nodoRdoDatos);

            }
            var list = (from l in data orderby Version.Parse(l.codigo_ordenamiento) select l).ToList();

            return list;
        }
        string QuitarLetras(string codigo)
        {
            return codigo.ToUpper().Replace("X", "").Replace("A", "");
        }

        public List<Wbs> GetWbsHijos(string codigo_padre, int ofertaId)
        {
            var items = _wbsRepository.GetAll()
                .Where(o => o.vigente == true)
                .Where(o => o.OfertaId == ofertaId)
                .Where(o => o.id_nivel_padre_codigo == codigo_padre).ToList();
            return items;
        }



        public List<RdoDatos> RdoDatosConTotales(Wbs wbs, string TipoReporte, int cabeceraId)
        {
            List<Wbs> hijos = GetWbsHijos(wbs.id_nivel_codigo, wbs.OfertaId);
            if (hijos.Count > 0)
            {
                var lista_hijos = new List<RdoDatos>();
                foreach (var h in hijos)
                {
                    var lhijos = RdoDatosConTotales(h, TipoReporte, cabeceraId);
                    lista_hijos.AddRange(lhijos);
                }
                return lista_hijos;
            }
            else
            {
                List<RdoDatos> list = new List<RdoDatos>();

                if (TipoReporte == "BUDGET")
                {
                    var items = this.GetRdoDetalles(wbs.Id, cabeceraId);
                    list.AddRange(items);
                }
                else
                {
                    var items = this.GetRdoDetallesEAC(wbs.Id, cabeceraId);
                    list.AddRange(items);
                }

                return list;
            }
        }

        public RdoDatos CalcularTotales(List<RdoDatos> lista)
        {
            decimal cantidad_eac = 0;
            decimal precio_unitario = 0;
            decimal cantidad_acumulada = 0;
            decimal cantidad_planificada = 0;
            decimal cantidad_diaria = 0;
            string nombre_actividad = "";
            decimal porcentaje_costo_eac_total = 0;
            decimal porcentaje_avance_actual_acumulado = 0;
            decimal costo_eac = 0;
            decimal cantidad_anterior = 0;
            decimal porcentaje_avance_previsto_acumulado = 0;
            decimal ac_diario = 0;
            decimal porcentaje_avance_anterior = 0;
            decimal ac_actual = 0;
            decimal porcentaje_avance_diario = 0;
            decimal porcentaje_presupuesto_total = 0;
            decimal ev_anterior = 0;
            decimal ac_anterior = 0;
            decimal ev_diario = 0;
            decimal costo_presupuesto = 0;
            decimal presupuesto_total = 0;
            decimal costo_eac_descuento = 0;
            decimal ac_actual_descuento = 0;

            var fecha_fin_prevista = new DateTime(1990, 1, 1);
            var fecha_inicio_prevista = new DateTime(1990, 1, 1);
            var fecha_inicio_real = new DateTime(1990, 1, 1);
            var fecha_fin_real = new DateTime(1990, 1, 1);

            decimal ev_actual = 0;

            decimal ern_value = 0;
            decimal pv_costo_planificado = 0;

            /* Costo Budget Version Anterior */
            decimal costo_budget_version_anterior = 0;
            decimal ev_actual_version_anterior = 0;

            foreach (var w in lista)
            {

                cantidad_eac += Decimal.Round(w.cantidad_eac, 8);
                precio_unitario += Decimal.Round(w.precio_unitario, 2);
                cantidad_acumulada += Decimal.Round(w.cantidad_acumulada, 8);
                cantidad_planificada += Decimal.Round(w.cantidad_planificada, 8);
                cantidad_diaria += Decimal.Round(w.cantidad_diaria, 8);
                porcentaje_costo_eac_total += Decimal.Round(w.porcentaje_costo_eac_total, 8);
                porcentaje_avance_actual_acumulado += Decimal.Round(w.porcentaje_avance_actual_acumulado, 8);
                costo_eac += Decimal.Round(w.costo_eac, 8);
                costo_eac_descuento += Decimal.Round(w.costo_eac_descuento, 8);
                cantidad_anterior += Decimal.Round(w.cantidad_anterior, 8);
                porcentaje_avance_previsto_acumulado += Decimal.Round(w.porcentaje_avance_previsto_acumulado, 8);
                ac_diario += Decimal.Round(w.ac_diario, 8);
                porcentaje_avance_anterior += Decimal.Round(w.porcentaje_avance_anterior, 8);
                ac_actual += Decimal.Round(w.ac_actual, 8);
                ac_actual_descuento += Decimal.Round(w.ac_actual_descuento, 8);
                porcentaje_avance_diario += Decimal.Round(w.porcentaje_avance_diario, 8);
                porcentaje_presupuesto_total += Decimal.Round(w.porcentaje_presupuesto_total, 8);
                ev_anterior += Decimal.Round(w.ev_anterior, 8);
                ac_anterior += Decimal.Round(w.ac_anterior, 8);
                ev_diario += Decimal.Round(w.ev_diario, 8);
                costo_presupuesto += Decimal.Round(w.costo_presupuesto, 8);
                presupuesto_total += Decimal.Round(w.presupuesto_total, 8);
                ev_actual += Decimal.Round(w.ev_actual, 8);
                pv_costo_planificado += Decimal.Round(w.pv_costo_planificado, 8);
                ern_value += Decimal.Round(w.ern_value, 8);
                /* Costo Budget Version Anterior*/
                costo_budget_version_anterior += Decimal.Round(w.costo_budget_version_anterior, 8);
                ev_actual_version_anterior += Decimal.Round(w.ev_actual_version_anterior, 8);
                if (w.fecha_fin_prevista.HasValue)
                {
                    if (w.fecha_fin_prevista > fecha_fin_prevista)
                    {
                        fecha_fin_prevista = w.fecha_fin_prevista.Value;
                    }
                }

                if (w.fecha_fin_real.HasValue)
                {
                    if (w.fecha_fin_real > fecha_fin_real)
                    {
                        fecha_fin_real = w.fecha_fin_real.Value;
                    }
                }
                if (w.fecha_inicio_prevista.HasValue)
                {
                    if (w.fecha_inicio_prevista < fecha_inicio_prevista && fecha_inicio_prevista != new DateTime(1990, 01, 01))
                    {
                        fecha_inicio_prevista = w.fecha_inicio_prevista.Value;
                    }
                    else if (fecha_inicio_prevista == new DateTime(1990, 01, 01))
                    {
                        fecha_inicio_prevista = w.fecha_inicio_prevista.Value;
                    }

                }

                if (w.fecha_inicio_real.HasValue)
                {
                    if (w.fecha_inicio_real < fecha_inicio_real && fecha_inicio_real != new DateTime(1990, 01, 01))
                    {
                        fecha_inicio_real = w.fecha_inicio_real.Value;
                    }
                    else if (fecha_inicio_real == new DateTime(1990, 01, 01))
                    {
                        fecha_inicio_real = w.fecha_inicio_real.Value;
                    }
                }


            }

            var datos = new RdoDatos()
            {
                cantidad_eac = Decimal.Round(cantidad_eac, 8),
                precio_unitario = Decimal.Round(precio_unitario, 2),
                cantidad_acumulada = Decimal.Round(cantidad_acumulada, 8),
                cantidad_planificada = Decimal.Round(cantidad_planificada, 8),
                cantidad_diaria = Decimal.Round(cantidad_diaria, 8),
                nombre_actividad = nombre_actividad,
                porcentaje_costo_eac_total = Decimal.Round(porcentaje_costo_eac_total, 8),
                porcentaje_avance_actual_acumulado = Decimal.Round(porcentaje_avance_actual_acumulado, 8),
                
                costo_eac = Decimal.Round(costo_eac, 8),
                costo_eac_descuento = Decimal.Round(costo_eac_descuento, 8),
                cantidad_anterior = Decimal.Round(cantidad_anterior, 8),
               
                porcentaje_avance_previsto_acumulado = Decimal.Round(porcentaje_avance_previsto_acumulado, 8),
                ac_diario = Decimal.Round(ac_diario, 8),
                porcentaje_avance_anterior = Decimal.Round(porcentaje_avance_anterior, 8),
                ac_actual = Decimal.Round(ac_actual, 8),
                ac_actual_descuento = Decimal.Round(ac_actual_descuento, 8),
                porcentaje_avance_diario = Decimal.Round(porcentaje_avance_diario, 8),
                porcentaje_presupuesto_total = Decimal.Round(porcentaje_presupuesto_total, 8),
                ev_anterior = Decimal.Round(ev_anterior, 8),
                ac_anterior = Decimal.Round(ac_anterior, 8),
                ev_diario = Decimal.Round(ev_diario, 8),

                costo_presupuesto = Decimal.Round(costo_presupuesto, 8),
                presupuesto_total = Decimal.Round(presupuesto_total, 8),
                fecha_inicio_prevista = fecha_inicio_prevista != new DateTime(1990, 01, 01) ? fecha_inicio_prevista : new DateTime?(),
                fecha_inicio_real = fecha_inicio_real != new DateTime(1990, 01, 01) ? fecha_inicio_real : new DateTime?(),
                fecha_fin_prevista = fecha_fin_prevista != new DateTime(1990, 01, 01) ? fecha_fin_prevista : new DateTime?(),
                fecha_fin_real = fecha_fin_real != new DateTime(1990, 01, 01) ? fecha_fin_real : new DateTime?(),
                codigo_preciario = "",
                ev_actual = Decimal.Round(ev_actual, 8),
                pv_costo_planificado = Decimal.Round(pv_costo_planificado, 8),
                UM = "",
                ern_value = Decimal.Round(ern_value, 8),
                costo_budget_version_anterior = Decimal.Round(costo_budget_version_anterior, 8),
                ev_actual_version_anterior = Decimal.Round(ev_actual_version_anterior, 8),
            };
            
            /*
            var costoPresupuestoVersionAnterior = datos.costo_budget_version_anterior; //Traer Costo Presupuesto Anterior nueva Columna 
            if (costoPresupuestoVersionAnterior > 0)
            {

                datos.porcentaje_avance_anterior = Decimal.Round(Decimal.Round(datos.ev_actual_version_anterior, 8) / Decimal.Round(costoPresupuestoVersionAnterior, 8), 8);// 
            }
            else
            {
                datos.porcentaje_avance_anterior = 0;
            }
            */
            if (datos.costo_presupuesto > 0)
            {


                
                datos.porcentaje_avance_anterior = Decimal.Round(Decimal.Round(datos.ev_anterior, 8) / Decimal.Round(datos.costo_presupuesto, 8), 8);




                datos.porcentaje_avance_actual_acumulado = Decimal.Round(Decimal.Round(datos.ev_actual, 8) / Decimal.Round(datos.costo_presupuesto, 8), 8);


                /* datos.porcentaje_avance_diario = Decimal.Round(Decimal.Round(datos.ev_diario, 8) / Decimal.Round(datos.costo_presupuesto, 8), 8);//si

                 */

                datos.porcentaje_avance_diario = datos.porcentaje_avance_actual_acumulado - datos.porcentaje_avance_anterior;


                datos.porcentaje_avance_previsto_acumulado = Decimal.Round(Decimal.Round(datos.pv_costo_planificado, 8) / Decimal.Round(datos.costo_presupuesto, 8), 8);




            }
            else
            {
                datos.porcentaje_avance_anterior = 0;
                datos.porcentaje_avance_diario = 0;
                datos.porcentaje_avance_actual_acumulado = 0;
                datos.porcentaje_avance_previsto_acumulado = 0;
            }

            return datos;
        }

        public List<RdoDatos> GetRdoAdicionales(int RdoCabeceraId, string TipoReporte)
        {

            var cabera = Repository.Get(RdoCabeceraId);

            var ofertas = _ofertaRepository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.es_final)
                .Where(o => o.ProyectoId == cabera.ProyectoId).ToList();

            var lista = new List<RdoDatos>();
            foreach (var oferta in ofertas)
            {
                var nodo = this.GenerarArbolAdicionales(oferta.Id, cabera.Id, TipoReporte);
                lista.AddRange(nodo);
            }

            List<int> posiciones_a_eliminar = new List<int>();
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].tipo == "Padre")
                {
                    var next = i + 1;
                    if (next == lista.Count)
                    {
                        lista.RemoveAt(i);
                    }
                    else
                    {
                        if (lista[i + 1].tipo == "Padre")
                        {
                            lista.RemoveAt(i);
                            i = 0;
                        }
                    }

                }
            }



            return lista;
        }

        public List<RdoDatos> GenerarArbolAdicionales(int ofertaId, int cabeceraId, string TipoReporte)
        {
            var i = _wbsRepository.GetAll().Where(c => c.id_nivel_padre_codigo == ".")
                .Where(o => o.OfertaId == ofertaId)
                .Where(o => o.vigente).ToList();


            var Lista = new List<RdoDatos>();


            foreach (var x in i)
            {
                var item = GenerarNodosAdicionales(x, cabeceraId, TipoReporte);
                Lista.AddRange(item);
            }

            return Lista;
        }

        public List<ProyectoObservacionDto> GetObservaciones(int ProyectoId)
        {
            var query = _proyectoObservacionRepository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.ProyectoId == ProyectoId);

            var list = (from i in query
                        select new ProyectoObservacionDto()
                        {
                            Observacion = i.Observacion,
                            FechaObservacion = i.FechaObservacion
                        }).ToList();
            return list;
        }

        public List<AvanceObraDto> GetActividades(int ProyectoId, DateTime fecha)
        {
            var query = _avanceRepository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.aprobado)
                .Where(o => o.Oferta.ProyectoId == ProyectoId)
                .Where(o => o.fecha_presentacion == fecha);

            var items = (from i in query
                         select new AvanceObraDto()
                         {
                             comentario = i.comentario
                         }).ToList();

            return items;
        }

        public List<RdoDatos> GenerarNodosAdicionales(Wbs wbs, int cabeceraId, string TipoReporte)
        {
            List<Wbs> hijos = GetWbsHijos(wbs.id_nivel_codigo, wbs.OfertaId);
            if (hijos.Count > 0)
            {
                var lista_hijos = new List<RdoDatos>();
                foreach (var h in hijos)
                {
                    var lhijos = GenerarNodosAdicionales(h, cabeceraId, TipoReporte);

                    lista_hijos.AddRange(lhijos);


                }

                List<RdoDatos> list = new List<RdoDatos>();
                var padre = new RdoDatos()
                {
                    tipo = "Padre",
                    nombre_actividad = wbs.nivel_nombre
                };

                if (lista_hijos.Count > 0)
                {
                    list.Add(padre);
                    list.AddRange(lista_hijos);
                }
                return list;
            }
            else
            {
                List<RdoDatos> list = new List<RdoDatos>();
                var actividad = new RdoDatos()
                {
                    tipo = "Actividad",
                    nombre_actividad = wbs.nivel_nombre,
                    ItemsPresupuestados = true,
                };


                var items = this.GetRdoDetallesEACAdicionales(wbs.Id, cabeceraId);
                if (items.Count > 0)
                {
                    list.Add(actividad);
                    list.AddRange(items);
                }



                return list;
            }
        }

        public List<RdoDatos> GetRdoDetallesAdicionales(int WbsId, int cabeceraId)
        {

            var query = _detalleRepository.GetAllIncluding(o => o.Computo)
                .Where(o => o.vigente == true)
                .Where(o => o.RsoCabeceraId == cabeceraId)
                .Where(o => o.WbsId == WbsId)
                .Where(o => o.Computo.presupuestado == false);


            var list = (from d in query
                        select new RdoDatos()
                        {
                            cantidad_eac = d.cantidad_eac,
                            precio_unitario = d.precio_unitario,
                            cantidad_acumulada = d.cantidad_acumulada,
                            cantidad_planificada = d.cantidad_planificada,
                            cantidad_diaria = d.cantidad_diaria,
                            tipo = "item",
                            nombre_actividad = d.nombre_actividad,
                            porcentaje_costo_eac_total = d.porcentaje_costo_eac_total,
                            porcentaje_avance_actual_acumulado = d.porcentaje_avance_actual_acumulado,
                            costo_eac = d.costo_eac,
                            cantidad_anterior = d.cantidad_anterior,
                            porcentaje_avance_previsto_acumulado = d.porcentaje_avance_previsto_acumulado,
                            ac_diario = d.ac_diario,
                            porcentaje_avance_anterior = d.porcentaje_avance_anterior,
                            ac_actual = d.ac_actual,
                            porcentaje_avance_diario = d.porcentaje_avance_diario,
                            porcentaje_presupuesto_total = d.porcentaje_presupuesto_total,
                            ev_anterior = d.ev_anterior,
                            ac_anterior = d.ac_anterior,
                            ev_diario = d.ev_diario,
                            fecha_fin_prevista = d.fecha_fin_prevista,
                            costo_presupuesto = d.costo_presupuesto,
                            presupuesto_total = d.presupuesto_total,
                            fecha_inicio_prevista = d.fecha_inicio_prevista,
                            fecha_inicio_real = d.fecha_inicio_real,
                            fecha_fin_real = d.fecha_fin_real,
                            codigo_preciario = d.codigo_preciario,
                            ev_actual = d.ev_actual,
                            pv_costo_planificado = d.pv_costo_planificado,
                            ern_value = d.ern_value
                        }).ToList();

            return list;
        }

        public List<RdoDatos> GetRdoDetallesEACAdicionales(int WbsId, int cabeceraId)
        {

            var query = _eacRepository.GetAllIncluding(o => o.Computo.Item)
                .Where(o => o.vigente)
                .Where(o => o.RsoCabeceraId == cabeceraId)
                .Where(o => o.WbsId == WbsId)
                .Where(o => o.Computo.Item.PendienteAprobacion || o.es_temporal).ToList();


            var data = (from d in query
                        select new RdoDatos()
                        {

                            computoId = d.ComputoId,
                            cantidad_eac = d.cantidad_eac,
                            precio_unitario = d.precio_unitario,
                            cantidad_acumulada = d.cantidad_acumulada,
                            cantidad_planificada = d.cantidad_planificada,
                            cantidad_diaria = d.cantidad_diaria,
                            tipo = "item",
                            nombre_actividad = d.nombre_actividad,
                            porcentaje_costo_eac_total = d.porcentaje_costo_eac_total,
                            porcentaje_avance_actual_acumulado = d.porcentaje_avance_actual_acumulado,
                            costo_eac = d.costo_eac,
                            cantidad_anterior = d.cantidad_anterior,
                            porcentaje_avance_previsto_acumulado = d.porcentaje_avance_previsto_acumulado,
                            ac_diario = d.ac_diario,
                            porcentaje_avance_anterior = d.porcentaje_avance_anterior,
                            ac_actual = d.ac_actual,
                            porcentaje_avance_diario = d.porcentaje_avance_diario,
                            porcentaje_presupuesto_total = d.porcentaje_presupuesto_total,
                            ev_anterior = d.ev_anterior,
                            ac_anterior = d.ac_anterior,
                            ev_diario = d.ev_diario,
                            fecha_fin_prevista = d.fecha_fin_prevista,
                            costo_presupuesto = d.costo_presupuesto,
                            presupuesto_total = d.presupuesto_total,
                            fecha_inicio_prevista = d.fecha_inicio_prevista,
                            fecha_inicio_real = d.fecha_inicio_real,
                            fecha_fin_real = d.fecha_fin_real,
                            codigo_preciario = d.codigo_preciario,
                            ev_actual = d.ev_actual,
                            pv_costo_planificado = d.pv_costo_planificado,
                            ern_value = d.ern_value,
                            codigo_especialidad = d.codigo_especialidad,
                            codigo_grupo = d.codigo_grupo,
                            es_temporal = d.es_temporal,
                            id_rubro = d.id_rubro,
                            codigo_ordenamiento = this.QuitarLetras(d.codigo_preciario)

                        }).ToList();


            foreach (var nodoRdoDatos in data)
            {
                var apu = new List<RdoDatos>();
                apu.Add(nodoRdoDatos);
                var costo_eac_desc = this.ObtenerTotalesEspecialidad(apu, "COSTO_EAC", 0);
                var ac_actual_desc = this.ObtenerTotalesEspecialidad(apu, "AC_ACTUAL", 0);

                nodoRdoDatos.costo_eac_descuento = nodoRdoDatos.costo_eac - this.ObtenerDescuentoEspecialidadAPU(costo_eac_desc, nodoRdoDatos);
                nodoRdoDatos.ac_actual_descuento = nodoRdoDatos.ac_actual - this.ObtenerDescuentoEspecialidadAPU(ac_actual_desc, nodoRdoDatos);

            }

            var list = (from l in data orderby Version.Parse(l.codigo_ordenamiento) select l).ToList();
            return list;
        }

        public int SumatoriaDisruptivo(int ProyectoId)
        {
            var query = _avanceRepository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.aprobado)
                .Where(o => o.Oferta.es_final)
                .Where(o => o.Oferta.ProyectoId == ProyectoId);

            var queryDisruptivos = _obraDisruptivoRepository.GetAll()
                .Where(o => o.vigente);

            var disruptivos = (from d in queryDisruptivos
                               where (from a in query
                                      select a.Id).Contains(d.ProyectoId)
                               select new ObraDisruptivoDto()
                               {
                                   ProyectoId = d.ProyectoId
                               }).Distinct().ToList();

            return disruptivos.Count;
        }

        public ExcelPackage GenerarCurvaRDO(int ProyectoId, int RdoCabeceraId)
        {
            // Detalles del Proyecto

            var proyecto = _proyectoRepository.Get(ProyectoId);



            ExcelPackage package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("CURVA");
            sheet.View.ZoomScale = 80;

            //Cabeceras Proyecto
            sheet.Cells["B2:L2"].Merge = true;
            sheet.Cells["B2:L2"].Style.WrapText = true;
            sheet.Cells["B2:L2"].Style.Font.Bold = true;
            sheet.Cells["B2:L2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["B2:L2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells["B2:L2"].Style.Font.Size = 9;

            sheet.Cells["B2:L2"].Value = "PROYECTO " + proyecto.codigo + " " + proyecto.descripcion_proyecto;

            //ESPACIO

            sheet.Cells["B3:D3"].Merge = true;

            //Fechas Proyecto
            sheet.Cells["B4:D4"].Merge = true;
            sheet.Cells["B4:D4"].Style.WrapText = true;
            sheet.Cells["B4:D4"].Style.Font.Size = 9;
            sheet.Cells["B4:D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["B4:D4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells["B4:D4"].Value = "Fecha Inicial: " + "01/01/2019";

            sheet.Cells["B5:D5"].Merge = true;
            sheet.Cells["B5:D5"].Style.WrapText = true;
            sheet.Cells["B5:D5"].Style.Font.Size = 9;
            sheet.Cells["B5:D5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["B5:D5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells["B5:D5"].Value = "Fecha Fin: " + "30/06/2019";

            sheet.Cells["B6:D6"].Merge = true;
            sheet.Cells["B6:D6"].Style.WrapText = true;
            sheet.Cells["B6:D6"].Style.Font.Size = 9;
            sheet.Cells["B6:D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["B6:D6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells["B6:D6"].Value = "Fecha Generación: " + DateTime.Today.ToShortDateString();


            //ESPACIO

            sheet.Cells["B7:D7"].Merge = true;

            //CABECERA NOMBRES
            sheet.Column(2).Width = 15;
            sheet.Column(3).Width = 20;
            sheet.Column(4).Width = 20;

            sheet.Cells["B8"].Value = "Fecha";
            sheet.Cells["B8"].Style.Font.Size = 9;
            sheet.Cells["B8"].Style.WrapText = true;
            sheet.Cells["B8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["B8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells["B8"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells["B8"].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
            sheet.Cells["B8"].Style.Font.Color.SetColor(Color.White);


            sheet.Cells["C8"].Value = "% Previsto Acumulado";
            sheet.Cells["C8"].Style.Font.Size = 9;
            sheet.Cells["C8"].Style.WrapText = true;
            sheet.Cells["C8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["C8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells["C8"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells["C8"].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
            sheet.Cells["C8"].Style.Font.Color.SetColor(Color.White);

            sheet.Cells["D8"].Value = "% Real Acumulado";
            sheet.Cells["D8"].Style.WrapText = true;
            sheet.Cells["D8"].Style.Font.Size = 9;
            sheet.Cells["D8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["D8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells["D8"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells["D8"].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
            sheet.Cells["D8"].Style.Font.Color.SetColor(Color.White);


            ///
            var proceso = this.ActualizarCurva(proyecto.Id, RdoCabeceraId);

            if (proceso.Count > 0)
            {
                //var datos_curva = _curvaRepository.GetAll().Where(c => c.ProyectoId == proyecto.Id).ToList();

                if (proceso.Count > 0)
                {
                    int i = 9;
                    foreach (var d in proceso)
                    {
                        sheet.Cells[i, 2].Value = d.fecha;
                        sheet.Cells[i, 2].Style.Numberformat.Format = "dd-mmm-yy";


                        sheet.Cells[i, 3].Value = d.valor_previsto_acumulado;
                        sheet.Cells[i, 3].Style.Numberformat.Format = "0.0%";




                        sheet.Cells[i, 4].Value = d.valor_real_acumulado;
                        sheet.Cells[i, 4].Style.Numberformat.Format = "0.0%";

                        i++;
                    }


                    //POSICION DE LA CURVA    
                    ExcelChart ec = (ExcelLineChart)sheet.Drawings.AddChart("Curva Rdo", eChartType.Line);
                    ec.SetPosition(8, 0, 8, 0);
                    ec.SetSize(800, 300);

                    ec.Title.Text = "Curva de Avance RDO";

                    int row = sheet.Dimension.End.Row;
                    var rangeX = sheet.Cells["B9:B" + row]; // X-Axis
                    var range1 = sheet.Cells["C9:C" + row]; // 1ra LineSerie

                    ExcelLineChartSerie serie1 = (ExcelLineChartSerie)ec.Series.Add(range1, rangeX);
                    serie1.Header = sheet.Cells["C8"].Value.ToString();

                    var rangeX2 = sheet.Cells["B9:B" + row]; // X-Axis
                    var range12 = sheet.Cells["D9:D" + row]; // 2da Seria

                    ExcelLineChartSerie serie2 = (ExcelLineChartSerie)ec.Series.Add(range12, rangeX2);

                    serie2.Header = sheet.Cells["D8"].Value.ToString();

                }

            }
            else
            {
                var fecha = new DateTime(2018, 01, 09);
                double a = 0.05;
                double b = 0.02;
                for (int i = 9; i <= 29; i++)
                {

                    fecha = fecha.AddDays(1);
                    sheet.Cells[i, 2].Value = fecha;
                    sheet.Cells[i, 2].Style.Numberformat.Format = "dd-mmm-yy";


                    sheet.Cells[i, 3].Value = a;
                    sheet.Cells[i, 3].Style.Numberformat.Format = "0.0%";

                    sheet.Cells[i, 4].Value = b;
                    sheet.Cells[i, 4].Style.Numberformat.Format = "0.0%";

                    a = a + 0.04;
                    b = b + 0.03;
                }


                //POSICION DE LA CURVA    
                ExcelChart ec = (ExcelLineChart)sheet.Drawings.AddChart("Curva Rdo", eChartType.Line);
                ec.SetPosition(8, 0, 8, 0);
                ec.SetSize(800, 300);

                ec.Title.Text = "Curva de Avance RDO";

                int row = sheet.Dimension.End.Row;
                var rangeX = sheet.Cells["B9:B" + row]; // X-Axis
                var range1 = sheet.Cells["C9:C" + row]; // 1ra LineSerie

                ExcelLineChartSerie serie1 = (ExcelLineChartSerie)ec.Series.Add(range1, rangeX);
                serie1.Header = sheet.Cells["C8"].Value.ToString();

                var rangeX2 = sheet.Cells["B9:B" + row]; // X-Axis
                var range12 = sheet.Cells["D9:D" + row]; // 2da Seria

                ExcelLineChartSerie serie2 = (ExcelLineChartSerie)ec.Series.Add(range12, rangeX2);

                serie2.Header = sheet.Cells["D8"].Value.ToString();

            }








            return package;
        }

        public List<RsoCurva> ActualizarCurva(int ProyectoId, int RdoCabeceraId)
        {
            //VACIO TODO LOS DATOS DEL PROYECTO 
            var cabecera = Repository.Get(RdoCabeceraId);
            var rdocabecera = Repository.Get(RdoCabeceraId);



            var rdoDetalles = _eacRepository.GetAll()
                                            .Where(c => c.vigente)
                                            .Where(c => c.RsoCabeceraId == RdoCabeceraId)
                                            .ToList();

            var curva_datos = _curvaRepository.GetAll().Where(z => z.ProyectoId == ProyectoId).ToList();

            foreach (var dc in curva_datos)
            {
                _curvaRepository.Delete(dc);
            }



            var rdo_detalles = _detalleRepository.GetAllIncluding(c => c.RsoCabecera, c => c.Computo)
                                                    .Where(c => c.RsoCabeceraId == RdoCabeceraId)
                                                    .Where(c => c.vigente)

                                                    .ToList();




            //Prueba

            List<RsoCurva> ListaCurva = new List<RsoCurva>();

            //SACO RANGO FECHAS INICIAL Y FINAL
            DateTime fecha_inicial = (from f in rdo_detalles
                                      where f.fecha_inicio_prevista != null
                                      select f.fecha_inicio_prevista).Min(c => c.Value.Date);
            DateTime fecha_final = (from f in rdo_detalles
                                    where f.fecha_fin_prevista != null
                                    select f.fecha_fin_prevista).Max(c => c.Value.Date);

            //DateTime fecha = fecha_inicial;


            decimal valor = 0;
            decimal valor_prev_acumulado = 0;
            decimal valor_acum = 0;
            decimal valor_previsto_total = 0;

            decimal valor_real = 0;
            decimal valor_real_acumulado = 0;
            decimal porcentaje_previsto = 0;
            decimal porcentaje_previsto_acumuldado = 0;

            decimal monto_total_presupuestado = Decimal.Round(
                                          (from c in rdo_detalles
                                           select Decimal.Round(c.costo_presupuesto, 2)).Sum()
                                            , 2);





            for (DateTime fecha = fecha_inicial; fecha <= fecha_final; fecha = fecha.AddDays(1))
            {
                foreach (var i in rdo_detalles)
                {
                    if (i.Computo.Wbs.fecha_final != null && i.Computo.Wbs.fecha_inicial != null
                       && fecha >= i.Computo.Wbs.fecha_inicial && fecha <= i.Computo.Wbs.fecha_final
                       )
                    {

                        valor = ((i.costo_presupuesto) / monto_total_presupuestado) /
                             ((i.Computo.Wbs.fecha_final - i.Computo.Wbs.fecha_inicial).Value.Days + 1);

                        if (valor < 0)
                        {
                            ElmahExtension.LogToElmah(new Exception("Valor Negativo en Curva " + rdocabecera.Proyecto.nombre_proyecto + " " + valor + "" + i.ComputoId));
                        }

                    }
                    else
                    {
                        valor = 0;
                    }


                    valor_acum = valor_acum + Decimal.Round(valor, 6);

                }

                valor_previsto_total = Decimal.Round(valor_previsto_total + valor_acum, 4);

                //vALOR REAL ACUMULADO

                var valores_reales = _detalleRepository.GetAllIncluding(c => c.RsoCabecera, c => c.Computo)

                                                            .Where(c => c.RsoCabecera.es_definitivo)
                                                            .Where(c => c.RsoCabecera.fecha_rdo == fecha)
                                                           .Where(c => c.vigente)

                                                           .ToList();


                if (valores_reales.Count > 0)
                {


                    var avance_Actual_Acumulado = Decimal.Round(
                                              Decimal.Round((from c in valores_reales select Decimal.Round(c.ev_actual, 4)).Sum(), 4) /
                                              Decimal.Round((from c in valores_reales select Decimal.Round(c.costo_presupuesto, 4)).Sum(), 4)
                                              , 4);


                    valor_real_acumulado = valor_real_acumulado + avance_Actual_Acumulado;

                }
                if (fecha == fecha_final)
                {
                    Decimal.Round(
                valor_real_acumulado = Decimal.Round((from c in rdoDetalles select Decimal.Round(c.ev_actual, 4)).Sum(), 4) /
                                                  Decimal.Round((from c in rdoDetalles select Decimal.Round(c.costo_presupuesto, 4)).Sum(), 4)
                                                  , 4);
                }
                else
                {
                    if (valor_acum > 1)
                    {
                        valor_real_acumulado = valor_real_acumulado - Convert.ToDecimal(0.35);
                    }
                }


                CurvaProyectoRSO a = new CurvaProyectoRSO()
                {
                    ProyectoId = ProyectoId,
                    fecha = fecha,
                    valor_previsto = valor_acum,
                    valor_previsto_acumulado = (valor_previsto_total + valor_acum) < 1 ? valor_previsto_total + valor_acum : 1,
                    valor_real = valor_real,
                    valor_real_acumulado = valor_real_acumulado,

                };



                RsoCurva rc = new RsoCurva
                {
                    fecha = fecha,
                    valor_previsto = valor_acum,
                    valor_previsto_acumulado = (valor_previsto_total + valor_acum) < 1 ? valor_previsto_total + valor_acum : 1,
                    valor_real = valor_real,
                    valor_real_acumulado = valor_real_acumulado,

                };
                ListaCurva.Add(rc);


                var ingreso = _rsocurvaRepository.Insert(a);

                valor = 0;
                valor_acum = 0;



            }




            return ListaCurva;
        }

        public ExcelPackage MontoTotales(int Id, string TipoReporte)
        {
            int precision = 20;
            bool second_format = false;

            ExcelPackage excel = this.GenerarExcelRdo(Id, TipoReporte);
            int filascontrato2 = 0;
            var rdocabecera = Repository.Get(Id);
            var objetoProyecto = _proyectoRepository.Get(rdocabecera.ProyectoId);
            /* Formato Contrato 2*/
            var Project = _proyectoRepository.GetAll().Where(c => c.Id == rdocabecera.ProyectoId).Where(c => c.vigente).FirstOrDefault();
            if (Project != null && Project.Id > 0)
            {

                var contrato = _contratoRepository.GetAll().Where(c => c.Id == Project.contratoId).Where(c => c.vigente).FirstOrDefault();

                if (contrato != null && contrato.Formato.HasValue && contrato.Formato.Value == FormatoContrato.Contrato_2019)
                {
                    filascontrato2 = 11;
                    second_format = true;
                }
            }

            //var rdoCabecerasDefinitivas = _rdocabeceraRepository.GetAllIncluding(c => c.Proyecto)



            var rdoCabecerasDefinitivas = Repository.GetAllIncluding(c => c.Proyecto)
                                .Where(c => c.ProyectoId == rdocabecera.ProyectoId)
                               .Where(c => c.es_definitivo)
                               .Where(c => c.vigente)
                               .Where(c => c.fecha_rdo < rdocabecera.fecha_rdo)
                               .ToList();


            var RDOsCabecerasDefinitivas = _rdocabeceraRepository.GetAllIncluding(c => c.Proyecto)
                                .Where(c => c.ProyectoId == rdocabecera.ProyectoId)
                               .Where(c => c.es_definitivo)
                               .Where(c => c.vigente)
                               .OrderByDescending(c => c.fecha_rdo)
                               .Where(c => c.fecha_rdo <= rdocabecera.fecha_rdo)
                               .ToList();



            var ValoresRealesCurva = _curvaRepository.GetAllIncluding(c => c.Proyecto)
                                        .Where(c => c.ProyectoId == rdocabecera.ProyectoId)
                                        .Where(c => c.dato_migrado)
                                        .Where(c => c.fecha < rdocabecera.fecha_rdo)
                                        .ToList();


            var rdoDetalles = _eacRepository.GetAll()
                                            .Where(c => c.vigente)
                                            .Where(c => c.RsoCabeceraId == Id)
                                            .ToList();






            var lista_disruptivos = _obraDisruptivoRepository.GetAll()
                                                            .Where(c => c.vigente)
                                                            .Where(c => c.ProyectoId == rdocabecera.ProyectoId)
                                                            .Where(c => c.Proyecto.vigente)
                                                            .ToList();

            int diastotal = 0;

            if (lista_disruptivos.Count > 0)
            {
                foreach (var dis in lista_disruptivos)
                {
                    int diasTemp = 0;

                    if (dis.fecha_fin.HasValue)
                    {
                        diasTemp = ((dis.fecha_fin.Value - dis.fecha_inicio.Value).Days + 1) * (dis.porcentaje_disruptivo / 100);
                    }
                    else
                    {
                        diasTemp = ((rdocabecera.fecha_rdo - dis.fecha_inicio.Value).Days + 1) * (dis.porcentaje_disruptivo / 100);
                    }

                    diastotal = diastotal + diasTemp;
                }


            }


            //*/RDO Anterior/

            var rdoAnterior = Repository.GetAll().Where(c => c.vigente)
                                                           .Where(c => c.ProyectoId == rdocabecera.ProyectoId)
                                                           .Where(c => c.es_definitivo)
                                                           .Where(c => c.fecha_rdo < rdocabecera.fecha_rdo)
                                                          .OrderByDescending(c => c.fecha_rdo)
                                                           .FirstOrDefault();

            var detallesRDOAnterior = new List<RsoDetalleEac>();

            if (rdoAnterior != null)
            {

                var list = _detalleRepository.GetAll().Where(c => c.vigente)
                                            .Where(c => c.RsoCabeceraId == rdoAnterior.Id)
                                            .ToList();
                detallesRDOAnterior.AddRange(list);
            }


            MontosTotalesRDO m = new MontosTotalesRDO();

            if (rdoDetalles.Count > 0)
            {
                m.porcentajeBudget =
                    Decimal.Round((from c in rdoDetalles
                                   select Decimal.Round(c.porcentaje_presupuesto_total

            , precision)).Sum(), precision);
                m.porcentajeEAC =
                    Decimal.Round((from c in rdoDetalles select Decimal.Round(c.porcentaje_costo_eac_total, precision)).Sum(), precision);
                m.costoBudget =
                    Decimal.Round((from c in rdoDetalles select Decimal.Round(c.costo_presupuesto, precision)).Sum(), precision);

                m.costoEAC =
                    Decimal.Round((from c in rdoDetalles select Decimal.Round(c.costo_eac, precision)).Sum(), precision);

                m.ac_anterior =
                    Decimal.Round((from c in rdoDetalles select Decimal.Round(c.ac_anterior, precision)).Sum(), precision);
                m.ac_diario =
                    Decimal.Round((from c in rdoDetalles select Decimal.Round(c.ac_diario, precision)).Sum(), precision);
                m.ac_actual =
                    Decimal.Round((from c in rdoDetalles select Decimal.Round(c.ac_actual, precision)).Sum(), precision);
                m.ev_actual =
                    Decimal.Round((from c in rdoDetalles select Decimal.Round(c.ev_actual, precision)).Sum(), precision);

                m.ern_value =
                    Decimal.Round((from c in rdoDetalles select Decimal.Round(c.ern_value, precision)).Sum(), precision);

                m.ev_diario =
                    Decimal.Round((from c in rdoDetalles select Decimal.Round(c.ev_diario, precision)).Sum(), precision);

                m.ev_anterior =
                    Decimal.Round(
                                            (from c in rdoDetalles

                                             select Decimal.Round(c.ev_anterior, precision)).Sum()
                                             , precision);
                m.pv_costo_planificado = Decimal.Round(
                                             (from c in rdoDetalles

                                              select Decimal.Round(c.pv_costo_planificado, precision)).Sum()
                                             , precision);
                m.fecha_inicio_prevista = (from c in rdoDetalles
                                           where c.fecha_inicio_prevista != null

                                           select c.fecha_inicio_prevista).Min(c => c.Value);
                m.fecha_fin_prevista = (from c in rdoDetalles
                                        where c.fecha_fin_prevista != null

                                        select c.fecha_fin_prevista).Max(c => c.Value);

                m.fecha_inicio_real = (from c in rdoDetalles
                                       where c.fecha_inicio_real != null

                                       select c.fecha_inicio_real).Count() > 0 ?
                                       (from c in rdoDetalles
                                        where c.fecha_inicio_real != null

                                        select c.fecha_inicio_real).Min(c => c.Value) : new DateTime(1999, 01, 01);


                var fecha_reales = (from c in rdoDetalles
                                    where c.fecha_fin_real != null

                                    select c.fecha_fin_real).ToList();
                if (fecha_reales.Count > 0)
                {
                    m.fecha_fin_real = (from c in rdoDetalles
                                        where c.fecha_fin_real != null

                                        select c.fecha_fin_real).Max(c => c.Value);
                }
                else
                {
                    m.fecha_fin_real = new DateTime(1999, 01, 01);
                }

                decimal costo_presupuesto = Decimal.Round((from c in rdoDetalles

                                                           select Decimal.Round(c.costo_presupuesto, precision)).Sum(), precision);

               /* decimal costo_presupuesto_version_anterior = Decimal.Round((from c in detallesRDOAnterior

                                                                            select Decimal.Round(c.costo_presupuesto, precision)).Sum(), precision);

                if (costo_presupuesto_version_anterior > 0)
                {
                    m.avance_Acumulado_Anterior = Decimal.Round(
                                                     Decimal.Round((from c in detallesRDOAnterior

                                                                    select Decimal.Round(c.ev_actual, precision)).Sum(), precision) / costo_presupuesto_version_anterior, precision);
                }*/

                if (costo_presupuesto > 0)
                {
                      m.avance_Acumulado_Anterior = Decimal.Round(
                                                          Decimal.Round((from c in rdoDetalles

                                                                         select Decimal.Round(c.ev_anterior, precision)).Sum(), precision) / costo_presupuesto, precision);



                    m.avance_Actual_Acumulado = Decimal.Round(
                                                      Decimal.Round((from c in rdoDetalles

                                                                     select Decimal.Round(c.ev_actual, precision)).Sum(), precision) /
                                                     costo_presupuesto, precision);

                    m.Avance_Previsto_Acumulado = Decimal.Round(
                                                    Decimal.Round((from c in rdoDetalles

                                                                   select Decimal.Round(c.pv_costo_planificado, precision)).Sum(), precision) /
                                                  costo_presupuesto
                                                            , precision);

                    /*   m.avance_Diario = Decimal.Round(
                                                     Decimal.Round((from c in rdoDetalles

                                                                    select Decimal.Round(c.ev_diario, precision)).Sum(), precision) /
                                                    costo_presupuesto
                                                    , precision);
                                                    */

                    m.avance_Diario = Decimal.Round(m.avance_Actual_Acumulado - m.avance_Acumulado_Anterior
                                                    , precision);
                }

                if (Decimal.Round((from c in rdoDetalles

                                   select Decimal.Round(c.pv_costo_planificado, precision)).Sum(), precision) == 0)
                {
                    m.spi = 1;
                }
                else
                {
                    m.spi = Decimal.Round(Decimal.Round((from c in rdoDetalles

                                                         select Decimal.Round(c.ev_actual, precision)).Sum(), precision) /
                                     Decimal.Round((from c in rdoDetalles

                                                    select Decimal.Round(c.pv_costo_planificado, precision)).Sum(), precision), precision);
                }
                decimal ac_actual = Decimal.Round((from c in rdoDetalles

                                                   select Decimal.Round(c.ac_actual, precision)).Sum(), precision);

                if (ac_actual > 0)
                {
                    m.cpi = Decimal.Round(Decimal.Round((from c in rdoDetalles

                                                         select Decimal.Round(c.ev_actual, precision)).Sum(), precision) /
                                      ac_actual
                                        , precision);
                }
                else
                {
                    m.cpi = 1; // En caso de que no exista avance
                }

                m.plazo_previsto = ((from c in rdoDetalles
                                     where c.fecha_fin_prevista != null

                                     select c.fecha_fin_prevista).Max(c => c.Value) - (from c in rdoDetalles
                                                                                       where c.fecha_inicio_prevista != null

                                                                                       select c.fecha_inicio_prevista).Min(c => c.Value)).Days;
                ;
                //numero_horas 
                m.plazo_ajustado =
                    diastotal + Convert.ToDecimal(((from c in rdoDetalles
                                                    where c.fecha_fin_prevista != null

                                                    select c.fecha_fin_prevista).Max(c => c.Value) - (from c in rdoDetalles
                                                                                                      where c.fecha_inicio_prevista != null

                                                                                                      select c.fecha_inicio_prevista).Min(c => c.Value)).Days);
            }

            ExcelWorksheet hoja = excel.Workbook.Worksheets[1];
            ExcelWorksheet hojaAdicionales = excel.Workbook.Worksheets[2];
            #region Montos Totales
            //Porcentaje Budget
            hoja.Cells["H16"].Value = m.porcentajeBudget;
            hoja.Cells["H16"].Style.Numberformat.Format = "#0.00%";
            hoja.Cells["H16"].Style.Font.Color.SetColor(Color.White);
            hoja.Cells["H16"].Style.Font.Size = 17;


            //Porcentaje EAc
            hoja.Cells["I16"].Value = m.porcentajeEAC;
            hoja.Cells["I16"].Style.Numberformat.Format = "#0.00%";
            hoja.Cells["I16"].Style.Font.Color.SetColor(Color.White);
            hoja.Cells["I16"].Style.Font.Size = 17;

            //Costo Budget
            hoja.Cells["N16"].Value = m.costoBudget;


            //Costo EAc
            hoja.Cells["O16"].Value = m.costoEAC;

            //Ac Anterior
            hoja.Cells["P16"].Value = m.ac_anterior;

            //Ac diario

            hoja.Cells["Q16"].Value = m.ac_diario;

            //Ac Actual
            hoja.Cells["R16"].Value = m.ac_actual;

            //ev anterior
            hoja.Cells["S16"].Value = m.ev_anterior;

            //ev diario
            hoja.Cells["T16"].Value = m.ev_diario;
            //ev actual
            hoja.Cells["U16"].Value = m.ev_actual;

            hoja.Cells["V16"].Value = m.ern_value;



            // pv costos planificado

            hoja.Cells["W16"].Value = m.pv_costo_planificado;

            for (int i = 14; i <= 23; i++)
            {
                hoja.Cells[16, i].Style.WrapText = true;
                hoja.Cells[16, i].Style.Font.Size = 17;
                hoja.Cells[16, i].Style.Numberformat.Format = "#,##0.00";

                hoja.Cells[16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                hoja.Cells[16, i].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
            for (int i = 31; i <= 34; i++)
            {
                hoja.Cells[16, i].Style.WrapText = true;
                hoja.Cells[16, i].Style.Font.Size = 17;
                hoja.Cells[16, i].Style.Numberformat.Format = "#0.00%";
                hoja.Cells[16, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                hoja.Cells[16, i].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }


            // fecha inicio previsto

            hoja.Cells["X16"].Value = m.fecha_inicio_prevista;
            hoja.Cells["X16"].Style.WrapText = true;
            hoja.Cells["X16"].Style.Font.Size = 17;
            hoja.Cells["X16"].Style.Numberformat.Format = "dd-mmm-yy";
            hoja.Cells["X16"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            hoja.Cells["X16"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            //fecha fin previsto
            hoja.Cells["Y16"].Value = m.fecha_fin_prevista;
            hoja.Cells["Y16"].Style.WrapText = true;
            hoja.Cells["Y16"].Style.Font.Size = 17;
            hoja.Cells["Y16"].Style.Numberformat.Format = "dd-mmm-yy";
            hoja.Cells["Y16"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            hoja.Cells["Y16"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            // fecha inicio previsto
            hoja.Cells["Z16"].Style.Numberformat.Format = "dd-mmm-yy";
            hoja.Cells["Z16"].Value = m.fecha_inicio_real != new DateTime(1999, 01, 01) ? "" + m.fecha_inicio_real.Value.ToString("dd-MMM-yy") : "";
            hoja.Cells["Z16"].Style.WrapText = true;
            hoja.Cells["Z16"].Style.Font.Size = 17;
            hoja.Cells["Z16"].Style.Numberformat.Format = "dd-mmm-yy";
            hoja.Cells["Z16"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            hoja.Cells["Z16"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            if (m.avance_Actual_Acumulado == 1)
            {
                //fecha fin previsto
                if (m.fecha_fin_real == new DateTime(1999, 01, 01))
                {
                    hoja.Cells["AA16"].Value = "";
                }
                else
                {
                    hoja.Cells["AA16"].Value = m.fecha_fin_real;

                    hoja.Cells["AA16"].Style.WrapText = true;
                    hoja.Cells["AA16"].Style.Numberformat.Format = "dd-mmm-yy";
                }

                hoja.Cells["AA16"].Style.WrapText = true;
                hoja.Cells["AA16"].Style.Font.Size = 17;
                hoja.Cells["AA16"].Style.Numberformat.Format = "dd-mmm-yy";
                hoja.Cells["AA16"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                hoja.Cells["AA16"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            // Avance Acumulado Anterior 
            hoja.Cells["AE16"].Value = m.avance_Acumulado_Anterior;

            // Avance Diario
            hoja.Cells["AF16"].Value = m.avance_Diario;

            // Avance Actual Acumulado
            hoja.Cells["AG16"].Value = m.avance_Actual_Acumulado;


            // Avance Previsto Acumulado
            hoja.Cells["AH16"].Value = m.Avance_Previsto_Acumulado;

            hoja.Cells["C9:F9"].Value = m.fecha_inicio_prevista;
            hoja.Cells["C9:F9"].Style.Numberformat.Format = "dd-mmm-yy";
            hoja.Cells["G9:H9"].Value = m.fecha_fin_prevista;
            hoja.Cells["G9:H9"].Style.Numberformat.Format = "dd-mmm-yy";

            var fechaPrimerRDO = Repository.GetAll()
                          .Where(c => c.ProyectoId == rdocabecera.ProyectoId)
                          .Where(c => c.vigente)
                          .Where(c => c.es_definitivo)
                          .OrderBy(c => c.fecha_rdo).FirstOrDefault();


            hoja.Cells["I9:V9"].Merge = true;
            //hoja.Cells["I9:V9"].Value = m.fecha_inicio_real;
            hoja.Cells["I9:V9"].Value = fechaPrimerRDO != null ? fechaPrimerRDO.fecha_rdo : m.fecha_fin_real;
            hoja.Cells["I9:V9"].Style.Numberformat.Format = "dd-mmm-yy";

            if (m.avance_Actual_Acumulado == 1)
            {
                hoja.Cells["X9:Z9"].Merge = true;
                hoja.Cells["X9:Z9"].Value = m.fecha_fin_real;
                hoja.Cells["X9:Z9"].Style.Numberformat.Format = "dd-mmm-yy";

                hoja.Cells["X9:Z9"].Style.WrapText = true;
                hoja.Cells["X9:Z9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                hoja.Cells["X9:Z9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            }
            else
            {
                hoja.Cells["X9:Z9"].Merge = true;
                hoja.Cells["X9:Z9"].Value = ".";
                hoja.Cells["X9:Z9"].Style.Numberformat.Format = "dd-mmm-yy";

                hoja.Cells["X9:Z9"].Style.WrapText = true;
                hoja.Cells["X9:Z9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                hoja.Cells["X9:Z9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }
            bool tieneItemsPendientes = rdoDetalles.Where(c => c.PendienteAprobacion || c.es_temporal).ToList().Count > 0 ? true : false;

            hoja.Cells["C16:AJ16"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["C16:AJ16"].Style.Fill.BackgroundColor.SetColor(Color.Black);
            hoja.Cells["C16:AJ16"].Style.Font.Color.SetColor(Color.White);

            hoja.Cells["E16:G16"].Merge = true;
            hoja.Cells["E16:G16"].Value = tieneItemsPendientes ? "CONSTRUCCIÓN + ITEMS PENDIENTES" : "CONSTRUCCIÓN";
            hoja.Cells["E16:G16"].Style.WrapText = true;
            hoja.Cells["E16:G16"].Style.Font.Size = 18;
            hoja.Cells["E16:G16"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["E16:G16"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Row(16).Height = 34;
            hoja.Cells["AG9"].Value = m.spi;

            hoja.Cells["AG9"].Style.WrapText = true;
            hoja.Cells["AG9"].Style.Font.Size = 17;
            hoja.Cells["AG9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            hoja.Cells["AG9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["AG9"].Style.Numberformat.Format = "#,##0.00";



            double spi = decimal.ToDouble(m.spi);


            if (spi >= 1.0)
            {
                ExcelRange rng = hoja.Cells["AG9"];
                ExcelAddress address = new ExcelAddress(rng.Address);
                var v = hoja.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                v.Icon2.Value = 0.85;
                v.Icon3.Value = 1.0;
                v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;


            }
            else if (spi > 1.0 && spi <= 0.85)
            {
                ExcelRange rng = hoja.Cells["AG9"];
                ExcelAddress address = new ExcelAddress(rng.Address);

                var v = hoja.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                v.Icon2.Value = 0.85;
                v.Icon3.Value = 1.0;
                v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;

            }
            else //(spi < 0.85)
            {
                ExcelRange rng = hoja.Cells["AG9"];
                ExcelAddress address = new ExcelAddress(rng.Address);
                var v = hoja.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                v.Icon2.Value = 0.85;
                v.Icon3.Value = 1.0;
                v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;
            }


            hoja.Cells["AH9"].Value = m.cpi;
            hoja.Cells["AH9"].Style.WrapText = true;
            hoja.Cells["AH9"].Style.Font.Size = 17;
            hoja.Cells["AH9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            hoja.Cells["AH9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["AH9"].Style.Numberformat.Format = "#,##0.00";


            double cpi = decimal.ToDouble(m.cpi);
            if (cpi >= 1.0)
            {
                ExcelRange rng = hoja.Cells["AH9"];
                ExcelAddress address = new ExcelAddress(rng.Address);
                var v = hoja.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                v.Icon2.Value = 0.85;
                v.Icon3.Value = 1.0;
                v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;
            }
            else if (cpi > 1.0 && cpi <= 0.85)
            {
                ExcelRange rng = hoja.Cells["AH9"];
                ExcelAddress address = new ExcelAddress(rng.Address);
                var v = hoja.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                v.Icon2.Value = 0.85;
                v.Icon3.Value = 1.0;
                v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;
            }
            else
            {
                ExcelRange rng = hoja.Cells["AH9"];
                ExcelAddress address = new ExcelAddress(rng.Address);
                var v = hoja.ConditionalFormatting.AddThreeIconSet(address, eExcelconditionalFormatting3IconsSetType.TrafficLights1);
                v.Icon2.Value = 0.85;
                v.Icon3.Value = 1.0;
                v.Icon1.Type = eExcelConditionalFormattingValueObjectType.Num;
                v.Icon2.Type = eExcelConditionalFormattingValueObjectType.Num;
                v.Icon3.Type = eExcelConditionalFormattingValueObjectType.Num;
            }



            hoja.Cells["AC9:AD9"].Value = diastotal;// *DIAS_TOTALES_DISRUPTIVO_CABECERA
            hoja.Cells["AC9:AD9"].Style.WrapText = true;
            hoja.Cells["AC9:AD9"].Style.Font.Size = 17;
            hoja.Cells["AC9:AD9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["AC9:AD9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            hoja.Cells["AA9:AB9"].Value = m.plazo_previsto;
            hoja.Cells["AA9:AB9"].Style.WrapText = true;
            hoja.Cells["AA9:AB9"].Style.Font.Size = 17;
            hoja.Cells["AA9:AB9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["AA9:AB9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            hoja.Cells["AE9:AF9"].Value = m.plazo_ajustado;
            hoja.Cells["AE9:AF9"].Style.WrapText = true;
            hoja.Cells["AE9:AF9"].Style.Font.Size = 17;
            hoja.Cells["AE9:AF9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["AE9:AF9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            hoja.Column(25).Width = 28;

            var cabecera = Repository.Get(Id); //Cabecera De Rdo;

            hoja.Cells["I7:Z7"].Merge = true;
            if (cabecera.fecha_rdo != null && m.fecha_inicio_prevista != null && cabecera.fecha_rdo >= m.fecha_inicio_prevista)
            {

                hoja.Cells["I7:Z7"].Value = objetoProyecto.es_RSO ? "DOCUMENTO NO: SHY-FC-RSO-" + ((cabecera.fecha_rdo - m.fecha_inicio_prevista).Days - 1) : "DOCUMENTO NO: SHY-FC-RDO-" + ((cabecera.fecha_rdo - m.fecha_inicio_prevista).Days - 1);
            }
            else
            {
                hoja.Cells["I7:Z7"].Value = objetoProyecto.es_RSO ? "DOCUMENTO NO: SHY-FC-RSO-" : "DOCUMENTO NO: SHY-FC-RDO-";
            }
            //hoja.Cells["I7:Z7"].Style.WrapText = true;
            hoja.Cells["I7:Z7"].Style.Font.Bold = true;
            hoja.Cells["I7:Z7"].Style.Font.Size = 17;
            hoja.Cells["I7:Z7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["I7:Z7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C7:AJ7"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            string patharbolecuador = System.Web.HttpContext.Current.Server.MapPath("~/Views/LogosCPP/_arbolecuador.png");
            string pathpetroamazonas = System.Web.HttpContext.Current.Server.MapPath("~/Views/LogosCPP/_petroamazonas.png");
            string pathpetroEcuador = System.Web.HttpContext.Current.Server.MapPath("~/Views/LogosCPP/_petroecuador2.png");
            string pathcpp = System.Web.HttpContext.Current.Server.MapPath("~/Views/LogosCPP/_cpp.png");
            if (File.Exists((string)patharbolecuador))
            {
                Image _logoarbol = Image.FromFile(patharbolecuador);
                var picture = hoja.Drawings.AddPicture("arbolecuad", _logoarbol);
                picture.SetPosition(2, 0, 2, 0);
            }
            if (File.Exists((string)pathcpp))
            {
                Image _logocpp = Image.FromFile(pathcpp);
                var picture = hoja.Drawings.AddPicture("cpp", _logocpp);
                picture.SetPosition(2, 0, 4, 0);
            }

            if (objetoProyecto.usar_logo_prederminado)
            {
                if (File.Exists((string)pathpetroamazonas))
                {
                    Image _logopretro = Image.FromFile(pathpetroamazonas);
                    var picture = hoja.Drawings.AddPicture("pretroamazonas", _logopretro);
                    picture.SetPosition(2, 0, 32, 0);
                }
            }
            else
            {
                if (File.Exists((string)pathpetroEcuador))
                {
                    Image _logopetroEcuador = Image.FromFile(pathpetroEcuador);
                    var picture = hoja.Drawings.AddPicture("pretroecuador", _logopetroEcuador);
                    //picture.SetSize(440, 134);
                    picture.SetSize(410, 52);
                    //picture.SetPosition(2, 0, 31, 0);
                    picture.SetPosition(3, 0, 31, 0);

                }
            }


            #endregion


            // Detalles del Proyecto
            var proyecto = _proyectoRepository.Get(cabecera.ProyectoId);

            var sheet = excel.Workbook.Worksheets.Add("CURVA");
            sheet.View.ZoomScale = 40;

            //Cabeceras Proyecto
            sheet.Cells["B2:L2"].Merge = true;
            sheet.Cells["B2:L2"].Style.WrapText = true;
            sheet.Cells["B2:L2"].Style.Font.Bold = true;
            sheet.Cells["B2:L2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["B2:L2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells["B2:L2"].Style.Font.Size = 18;

            sheet.Cells["B2:L2"].Value = "PROYECTO " + proyecto.codigo + " " + proyecto.descripcion_proyecto;

            //ESPACIO

            sheet.Cells["B3:D3"].Merge = true;

            //Fechas Proyecto
            sheet.Cells["B4:D4"].Merge = true;
            sheet.Cells["B4:D4"].Style.WrapText = true;
            sheet.Cells["B4:D4"].Style.Font.Size = 12;
            sheet.Cells["B4:D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["B4:D4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells["B4:D4"].Value = "Fecha Inicial: " + m.fecha_inicio_prevista;

            sheet.Cells["B5:D5"].Merge = true;
            sheet.Cells["B5:D5"].Style.WrapText = true;
            sheet.Cells["B5:D5"].Style.Font.Size = 12;
            sheet.Cells["B5:D5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["B5:D5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells["B5:D5"].Value = "Fecha Fin: " + m.fecha_fin_prevista;

            sheet.Cells["B6:D6"].Merge = true;
            sheet.Cells["B6:D6"].Style.WrapText = true;
            sheet.Cells["B6:D6"].Style.Font.Size = 12;
            sheet.Cells["B6:D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["B6:D6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet.Cells["B6:D6"].Value = "Fecha Generación: " + DateTime.Today.ToShortDateString();


            //ESPACIO

            sheet.Cells["B7:D7"].Merge = true;

            //CABECERA NOMBRES
            sheet.Column(2).Width = 15;
            sheet.Column(3).Width = 20;
            sheet.Column(4).Width = 20;

            sheet.Cells["B8"].Value = "Fecha";
            sheet.Cells["B8"].Style.Font.Size = 9;
            sheet.Cells["B8"].Style.WrapText = true;
            sheet.Cells["B8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["B8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells["B8"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells["B8"].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
            sheet.Cells["B8"].Style.Font.Color.SetColor(Color.White);

            sheet.Cells["C8"].Value = "% Previsto Acumulado";
            sheet.Cells["C8"].Style.Font.Size = 9;
            sheet.Cells["C8"].Style.WrapText = true;
            sheet.Cells["C8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["C8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells["C8"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells["C8"].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
            sheet.Cells["C8"].Style.Font.Color.SetColor(Color.White);


            sheet.Cells["D8"].Value = "% Real Acumulado";
            sheet.Cells["D8"].Style.WrapText = true;
            sheet.Cells["D8"].Style.Font.Size = 9;
            sheet.Cells["D8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["D8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells["D8"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells["D8"].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
            sheet.Cells["D8"].Style.Font.Color.SetColor(Color.White);



            var curva_datos = _curvaRepository.GetAll().Where(z => z.ProyectoId == cabecera.ProyectoId)
                .Where(c => !c.dato_migrado).ToList();
            _curvaRepository.Delete(curva_datos);


            List<RdoCurva> ListaCurva = new List<RdoCurva>();



            //SACO RANGO FECHAS INICIAL Y FINAL

            var ultimoRDOGenerado = _rdocabeceraRepository.GetAllIncluding(c => c.Proyecto)
                          .Where(c => c.ProyectoId == rdocabecera.ProyectoId)
                         .Where(c => c.es_definitivo)
                         .Where(c => c.vigente)
                         .Where(c => c.fecha_rdo <= rdocabecera.fecha_rdo)
                         .OrderByDescending(c => c.fecha_rdo)
                         .FirstOrDefault();

            var rdoDetallesPrevisto = _eacRDORepository.GetAllIncluding(c => c.Computo.Wbs)
                                 .Where(c => c.vigente)
                                 .Where(c => c.RdoCabeceraId == ultimoRDOGenerado.Id)
                                 .ToList();



            /* DateTime fecha_inicial = (from f in rdoDetalles
                                       where f.fecha_inicio_prevista != null

                                       select f.fecha_inicio_prevista).Min(c => c.Value.Date);
                                       */
            /* 
           DateTime fecha_final = (from f in rdoDetalles
                                   where f.fecha_fin_prevista != null

                                   select f.fecha_fin_prevista).Max(c => c.Value.Date); */
            DateTime fecha_inicial = (from f in rdoDetallesPrevisto
                                      where f.fecha_inicio_prevista != null
                                      where f.fecha_inicio_prevista.HasValue

                                      select f.fecha_inicio_prevista).Min(c => c.Value.Date);



            DateTime fecha_final = (from f in rdoDetallesPrevisto
                                    where f.fecha_fin_prevista != null
                                    where f.fecha_fin_prevista.HasValue
                                    select f.fecha_fin_prevista).Max(c => c.Value.Date);

            DateTime fecha_inicial_real = rdocabecera.fecha_rdo.Date;

            var fechas_reales = (from f in rdoDetalles
                                 where f.fecha_inicio_real != null
                                 select f.fecha_inicio_real).ToList();
            if (fechas_reales.Count > 0)
            {
                fecha_inicial_real = fechas_reales.Min(c => c.Value.Date);
            }










            decimal valor = 0;
            decimal valor_prev_acumulado = 0;
            decimal valor_acum = 0;
            decimal valor_previsto_total = 0;

            decimal valor_real = 0;
            decimal valor_real_acumulado = 0;
            decimal porcentaje_previsto = 0;
            decimal porcentaje_previsto_acumuldado = 0;




            decimal monto_total_presupuestado = Decimal.Round(
                                              (from c in rdoDetallesPrevisto

                                               select Decimal.Round(c.costo_presupuesto, 8)).Sum()
                                                , 8);


            var avancesobra = _davanceRepository.GetAllIncluding(o => o.AvanceObra.Oferta)
            .Where(o => o.vigente == true)
            .Where(o => o.AvanceObra.Oferta.ProyectoId == cabecera.ProyectoId) // Agregado
                                                                               //.Where(o => o.ComputoId == computoId)
            .Where(o => o.AvanceObra.aprobado == true)
            .Where(o => o.AvanceObra.vigente == true).ToList();
            //.Where(o => o.AvanceObra.fecha_presentacion < fecha_reporte);

            if (fecha_final != null && rdocabecera.fecha_rdo > fecha_final)
            {
                fecha_final = rdocabecera.fecha_rdo;
            }


            decimal tempmonto_total_presupuestado;

            DateTime fechaInicio = fecha_inicial_real != null ? fecha_inicial < fecha_inicial_real ? fecha_inicial : fecha_inicial_real : DateTime.Now;

            var fechas = new List<DateTime>();
            fechas.AddRange(rdoCabecerasDefinitivas.Select(c => c.fecha_rdo).OrderBy(c => c).ToList());
            /*
                        foreach (var fecha in fechas)
                        {*/

            DateTime fecha_anterior_rso = new DateTime(1990, 01, 01);
            DateTime fecha_siguiente_rso = new DateTime(1990, 01, 01);
            RsoCabecera anteriorRSO = new RsoCabecera();
            var SiguienteRDO = new RsoCabecera();
            decimal valor_acumulado_rso_anterior = 0;
            decimal valor_promedio = 0;

            ElmahExtension.LogToElmah(new Exception("Fecha FInAl RSO" + fecha_final.ToShortDateString()));

            for (DateTime fecha = fechaInicio; fecha <= fecha_final; fecha = fecha.AddDays(1))
            {


                valor = 0;
                valor_acum = 0;
                foreach (var i in rdoDetallesPrevisto)
                {
                    if (i.Computo.Wbs.fecha_final != null && i.Computo.Wbs.fecha_inicial != null
                       && fecha.Date >= i.Computo.Wbs.fecha_inicial.Value.Date && fecha <= i.Computo.Wbs.fecha_final.Value.Date
                       )
                    {

                        valor = ((i.costo_presupuesto) / monto_total_presupuestado) /
                                 ((i.Computo.Wbs.fecha_final.Value.Date - i.Computo.Wbs.fecha_inicial.Value.Date).Days + 1);


                    }
                    else
                    {
                        valor = 0;
                    }

                    valor_acum = valor_acum + valor;

                }


                valor_previsto_total = valor_previsto_total + valor_acum;

                var CabeceraDefinita = (from rd in rdoCabecerasDefinitivas
                                        where rd.fecha_rdo == fecha
                                        where rd.ProyectoId == rdocabecera.ProyectoId
                                        select rd).FirstOrDefault();
                var RDOCabeceraDeRDOS = (from rd in RDOsCabecerasDefinitivas
                                         where rd.fecha_rdo == fecha
                                         where rd.ProyectoId == rdocabecera.ProyectoId
                                         select rd).FirstOrDefault();


                var ValoresCurvasMigrados = (from l in ValoresRealesCurva where l.fecha == fecha where l.valor_real_acumulado > 0 select l).FirstOrDefault();



                /* Avance Real de RDOS desde RDO Cabeceras*/
                if (RDOCabeceraDeRDOS != null && RDOCabeceraDeRDOS.Id > 0)
                {
                    valor_real_acumulado = RDOCabeceraDeRDOS.avance_real_acumulado;
                }


                /*Si Existe valor  Migrado*/
                if (ValoresCurvasMigrados != null && ValoresCurvasMigrados.valor_real_acumulado > 0)
                {
                    valor_real_acumulado = ValoresCurvasMigrados.valor_real_acumulado;
                }

                /*SI Existe RSO Generado*/
                if (CabeceraDefinita != null && CabeceraDefinita.Id > 0)
                {

                    if (CabeceraDefinita.fecha_rdo > fecha_anterior_rso)
                    {
                        anteriorRSO = CabeceraDefinita;
                        fecha_anterior_rso = CabeceraDefinita.fecha_rdo;
                    }

                    valor_real_acumulado = CabeceraDefinita.avance_real_acumulado;




                }

                if (rdocabecera.fecha_rdo == fecha)
                {
                    valor_real_acumulado = rdocabecera.avance_real_acumulado;
                }
                if (fecha > rdocabecera.fecha_rdo)
                {
                    valor_real_acumulado = -1;
                }



                if (anteriorRSO != null && anteriorRSO.Id > 0)
                {

                    SiguienteRDO = Repository.GetAll().Where(c => c.fecha_rdo > anteriorRSO.fecha_rdo)
                                                       .Where(c => c.Id != anteriorRSO.Id)
                                                       .Where(c => c.ProyectoId == rdocabecera.ProyectoId)
                                                       .OrderBy(c => c.fecha_rdo)
                                                       .Where(c => c.vigente)

                                                   .Where(c => c.fecha_rdo <= rdocabecera.fecha_rdo)
                                                       .Where(c => c.es_definitivo)
                                                       .FirstOrDefault();



                    if (SiguienteRDO != null && SiguienteRDO.Id > 0)
                    {
                        fecha_siguiente_rso = SiguienteRDO.fecha_rdo;

                        var porcentaje = SiguienteRDO.avance_real_acumulado - anteriorRSO.avance_real_acumulado;
                        var numerodias = (SiguienteRDO.fecha_rdo - anteriorRSO.fecha_rdo).Days;
                        decimal promediosuma = 0;

                        if (numerodias > 0)
                        {
                            promediosuma = porcentaje / numerodias;


                        }



                        DateTime fechaRSO = fecha;

                        for (DateTime fecharsoInicial = fechaRSO; fecharsoInicial <= fecha_siguiente_rso; fecharsoInicial = fecharsoInicial.AddDays(1))
                        {

                            valor = 0;
                            valor_acum = 0;

                            if (fecharsoInicial.Date != fecha.Date)
                            {
                                foreach (var i in rdoDetallesPrevisto)
                                {
                                    if (i.Computo.Wbs.fecha_final != null && i.Computo.Wbs.fecha_inicial != null
                                       && fecharsoInicial.Date >= i.Computo.Wbs.fecha_inicial.Value.Date && fecharsoInicial <= i.Computo.Wbs.fecha_final.Value.Date
                                       )
                                    {

                                        valor = ((i.costo_presupuesto) / monto_total_presupuestado) /
                                                 ((i.Computo.Wbs.fecha_final.Value.Date - i.Computo.Wbs.fecha_inicial.Value.Date).Days + 1);


                                    }
                                    else
                                    {
                                        valor = 0;
                                    }

                                    valor_acum = valor_acum + valor;

                                }

                                valor_previsto_total = valor_previsto_total + valor_acum;
                            }
                            bool fontbold = false;
                            if (fechaRSO == anteriorRSO.fecha_rdo)
                            {
                                valor_real_acumulado = anteriorRSO.avance_real_acumulado;
                                fontbold = true;
                            }
                            else
                            {

                                valor_real_acumulado = valor_real_acumulado + promediosuma;
                            }

                            if (fechaRSO == SiguienteRDO.fecha_rdo)
                            {
                                fontbold = true;
                                valor_real_acumulado = SiguienteRDO.avance_real_acumulado; //AGREGUE

                            }

                            CurvasProyecto real = new CurvasProyecto()
                            {
                                ProyectoId = cabecera.ProyectoId,
                                fecha = fecharsoInicial,
                                valor_previsto = valor_acum,
                                valor_previsto_acumulado = (valor_previsto_total + valor_acum) < 1 ? valor_previsto_total : 1,
                                valor_real = valor_real,
                                valor_real_acumulado = valor_real_acumulado,
                                dato_migrado = false
                            };
                            RdoCurva rsoc = new RdoCurva
                            {
                                fecha = fecharsoInicial,
                                valor_previsto = valor_acum,
                                valor_previsto_acumulado = (valor_previsto_total + valor_acum) < 1 ? valor_previsto_total : 1,
                                valor_real = valor_real,
                                valor_real_acumulado = valor_real_acumulado,
                                esCabeceraDefinitiva = fontbold

                            };
                            ListaCurva.Add(rsoc);

                            _curvaRepository.Insert(real);


                            CabeceraDefinita = null;



                            fechaRSO = fechaRSO.AddDays(1);
                            fecha = fecharsoInicial;
                            valor_previsto_total = real.valor_previsto_acumulado;
                        }
                        // fecha = fechaRSO.AddDays(-1);
                        anteriorRSO = SiguienteRDO;
                        valor_real_acumulado = SiguienteRDO.avance_real_acumulado;
                    }

                    if (SiguienteRDO == null)
                    {
                        valor_previsto_total = valor_previsto_total - valor_acum;
                        valor = 0;
                        valor_acum = 0;
                        fecha = fecha.AddDays(-1);

                        anteriorRSO = new RsoCabecera();
                        continue;
                    }
                    else
                    {

                        valor = 0;
                        valor_acum = 0;
                        continue;
                    }



                }






                // if (fecha <=fecha_final) {
                CurvasProyecto a = new CurvasProyecto()
                {
                    ProyectoId = cabecera.ProyectoId,
                    fecha = fecha,
                    valor_previsto = valor_acum,
                    valor_previsto_acumulado = (valor_previsto_total + valor_acum) < 1 ? valor_previsto_total : 1,
                    valor_real = valor_real,
                    valor_real_acumulado = valor_real_acumulado,
                    dato_migrado = false
                };
                RdoCurva rc = new RdoCurva
                {
                    fecha = fecha,
                    valor_previsto = valor_acum,
                    valor_previsto_acumulado = (valor_previsto_total + valor_acum) < 1 ? valor_previsto_total : 1,
                    valor_real = valor_real,
                    valor_real_acumulado = valor_real_acumulado,

                };
                ListaCurva.Add(rc);


                var ingreso = _curvaRepository.Insert(a);

                valor = 0;
                valor_acum = 0;

                // ElmahExtension.LogToElmah(new Exception("Fecha FInal" + fecha_final.ToShortDateString()));
                //  }
            }

            if (ListaCurva.Count > 0)
            {

                var primeroLista = ListaCurva.FirstOrDefault();
                RdoCurva Adicional_Especial = new RdoCurva
                {
                    fecha = primeroLista.fecha.AddDays(-1), // FECHA INICIAL -1
                    valor_previsto = 0,
                    valor_previsto_acumulado = 0,
                    valor_real = 0,
                    valor_real_acumulado = 0,
                };

                if (primeroLista.valor_real_acumulado > 0)
                { // SOLO CUANDO LA FECHA INICIAL INICIE CON EL PORCENTAJE DE AVANCE REAL >0
                    ListaCurva.Add(Adicional_Especial);
                }
                int i = 9;
                foreach (var d in ListaCurva.OrderBy(c => c.fecha))
                {

                    sheet.Cells[i, 2].Value = d.fecha;
                    sheet.Cells[i, 2].Style.Numberformat.Format = "dd-mmm-yy";
                    if (d.esCabeceraDefinitiva)
                    {
                        sheet.Cells[i, 2].Style.Font.Bold = true;
                    }


                    sheet.Cells[i, 3].Value = Decimal.Round(d.valor_previsto_acumulado, precision);
                    sheet.Cells[i, 3].Style.Numberformat.Format = "#0.000%";
                    if (d.esCabeceraDefinitiva)
                    {
                        sheet.Cells[i, 3].Style.Font.Bold = true;
                    }

                    if (d.valor_real_acumulado > -1)
                    {
                        sheet.Cells[i, 4].Value = d.valor_real_acumulado;
                        sheet.Cells[i, 4].Style.Numberformat.Format = "#0.000%";
                        if (d.esCabeceraDefinitiva)
                        {
                            sheet.Cells[i, 4].Style.Font.Bold = true;
                        }
                    }
                    else
                    {
                        sheet.Cells[i, 4].Style.Numberformat.Format = "0%";
                        sheet.Cells[i, 4].FormulaR1C1 = "";

                        if (d.esCabeceraDefinitiva)
                        {
                            sheet.Cells[i, 4].Style.Font.Bold = true;
                        }

                    }

                    i++;
                }


                //POSICION DE LA CURVA    
                ExcelChart ec = (ExcelLineChart)sheet.Drawings.AddChart("Curva Rdo", eChartType.Line);
                ec.SetPosition(8, 0, 8, 0);
                ec.SetSize(1500, 800);
                ec.DisplayBlanksAs = eDisplayBlanksAs.Gap;
                ec.PlotArea.Border.Fill.Color = Color.White;

                ec.ShowDataLabelsOverMaximum = true;
                ec.PlotArea.Fill.Style = eFillStyle.SolidFill;
                ec.Legend.Position = eLegendPosition.Bottom;
                ec.Legend.Font.Size = 18;
                ec.PlotArea.Border.LineStyle = OfficeOpenXml.Drawing.eLineStyle.Solid;

                ec.PlotArea.Fill.Color = Color.FromArgb(217, 217, 217);

                ec.YAxis.MaxValue = 1;
                ec.YAxis.Format = "#0%";
                ec.YAxis.Font.Size = 18;
                ec.YAxis.MajorUnit = 0.2;
                ec.YAxis.Fill.Color = Color.White;
                ec.XAxis.MajorUnit = 15;
                ec.XAxis.Font.Size = 18;
                ec.XAxis.Fill.Color = Color.White;

                ec.YAxis.MajorGridlines.LineStyle = OfficeOpenXml.Drawing.eLineStyle.Solid;
                ec.YAxis.MajorGridlines.Fill.Color = Color.White;
                ec.XAxis.MajorGridlines.LineStyle = OfficeOpenXml.Drawing.eLineStyle.Solid;
                ec.XAxis.MajorGridlines.Fill.Color = Color.White;
                ec.XAxis.TickLabelPosition = eTickLabelPosition.High;
                ec.Border.Fill.Color = Color.White;
                //CURVA HOJA 1
                //CURVA HOJA 1


                var celdacurva = "A1";
                int ultima = Convert.ToInt32("0" + hoja.Cells[celdacurva].Value);

                ExcelChart hoja1 = (ExcelLineChart)hoja.Drawings.AddChart("Curva Rdo", eChartType.Line);
                hoja1.SetPosition(ultima, 0, 2, 0);
                hoja1.SetSize(5600, 800);

                hoja1.PlotArea.Fill.Style = eFillStyle.SolidFill;

                hoja1.Legend.Position = eLegendPosition.Bottom;
                hoja1.Legend.Font.Size = 18;
                hoja1.PlotArea.Border.LineStyle = OfficeOpenXml.Drawing.eLineStyle.Solid;
                hoja1.PlotArea.Fill.Color = Color.FromArgb(217, 217, 217);
                hoja1.DisplayBlanksAs = eDisplayBlanksAs.Gap;


                hoja1.YAxis.MaxValue = 1;
                hoja1.YAxis.Format = "#0%";
                hoja1.YAxis.Fill.Color = Color.White;
                hoja1.YAxis.Font.Size = 18;
                hoja1.YAxis.MajorUnit = 0.2;
                hoja1.XAxis.MajorTimeUnit = eTimeUnit.Days;


                decimal rango = (m.fecha_fin_prevista - m.fecha_inicio_prevista).Days / objetoProyecto.periodo_curva;
                hoja1.XAxis.MajorUnit = rango > 0 ? Convert.ToInt32(Math.Floor(rango)) : objetoProyecto.periodo_curva;


                hoja1.XAxis.Font.Size = 18;
                hoja1.XAxis.Fill.Color = Color.White;

                hoja1.YAxis.MajorGridlines.LineStyle = OfficeOpenXml.Drawing.eLineStyle.Solid;
                hoja1.YAxis.MajorGridlines.Fill.Color = Color.White;
                hoja1.XAxis.MajorGridlines.LineStyle = OfficeOpenXml.Drawing.eLineStyle.Solid;
                hoja1.XAxis.MajorGridlines.Fill.Color = Color.White;

                ec.Title.Text = "Total Construcción";
                hoja1.Title.Text = "Total Construcción";

                int row = sheet.Dimension.End.Row;
                var rangeX = sheet.Cells["B9:B" + row]; // X-Axis
                var range1 = sheet.Cells["C9:C" + row]; // 1ra LineSerie

                ExcelLineChartSerie serie1 = (ExcelLineChartSerie)ec.Series.Add(range1, rangeX);
                serie1.LineColor = Color.FromArgb(0, 112, 192);
                serie1.LineWidth = 4;

                ExcelLineChartSerie seriehoja1 = (ExcelLineChartSerie)hoja1.Series.Add(range1, rangeX);
                seriehoja1.LineColor = Color.FromArgb(0, 112, 192);
                seriehoja1.LineWidth = 4;


                serie1.Header = sheet.Cells["C8"].Value.ToString();
                seriehoja1.Header = sheet.Cells["C8"].Value.ToString();


                var rangeX2 = sheet.Cells["B9:B" + row]; // X-Axis
                var range12 = sheet.Cells["D9:D" + row]; // 2da Seria

                ExcelLineChartSerie serie2 = (ExcelLineChartSerie)ec.Series.Add(range12, rangeX2);
                serie2.LineColor = Color.Red;
                serie2.LineWidth = 4;


                ExcelLineChartSerie seriehoja2 = (ExcelLineChartSerie)hoja1.Series.Add(range12, rangeX2);
                seriehoja2.LineColor = Color.Red;
                seriehoja2.LineWidth = 4;

                serie2.Header = sheet.Cells["D8"].Value.ToString();
                seriehoja2.Header = sheet.Cells["D8"].Value.ToString();



            }

            else
            {
                var fechac = new DateTime(2018, 01, 09);
                double x = 0.05;
                double y = 0.02;
                for (int i = 9; i <= 29; i++)
                {

                    fechac = fechac.AddDays(1);
                    sheet.Cells[i, 2].Value = fechac;
                    sheet.Cells[i, 2].Style.Numberformat.Format = "dd-mmm-yy";


                    sheet.Cells[i, 3].Value = x;
                    sheet.Cells[i, 3].Style.Numberformat.Format = "0.0%";

                    sheet.Cells[i, 4].Value = y;
                    sheet.Cells[i, 4].Style.Numberformat.Format = "0.0%";

                    x = x + 0.04;
                    y = y + 0.03;
                }


                //POSICION DE LA CURVA    
                ExcelChart ec = (ExcelLineChart)sheet.Drawings.AddChart("Curva Rdo", eChartType.Line);
                ec.SetPosition(8, 0, 8, 0);
                ec.SetSize(800, 300);

                ec.Title.Text = "Curva de Avance RDO";

                int row = sheet.Dimension.End.Row;
                var rangeX = sheet.Cells["B9:B" + row]; // X-Axis
                var range1 = sheet.Cells["C9:C" + row]; // 1ra LineSerie

                ExcelLineChartSerie serie1 = (ExcelLineChartSerie)ec.Series.Add(range1, rangeX);
                serie1.Header = sheet.Cells["C8"].Value.ToString();

                var rangeX2 = sheet.Cells["B9:B" + row]; // X-Axis
                var range12 = sheet.Cells["D9:D" + row]; // 2da Seria

                ExcelLineChartSerie serie2 = (ExcelLineChartSerie)ec.Series.Add(range12, rangeX2);

                serie2.Header = sheet.Cells["D8"].Value.ToString();

            }



            /**/

            if (second_format)
            {
                string cellEarnValue = "V" + 15 + ""; //CABECERA PRINCIPAL
                hoja.Cells[cellEarnValue].Value = hoja.Cells["K1"].Value;
                hoja.Cells[cellEarnValue].Style.Numberformat.Format = "#,##0.00";


                var rangoInicialdescuento = Int32.Parse(hoja.Cells["L1"].Value.ToString());
                var rangoFinaldescuento = Int32.Parse(hoja.Cells["M1"].Value.ToString());

                var indicecolor = rangoInicialdescuento;
                string rangocolor = "AS" + indicecolor + ":AT" + indicecolor;
                hoja.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hoja.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(244, 176, 132));
                indicecolor++;
                rangocolor = "AS" + indicecolor + ":AT" + indicecolor;
                hoja.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hoja.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(252, 228, 214));

                indicecolor++;
                rangocolor = "AS" + indicecolor + ":AT" + indicecolor;
                hoja.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hoja.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(112, 173, 71));
                indicecolor++;
                rangocolor = "AS" + indicecolor + ":AT" + indicecolor;
                hoja.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hoja.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(169, 208, 142));
                indicecolor++;
                rangocolor = "AS" + indicecolor + ":AT" + indicecolor;
                hoja.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hoja.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(91, 155, 213));
                indicecolor++;
                rangocolor = "AS" + indicecolor + ":AT" + indicecolor;
                hoja.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hoja.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(189, 215, 238));
                indicecolor++;
                rangocolor = "AS" + indicecolor + ":AT" + indicecolor;
                hoja.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hoja.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
                indicecolor++;
                rangocolor = "AS" + indicecolor + ":AT" + indicecolor;
                hoja.Cells[rangocolor].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                hoja.Cells[rangocolor].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 230, 153));

                rangocolor = "AS" + 8 + ":AT" + 8;
                hoja.Cells[rangocolor].Merge = true;
                hoja.Cells[rangocolor].Value = "COLUMNAS AUXILIARES";
                hoja.Cells[rangocolor].Style.Font.Color.SetColor(Color.White);

                rangocolor = "AS" + 9 + ":AT" + 9;
                hoja.Cells[rangocolor].Merge = true;
                hoja.Cells[rangocolor].Value = "Descuentos por item";
                hoja.Cells[rangocolor].Style.Font.Color.SetColor(Color.White);
                //Completar Rango Descuentos EARN VALUE
                for (int i = rangoInicialdescuento; i <= rangoFinaldescuento; i++)
                {
                    hoja.Cells[i, 22].Value = hoja.Cells[i, 25].Value;
                    hoja.Cells[i, 22].Style.Numberformat.Format = "#,##0.00";



                    hoja.Cells[i, 25].Value = "";

                }

                hoja.InsertRow(15, 1);
                hoja.InsertRow(17, 1);

                hojaAdicionales.InsertRow(15, 1);
                hojaAdicionales.InsertRow(17, 1);


            }
            //Column Group
            for (var i = 13; i <= 23; i++)
            {
                hoja.Column(i).OutlineLevel = 1;
                hoja.Column(i).Collapsed = true;
            }
            return excel;
        }


        public bool EliminarPeriodoRdoCabecera(int Id, DateTime fecha_registro)
        {
            var rdoinicio = Repository.Get(Id);
            var rdoPosteriores = Repository.GetAll()
                                           .Where(c => c.vigente)
                                           .Where(c => c.ProyectoId == rdoinicio.ProyectoId)
                                           .Where(c => c.fecha_rdo > rdoinicio.fecha_rdo).ToList();

            var rdoPosterioresGeneracion = Repository.GetAll().Where(c => c.vigente)
                                                              .Where(c => c.fecha_rdo > fecha_registro)
                                                              .Where(c => c.ProyectoId == rdoinicio.ProyectoId)
                                                              .ToList();

            if (rdoPosterioresGeneracion.Count > 0)
            {
                rdoPosteriores.AddRange(rdoPosterioresGeneracion);

                foreach (var rso in rdoPosteriores)
                {
                    var detalle = _detalleRepository.GetAll().Where(c => c.vigente).Where(c => c.RsoCabeceraId == rso.Id).ToList();
                    if (detalle.Count > 0)
                    {

                        _detalleRepository.Delete(detalle);


                    }
                    Repository.Delete(rso.Id);
                }
            }
            return true;




        }
        public int EliminarRdoCabecera(int RdoCabeceraId)
        {
            var detalle = _detalleRepository.GetAll().Where(c => c.vigente).Where(c => c.RsoCabeceraId == RdoCabeceraId).ToList();
            if (detalle.Count > 0)
            {

                _detalleRepository.Delete(detalle);


            }
            Repository.Delete(RdoCabeceraId);

            return 1;
        }
        public List<ProyectoObservacionDto> ListarPorProyectoTipo(int ProyectoId, TipoComentario Tipo)
        {
            var query = _proyectoObservacionRepository.GetAllIncluding(c => c.TipoObservacion).Where(o => o.vigente).Where(o => o.ProyectoId == ProyectoId)
                                                                 .Where(o => o.Tipo == Tipo)

                                                                 .ToList();
            var items = (from i in query
                         select new ProyectoObservacionDto()
                         {
                             Id = i.Id,
                             ProyectoId = i.ProyectoId,
                             NombreProyecto = i.Proyecto.nombre_proyecto,
                             FechaObservacion = i.FechaObservacion,
                             Observacion = i.Observacion,
                             FormatFecha = i.FechaObservacion.ToShortDateString(),
                             NombreTipoObservacion = i.TipoObservacion.nombre,
                             Codigo = i.TipoObservacion.codigo,
                             TipoObservacionId = i.TipoObservacionId,
                             vigente = i.vigente,
                             Tipo = i.Tipo,

                         }).ToList();

            return items;
        }



        public decimal ObtenerCantidadAcumuladaAnterior(int computoId, DateTime fecha_reporte, int proyectoId)
        {
            decimal cantidad_acumulada = 0;
            var query = _davanceRepository.GetAllIncluding(o => o.AvanceObra.Oferta)
                .Where(o => o.vigente == true)
                .Where(o => o.AvanceObra.Oferta.ProyectoId == proyectoId) // Agregado
                .Where(o => o.ComputoId == computoId)
                .Where(o => o.AvanceObra.aprobado == true)
                 .Where(o => o.AvanceObra.vigente == true)
                .Where(o => o.AvanceObra.fecha_presentacion < fecha_reporte);

            var detalles = (from d in query
                            select new DetalleAvanceObraDto()
                            {
                                cantidad_diaria = d.cantidad_diaria,
                                fecha_registro = d.fecha_registro,
                                total = d.total
                            }).ToList();

            foreach (var d in detalles)
            {
                cantidad_acumulada += d.cantidad_diaria;
            }

            return cantidad_acumulada;
        }

        public decimal ObtenerCantidadActual(int computoId, DateTime fecha_reporte, int proyectoId)
        {
            decimal cantidad_acumulada = 0;
            var query = _davanceRepository.GetAllIncluding(o => o.AvanceObra.Oferta)
                .Where(o => o.vigente == true)
                .Where(o => o.AvanceObra.Oferta.ProyectoId == proyectoId)
                .Where(o => o.ComputoId == computoId)
                .Where(o => o.AvanceObra.aprobado == true)
                .Where(o => o.AvanceObra.vigente == true)
                .Where(o => o.AvanceObra.fecha_presentacion == fecha_reporte);

            var detalles = (from d in query
                            select new DetalleAvanceObraDto()
                            {
                                cantidad_diaria = d.cantidad_diaria,
                                fecha_registro = d.fecha_registro,
                                total = d.total
                            }).ToList();

            foreach (var d in detalles)
            {
                cantidad_acumulada += d.cantidad_diaria;
            }

            return cantidad_acumulada;
        }

        public TotalesDescuentoRdo ObtenerTotalesEspecialidad(List<RdoDatos> ac, string col, decimal total)
        {
            var ObrasMecanicas = (from e in ac where e.codigo_especialidad == ProyectoCodigos.OBRAS_MECANICAS select e).ToList();
            var ObrasElectricas = (from e in ac where e.codigo_especialidad == ProyectoCodigos.OBRAS_ELECTRICAS select e).ToList();
            var Instrumentos_Control = (from e in ac where e.codigo_especialidad == ProyectoCodigos.OBRAS_INSTRUMENTOS_CONTROL select e).ToList();


            TotalesDescuentoRdo r = new TotalesDescuentoRdo();
            if (col == "COSTO_BUGET")
            {
                r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = (from e in ObrasMecanicas select e.costo_presupuesto).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = (from e in ObrasElectricas select e.costo_presupuesto).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = (from e in Instrumentos_Control select e.costo_presupuesto).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_CIVILES = total - r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                r.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES = 0;

                r.TOTAL_DESCUENTO_APLICADO = total - r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
            }
            if (col == "COSTO_EAC")
            {
                r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = (from e in ObrasMecanicas select e.costo_eac).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = (from e in ObrasElectricas select e.costo_eac).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = (from e in Instrumentos_Control select e.costo_eac).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_CIVILES = total - r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                r.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES = 0;

                r.TOTAL_DESCUENTO_APLICADO = total - r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
            }
            if (col == "AC_ANTERIOR")
            {
                r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = (from e in ObrasMecanicas select e.ac_anterior).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = (from e in ObrasElectricas select e.ac_anterior).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = (from e in Instrumentos_Control select e.ac_anterior).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_CIVILES = total - r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                r.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES = 0;

                r.TOTAL_DESCUENTO_APLICADO = total - r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
            }
            if (col == "AC_DIARIO")
            {
                r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = (from e in ObrasMecanicas select e.ac_diario).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = (from e in ObrasElectricas select e.ac_diario).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = (from e in Instrumentos_Control select e.ac_diario).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_CIVILES = total - r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                r.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES = 0;
                r.TOTAL_DESCUENTO_APLICADO = total - r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
            }
            if (col == "AC_ACTUAL")
            {
                r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = (from e in ObrasMecanicas select e.ac_actual).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = (from e in ObrasElectricas select e.ac_actual).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = (from e in Instrumentos_Control select e.ac_actual).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_CIVILES = total - r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                r.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES = 0;
                r.TOTAL_DESCUENTO_APLICADO = total - r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
            }
            if (col == "EV_ANTERIOR")
            {
                r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = (from e in ObrasMecanicas select e.ev_anterior).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = (from e in ObrasElectricas select e.ev_anterior).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = (from e in Instrumentos_Control select e.ev_anterior).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_CIVILES = total - r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                r.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES = 0;
                r.TOTAL_DESCUENTO_APLICADO = total - r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
            }
            if (col == "EV_DIARIO")
            {
                r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = (from e in ObrasMecanicas select e.ev_diario).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = (from e in ObrasElectricas select e.ev_diario).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = (from e in Instrumentos_Control select e.ev_diario).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_CIVILES = total - r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                r.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES = 0;
                r.TOTAL_DESCUENTO_APLICADO = total - r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
            }
            if (col == "EV_ACTUAL")
            {
                r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = (from e in ObrasMecanicas select e.ev_actual).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = (from e in ObrasElectricas select e.ev_actual).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = (from e in Instrumentos_Control select e.ev_actual).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_CIVILES = total - r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                r.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES = 0;
                r.TOTAL_DESCUENTO_APLICADO = total - r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
            }
            if (col == "EARN_VALUE")
            {
                r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = (from e in ObrasMecanicas select e.ern_value).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = (from e in ObrasElectricas select e.ern_value).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = (from e in Instrumentos_Control select e.ern_value).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_CIVILES = total - r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                r.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES = 0;
                r.TOTAL_DESCUENTO_APLICADO = total - r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;

            }
            if (col == "PV_COSTO_PLANIFICADO")
            {
                r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = (from e in ObrasMecanicas select e.pv_costo_planificado).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = (from e in ObrasElectricas select e.pv_costo_planificado).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = (from e in Instrumentos_Control select e.pv_costo_planificado).ToList().Sum();
                r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * Convert.ToDecimal(0.01);

                r.VALOR_COSTO_DIRECTO_OBRAS_CIVILES = total - r.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
                r.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES = 0;
                r.TOTAL_DESCUENTO_APLICADO = total - r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS - r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
            }
            return r;

        }



        public decimal ObtenerDescuentoEspecialidadAPU(TotalesDescuentoRdo r, RdoDatos ac)
        {
            string e = ac.codigo_especialidad;

            if (e == "EOBRACIVIL")
            {
                return r.D_VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
            }
            if (e == "EOBRAELECTRICA")
            {
                return r.D_VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS;
            }
            if (e == "EOBRAMECANICA")
            {
                return r.D_VALOR_COSTO_DIRECTO_OBRAS_MECANICAS;
            }
            if (e == "EOBRAINSCONTROL")
            {
                return r.D_VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
            }
            if (e == "ESERVESPECIALES")
            {
                return 0;
            }
            if (e == "EPROCURA")
            {
                return 0;
            }
            if (e == "ESUBCONTRATOS")
            {
                return 0;
            }

            return 0;

        }


        public int nivel_mas_alto(int Id)
        {
            var lista = _wbsRepository.GetAllIncluding(x => x.Catalogo)
                .Where(o => o.vigente == true)
                .Where(o => o.OfertaId == Id)
                .Where(o => o.es_actividad == true)
                .ToList();
            int mayor = 0;


            foreach (var item in lista)
            {
                int contnivel = this.contarnivel(item.Id, item.OfertaId);
                if (contnivel >= mayor)
                {
                    mayor = contnivel;
                }


            }
            return mayor;
        }
        public int contarnivel(int id, int OfertaId)
        {
            List<Wbs> Jerarquia = new List<Wbs>();

            Wbs item = _wbsRepository.Get(id);
            Jerarquia.Add(item);
            while (item.id_nivel_padre_codigo != ".")
            {

                item = _wbsRepository.GetAll()
                    .Where(c => c.id_nivel_codigo == item.id_nivel_padre_codigo)
                    .Where(C => C.vigente)
                    .Where(c => c.OfertaId == OfertaId).FirstOrDefault();

                Jerarquia.Add(item);
            }
            return Jerarquia.Count();
        }

        #region METODOS PARA LA GENERACION DEL RDO CON PENDIENTES DE APROBACION CON PRESUPUESTO INDEPENDIENTE
        public List<Contrato> GetContratos()
        {
            return _contratoRepository.GetAll().Where(c => c.vigente).ToList();
        }

        public List<Proyecto> GetProyectos(int ContratoId)
        {
            return _proyectoRepository.GetAll().Where(c => c.vigente).Where(c => c.contratoId == ContratoId).ToList();
        }

        public ExcelPackage GenerarExcelCurva(ModelHistoricoCurva model)
        {

            var proyecto = _proyectoRepository.GetAllIncluding(c => c.Contrato).Where(c => c.Id == model.ProyectoId).FirstOrDefault();
            var list = _curvaRepository.GetAll().Where(c => c.ProyectoId == proyecto.Id).Where(c => c.dato_migrado).ToList();
            ExcelPackage package = new ExcelPackage();
            var workbook = package.Workbook;
            var h = workbook.Worksheets.Add("FormatoCurva");

            string cell = "";
            int count = 1;
            cell = "A" + count;
            h.Cells[cell].Value = proyecto.Id;
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            cell = "B" + count + ":D" + count;
            h.Cells[cell].Merge = true;
            h.Cells[cell].Value = proyecto.codigo;
            h.Cells[cell].Style.Font.Bold = true;
            h.Cells[cell].Style.WrapText = true;
            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            h.Cells[2, 1, 3, 4].Merge = true;
            h.Cells[2, 1, 3, 4].Style.Font.Italic = true;
            h.Cells[2, 1, 3, 4].Style.WrapText = true;
            h.Cells[2, 1, 3, 4].Value = proyecto.nombre_proyecto;
            h.Cells[2, 1, 3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            h.Cells[2, 1, 3, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            count = 4;
            h.Row(count).Height = 20;
            h.Column(1).Width = 10;
            h.Column(1).Hidden = true;
            h.Column(2).Width = 15;
            h.Column(3).Width = 20;
            h.Column(4).Width = 20;

            cell = "A" + count;
            h.Cells[cell].Value = "Id";

            cell = "B" + count;
            h.Cells[cell].Value = "Fecha";
            cell = "C" + count;
            h.Cells[cell].Value = "% Avance Previsto";
            h.Cells[cell].Style.WrapText = true;
            cell = "D" + count;
            h.Cells[cell].Value = "% Avance Real";
            h.Cells[cell].Style.WrapText = true;
            cell = "A" + count + ":D" + count;

            h.Cells[cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            h.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(48, 84, 150));
            h.Cells[cell].Style.Font.Color.SetColor(Color.White);
            h.Cells[cell].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            h.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            count++;

            var FechaFin = model.FechaFin;
            var FechaInicio = model.FechaInicio;
            for (DateTime fecha = FechaInicio; fecha <= FechaFin; fecha = fecha.AddDays(1))
            {
                cell = "B" + count;
                h.Cells[cell].Value = fecha;
                h.Cells[cell].Style.Numberformat.Format = "DD/MM/YYYY";

                var existe = (from l in list where l.fecha.ToShortDateString() == fecha.ToShortDateString() where l.dato_migrado select l).FirstOrDefault();
                if (existe != null && existe.Id > 0)
                {

                    cell = "A" + count;
                    h.Cells[cell].Value = existe.Id;
                    cell = "C" + count;
                    h.Cells[cell].Value = existe.valor_previsto_acumulado;
                    h.Cells[cell].Style.Numberformat.Format = "0.000000";
                    h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Blue);
                    cell = "D" + count;
                    h.Cells[cell].Value = existe.valor_real_acumulado;
                    h.Cells[cell].Style.Numberformat.Format = "0.000000";
                    h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Blue);
                }
                else
                {


                    cell = "C" + count;
                    h.Cells[cell].Style.Numberformat.Format = "0.000000";
                    h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Dotted, System.Drawing.Color.Red);
                    cell = "D" + count;
                    h.Cells[cell].Style.Numberformat.Format = "0.000000";
                    h.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Dotted, System.Drawing.Color.Red);

                }
                count++;
            }

            h.View.PageBreakView = true;
            /* workSheet.PrinterSettings.PrintArea = workSheet.Cells[2, 3, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column];

             workSheet.PrinterSettings.Orientation = eOrientation.Landscape;
             */
            h.PrinterSettings.FitToPage = true;



            return package;
        }

        public DateTime FechaMinimaAvanceObra(int ProyectoId)
        {
            var avances = _avanceRepository.GetAllIncluding(c => c.Oferta)
                .Where(c => c.fecha_presentacion.HasValue)
                .Where(c => c.aprobado)
                .Where(c => c.vigente)
                .Where(c => c.Oferta.ProyectoId == ProyectoId)
                .ToList();
            var fechaMinima = (from f in avances select f.fecha_presentacion).Min();
            return fechaMinima.HasValue ? fechaMinima.Value : new DateTime(1990, 01, 01);
        }

        public string ActualizarFechasHistoricos(HttpPostedFileBase UploadedFile)
        {
            string resultado = "";
            if (UploadedFile != null)
            {

                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string fileName = UploadedFile.FileName;
                    string fileContentType = UploadedFile.ContentType;
                    byte[] fileBytes = new byte[UploadedFile.ContentLength];
                    var data = UploadedFile.InputStream.Read(fileBytes, 0,
                        Convert.ToInt32(UploadedFile.ContentLength));

                    using (var package = new ExcelPackage(UploadedFile.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;

                        var noOfRow = workSheet.Dimension.End.Row;
                        int proyectoId = Int32.Parse("0" + (workSheet.Cells[1, 1].Text ?? "0").ToString());

                        if (proyectoId > 0)
                        {
                            var list = _curvaRepository.GetAll().Where(c => c.ProyectoId == proyectoId).Where(c => c.dato_migrado).ToList();
                            for (int rowIterator = 5; rowIterator <= noOfRow; rowIterator++)
                            {

                                var fecha = (workSheet.Cells[rowIterator, 2].Text ?? "01/01/1999").ToString();
                                var campo_porcentajePrevisto = (workSheet.Cells[rowIterator, 3].Value ?? "").ToString();
                                var campo_porcentajeReal = (workSheet.Cells[rowIterator, 4].Value ?? "").ToString();
                                DateTime tempFecha = DateTime.Parse(DateTime.Parse(fecha).ToShortDateString());

                                var cantidadPrevisto = Decimal.Parse("0" + campo_porcentajePrevisto, NumberStyles.Float); //Para que acepte 10 exp 

                                var cantidadReal = Decimal.Parse("0" + campo_porcentajeReal, NumberStyles.Float); //Para que acepte 10 exp 

                                var existe = (from l in list where l.fecha.ToShortDateString() == tempFecha.ToShortDateString() where l.dato_migrado select l).FirstOrDefault();
                                if (existe != null && existe.Id > 0)
                                {
                                    var curvafecha = _curvaRepository.GetAll().Where(c => c.Id == existe.Id).FirstOrDefault();
                                    if (curvafecha != null && curvafecha.Id > 0)
                                    {
                                        if (cantidadReal == 0)
                                        {
                                            curvafecha.valor_previsto_acumulado = cantidadPrevisto;
                                            curvafecha.valor_real_acumulado = cantidadReal;

                                            _curvaRepository.Delete(curvafecha);
                                        }
                                        else
                                        {

                                            curvafecha.valor_previsto_acumulado = cantidadPrevisto;
                                            curvafecha.valor_real_acumulado = cantidadReal;
                                            _curvaRepository.Update(curvafecha);
                                        }

                                    }

                                }
                                else
                                {
                                    if (cantidadReal > 0)
                                    {
                                        var nuevo = new CurvasProyecto()
                                        {
                                            ProyectoId = proyectoId,
                                            dato_migrado = true,
                                            fecha = tempFecha,
                                            Id = 0,
                                            valor_previsto = 0,
                                            valor_previsto_acumulado = cantidadPrevisto,
                                            valor_real = 0,
                                            valor_real_acumulado = cantidadReal
                                        };
                                        _curvaRepository.Insert(nuevo);
                                    }

                                }


                            }
                            return "OK";
                        }
                        else
                        {
                            return "No se encuentra la referencia del Proyecto en el Excel, descargar el Formato Nuevamente";
                        }
                    }
                }
                else
                {
                    resultado = "";
                    resultado = "NO_EXCEL";
                }
            }
            else
            {
                resultado = "";
                resultado = "SIN_ARCHIVO";
            }
            return resultado;
        }

        public string CreateRsoLast(int ProyectoId, DateTime fecha, int Id)
        {

            var rdo = _rdocabeceraRepository.GetAll()
                                            .Where(x => x.es_definitivo)
                                            .Where(x => x.vigente)
                                            .Where(x => x.ProyectoId == ProyectoId)
                                            .Where(x => x.fecha_rdo == fecha)
                                            .OrderByDescending(x => x.fecha_rdo)
                                            .FirstOrDefault();
            if (rdo == null)
            {
                return "NO_RDO_INICIO";
            }
            else
            {

                var detallesEACRdo = _eacRDORepository.GetAll()
                                                             .Where(x => x.vigente)
                                                             .Where(x => x.RdoCabeceraId == rdo.Id)
                                                             .ToList();


                var anteriorRso = Repository.GetAll()
                                            .Where(x => x.es_definitivo)
                                            .Where(x => x.vigente)
                                            .Where(x => x.ProyectoId == ProyectoId)
                                            .Where(x => x.fecha_rdo < fecha)
                                            .OrderByDescending(x => x.fecha_rdo)
                                            .FirstOrDefault();

                var rsoEntity = new RsoCabecera()
                {
                    avance_real_acumulado = rdo.avance_real_acumulado,
                    fecha_envio = rdo.fecha_envio,
                    fecha_rdo = fecha,
                    estado = true,
                    codigo_rdo = rdo.codigo_rdo,
                    emitido = rdo.emitido,
                    es_definitivo = rdo.es_definitivo,
                    observacion = rdo.observacion,
                    ProyectoId = ProyectoId,
                    version = "A",
                    vigente = true
                };


                int RsoCabeceraId = Repository.InsertAndGetId(rsoEntity);
                ElmahExtension.LogToElmah(new Exception("Count Detalles RSO: " + detallesEACRdo.Count));
                var rsoDetallesAnteriorRSO = new List<RsoDetalleEac>();
                if (anteriorRso != null)
                {
                    rsoEntity.fecha_inicio = anteriorRso.fecha_rdo.AddDays(1);
                    var rsoDetallesAnteriores = _detalleRepository.GetAll()
                                                                .Where(x => x.RsoCabeceraId == anteriorRso.Id)
                                                                .Where(x => x.vigente)
                                                                .ToList();
                    if (rsoDetallesAnteriores.Count > 0)
                    {
                        rsoDetallesAnteriorRSO.AddRange(rsoDetallesAnteriores);
                    }
                }

                foreach (var d in detallesEACRdo)
                {


                    var rsoDetalleEntity = new RsoDetalleEac()
                    {
                        ac_actual = d.ac_actual,
                        ac_anterior = d.ac_anterior,
                        ac_diario = d.ac_diario,
                        cantidad_acumulada = d.cantidad_acumulada,
                        cantidad_anterior = d.cantidad_anterior,
                        cantidad_diaria = d.cantidad_diaria,
                        cantidad_eac = d.cantidad_eac,
                        cantidad_planificada = d.cantidad_planificada,
                        codigo_especialidad = d.codigo_especialidad,
                        codigo_grupo = d.codigo_grupo,
                        codigo_preciario = d.codigo_preciario,
                        ComputoId = d.ComputoId,
                        costo_budget_version_anterior = d.costo_budget_version_anterior,
                        costo_eac = d.costo_eac,
                        costo_presupuesto = d.costo_presupuesto,
                        ern_value = d.ern_value,
                        es_temporal = d.es_temporal,
                        ev_actual = d.ev_actual,
                        ev_diario = d.ev_diario,
                        ev_actual_version_anterior = d.ev_actual_version_anterior,
                        ev_anterior = d.ev_anterior,
                        fecha_fin_prevista = d.fecha_fin_prevista,
                        fecha_fin_real = d.fecha_fin_real,
                        fecha_inicio_prevista = d.fecha_inicio_prevista,
                        fecha_inicio_real = d.fecha_inicio_real,
                        ganancia = d.ganancia,
                        id_rubro = d.id_rubro,
                        ItemId = d.ItemId,
                        nombre_actividad = d.nombre_actividad,
                        PendienteAprobacion = d.PendienteAprobacion,
                        porcentaje_avance_actual_acumulado = d.porcentaje_avance_actual_acumulado,
                        porcentaje_avance_anterior = d.porcentaje_avance_anterior,
                        porcentaje_avance_diario = d.porcentaje_avance_diario,
                        porcentaje_avance_previsto_acumulado = d.porcentaje_avance_previsto_acumulado,
                        porcentaje_costo_eac_total = d.porcentaje_costo_eac_total,
                        porcentaje_presupuesto_total = d.porcentaje_presupuesto_total,
                        precio_unitario = d.precio_unitario,
                        presupuesto_total = d.presupuesto_total,
                        pv_costo_planificado = d.pv_costo_planificado,
                        RsoCabeceraId = RsoCabeceraId,
                        UM = d.UM,
                        vigente = d.vigente,
                        WbsId = d.WbsId
                    };
                    if (anteriorRso != null)
                    {
                        //Cantidades AC
                        var acAnteriorComputo = (from r in rsoDetallesAnteriorRSO
                                                 where r.vigente
                                                 where r.ComputoId == rsoDetalleEntity.ComputoId
                                                 select r.ac_actual
                                               ).Sum();

                        rsoDetalleEntity.ac_anterior = acAnteriorComputo;
                        rsoDetalleEntity.ac_diario = rsoDetalleEntity.ac_actual- acAnteriorComputo;


                        //Cantidad Acumulada

                    var cantidadAcumuladaComputo= (from r in rsoDetallesAnteriorRSO
                                                   where r.vigente
                                                   where r.ComputoId == rsoDetalleEntity.ComputoId
                                                   select r.cantidad_acumulada
                                               ).Sum();

                        rsoDetalleEntity.cantidad_anterior = cantidadAcumuladaComputo;
                        rsoDetalleEntity.cantidad_diaria = rsoDetalleEntity.cantidad_acumulada - cantidadAcumuladaComputo;

                        // EV ACTUAL

                        var evAnteriorComputo = (from r in rsoDetallesAnteriorRSO
                                                        where r.vigente
                                                        where r.ComputoId == rsoDetalleEntity.ComputoId
                                                        select r.ev_actual
                                                   ).Sum();

                        rsoDetalleEntity.ev_anterior = evAnteriorComputo;
                        rsoDetalleEntity.ev_diario = rsoDetalleEntity.ev_actual - evAnteriorComputo;


                        //PorcentajeAvanceAnterior=
                        var porcentajeAvanceAnteriorComputo = (from r in rsoDetallesAnteriorRSO
                                                 where r.vigente
                                                 where r.ComputoId == rsoDetalleEntity.ComputoId
                                                 select r.porcentaje_avance_actual_acumulado
                                                  ).Sum();

                        rsoDetalleEntity.porcentaje_avance_anterior = porcentajeAvanceAnteriorComputo;
                        rsoDetalleEntity.porcentaje_avance_diario = rsoDetalleEntity.porcentaje_avance_actual_acumulado - porcentajeAvanceAnteriorComputo;




                    }

                    _eacRepository.Insert(rsoDetalleEntity);
                }



            }


            return "OK";




        }

        #endregion
    }
}

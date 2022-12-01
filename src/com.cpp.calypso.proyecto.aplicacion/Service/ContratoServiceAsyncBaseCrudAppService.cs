using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;

using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;


namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ContratoServiceAsyncBaseCrudAppService : AsyncBaseCrudAppService<Contrato, ContratoDto, PagedAndFilteredResultRequestDto>, IContratoAsyncBaseCrudAppService
    {

        private readonly IBaseRepository<ContratoDocumentoBancario> _repositoryContratoDocumentoBancario;
        private readonly IBaseRepository<CentrodecostosContrato> _repositoryCentroCostosContrato;
        private readonly IBaseRepository<Adenda> _repositoryAdenda;
        private readonly IBaseRepository<Proyecto> _repositoryProyecto;
        private readonly IBaseRepository<Requerimiento> _requerimientoRepository;
        private readonly IBaseRepository<Empresa> _empresaRepository;
        private readonly IBaseRepository<Cliente> _clienteRepository;
        private IOfertaComercialAsyncBaseCrudAppService _ofertaService { get; }
        public ContratoServiceAsyncBaseCrudAppService(IBaseRepository<Contrato> repository,

            IBaseRepository<ContratoDocumentoBancario> repositoryContratoDocumentoBancario,
            IBaseRepository<CentrodecostosContrato> repositoryCentroCostosContrato,
            IBaseRepository<Adenda> repositoryAdenda,
            IBaseRepository<Proyecto> repositoryProyecto,
            IBaseRepository<Empresa> empresaRepository,
            IBaseRepository<Cliente> clienteRepository,
            IBaseRepository<Requerimiento> requerimientoRepository,
            IOfertaComercialAsyncBaseCrudAppService ofertaService
            ) : base(repository)


        {
            _repositoryContratoDocumentoBancario = repositoryContratoDocumentoBancario;
            _repositoryCentroCostosContrato = repositoryCentroCostosContrato;
            _repositoryAdenda = repositoryAdenda;
            _repositoryProyecto = repositoryProyecto;
            _empresaRepository = empresaRepository;
            _clienteRepository = clienteRepository;
            _requerimientoRepository = requerimientoRepository;
            _ofertaService = ofertaService;

        }

        public Task CancelarVigencia(int contratoId)
        {
            throw new NotImplementedException();
        }

        public List<AdendaDto> GetAdendas(int contratoId)
        {
            var adendasQuery = _repositoryAdenda.GetAllIncluding(c => c.Contrato, c => c.ArchivosContrato, c => c.ArchivosContrato.Archivos);
            var adendas = (from c in adendasQuery
                           where c.ContratoId == contratoId
                           where c.vigente == true
                           select new AdendaDto
                           {
                               Id = c.Id,
                               codigo = c.codigo,
                               descripcion = c.descripcion,
                               vigente = c.vigente,
                               fecha = c.fecha,
                               ContratoId = c.ContratoId,
                               Contrato = c.Contrato,
                               ArchivosContratoId = c.ArchivosContratoId,
                               ArchivosContrato = c.ArchivosContrato,


                           }).ToList();

            return adendas;
        }

        public List<CentrocostosContratoDto> GetCentrocostosContratos(int contratoId)
        {
            var centrocostosQuery = _repositoryCentroCostosContrato.GetAllIncluding(c => c.Contrato);
            var centrocostos = (from c in centrocostosQuery
                                where c.ContratoId == contratoId && c.vigente == true
                                select new CentrocostosContratoDto
                                {
                                    Id = c.Id,
                                    observaciones = c.observaciones,
                                    estado = c.estado,
                                    vigente = c.vigente,
                                    ContratoId = c.ContratoId,
                                    Contrato = c.Contrato,
                                    id_centrocostos = c.id_centrocostos,


                                }).ToList();

            return centrocostos;
        }

        public List<ContratoDocumentoBancarioDto> GetContratoDocumentoBancarios(int contratoId)
        {
            var contratosdocumentosbancariosQuery = _repositoryContratoDocumentoBancario.GetAllIncluding(c => c.Contrato, c => c.ArchivosContrato);

            var contratosdocumentosbancarios = (from r in contratosdocumentosbancariosQuery
                                                where r.ContratoId == contratoId
                                                where r.vigente == true
                                                select new ContratoDocumentoBancarioDto
                                                {
                                                    Id = r.Id,
                                                    tipo_documento = r.tipo_documento,
                                                    InstitucionFinancieraId = r.InstitucionFinancieraId,
                                                    codigo = r.codigo,
                                                    fecha_emision = r.fecha_emision,
                                                    fecha_vencimiento = r.fecha_vencimiento,
                                                    estado = r.estado,
                                                    vigente = r.vigente,
                                                    InstitucionFinanciera = r.InstitucionFinanciera,
                                                    ContratoId = r.ContratoId,
                                                    Contrato = r.Contrato,
                                                    concepto = r.concepto,
                                                    fecha_notificacion = r.fecha_notificacion,
                                                    notificado_cliente = r.notificado_cliente,
                                                    ArchivosContratoId = r.ArchivosContratoId,
                                                    ArchivosContrato = r.ArchivosContrato,

                                                }).ToList();
            return contratosdocumentosbancarios;
        }

        public List<Contrato> GetContratos()
        {
            var contratoQuery = Repository.GetAllIncluding(c => c.Cliente, c => c.Empresa, c => c.ContratoDocumentoBancarios).Where(e => e.vigente == true).ToList();

            return contratoQuery;
        }

        public async Task<ContratoDto> GetDetalle(int contratoId)
        {
            var contratoQuery = Repository.GetAll();
            ContratoDto item = (from c in contratoQuery

                                where c.Id == contratoId
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
                                    vigente = c.vigente,
                                    dias_plazo_pago = c.dias_plazo_pago,
                                    Empresa = c.Empresa,
                                    Cliente = c.Cliente,
                                    CentrodecostosContratos = c.CentrodecostosContratos,
                                    ContratoDocumentoBancarios = c.ContratoDocumentoBancarios,
                                    codigo_generado = c.codigo_generado,
                                    sitio_referencia = c.sitio_referencia,
                                    Formato = c.Formato.Value
                                }).FirstOrDefault();

            return item;
        }

        public List<ContratoDto> GetContratosDto()
        {
            var contratoQuery = Repository.GetAll();
            List<ContratoDto> item = (from c in contratoQuery
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
                                          vigente = c.vigente,
                                          dias_plazo_pago = c.dias_plazo_pago,
                                          Empresa = c.Empresa,
                                          Cliente = c.Cliente,
                                          CentrodecostosContratos = c.CentrodecostosContratos,
                                          ContratoDocumentoBancarios = c.ContratoDocumentoBancarios,
                                          codigo_generado = c.codigo_generado,
                                          nombrecompleto = c.Codigo + " - " + c.objeto,
                                          sitio_referencia = c.sitio_referencia,
                                      }).ToList();
            return item;
        }

        public List<ProyectoDto> GetProyectos(int contratoId)
        {
            var proyectosquery = _repositoryProyecto.GetAllIncluding(c => c.Contrato);

            var proyectos = (from r in proyectosquery
                             where r.contratoId == contratoId
                             where r.vigente == true
                             select new ProyectoDto
                             {
                                 Id = r.Id,
                                 contratoId = r.contratoId,
                                 codigo = r.codigo,
                                 nombre_proyecto = r.nombre_proyecto,
                                 descripcion_proyecto = r.descripcion_proyecto,
                                 centroCostosId = r.centroCostosId,
                                 alcance_basico = r.alcance_basico,
                                 presupuesto = r.presupuesto,
                                 comentarios = r.comentarios,
                                 monto_ofertado = r.monto_ofertado,
                                 monto_aprobado_orden_trabajo = r.monto_ofertado,
                                 monto_certificado_orden_trabajo = r.monto_certificado_orden_trabajo,
                                 monto_facturado = r.monto_facturado,
                                 monto_cobrado = r.monto_cobrado,
                                 fecha_estimada_inicio = r.fecha_estimada_inicio,
                                 fecha_acta_entrega = r.fecha_acta_entrega,
                                 fecha_estimada_fin = r.fecha_estimada_fin,
                                 fecha_recepcion_provisoria = r.fecha_recepcion_provisoria,
                                 fecha_recepcion_definitiva = r.fecha_recepcion_definitiva,
                                 estado_proyecto = r.estado_proyecto,
                                 vigente = r.vigente,
                                 Contrato = r.Contrato,
                                 codigo_generado = r.codigo_generado,
                                 LocacionId = r.LocacionId,
                                 monto_aprobado_os = r.monto_aprobado_os,
                                 monto_aprobado_os_construccion = r.monto_aprobado_os_construccion,
                                 monto_aprobado_os_ingenieria = r.monto_aprobado_os_ingenieria,
                                 monto_aprobado_os_suministros = r.monto_aprobado_os_suministros,

                             }).ToList();
            return proyectos;
        }
        public bool ComprobarYBorrarContrato(int ContratoId)
        {
            bool res = true;
            var contratoquery = Repository.GetAll();
            var icontrato = (from c in contratoquery
                             where c.Id == ContratoId
                             select new ContratoDto
                             {
                                 Id = c.Id,

                                 EmpresaId = c.EmpresaId,

                                 ClienteId = c.ClienteId,

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
                                 vigente = c.vigente,
                                 sitio_referencia = c.sitio_referencia,
                             }).SingleOrDefault();


            var hijosActivos = false;

            var centrodecosotos =
            this.GetCentrocostosContratos(ContratoId).Count > 0 ? hijosActivos = true : hijosActivos = false;

            var documentos =
                this.GetContratoDocumentoBancarios(ContratoId).Count > 0 ? hijosActivos = true : hijosActivos = false;
            var adendas =
                this.GetAdendas(ContratoId).Count > 0 ? hijosActivos = true : hijosActivos = false;
            var proyecto =
               this.GetProyectos(ContratoId).Count > 0 ? hijosActivos = true : hijosActivos = false;

            if (hijosActivos)
            {
                return false;
            }
            else
            {
                icontrato.vigente = false;
                Repository.Update(Mapper.Map<Contrato>(icontrato));
                return true;
            }
        }

        public ProyectoDto CrearProyectoporContratoAsync(ContratoDto contrato, int idcontrato)
        {

            ProyectoDto proyecto = new ProyectoDto()
            {
                codigo = contrato.Codigo + "P" + (_repositoryProyecto.GetAll().Count() + 1),
                nombre_proyecto = "Nombre del Proyecto Pendiente",
                descripcion_proyecto = "Prendiente",
                centroCostosId = 0,
                alcance_basico = "Pendiente",
                presupuesto = 0,
                comentarios = "Pendiente",
                monto_ofertado = 0,
                monto_aprobado_orden_trabajo = 0,
                monto_certificado_orden_trabajo = 0,
                monto_facturado = 0,
                monto_cobrado = 0,
                fecha_estimada_inicio = new DateTime(1990, 1, 1),
                fecha_acta_entrega = new DateTime(1990, 1, 1),
                fecha_estimada_fin = new DateTime(1990, 1, 1),
                fecha_recepcion_provisoria = new DateTime(1990, 1, 1),
                fecha_recepcion_definitiva = new DateTime(1990, 1, 1),
                estado_proyecto = true,
                vigente = true,
                monto_aprobado_os = 0,
                monto_aprobado_os_construccion = 0,
                monto_aprobado_os_ingenieria = 0,
                monto_aprobado_os_suministros = 0,

            };
            proyecto.contratoId = idcontrato;
            var o = _repositoryProyecto.Insert(Mapper.Map<Proyecto>(proyecto));
            return Mapper.Map<ProyectoDto>(o);
        }

        public List<ContratoDto> GetContratosporEC(int EmpresaId, int ClienteId)
        {
            var query = Repository.GetAllIncluding(c => c.Empresa, c => c.Cliente);

            var contratos = (from c in query
                             where c.EmpresaId == EmpresaId
                             where c.ClienteId == ClienteId
                             where c.vigente == true
                             select new ContratoDto
                             {
                                 Id = c.Id,

                                 EmpresaId = c.EmpresaId,

                                 ClienteId = c.ClienteId,

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
                                 vigente = c.vigente,
                                 dias_plazo_pago = c.dias_plazo_pago,
                                 //Cliente = c.Cliente,
                                 //Empresa = c.Empresa,
                                 Codigo = c.Codigo,
                                 //ContratoDocumentoBancarios = c.ContratoDocumentoBancarios.Where(r => r.vigente == true).ToList(),
                                 codigo_generado = c.codigo_generado,
                                 sitio_referencia = c.sitio_referencia,
                             }).ToList();
            return contratos;
        }

        public async Task<bool> EliminarVigencia(int contratoId)
        {
            bool resul = false;
            var contrato = await GetDetalle(contratoId);
            if (contrato != null)
            {
                var dc = _repositoryContratoDocumentoBancario.GetAll().Where(c => c.ContratoId == contratoId).Where(c => c.vigente == true).ToList();
                var cc = _repositoryCentroCostosContrato.GetAll().Where(c => c.ContratoId == contratoId).Where(c => c.vigente == true).ToList();
                var a = _repositoryAdenda.GetAll().Where(c => c.ContratoId == contratoId).Where(c => c.vigente == true).ToList();
                var p = _repositoryProyecto.GetAll().Where(c => c.contratoId == contratoId).Where(c => c.vigente == true).ToList();
                if (dc.Count > 0 || cc.Count > 0 || a.Count > 0 || p.Count > 0)
                {
                    resul = true;
                }
                else
                {
                    contrato.vigente = false;
                    Repository.InsertOrUpdate(MapToEntity(contrato));
                }

            }

            return resul;
        }

        public EmpresasClientesContrato ListaEmpresaClienteporContrato(int ContratoId)
        {
            var contratos = Repository.GetAllIncluding(c => c.Empresa, c => c.Cliente)
                                        .Where(c => c.vigente)
                                        .Where(c => c.Id == ContratoId)
                                       .ToList();

            var empresa = (from e in contratos select e.Empresa).ToList();
            var cliente = (from c in contratos select c.Cliente).ToList();

            EmpresasClientesContrato lista = new EmpresasClientesContrato
            {
                Empresas = empresa,
                Clientes = cliente
            };

            return lista;

        }

        public ContratoDto InformacionContratoFromProyecto(int ProyectoId)
        {
            var data = _repositoryProyecto.GetAllIncluding(c => c.Contrato)
                                          .Where(c => c.Id == ProyectoId)
                                          .Where(c => c.vigente)
                                          .FirstOrDefault();
            if (data != null && data.Id > 0)
            {
                ContratoDto e = new ContratoDto();
                e.Id = data.contratoId;
                e.Formato = data.Contrato.Formato;
                e.vigente = data.Contrato.vigente;
                return e;
            }
            else
            {
                return new ContratoDto();
            }
        }

        public List<InfoContrato> InfoContrato(int ClienteId)
        {
            var query = Repository.GetAllList().Where(c => c.vigente).Where(c => c.ClienteId == ClienteId).ToList();

            var list = (from c in query
                        select new InfoContrato
                        {
                            Id = c.Id,
                            codigo = c.Codigo,
                            nombre = c.descripcion
                        }).ToList();

            return list;
        }

        public List<InfoCliente> InfoCliente()
        {
            var query = _clienteRepository.GetAll().Where(c => c.vigente).ToList();

            var list = (from c in query
                        select new InfoCliente
                        {
                            Id = c.Id,
                            nombre = c.razon_social
                        }).ToList();

            return list;

        }

        public string OpenOutlook()
        {
                        //Microsoft.Office.Interop.Outlook._NameSpace = null;

            Microsoft.Office.Interop.Outlook.MailItem mail = null;

            Microsoft.Office.Interop.Outlook.MAPIFolder outbox = null;

            try

            {

                Outlook.Application app = new Outlook.Application();

                Microsoft.Office.Interop.Outlook._NameSpace nSpace = app.GetNamespace("MAPI");

                nSpace.Logon(null, null, false, false);

                outbox = nSpace.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderDrafts);

                //mail.Recipients.Add(teste@teste.com);

                mail = (Microsoft.Office.Interop.Outlook.MailItem)app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);

                mail.To = "teste@teste.com";

                mail.Subject = "teste2";

                mail.Body = "Hello";

                mail.SaveSentMessageFolder = outbox;

                mail.Display(true);

                //memo.Send();

                return "OK";
            }

            catch (System.Exception ex)

            {

               return "EXC "+ ex.ToString();

                // Console.WriteLine(ex.ToString());

            }

        }

        public string OpenOutlookProcess()
        {

            try
            {
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                // Set HTMLBody. 
                //add the body of the email
                oMsg.HTMLBody = "Hello, Jawed your message body will go here!!";
                //Add an attachment.
                String sDisplayName = "MyAttachment";
                int iPosition = (int)oMsg.Body.Length + 1;
                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                //now attached the file
                //Outlook.Attachment oAttach = oMsg.Attachments.Add(@"C:\\fileName.jpg", iAttachType, iPosition, sDisplayName);
                //Subject line
                oMsg.Subject = "Your Subject will go here.";
                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                // Change the recipient in the next line if necessary.
                Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add("jawed.ace@gmail.com");
                oRecip.Resolve();
                // Send.
                oMsg.Send();
                // Clean up.
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;
            }//end of try block
            catch (Exception ex)

            {

                return "EXCEPCION "+ex.Message;
            }//end of catch
            return "OK";
        }

        public ExcelPackage StackedColumn(ReportDto r)
        {

            var data = _ofertaService.GetDatosAdicionales(r);
            int aprobados = (from ap in data where ap.estadoOfertaCodigo == "TAprobado" select ap).ToList().Count;
            int cancelados = (from ap in data where ap.estadoOfertaCodigo == "TCancelado" select ap).ToList().Count;
            int presentados = (from ap in data where ap.estadoOfertaCodigo == "TPresentado" select ap).ToList().Count;

            ExcelPackage package = new ExcelPackage();
            var workbook = package.Workbook;
            var worksheet = workbook.Worksheets.Add("Sheet1");


            //fill cell data with a loop, note that row and column indexes start at 1
            int pierow = 1;

            worksheet.Cells["A" + pierow].Value = "Aprobado";
            worksheet.Cells["A" + (pierow + 1)].Value = aprobados;
            worksheet.Cells["B" + pierow].Value = "Cancelado";
            worksheet.Cells["B" + (pierow + 1)].Value = cancelados;
            worksheet.Cells["C" + pierow].Value = "Presentado";
            worksheet.Cells["C" + (pierow + 1)].Value = presentados;

            pierow++;
            //create a new piechart of type Pie3D
            ExcelPieChart pieChart = worksheet.Drawings.AddChart("pieChart", eChartType.Pie3D) as ExcelPieChart;

            //set the title
            pieChart.Title.Text = "ADICIONALES 2019 - 2020";

            //select the ranges for the pie. First the values, then the header range
            pieChart.Series.Add(ExcelRange.GetAddress(2, 1, 2, 3), ExcelRange.GetAddress(1, 1, 1, 3));


            //show the percentages in the pie
            pieChart.DataLabel.ShowPercent = true;
            pieChart.DataLabel.ShowCategory = true;
            //size of the chart
            pieChart.SetSize(500, 400);

            //add the chart at cell C5
            pieChart.SetPosition(4, 0, 2, 0);

            pierow++;
            int header = pierow;
            worksheet.Cells["C" + header].Value = "Aprobado";
            worksheet.Cells["D" + header].Value = "Cancelado";
            worksheet.Cells["E" + header].Value = "Presentado";

            var proyectos = (from p in data orderby p.codigoProyecto select p.codigoProyecto).Distinct();

            pierow++;
            int row = pierow;
            int initial = pierow;
            string cell = "B" + row;

            foreach (var proyecto in proyectos)
            {
                worksheet.Cells["B" + row].Value = proyecto;
                aprobados = (from ap in data
                             where ap.estadoOfertaCodigo == "TAprobado"
                             where ap.codigoProyecto == proyecto
                             select ap).ToList().Count;
                cancelados = (from ap in data
                              where ap.estadoOfertaCodigo == "TCancelado"
                              where ap.codigoProyecto == proyecto
                              select ap).ToList().Count;
                presentados = (from ap in data
                               where ap.estadoOfertaCodigo == "TPresentado"
                               where ap.codigoProyecto == proyecto
                               select ap).ToList().Count;


                worksheet.Cells["C" + row].Value = aprobados;
                worksheet.Cells["D" + row].Value = cancelados;
                worksheet.Cells["E" + row].Value = presentados;
                row++;
            }
            var chart = worksheet.Drawings.AddChart("chart", eChartType.ColumnStacked);
            chart.Legend.Position = eLegendPosition.Bottom;

            var series = chart.Series.Add("C" + initial + ": C" + row, "B" + initial + ": B" + row);
            series.HeaderAddress = new ExcelAddress("'Sheet1'!C" + header);

            var series2 = chart.Series.Add("D" + initial + ":D" + row, "B" + initial + ":B" + row);
            series2.HeaderAddress = new ExcelAddress("'Sheet1'!D" + header);


            var series3 = chart.Series.Add("E" + initial + ":E" + row, "B" + initial + ":B" + row);
            series3.HeaderAddress = new ExcelAddress("'Sheet1'!E" + header);


            return package;


        }

        public void ThisAddIn_Startup()
        {

        }
    }
    }


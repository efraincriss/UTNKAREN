using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class CertificadoIngenieriaAsyncBaseCrudAppService : AsyncBaseCrudAppService<CertificadoIngenieria, CertificadoIngenieriaDto, PagedAndFilteredResultRequestDto>, ICertificadoIngenieriaAsyncBaseCrudAppService
    {
        IBaseRepository<Proyecto> _proyecto;
        IBaseRepository<Secuencial> _secuencial;
        IBaseRepository<CertificadoIngenieriaDetalle> _detalle;
        IBaseRepository<DetalleItemIngenieria> _detalleItem;
        IBaseRepository<DetalleAvanceIngenieria> _detalleAvanceI;
        IBaseRepository<Oferta> _ofertaRepository;
        IBaseRepository<Computo> _computoRepository;
        public CertificadoIngenieriaAsyncBaseCrudAppService(
        IBaseRepository<CertificadoIngenieria> repository,
        IBaseRepository<CertificadoIngenieriaDetalle> detalle,
          IBaseRepository<DetalleItemIngenieria> detalleItem,
        IBaseRepository<Oferta> ofertaRepository,
        IBaseRepository<Computo> computoRepository,
        IBaseRepository<Proyecto> proyecto,
          IBaseRepository<Secuencial> secuencial,

        IBaseRepository<DetalleAvanceIngenieria> detalleAvanceI
      ) : base(repository)
        {
            _proyecto = proyecto;
            _detalle = detalle;
            _ofertaRepository = ofertaRepository;
            _computoRepository = computoRepository;
            _detalleItem = detalleItem;
            _detalleAvanceI = detalleAvanceI;
            _secuencial = secuencial;
        }
        public int DeleteCertificado(int id)
        {
            var e = Repository.Get(id);
            var detalle = _detalle.GetAll().Where(c => c.CertificadoIngenieriaId == e.Id).ToList();
            if (detalle.Count > 0)
            {
                foreach (var di in detalle)
                {
                    var it = _detalleItem.GetAllIncluding(c => c.DetalleAvanceIngenieria).Where(c => c.Id == di.DetalleItemIngenieriaId).FirstOrDefault();
                    var da = _detalleAvanceI.Get(it.DetalleAvanceIngenieriaId);
                    da.estacertificado = false;
                    _detalleAvanceI.Update(da);

                }


                _detalle.Delete(detalle);
            }
            Repository.Delete(e);
            return 1;
        }

        public int GenerarCertificado(int Id, DateTime fechaCorte, DateTime fechaEmision)
        {
            /*Datos Cabecera*/

            var oferta = _ofertaRepository.GetAllIncluding(c => c.Requerimiento, c => c.Proyecto).Where(c => c.Id == Id).FirstOrDefault();
            var proyecto = _proyecto.GetAll().Where(c => c.Id == oferta.ProyectoId).FirstOrDefault();
            var computos = _computoRepository.GetAllIncluding(c => c.Item.Grupo, c => c.Wbs.Oferta).Where(c => c.vigente)
                                                                      .Where(c => c.Item.Grupo.codigo == ProyectoCodigos.CODE_INGENIERIA)
                                                                      .Where(c => c.Wbs.OfertaId == Id)
                                                                      .Where(c => c.Wbs.Oferta.es_final)
                                                                      .ToList();

            var antiguo = Repository.GetAll().Where(c => c.ProyectoId == proyecto.Id).Where(c => c.fechaCorte == fechaCorte)
                                                                                .Where(c => c.fechaEmision == fechaEmision)
                                                                                .Where(c => c.esDefinitivo).FirstOrDefault();

            string version = "A";
            if (antiguo != null && antiguo.Id > 0)
            {
                var letra = antiguo.version.ToCharArray()[0];
                letra++;
                version = letra + "";
                antiguo.esDefinitivo = false;
                Repository.Update(antiguo);
            }
            int valor = 1;
            var secuencial = _secuencial.GetAll().Where(c => c.ProyectoId == proyecto.Id).FirstOrDefault();
            if (secuencial != null && secuencial.Id > 0)
            {
                valor = secuencial.secuencia;
            }

            string numerocertificado = proyecto.codigo + " - " + String.Format("{0:000000}", valor);

            var certificado = new CertificadoIngenieria()
            {
                Id = 0,
                ProyectoId = proyecto.Id,
                esDefinitivo = true,
                fechaCorte = fechaCorte,
                fechaEmision = fechaEmision,
                fase = "",
                fechaEnvio = null,
                numero_certificado = numerocertificado,
                ordenServicio = "",
                porcentajeAvance = 0,
                version = version,
                vigente = true,
                estado = EstadoCertificadoIngenieria.Registrado

            };
            int CertificadoId = Repository.InsertAndGetId(certificado);
            if (CertificadoId > 0)
            {

                var datos = this.Datos(oferta.Id);


                foreach (var d in datos)
                {
                    CertificadoIngenieriaDetalle e = new CertificadoIngenieriaDetalle()
                    {
                        Id = 0,
                        CertificadoIngenieriaId = CertificadoId,
                        DetalleItemIngenieriaId = d.Id,
                        ColaboradorId = d.ColaboradorId,
                        Categoria = d.Categoria,
                        CostoTotal = d.CostoTotal,
                        Nombre = d.Nombre,
                        Rubro = d.Rubro,
                        Tarifa = d.Tarifa,
                        TipoColaborador = d.TipoColaborador,
                        TotalHoras = d.TotalHoras,
                        Unidad = d.Unidad,
                        Etapa=d.Etapa

                    };
                    _detalle.Insert(e);

                    /*var ditems = _detalleItem.GetAllIncluding(c => c.DetalleAvanceIngenieria).Where(c => c.Id == d.Id).FirstOrDefault();
                    var davance = _detalleAvanceI.Get(ditems.DetalleAvanceIngenieriaId);
                    davance.estacertificado = true;
                    _detalleAvanceI.Update(davance);*/



                }
                /*  var uproyecto = _proyecto.GetAll().Where(c => c.Id == proyecto.Id).FirstOrDefault();
                  uproyecto.secuencial_certificado = uproyecto.secuencial_certificado + 1;
                  _proyecto.Update(uproyecto);*/

            }


            return proyecto.Id;

        }

        public List<IngenieriaDatos> Datos(int id) //OfertaId
        {
            List<IngenieriaDatos> datos = new List<IngenieriaDatos>();

            var query = _detalleItem.GetAllIncluding(c => c.DetalleAvanceIngenieria.AvanceIngenieria,
                                                               c => c.Colaborador.Cargo,
                                                               c => c.DetalleAvanceIngenieria.Computo.Item)
                                            .Where(c => c.DetalleAvanceIngenieria.AvanceIngenieria.OfertaId == id)
                                            .Where(c => c.vigente)
                                            .Where(c => c.DetalleAvanceIngenieria.vigente)
                                            .Where(c => !c.DetalleAvanceIngenieria.estacertificado)
                                            .Where(c => c.DetalleAvanceIngenieria.AvanceIngenieria.vigente)
                                            .ToList();
            var list = (from q in query
                        select new IngenieriaDatos()
                        {
                            Id = q.Id,
                            Rubro = q.DetalleAvanceIngenieria.Computo.Item.codigo,
                            ColaboradorId = q.ColaboradorId,
                            Nombre = q.Colaborador.apellidos + " " + q.Colaborador.nombres,
                            Categoria = q.DetalleAvanceIngenieria.Computo.Item.nombre,
                            Unidad = "HH",
                            TotalHoras = q.cantidad_horas,
                            Tarifa = q.Colaborador.Cargo.precio_unitario,
                            CostoTotal = (q.cantidad_horas * q.Colaborador.Cargo.precio_unitario),
                            TipoColaborador = q.Colaborador.tipo,
                            Etapa= Enum.GetName(typeof(DetalleItemIngenieria.Etapa), q.etapa)
                        }).ToList();
            datos.AddRange(list);

            /**/

            return datos;

        }


        public CertificadoIngenieriaDto GetDetalle(int Id)
        {
            var i = Repository.GetAllIncluding(c => c.Proyecto).Where(c => c.Id == Id)
                                                           .FirstOrDefault();
            var ci = new CertificadoIngenieriaDto()
            {
                Id = i.Id,
                ProyectoId = i.ProyectoId,
                nombreProyecto = i.Proyecto.nombre_proyecto,
                fechaCorte = i.fechaCorte,
                fechaEmision = i.fechaEmision,
                fechaEnvio = i.fechaEnvio,
                formatFechaCorte = i.fechaCorte.ToShortDateString(),
                formatFechaEmision = i.fechaEmision.ToShortDateString(),
                formatFechaEnvio = i.fechaEnvio.HasValue ? i.fechaEnvio.Value.ToShortDateString() : "",
                esDefinitivo = i.esDefinitivo,
                version = i.version,
                formatDefinitivo = i.esDefinitivo ? "SI" : "NO",
                fase = i.fase,
                numero_certificado = i.numero_certificado,
                ordenServicio = i.ordenServicio,
                porcentajeAvance = i.porcentajeAvance,
                vigente = i.vigente,
                estado = i.estado,
                formatEstado = Enum.GetName(typeof(EstadoCertificadoIngenieria), i.estado),
                Proyecto = i.Proyecto

            };
            //Periodo fecha de Corte;
            DateTime fechaCorte = i.fechaCorte;
            int mesactual = fechaCorte.Month;
           
            DateTime fechaInicio = i.fechaEmision;
            ci.periodo = fechaInicio.ToShortDateString() + " - " + fechaCorte.ToShortDateString();

            var list = _detalle.GetAll().Where(c => c.CertificadoIngenieriaId == i.Id).ToList();

            ci.detalles = list;

            return ci;


        }

        public List<CertificadoIngenieriaDto> ListAll(int Id)
        {
            var query = Repository.GetAllIncluding(c => c.Proyecto).Where(c => c.vigente).Where(c => c.ProyectoId == Id).ToList();
            var list = (from i in query
                        select new CertificadoIngenieriaDto()
                        {
                            Id = i.Id,
                            ProyectoId = i.ProyectoId,
                            Proyecto = i.Proyecto,
                            nombreProyecto = i.Proyecto.nombre_proyecto,
                            fechaCorte = i.fechaCorte,
                            fechaEmision = i.fechaEmision,
                            fechaEnvio = i.fechaEnvio,
                            formatFechaCorte = i.fechaCorte.ToShortDateString(),
                            formatFechaEmision = i.fechaEmision.ToShortDateString(),
                            formatFechaEnvio = i.fechaEnvio.HasValue ? i.fechaEnvio.Value.ToShortDateString() : "",
                            esDefinitivo = i.esDefinitivo,
                            version = i.version,
                            formatDefinitivo = i.esDefinitivo ? "SI" : "NO",
                            fase = i.fase,
                            numero_certificado = i.numero_certificado,
                            ordenServicio = i.ordenServicio,
                            porcentajeAvance = i.porcentajeAvance,
                            vigente = i.vigente,
                            estado = i.estado,
                            formatEstado = Enum.GetName(typeof(EstadoCertificadoIngenieria), i.estado),

                        }).ToList();

            foreach (var i in list)
            {
                //Periodo fecha de Corte;
                DateTime fechaCorte = i.fechaCorte;
                int mesactual = fechaCorte.Month;

                DateTime fechaInicio =i.fechaEmision;
                i.periodo = fechaInicio.ToShortDateString() + " - " + fechaCorte.ToShortDateString();

                var detalles = _detalle.GetAll().Where(c => c.CertificadoIngenieriaId == i.Id).ToList();

                i.detalles = detalles;

            }

            return list;
        }

        public ExcelPackage ObtenerCertificadoIngenieria(int Id)
        {
            /*Datos Cabecera*/

            var cabecera = this.GetDetalle(Id);

            var oferta_definitiva = _ofertaRepository.GetAll().Where(c => c.ProyectoId == cabecera.ProyectoId)
                                                            .Where(c => c.es_final)
                                                            .Where(c => c.vigente)
                                                            .OrderByDescending(c => c.version).FirstOrDefault();

            var computos = _computoRepository.GetAllIncluding(c => c.Item.Grupo, c => c.Wbs.Oferta).Where(c => c.vigente)
                                                                      .Where(c => c.Item.Grupo.codigo == ProyectoCodigos.CODE_INGENIERIA)
                                                                      .Where(c => c.Wbs.OfertaId == oferta_definitiva.Id)
                                                                      .Where(c => c.Wbs.Oferta.es_final)
                                                                      .ToList();



            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificadoIngenieria.xlsx");
            if (File.Exists((string)filename))
            {


                FileInfo newFile = new FileInfo(filename);

                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("Ingenieria", pck.Workbook.Worksheets[1]);

            }
            ExcelWorksheet h = excel.Workbook.Worksheets[1];
            string cell = "C6";
            h.Cells[cell].Value = cabecera.Proyecto.codigo + " " + cabecera.Proyecto.nombre_proyecto;
            cell = "C7";
            h.Cells[cell].Value = cabecera.fase;
            cell = "C8";

            h.Cells[cell].Value = cabecera.fase;
            cell = "G8";

            h.Cells[cell].Value = cabecera.formatFechaEmision;
            cell = "C9";
            h.Cells[cell].Value = cabecera.numero_certificado;
            cell = "G7";
            h.Cells[cell].Value = (from c in computos select c.cantidad).ToList().Sum();
            cell = "G9";
            h.Cells[cell].Value = cabecera.formatFechaCorte;
            cell = "C10";
            h.Cells[cell].Value = cabecera.periodo;

            int first_row = 13;
            int count = first_row;


            var data_directos = (from d in cabecera.detalles where d.TipoColaborador == TipoColaborador.Directo select d).ToList();
            var data_indirectos = (from d in cabecera.detalles where d.TipoColaborador == TipoColaborador.Indirecto select d).ToList();
            decimal suma_horas_directos = 0;
            decimal suma_horas_directosID = 0;
            decimal suma_horas_directosIB = 0;
            decimal suma_horas_directosAB = 0;
            decimal suma_costo_total_directos = 0;
            decimal suma_horas_indirectos = 0;
            decimal suma_costo_total_indirectos = 0;


            decimal monto_total_ingenieria = 0;

            decimal total_actual = 0;
            decimal total_costo_total = 0;


            var certanterior = Repository.GetAll().Where(c => c.fechaCorte < cabecera.fechaCorte).Where(c => c.estado == EstadoCertificadoIngenieria.Aprobado).Where(c=>c.esDefinitivo).Where(c=>c.vigente).OrderByDescending(c=>c.fechaCorte).FirstOrDefault();
            decimal total_anterior = 0;
            decimal total_costo_anterior = 0;

            if (certanterior != null && certanterior.Id > 0) {
                total_anterior = certanterior.totalacumulado;
                total_costo_anterior = certanterior.totalusd;
            }

            decimal total_acumulado = 0;
            decimal total_costo_acumulado = 0;


            if (data_directos.Count > 0)
            {
                suma_horas_directos = (from s in data_directos select s.TotalHoras).Sum();
                suma_costo_total_directos = (from s in data_directos select s.CostoTotal).Sum();
                suma_horas_directosID = (from s in data_directos where s.Etapa=="ID" select s.TotalHoras).Sum();
                suma_horas_directosIB = (from s in data_directos where s.Etapa == "IB" select s.TotalHoras).Sum();
                suma_horas_directosAB = (from s in data_directos where s.Etapa == "AB" select s.TotalHoras).Sum();
            }
            if (data_indirectos.Count > 0)
            {
                suma_horas_indirectos = (from s in data_indirectos select s.TotalHoras).Sum();
                suma_costo_total_indirectos = (from s in data_indirectos select s.CostoTotal).Sum();
            }
            if (cabecera.detalles.Count > 0)
            {
                total_actual = (from s in cabecera.detalles select s.TotalHoras).Sum();
                total_costo_total = (from s in cabecera.detalles select s.CostoTotal).Sum();
            }


            

            var colId = (from co in data_directos select co.ColaboradorId).Distinct().ToList();
            int datacount = colId.Count;
            h.InsertRow(count + 1, datacount - 1);
            foreach (var j in colId)
            {
                var i = (from x in data_directos where x.ColaboradorId == j select x).FirstOrDefault();
                var sumahoras = (from x in data_directos where x.ColaboradorId == j select x.TotalHoras).ToList().Sum();
                var sumacostototal = (from x in data_directos where x.ColaboradorId == j select x.CostoTotal).ToList().Sum();
                cell = "A" + count;
                h.Cells[cell].Value = i.Rubro;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "B" + count;
                h.Cells[cell].Value = i.Nombre;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "C" + count;
                h.Cells[cell].Value = i.Categoria;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "D" + count;
                h.Cells[cell].Value = i.Unidad;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "E" + count;
                h.Cells[cell].Value = sumahoras;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "F" + count;
                h.Cells[cell].Value = i.Tarifa;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "G" + count;
                h.Cells[cell].Value = sumacostototal;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                count++;
            }

            /*foreach (var i in data_directos)
            {

            }*/


            

            var colIdI = (from co in data_indirectos select co.ColaboradorId).Distinct().ToList();
            count = first_row + datacount + 3;
            h.InsertRow(count + 1, colIdI.Count - 1);
            foreach (var j in colIdI)
            {
                var i = (from x in data_indirectos where x.ColaboradorId == j select x).FirstOrDefault();
                var sumahoras = (from x in data_indirectos where x.ColaboradorId == j select x.TotalHoras).ToList().Sum();
                var sumacostototal = (from x in data_indirectos where x.ColaboradorId == j select x.CostoTotal).ToList().Sum();
                cell = "A" + count;
                h.Cells[cell].Value = i.Rubro;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "B" + count;
                h.Cells[cell].Value = i.Nombre;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "C" + count;
                h.Cells[cell].Value = i.Categoria;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "D" + count;
                h.Cells[cell].Value = i.Unidad;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "E" + count;
                h.Cells[cell].Value = sumahoras;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "F" + count;
                h.Cells[cell].Value = i.Tarifa;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "G" + count;
                h.Cells[cell].Value = sumacostototal;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                count++;
            }/*
                foreach (var i in data_indirectos)
            {
               
            }*/

            /*TOTALES DIRECTOS*/
            var d_sub_horas = (from c in h.Cells where c.Value?.ToString().Contains("SUBD") == true select c).FirstOrDefault();
            h.Cells[d_sub_horas.Address].Value = suma_horas_directos;
            var ib = (from c in h.Cells where c.Value?.ToString().Contains("TIB") == true select c).FirstOrDefault();
            h.Cells[ib.Address].Value = suma_horas_directosIB;
            var id = (from c in h.Cells where c.Value?.ToString().Contains("TIDD") == true select c).FirstOrDefault();
            h.Cells[id.Address].Value = suma_horas_directosID;
            var AB = (from c in h.Cells where c.Value?.ToString().Contains("TABB") == true select c).FirstOrDefault();
            h.Cells[AB.Address].Value = suma_horas_directosAB;

            var d_sub_costo_t = (from c in h.Cells where c.Value?.ToString().Contains("SUBDT") == true select c).FirstOrDefault();
            h.Cells[d_sub_costo_t.Address].Value = suma_costo_total_directos;
            /*TOTALES INDIRECTOS*/

            var d_subi_horas = (from c in h.Cells where c.Value?.ToString().Contains("TTIND") == true select c).FirstOrDefault();
            h.Cells[d_subi_horas.Address].Value = suma_horas_indirectos;
            var d_subi_costo_t = (from c in h.Cells where c.Value?.ToString().Contains("SUBIT") == true select c).FirstOrDefault();
            h.Cells[d_subi_costo_t.Address].Value = suma_costo_total_indirectos;

            /* TOTAL ACTUAL Y USD*/
            var t_horas = (from c in h.Cells where c.Value?.ToString().Contains("TEACTUAL") == true select c).FirstOrDefault();
            h.Cells[t_horas.Address].Value = total_actual;
            var t_costo_total = (from c in h.Cells where c.Value?.ToString().Contains("TUACTUAL") == true select c).FirstOrDefault();
            h.Cells[t_costo_total.Address].Value = total_costo_total;

            /* TOTAL ANTERIOR Y USD*/
            var t_A = (from c in h.Cells where c.Value?.ToString().Contains("TAA") == true select c).FirstOrDefault();
            h.Cells[t_A.Address].Value = total_anterior;
            var t_costo_a = (from c in h.Cells where c.Value?.ToString().Contains("TC") == true select c).FirstOrDefault();
            h.Cells[t_costo_a.Address].Value = total_costo_anterior;

            total_acumulado = total_anterior + total_actual;
            total_costo_acumulado = total_costo_anterior + total_costo_total;

            var t_acumulado = (from c in h.Cells where c.Value?.ToString().Contains("TM") == true select c).FirstOrDefault();
            h.Cells[t_acumulado.Address].Value = total_acumulado;
            var t_costo_acum = (from c in h.Cells where c.Value?.ToString().Contains("TCM") == true select c).FirstOrDefault();
            h.Cells[t_costo_acum.Address].Value = total_costo_acumulado;



            /*MONTO TOTAL INGENIERIA*/
            if (computos.Count > 0)
            {
                monto_total_ingenieria = (from i in computos select i.costo_total).Sum();
            }
            var moi = (from c in h.Cells where c.Value?.ToString().Contains("MOI") == true select c).FirstOrDefault();
            h.Cells[moi.Address].Value = monto_total_ingenieria;

            decimal saldovs = monto_total_ingenieria - total_costo_acumulado;
            var svs = (from c in h.Cells where c.Value?.ToString().Contains("MT") == true select c).FirstOrDefault();
            h.Cells[svs.Address].Value = saldovs;


            var cert = Repository.Get(Id);
            cert.totalacumulado = total_actual;
            cert.totalusd = total_costo_total;
            Repository.Update(cert);
            return excel;
        }

        public int Anular(int id)
        {
            var e = Repository.Get(id);
            e.estado = EstadoCertificadoIngenieria.Anulado;
            Repository.Update(e);

            var secuencia = _secuencial.GetAllIncluding().Where(c => c.ProyectoId == e.ProyectoId).FirstOrDefault();
            if (secuencia != null && secuencia.Id > 0)
            {
                if (secuencia.secuencia > 1)
                {
                    secuencia.secuencia = secuencia.secuencia - 1;
                    _secuencial.Update(secuencia);
                }
            }


            var anuladritems = _detalle.GetAll().Where(c => c.CertificadoIngenieriaId == id).ToList();
            foreach (var di in anuladritems)
            {
                var it = _detalleItem.GetAllIncluding(c => c.DetalleAvanceIngenieria).Where(c => c.Id == di.DetalleItemIngenieriaId).FirstOrDefault();
                var da = _detalleAvanceI.Get(it.DetalleAvanceIngenieriaId);
                da.estacertificado = false;
                _detalleAvanceI.Update(da);

            }



            return 1;
        }
        public int Aprobar(int id)
        {
            var e = Repository.Get(id);
            e.estado = EstadoCertificadoIngenieria.Aprobado;
            Repository.Update(e);


            var secuencia = _secuencial.GetAllIncluding().Where(c => c.ProyectoId == e.ProyectoId).FirstOrDefault();
            if (secuencia != null && secuencia.Id > 0)
            {

                secuencia.secuencia = secuencia.secuencia + 1;
                _secuencial.Update(secuencia);
            }

            var anuladritems = _detalle.GetAll().Where(c => c.CertificadoIngenieriaId == id).ToList();
            foreach (var di in anuladritems)
            {
                var it = _detalleItem.GetAllIncluding(c => c.DetalleAvanceIngenieria).Where(c => c.Id == di.DetalleItemIngenieriaId).FirstOrDefault();
                var da = _detalleAvanceI.Get(it.DetalleAvanceIngenieriaId);
                da.estacertificado = true;
                _detalleAvanceI.Update(da);

            }



            return 1;
        }
    }
}

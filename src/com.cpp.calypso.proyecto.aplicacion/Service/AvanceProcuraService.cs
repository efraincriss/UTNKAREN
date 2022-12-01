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
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class AvanceProcuraServiceAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<AvanceProcura, AvanceProcuraDto, PagedAndFilteredResultRequestDto>, IAvanceProcuraAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<DetalleAvanceProcura> _detalleAvanceProcuraRepository;
        private readonly IBaseRepository<Proyecto> _proyectoRepository;
        private readonly IBaseRepository<Oferta> _ofertaRepository;
        private readonly IBaseRepository<Wbs> _wbsrepository;
        private readonly IBaseRepository<Computo> _computorepository;
        private readonly IBaseRepository<Colaborador> _colaboradorepository;
        private readonly IDetalleAvanceProcuraAsyncBaseCrudAppService _detalleAvanceProcura;

        public AvanceProcuraServiceAsyncBaseCrudAppService(
            IBaseRepository<AvanceProcura> repository,
            IBaseRepository<DetalleAvanceProcura> detalleAvanceProcuraRepository,
            IBaseRepository<DetalleItemIngenieria> detalleitemingenieria,
            IBaseRepository<Oferta> ofertaRepository,
            IBaseRepository<Proyecto> proyectoRepository,
            IBaseRepository<Wbs> wbsrepository,
            IBaseRepository<Computo> computorepository,
            IBaseRepository<Colaborador> colaboradorepository,
            IDetalleAvanceProcuraAsyncBaseCrudAppService detalleAvanceProcura
        ) : base(repository)
        {
            _detalleAvanceProcuraRepository = detalleAvanceProcuraRepository;

            _proyectoRepository = proyectoRepository;
            _ofertaRepository = ofertaRepository;
            _wbsrepository = wbsrepository;
            _computorepository = computorepository;
            _colaboradorepository = colaboradorepository;
            _detalleAvanceProcura = detalleAvanceProcura;
        }

        public decimal GetMontoPresupuestado(int ofertaId)
        {
            decimal monto_presupuestado = 0;
            var computos = _detalleAvanceProcura.GetComputos(ofertaId);

            foreach (var c in computos)
            {
                monto_presupuestado += c.costo_total;
            }
            return monto_presupuestado;
        }

        public bool comprobarfecha(DateTime fechadesde, DateTime fechahasta)
        {
            if (fechahasta > fechadesde)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Eliminar(int avanceProcuraId)
        {
            var orden = Repository.Get(avanceProcuraId);
            if (orden != null)
            {
                orden.vigente = false;
                Repository.Update(orden);
                return orden.Id;
            }

            return 0;

        }

        public AvanceProcuraDto getdetalles(int AvanceProcuraId)
        {
            var query = Repository.GetAllIncluding(o => o.Oferta, c => c.Oferta.Proyecto, c => c.Oferta.Proyecto.codigo);
            var items = (from a in query

                         where a.vigente == true
                         where a.Id == AvanceProcuraId
                         select new AvanceProcuraDto()
                         {

                             Id = a.Id,
                             CertificadoId = a.CertificadoId,
                             OfertaId = a.OfertaId,
                             fecha_desde = a.fecha_desde,
                             fecha_hasta = a.fecha_hasta,
                             fecha_presentacion = a.fecha_presentacion,
                             estado = a.estado,
                             vigente = a.vigente,
                             Oferta = a.Oferta,
                             Proyecto = a.Oferta.Proyecto,
                             aprobado = a.aprobado,
                             monto_procura = a.monto_procura

                         }).FirstOrDefault();

            return items;

        }

        public List<AvanceProcuraDto> ListarPorOferta(int ofertaId)
        {
            var query = Repository.GetAllIncluding(o => o.Oferta, c => c.Oferta.Proyecto, c => c.Oferta.Proyecto.codigo).Where(o => o.vigente == true)
                .Where(o => o.OfertaId == ofertaId);

            var items = (from a in query

                         select new AvanceProcuraDto()
                         {
                             Id = a.Id,
                             CertificadoId = a.CertificadoId,
                             OfertaId = a.OfertaId,
                             fecha_desde = a.fecha_desde,
                             fecha_hasta = a.fecha_hasta,
                             fecha_presentacion = a.fecha_presentacion,
                             estado = a.estado,
                             vigente = a.vigente,
                             Oferta = a.Oferta,
                             Proyecto = a.Oferta.Proyecto,
                             aprobado = a.aprobado,
                             monto_procura = a.monto_procura,

                         }).ToList();


            return items;
        }
        public List<OfertaDto> ListarOfertasDeProyecto(int ProyectoId)
        {
            var OfertaQuery = _ofertaRepository.GetAllIncluding(c => c.Proyecto.Contrato.Cliente);

            var items = (from o in OfertaQuery
                         where o.vigente == true
                         where o.ProyectoId == ProyectoId
                         where o.es_final == true
                         select new OfertaDto()
                         {
                             Id = o.Id,
                             codigo = o.codigo,
                             fecha_oferta = o.fecha_oferta,
                             estado_oferta = o.estado_oferta,
                             cliente_razon_social = o.Proyecto.Contrato.Cliente.razon_social,
                             proyecto_codigo = o.Proyecto.codigo
                         }).ToList();
            return items;
        }
        public List<AvanceProcuraDto> ListarAvancesDeOfertaSinCertificar(int OfertaId)
        {
            var AvanceObraQuery = Repository.GetAll();
            var items = (from a in AvanceObraQuery
                         where a.vigente == true
                         where a.OfertaId == OfertaId
                         where a.CertificadoId == 0
                         select new AvanceProcuraDto()
                         {
                             Id = a.Id,
                             CertificadoId = a.CertificadoId,
                             OfertaId = a.OfertaId,
                             fecha_desde = a.fecha_desde,
                             fecha_hasta = a.fecha_hasta,
                             fecha_presentacion = a.fecha_presentacion,
                             estado = a.estado,
                             vigente = a.vigente,
                             Oferta = a.Oferta,
                             Proyecto = a.Oferta.Proyecto,
                             aprobado = a.aprobado,
                             monto_procura = a.monto_procura
                         }).ToList();

            return items;
        }

        public List<DetalleAvanceProcuraDto> ListarPorAvanceProcura(int avanceProcuraId)
        {
            var query = _detalleAvanceProcuraRepository.GetAllIncluding(o => o.AvanceProcura, o => o.DetalleOrdenCompra, o => o.DetalleOrdenCompra.OrdenCompra, o => o.AvanceProcura.Oferta, o => o.AvanceProcura.Oferta.Proyecto, o => o.DetalleOrdenCompra.Computo.Item);
            var items = (from a in query
                         where a.AvanceProcuraId == avanceProcuraId
                         where a.vigente == true
                         where a.estacertificado == false
                         select new DetalleAvanceProcuraDto()
                         {
                             Id = a.Id,
                             vigente = a.vigente,
                             AvanceProcuraId = a.AvanceProcuraId,
                             AvanceProcura = a.AvanceProcura,
                             DetalleOrdenCompraId = a.DetalleOrdenCompraId,
                             DetalleOrdenCompra = a.DetalleOrdenCompra,
                             Item = a.DetalleOrdenCompra.Computo.Item,
                             fecha_real = a.fecha_real,
                             estado = a.estado,
                             valor_real = a.valor_real,
                             cantidad = a.cantidad,
                             precio_unitario = a.precio_unitario,
                             calculo_anterior = a.calculo_anterior,
                             calculo_diario = a.calculo_diario,
                             ingreso_acumulado = a.ingreso_acumulado,
                             OrdenCompra = a.DetalleOrdenCompra.OrdenCompra,
                             Computo = a.DetalleOrdenCompra.Computo,

                         }).ToList();



            foreach (var d in items)
            {
                d.fechar = d.fecha_real.ToShortDateString();
            }
            return items;
        }

        public List<DetalleAvanceProcuraDto> ListarDetallesAvanceProcuraProyecto(int ProyectoId)
        {
            List<DetalleAvanceProcuraDto> detallesavanceobra = new List<DetalleAvanceProcuraDto>();


            var ofertas = this.ListarOfertasDeProyecto(ProyectoId);
            if (ofertas.Count > 0)
            {
                foreach (var offinal in ofertas)
                {

                    var avances = this.ListarAvancesDeOfertaSinCertificar(offinal.Id);

                    if (avances.Count > 0)
                    {
                        foreach (var ovance in avances)
                        {

                            var detalles = this.ListarPorAvanceProcura(ovance.Id);

                            if (detalles.Count > 0)
                            {
                                foreach (var da in detalles)
                                {
                                    detallesavanceobra.Add(da);
                                }
                            }

                        }


                    }

                }

            }


            return detallesavanceobra;

        }


        public ExcelPackage ObtenerCertificadoProcura(int Id)
        {
            var fecha_corte = DateTime.Now.Date;
            /*Datos Cabecera*/

            /* var oferta = _ofertaRepository.GetAllIncluding(c => c.Requerimiento, c => c.Proyecto).Where(c => c.Id == Id).FirstOrDefault();
             var computos = _computorepository.GetAllIncluding(c => c.Item.Grupo, c => c.Wbs.Oferta).Where(c => c.vigente)
                                                                       .Where(c => c.Item.Grupo.codigo == ProyectoCodigos.CODE_PROCURA_CONTRATISTA)
                                                                       .Where(c => c.Wbs.OfertaId == Id)
                                                                       .Where(c => c.Wbs.Oferta.es_final)
                                                                       .ToList();*/


            // var datos = this.Datos(oferta.Id);


            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/SuministrosPlanti.xlsx");
            if (File.Exists((string)filename))
            {


                FileInfo newFile = new FileInfo(filename);

                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("Suministros", pck.Workbook.Worksheets[1]);

            }
            ExcelWorksheet h = excel.Workbook.Worksheets[1];
            var number = "" + h.Cells["J2"].Value;
            decimal costo = Convert.ToDecimal(number);


            var ofertas_definitivas = _ofertaRepository.GetAllIncluding(c => c.Proyecto)
                                                   .Where(c => c.es_final)
                                                   .Where(c => c.vigente)
                                                   .ToList();

            List<int> meses = new List<int>();
            int count = 5;
            string cell = "";

            foreach (var o in ofertas_definitivas)
            {

                var avanceprocura = Repository.GetAll().Where(c => c.fecha_presentacion <= fecha_corte).Where(c=>c.OfertaId==o.Id).Where(c=>c.vigente).ToList();
                var avanceplanificados = Repository.GetAll().Where(c => c.fecha_presentacion <= fecha_corte).Where(c => c.OfertaId == o.Id).Where(c => c.vigente).ToList();

                var listmes = (from f in avanceprocura select f.fecha_presentacion.Month).Distinct().ToList();
                if (listmes.Count > 0)
                {
                    meses.AddRange(listmes);
                }

            }
            var fechasdistintas = (from x in meses orderby x select x).Distinct().ToList();
            h.InsertColumn(11, fechasdistintas.Count * 2);
            int rowcabecera = 4;
            var column = 11;
            foreach (var f in fechasdistintas)
            {
                h.Cells[4, column].Value = "Avance % " + this.AbrevMonthName(f);
                h.Cells[4, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                h.Cells[4, column].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 217, 102));
                h.Cells[4, column].Style.WrapText = true;
                h.Cells[4, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                h.Cells[4, column].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Column(column).Width = 12;

                h.Cells[4, column + 1].Value = "Avance Costo " + this.AbrevMonthName(f);
                h.Cells[4, column + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                h.Cells[4, column + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 217, 102));
                h.Cells[4, column + 1].Style.WrapText = true;
                h.Cells[4, column + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                h.Cells[4, column + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                h.Column(column + 1).Width = 14;
                column = column + 2;
            }
            int ncol = h.Dimension.End.Column;
            foreach (var o in ofertas_definitivas)
            {
                var datos = this.Datos(fecha_corte, costo, o.Id);
                var distintcomputos = (from d in datos select d.ComputoId).Distinct().ToList();


                if (datos.Count > 0)
                {
                    h.InsertRow(count, datos.Count);
                    int inicial = count;
                    int final = count;
                    foreach (var e in distintcomputos)
                    {
                        var d = (from x in datos where x.ComputoId == e select x).FirstOrDefault();

                        cell = "B" + count;
                        h.Cells[cell].Value = d.Proyecto;
                        cell = "C" + count;
                        h.Cells[cell].Value = d.MR;
                        cell = "D" + count;
                        h.Cells[cell].Value = d.OC;
                        cell = "E" + count;
                        h.Cells[cell].Value = d.DescripcionSuministros;
                        cell = "F" + count;
                        h.Cells[cell].Value = d.PO;
                        cell = "G" + count;
                        h.Cells[cell].Value = d.FAT;
                        cell = "H" + count;
                        h.Cells[cell].Value = d.ArriboETA;
                        cell = "I" + count;
                        h.Cells[cell].Value = d.Costo;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                        cell = "J" + count;
                        h.Cells[cell].Value = d.Venta;
                        h.Cells[cell].Style.Numberformat.Format = "#,##0.00";

                        var col = 11;
                        foreach (var f in fechasdistintas)
                        {
                            h.Cells[count, col].Value = ((from x in datos where x.ComputoId == e where x.FechaPresentacionAvance.Month == f select x.Porcentaje).Sum()/100);
                            h.Cells[count, col].Style.Numberformat.Format = "#0.00%";
                            h.Cells[count, col+1].Value = (from x in datos where x.ComputoId == e where x.FechaPresentacionAvance.Month == f select x.ValorRealProcura).Sum();
                            h.Cells[count, col+1].Style.Numberformat.Format = "#,##0.00";
                            col = col + 2;
                        }
                        h.Cells[count, ncol - 1].Value = d.AvanceProcentajeTotal;
                        h.Cells[count, ncol - 1].Style.Numberformat.Format = "#0.00%";

                        h.Cells[count, ncol].Value = d.AvanceCostoTotal;
                        h.Cells[count, ncol].Style.Numberformat.Format = "#,##0.00";

                        string cuerpo = "B" + count + ":J" + count;
                        h.Cells[cuerpo].Style.Font.Bold = false;
                        h.Cells[cuerpo].Style.Font.Color.SetColor(Color.Black);
                        h.Cells[cuerpo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        h.Cells[cuerpo].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                        count++;
                        final++;
                    }
                    h.Cells[inicial, 2, final, 12].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                    cell = "B" + count;
                    h.Cells[cell].Value = "SUB TOTAL " + datos[0].Proyecto;

                    /*Totales*/
                    decimal totalCosto = (from e in datos where e.FechaPresentacionAvance.Date<=fecha_corte.Date select e.Costo).ToList().Sum();
                    decimal totalVenta = (from e in datos where e.FechaPresentacionAvance.Date <= fecha_corte.Date select e.Venta).ToList().Sum();

                    cell = "I" + count;
                    h.Cells[cell].Value = totalCosto;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
                    cell = "J" + count;
                    h.Cells[cell].Value = totalVenta;
                    h.Cells[cell].Style.Numberformat.Format = "#,##0.00";
             
                    h.Cells[count, ncol].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    int cfinal = h.Dimension.End.Column;
                    var rango_incio = h.Cells[inicial, cfinal-1].Address;
                    var rango_final = h.Cells[final-1, cfinal - 1].Address;
                    var rangosumar = "$" + rango_incio + ":" + "$" + rango_final;
          
                   // h.Cells[count, ncol - 1].Formula = "=SUM(" + rangosumar + ")";
                    h.Cells[count, ncol - 1].Style.Numberformat.Format = "#0.00%";

                     rango_incio = h.Cells[inicial, cfinal].Address;
                     rango_final = h.Cells[final-1, cfinal].Address;
                     rangosumar = "$" + rango_incio + ":" + "$" + rango_final;

                    h.Cells[count, ncol].Formula = "=SUM(" + rangosumar + ")";
                    h.Cells[count, ncol].Style.Numberformat.Format = "#,##0.00";


                    int colfinal = h.Dimension.End.Column;
                    h.Cells[count,2,count,colfinal].Style.Font.Bold = true;
                    h.Cells[count, 2, count, colfinal].Style.Font.Color.SetColor(Color.White);
                    h.Cells[count, 2, count, colfinal].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    h.Cells[count, 2, count, colfinal].Style.Fill.BackgroundColor.SetColor(Color.Black);
                    count++;

                }
            }
            h.Cells["B1"].Value = count - 1;
            h.Cells["B1"].Style.Numberformat.Format = "0";


            /*

            int ncol = h.Dimension.End.Column;


            var valuefinal = Int32.Parse(""+h.Cells["B1"].Value);
            for (int i = 5; i <=valuefinal; i++)
            {
                var rango_incio = h.Cells[i, 11].Address;
                var rango_final = h.Cells[i, ncol-2].Address;
                var rangosumar = "$" + rango_incio + ":" + "$" + rango_final;
                h.Cells[i, ncol-1].Formula = "=SUM(" + rangosumar + ")";
            }
            */


            return excel;
        }

        public List<ProcuraDatos> Datos(DateTime fecha_corte, decimal costo, int OfertaId)
        {
            var avances = _detalleAvanceProcuraRepository.GetAllIncluding(i => i.AvanceProcura.Oferta.Proyecto, i => i.DetalleOrdenCompra.OrdenCompra, i => i.DetalleOrdenCompra.Computo.Item)
                                                        .Where(i => i.vigente)
                                                        .Where(i => i.AvanceProcura.OfertaId == OfertaId)
                                                        .ToList();
            var list = (from d in avances
                        select new ProcuraDatos()
                        {
                            Id = d.Id,
                            ComputoId = d.DetalleOrdenCompra.ComputoId,
                            Porcentaje = d.DetalleOrdenCompra.porcentaje,
                            DetalleOrdenCompraId = d.DetalleOrdenCompraId,
                            Proyecto = d.AvanceProcura.Oferta.Proyecto.codigo,
                            ProyectoId = d.AvanceProcura.Oferta.Proyecto.Id,
                            MR = d.DetalleOrdenCompra.OrdenCompra.referencia,
                            OC = d.DetalleOrdenCompra.OrdenCompra.nro_pedido_compra,
                            PO = (from f in avances orderby f.DetalleOrdenCompra.fecha where f.Id == d.Id select f.DetalleOrdenCompra.fecha).Min().ToShortDateString(),
                            FAT = "",
                            ArriboETA = (from f in avances orderby f.DetalleOrdenCompra.fecha where f.Id == d.Id select f.DetalleOrdenCompra.fecha).Max().ToShortDateString(),
                            FechaAvance = d.fecha_real,
                            Costo = d.DetalleOrdenCompra.Computo.costo_total,//d.valor_real,
                            Venta = d.DetalleOrdenCompra.Computo.costo_total * costo,
                            DescripcionSuministros = d.DetalleOrdenCompra.Computo.Item.nombre,
                            Cantidad=d.cantidad,
                            PorcentajeDetalle=d.DetalleOrdenCompra.porcentaje,
                            ValorRealProcura=d.valor_real,
                            FechaPresentacionAvance=d.AvanceProcura.fecha_presentacion


                        }).ToList();

            foreach (var i in list)
            {
                i.AvanceProcentajeTotal = this.AvancePorcentajeTotal(i.ComputoId, fecha_corte, avances, i.Venta);
                i.AvanceCostoTotal = this.AvanceCostoTotal(i.ComputoId, avances, fecha_corte, i.Venta);
            }

            return list;

        }
        public string MonthName(int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return dtinfo.GetMonthName(month);
        }
        public string AbrevMonthName(int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return dtinfo.GetAbbreviatedMonthName(month);
        }
        public decimal AvancePorcentajeTotal(int ComputoId,DateTime fecha, List<DetalleAvanceProcura> list, decimal venta)
        {
            decimal x = (from d in list  where d.DetalleOrdenCompra.ComputoId==ComputoId where d.AvanceProcura.fecha_presentacion.Date<=fecha.Date select d.DetalleOrdenCompra.porcentaje).Sum() / 100;
            return x;
        }
        public decimal AvanceCostoTotal(int ComputoId, List<DetalleAvanceProcura> list, DateTime fecha, decimal venta)
        {
            decimal x = (from d in list where d.DetalleOrdenCompra.ComputoId == ComputoId where d.AvanceProcura.fecha_presentacion.Date <= fecha.Date select d.valor_real).Sum();
            return x;

        }
    }
}


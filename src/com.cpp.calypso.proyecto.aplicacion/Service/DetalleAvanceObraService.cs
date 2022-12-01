using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.Style;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class DetalleAvanceObraAsyncBaseCrudAppService : AsyncBaseCrudAppService<DetalleAvanceObra, DetalleAvanceObraDto, PagedAndFilteredResultRequestDto>, IDetalleAvanceObraAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Computo> _computoRepository;
        private readonly IBaseRepository<Item> _itemRepository;
        private readonly IBaseRepository<Proyecto> _proyectoRepository;
        private readonly IBaseRepository<AvanceObra> _avanceobraRepository;
        private readonly IBaseRepository<DetalleAvanceObra> _davanceobraRepository;

        public DetalleAvanceObraAsyncBaseCrudAppService(
            IBaseRepository<DetalleAvanceObra> repository,
            IBaseRepository<Computo> computoRepository,
            IBaseRepository<AvanceObra> avanceobraRepository,
            IBaseRepository<DetalleAvanceObra> davanceobraRepository,
             IBaseRepository<Proyecto> proyectoRepository,
             IBaseRepository<Item> itemRepository
            ) : base(repository)
        {
            _computoRepository = computoRepository;
            _avanceobraRepository = avanceobraRepository;
            _davanceobraRepository = davanceobraRepository;
            _proyectoRepository = proyectoRepository;
            _itemRepository = itemRepository;
        }

        public List<ComputoDto> ListaComputosPorOferta(int ofertaId)
        {
            var computoQuery = _computoRepository.GetAllIncluding(a => a.Wbs.Oferta, c => c.Item);

            var items = (from c in computoQuery
                         where c.vigente == true
                         where c.Wbs.Oferta.Id == ofertaId
                         select new ComputoDto()
                         {
                             Id = c.Id,
                             /*  area = c.Wbs.AreaId,
                               diciplina = c.Wbs.DisciplinaId,
                               elemento = c.Wbs.ElementoId,
                               actividad = c.Wbs.ActividadId,
                               */
                             item_codigo = c.Item.codigo,
                             item_nombre = c.Item.nombre,

                         }).ToList();
            return items;
        }


        public async Task<int> CreateDetalleAvance(DetalleAvanceObraDto detalle, decimal cantidad_eac)
        {
            var count = Repository
                .GetAll()
                .Where(o => o.vigente == true)
                .Where(o => o.AvanceObraId == detalle.AvanceObraId).Count(o => o.ComputoId == detalle.ComputoId);

            if (count > 0)
            {
                return -1;
            }
            detalle.fecha_registro = DateTime.Now;
            detalle.vigente = true;

            var id = await Repository.InsertAndGetIdAsync(MapToEntity(detalle));
            var computo = _computoRepository.Get(detalle.ComputoId);
            computo.cantidad_eac = cantidad_eac;
            _computoRepository.Update(computo);
            return id;
        }


        public decimal calcularvalor(int AvanceObraId)
        {

            decimal total = 0;
            var query = _davanceobraRepository.GetAllIncluding(c => c.AvanceObra, c => c.AvanceObra.Oferta);
            var items = (from a in query
                         where a.vigente == true
                         where a.AvanceObraId == AvanceObraId
                         select new DetalleAvanceObraDto()
                         {
                             Id = a.Id,
                             vigente = a.vigente,
                             AvanceObraId = a.AvanceObraId,
                             ComputoId = a.ComputoId,
                             Computo = a.Computo,
                             precio_unitario = a.precio_unitario,
                             calculo_anterior = a.calculo_anterior,
                             calculo_diario = a.calculo_diario,
                             ingreso_acumulado = a.ingreso_acumulado,
                             total = a.total
                         }).ToList();


            foreach (var i in items)
            {
                total = total + i.total;

            }
            var avance = _avanceobraRepository.Get(AvanceObraId);
            avance.monto_total = total;
            var resultado = _avanceobraRepository.Update(avance);
            return total;

        }

        public decimal GetMonto(int AvanceObraId)
        {
            decimal total = 0;
            var query = _davanceobraRepository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.AvanceObraId == AvanceObraId)
                .Where(o => o.total > 0);

            var totales = (from i in query
                           select new DetalleAvanceObraDto()
                           {
                               total = i.total
                           }).ToList();

            foreach (var t in totales)
            {
                total += t.total;
            }

            return total;
        }

        public async Task<int> GuardarDetalles(List<ComputoAvanceObra> lista, int AvanceObraId)
        {
            var cont = 0;
            decimal total = 0;
            foreach (var i in lista)
            {

                var dato = _computoRepository.Get(i.ComputoId);
                if (i.CantidadEAC >= 0 && i.CantidadEAC >= i.CantidadAnterior)
                {
                    dato.cantidad_eac = i.CantidadEAC;

                    _computoRepository.Update(dato);
                }




                if (i.CantidadAcumulada != 0)
                {
                    var detalle = new DetalleAvanceObraDto()
                    {
                        AvanceObraId = AvanceObraId,
                        ComputoId = i.ComputoId,
                        cantidad_acumulada = i.CantidadAcumulada,
                        cantidad_acumulada_anterior = i.CantidadAnterior,
                        fecha_registro = DateTime.Now,
                        cantidad_diaria = i.CantidadAcumulada - i.CantidadAnterior,
                        precio_unitario = i.PrecioUnitario,
                        total = (i.CantidadAcumulada - i.CantidadAnterior) * i.PrecioUnitario,
                        vigente = true,

                    };

                    var itemGuardado = await Repository.InsertOrUpdateAsync(MapToEntity(detalle));
                    total += detalle.total;
                    // Actualizar Computo
                    var computo = _computoRepository.Get(detalle.ComputoId);
                    computo.cantidad_eac = i.CantidadEAC;
                    _computoRepository.Update(computo);
                    // =====================================================
                    if (itemGuardado.Id > 0)
                    {
                        cont++;
                    }
                }
                //AQUI ACTUALIZACIÓN EAC A CERO
                /*if (i.CantidadEAC == 0)
                {
                    var computo = _computoRepository.Get(i.ComputoId);
                    computo.cantidad_eac = i.CantidadEAC;
                    _computoRepository.Update(computo);
                }
                */

            }

            var anterior = this.GetMonto(AvanceObraId);
            var avance = _avanceobraRepository.Get(AvanceObraId);
            avance.monto_total = anterior + total;
            _avanceobraRepository.Update(avance);
            return cont;
        }


        public async Task<int> GuardarDetallesNegativos(ComputoAvanceObra lista, int AvanceObraId)
        {
            var cont = 0;
            decimal total = 0;
            var i = lista;


            var dato = _computoRepository.Get(i.ComputoId);
            if (i.CantidadEAC >= 0 && i.CantidadEAC >= i.CantidadAnterior)
            {
                dato.cantidad_eac = i.CantidadEAC;

                _computoRepository.Update(dato);
            }

            var cantidadAcumulada = 0; //Se Envia cantidad Acumulada en cero
            var detalle = new DetalleAvanceObraDto()
            {
                AvanceObraId = AvanceObraId,
                ComputoId = i.ComputoId,
                cantidad_acumulada = cantidadAcumulada, //Cantidad Acumulada
                cantidad_acumulada_anterior = i.CantidadAnterior,
                fecha_registro = DateTime.Now,
                cantidad_diaria = cantidadAcumulada - i.CantidadAnterior,
                precio_unitario = i.PrecioUnitario,
                total = (cantidadAcumulada - i.CantidadAnterior) * i.PrecioUnitario,
                vigente = true,

            };

            var itemGuardado = await Repository.InsertOrUpdateAsync(MapToEntity(detalle));
            total += detalle.total;
            // Actualizar Computo
            var computo = _computoRepository.Get(detalle.ComputoId);
            computo.cantidad_eac = i.CantidadEAC;
            _computoRepository.Update(computo);
            // =====================================================
            if (itemGuardado.Id > 0)
            {
                cont++;
            }




            var anterior = this.GetMonto(AvanceObraId);
            var avance = _avanceobraRepository.Get(AvanceObraId);
            avance.monto_total = anterior + total;
            _avanceobraRepository.Update(avance);
            return cont;
        }


        public bool cambiaracertificado(int id)
        {
            var r = Repository.Get(id);
            r.estacertificado = false;
            var d = Repository.Update(r);
            return true;
        }

        public int Eliminar(int DetalleAvanceObra)
        {

            var detalle = Repository.Get(DetalleAvanceObra);
            detalle.vigente = false;
            Repository.Update(detalle);

            var total = detalle.total;
            var anterior = this.GetMonto(detalle.AvanceObraId);
            var avance = _avanceobraRepository.Get(detalle.AvanceObraId);
            avance.monto_total = anterior - total;
            _avanceobraRepository.Update(avance);
            return detalle.AvanceObraId;
        }



        public async Task<string> SubirExcelAvanceObra(HttpPostedFileBase UploadedFile, int AvanceObraId)
        {
            var AvanceObra = _avanceobraRepository.Get(AvanceObraId);

            var DetallesAvanceObra = Repository.GetAll()
                                             .Where(c => c.vigente)
                                             .Where(c => c.AvanceObra.vigente)
                                             .Where(c => c.AvanceObraId == AvanceObraId)
                                             .ToList();

            string fileName = UploadedFile.FileName;
            string fileContentType = UploadedFile.ContentType;
            byte[] fileBytes = new byte[UploadedFile.ContentLength];
            var data = UploadedFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(UploadedFile.ContentLength));
            using (var package = new ExcelPackage(UploadedFile.InputStream))
            {

                //VARIABLES MANEJO DEL EXCEL
                var HojaExcel = package.Workbook.Worksheets;
                var Hoja = HojaExcel[1];
                var NumeroColumnas = Hoja.Dimension.End.Column;
                var NumeroDeFilas = Hoja.Dimension.End.Row;
                //PARAMETROS INICIALES
                int FilaInicial = 2;
                int ColumnaComputoId = 1;
                int ColumnaCantidadEAC = 5;
                int ColumnaCantidadAnterior = 6;
                int ColumnaCantidadAcumulada = 7;
                int ColumnaCantidadAjustada = 8;


                for (int fila = FilaInicial; fila <= NumeroDeFilas; fila++)
                {
                    //Recibe Texto del Excel
                    var ValorComputoId = (Hoja.Cells[fila, ColumnaComputoId].Value ?? "").ToString();
                    var ValorCantidadEAC = (Hoja.Cells[fila, ColumnaCantidadEAC].Value ?? "").ToString();
                    var ValorCantidadAnterior = (Hoja.Cells[fila, ColumnaCantidadAnterior].Value ?? "").ToString();
                    var ValorCantidadAcumulada = (Hoja.Cells[fila, ColumnaCantidadAcumulada].Value ?? "").ToString();
                    var ValorTipoCantidadAjustada = (Hoja.Cells[fila, ColumnaCantidadAjustada].Value ?? "").ToString();

                    //Valores Iniciales Campos
                    int ComputoId = 0;
                    decimal CantidadEAC = 0;
                    decimal CantidadAnterior = 0;
                    decimal CantidadAcumulada = 0;

                    if (ValorComputoId.Length > 0)
                    {
                        ComputoId = Int32.Parse(ValorComputoId);
                    }
                    if (ValorCantidadEAC.Length > 0)
                    {
                        CantidadEAC = Decimal.Parse("0" + ValorCantidadEAC, NumberStyles.Float); //acepte 10 exp e-8, 0,000001
                    }
                    if (ValorCantidadAnterior.Length > 0)
                    {
                        CantidadAnterior = Decimal.Parse("0" + ValorCantidadAnterior, NumberStyles.Float); //acepte 10 exp e-8, 0,000001
                    }
                    if (ValorCantidadAcumulada.Length > 0)
                    {
                        CantidadAcumulada = Decimal.Parse("0" + ValorCantidadAcumulada, NumberStyles.Float); //acepte 10 exp e-8, 0,000001
                    }
                    if (ValorTipoCantidadAjustada.Length > 0) {
                        ValorTipoCantidadAjustada = ValorTipoCantidadAjustada.ToUpper();
                    }
                    //Verificamos CantidadesAcumuladas Mayores  al EAC
                    if (ComputoId > 0)
                    {
                        var Computo = _computoRepository.Get(ComputoId);
                        if (ValorTipoCantidadAjustada.Length > 0)
                        {
                            if (ValorTipoCantidadAjustada == ProyectoCodigos.CANTIDAD_AJUSTADA_INGENIERIA || ValorTipoCantidadAjustada == ProyectoCodigos.CANTIDAD_AJUSTADA_RED_LINE || ValorTipoCantidadAjustada == ProyectoCodigos.CANTIDAD_AJUSTADA_TOPOGRAFIA)
                            {
                                Computo.cantidadAjustada = true;
                                Computo.tipo = ValorTipoCantidadAjustada;
                                _computoRepository.Update(Computo);
                            }


                        }
                        else {
                            if (Computo.cantidadAjustada && Computo.tipo != null) {
                                Computo.cantidadAjustada = false;
                                Computo.tipo = null;
                                _computoRepository.Update(Computo);
                            }
                        }
                        var DetalleAvanceObra = (from da in DetallesAvanceObra
                                                 where da.ComputoId == ComputoId
                                                 select da
                                                 ).FirstOrDefault();
                        if (DetalleAvanceObra != null && DetalleAvanceObra.Id > 0)// Existe Detalle Avance de Obra
                        {

                            if (CantidadEAC > 0)
                            {
                                if (CantidadAcumulada > 0 && CantidadAcumulada > DetalleAvanceObra.cantidad_acumulada)
                                {
                                    DetalleAvanceObra.cantidad_acumulada = CantidadAcumulada;
                                    DetalleAvanceObra.cantidad_acumulada_anterior = DetalleAvanceObra.cantidad_acumulada_anterior;
                                    DetalleAvanceObra.fecha_registro = AvanceObra.fecha_presentacion;
                                    DetalleAvanceObra.cantidad_diaria = CantidadAcumulada - DetalleAvanceObra.cantidad_acumulada_anterior;
                                    DetalleAvanceObra.precio_unitario = Computo.precio_unitario;
                                    DetalleAvanceObra.total = (CantidadAcumulada - DetalleAvanceObra.cantidad_acumulada_anterior) * Computo.precio_unitario;
                                    DetalleAvanceObra.vigente = true;
                                    var DetalleGuardado = Repository.Update(DetalleAvanceObra);

                                }
                                Computo.cantidad_eac = CantidadEAC;
                                _computoRepository.Update(Computo);
                            }
                            else
                            if (CantidadEAC == 0 && CantidadAnterior >= 0)
                            {
                                var CantidadAcumReal = 0;
                                DetalleAvanceObra.cantidad_acumulada = CantidadAcumReal;
                                DetalleAvanceObra.cantidad_acumulada_anterior = DetalleAvanceObra.cantidad_acumulada_anterior;
                                DetalleAvanceObra.fecha_registro = AvanceObra.fecha_presentacion;
                                DetalleAvanceObra.cantidad_diaria = CantidadAcumReal - DetalleAvanceObra.cantidad_acumulada_anterior;
                                DetalleAvanceObra.precio_unitario = Computo.precio_unitario;
                                DetalleAvanceObra.total = (CantidadAcumReal - DetalleAvanceObra.cantidad_acumulada_anterior) * Computo.precio_unitario;
                                DetalleAvanceObra.vigente = true;
                                var DetalleGuardado = Repository.Update(DetalleAvanceObra);

                                Computo.cantidad_eac = CantidadEAC;
                                _computoRepository.Update(Computo);
                            }

                        }
                        else//No Existe Detalle
                        {
                            if (CantidadEAC > 0)
                            {
                                if (CantidadAcumulada > 0)
                                {
                                    var cant_anterior = this.ObtenerCantidadAcumulada(ComputoId, AvanceObra.fecha_presentacion.Value, AvanceObra.OfertaId);
                                    var detalle = new DetalleAvanceObra()
                                    {
                                        AvanceObraId = AvanceObraId,
                                        ComputoId = ComputoId,
                                        cantidad_acumulada = CantidadAcumulada,
                                        cantidad_acumulada_anterior = cant_anterior,
                                        fecha_registro = AvanceObra.fecha_presentacion,
                                        cantidad_diaria = CantidadAcumulada - cant_anterior,
                                        precio_unitario = Computo.precio_unitario,
                                        total = (CantidadAcumulada - cant_anterior) * Computo.precio_unitario,
                                        vigente = true,
                                    };

                                    var DetalleGuardado = Repository.Insert(detalle);


                                }
                                Computo.cantidad_eac = CantidadEAC;
                                _computoRepository.Update(Computo);
                            }
                            else
                            if (CantidadEAC == 0 && CantidadAnterior > 0)
                            {
                                var linea = new ComputoAvanceObra()
                                {
                                    CantidadAcumulada = CantidadAcumulada,
                                    CantidadAnterior = CantidadAnterior,
                                    CantidadEAC = CantidadEAC,
                                    ComputoId = Computo.Id,

                                };
                                await this.GuardarDetallesNegativos(linea, AvanceObraId);

                            }

                        }

                    }

                }
            }
            return "OK";
        }


        public List<string> ValidarSubirExcelAvanceObra(HttpPostedFileBase UploadedFile, int AvanceObraId)
        {
            var AvanceObra = _avanceobraRepository.Get(AvanceObraId);

            var DetallesAvanceObra = Repository.GetAll()
                                             .Where(c => c.vigente)
                                             .Where(c => c.AvanceObra.vigente)
                                             .Where(c => c.AvanceObraId == AvanceObraId)
                                             .ToList();

            List<string> errores = new List<string>();


            string fileName = UploadedFile.FileName;
            string fileContentType = UploadedFile.ContentType;
            byte[] fileBytes = new byte[UploadedFile.ContentLength];
            var data = UploadedFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(UploadedFile.ContentLength));
            using (var package = new ExcelPackage(UploadedFile.InputStream))
            {

                //VARIABLES MANEJO DEL EXCEL
                var HojaExcel = package.Workbook.Worksheets;
                var Hoja = HojaExcel[1];
                var NumeroColumnas = Hoja.Dimension.End.Column;
                var NumeroDeFilas = Hoja.Dimension.End.Row;
                //PARAMETROS INICIALES
                int FilaInicial = 2;
                int ColumnaComputoId = 1;
                int ColumnaCantidadEAC = 5;
                int ColumnaCantidadAnterior = 6;
                int ColumnaCantidadAcumulada = 7;

                for (int fila = FilaInicial; fila <= NumeroDeFilas; fila++)
                {
                    //Recibe Texto del Excel
                    var ValorComputoId = (Hoja.Cells[fila, ColumnaComputoId].Value ?? "").ToString();
                    var ValorCantidadEAC = (Hoja.Cells[fila, ColumnaCantidadEAC].Value ?? "").ToString();
                    var ValorCantidadAnterior = (Hoja.Cells[fila, ColumnaCantidadAnterior].Value ?? "").ToString();
                    var ValorCantidadAcumulada = (Hoja.Cells[fila, ColumnaCantidadAcumulada].Value ?? "").ToString();

                    //Valores Iniciales Campos
                    int ComputoId = 0;
                    decimal CantidadEAC = 0;
                    decimal CantidadAnterior = 0;
                    decimal CantidadAcumulada = 0;

                    if (ValorComputoId.Length > 0)
                    {
                        ComputoId = Int32.Parse(ValorComputoId);
                    }
                    if (ValorCantidadEAC.Length > 0)
                    {
                        CantidadEAC = Decimal.Parse("0" + ValorCantidadEAC, NumberStyles.Float); //acepte 10 exp e-8, 0,000001
                    }
                    if (ValorCantidadAnterior.Length > 0)
                    {
                        CantidadAnterior = Decimal.Parse("0" + ValorCantidadAnterior, NumberStyles.Float); //acepte 10 exp e-8, 0,000001
                    }
                    if (ValorCantidadAcumulada.Length > 0)
                    {
                        CantidadAcumulada = Decimal.Parse("0" + ValorCantidadAcumulada, NumberStyles.Float); //acepte 10 exp e-8, 0,000001
                    }

                    //Verificamos CantidadesAcumuladas Mayores  al EAC
                    if (ComputoId > 0)
                    {
                        if (CantidadEAC > 0)
                        {
                            if (CantidadAcumulada > CantidadEAC)
                            {
                                errores.Add("Fila: " + fila + "El Valor Acumulado es mayor al EAC");
                            }
                        }
                    }

                }
            }
            return errores;
        }

        public decimal ObtenerCantidadAcumulada(int computoId, DateTime fecha_reporte, int ofertaId)
        {
            decimal cantidad_acumulada = 0;
            var query = Repository.GetAllIncluding(o => o.AvanceObra)
                .Where(o => o.vigente == true)
                .Where(o => o.ComputoId == computoId)
                .Where(o => o.AvanceObra.aprobado == true)
                .Where(o => o.AvanceObra.vigente)
                .Where(o => o.AvanceObra.fecha_presentacion < fecha_reporte)
                .Where(o => o.AvanceObra.OfertaId == ofertaId);

            var detalles = (from d in query
                            select new DetalleAvanceIngenieriaDto()
                            {
                                cantidad_horas = d.cantidad_diaria,
                            }).ToList();

            foreach (var d in detalles)
            {
                cantidad_acumulada += d.cantidad_horas;
            }
            return cantidad_acumulada;
        }

        public ExcelPackage CargaMasivaIDS(int proyectoId)
        {

            var proyecto = _proyectoRepository.Get(proyectoId);
            ExcelPackage excel = new ExcelPackage();
            string nombretab = "Carga Masiva IDS_" + proyecto.codigo;
            var hoja = excel.Workbook.Worksheets.Add(nombretab);

            var computos = Repository.GetAll()
                                .Where(c => c.vigente)
                                .Where(c => c.AvanceObra.vigente)
                                .Where(c => c.AvanceObra.aprobado)
                                .Where(c => c.AvanceObra.Oferta.vigente)
                                .Where(c => c.AvanceObra.Oferta.es_final)
                                .Where(c => c.AvanceObra.Oferta.ProyectoId == proyectoId)
                                .Where(c => c.Computo.vigente).Select(c => c.Computo).OrderBy(c => c.Item.codigo).ToList();
            hoja.Column(1).Hidden = true;
            hoja.Column(3).Width = 100;


            var items = (from com in computos select com.ItemId).Distinct().ToList();
            hoja.Cells["A1"].Value = proyecto.Id;
            int fila = 1;

            string celda = "C1";
            hoja.Cells[celda].Value = "PROYECTO: " + proyecto.codigo + " " + proyecto.nombre_proyecto;
            hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celda].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 117, 181));
            hoja.Cells[celda].Style.Font.Color.SetColor(Color.White);
            hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            fila++;
            celda = "A" + fila;

            hoja.Cells[celda].Value = "ID";
            hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celda].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 117, 181));
            hoja.Cells[celda].Style.Font.Color.SetColor(Color.White);
            hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            celda = "B" + fila;
            hoja.Cells[celda].Value = "CÓDIGO";
            hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celda].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 117, 181));
            hoja.Cells[celda].Style.Font.Color.SetColor(Color.White);
            hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            celda = "C" + fila;
            hoja.Cells[celda].Value = "ITEM";

            hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celda].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(47, 117, 181));
            hoja.Cells[celda].Style.Font.Color.SetColor(Color.White);
            hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            celda = "D" + fila;
            hoja.Cells[celda].Value = "ID RDO";
            hoja.Cells[celda].Style.Font.Bold = true;
            hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celda].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[celda].Style.Fill.BackgroundColor.SetColor(Color.OrangeRed);
            hoja.Cells[celda].Style.Font.Color.SetColor(Color.White);
            hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            fila++;
            foreach (var Id in items)
            {
                var e = _itemRepository.GetAll().Where(c => c.Id == Id).FirstOrDefault();
                if (e != null)
                {
                    celda = "A" + fila;
                    hoja.Cells[celda].Value = e.Id;
                    celda = "B" + fila;
                    hoja.Cells[celda].Value = e.codigo;
                    celda = "C" + fila;
                    hoja.Cells[celda].Value = e.nombre;
                    hoja.Cells[celda].Style.WrapText = true;

                    var computo = (from comp in computos where comp.ItemId == e.Id select comp).FirstOrDefault();
                    if (computo != null && computo.id_rubro_RDO != null)
                    {
                        celda = "D" + fila;
                        hoja.Cells[celda].Value = computo.id_rubro_RDO;
                    }

                }
                fila++;
            }

            return excel;

        }

        public async Task<string> CargarArchivoIDS(HttpPostedFileBase UploadedFile, int ofertaId)
        {
            /*var computos = Repository.GetAll()
                               .Where(c => c.vigente)
                               .Where(c => c.AvanceObra.vigente)
                               .Where(c => c.AvanceObra.aprobado)
                               .Where(c => c.AvanceObra.Oferta.vigente)
                               .Where(c => c.AvanceObra.Oferta.es_final)
                               .Where(c => c.AvanceObra.OfertaId == ofertaId)
                               .Where(c=>c.Computo.Item.GrupoId==2)
                               .Where(c => c.Computo.vigente).Select(c => c.Computo).ToList();*/


            var computos = _computoRepository.GetAllIncluding(c => c.Item).Where(c => c.Wbs.OfertaId == ofertaId)
                              .Where(c => c.vigente).ToList();
            string fileName = UploadedFile.FileName;
            string fileContentType = UploadedFile.ContentType;
            byte[] fileBytes = new byte[UploadedFile.ContentLength];
            var data = UploadedFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(UploadedFile.ContentLength));
            using (var package = new ExcelPackage(UploadedFile.InputStream))
            {

                //VARIABLES MANEJO DEL EXCEL
                var HojaExcel = package.Workbook.Worksheets;
                var Hoja = HojaExcel[1];
                var NumeroColumnas = Hoja.Dimension.End.Column;
                var NumeroDeFilas = Hoja.Dimension.End.Row;
                //PARAMETROS INICIALES
                int FilaInicial = 3;
                int ColumnaitemId = 1;
                int ColumnaValorIdRDO = 4;

                for (int fila = FilaInicial; fila <= NumeroDeFilas; fila++)
                {
                    //Recibe Texto del Excel
                    var ValorItemId = (Hoja.Cells[fila, ColumnaitemId].Value ?? "").ToString();
                    var ValorIdRDO = (Hoja.Cells[fila, ColumnaValorIdRDO].Value ?? "").ToString();
                    //Valores Iniciales Campos
                    int ItemId = 0;
                    if (ValorItemId.Length > 0)
                    {
                        ItemId = Int32.Parse(ValorItemId);
                    }
                    if (ItemId > 0) { 
                    var listado_computos = (from i in computos where i.Id == ItemId select i).ToList();
                    foreach (var ce in listado_computos)
                    {
                        var e = _computoRepository.Get(ce.Id);
                        if (e != null)
                        {
                            e.id_rubro_RDO = ValorIdRDO;
                            _computoRepository.Update(e);
                        }

                    }
                    }
                }
            }
            return "OK";
        }

    }
}


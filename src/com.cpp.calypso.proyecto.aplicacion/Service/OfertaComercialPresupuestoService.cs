using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class OfertaComercialPresupuestoAsyncBaseCrudAppService : AsyncBaseCrudAppService<OfertaComercialPresupuesto, OfertaComercialPresupuestoDto, PagedAndFilteredResultRequestDto>, IOfertaComercialPresupuestoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Presupuesto> _presupuestorepository;
        private readonly IBaseRepository<ComputoPresupuesto> _computopresupuestoRepository;
        private readonly IBaseRepository<WbsComercial> _wbscomercialRepository;
        private readonly IBaseRepository<ComputoComercial> _computocomercialRepository;
        private readonly IBaseRepository<OfertaComercial> _ofertacomercialRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<Item> _itemRepository;
        private readonly IBaseRepository<WbsPresupuesto> _wbspresupuestoRepository;
        private readonly IBaseRepository<Requerimiento> _requerimientoRepository;
        private readonly IBaseRepository<DetallePreciario> _detallepreciarioRepository;
        private readonly IBaseRepository<Preciario> _preciarioRepository;
        private readonly ItemServiceAsyncBaseCrudAppService _itemservice;
        private readonly IBaseRepository<DetalleAvanceObra> _detalleavancebrarepository;

        public OfertaComercialPresupuestoAsyncBaseCrudAppService(

            IBaseRepository<OfertaComercialPresupuesto> repository,
            IBaseRepository<Presupuesto> presupuestorepository,
            IBaseRepository<WbsPresupuesto> wbspresupuestoRepository,
            IBaseRepository<ComputoPresupuesto> computopresupuestoRepository,
             IBaseRepository<WbsComercial> wbscomercialRepository,
            IBaseRepository<ComputoComercial> computocomercialRepository,

            IBaseRepository<OfertaComercial> ofertacomercialRepository,
            IBaseRepository<Catalogo> catalogoRepository,
           IBaseRepository<Item> itemRepository,
                   IBaseRepository<Requerimiento> requerimientoRepository,
                   IBaseRepository<DetallePreciario> detallepreciarioRepository,
                    IBaseRepository<Preciario> preciarioRepository,
                    IBaseRepository<DetalleAvanceObra> detalleavancebrarepository
            ) : base(repository)
        {
            _presupuestorepository = presupuestorepository;
            _computopresupuestoRepository = computopresupuestoRepository;
            _wbscomercialRepository = wbscomercialRepository;
            _computocomercialRepository = computocomercialRepository;
            _ofertacomercialRepository = ofertacomercialRepository;
            _catalogoRepository = catalogoRepository;
            _itemRepository = itemRepository;
            _wbspresupuestoRepository = wbspresupuestoRepository;
            _requerimientoRepository = requerimientoRepository;
            _detalleavancebrarepository = detalleavancebrarepository;

            _itemservice = new ItemServiceAsyncBaseCrudAppService(itemRepository, detallepreciarioRepository,
               preciarioRepository, catalogoRepository, _detalleavancebrarepository, _computocomercialRepository,computopresupuestoRepository);
        }

        public string CrearOfertaComercialPresupuesto(OfertaComercialPresupuesto ofertapresupuesto) //Id Oferta Comercial
        {
            var requerimiento = _requerimientoRepository.Get(ofertapresupuesto.RequerimientoId);

            var presupuestodefinitivo = _presupuestorepository.GetAll()
                .Where(c => c.RequerimientoId == requerimiento.Id)
                .Where(c => c.es_final == true)
                .Where(c => c.vigente == true)
                .FirstOrDefault();

            if (presupuestodefinitivo != null && presupuestodefinitivo.Id > 0)
            {
                ofertapresupuesto.PresupuestoId = presupuestodefinitivo.Id;

                // consultar si ya exite en otra ofertacomercial

                var existe = Repository.GetAll()
                       .Where(c => c.PresupuestoId == presupuestodefinitivo.Id)
                       .Where(c => c.vigente)
                       .Where(c => c.OfertaComercialId == ofertapresupuesto.OfertaComercialId)
                      // .Where(c=>c.OfertaComercial.OfertaPadreId!=ofertapresupuesto.OfertaComercialId)
                       .FirstOrDefault();
                if (existe != null)
                {

                    return "El presupuesto: " + presupuestodefinitivo.descripcion + "" +
                            " ya esta siendo usado en la Oferta Comercial: " + existe.OfertaComercial.codigo;
                }
            }

            if (ofertapresupuesto.PresupuestoId == 0)
            {
                ofertapresupuesto.PresupuestoId = null;
            }
            var nuevoid = Repository.InsertAndGetId(ofertapresupuesto);



            /*  var o = _ofertacomercialRepository.Get(ofertapresupuesto.OfertaComercialId);
              o.estado_oferta = 5183;
              var resultado = _ofertacomercialRepository.Update(o);

              */

            return "creado";

            /*
            }
            else
            {

                ofertapresupuesto.PresupuestoId = null;

                return "no_tiene_presupuesto_definitivo";
            }
            */



        }

        public List<OfertaComercialPresupuesto> Listar(int Id) //id Oferta Comercial
        {
            var query = Repository.GetAllIncluding(c=>c.Requerimiento.Proyecto,c=>c.Presupuesto)
                .Where(c => c.vigente == true)
                .Where(c => c.OfertaComercialId == Id).
                ToList();
            return query;

        }


        public ExcelPackage GenerarExcelCabecera(int Id, int nivel_maximo)
        {

            var oferta = _ofertacomercialRepository.Get(Id);
            //Lista Wbs

            var relaciones = Repository.GetAll()
             .Where(p => p.OfertaComercialId == oferta.Id)
             .Where(p => p.vigente == true).ToList();

            List<List<WbsComercialDto>> LISTASWBS = new List<List<WbsComercialDto>>();
            if (relaciones.Count > 0)
            {
                foreach (var item in relaciones)
                {

                    var query = _wbscomercialRepository.GetAllIncluding(x => x.Catalogo)
               .Where(o => o.vigente == true)
               .Where(o => o.OfertaComercialId == oferta.Id)
               .Where(o => o.referencia_presupuesto == item.PresupuestoId)
               .Where(o => o.es_actividad == true).ToList();

                    var wbs = (from w in query
                               select new WbsComercialDto()
                               {
                                   Id = w.Id,
                                   OfertaComercialId = w.OfertaComercialId,
                                   fecha_inicial = w.fecha_inicial,
                                   fecha_final = w.fecha_final,
                                   id_nivel_padre_codigo = w.id_nivel_padre_codigo,
                                   id_nivel_codigo = w.id_nivel_codigo,
                                   nivel_nombre = w.nivel_nombre,
                                   observaciones = w.observaciones,
                                   DisciplinaId = w.DisciplinaId,
                                   Catalogo = w.Catalogo,
                                   referencia_presupuesto = w.referencia_presupuesto

                               }).ToList();


                    foreach (var w in wbs)
                    {
                        var name = _wbscomercialRepository
                            .GetAll()
                            .Where(o => o.vigente == true)
                            .Where(o => o.OfertaComercialId == w.OfertaComercialId)
                            .Where(o => o.referencia_presupuesto == w.referencia_presupuesto)
                            .Where(o => o.id_nivel_codigo == w.id_nivel_padre_codigo).FirstOrDefault();
                        if (name != null)
                        {
                            w.nombre_padre = name.nivel_nombre;
                        }
                    }

                    LISTASWBS.Add(wbs);

                }



            }



            var listacomputos = _computocomercialRepository.GetAllIncluding(x => x.WbsComercial.OfertaComercial, x => x.Item).Where(x => x.vigente == true).ToList();
            var computos = (from z in listacomputos
                            where z.WbsComercial.OfertaComercialId == oferta.Id && z.vigente == true
                            select new ComputoComercialDto
                            {
                                Id = z.Id,
                                ItemId = z.ItemId,
                                WbsComercialId = z.WbsComercialId,
                                item_codigo = z.Item.codigo,
                                item_nombre = z.Item.nombre,
                                cantidad = z.cantidad,
                                cantidad_eac = z.cantidad_eac,
                                costo_total = z.costo_total,
                                precio_unitario = z.precio_unitario,
                                Item = z.Item,
                                precio_incrementado = z.precio_incrementado,
                                precio_ajustado = z.precio_ajustado,
                                diferente = z.cantidad_eac != z.cantidad,
                                total_pu = (z.cantidad * z.precio_unitario),
                                total_pu_aui = (z.cantidad * z.precio_incrementado),

                            }).ToList();

            foreach (var e in computos)
            {
                var padreid = _itemRepository.GetAll()
                    .Where(y => y.vigente == true)
                    .Where(y => y.codigo == e.Item.item_padre)
                    .FirstOrDefault();

                if (padreid != null && padreid.Id > 0)
                {

                    e.item_padre_nombre = padreid.nombre;
                    e.item_padre_codigo = padreid.codigo;
                }
            }
            // var items = _itemservice.GetItemsporContratoActivo(oferta.Proyecto.contratoId, oferta.fecha_oferta.Value);
            //var items = _itemservice.ArbolWbsExcel(oferta.ContratoId, oferta.fecha_oferta.GetValueOrDefault());
            var items = _itemservice.ArbolItemsComputoComercial(oferta.Id);
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Carga Computo");

            workSheet.TabColor = System.Drawing.Color.Azure;
            /*workSheet.Protection.IsProtected = true;
              workSheet.Protection.SetPassword("pmdis");
              */
            workSheet.DefaultRowHeight = 15;

            //Header of table  
            //  

            workSheet.View.ZoomScale = 60;
            int row = nivel_maximo;
            for (int i = 1; i <= row; i++)
            {

                workSheet.Row(i).Style.Font.Bold = true;
            }

            int columna = 6;


            //IDS PARA ELIMINAR OCULTOS
            int wbsidocultuo = 0;
            int disciplinaoculto = 0;

            foreach (var rwbs in LISTASWBS)
            {
                foreach (var itemswbs in rwbs.OrderBy(l => l.id_nivel_codigo))
                {


                    int fila = nivel_maximo + 4;
                    List<WbsComercial> Jerarquia = new List<WbsComercial>();

                    WbsComercial item = _wbscomercialRepository.Get(itemswbs.Id);
                    Jerarquia.Add(item);
                    while (item.id_nivel_padre_codigo != ".")
                    {
                        //
                        item = _wbscomercialRepository.GetAll()
                            .Where(x => x.referencia_presupuesto == item.referencia_presupuesto)
                            .Where(X => X.id_nivel_codigo == item.id_nivel_padre_codigo)

                            .Where(C => C.vigente).FirstOrDefault();

                        Jerarquia.Add(item);
                    }
                    int a = Jerarquia.Count();
                    int rowtyle = nivel_maximo - 1;
                    foreach (var wbsj in Jerarquia)
                    {
                        if (rowtyle > 0)
                        {
                            workSheet.Cells[rowtyle, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[rowtyle, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);

                            workSheet.Cells[rowtyle, columna].Style.Border.Left.Style =
                            workSheet.Cells[rowtyle, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                            rowtyle--;
                        }
                        if (a > 0)
                        {
                            if (wbsj.es_actividad)
                            {
                                workSheet.Cells[fila - 4, columna].Value = wbsj.nivel_nombre;
                                workSheet.Cells[fila - 4, columna].Value = wbsj.nivel_nombre;
                                workSheet.Column(columna).Width = 25;
                                workSheet.Cells[fila - 4, columna].Style.WrapText = true;
                                workSheet.Cells[fila - 4, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                workSheet.Cells[fila - 4, columna].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                workSheet.Cells[fila - 4, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[fila - 4, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                                workSheet.Cells[fila - 4, columna].Style.Border.Top.Style = workSheet.Cells[fila - 4, columna].Style.Border.Left.Style = workSheet.Cells[fila - 4, columna].Style.Border.Right.Style = workSheet.Cells[fila - 4, columna].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                workSheet.Cells[fila - 2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[fila - 2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            }
                            else
                            {
                                workSheet.Cells[a, columna].Value = wbsj.nivel_nombre;

                                workSheet.Cells[a, columna].Style.WrapText = true;
                                workSheet.Cells[a, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                workSheet.Cells[a, columna].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                workSheet.Cells[a, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                workSheet.Cells[a, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                                workSheet.Cells[a, columna].Style.Border.Left.Style =
                                workSheet.Cells[a, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                                /*var valornuevo = workSheet.Cells[a, columna].Text;
                                var valorantiguo = workSheet.Cells[a, (columna - 1)].Text;
                                if (valornuevo == valorantiguo)
                                {
                                    string rango1 = workSheet.Cells[a, columna - 1].Address;

                                    string rango2 = workSheet.Cells[a, columna].Address;
                                    string range = rango1 + ":" + rango2;

                                    workSheet.Select(range);
                                    workSheet.SelectedRange.Merge = true;
                                    workSheet.SelectedRange.Value = valornuevo;

                                }
                                */

                            }
                        }

                        a--;
                    }
                    workSheet.Row(fila - 3).Hidden = true;
                    workSheet.Cells[fila - 3, columna].Value = itemswbs.Id;
                    workSheet.Cells[fila - 3, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[fila - 3, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                    workSheet.Cells[fila - 3, columna].Style.Border.Left.Style =
                           workSheet.Cells[fila - 3, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                    wbsidocultuo = (fila - 3);
                    disciplinaoculto = (fila - 2);
                    if (itemswbs.DisciplinaId >= 0)
                    {
                        workSheet.Cells[fila - 2, columna].Value = itemswbs.Catalogo.nombre;
                        workSheet.Cells[fila - 2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[fila - 2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                        workSheet.Cells[fila - 2, columna].Style.Border.Left.Style =
                           workSheet.Cells[fila - 2, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        workSheet.Cells[fila - 2, columna].Style.Border.Top.Style = workSheet.Cells[fila - 2, columna].Style.Border.Left.Style = workSheet.Cells[fila - 2, columna].Style.Border.Right.Style = workSheet.Cells[fila - 2, columna].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;


                    }
                    else
                    {
                        workSheet.Row(fila - 2).Hidden = true;

                    }

                    columna = columna + 1;
                }
            }



            workSheet.Cells[nivel_maximo, 1].Value = "Grupo";
            workSheet.Cells[nivel_maximo, 2].Value = "Id";
            workSheet.Cells[nivel_maximo, 3].Value = "ITEM";
            workSheet.Cells[nivel_maximo, 4].Value = "DESCRIPCIÓN";
            workSheet.Cells[nivel_maximo, 5].Value = "UNIDAD";

            string rango = workSheet.Cells[nivel_maximo, 2].Address + ":" + workSheet.Cells[nivel_maximo, 5].Address;
            workSheet.Cells[rango].AutoFilter = true;
            for (int i = 2; i <= 5; i++)
            {
                workSheet.Cells[nivel_maximo, i].Style.WrapText = true;
                workSheet.Cells[nivel_maximo, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[nivel_maximo, i].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[nivel_maximo + 2, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[nivel_maximo + 2, i].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[nivel_maximo, i].Style.Border.Top.Style = workSheet.Cells[nivel_maximo, i].Style.Border.Left.Style = workSheet.Cells[nivel_maximo, i].Style.Border.Right.Style = workSheet.Cells[nivel_maximo, i].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                workSheet.Cells[nivel_maximo, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[nivel_maximo, i].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            workSheet.Column(3).Width = 15;
            workSheet.Column(4).Width = 80;
            workSheet.Column(5).Width = 15;
            workSheet.Column(3).Style.WrapText = true;
            workSheet.Column(4).Style.WrapText = true;
            workSheet.Column(5).Style.WrapText = true;
            workSheet.Column(1).Style.Font.Bold = true;
            workSheet.Column(1).Hidden = true;
            workSheet.Column(2).Hidden = true;

            workSheet.Row(nivel_maximo).Height = 15;


            //int inicio de las filas
            //int c = 5;
            int c = nivel_maximo + 3;
            workSheet.View.FreezePanes(6, 1);
            workSheet.View.FreezePanes(c, 6);
            foreach (var pitem in items)
            {

                workSheet.Cells[c, 1].Value = pitem.GrupoId;

                workSheet.Cells[c, 2].Value = pitem.Id;
                workSheet.Cells[c, 3].Value = pitem.codigo;
                workSheet.Cells[c, 4].Value = pitem.nombre;
                workSheet.Cells[c, 4].Style.WrapText = true;
                if (pitem.UnidadId != 0)
                {
                    workSheet.Cells[c, 5].Value = this.nombrecatalogo2(pitem.UnidadId);

                }

                if (pitem.item_padre == ".")
                {
                    int padre = workSheet.Dimension.End.Column;
                    while (padre > 0)
                    {
                        workSheet.Cells[c, padre].Style.Font.Bold = true;
                        workSheet.Cells[c, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[c, padre].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        workSheet.Cells[c, padre].Style.Border.Top.Style = workSheet.Cells[c, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        padre--;

                    }

                }
                if (pitem.item_padre != "." && pitem.para_oferta == false)
                {
                    int padre = workSheet.Dimension.End.Column;

                    while (padre > 0)
                    {
                        workSheet.Cells[c, padre].Style.Font.Bold = true;
                        workSheet.Cells[c, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[c, padre].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                        workSheet.Cells[c, padre].Style.Border.Top.Style = workSheet.Cells[c, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        padre--;

                    }

                }

                workSheet.Cells[c, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                workSheet.Cells[c, 2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                workSheet.Cells[c, workSheet.Dimension.End.Column].Style.Border.Right.Style = ExcelBorderStyle.Medium;




                workSheet.Row(c).Style.Locked = false;
                c = c + 1;
            }


            //EMPIEZA PARTE DE LA DERECHA CALCULOS Y TODO

            var noOfCol = workSheet.Dimension.End.Column;
            var noOfRow = workSheet.Dimension.End.Row;

            workSheet.Cells[nivel_maximo, noOfCol + 1].Value = "CANTIDAD ESTIMADA";
            workSheet.Cells[nivel_maximo, noOfCol + 2].Value = "PRECIO UNITARIO";
            workSheet.Cells[nivel_maximo, noOfCol + 3].Value = "COSTO TOTAL ESTIMADO";




            // Estilos a la Parte Derecha de Calculos
            for (int a = noOfCol; a <= noOfCol + 3; a++)
            {
                workSheet.Column(a).Width = 25;
                workSheet.Cells[nivel_maximo, a].Style.WrapText = true;
                workSheet.Cells[nivel_maximo, a].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[nivel_maximo, a].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[nivel_maximo, a].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[nivel_maximo, a].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[nivel_maximo, a].Style.Border.Top.Style =
                workSheet.Cells[nivel_maximo, a].Style.Border.Left.Style =
                workSheet.Cells[nivel_maximo, a].Style.Border.Right.Style =
                workSheet.Cells[nivel_maximo, a].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            }
            for (int j = 6; j <= noOfCol; j++)
            {

                //Valores Parte Derecha





                var wbsid = (workSheet.Cells[nivel_maximo + 1, j].Value ?? "").ToString();
                for (int i = nivel_maximo + 3; i <= noOfRow; i++)
                {
                    var itemid = (workSheet.Cells[i, 2].Value ?? "").ToString();

                    foreach (var itemss in computos)
                    {

                        if (Convert.ToString(itemss.WbsComercialId) == wbsid && Convert.ToString(itemss.ItemId) == itemid)
                        {

                            //


                            workSheet.Cells[i, j].Value = itemss.cantidad;
                            workSheet.Cells[i, noOfCol + 2].Value = itemss.precio_unitario;
                            workSheet.Cells[i, j].Style.Numberformat.Format = "#,##0.00";
                            workSheet.Cells[i, noOfCol + 1].Style.Numberformat.Format = "#,##0.00";
                            workSheet.Cells[i, noOfCol + 2].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                            workSheet.Cells[i, noOfCol + 3].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


                        }

                    }


                }




            }

            int ncol = workSheet.Dimension.End.Column;

            for (int i = nivel_maximo + 3; i <= noOfRow; i++)
            {
                var rango_incio = workSheet.Cells[i, 6].Address;
                var rango_final = workSheet.Cells[i, ncol - 3].Address;
                var rangosumar = "$" + rango_incio + ":" + "$" + rango_final;
                workSheet.Cells[i, ncol - 2].Formula = "=SUM(" + rangosumar + ")";

                var cantidad = workSheet.Cells[i, ncol - 2].Value;
                var precio = workSheet.Cells[i, ncol - 2].Value;

                var dcantidad = workSheet.Cells[i, ncol - 2].Address;
                var dprecio = workSheet.Cells[i, ncol - 1].Address;
                workSheet.Cells[i, ncol].FormulaR1C1 = "=ROUND(" + dcantidad + "*" + dprecio + ", 2)";

            }

            int coln = workSheet.Dimension.End.Column;
            while (coln > 0)
            {
                workSheet.Cells[workSheet.Dimension.End.Row, coln].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                coln--;

            }

            // rango total
            string costoi = workSheet.Cells[nivel_maximo + 3, ncol].Address;
            string costoif = workSheet.Cells[noOfRow, ncol].Address;
            string rangovalortotal = "$" + costoi + ":$" + costoif;


            int bfila = noOfRow;
            bfila = bfila + 2;
            //CALCULOS BASE
            //Ingenieria
            workSheet.Cells[bfila, 4].Value = "Sub - total Ingeniería";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =
            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;


            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;


            //suma ingenieria

            string dinicio = workSheet.Cells[nivel_maximo + 3, 1].Address;
            string dfinal = workSheet.Cells[noOfRow, 1].Address;
            string rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            string grupo = "" + 1 + "";

            var formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string sumaingenieria = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;


            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            bfila++;
            //Procura
            workSheet.Cells[bfila, 4].Value = "Sub-total Procura";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
               workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;


            //suma procura

            dinicio = workSheet.Cells[nivel_maximo + 3, 1].Address;
            dfinal = workSheet.Cells[noOfRow, 1].Address;
            rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            grupo = "" + 3 + "";

            formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string suma_procura = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //CALCULOS BASE
            //Reembolsables
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Sub-total Reembolsables";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, noOfCol + 3].Value = 0;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string sumareembolsables = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;

            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //Consturccion
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Sub-total Construcción";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
             workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            dinicio = workSheet.Cells[nivel_maximo + 3, 1].Address;
            dfinal = workSheet.Cells[noOfRow, 1].Address;
            rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            grupo = "" + 2 + "";

            formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string suma_contruccion = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //Descuento
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Descuento por <Descripción del concepto>";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
             workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;


            workSheet.Cells[bfila, noOfCol + 3].Value = 0;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            //Descuento
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administración";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
                        workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            string administracion = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;



            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administracion sobre Obra (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.4119;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";

            //contruccion

            string formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_contruccion;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string valoradministracion_obra = workSheet.Cells[bfila, noOfCol + 3].Address;

            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Imprevistos sobre Obra (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.03;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";

            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_contruccion;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string valorimprevistos_obra = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;
            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Utilidad sobre Obra (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.12;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";


            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_contruccion;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;



            string valor_utilidadObra = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;
            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administracion sobre Procura Contratista (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.1;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";




            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_procura;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string valor_procura = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administracion sobre Reembolsables (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.1;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";


            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + sumareembolsables;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string valor_reembolsables = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;
            //envio de datos a administracion

            formula_calculo = "=SUM(" + valoradministracion_obra + ":" + valor_reembolsables + ")";
            workSheet.Cells[administracion].Formula = formula_calculo;
            workSheet.Cells[administracion].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[administracion].Style.WrapText = true;


            bfila++;
            workSheet.Cells[bfila, 4].Value = "TOTAL";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            //TOTAL

            formula_calculo = "=SUM(" + sumaingenieria + "+" + suma_procura + "+" + suma_contruccion + "+" + sumareembolsables + "+" + administracion + ")";
            workSheet.Cells[bfila, noOfCol + 3].Formula = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;
            string valortotal = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            workSheet.InsertRow(1, 3);


            workSheet.Cells["D1"].Value = "PROYECTO BLOQUE X";
            workSheet.Cells["D1"].Style.WrapText = true;
            workSheet.Cells["D1"].Style.Font.Bold = true;

            workSheet.Cells["D2"].Value = "Anexo Propuesta Económica";
            workSheet.Cells["D2"].Style.WrapText = true;
            workSheet.Cells["D2"].Style.Font.Bold = true;

            workSheet.Cells["D3"].Value = oferta.descripcion;
            workSheet.Cells["D3"].Style.WrapText = true;
            workSheet.Cells["D3"].Style.Font.Bold = true;
            return excel;
        }

        public string nombrecatalogo2(int tipocatagoid)
        {
            var a = _catalogoRepository.Get(tipocatagoid);
            if (a != null && a.Id > 0)
            {
                return a.nombre;
            }
            else
            {
                return "";
            }
        }

        public List<ComputoComercialDto> GetComputosPorOfertaComercial(int id)
        {
            var listacomputos = _computocomercialRepository.GetAllIncluding(c => c.WbsComercial.OfertaComercial, c => c.Item).Where(c => c.vigente == true).ToList();
            var computos = (from c in listacomputos
                            where c.WbsComercial.OfertaComercialId == id && c.vigente == true
                            select new ComputoComercialDto
                            {
                                Id = c.Id,
                                ItemId = c.ItemId,
                                WbsComercialId = c.WbsComercialId,
                                actividad_nombre = c.WbsComercial.nivel_nombre,
                                oferta = c.WbsComercial.OfertaComercial.codigo,
                                item_codigo = c.Item.codigo,
                                item_nombre = c.Item.nombre,
                                cantidad = c.cantidad,
                                cantidad_eac = c.cantidad_eac,
                                costo_total = c.costo_total,
                                precio_unitario = c.precio_unitario,
                                Item = c.Item,
                                precio_incrementado = c.precio_incrementado,
                                precio_ajustado = c.precio_ajustado,
                                diferente = c.cantidad_eac != c.cantidad,
                                total_pu = (c.cantidad * c.precio_unitario),
                                total_pu_aui = (c.cantidad * c.precio_incrementado)

                            }).ToList();

            foreach (var e in computos)
            {

                var padreid = _itemRepository.GetAll().Where(c => c.vigente == true).Where(c => c.item_padre == e.Item.item_padre)
              .FirstOrDefault();
                if (padreid.Id != 0)
                {
                    var I = _itemRepository.Get(padreid.Id);
                    e.item_padre_nombre = I.nombre;
                    e.item_padre_codigo = I.codigo;
                }
            }

            return computos;
        }

        public List<WbsComercialDto> ListaWbs(int Id)
        {
            var query = _wbscomercialRepository.GetAllIncluding(c => c.Catalogo)
                .Where(o => o.vigente == true)
                .Where(o => o.OfertaComercialId == Id)
                .Where(o => o.es_actividad == true)
                .OrderBy(o => o.id_nivel_codigo);

            var items = (from w in query
                         select new WbsComercialDto()
                         {
                             Id = w.Id,
                             OfertaComercialId = w.OfertaComercialId,
                             fecha_inicial = w.fecha_inicial,
                             fecha_final = w.fecha_final,
                             id_nivel_padre_codigo = w.id_nivel_padre_codigo,
                             id_nivel_codigo = w.id_nivel_codigo,
                             nivel_nombre = w.nivel_nombre,
                             observaciones = w.observaciones,
                             DisciplinaId = w.DisciplinaId,
                             Catalogo = w.Catalogo

                         }).ToList();

            try
            {
                foreach (var w in items)
                {
                    var name = _wbscomercialRepository
                        .GetAll()
                        .Where(o => o.vigente == true)
                        .Where(o => o.OfertaComercialId == w.OfertaComercialId).SingleOrDefault(o => o.id_nivel_codigo == w.id_nivel_padre_codigo);
                    if (name != null)
                    {
                        if (name.Id > 0)
                        {
                            w.nombre_padre = name.nivel_nombre;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var msmg = e.Message;
            }

            return items;

        }

        public decimal MontoPresupuestoIngenieria(int OfertaId)
        {
            decimal montoingenieria = 0;
            var listacomputos = _computocomercialRepository.GetAllIncluding(c => c.WbsComercial.OfertaComercial, c => c.Item)
                .Where(c => c.vigente == true).Where(c => c.WbsComercial.OfertaComercialId == OfertaId).Where(c => c.Item.GrupoId == 1)
                .ToList();

            if (listacomputos != null && listacomputos.Count >= 0)
            {
                montoingenieria = (from x in listacomputos select x.costo_total).Sum(); //Presupuesto Total

            }

            return montoingenieria;
        }

        public decimal MontoPresupuestoConstruccion(int OfertaId)
        {

            decimal montoingenieria = 0;
            var listacomputos = _computocomercialRepository.GetAllIncluding(c => c.WbsComercial.OfertaComercial, c => c.Item)
                .Where(c => c.vigente == true).Where(c => c.WbsComercial.OfertaComercialId == OfertaId).Where(c => c.Item.GrupoId == 2)
                .ToList();

            if (listacomputos != null && listacomputos.Count >= 0)
            {
                montoingenieria = (from x in listacomputos select x.costo_total).Sum(); //Presupuesto Total

            }

            return montoingenieria;
        }

        public decimal MontoPresupuestoProcura(int OfertaId)
        {
            decimal montoingenieria = 0;
            var listacomputos = _computocomercialRepository.GetAllIncluding(c => c.WbsComercial.OfertaComercial, c => c.Item)
                .Where(c => c.vigente == true).Where(c => c.WbsComercial.OfertaComercialId == OfertaId).Where(c => c.Item.GrupoId == 3)
                .ToList();

            if (listacomputos != null && listacomputos.Count >= 0)
            {
                montoingenieria = (from x in listacomputos select x.costo_total).Sum(); //Presupuesto Total

            }

            return montoingenieria;
        }
        public decimal sumacantidades(int OfertaId, int ItemId)
        {

            var listacomputos = _computocomercialRepository.GetAllIncluding(c => c.WbsComercial.OfertaComercial, c => c.Item)
                .Where(c => c.vigente == true).Where(c => c.WbsComercial.OfertaComercialId == OfertaId).Where(c => c.ItemId == ItemId);
            var items = listacomputos.Select(c => c.cantidad).Sum();
            return items;
        }


        public WbsComercialDto ObtenerPadre(string id_nivel_padre_codigo)
        {
            var items = _wbscomercialRepository.GetAll();

            var ItemPadre = (from c in items
                             where c.vigente == true
                             where c.id_nivel_codigo == id_nivel_padre_codigo
                             select new WbsComercialDto
                             {
                                 Id = c.Id,
                                 fecha_inicial = c.fecha_inicial,
                                 id_nivel_codigo = c.id_nivel_codigo,
                                 id_nivel_padre_codigo = c.id_nivel_padre_codigo,
                                 nivel_nombre = c.nivel_nombre,
                                 es_actividad = c.es_actividad,
                             }).FirstOrDefault();
            if (ItemPadre != null)
            {
                return ItemPadre;

            }
            else
            {
                return null;
            }
        }

        public List<WbsComercialDto> Jerarquiawbs(int id)
        {
            List<WbsComercialDto> Jerarquia = new List<WbsComercialDto>();

            WbsComercialDto item = this.DatosWbs(id);
            Jerarquia.Add(item);
            while (item.id_nivel_padre_codigo != ".")
            {

                item = this.ObtenerPadre(item.id_nivel_padre_codigo);
                Jerarquia.Add(item);
            }

            return Jerarquia.OrderBy(c => c.id_nivel_codigo).ToList();
        }

        public WbsComercialDto DatosWbs(int id)
        {
            var items = _wbscomercialRepository.GetAll();

            var ItemPadre = (from c in items
                             where c.vigente == true
                             where c.Id == id
                             select new WbsComercialDto
                             {
                                 Id = c.Id,
                                 fecha_inicial = c.fecha_inicial,
                                 id_nivel_codigo = c.id_nivel_codigo,
                                 id_nivel_padre_codigo = c.id_nivel_padre_codigo,
                                 nivel_nombre = c.nivel_nombre,
                                 es_actividad = c.es_actividad,
                             }).FirstOrDefault();
            if (ItemPadre != null)
            {
                return ItemPadre;

            }
            else
            {
                return null;
            }
        }

        public ExcelPackage GenerarExcelCargaFechas(int id)
        {

            var oferta = _ofertacomercialRepository.Get(id);
            //Lista Wbs

            var relaciones = Repository.GetAll()
             .Where(p => p.OfertaComercialId == oferta.Id)
             .Where(p => p.vigente == true).ToList();

            List<List<WbsComercialDto>> LISTASWBS = new List<List<WbsComercialDto>>();
            if (relaciones.Count > 0)
            {
                foreach (var item in relaciones)
                {

                    var query = _wbscomercialRepository.GetAllIncluding(x => x.Catalogo)
               .Where(o => o.vigente == true)
               .Where(o => o.OfertaComercialId == oferta.Id)
               .Where(o => o.referencia_presupuesto == item.PresupuestoId)
               .ToList();

                    var wbs = (from w in query
                               select new WbsComercialDto()
                               {
                                   Id = w.Id,
                                   OfertaComercialId = w.OfertaComercialId,
                                   fecha_inicial = w.fecha_inicial,
                                   fecha_final = w.fecha_final,
                                   id_nivel_padre_codigo = w.id_nivel_padre_codigo,
                                   id_nivel_codigo = w.id_nivel_codigo,
                                   nivel_nombre = w.nivel_nombre,
                                   observaciones = w.observaciones,
                                   DisciplinaId = w.DisciplinaId,
                                   Catalogo = w.Catalogo,
                                   referencia_presupuesto = w.referencia_presupuesto,
                                   es_actividad = w.es_actividad,
                                   vigente = w.vigente,


                               }).ToList();


                    foreach (var w in wbs)
                    {
                        var name = _wbscomercialRepository
                            .GetAll()
                            .Where(o => o.vigente == true)
                            .Where(o => o.OfertaComercialId == w.OfertaComercialId)
                            .Where(o => o.referencia_presupuesto == w.referencia_presupuesto)
                            .SingleOrDefault(o => o.id_nivel_codigo == w.id_nivel_padre_codigo);
                        if (name != null)
                        {
                            w.nombre_padre = name.nivel_nombre;
                        }
                    }

                    LISTASWBS.Add(wbs);

                }



            }
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Croonograma");
            workSheet.Cells[1, 1].Value = "Id";

            workSheet.Cells[1, 2].Value = "CÓDIGO";
            workSheet.Cells[1, 2].Style.WrapText = true;
            workSheet.Cells[1, 2].Style.Font.Bold = true;
            workSheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[1, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
            workSheet.Cells[1, 2].Style.Font.Color.SetColor(Color.White);


            workSheet.Cells[1, 3].Value = "DESCRIPCIÓN";
            workSheet.Cells[1, 3].Style.WrapText = true;
            workSheet.Cells[1, 3].Style.Font.Bold = true;
            workSheet.Cells[1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[1, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 3].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
            workSheet.Cells[1, 3].Style.Font.Color.SetColor(Color.White);

            workSheet.Cells[1, 4].Value = "FECHA INICIAL";

            workSheet.Cells[1, 4].Style.WrapText = true;
            workSheet.Cells[1, 4].Style.Font.Bold = true;
            workSheet.Cells[1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 4].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
            workSheet.Cells[1, 4].Style.Font.Color.SetColor(Color.White);

            workSheet.Cells[1, 5].Value = "FECHA FINAL";
            workSheet.Cells[1, 5].Style.WrapText = true;
            workSheet.Cells[1, 5].Style.Font.Bold = true;
            workSheet.Cells[1, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[1, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 5].Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue);
            workSheet.Cells[1, 5].Style.Font.Color.SetColor(Color.White);


            //workSheet.Cells["B4:D4"].AutoFilter = true;

            workSheet.Column(1).Width = 20;
            workSheet.Column(2).Width = 20;
            workSheet.Column(3).Width = 50;
            workSheet.Column(4).Width = 20;
            workSheet.Column(5).Width = 20;
            workSheet.Column(1).Style.Font.Bold = true;
            workSheet.Column(1).Hidden = true;






            int row = 2;
            foreach (var wbslista in LISTASWBS)
            {
                foreach (var itemswbs in wbslista.OrderBy(c => c.id_nivel_codigo))
                {

                    workSheet.Cells[row, 1].Value = itemswbs.Id;
                    workSheet.Cells[row, 2].Value = itemswbs.id_nivel_codigo;
                    workSheet.Cells[row, 3].Value = itemswbs.nivel_nombre;
                    workSheet.Cells[row, 4].Style.Numberformat.Format = "dd/mm/yyyy";
                    workSheet.Cells[row, 5].Style.Numberformat.Format = "dd/mm/yyyy";

                    //COLORES
                    if (itemswbs.es_actividad)
                    {

                        workSheet.Cells[row, 2].Style.Font.Bold = true;
                        workSheet.Cells[row, 2].Style.Fill.PatternType = ExcelFillStyle.LightGrid;
                        workSheet.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);

                        workSheet.Cells[row, 3].Style.Font.Bold = true;
                        workSheet.Cells[row, 3].Style.Fill.PatternType = ExcelFillStyle.LightGrid;
                        workSheet.Cells[row, 3].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);


                        workSheet.Cells[row, 4].Style.Font.Bold = true;
                        workSheet.Cells[row, 4].Style.Fill.PatternType = ExcelFillStyle.LightGrid;
                        workSheet.Cells[row, 4].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);


                        workSheet.Cells[row, 5].Style.Font.Bold = true;
                        workSheet.Cells[row, 5].Style.Fill.PatternType = ExcelFillStyle.LightGrid;
                        workSheet.Cells[row, 5].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);


                        //DATOS

                        if (itemswbs.fecha_inicial != null)
                        {


                            workSheet.Cells[row, 4].Value = itemswbs.fecha_inicial;
                            workSheet.Cells[row, 4].Style.Numberformat.Format = "dd/mm/yyyy";
                        }
                        if (itemswbs.fecha_final != null)
                        {
                            workSheet.Cells[row, 5].Value = itemswbs.fecha_final.Value;
                            workSheet.Cells[row, 5].Style.Numberformat.Format = "dd/mm/yyyy";
                        }


                    }
                    row++;

                }

            }



            return excel;
        }

        public List<WbsComercialDto> ArbolWbsExcel(int OfertaId)
        {
            List<WbsComercialDto> all = new List<WbsComercialDto>();
            var wbs = this.ListaWbs(OfertaId);
            foreach (var item in wbs)
            {

                var x = this.Jerarquiawbs(item.Id);
                foreach (var item2 in x)
                {
                    all.Add(item2);
                }
                all.Add(item);

            }
            return all.GroupBy(p => p.Id).Select(g => g.FirstOrDefault()).OrderBy(c => c.id_nivel_codigo).ToList();
        }

        public int ActualizarDatos(OfertaComercialPresupuesto ofertapresupuesto)
        {
            int presupuesto_enviado = -1;

            var relaciones = Repository.GetAll()
                .Where(p => p.OfertaComercialId == ofertapresupuesto.OfertaComercialId)
                .Where(p => p.vigente == true).ToList();

            if (relaciones.Count > 0)
            {
                foreach (var borado in relaciones)
                {
                    var computos = _computocomercialRepository.GetAll()
                         .Where(x => x.WbsComercial.OfertaComercialId == borado.OfertaComercialId)
                         .Where(x => x.vigente)
                         .ToList();

                    if (computos.Count > 0)
                    {
                        _computocomercialRepository.Delete(computos);
                    }
                    var wbs = _wbscomercialRepository.GetAll()
                         .Where(x => x.OfertaComercialId == borado.OfertaComercialId)
                         .Where(x => x.vigente)
                         .ToList();

                    if (wbs.Count > 0)
                    {
                        _wbscomercialRepository.Delete(wbs);
                        /*
                        foreach (var item in wbs)
                        {
                            _wbscomercialRepository.Delete(item);
                        } */
                    }

                }

                foreach (var item in relaciones)
                {


                    if (item.PresupuestoId != null && item.PresupuestoId > 0)
                    {
                        //llenar tablas wbs comercial y computos comercial

                        var wbs = _wbspresupuestoRepository.GetAll()
                           .Where(c => c.PresupuestoId == item.PresupuestoId)
                           .Where(c => c.vigente == true)
                           .ToList();

                        if (wbs != null && wbs.Count > 0)
                        {
                            foreach (var wbsp in wbs)
                            {
                                var computos = new List<ComputoPresupuesto>();
                                if (wbsp.es_actividad)
                                {
                                    computos = _computopresupuestoRepository.GetAll()
                                        .Where(c => c.vigente == true)
                                        .Where(c => c.WbsPresupuestoId == wbsp.Id)
                                        .ToList();
                                }
                                WbsComercial a = new WbsComercial
                                {
                                    Id = 0,
                                    id_nivel_codigo = wbsp.id_nivel_codigo,
                                    id_nivel_padre_codigo = wbsp.id_nivel_padre_codigo,
                                    nivel_nombre = wbsp.nivel_nombre,
                                    observaciones = wbsp.observaciones,
                                    OfertaComercialId = ofertapresupuesto.OfertaComercialId,
                                    DisciplinaId = wbsp.DisciplinaId,
                                    es_actividad = wbsp.es_actividad,
                                    estado = wbsp.estado,
                                    fecha_final = wbsp.fecha_final,
                                    fecha_inicial = wbsp.fecha_inicial,
                                    vigente = true,
                                    revision = "",
                                    referencia_presupuesto = item.PresupuestoId.Value
                                };


                                var WbsId = _wbscomercialRepository.InsertAndGetId(a);

                                if (WbsId > 0)
                                {
                                    if (computos != null && computos.Count > 0)
                                    {
                                        foreach (var c in computos)
                                        {
                                            ComputoComercial computoc = new ComputoComercial
                                            {
                                                Id = 0,
                                                WbsComercialId = WbsId,
                                                ItemId = c.ItemId,
                                                precio_ajustado = c.precio_ajustado,
                                                fecha_actualizacion = c.fecha_actualizacion,
                                                vigente = c.vigente,
                                                cantidad = c.cantidad,
                                                cantidad_eac = c.cantidad_eac,
                                                codigo_item_alterno = c.codigo_item_alterno,
                                                codigo_primavera = c.codigo_primavera,
                                                costo_total = c.costo_total,
                                                estado = c.estado,
                                                fecha_eac = c.fecha_eac,
                                                fecha_registro = c.fecha_registro,
                                                precio_unitario = c.precio_unitario,
                                                precio_base = c.precio_base,
                                                precio_aplicarse = c.precio_aplicarse,
                                                precio_incrementado = c.precio_incrementado,
                                                presupuestado = c.presupuestado,

                                            };


                                            var ComputoId = _computocomercialRepository.Insert(computoc);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }


            return ofertapresupuesto.OfertaComercialId > 0 ? ofertapresupuesto.OfertaComercialId : 0;

        }

        public int Eliminar(int Id)
        {
            var relacion = Repository.Get(Id);
            relacion.vigente = false;
            var resultado = Repository.Update(relacion);
            return relacion.OfertaComercialId;
        }

        public bool CambiarEmitidosRequerimientosOferta(int Id)
        {
            var lista = Repository.GetAll().Where(c => c.OfertaComercialId == Id)
                .Where(c => c.vigente == true).
                ToList();

            foreach (var item in lista)
            {

                var requerimiento = _requerimientoRepository.Get(item.RequerimientoId);
                requerimiento.estado_presupuesto = 4181;
                var resultado = _requerimientoRepository.Update(requerimiento);
            }

            return true;

        }

        public int nivel_mas_alto(int Id, int presupuestoid)
        {
            var lista = _wbscomercialRepository.GetAllIncluding(x => x.Catalogo)
                .Where(o => o.vigente == true)
                .Where(o => o.OfertaComercialId == Id)
                .Where(o => o.es_actividad == true)
                .Where(o => o.referencia_presupuesto == presupuestoid)
                .ToList();
            int mayor = 0;


            foreach (var item in lista)
            {
                int contnivel = this.contarnivel(item.Id, item.referencia_presupuesto);
                if (contnivel >= mayor)
                {
                    mayor = contnivel;
                }


            }
            return mayor;
        }

        public int contarnivel(int id, int presupuestoid)
        {
            List<WbsComercial> Jerarquia = new List<WbsComercial>();

            WbsComercial item = _wbscomercialRepository.Get(id);
            Jerarquia.Add(item);
            while (item.id_nivel_padre_codigo != ".")
            {

                item = _wbscomercialRepository.GetAll()
                    .Where(c => c.id_nivel_codigo == item.id_nivel_padre_codigo)
                    .Where(C => C.vigente)
                    .Where(c => c.referencia_presupuesto == presupuestoid)
                    .FirstOrDefault();

                Jerarquia.Add(item);
            }
            return Jerarquia.Count();
        }

        public int mas_alto_multiple(int id)
        {

            var relaciones = Repository.GetAll()
                .Where(p => p.OfertaComercialId == id)
                .Where(p => p.vigente == true).ToList();

            int masalto = 0;
            foreach (var item in relaciones)
            {
                if (item.PresupuestoId != null)
                {
                    int numero = this.nivel_mas_alto(id, item.PresupuestoId.Value);

                    if (numero > masalto)
                    {
                        masalto = numero;
                    }
                }
            }

            return masalto;

        }



        public List<WbsComercial> EstructuraWbs(int Id) // Id OfertaComercial
        {
            List<WbsComercial> all = new List<WbsComercial>();
            var wbs = _wbscomercialRepository.GetAll().Where(c => c.vigente)
                                         .Where(c => c.id_nivel_padre_codigo == ".")
                                          .Where(c => c.OfertaComercialId == Id).ToList();
            if (wbs.Count > 0)
            {
                foreach (var item in wbs)
                {
                    all.Add(item);
                    var lista = this.ObtenerWbsHijos(Id, item.id_nivel_codigo, all);
                }
            }
            return all;

        }

        public List<WbsComercial> ObtenerWbsHijos(int Id, string id_nivel_codigo, List<WbsComercial> estructura)
        {
            var wbs = _wbscomercialRepository.GetAll().Where(c => c.vigente)
                                       .Where(c => c.id_nivel_padre_codigo == id_nivel_codigo)
                                       .Where(c => c.OfertaComercialId == Id).ToList();
            if (wbs.Count > 0)
            {

                foreach (var item in wbs)
                {
                    estructura.Add(item);
                    var hijos = this.ObtenerWbsHijos(Id, item.id_nivel_codigo, estructura);
                }
            }

            return estructura;
        }

       public string nombrecatalogo(int tipocatagoid)
        {
            var a = _catalogoRepository.Get(tipocatagoid);
            if (a != null && a.Id > 0)
            {
                return a.nombre;
            }
            else
            {
                return "";
            }

        }

        public string nombreexcelofertaeconomica(int Id)
        {
            string resultado = "Propuesta de  ";
            var oferta_comercial = _ofertacomercialRepository.Get(Id);


            var proyectosligados = Repository.GetAll().Where(c => c.vigente)
                                                       .Where(c => c.OfertaComercialId == Id)
                                                       .Select(c => c.Requerimiento.Proyecto.codigo).Distinct().ToArray();

            var requerimiento = Repository.GetAllIncluding(c => c.Requerimiento.Proyecto).Where(c => c.vigente)
                                                       .Where(c => c.OfertaComercialId == Id)
                                                     .ToList();


            if (proyectosligados.Length == 1)
            {

                if (requerimiento != null && requerimiento.Count == 1 && requerimiento[0].Requerimiento.tipo_requerimiento == TipoRequerimiento.Principal)
                {
                    resultado = resultado + "Trabajos para " + requerimiento[0].Requerimiento.codigo + " " + requerimiento[0].Requerimiento.Proyecto.descripcion_proyecto
                        ;
                }

                else
                {
                    resultado = resultado + "Trabajos Adicionales para " + string.Join(" ; ", requerimiento.Select(c => c.Requerimiento.Proyecto.codigo)) + "-" + requerimiento[0].Requerimiento.Proyecto.descripcion_proyecto + "\n" + oferta_comercial.descripcion;
                    return resultado;
                }
            }
            else if (proyectosligados.Length > 1)
            {
                resultado = resultado + "Trabajos Adicionales para " + string.Join(" ; ", requerimiento.Select(c => c.Requerimiento.Proyecto.codigo)) + "\n" + oferta_comercial.descripcion;
                return resultado;
            }



            return resultado;

        }

        public List<Proyecto> ListadoProyectos(int OfertaComercialId)
        {
            var proyectos = Repository.GetAll().Where(c => c.vigente).Where(c => c.OfertaComercialId == OfertaComercialId).Select(c => c.Requerimiento.Proyecto).Distinct().ToList();

            var lista = (from p in proyectos
                         select new ProyectoDto()
                         {
                             Id = p.Id,
                             codigo = p.codigo,
                             descripcion_proyecto = p.descripcion_proyecto
                         }).ToList();

            return proyectos;
        }




        public ExcelPackage SecondFormatPropuestaEconomica(int Id, int nivel_maximo)
        {
            //MAXIMO NIVEL WBS

            int nivel_maximo_wbs = nivel_maximo;
            //Datos Oferta Económica
            var oferta = _ofertacomercialRepository.Get(Id);

            //Lista de los Wbs

            var estructurawbs = this.EstructuraWbs(Id); //Estructura Wbs de la Ofertal Comercial


            //Saco todos los Computos
            var listacomputos = _computocomercialRepository
                           .GetAllIncluding(x => x.WbsComercial, x => x.Item, x => x.Item.Grupo, x => x.Item.Especialidad)
                           .Where(x => x.vigente)
                           .Where(x => x.Item.EspecialidadId.HasValue)
                           .Where(x => x.WbsComercial.OfertaComercialId == Id)
                           .ToList();

            var computos = (from z in listacomputos
                            where z.WbsComercial.OfertaComercialId == oferta.Id
                            where z.vigente == true
                            select new ComputoComercialDto
                            {
                                Id = z.Id,
                                ItemId = z.ItemId,
                                WbsComercialId = z.WbsComercialId,
                                item_codigo = z.Item.codigo,
                                item_nombre = z.Item.nombre,
                                cantidad = z.cantidad,
                                cantidad_eac = z.cantidad_eac,
                                costo_total = z.costo_total,
                                precio_unitario = z.precio_unitario,
                                Item = z.Item,
                                precio_incrementado = z.precio_incrementado,
                                precio_ajustado = z.precio_ajustado,
                                diferente = z.cantidad_eac != z.cantidad,
                                total_pu = (z.cantidad * z.precio_unitario),
                                total_pu_aui = (z.cantidad * z.precio_incrementado),
                                vigente = z.vigente,
                                codigo_especialidad = z.Item.Especialidad != null ? z.Item.Especialidad.codigo : "",
                                codigo_grupo = z.Item.Grupo.codigo

                            }).ToList();

            foreach (var e in computos)
            {
                int padreid = _itemservice.buscaridentificadorpadre(e.Item.item_padre);
                if (padreid != 0)
                {
                    var I = _itemservice.GetDetalle(padreid);
                    e.item_padre_nombre = I.Result.nombre;
                    e.item_padre_codigo = I.Result.codigo;
                }
            }


            //Inicio de Documento
            // var items = _itemservice.ArbolWbsExcel(oferta.ContratoId, oferta.fecha_oferta.GetValueOrDefault());
            //var items = _itemRepository.GetAll().Where(x => x.vigente).ToList();
            var items = _itemservice.ArbolItemsComputoComercial(oferta.Id);
           // var items = _itemservice.ItemsMatrizComercial(oferta.ContratoId, oferta.fecha_oferta.GetValueOrDefault(), computos);
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Propuesta Económica");

            workSheet.TabColor = System.Drawing.Color.Azure;
            workSheet.DefaultRowHeight = 15;
            workSheet.View.ZoomScale = 85;

            workSheet.Protection.IsProtected = false;



            int row = nivel_maximo; //fila del wbs
            for (int i = 1; i <= row; i++)
            {
                workSheet.Row(i).Style.Font.Bold = true;

            }






            int columna = 6;

            foreach (var itemswbs in estructurawbs.Where(x => x.es_actividad).ToList())
            {
                int fila = nivel_maximo + 4;
                List<WbsComercial> Jerarquia = new List<WbsComercial>();
                WbsComercial item = _wbscomercialRepository.Get(itemswbs.Id);
                Jerarquia.Add(item);
                while (item.id_nivel_padre_codigo != ".")
                {
                    //
                    item = (from x in estructurawbs
                            where x.id_nivel_codigo == item.id_nivel_padre_codigo
                            where x.vigente
                            where x.OfertaComercialId == item.OfertaComercialId
                            select x).FirstOrDefault();
                    if (item != null)
                    {
                        Jerarquia.Add(item);
                    }

                }
                int a = Jerarquia.Count();
                int rowtyle = nivel_maximo - 1;
                foreach (var wbsj in Jerarquia)
                {
                    if (rowtyle > 0)
                    {

                        workSheet.Cells[rowtyle, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[rowtyle, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                        workSheet.Cells[rowtyle, columna].Style.Border.Left.Style =
                        workSheet.Cells[rowtyle, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        rowtyle--;
                    }
                    if (a > 0)
                    {
                        if (wbsj.es_actividad)


                        {
                            //PRIMERA HOJA
                            workSheet.Cells[fila - 4, columna].Value = wbsj.nivel_nombre;
                            workSheet.Cells[fila - 4, columna].Value = wbsj.nivel_nombre;
                            workSheet.Column(columna).Width = 25;
                            workSheet.Cells[fila - 4, columna].Style.WrapText = true;
                            workSheet.Cells[fila - 4, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[fila - 4, columna].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[fila - 4, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[fila - 4, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            workSheet.Cells[fila - 4, columna].Style.Border.Top.Style =
                            workSheet.Cells[fila - 4, columna].Style.Border.Left.Style =
                            workSheet.Cells[fila - 4, columna].Style.Border.Right.Style =
                            workSheet.Cells[fila - 4, columna].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                            workSheet.Cells[fila - 2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[fila - 2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);

                        }
                        else
                        {
                            workSheet.Cells[a, columna].Value = wbsj.nivel_nombre;
                            workSheet.Cells[a, columna].Style.WrapText = true;
                            workSheet.Cells[a, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[a, columna].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[a, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[a, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                            workSheet.Cells[a, columna].Style.Border.Left.Style =
                            workSheet.Cells[a, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                        }
                    }

                    a--;
                }

                workSheet.Row(fila - 3).Hidden = true;
                workSheet.Cells[fila - 3, columna].Value = itemswbs.Id;
                workSheet.Cells[fila - 3, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[fila - 3, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                workSheet.Cells[fila - 3, columna].Style.Border.Left.Style =
                workSheet.Cells[fila - 3, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;




                if (itemswbs.DisciplinaId >= 0)
                {
                    //PRIMERA HOJA
                    workSheet.Cells[fila - 2, columna].Value = itemswbs.Catalogo.nombre;
                    workSheet.Cells[fila - 2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[fila - 2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                    workSheet.Cells[fila - 2, columna].Style.Border.Left.Style =
                    workSheet.Cells[fila - 2, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    workSheet.Cells[fila - 2, columna].Style.Border.Top.Style =
                    workSheet.Cells[fila - 2, columna].Style.Border.Left.Style =
                    workSheet.Cells[fila - 2, columna].Style.Border.Right.Style =
                    workSheet.Cells[fila - 2, columna].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                }
                else
                {
                    //PRIMERA HOJA
                    workSheet.Row(fila - 2).Hidden = true;
                }
                workSheet.Cells[1, columna].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                columna = columna + 1;
            }


            workSheet.Cells[nivel_maximo, 1].Value = "Grupo";
            workSheet.Cells[nivel_maximo, 2].Value = "Id";

            workSheet.Cells[1, 3, nivel_maximo, 3].Merge = true;
            workSheet.Cells[1, 3, nivel_maximo, 3].Style.WrapText = true;
            workSheet.Cells[1, 3, nivel_maximo, 3].Value = "ITEM";
            workSheet.Cells[1, 3, nivel_maximo, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 3, nivel_maximo, 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[1, 3, nivel_maximo, 3].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            workSheet.Cells[1, 3, nivel_maximo, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, 3, nivel_maximo, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            workSheet.Cells[1, 4, nivel_maximo, 4].Merge = true;
            workSheet.Cells[1, 4, nivel_maximo, 4].Style.WrapText = true;
            workSheet.Cells[1, 4, nivel_maximo, 4].Value = "DESCRIPCIÓN";
            workSheet.Cells[1, 4, nivel_maximo, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 4, nivel_maximo, 4].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[1, 4, nivel_maximo, 4].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            workSheet.Cells[1, 4, nivel_maximo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, 4, nivel_maximo, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells[1, 5, nivel_maximo, 5].Merge = true;
            workSheet.Cells[1, 5, nivel_maximo, 5].Style.WrapText = true;
            workSheet.Cells[1, 5, nivel_maximo, 5].Value = "UNIDAD";
            workSheet.Cells[1, 5, nivel_maximo, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 5, nivel_maximo, 5].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[1, 5, nivel_maximo, 5].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            workSheet.Cells[1, 5, nivel_maximo, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, 5, nivel_maximo, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



            //EMPIEZA PARTE DE LA DERECHA CALCULOS Y TODO

            var noOfCol = workSheet.Dimension.End.Column;




            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Merge = true;
            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Style.WrapText = true;
            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Value = "CANTIDAD ESTIMADA";
            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Merge = true;
            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Style.WrapText = true;
            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Value = "PRECIO UNITARIO";
            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Merge = true;
            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Style.WrapText = true;
            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Value = "COSTO TOTAL ESTIMADO";
            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            workSheet.Column(3).Width = 15;
            workSheet.Column(4).Width = 80;
            workSheet.Column(5).Width = 15;
            workSheet.Column(3).Style.WrapText = true;
            workSheet.Column(4).Style.WrapText = true;
            workSheet.Column(5).Style.WrapText = true;
            workSheet.Column(1).Style.Font.Bold = true;
            workSheet.Column(1).Width = 3;
            workSheet.Column(2).Width = 3;
            workSheet.Column(1).Style.Font.Color.SetColor(Color.White);
            workSheet.Column(2).Style.Font.Color.SetColor(Color.White);

            workSheet.Column(noOfCol + 1).Width = 18;
            workSheet.Column(noOfCol + 2).Width = 18;
            workSheet.Column(noOfCol + 3).Width = 18;

            workSheet.Row(nivel_maximo).Height = 15;


            //int inicio de las filas
            //int c = 5;
            int c = nivel_maximo + 3;
            workSheet.View.FreezePanes(6, 1);
            workSheet.View.FreezePanes(c + 4, 6);

            //PRIMERA HOJA 1-6
            foreach (var pitem in items)
            {

                workSheet.Cells[c, 1].Value = pitem.GrupoId;
                workSheet.Cells[c, 2].Value = pitem.Id;
                workSheet.Cells[c, 3].Value = pitem.codigo;
                workSheet.Cells[c, 4].Value = pitem.nombre;
                workSheet.Cells[c, 4].Style.WrapText = true;
                if (pitem.UnidadId != 0)
                {
                    workSheet.Cells[c, 5].Value = this.nombrecatalogo(pitem.UnidadId);

                }

                if (pitem.item_padre == ".")
                {
                    int padre = workSheet.Dimension.End.Column;
                    workSheet.Cells[c, 3, c, padre].Style.Font.Bold = true;
                    workSheet.Cells[c, 3, c, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[c, 3, c, padre].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    workSheet.Cells[c, 3, c, padre].Style.Border.Top.Style =
                    workSheet.Cells[c, 3, c, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                }
                if (pitem.item_padre != "." && pitem.para_oferta == false)
                {
                    int padre = workSheet.Dimension.End.Column;

                    workSheet.Cells[c, 3, c, padre].Style.Font.Bold = true;
                    workSheet.Cells[c, 3, c, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[c, 3, c, padre].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    workSheet.Cells[c, 3, c, padre].Style.Border.Top.Style =
                    workSheet.Cells[c, 3, c, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                }
                workSheet.Cells[c, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                workSheet.Cells[c, 3].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                workSheet.Cells[c, workSheet.Dimension.End.Column].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                workSheet.Row(c).Style.Locked = false;
                c = c + 1;
            }
            var noOfRow = workSheet.Dimension.End.Row;
            for (int j = 6; j <= noOfCol; j++)
            {

                var wbsid = (workSheet.Cells[nivel_maximo + 1, j].Value ?? "").ToString();
                for (int i = nivel_maximo + 3; i <= noOfRow; i++)
                {
                    var itemid = (workSheet.Cells[i, 2].Value ?? "").ToString();

                    var computo = (from co in computos
                                   where co.WbsComercialId == Convert.ToInt32(wbsid.Length > 0 ? wbsid : "0")
                                   where co.ItemId == Convert.ToInt32(itemid.Length > 0 ? itemid : "0")
                                   where co.vigente
                                   select co).FirstOrDefault();
                    if (computo != null)
                    {
                        workSheet.Cells[i, j].Value = computo.cantidad;
                        workSheet.Cells[i, noOfCol + 2].Value = computo.precio_unitario;
                        workSheet.Cells[i, j].Style.Numberformat.Format = "#,##0.00";
                        workSheet.Cells[i, noOfCol + 1].Style.Numberformat.Format = "#,##0.00";
                        workSheet.Cells[i, noOfCol + 2].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                        workSheet.Cells[i, noOfCol + 3].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";



                        var rango_incio = workSheet.Cells[i, 6].Address;
                        var rango_final = workSheet.Cells[i, noOfCol].Address;
                        var rangosumar = "$" + rango_incio + ":" + "$" + rango_final;
                        workSheet.Cells[i, noOfCol + 1].Formula = "=SUM(" + rangosumar + ")";

                        var cantidad = workSheet.Cells[i, noOfCol + 1].Value;
                        var precio = workSheet.Cells[i, noOfCol + 2].Value;

                        var dcantidad = workSheet.Cells[i, noOfCol + 1].Address;
                        var dprecio = workSheet.Cells[i, noOfCol + 2].Address;
                        workSheet.Cells[i, noOfCol + 3].FormulaR1C1 = "=ROUND($" + dcantidad + "*$" + dprecio + ", 2)";

                    }


                }




            }

            // RANGOS FINALES

            int ncol = workSheet.Dimension.End.Column;
            // rango total
            string costoi = workSheet.Cells[nivel_maximo + 3, ncol].Address;
            string costoif = workSheet.Cells[noOfRow, ncol].Address;
            string rangovalortotal = "$" + costoi + ":$" + costoif;




            #region CalculosAnteriores
            /*//CALCULOS BASE
            //Ingenieria
            workSheet.Cells[bfila, 4].Value = "Sub - total Ingeniería";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =
            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;


            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;


            //suma ingenieria

            string dinicio = workSheet.Cells[nivel_maximo + 3, 1].Address;
            string dfinal = workSheet.Cells[noOfRow, 1].Address;
            string rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            string grupo = "" + 1 + "";

            var formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string sumaingenieria = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;


            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            bfila++;
            //Procura
            workSheet.Cells[bfila, 4].Value = "Sub-total Procura";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
               workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;


            //suma procura

            dinicio = workSheet.Cells[nivel_maximo + 3, 1].Address;
            dfinal = workSheet.Cells[noOfRow, 1].Address;
            rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            grupo = "" + 3 + "";

            formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string suma_procura = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //CALCULOS BASE
            //Reembolsables
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Sub-total Reembolsables";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, noOfCol + 3].Value = 0;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string sumareembolsables = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;

            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //Consturccion
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Sub-total Construcción";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
             workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            dinicio = workSheet.Cells[nivel_maximo + 3, 1].Address;
            dfinal = workSheet.Cells[noOfRow, 1].Address;
            rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            grupo = "" + 2 + "";

            formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string suma_contruccion = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //Descuento
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Descuento por <Descripción del concepto>";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
             workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Row(bfila).Hidden = true;
            workSheet.Cells[bfila, noOfCol + 3].Value = 0;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            //Descuento
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administración";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
                        workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            string administracion = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;



            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administracion sobre Obra (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.4119;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";

            //contruccion

            string formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_contruccion;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string valoradministracion_obra = workSheet.Cells[bfila, noOfCol + 3].Address;

            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Imprevistos sobre Obra (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.03;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";

            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_contruccion;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string valorimprevistos_obra = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;
            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Utilidad sobre Obra (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.12;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";


            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_contruccion;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;



            string valor_utilidadObra = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;
            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administracion sobre Procura Contratista (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.1;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";




            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_procura;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string valor_procura = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administracion sobre Reembolsables (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.1;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";


            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + sumareembolsables;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string valor_reembolsables = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;
            //envio de datos a administracion

            formula_calculo = "=SUM(" + valoradministracion_obra + ":" + valor_reembolsables + ")";
            workSheet.Cells[administracion].Formula = formula_calculo;
            workSheet.Cells[administracion].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[administracion].Style.WrapText = true;

           
            bfila++;
            workSheet.Cells[bfila, 4].Value = "TOTAL";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            //TOTAL

            formula_calculo = "=SUM(" + sumaingenieria + "+" + suma_procura + "+" + suma_contruccion + "+" + sumareembolsables + "+" + administracion + ")";
            workSheet.Cells[bfila, noOfCol + 3].Formula = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;
            string valortotal = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            */
            #endregion

            #region Totales Formato Contrato 2
            var t = this.TotalesSecondFormat(listacomputos);
            var hoja = workSheet;
            var rowf = noOfRow + 2;

            string cell = "C" + rowf + ":E" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "A - VALOR COSTO TOTAL INGENIERÍA BASICA Y DETALLE (AIU incluido)   -  ANEXO 1";
            hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Font.Color.SetColor(Color.White);
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 128, 128));

            hoja.Cells[rowf, noOfCol + 3].Value = t.A_VALOR_COSTO_TOTAL_INGENIERÍA_BASICA_YDETALLE_AIU_ANEXO1;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf = rowf + 2;

            cell = "C" + rowf + ":E" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "VALOR PROCURA (Equipos mayores y materiales no incluidos en items de pago)";
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));

            hoja.Cells[rowf, noOfCol + 3].Value = t.VALOR_PROCURA;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf++;

            cell = "C" + rowf + ":D" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "Administracion sobre Procura Contratista (%)";
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));

            cell = "E" + rowf + "";
            hoja.Cells[cell].Value = 0.10;
            hoja.Cells[cell].Style.Numberformat.Format = "0.00%";
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));

            hoja.Cells[rowf, noOfCol + 3].Value = t.Administracion_sobre_Procura_Contratista;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf++;
            cell = "C" + rowf + ":E" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "B - VALOR COSTO DIRECTO PROCURA CONTRATISTA    -   ANEXO 2";
            hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Font.Color.SetColor(Color.White);
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 128, 128));

            hoja.Cells[rowf, noOfCol + 3].Value = t.B_VALOR_COSTO_DIRECTO_PROCURA_CONTRATISTA_ANEXO2;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);



            rowf = rowf + 2;
            cell = "C" + rowf + ":E" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "VALOR SUBCONTRATOS";
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(180, 198, 231));

            hoja.Cells[rowf, noOfCol + 3].Value = t.VALOR_SUBCONTRATOS;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf++;
            cell = "C" + rowf + ":D" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "Administracion Subcontratos (%)";
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(180, 198, 231));

            cell = "E" + rowf + "";
            hoja.Cells[cell].Value = 0.1838;
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Numberformat.Format = "0.00%";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(180, 198, 231));

            hoja.Cells[rowf, noOfCol + 3].Value = t.Administracion_sobre_Subcontratos_Contratista;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf++;

            cell = "C" + rowf + ":E" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "C - VALOR COSTO DIRECTO SUBCONTRATOS CONTRATISTA";
            hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Font.Color.SetColor(Color.White);
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(48, 84, 150));

            hoja.Cells[rowf, noOfCol + 3].Value = t.C_VALOR_COSTO_DIRECTO_SUBCONTRATOS_CONTRATISTA;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf = rowf + 2;

            cell = "C" + rowf + ":E" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "FACTOR OBRAS CIVILES";
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));

            hoja.Cells[rowf, noOfCol + 3].Value = t.VALOR_COSTO_DIRECTO_OBRAS_CIVILES;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf++;

            cell = "C" + rowf + ":E" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "FACTOR OBRAS MECÁNICAS";
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));
            hoja.Cells[rowf, noOfCol + 3].Value = t.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf++;

            cell = "C" + rowf + ":E" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "FACTOR OBRAS ELECTRICAS";
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));
            hoja.Cells[rowf, noOfCol + 3].Value = t.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            rowf++;

            cell = "C" + rowf + ":E" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "FACTOR OBRAS INSTRUMENTOS & CONTROL";
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));

            hoja.Cells[rowf, noOfCol + 3].Value = t.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf++;

            cell = "C" + rowf + ":E" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "FACTOR SERVICIOS ESPECIALES";
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));

            hoja.Cells[rowf, noOfCol + 3].Value = t.VALOR_COSTO_DIRECTO_SERVICIOS_ESPECIALES;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf++;

            cell = "C" + rowf + ":E" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "FACTOR 1% ITEMS MECÁNICOS, ELÉCTRICOS E INSTRUMENTACIÓN";
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(180, 198, 231));

            hoja.Cells[rowf, noOfCol + 3].Value = t.DESCUENTO_ITEMS_MECÁNICOS_ELÉCTRICOS_INSTRUMENTACIÓN;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf++;
            cell = "C" + rowf + ":E" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "D - VALOR COSTO DIRECTO CONSTRUCCIÓN";
            hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Font.Color.SetColor(Color.White);
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 128, 128));

            hoja.Cells[rowf, noOfCol + 3].Value = t.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf = rowf + 2;
            cell = "C" + rowf + ":D" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "Administracion sobre Obra (%)";
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));

            cell = "E" + rowf + "";
            hoja.Cells[cell].Value = 0.4119;
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Numberformat.Format = "0.00%";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));

            hoja.Cells[rowf, noOfCol + 3].Value = t.Administracion_sobre_Obra;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf++;

            cell = "C" + rowf + ":D" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "Imprevistos sobre Obra (%)";
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));



            cell = "E" + rowf + "";
            hoja.Cells[cell].Value = 0.03;
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Numberformat.Format = "0.00%";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));

            hoja.Cells[rowf, noOfCol + 3].Value = t.Imprevistos_sobre_Obra;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf++;

            cell = "C" + rowf + ":D" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "Utilidad sobre Obra (%)";
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));

            cell = "E" + rowf + "";
            hoja.Cells[cell].Value = 0.12;
            //hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Numberformat.Format = "0.00%";
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));

            hoja.Cells[rowf, noOfCol + 3].Value = t.Utilidad_sobre_Obra;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf++;
            cell = "C" + rowf + ":E" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "E - INDIRECTOS SOBRE VALOR COSTO DIRECTO CONSTRUCCIÓN";
            hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Font.Color.SetColor(Color.White);
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 128, 128));

            hoja.Cells[rowf, noOfCol + 3].Value = t.E_INDIRECTOS_SOBRE_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            rowf = rowf + 2;

            cell = "C" + rowf + ":E" + rowf;
            hoja.Cells[cell].Merge = true;
            hoja.Cells[cell].Value = "COSTO TOTAL DEL PROYECTO   A + B + C + D + E";
            hoja.Cells[cell].Style.Font.Bold = true;
            hoja.Cells[cell].Style.Font.Name = "Arial";
            hoja.Cells[cell].Style.Font.Color.SetColor(Color.White);
            hoja.Cells[cell].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[cell].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[cell].Style.Fill.BackgroundColor.SetColor(Color.Black);

            hoja.Cells[rowf, noOfCol + 3].Value = t.COSTO_TOTAL_DEL_PROYECTO_ABCDE;
            hoja.Cells[rowf, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            hoja.Cells[rowf, noOfCol + 3].Style.WrapText = true;
            hoja.Cells[rowf, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            #endregion
            //AÑADIR CABECERA



            //ELIMINAR ROWS OCULTAS

            /*
            if (nivel_maximo_wbs > 0)
            {
                workSheet.DeleteRow(nivel_maximo_wbs + 1);
            }
            if (nivel_maximo_wbs > 0)
            {
                workSheet.DeleteRow(nivel_maximo_wbs + 2);
            }
            */

            workSheet.InsertRow(1, 4);

            workSheet.Cells[2, 3, 2, workSheet.Dimension.End.Column].Style.Border.Top.Style = ExcelBorderStyle.Medium;
            workSheet.Cells[2, 3, 2, workSheet.Dimension.End.Column].Style.Border.Top.Color.SetColor(Color.Blue);
            workSheet.Cells[2, 3, workSheet.Dimension.End.Row, 3].Style.Border.Left.Style = ExcelBorderStyle.Medium;
            workSheet.Cells[2, 3, workSheet.Dimension.End.Row, 3].Style.Border.Left.Color.SetColor(Color.Blue);
            workSheet.Cells[2, workSheet.Dimension.End.Column, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column].Style.Border.Right.Style = ExcelBorderStyle.Medium;
            workSheet.Cells[2, workSheet.Dimension.End.Column, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column].Style.Border.Right.Color.SetColor(Color.Blue);

            string nombre = this.nombreexcelofertaeconomica(oferta.Id);
            string contrato = oferta.Contrato != null && oferta.Contrato.sitio_referencia != null ? oferta.Contrato.sitio_referencia : "";
            if (contrato.Length > 0)
            {
                workSheet.Cells["D2"].Value = contrato;
            }
            else
            {
                workSheet.Cells["D2"].Value = "SITIO REFERENCIA?";
            }

            workSheet.Cells["D2"].Style.WrapText = true;
            workSheet.Cells["D2"].Style.Font.Bold = true;
            workSheet.Cells["D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["D2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells[2, workSheet.Dimension.End.Column - 2, 2, workSheet.Dimension.End.Column - 1].Merge = true;
            workSheet.Cells[2, workSheet.Dimension.End.Column - 2, 2, workSheet.Dimension.End.Column - 1].Value = oferta.codigo;
            workSheet.Cells[2, workSheet.Dimension.End.Column - 2, 2, workSheet.Dimension.End.Column - 1].Style.WrapText = true;
            workSheet.Cells[2, workSheet.Dimension.End.Column - 2, 2, workSheet.Dimension.End.Column - 1].Style.Font.Bold = true;
            workSheet.Cells[2, workSheet.Dimension.End.Column - 2, 2, workSheet.Dimension.End.Column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[2, workSheet.Dimension.End.Column - 2, 2, workSheet.Dimension.End.Column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells[2, workSheet.Dimension.End.Column].Value = "Revisión " + oferta.version;
            workSheet.Cells[2, workSheet.Dimension.End.Column].Style.WrapText = true;
            workSheet.Cells[2, workSheet.Dimension.End.Column].Style.Font.Bold = true;
            workSheet.Cells[2, workSheet.Dimension.End.Column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[2, workSheet.Dimension.End.Column].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells["D3"].Value = "Anexo-Propuesta Económica";
            workSheet.Cells["D3"].Style.WrapText = true;
            workSheet.Cells["D3"].Style.Font.Bold = true;
            workSheet.Cells["D3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["D3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells["D4"].Value = nombre;
            workSheet.Cells["D4"].Style.WrapText = true;
            workSheet.Cells["D4"].Style.Font.Bold = true;
            workSheet.Cells["D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["D4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells[workSheet.Dimension.End.Row, 3, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            workSheet.Cells[workSheet.Dimension.End.Row, 3, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column].Style.Border.Bottom.Color.SetColor(Color.Blue);

            //FONDO BLANCO
            workSheet.Cells[2, 3, 4, workSheet.Dimension.End.Column].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[2, 3, 4, workSheet.Dimension.End.Column].Style.Fill.BackgroundColor.SetColor(Color.White);

            //COLUMNAS WIDTH ULTIMAS Y PENULTIMA
            workSheet.Column(workSheet.Dimension.End.Column).Width = 20;
            workSheet.Column(workSheet.Dimension.End.Column - 1).Width = 20; //PENULTIMO



            //FORMATO A UNA PAGINA
            workSheet.View.PageBreakView = true;
            workSheet.PrinterSettings.PrintArea = workSheet.Cells[2, 3, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column];

            workSheet.PrinterSettings.Orientation = eOrientation.Landscape;

            workSheet.PrinterSettings.FitToPage = true;
            workSheet.Cells[1, 1, workSheet.Dimension.End.Row, 2].Style.Font.Color.SetColor(Color.FromArgb(159, 159, 159));

            workSheet.Cells[5, 3, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column].AutoFilter = true;


            return excel;

        }


        public ExcelPackage GenerarPropuestaEconomica(int Id, int nivel_maximo)
        {
            //MAXIMO NIVEL WBS

            int nivel_maximo_wbs = nivel_maximo;
            //Datos Oferta Económica
            var oferta = _ofertacomercialRepository.Get(Id);

            //Lista de los Wbs

            var estructurawbs = this.EstructuraWbs(Id); //Estructura Wbs de la Ofertal Comercial


            //Saco todos los Computos
            var computos = _computocomercialRepository
                          .GetAllIncluding(x => x.WbsComercial, x => x.Item, x => x.Item.Grupo, x => x.Item.Especialidad)
                           .Where(x => x.vigente)
                           .Where(x => x.WbsComercial.OfertaComercialId == Id)
                           .ToList();

            var listacomputos = (from z in computos
                                 where z.WbsComercial.OfertaComercialId == oferta.Id
                            where z.vigente == true
                            select new ComputoComercialDto
                            {
                                Id = z.Id,
                                ItemId = z.ItemId,
                                WbsComercialId = z.WbsComercialId,
                                item_codigo = z.Item.codigo,
                                item_nombre = z.Item.nombre,
                                cantidad = z.cantidad,
                                cantidad_eac = z.cantidad_eac,
                                costo_total = z.costo_total,
                                precio_unitario = z.precio_unitario,
                                Item = z.Item,
                                precio_incrementado = z.precio_incrementado,
                                precio_ajustado = z.precio_ajustado,
                                diferente = z.cantidad_eac != z.cantidad,
                                total_pu = (z.cantidad * z.precio_unitario),
                                total_pu_aui = (z.cantidad * z.precio_incrementado),
                                vigente = z.vigente,
                                codigo_especialidad = z.Item.Especialidad != null ? z.Item.Especialidad.codigo : "",
                                codigo_grupo = z.Item.Grupo.codigo

                            }).ToList();

            foreach (var e in listacomputos)
            {
                int padreid = _itemservice.buscaridentificadorpadre(e.Item.item_padre);
                if (padreid != 0)
                {
                    var I = _itemservice.GetDetalle(padreid);
                    e.item_padre_nombre = I.Result.nombre;
                    e.item_padre_codigo = I.Result.codigo;
                }
            }



            //Inicio de Documento
            // var items = _itemservice.ArbolWbsExcel(oferta.ContratoId, oferta.fecha_oferta.GetValueOrDefault());
            //var items = _itemRepository.GetAll().Where(x => x.vigente).ToList();
            var items = _itemservice.ArbolItemsComputoComercial(oferta.Id);
           // var items = _itemservice.ItemsMatrizComercial(oferta.ContratoId, oferta.fecha_oferta.GetValueOrDefault(),listacomputos);
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Propuesta Económica");

            workSheet.TabColor = System.Drawing.Color.Azure;
            workSheet.DefaultRowHeight = 15;
            workSheet.View.ZoomScale = 85;

            workSheet.Protection.IsProtected = false;



            int row = nivel_maximo; //fila del wbs
            for (int i = 1; i <= row; i++)
            {
                workSheet.Row(i).Style.Font.Bold = true;

            }






            int columna = 6;

            foreach (var itemswbs in estructurawbs.Where(x => x.es_actividad).ToList())
            {
                int fila = nivel_maximo + 4;
                List<WbsComercial> Jerarquia = new List<WbsComercial>();
                WbsComercial item = _wbscomercialRepository.Get(itemswbs.Id);
                Jerarquia.Add(item);
                while (item.id_nivel_padre_codigo != ".")
                {
                    //
                    item = (from x in estructurawbs
                            where x.id_nivel_codigo == item.id_nivel_padre_codigo
                            where x.vigente
                            where x.OfertaComercialId == item.OfertaComercialId
                            select x).FirstOrDefault();
                    if (item != null)
                    {
                        Jerarquia.Add(item);
                    }

                }
                int a = Jerarquia.Count();
                int rowtyle = nivel_maximo - 1;
                foreach (var wbsj in Jerarquia)
                {
                    if (rowtyle > 0)
                    {

                        workSheet.Cells[rowtyle, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[rowtyle, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                        workSheet.Cells[rowtyle, columna].Style.Border.Left.Style =
                        workSheet.Cells[rowtyle, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        rowtyle--;
                    }
                    if (a > 0)
                    {
                        if (wbsj.es_actividad)


                        {
                            //PRIMERA HOJA
                            workSheet.Cells[fila - 4, columna].Value = wbsj.nivel_nombre;
                            workSheet.Cells[fila - 4, columna].Value = wbsj.nivel_nombre;
                            workSheet.Column(columna).Width = 25;
                            workSheet.Cells[fila - 4, columna].Style.WrapText = true;
                            workSheet.Cells[fila - 4, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[fila - 4, columna].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[fila - 4, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[fila - 4, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            workSheet.Cells[fila - 4, columna].Style.Border.Top.Style =
                            workSheet.Cells[fila - 4, columna].Style.Border.Left.Style =
                            workSheet.Cells[fila - 4, columna].Style.Border.Right.Style =
                            workSheet.Cells[fila - 4, columna].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                            workSheet.Cells[fila - 2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[fila - 2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);

                        }
                        else
                        {
                            workSheet.Cells[a, columna].Value = wbsj.nivel_nombre;
                            workSheet.Cells[a, columna].Style.WrapText = true;
                            workSheet.Cells[a, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[a, columna].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[a, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[a, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                            workSheet.Cells[a, columna].Style.Border.Left.Style =
                            workSheet.Cells[a, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                        }
                    }

                    a--;
                }

                workSheet.Row(fila - 3).Hidden = true;
                workSheet.Cells[fila - 3, columna].Value = itemswbs.Id;
                workSheet.Cells[fila - 3, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[fila - 3, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                workSheet.Cells[fila - 3, columna].Style.Border.Left.Style =
                workSheet.Cells[fila - 3, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;




                if (itemswbs.DisciplinaId >= 0)
                {
                    //PRIMERA HOJA
                    workSheet.Cells[fila - 2, columna].Value = itemswbs.Catalogo.nombre;
                    workSheet.Cells[fila - 2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[fila - 2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                    workSheet.Cells[fila - 2, columna].Style.Border.Left.Style =
                    workSheet.Cells[fila - 2, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    workSheet.Cells[fila - 2, columna].Style.Border.Top.Style =
                    workSheet.Cells[fila - 2, columna].Style.Border.Left.Style =
                    workSheet.Cells[fila - 2, columna].Style.Border.Right.Style =
                    workSheet.Cells[fila - 2, columna].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                }
                else
                {
                    //PRIMERA HOJA
                    workSheet.Row(fila - 2).Hidden = true;
                }
                workSheet.Cells[1, columna].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                columna = columna + 1;
            }


            workSheet.Cells[nivel_maximo, 1].Value = "Grupo";
            workSheet.Cells[nivel_maximo, 2].Value = "Id";

            workSheet.Cells[1, 3, nivel_maximo, 3].Merge = true;
            workSheet.Cells[1, 3, nivel_maximo, 3].Style.WrapText = true;
            workSheet.Cells[1, 3, nivel_maximo, 3].Value = "ITEM";
            workSheet.Cells[1, 3, nivel_maximo, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 3, nivel_maximo, 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[1, 3, nivel_maximo, 3].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            workSheet.Cells[1, 3, nivel_maximo, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, 3, nivel_maximo, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            workSheet.Cells[1, 4, nivel_maximo, 4].Merge = true;
            workSheet.Cells[1, 4, nivel_maximo, 4].Style.WrapText = true;
            workSheet.Cells[1, 4, nivel_maximo, 4].Value = "DESCRIPCIÓN";
            workSheet.Cells[1, 4, nivel_maximo, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 4, nivel_maximo, 4].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[1, 4, nivel_maximo, 4].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            workSheet.Cells[1, 4, nivel_maximo, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, 4, nivel_maximo, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells[1, 5, nivel_maximo, 5].Merge = true;
            workSheet.Cells[1, 5, nivel_maximo, 5].Style.WrapText = true;
            workSheet.Cells[1, 5, nivel_maximo, 5].Value = "UNIDAD";
            workSheet.Cells[1, 5, nivel_maximo, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 5, nivel_maximo, 5].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[1, 5, nivel_maximo, 5].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            workSheet.Cells[1, 5, nivel_maximo, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, 5, nivel_maximo, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



            //EMPIEZA PARTE DE LA DERECHA CALCULOS Y TODO

            var noOfCol = workSheet.Dimension.End.Column;




            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Merge = true;
            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Style.WrapText = true;
            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Value = "CANTIDAD ESTIMADA";
            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, noOfCol + 1, nivel_maximo, noOfCol + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Merge = true;
            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Style.WrapText = true;
            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Value = "PRECIO UNITARIO";
            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, noOfCol + 2, nivel_maximo, noOfCol + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Merge = true;
            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Style.WrapText = true;
            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Value = "COSTO TOTAL ESTIMADO";
            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, noOfCol + 3, nivel_maximo, noOfCol + 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            workSheet.Column(3).Width = 15;
            workSheet.Column(4).Width = 80;
            workSheet.Column(5).Width = 15;
            workSheet.Column(3).Style.WrapText = true;
            workSheet.Column(4).Style.WrapText = true;
            workSheet.Column(5).Style.WrapText = true;
            workSheet.Column(1).Style.Font.Bold = true;
            workSheet.Column(1).Width = 3;
            workSheet.Column(2).Width = 3;
            workSheet.Column(1).Style.Font.Color.SetColor(Color.White);
            workSheet.Column(2).Style.Font.Color.SetColor(Color.White);

            workSheet.Column(noOfCol + 1).Width = 18;
            workSheet.Column(noOfCol + 2).Width = 18;
            workSheet.Column(noOfCol + 3).Width = 18;

            workSheet.Row(nivel_maximo).Height = 15;


            //int inicio de las filas
            //int c = 5;
            int c = nivel_maximo + 3;
            workSheet.View.FreezePanes(6, 1);
            workSheet.View.FreezePanes(c + 4, 6);

            //PRIMERA HOJA 1-6
            foreach (var pitem in items)
            {

                workSheet.Cells[c, 1].Value = pitem.GrupoId;
                workSheet.Cells[c, 2].Value = pitem.Id;
                workSheet.Cells[c, 3].Value = pitem.codigo;
                workSheet.Cells[c, 4].Value = pitem.nombre;
                workSheet.Cells[c, 4].Style.WrapText = true;
                if (pitem.UnidadId != 0)
                {
                    workSheet.Cells[c, 5].Value = this.nombrecatalogo(pitem.UnidadId);

                }

                if (pitem.item_padre == ".")
                {
                    int padre = workSheet.Dimension.End.Column;
                    workSheet.Cells[c, 3, c, padre].Style.Font.Bold = true;
                    workSheet.Cells[c, 3, c, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[c, 3, c, padre].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    workSheet.Cells[c, 3, c, padre].Style.Border.Top.Style =
                    workSheet.Cells[c, 3, c, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                }
                if (pitem.item_padre != "." && pitem.para_oferta == false)
                {
                    int padre = workSheet.Dimension.End.Column;

                    workSheet.Cells[c, 3, c, padre].Style.Font.Bold = true;
                    workSheet.Cells[c, 3, c, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[c, 3, c, padre].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    workSheet.Cells[c, 3, c, padre].Style.Border.Top.Style =
                    workSheet.Cells[c, 3, c, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                }
                workSheet.Cells[c, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                workSheet.Cells[c, 3].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                workSheet.Cells[c, workSheet.Dimension.End.Column].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                workSheet.Row(c).Style.Locked = false;
                c = c + 1;
            }
            var noOfRow = workSheet.Dimension.End.Row;
            for (int j = 6; j <= noOfCol; j++)
            {

                var wbsid = (workSheet.Cells[nivel_maximo + 1, j].Value ?? "").ToString();
                for (int i = nivel_maximo + 3; i <= noOfRow; i++)
                {
                    var itemid = (workSheet.Cells[i, 2].Value ?? "").ToString();

                    var computo = (from co in computos
                                   where co.WbsComercialId == Convert.ToInt32(wbsid.Length > 0 ? wbsid : "0")
                                   where co.ItemId == Convert.ToInt32(itemid.Length > 0 ? itemid : "0")
                                   where co.vigente
                                   select co).FirstOrDefault();
                    if (computo != null)
                    {
                        workSheet.Cells[i, j].Value = computo.cantidad;
                        workSheet.Cells[i, noOfCol + 2].Value = computo.precio_unitario;
                        workSheet.Cells[i, j].Style.Numberformat.Format = "#,##0.00";
                        workSheet.Cells[i, noOfCol + 1].Style.Numberformat.Format = "#,##0.00";
                        workSheet.Cells[i, noOfCol + 2].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                        workSheet.Cells[i, noOfCol + 3].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";



                        var rango_incio = workSheet.Cells[i, 6].Address;
                        var rango_final = workSheet.Cells[i, noOfCol].Address;
                        var rangosumar = "$" + rango_incio + ":$" + rango_final;
                        workSheet.Cells[i, noOfCol + 1].Formula = "=SUM(" + rangosumar + ")";

                        var cantidad = workSheet.Cells[i, noOfCol + 1].Value;
                        var precio = workSheet.Cells[i, noOfCol + 2].Value;

                        var dcantidad = workSheet.Cells[i, noOfCol + 1].Address;
                        var dprecio = workSheet.Cells[i, noOfCol + 2].Address;
                        workSheet.Cells[i, noOfCol + 3].FormulaR1C1 = "=ROUND($" + dcantidad + "*$" + dprecio + ", 2)";

                    }


                }




            }

            // RANGOS FINALES

            int ncol = workSheet.Dimension.End.Column;
            // rango total
            string costoi = workSheet.Cells[nivel_maximo + 3, ncol].Address;
            string costoif = workSheet.Cells[noOfRow, ncol].Address;
            string rangovalortotal = "$" + costoi + ":$" + costoif;


            int bfila = noOfRow;
            bfila = bfila + 2;
            //CALCULOS BASE
            //Ingenieria
            workSheet.Cells[bfila, 4].Value = "Sub - total Ingeniería";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =
            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;


            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;


            //suma ingenieria

            string dinicio = workSheet.Cells[nivel_maximo + 3, 1].Address;
            string dfinal = workSheet.Cells[noOfRow, 1].Address;
            string rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            string grupo = "" + 1 + "";

            var formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string sumaingenieria = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;


            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            bfila++;
            //Procura
            workSheet.Cells[bfila, 4].Value = "Sub-total Procura";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
               workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;


            //suma procura

            dinicio = workSheet.Cells[nivel_maximo + 3, 1].Address;
            dfinal = workSheet.Cells[noOfRow, 1].Address;
            rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            grupo = "" + 3 + "";

            formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string suma_procura = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //CALCULOS BASE
            //Reembolsables
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Sub-total Reembolsables";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, noOfCol + 3].Value = 0;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string sumareembolsables = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;

            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //Consturccion
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Sub-total Construcción";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
             workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            dinicio = workSheet.Cells[nivel_maximo + 3, 1].Address;
            dfinal = workSheet.Cells[noOfRow, 1].Address;
            rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            grupo = "" + 2 + "";

            formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string suma_contruccion = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //Descuento
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Descuento por <Descripción del concepto>";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
             workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Row(bfila).Hidden = true;
            workSheet.Cells[bfila, noOfCol + 3].Value = 0;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            //Descuento
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administración";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
                        workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            string administracion = "$" + workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;



            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administracion sobre Obra (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.4119;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";

            //contruccion

            string formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_contruccion;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string valoradministracion_obra = workSheet.Cells[bfila, noOfCol + 3].Address;

            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Imprevistos sobre Obra (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.03;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";

            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_contruccion;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string valorimprevistos_obra = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;
            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Utilidad sobre Obra (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.12;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";


            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_contruccion;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;



            string valor_utilidadObra = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;
            //
            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administracion sobre Procura Contratista (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.1;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";




            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + suma_procura;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string valor_procura = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            bfila++;
            workSheet.Cells[bfila, 4].Value = "Administracion sobre Reembolsables (%)";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;

            workSheet.Cells[bfila, 5].Value = 0.1;
            workSheet.Cells[bfila, 5].Style.WrapText = true;

            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            workSheet.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            workSheet.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 5].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";


            //caculo
            formula_calculo = "=" + workSheet.Cells[bfila, 5].Address + "*" + sumareembolsables;
            workSheet.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string valor_reembolsables = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;
            //envio de datos a administracion

            formula_calculo = "=SUM(" + valoradministracion_obra + ":" + valor_reembolsables + ")";
            workSheet.Cells[administracion].Formula = formula_calculo;
            workSheet.Cells[administracion].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[administracion].Style.WrapText = true;

            /*  for (var i = 0; i <=4; i++)
              {
                  workSheet.Row(bfila-i).OutlineLevel = 1;
                  workSheet.Row(bfila-i).Collapsed = true;
              }

      */
            bfila++;
            workSheet.Cells[bfila, 4].Value = "TOTAL";
            workSheet.Cells[bfila, 4].Style.WrapText = true;
            workSheet.Cells[bfila, 4].Style.Border.Top.Style =
            workSheet.Cells[bfila, 4].Style.Border.Left.Style =

            workSheet.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[bfila, 4].Style.Font.Bold = true;
            workSheet.Cells[bfila, 5].Style.Border.Top.Style =
            workSheet.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            //TOTAL

            formula_calculo = "=SUM(" + sumaingenieria + "+" + suma_procura + "+" + suma_contruccion + "+" + sumareembolsables + "+" + administracion + ")";
            workSheet.Cells[bfila, noOfCol + 3].Formula = formula_calculo;
            workSheet.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            workSheet.Cells[bfila, noOfCol + 3].Style.WrapText = true;
            string valortotal = workSheet.Cells[bfila, noOfCol + 3].Address;
            workSheet.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         workSheet.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;



            //AÑADIR CABECERA



            //ELIMINAR ROWS OCULTAS

            /*
            if (nivel_maximo_wbs > 0)
            {
                workSheet.DeleteRow(nivel_maximo_wbs + 1);
            }
            if (nivel_maximo_wbs > 0)
            {
                workSheet.DeleteRow(nivel_maximo_wbs + 2);
            }
            */

            workSheet.InsertRow(1, 4);

            workSheet.Cells[2, 3, 2, workSheet.Dimension.End.Column].Style.Border.Top.Style = ExcelBorderStyle.Medium;
            workSheet.Cells[2, 3, 2, workSheet.Dimension.End.Column].Style.Border.Top.Color.SetColor(Color.Blue);
            workSheet.Cells[2, 3, workSheet.Dimension.End.Row, 3].Style.Border.Left.Style = ExcelBorderStyle.Medium;
            workSheet.Cells[2, 3, workSheet.Dimension.End.Row, 3].Style.Border.Left.Color.SetColor(Color.Blue);
            workSheet.Cells[2, workSheet.Dimension.End.Column, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column].Style.Border.Right.Style = ExcelBorderStyle.Medium;
            workSheet.Cells[2, workSheet.Dimension.End.Column, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column].Style.Border.Right.Color.SetColor(Color.Blue);

            string nombre = this.nombreexcelofertaeconomica(oferta.Id);
            string contrato = oferta.Contrato != null && oferta.Contrato.sitio_referencia != null ? oferta.Contrato.sitio_referencia : "";
            if (contrato.Length > 0)
            {
                workSheet.Cells["D2"].Value = contrato;
            }
            else
            {
                workSheet.Cells["D2"].Value = "SITIO REFERENCIA?";
            }

            workSheet.Cells["D2"].Style.WrapText = true;
            workSheet.Cells["D2"].Style.Font.Bold = true;
            workSheet.Cells["D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["D2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells[2, workSheet.Dimension.End.Column - 2, 2, workSheet.Dimension.End.Column - 1].Merge = true;
            workSheet.Cells[2, workSheet.Dimension.End.Column - 2, 2, workSheet.Dimension.End.Column - 1].Value = oferta.codigo;
            workSheet.Cells[2, workSheet.Dimension.End.Column - 2, 2, workSheet.Dimension.End.Column - 1].Style.WrapText = true;
            workSheet.Cells[2, workSheet.Dimension.End.Column - 2, 2, workSheet.Dimension.End.Column - 1].Style.Font.Bold = true;
            workSheet.Cells[2, workSheet.Dimension.End.Column - 2, 2, workSheet.Dimension.End.Column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[2, workSheet.Dimension.End.Column - 2, 2, workSheet.Dimension.End.Column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells[2, workSheet.Dimension.End.Column].Value = "Revisión " + oferta.version;
            workSheet.Cells[2, workSheet.Dimension.End.Column].Style.WrapText = true;
            workSheet.Cells[2, workSheet.Dimension.End.Column].Style.Font.Bold = true;
            workSheet.Cells[2, workSheet.Dimension.End.Column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[2, workSheet.Dimension.End.Column].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells["D3"].Value = "Anexo-Propuesta Económica";
            workSheet.Cells["D3"].Style.WrapText = true;
            workSheet.Cells["D3"].Style.Font.Bold = true;
            workSheet.Cells["D3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["D3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells["D4"].Value = nombre;
            workSheet.Cells["D4"].Style.WrapText = true;
            workSheet.Cells["D4"].Style.Font.Bold = true;
            workSheet.Cells["D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["D4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            workSheet.Cells[workSheet.Dimension.End.Row, 3, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            workSheet.Cells[workSheet.Dimension.End.Row, 3, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column].Style.Border.Bottom.Color.SetColor(Color.Blue);

            //FONDO BLANCO
            workSheet.Cells[2, 3, 4, workSheet.Dimension.End.Column].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[2, 3, 4, workSheet.Dimension.End.Column].Style.Fill.BackgroundColor.SetColor(Color.White);

            //COLUMNAS WIDTH ULTIMAS Y PENULTIMA
            workSheet.Column(workSheet.Dimension.End.Column).Width = 20;
            workSheet.Column(workSheet.Dimension.End.Column - 1).Width = 20; //PENULTIMO



            //FORMATO A UNA PAGINA
            workSheet.View.PageBreakView = true;
            workSheet.PrinterSettings.PrintArea = workSheet.Cells[2, 3, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column];

            workSheet.PrinterSettings.Orientation = eOrientation.Landscape;

            workSheet.PrinterSettings.FitToPage = true;
            workSheet.Cells[1, 1, workSheet.Dimension.End.Row, 2].Style.Font.Color.SetColor(Color.FromArgb(159, 159, 159));

            workSheet.Cells[5, 3, workSheet.Dimension.End.Row, workSheet.Dimension.End.Column].AutoFilter = true;


            return excel;

        }

        public TotalesSegundoContrato TotalesSecondFormat(List<ComputoComercial> computos)
        {
            if (computos.Count > 0)
            {
                TotalesSegundoContrato t = new TotalesSegundoContrato();
                t.A_VALOR_COSTO_TOTAL_INGENIERÍA_BASICA_YDETALLE_AIU_ANEXO1 = (from c in computos
                                                                               where c.Item.Grupo.codigo == ProyectoCodigos.CODE_INGENIERIA
                                                                               select c.costo_total).Sum();
                t.VALOR_PROCURA = (from c in computos
                                   where c.Item.Grupo.codigo == ProyectoCodigos.CODE_PROCURA_CONTRATISTA
                                   select c.costo_total).Sum();
                t.Administracion_sobre_Procura_Contratista = (from c in computos
                                                              where c.Item.Grupo.codigo == ProyectoCodigos.CODE_PROCURA_CONTRATISTA
                                                              select c.costo_total).Sum() * Convert.ToDecimal(0.10);
                t.B_VALOR_COSTO_DIRECTO_PROCURA_CONTRATISTA_ANEXO2 = t.VALOR_PROCURA + t.Administracion_sobre_Procura_Contratista;
                t.VALOR_SUBCONTRATOS = (from c in computos
                                        where c.Item.Grupo.codigo == ProyectoCodigos.CODE_SUBCONTRATOS_CONTRATISTA
                                        select c.costo_total).Sum();
                t.Administracion_sobre_Subcontratos_Contratista = (from c in computos
                                                                   where c.Item.Grupo.codigo == ProyectoCodigos.CODE_SUBCONTRATOS_CONTRATISTA
                                                                   select c.costo_total).Sum() * Convert.ToDecimal(0.1838);

                t.C_VALOR_COSTO_DIRECTO_SUBCONTRATOS_CONTRATISTA = t.VALOR_SUBCONTRATOS + t.Administracion_sobre_Subcontratos_Contratista;
                t.VALOR_COSTO_DIRECTO_OBRAS_CIVILES = (from c in computos
                                                       where c.Item.Especialidad.codigo == ProyectoCodigos.OBRAS_CIVILES
                                                       select c.costo_total).Sum();
                t.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = (from c in computos
                                                         where c.Item.Especialidad.codigo == ProyectoCodigos.OBRAS_MECANICAS
                                                         select c.costo_total).Sum();
                t.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = (from c in computos
                                                          where c.Item.Especialidad.codigo == ProyectoCodigos.OBRAS_ELECTRICAS
                                                          select c.costo_total).Sum();
                t.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = (from c in computos
                                                                     where c.Item.Especialidad.codigo == ProyectoCodigos.OBRAS_INSTRUMENTOS_CONTROL
                                                                     select c.costo_total).Sum();
                t.VALOR_COSTO_DIRECTO_SERVICIOS_ESPECIALES = (from c in computos
                                                              where c.Item.Especialidad.codigo == ProyectoCodigos.SERVICIOS_EPECIALES
                                                              select c.costo_total).Sum();

                t.DESCUENTO_ITEMS_MECÁNICOS_ELÉCTRICOS_INSTRUMENTACIÓN = //(t.VALOR_COSTO_DIRECTO_OBRAS_CIVILES * Convert.ToDecimal(0.01))+
                                                                        (t.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * Convert.ToDecimal(0.01)) +
                                                                        (t.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * Convert.ToDecimal(0.01)) +
                                                                        (t.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * Convert.ToDecimal(0.01)) +
                                                                         (t.VALOR_COSTO_DIRECTO_SERVICIOS_ESPECIALES * Convert.ToDecimal(0.01))
                                                                        ;
                t.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN = (from c in computos
                                                        where c.Item.Grupo.codigo == ProyectoCodigos.CODE_CONSTRUCCION
                                                        select c.costo_total).Sum();
                t.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN = t.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN - t.DESCUENTO_ITEMS_MECÁNICOS_ELÉCTRICOS_INSTRUMENTACIÓN;

                t.Administracion_sobre_Obra = t.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN * Convert.ToDecimal(0.4119);
                /*(from c in computos
                where c.codigo_grupo == ProyectoCodigos.CODE_CONSTRUCCION
                select c.costo_total).Sum() * Convert.ToDecimal(0.4119);*/
                t.Imprevistos_sobre_Obra = t.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN * Convert.ToDecimal(0.03);
                /* (from c in computos
                 where c.codigo_grupo == ProyectoCodigos.CODE_CONSTRUCCION
                 select c.costo_total).Sum() * Convert.ToDecimal(0.03);*/
                t.Utilidad_sobre_Obra = t.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN * Convert.ToDecimal(0.12);
                /* (from c in computos
                                      where c.codigo_grupo == ProyectoCodigos.CODE_CONSTRUCCION
                                      select c.costo_total).Sum() * Convert.ToDecimal(0.12);*/
                t.E_INDIRECTOS_SOBRE_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN = t.Administracion_sobre_Obra + t.Imprevistos_sobre_Obra + t.Utilidad_sobre_Obra;

                t.COSTO_TOTAL_DEL_PROYECTO_ABCDE = (t.A_VALOR_COSTO_TOTAL_INGENIERÍA_BASICA_YDETALLE_AIU_ANEXO1 +
                                                  t.B_VALOR_COSTO_DIRECTO_PROCURA_CONTRATISTA_ANEXO2 +
                                                  t.C_VALOR_COSTO_DIRECTO_SUBCONTRATOS_CONTRATISTA +
                                                  t.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN +
                                                  t.E_INDIRECTOS_SOBRE_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN
                                                  );
                //- t.DESCUENTO_ITEMS_MECÁNICOS_ELÉCTRICOS_INSTRUMENTACIÓN;


                return t;

            }
            else
            {
                return new TotalesSegundoContrato();
            }
        }

        public decimal montoOfertado(int OfertaId)
        {
            decimal montoofertad = 0;
            var presupuestos = Repository.GetAllIncluding(c => c.Presupuesto)
                                         .Where(c => c.vigente)
                                         .Where(c => c.OfertaComercialId == OfertaId)
                                         .Where(c => c.PresupuestoId.HasValue)
                                         .ToList();
            if (presupuestos.Count > 0)
            {
                foreach (var p in presupuestos)
                {
                    if (p.Presupuesto != null && p.Presupuesto.Id > 0)
                    {
                        montoofertad = montoofertad + p.Presupuesto.monto_total;
                    }


                }
            }
            else {
                return -1;
            }

            return montoofertad;

        }
    }
}



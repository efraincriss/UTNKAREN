using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class ComputoPresupuestoAsyncBaseCrudAppService : AsyncBaseCrudAppService<ComputoPresupuesto, ComputoPresupuestoDto, PagedAndFilteredResultRequestDto>, IComputoPresupuestoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Item> _itemRepository;
        private readonly IBaseRepository<DetallePreciario> _repositoryDetallePreciario;
        private readonly IBaseRepository<Preciario> _repositorypreciario;
        private readonly IBaseRepository<Catalogo> _repositorycatalogo;
        private readonly IItemAsyncBaseCrudAppService _itemservice;

        private readonly IBaseRepository<Ganancia> _gananciarepository;
        public readonly IBaseRepository<DetalleGanancia> _detallegananciarepository;
        private readonly IBaseRepository<DetalleAvanceObra> _detalleavancebrarepository;
        private readonly IBaseRepository<ComputoComercial> _computocomercialrepository;

        public ComputoPresupuestoAsyncBaseCrudAppService(
            IBaseRepository<ComputoPresupuesto> repository,
            IBaseRepository<Item> itemRepository,
            IBaseRepository<DetallePreciario> repositoryDetallePreciario,
            IBaseRepository<Preciario> repositorypreciario,
            IBaseRepository<Catalogo> repositorycatalogo,
                   IBaseRepository<Ganancia> gananciarepository,
            IBaseRepository<DetalleGanancia> detallegananciarepository,
            IBaseRepository<DetalleAvanceObra> detalleavancebrarepository,
             IBaseRepository<ComputoComercial> computocomercialrepository
            ) : base(repository)
        {
            _itemRepository = itemRepository;
            _repositoryDetallePreciario = repositoryDetallePreciario;
            _repositorypreciario = repositorypreciario;
            _repositorycatalogo = repositorycatalogo;
            _detalleavancebrarepository =detalleavancebrarepository;
            _computocomercialrepository = computocomercialrepository;
            _itemservice = new ItemServiceAsyncBaseCrudAppService(itemRepository, repositoryDetallePreciario,
                repositorypreciario, repositorycatalogo, _detalleavancebrarepository, _computocomercialrepository, repository);

            _gananciarepository = gananciarepository;
            _detallegananciarepository = detallegananciarepository;
        }

        public List<ComputoPresupuestoDto> GetComputosPorPresupuesto(int id)
        {
            var listacomputos = Repository.GetAll().Where(c => c.vigente == true).Where(c=>c.WbsPresupuesto.PresupuestoId==id).ToList();
            var computos = (from c in listacomputos
                            where c.WbsPresupuesto.PresupuestoId == id
                            where c.vigente == true
                            select new ComputoPresupuestoDto
                            {
                                Id = c.Id,
                                WbsPresupuestoId = c.WbsPresupuestoId,
                                wbs_actividad = c.WbsPresupuesto.nivel_nombre,
                                ItemId = c.ItemId,
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
                                total_pu_aui = (c.cantidad * c.precio_incrementado),
                                precio_aplicarse=c.precio_aplicarse,
                                precio_base=c.precio_base,                   

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

            return computos;
        }


        public List<TreeWbsComputo> TreeComputo(int WbsPresupuestoId)
        {
            //listacomputo//
            List<ComputoPresupuestoDto> lc = this.GetComputosporWbsOferta(WbsPresupuestoId);
            var Lista = new List<TreeWbsComputo>();
            foreach (var r in lc)
            {
                string lab = "";
                string alterno = "";
                if (r.item_GrupoId == 3)
                {
                    lab = r.codigo_item_alterno + " " + r.item_nombre;
                }
                else
                {
                    lab = r.item_codigo + " " + r.item_nombre;
                }
                if (r.codigo_item_alterno != null && r.codigo_item_alterno.Length > 0)
                {
                    alterno = r.codigo_item_alterno;
                }
                else
                {
                    alterno = r.item_codigo;
                }
                var jcomputo = new TreeWbsComputo()
                {
                    key = r.Id,
                    selectable = true,
                    label = lab,
                    data = r.Id + "!" + r.cantidad + "!" + r.precio_unitario + "!" + r.costo_total + "!" +
                           r.item_padre_nombre + "!" + alterno + "!" + r.item_nombre + "!" +
                           r.fecha_registro.GetValueOrDefault().ToShortDateString() + "!" +
                           r.fecha_actualizacion.GetValueOrDefault().ToShortDateString() + "!" + r.cantidad_eac + "!" +
                           NombreCatalogo(r.item_UnidadId),
                    icon = "fa fa-fw fa-file-word-o",
                    tipo = "computo",
                    nombres = r.item_codigo+ ", " + r.item_nombre,
                    draggable = false,
                    droppable = false,
                };
                Lista.Add(jcomputo);
            }
            return Lista;
        }

        public ComputoPresupuesto editarcomputoexiste(int WbsOfertaId, int ItemId, int PresupuestoId)
        {


            var computo = Repository.GetAll()
                .Where(c => c.WbsPresupuestoId == WbsOfertaId)
                .Where(c => c.ItemId == ItemId).Where(
                c => c.vigente == true).Where(c=>c.WbsPresupuesto.PresupuestoId== PresupuestoId).
                FirstOrDefault();
            if (computo != null && computo.Id > 0)
            {
                return computo;
            }

            return new ComputoPresupuesto();
        }
        public List<ComputoPresupuestoDto> GetComputosporWbsOferta(int WbsPresupuestoId, DateTime? fecha = null) // Fecha apra 
        {
            var computosQuery = Repository.GetAll().Where(c=>c.WbsPresupuestoId== WbsPresupuestoId);
            var computos = (from c in computosQuery
                            where c.WbsPresupuestoId == WbsPresupuestoId
                            where c.vigente == true
                            select new ComputoPresupuestoDto
                            {
                                Id = c.Id,
                                WbsPresupuestoId = c.WbsPresupuestoId,
                                cantidad = c.cantidad,
                                precio_unitario = c.precio_unitario,
                                costo_total = c.costo_total,
                                estado = c.estado,
                                vigente = c.vigente,
                                //WbsPresupuesto = c.WbsPresupuesto,
                                precio_base = c.precio_base, //Nuevos//
                                precio_ajustado = c.precio_ajustado,
                                precio_aplicarse = c.precio_aplicarse,
                                precio_incrementado = c.precio_incrementado,
                                ItemId = c.ItemId,
                                //Item = c.Item,
                                item_codigo = c.Item.codigo,
                                item_nombre = c.Item.nombre,
                                item_padre_codigo=c.Item.item_padre,
                                item_UnidadId=c.Item.UnidadId,
                                item_GrupoId=c.Item.GrupoId,
                                fecha_registro = c.fecha_registro.Value,
                                fecha_actualizacion = c.fecha_actualizacion.Value,
                                cantidad_eac = c.cantidad_eac,
                                fecha_eac = c.fecha_eac,
                                codigo_item_alterno = c.codigo_item_alterno
                            }).ToList();

            foreach (var e in computos)
            {
                if (e.item_padre_codigo != null && e.item_padre_codigo.Length > 0)
                {
                    var name = _itemRepository.GetAll()
                        .Where(o => o.vigente == true)
                        .FirstOrDefault(o => o.codigo == e.item_padre_codigo);

                    if (name != null && name.Id > 0)
                    {
                        e.item_padre_nombre = name.nombre;
                        if (e.item_UnidadId > 0) {
                            e.nombre_unidad = NombreCatalogo(e.item_UnidadId);
                        }
                        
                    }
                }
            }
            return computos;
        }

        public string NombreCatalogo(int tipocatagoid)
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

        public bool EliminarVigencia(int ComputoId)
        {
            bool resul = false;
            var proyecto = Repository.Get(ComputoId);
            if (proyecto != null)
            {
                proyecto.Cambio = ComputoPresupuesto.TipoCambioComputo.Eliminado;
                proyecto.vigente = false; 
               
                Repository.InsertOrUpdate(proyecto);
                resul = true;
            }
            return resul;
        }

        public bool comprobarexistenciaitem(int WbsPresupuestoId, int ItemId)
        {
            bool encontrado = false;

            var lista = Repository.GetAll().Where(c => c.WbsPresupuestoId == WbsPresupuestoId).Where(c => c.ItemId == ItemId).Where(
                c => c.vigente == true).ToList();
            if (lista.Count > 0)
            {
                encontrado = true;
            }
            return encontrado;
        }

        public int GetComputosporOfertaProcura(int PresupuestoId)
        {
            var computosQuery = Repository.GetAllIncluding(o => o.WbsPresupuesto, o => o.Item).Where(o => o.WbsPresupuesto.PresupuestoId == PresupuestoId).
                Where(o => o.vigente == true).Where(o => o.Item.GrupoId == 3).ToList();
            return computosQuery.Count;
        }

        public bool EditarCantidadComputo(int id, decimal cantidad, decimal cantidad_eac)
        {
            var a = Repository.Get(id);
            a.cantidad = cantidad;
            a.costo_total = 0;
            if (cantidad_eac == 0)
            {
                a.cantidad_eac = cantidad;
            }
            else
            {
                a.cantidad_eac = cantidad_eac;
            }
            a.fecha_actualizacion = DateTime.Now;
            a.Cambio = ComputoPresupuesto.TipoCambioComputo.Editado;
            var resultado = Repository.Update(a);
            if (resultado.Id > 0)
            {
                return true;
            }
            return false;
        }

        public ExcelPackage GenerarExcelCabecera(PresupuestoDto oferta)
        {
            // Datos básicos del documento
            ExcelPackage excelPackage = new ExcelPackage();
            excelPackage.Workbook.Properties.Author = "CPP";
            excelPackage.Workbook.Properties.Title = "Prespuestos";
            excelPackage.Workbook.Properties.Subject = "Generación de Presupuesto";
            excelPackage.Workbook.Properties.Created = DateTime.Now;

            //Crea una hoja de trabajo
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Presupuesto");

            // Lineas de divición
            worksheet.View.ShowGridLines = false;

            // Cabecera de nombres 
            worksheet.Cells["B2:BH2"].Style.Font.SetFromFont(new System.Drawing.Font("Arial", 12, FontStyle.Bold));
            worksheet.Cells["B2:BH2"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            worksheet.Cells["B2:BH2"].Style.Border.Bottom.Color.SetColor(Color.Blue);


            worksheet.Cells["B3:B1569"].Style.Font.SetFromFont(new System.Drawing.Font("Arial", 12, FontStyle.Bold));
            worksheet.Cells["B3:B1569"].Style.Border.Left.Style = ExcelBorderStyle.Medium;
            worksheet.Cells["B3:B1569"].Style.Border.Left.Color.SetColor(Color.Blue);
            worksheet.Cells["BH3:BH1569"].Style.Font.SetFromFont(new System.Drawing.Font("Arial", 12, FontStyle.Bold));
            worksheet.Cells["BH3:BH1569"].Style.Border.Right.Style = ExcelBorderStyle.Medium;
            worksheet.Cells["BH3:BH569"].Style.Border.Right.Color.SetColor(Color.Blue);

            worksheet.Cells["B4:D4"].Merge = true;
            worksheet.Cells["B4:D4"].Value =
                " Locación Proyecto #: " + this.nombrecatalogo2(oferta.Proyecto.LocacionId);
            worksheet.Cells["B4:D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["B4:D4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["B4:D4"].Style.Font.Size = 12;

            worksheet.Cells["C5:D5"].Merge = true;
            worksheet.Cells["C5:D5"].Value = "(" + oferta.Proyecto.codigo + ") " +
                                             oferta.Proyecto.descripcion_proyecto;
            worksheet.Cells["C5:D5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["C5:D5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["C5:D5"].Style.Font.Size = 12;
            worksheet.Cells["BD5:BF5"].Merge = true;
            worksheet.Cells["BD5:BF5"].Value = oferta.codigo;
            worksheet.Cells["BD5:BF5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["BD5:BF5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["BD5:BF5"].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
            worksheet.Cells["BG5:BH5"].Merge = true;
            worksheet.Cells["BG5:BH5"].Value = "Revisión " + oferta.version;
            worksheet.Cells["BG5:BH5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["BG5:BH5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["BG5:BH5"].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
            return excelPackage;
        }

        public string nombrecatalogo2(int tipocatagoid)
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


        public string ActualizarCostoTotal(int ofertaid, int ContratoId, int PreciarioId, DateTime FechaOferta, bool Validado)
        {

            String resultado = "";
            //

            decimal gananciacontruccion = 1;
            decimal gananciaprocura = 1;
            if (Validado)
            {

                /* Sección Obtención Ganancias*/
                Ganancia ganancia = null;
                var existeGananciaSNFecha = _gananciarepository.GetAll().Where(c => c.ContratoId == ContratoId)
                                                             .Where(c => !c.fecha_fin.HasValue)
                                                             .Where(c=>c.vigente)
                                                             .Where(c=>FechaOferta>=c.fecha_inicio)
                                                             .FirstOrDefault();
                if (existeGananciaSNFecha != null)
                {
                    ganancia = existeGananciaSNFecha;
                }
                else
                {
                   
                   var gananciaConFechas = _gananciarepository.GetAll().Where(c => c.ContratoId == ContratoId)
                        .Where(c => c.fecha_inicio <= FechaOferta)
                        .Where(c => c.fecha_fin >= FechaOferta).Where(c => c.vigente == true).FirstOrDefault();

                    if (gananciaConFechas != null) {
                        ganancia = gananciaConFechas;
                    }

                }
                /* FN*/
                
               


                if (ganancia != null && ganancia.Id > 0)
                {
                    var detalleganancia = _detallegananciarepository.GetAll().Where(c => c.GananciaId == ganancia.Id).
                         Where(c => c.vigente == true).ToList();

                    if (detalleganancia.Count > 0)
                    {

                        gananciacontruccion = (from e in detalleganancia where e.GrupoItemId == 2 select e.valor).Sum();
                        gananciaprocura = (from e in detalleganancia where e.GrupoItemId == 3 select e.valor).Sum();

                    }

                }


                var computos = this.GetComputosPorPresupuesto(ofertaid);

                if (computos.Count > 0 && PreciarioId > 0)
                {

                    //detallespreciario

                    var itemspreciario = _repositoryDetallePreciario.GetAll().Where(c => c.PreciarioId == PreciarioId).Where(e => e.vigente == true).ToList();

                    foreach (var actual in computos)
                    {
                        var item = Repository.Get(actual.Id);

                        var preciounitario = (from e in itemspreciario
                                              where e.ItemId == item.ItemId
                                              where e.vigente == true
                                              select e.precio_unitario).FirstOrDefault();

                        if (preciounitario > 0)
                        {




                            item.precio_base = preciounitario;

                            if (item.Item.GrupoId == 1)
                            {

                            }

                            if (item.Item.GrupoId == 2)
                            {
                                if (gananciacontruccion > 1)
                                {
                                    item.precio_incrementado = item.precio_unitario * (gananciacontruccion / 100);
                                }
                            }

                            if (item.Item.GrupoId == 3)
                            {
                                if (gananciaprocura > 1)
                                {
                                    item.precio_incrementado = item.precio_unitario * (gananciaprocura / 100);
                                }
                            }

                            Decimal costototal = 0;

                            if (item.precio_ajustado > 0)
                            {

                                costototal = item.cantidad * item.precio_ajustado;
                                item.precio_unitario = item.precio_ajustado;
                                item.precio_aplicarse = "precio ajus";
                            }
                            else
                            {
                                costototal = item.cantidad * item.precio_base; // multiplicas por precio ba
                                //costototal = icomp.cantidad * icomp.precio_incrementado;
                                item.precio_aplicarse = "precio base";
                                item.precio_unitario = item.precio_base;
                            }

                            item.costo_total = costototal;
                            item.fecha_registro = item.fecha_registro;
                            item.fecha_actualizacion = item.fecha_actualizacion;
                            item.precio_incrementado = item.precio_incrementado;
                            Repository.Update(item);

                        }

                        else
                        {
                            resultado = "EL precio unitario del Item: " + item.Item.codigo + " " +
                                        item.Item.nombre +
                                        " es cero verifique su preciario o incluya un precio ajustado";

                        }
                    }

                }

            }

            return resultado;

        }


        public decimal sumacantidades(int OfertaId, int ItemId)
        {

            var listacomputos = Repository.GetAllIncluding(c => c.WbsPresupuesto.Presupuesto.Proyecto, c => c.Item)
                .Where(c => c.vigente == true).Where(c => c.WbsPresupuesto.PresupuestoId == OfertaId).Where(c => c.ItemId == ItemId);
            var items = listacomputos.Select(c => c.cantidad).Sum();
            return items;

        }

        public decimal MontoPresupuestoIngenieria(int OfertaId)
        {
            decimal montoingenieria = 0;
            var listacomputos = Repository.GetAllIncluding(c => c.WbsPresupuesto.Presupuesto, c => c.Item)
                .Where(c => c.vigente == true).Where(c => c.WbsPresupuesto.PresupuestoId == OfertaId).Where(c => c.Item.GrupoId == 1)
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
            var listacomputos = Repository.GetAllIncluding(c => c.WbsPresupuesto.Presupuesto, c => c.Item)
                .Where(c => c.vigente == true).Where(c => c.WbsPresupuesto.PresupuestoId == OfertaId).Where(c => c.Item.GrupoId == 2)
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
            var listacomputos = Repository.GetAllIncluding(c => c.WbsPresupuesto.Presupuesto, c => c.Item)
                .Where(c => c.vigente == true).Where(c => c.WbsPresupuesto.PresupuestoId == OfertaId).Where(c => c.Item.GrupoId == 3)
                .ToList();

            if (listacomputos != null && listacomputos.Count >= 0)
            {
                montoingenieria = (from x in listacomputos select x.costo_total).Sum(); //Presupuesto Total

            }

            return montoingenieria;
        }

        public ComputoPresupuesto ActualizarprecioAjustado(ComputoPresupuesto seleccionado)
        {
            ComputoPresupuesto actual = Repository.Get(seleccionado.Id);
           
            if (seleccionado.precio_ajustado > 0)
            {

                Decimal precioaj = seleccionado.precio_ajustado;
                actual.precio_unitario = precioaj;
               
                actual.precio_aplicarse = "precio ajus";
                actual.costo_total = actual.cantidad * precioaj;
                actual.precio_ajustado =precioaj;
                Repository.Update(actual);

            }
            else if (seleccionado.precio_ajustado <= 0)
            {
                if (actual.precio_base > 0)
                {
                    actual.precio_aplicarse = "precio base";
                }
                else
                {
                    actual.precio_aplicarse = "";

                }

                actual.precio_unitario = actual.precio_base;
                actual.costo_total = actual.precio_unitario* actual.precio_base;
                Repository.Update(actual);
            }
         
            return actual;
        }
    }
}


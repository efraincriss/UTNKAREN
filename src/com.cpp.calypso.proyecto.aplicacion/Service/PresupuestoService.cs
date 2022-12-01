using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Abp.Domain.Uow;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.framework;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class PresupuestoAsyncBaseCrudAppService : AsyncBaseCrudAppService<Presupuesto, PresupuestoDto,
        PagedAndFilteredResultRequestDto>, IPresupuestoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Proyecto> _repositoryProyecto;
        private readonly IBaseRepository<Requerimiento> _repositoryRequerimiento;
        private readonly IBaseRepository<Oferta> _repositpryOferta;
        private readonly IBaseRepository<WbsPresupuesto> _respositoryWbsPresupuesto;
        private readonly IBaseRepository<ComputoPresupuesto> _repositoryComputoPresupuesto;
        private readonly IBaseRepository<OfertaComercialPresupuesto> _ofertaComercialPresupuestoRepo;
        private readonly IBaseRepository<Wbs> _repositoryWbs;
        private readonly IBaseRepository<Computo> _repositoryComputo;
        private readonly IItemAsyncBaseCrudAppService _itemservice;

        private readonly IBaseRepository<Catalogo> _repositorycatalogo;
        private readonly IBaseRepository<Preciario> _repositorypreciario;
        private readonly IBaseRepository<DetallePreciario> _repositorydetallepreciario;
        public readonly IBaseRepository<Item> itemrepository;
        private readonly IDetallePreciarioAsyncBaseCrudAppService detallepreciarioService;

        //
        private readonly IBaseRepository<Ganancia> _gananciarepository;
        private readonly IBaseRepository<DetalleGanancia> _detallegananciarepository;
        private readonly IBaseRepository<DetalleAvanceObra> _detalleavancebrarepository;
        private readonly IBaseRepository<ComputoComercial> _computocomercialrepository;


        private readonly IBaseRepository<ArchivoPresupuesto> _archivoPresupuesto;
        public PresupuestoAsyncBaseCrudAppService(
            IBaseRepository<Presupuesto> repository,
            IBaseRepository<Proyecto> repositoryProyecto,
            IBaseRepository<Requerimiento> repositoryRequerimiento,
            IBaseRepository<Oferta> repositpryOferta,
            IBaseRepository<WbsPresupuesto> respositoryWbsPresupuesto,
            IBaseRepository<ComputoPresupuesto> repositoryComputoPresupuesto,
            IBaseRepository<Wbs> repositoryWbs,
            IBaseRepository<Computo> repositoryComputo,
            IBaseRepository<Item> itemrepository,
                IBaseRepository<Catalogo> repositorycatalogo,
            IBaseRepository<DetallePreciario> repositorydetallepreciario,
                    IBaseRepository<Preciario> repositorypreciario,

                //
                IBaseRepository<Ganancia> gananciarepository,
                IBaseRepository<DetalleGanancia> detallegananciarepository,
                IBaseRepository<DetalleAvanceObra> detalleavancebrarepository,
                IBaseRepository<ComputoComercial> computocomercialrepository,
                IBaseRepository<OfertaComercialPresupuesto> ofertaComercialPresupuestoRepo,
                 IBaseRepository<ArchivoPresupuesto> archivoPresupuesto
            ) : base(repository)
        {
            _repositoryProyecto = repositoryProyecto;
            _repositoryRequerimiento = repositoryRequerimiento;
            _repositpryOferta = repositpryOferta;
            _respositoryWbsPresupuesto = respositoryWbsPresupuesto;
            _repositoryComputoPresupuesto = repositoryComputoPresupuesto;
            _repositoryWbs = repositoryWbs;
            _detalleavancebrarepository = detalleavancebrarepository;
            _ofertaComercialPresupuestoRepo = ofertaComercialPresupuestoRepo;

            _repositoryComputo = repositoryComputo;
            _computocomercialrepository = computocomercialrepository;
            _itemservice = new ItemServiceAsyncBaseCrudAppService(itemrepository, repositorydetallepreciario,
               repositorypreciario, repositorycatalogo, detalleavancebrarepository, _computocomercialrepository, _repositoryComputoPresupuesto);
            detallepreciarioService =
               new DetallePreciarioServiceAsyncBaseCrudAppService(repositorydetallepreciario, repositorypreciario, itemrepository, repositorycatalogo);

            _gananciarepository = gananciarepository;
            _detallegananciarepository = detallegananciarepository;
            this.itemrepository = itemrepository;
            _repositorydetallepreciario = repositorydetallepreciario;
            _repositorycatalogo = repositorycatalogo;
            _repositorypreciario = repositorypreciario;
            _archivoPresupuesto = archivoPresupuesto;
        }

        public int SecuencialPresupuesto(int ProyectoId)
        {
            var proyecto = _repositoryProyecto.Get(ProyectoId);
            var query = Repository
                .GetAll()
                .Where(o => o.vigente).Count(o => o.Proyecto.contratoId == proyecto.contratoId);
            query++;
            return query;
        }

        public async Task<int> CrearPresupuesto(PresupuestoDto presupuesto)
        {
            var p = MapToEntity(presupuesto);
            p.estado_aprobacion = Presupuesto.EstadoAprobacion.PendienteAprobacion;
            p.estado_emision = Presupuesto.EstadoEmision.EnPreparacion;
            p.version = "A";
            p.vigente = true;
            var requerimiento = _repositoryRequerimiento.Get(presupuesto.RequerimientoId);
            p.ProyectoId = requerimiento.ProyectoId;
            var cod = String.Format("{0:0000}", this.SecuencialPresupuesto(requerimiento.ProyectoId));
            p.codigo = cod;


            var queryDefinitiva = Repository
                .GetAll()
                .Where(o => o.vigente)
                .Where(o => o.RequerimientoId == presupuesto.RequerimientoId)
                .Count(o => o.es_final);

            p.es_final = queryDefinitiva <= 0;

            var id = await Repository.InsertAndGetIdAsync(p);
            return id;
        }

        public PresupuestoDto DetallePresupuestoConEnumerable(int PresupuestoId)
        {
            var presupuesto = Repository.Get(PresupuestoId);
            var dto = MapToEntityDto(presupuesto);
            dto.NombreEstadoAprobacion = dto.GetDisplayName(dto.estado_aprobacion);
            dto.NombreEstadoEmision = dto.GetDisplayName(dto.estado_emision);
            dto.NombreClase = dto.GetDisplayName(dto.Clase.GetValueOrDefault());
            return dto;
        }

        public List<PresupuestoDto> ListarPorRequerimiento(int RequerimientoId)
        {
            var query = Repository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.RequerimientoId == RequerimientoId)
                .OrderByDescending(o => o.fecha_registro);

            var lista = (from p in query
                         select new PresupuestoDto()
                         {
                             Id = p.Id,
                             Proyecto = p.Proyecto,
                             codigo = p.codigo,
                             alcance = p.alcance,
                             descripcion = p.descripcion,
                             version = p.version,
                             fecha_registro = p.fecha_registro,
                             es_final = p.es_final,

                         }).ToList();

            foreach (var p in lista)
            {
                p.NombreEstadoAprobacion = p.GetDisplayName(p.estado_aprobacion);
                p.NombreEstadoEmision = p.GetDisplayName(p.estado_emision);
                p.NombreClase = p.GetDisplayName(p.Clase.GetValueOrDefault());
            }

            return lista;
        }

        public async Task ActualizarPresupuesto(PresupuestoDto p)
        {
            var oferta = Repository.Get(p.Id);
            oferta.Clase = p.Clase;
            oferta.descripcion = p.descripcion;
            oferta.alcance = p.alcance;
            oferta.descuento = p.descuento;
            oferta.justificacion_descuento = p.justificacion_descuento;
            oferta.fecha_registro = p.fecha_registro;
            await Repository.UpdateAsync(oferta);
        }
        public async Task ActualizarPresupuestoEmail(PresupuestoDto p)
        {
            var oferta = Repository.Get(p.Id);
            oferta.asuntoCorreo = p.asuntoCorreo;
            oferta.descripcionCorreo = p.descripcionCorreo;
            await Repository.UpdateAsync(oferta);
        }

        public async Task<bool> AprobarPresupuesto(int PresupuestoId)
        {
            var presupuesto = Repository.Get(PresupuestoId);
            if (presupuesto == null) return false;

            var definitivo = Repository
                .GetAll()
                .Where(o => o.vigente)
                .Where(o => o.RequerimientoId == presupuesto.RequerimientoId)
                .FirstOrDefault(o => o.es_final);

            if (definitivo != null)
            {
                definitivo.es_final = false;

                // Cambio todo los computos Cambio Null

                await Repository.UpdateAsync(definitivo);
            }


            presupuesto.estado_aprobacion = Presupuesto.EstadoAprobacion.Aprobado;
            presupuesto.es_final = true;
            presupuesto.fecha_actualizacion = DateTime.Now;
            await Repository.UpdateAsync(presupuesto);
            return true;
        }

        public async Task<bool> DesaprobarPresupuesto(int PresupuestoId)
        {
            var presupuesto = Repository.Get(PresupuestoId);
            if (presupuesto == null) return false;
            presupuesto.estado_aprobacion = Presupuesto.EstadoAprobacion.PendienteAprobacion;
            presupuesto.es_final = false;

            await Repository.UpdateAsync(presupuesto);
            return true;
        }

        public PresupuestoDto ObtenerPresupuestoDefinitivo(int RequerimientoId)
        {
            var presupuesto = Repository
                .GetAll()
                .Where(o => o.vigente)
                .Where(o => o.es_final)
                .FirstOrDefault(o => o.RequerimientoId == RequerimientoId);

            return MapToEntityDto(presupuesto);
        }

        public string CrearNuevaVersion(PresupuestoDto pre)
        {
            var ultimo = Repository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.RequerimientoId == pre.RequerimientoId)
                .OrderByDescending(o => o.version)
                .FirstOrDefault();
            if (ultimo == null) return "NO_HAY_PRESUPUESTO";

            var OfertaDefinitiva = _repositpryOferta.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.RequerimientoId == pre.RequerimientoId)
                .FirstOrDefault(o => o.es_final);

            if (OfertaDefinitiva == null) return "NO_EXISTE_RDO_DEFINITIVO";

            var requerimiento = _repositoryRequerimiento.Get(pre.RequerimientoId);

            //var version = ultimo.version.ToCharArray()[0]++;
            var version = ultimo.version[0];
            version++;

            var cod = String.Format("{0:0000}", this.SecuencialPresupuesto(requerimiento.ProyectoId));
            var presupuesto = new Presupuesto()
            {
                OrigenDeDatosId = pre.OrigenDatosId,
                Clase = pre.Clase,
                descripcion = pre.descripcion,
                fecha_registro = pre.fecha_registro,
                ProyectoId = requerimiento.ProyectoId,
                alcance = pre.alcance,
                monto_ingenieria = pre.monto_ingenieria,
                monto_construccion = pre.monto_construccion,
                monto_suministros = pre.monto_suministros,
                version = version.ToString(),
                es_final = false,
                vigente = true,
                estado_aprobacion = Presupuesto.EstadoAprobacion.PendienteAprobacion,
                estado_emision = Presupuesto.EstadoEmision.EnPreparacion,
                RequerimientoId = pre.RequerimientoId,
                codigo = cod,
                origen = pre.origen,
            };

            var id = Repository.InsertAndGetId(presupuesto);


            // Cambiar Estado Requerimiento
            var p = Repository.Get(id);
            var x = _repositoryRequerimiento.Get(p.RequerimientoId);

            x.estado_presupuesto = 4179;
            x.ultima_version = p.version;
            x.ultimo_origen = _repositorycatalogo.Get(p.origen.GetValueOrDefault()).nombre.ToString();
            x.ultima_clase = p.Clase.GetValueOrDefault().ToString();
            var resultado = _repositoryRequerimiento.Update(x);

            ///
            var wbsQuery = _repositoryWbs.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.OfertaId == OfertaDefinitiva.Id);
            var wbs = (from w in wbsQuery
                       select new WbsPresupuestoDto()
                       {
                           PresupuestoId = id,
                           es_actividad = w.es_actividad,
                           estado = w.estado,
                           observaciones = w.observaciones,
                           vigente = w.vigente,
                           Id = w.Id,
                           fecha_final = w.fecha_final,
                           fecha_inicial = w.fecha_inicial,
                           id_nivel_codigo = w.id_nivel_codigo,
                           id_nivel_padre_codigo = w.id_nivel_padre_codigo,
                           nivel_nombre = w.nivel_nombre,
                           WbsId = w.Id,
                       }).ToList();

            foreach (var w in wbs)
            {
                var newWbs = _respositoryWbsPresupuesto.InsertAndGetId(Mapper.Map<WbsPresupuesto>(w));
                var queryComputo = _repositoryComputo.GetAll()
                    .Where(o => o.vigente)
                    .Where(o => o.WbsId == w.Id);
                var computos = (from c in queryComputo
                                select new ComputoPresupuestoDto()
                                {
                                    ItemId = c.ItemId,
                                    WbsPresupuestoId = newWbs,
                                    cantidad = c.cantidad_eac,
                                    costo_total = c.costo_total,
                                    estado = c.estado,
                                    precio_unitario = c.precio_unitario,
                                    fecha_registro = c.fecha_registro,
                                    fecha_actualizacion = c.fecha_actualizacion,
                                    vigente = c.vigente,
                                    ComputoId = c.Id,
                                    cantidad_eac = c.cantidad_eac,
                                    Cambio = null,
                                    //

                                }).ToList();
                foreach (var c in computos)
                {
                    _repositoryComputoPresupuesto.Insert(Mapper.Map<ComputoPresupuesto>(c));
                }
            }

            return "Ok";
        }


        public ExcelPackage GenerarExcelCarga(PresupuestoDto oferta, int nivel_maximo)
        {
            //Lista Wbs
            var estructurawbs = this.EstructuraWbs(oferta.Id);

            //

            var listacomputos = _repositoryComputoPresupuesto
                                .GetAllIncluding(x => x.WbsPresupuesto.Presupuesto.Proyecto, x => x.Item.Grupo)
                                .Where(x => x.vigente == true)
                                 .Where(x => x.WbsPresupuesto.PresupuestoId == oferta.Id)
                                 .Where(x => x.WbsPresupuesto.vigente)
                                .ToList();

            var computos = (from z in listacomputos
                            where z.WbsPresupuesto.PresupuestoId == oferta.Id && z.vigente == true
                            select new ComputoPresupuestoDto
                            {
                                Id = z.Id,
                                ItemId = z.ItemId,
                                WbsPresupuestoId = z.WbsPresupuestoId,
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
                                Cambio = z.Cambio,
                                codigo_item_alterno = z.codigo_item_alterno


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

            //var items = _itemservice.ArbolWbsExcel(oferta.Proyecto.contratoId, oferta.fecha_registro.GetValueOrDefault()); 
            // var items = _itemservice.ArbolWbsExcelPresupuesto(oferta.Proyecto.contratoId, oferta.fecha_registro.GetValueOrDefault());
            var items = _itemservice.EstructuraItems(oferta.Proyecto.contratoId, oferta.fecha_registro.GetValueOrDefault());
            //var itemsprocura = _itemservice.ObtenerItemsProcura();'

            var procuraitems = new List<Item>();
            var subcontratositems = new List<Item>();
            var listprocura = (from p in computos where p.Item.Grupo.codigo == ProyectoCodigos.CODE_PROCURA_CONTRATISTA select p.Item).Distinct().ToList();
            if (listprocura.Count > 0)
            {
                procuraitems = (from e in listprocura
                                orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                select e).ToList();
            }

            var listsubcontratos = (from p in computos where p.Item.Grupo.codigo == ProyectoCodigos.CODE_SUBCONTRATOS_CONTRATISTA select p.Item).Distinct().ToList();
            if (listsubcontratos.Count > 0)
            {
                subcontratositems = (from e in listsubcontratos
                                     orderby Convert.ToInt32(e.codigo.ToUpper().Replace("X", "").Replace("A", "").Replace(".", ""))
                                     select e).ToList();
            }

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Cómputos 1-6");
            var workSheetp = excel.Workbook.Worksheets.Add("Cómputos Procura");


            //PRIMERA HOJA
            workSheet.TabColor = System.Drawing.Color.Navy;
            workSheet.DefaultRowHeight = 12;
            workSheet.View.ZoomScale = 80;

            //SEGUNDA HOJA
            workSheetp.TabColor = System.Drawing.Color.Yellow;
            workSheetp.DefaultRowHeight = 12;
            workSheetp.View.ZoomScale = 80;


            int row = nivel_maximo;
            for (int i = 1; i <= row; i++)
            {
                //PRIMERA HOJA
                workSheet.Row(i).Style.Font.Bold = true;

                //SEGUNDA HOJA
                workSheetp.Row(i).Style.Font.Bold = true;
            }

            int columna = 6;

            foreach (var itemswbs in estructurawbs.Where(x => x.es_actividad).ToList())
            {


                int fila = nivel_maximo + 4;
                List<WbsPresupuesto> Jerarquia = new List<WbsPresupuesto>();
                WbsPresupuesto item = _respositoryWbsPresupuesto.Get(itemswbs.Id);
                Jerarquia.Add(item);
                while (item.id_nivel_padre_codigo != ".")
                {
                    //
                    item = (from x in estructurawbs
                            where x.id_nivel_codigo == item.id_nivel_padre_codigo
                            where x.vigente
                            where x.PresupuestoId == item.PresupuestoId
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
                        //PRIMERA HOJA 
                        workSheet.Cells[rowtyle, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[rowtyle, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                        workSheet.Cells[rowtyle, columna].Style.Border.Left.Style =
                        workSheet.Cells[rowtyle, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                        //SEGUNDA HOJA

                        workSheetp.Cells[rowtyle, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheetp.Cells[rowtyle, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                        workSheetp.Cells[rowtyle, columna].Style.Border.Left.Style =
                        workSheetp.Cells[rowtyle, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;

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

                            //SEGUNDA HOJA

                            workSheetp.Cells[fila - 4, columna].Value = wbsj.nivel_nombre;
                            workSheetp.Cells[fila - 4, columna].Value = wbsj.nivel_nombre;
                            workSheetp.Column(columna).Width = 25;
                            workSheetp.Cells[fila - 4, columna].Style.WrapText = true;
                            workSheetp.Cells[fila - 4, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheetp.Cells[fila - 4, columna].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheetp.Cells[fila - 4, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheetp.Cells[fila - 4, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            workSheetp.Cells[fila - 4, columna].Style.Border.Top.Style =
                            workSheetp.Cells[fila - 4, columna].Style.Border.Left.Style =
                            workSheetp.Cells[fila - 4, columna].Style.Border.Right.Style =
                            workSheetp.Cells[fila - 4, columna].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                            workSheetp.Cells[fila - 2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheetp.Cells[fila - 2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        }
                        else
                        {
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

                            //PRIMERA HOJA
                            workSheet.Cells[a, columna].Value = wbsj.nivel_nombre;
                            workSheet.Cells[a, columna].Style.WrapText = true;
                            workSheet.Cells[a, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheet.Cells[a, columna].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheet.Cells[a, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheet.Cells[a, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                            workSheet.Cells[a, columna].Style.Border.Left.Style =
                            workSheet.Cells[a, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                            //SEGUNDA HOJA
                            workSheetp.Cells[a, columna].Value = wbsj.nivel_nombre;
                            workSheetp.Cells[a, columna].Style.WrapText = true;
                            workSheetp.Cells[a, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            workSheetp.Cells[a, columna].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            workSheetp.Cells[a, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            workSheetp.Cells[a, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                            workSheetp.Cells[a, columna].Style.Border.Left.Style =
                            workSheetp.Cells[a, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                        }
                    }

                    a--;
                }
                //PRIMERA HOJA
                workSheet.Row(fila - 3).Hidden = true;
                workSheet.Cells[fila - 3, columna].Value = itemswbs.Id;
                workSheet.Cells[fila - 3, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[fila - 3, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                workSheet.Cells[fila - 3, columna].Style.Border.Left.Style =
                workSheet.Cells[fila - 3, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;

                //SEGUNDA HOJA

                workSheetp.Row(fila - 3).Hidden = true;
                workSheetp.Cells[fila - 3, columna].Value = itemswbs.Id;
                workSheetp.Cells[fila - 3, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheetp.Cells[fila - 3, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                workSheetp.Cells[fila - 3, columna].Style.Border.Left.Style =
                workSheetp.Cells[fila - 3, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;


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

                    //SEGUNDA HOJA
                    workSheetp.Cells[fila - 2, columna].Value = itemswbs.Catalogo.nombre;
                    workSheetp.Cells[fila - 2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheetp.Cells[fila - 2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                    workSheetp.Cells[fila - 2, columna].Style.Border.Left.Style =
                    workSheetp.Cells[fila - 2, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    workSheetp.Cells[fila - 2, columna].Style.Border.Top.Style =
                    workSheetp.Cells[fila - 2, columna].Style.Border.Left.Style =
                    workSheetp.Cells[fila - 2, columna].Style.Border.Right.Style =
                    workSheetp.Cells[fila - 2, columna].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                }
                else
                {
                    //PRIMERA HOJA
                    workSheet.Row(fila - 2).Hidden = true;

                    //SEGUNDA HOJA
                    workSheetp.Row(fila - 2).Hidden = true;
                }

                columna = columna + 1;
            }

            //INICIO CUERPO

            //PRIMERA HOJA
            workSheet.Cells[nivel_maximo, 1].Value = "Para Oferta";
            workSheet.Cells[nivel_maximo, 2].Value = "Id";
            workSheet.Cells[nivel_maximo, 3].Value = "ITEM";
            workSheet.Cells[nivel_maximo, 4].Value = "DESCRIPCIÓN";
            workSheet.Cells[nivel_maximo, 5].Value = "UNIDAD";

            //SEGUNDA HOJA
            workSheetp.Cells[nivel_maximo, 1].Value = "Para Oferta";
            workSheetp.Cells[nivel_maximo, 2].Value = "Id";
            workSheetp.Cells[nivel_maximo, 3].Value = "CÓDIGO ALTERNO";
            workSheetp.Cells[nivel_maximo, 4].Value = "DESCRIPCIÓN";
            workSheetp.Cells[nivel_maximo, 5].Value = "UNIDAD";


            string rango = workSheet.Cells[nivel_maximo, 2].Address + ":" +
                           workSheet.Cells[nivel_maximo, 5].Address;

            string rangop = workSheetp.Cells[nivel_maximo, 2].Address + ":" +
                                workSheetp.Cells[nivel_maximo, 5].Address;

            //PRIMERA HOJA
            workSheet.Cells[rango].AutoFilter = true;
            //SEGUNDA HOJA

            workSheetp.Cells[rangop].AutoFilter = true;


            for (int i = 2; i <= 5; i++)
            {

                //PRIMERA HOJA
                workSheet.Cells[nivel_maximo, i].Style.WrapText = true;
                workSheet.Cells[nivel_maximo, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[nivel_maximo, i].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[nivel_maximo + 2, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[nivel_maximo + 2, i].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheet.Cells[nivel_maximo, i].Style.Border.Top.Style =
                workSheet.Cells[nivel_maximo, i].Style.Border.Left.Style =
                workSheet.Cells[nivel_maximo, i].Style.Border.Right.Style =
                workSheet.Cells[nivel_maximo, i].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                workSheet.Cells[nivel_maximo, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[nivel_maximo, i].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                //SEGUNDA HOJA

                workSheetp.Cells[nivel_maximo, i].Style.WrapText = true;
                workSheetp.Cells[nivel_maximo, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheetp.Cells[nivel_maximo, i].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheetp.Cells[nivel_maximo + 2, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheetp.Cells[nivel_maximo + 2, i].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                workSheetp.Cells[nivel_maximo, i].Style.Border.Top.Style =
                workSheetp.Cells[nivel_maximo, i].Style.Border.Left.Style =
                workSheetp.Cells[nivel_maximo, i].Style.Border.Right.Style =
                workSheetp.Cells[nivel_maximo, i].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                workSheetp.Cells[nivel_maximo, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheetp.Cells[nivel_maximo, i].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            //PRIMERA HOJA
            workSheet.Column(3).Width = 18;
            workSheet.Column(4).Width = 80;
            workSheet.Column(5).Width = 18;
            workSheet.Column(1).Style.Font.Bold = true;
            workSheet.Column(1).Hidden = true;
            workSheet.Column(2).Hidden = true;
            //SEGUNDA HOJA
            workSheetp.Column(3).Width = 18;
            workSheetp.Column(4).Width = 80;
            workSheetp.Column(5).Width = 18;
            workSheetp.Column(1).Style.Font.Bold = true;
            workSheetp.Column(1).Hidden = true;
            workSheetp.Column(2).Hidden = true;


            //INICIO DE FILA PRIMERA HOJA
            //int c = 5;
            int c = nivel_maximo + 3;
            workSheet.View.FreezePanes(6, 1);
            workSheet.View.FreezePanes(c, 6);



            //INICIO DE FILA SEGUNDA HOJA HOJA
            int c2 = nivel_maximo + 3;
            workSheetp.View.FreezePanes(6, 1);
            workSheetp.View.FreezePanes(c2, 6);

            //PRIMERA HOJA 1-6
            foreach (var pitem in items)
            {

                workSheet.Cells[c, 1].Value = pitem.para_oferta;
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

            //SEGUNDA HOJA


            //SEGUNDA HOJA PROCURA PRECIO


            var ncolp = workSheetp.Dimension.End.Column; //columnas procura

            workSheetp.Column(ncolp + 1).Width = 15;
            workSheetp.Cells[nivel_maximo, ncolp + 1].Value = "PRECIO UNITARIO";
            workSheetp.Cells[nivel_maximo, ncolp + 1].Style.WrapText = true;
            workSheetp.Cells[nivel_maximo, ncolp + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheetp.Cells[nivel_maximo, ncolp + 1].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
            workSheetp.Cells[nivel_maximo, ncolp + 1].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            workSheetp.Cells[nivel_maximo, ncolp + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheetp.Cells[nivel_maximo, ncolp + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            ////////////////////////////////////////////////////////////////////////////////////////////////////

            excel.Workbook.Worksheets.Add("SubContratos", excel.Workbook.Worksheets[2]);

            var hojasub = excel.Workbook.Worksheets[3];

            //SEGUNDA HOJA
            foreach (var pitem in procuraitems) //items procura
            {

                workSheetp.Cells[c2, 1].Value = pitem.para_oferta;
                workSheetp.Cells[c2, 2].Value = pitem.Id;
                // workSheetp.Cells[c2, 3].Value = pitem.codigo;
                workSheetp.Cells[c2, 4].Value = pitem.nombre;
                workSheetp.Cells[c2, 4].Style.WrapText = true;
                if (pitem.UnidadId != 0)
                {
                    workSheetp.Cells[c2, 5].Value = this.nombrecatalogo(pitem.UnidadId);

                }

                if (pitem.item_padre == ".")
                {
                    int padre = workSheetp.Dimension.End.Column;
                    while (padre > 0)
                    {
                        workSheetp.Cells[c2, padre].Style.Font.Bold = true;
                        workSheetp.Cells[c2, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheetp.Cells[c2, padre].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        workSheetp.Cells[c2, padre].Style.Border.Top.Style =
                        workSheetp.Cells[c2, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        padre--;

                    }

                }
                if (pitem.item_padre != "." && pitem.para_oferta == false)
                {
                    int padre = workSheetp.Dimension.End.Column;
                    while (padre > 0)
                    {
                        workSheetp.Cells[c2, padre].Style.Font.Bold = true;
                        workSheetp.Cells[c2, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheetp.Cells[c2, padre].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                        workSheetp.Cells[c2, padre].Style.Border.Top.Style =
                        workSheetp.Cells[c2, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        padre--;

                    }

                }

                workSheetp.Cells[c2, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                workSheetp.Cells[c2, 2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                workSheetp.Cells[c2, workSheetp.Dimension.End.Column].Style.Border.Right.Style =
                workSheetp.Cells[c2, workSheetp.Dimension.End.Column].Style.Border.Left.Style = ExcelBorderStyle.Medium;




                workSheetp.Row(c2).Style.Locked = false;
                c2 = c2 + 1;
            }
            var nrowp = workSheetp.Dimension.End.Row;
            //SEGUNDA HOJA PROCURA
            for (int j = 6; j <= ncolp; j++)
            {
                var wbsid = Convert.ToInt32((workSheetp.Cells[nivel_maximo + 1, j].Value ?? "0").ToString());
                for (int i = nivel_maximo + 3; i <= nrowp; i++)
                {
                    var itemid = Convert.ToInt32((workSheetp.Cells[i, 2].Value ?? "0").ToString());

                    var computo = (from x in computos
                                   where x.WbsPresupuestoId == wbsid
                                   where x.ItemId == itemid
                                   select x).FirstOrDefault();

                    if (computo != null && computo.Id > 0)
                    {
                        workSheetp.Cells[i, j].Value = computo.cantidad;
                        workSheetp.Cells[i, ncolp + 1].Value = computo.precio_unitario;

                        if (computo.codigo_item_alterno != null && computo.codigo_item_alterno.Length > 0)
                        {
                            workSheetp.Cells[i, 3].Value = computo.codigo_item_alterno;
                        }


                    }


                }

            }



            /*TERCERA */
            foreach (var pitem in subcontratositems) //items procura
            {

                hojasub.Cells[c2, 1].Value = pitem.para_oferta;
                hojasub.Cells[c2, 2].Value = pitem.Id;
                // workSheetp.Cells[c2, 3].Value = pitem.codigo;
                hojasub.Cells[c2, 4].Value = pitem.nombre;
                hojasub.Cells[c2, 4].Style.WrapText = true;
                if (pitem.UnidadId != 0)
                {
                    hojasub.Cells[c2, 5].Value = this.nombrecatalogo(pitem.UnidadId);

                }

                if (pitem.item_padre == ".")
                {
                    int padre = hojasub.Dimension.End.Column;
                    while (padre > 0)
                    {
                        hojasub.Cells[c2, padre].Style.Font.Bold = true;
                        hojasub.Cells[c2, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hojasub.Cells[c2, padre].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        hojasub.Cells[c2, padre].Style.Border.Top.Style =
                        hojasub.Cells[c2, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        padre--;

                    }

                }
                if (pitem.item_padre != "." && pitem.para_oferta == false)
                {
                    int padre = hojasub.Dimension.End.Column;
                    while (padre > 0)
                    {
                        hojasub.Cells[c2, padre].Style.Font.Bold = true;
                        hojasub.Cells[c2, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hojasub.Cells[c2, padre].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                        hojasub.Cells[c2, padre].Style.Border.Top.Style =
                        hojasub.Cells[c2, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        padre--;

                    }

                }

                hojasub.Cells[c2, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                hojasub.Cells[c2, 2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                hojasub.Cells[c2, hojasub.Dimension.End.Column].Style.Border.Right.Style =
                hojasub.Cells[c2, hojasub.Dimension.End.Column].Style.Border.Left.Style = ExcelBorderStyle.Medium;




                hojasub.Row(c2).Style.Locked = false;
                c2 = c2 + 1;
            }
            var nrowsub = hojasub.Dimension.End.Row;
            //SEGUNDA HOJA PROCURA
            for (int j = 6; j <= ncolp; j++)
            {
                var wbsid = Convert.ToInt32((hojasub.Cells[nivel_maximo + 1, j].Value ?? "0").ToString());
                for (int i = nivel_maximo + 3; i <= nrowsub; i++)
                {
                    var itemid = Convert.ToInt32((hojasub.Cells[i, 2].Value ?? "0").ToString());

                    var computo = (from x in computos
                                   where x.WbsPresupuestoId == wbsid
                                   where x.ItemId == itemid
                                   select x).FirstOrDefault();

                    if (computo != null && computo.Id > 0)
                    {
                        hojasub.Cells[i, j].Value = computo.cantidad;
                        hojasub.Cells[i, ncolp + 1].Value = computo.precio_unitario;

                        if (computo.codigo_item_alterno != null && computo.codigo_item_alterno.Length > 0)
                        {
                            hojasub.Cells[i, 3].Value = computo.codigo_item_alterno;
                        }


                    }


                }

            }



            //////////////////////////////////////////////////////////////////////////////////////
            var noOfCol = workSheet.Dimension.End.Column;
            var noOfRow = workSheet.Dimension.End.Row;

            int coln = workSheet.Dimension.End.Column;

            //CALCULOS PRIMERA HOJA


            while (coln > 0)
            {
                workSheet.Cells[workSheet.Dimension.End.Row, coln].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                coln--;

            }
            //PRIMERA HJA
            for (int j = 6; j <= noOfCol; j++)
            {
                for (int i = nivel_maximo + 3; i <= noOfRow; i++)
                {

                    //CAMPOS
                    var campo_item = (workSheet.Cells[i, 2].Value ?? "0").ToString();
                    var campo_wbs = (workSheet.Cells[nivel_maximo + 1, j].Value ?? "0").ToString();
                    var campo_para_oferta = (workSheet.Cells[i, 1].Value ?? "").ToString();

                    //VALORES
                    var ItemId = 0;
                    var WbsPresupuestoId = 0;

                    //CONVERSIONES
                    if (campo_item.Length > 0)
                    {
                        ItemId = Int32.Parse(campo_item);
                    }
                    if (campo_wbs.Length > 0)
                    {
                        WbsPresupuestoId = Int32.Parse(campo_wbs);
                    }

                    //VERIFICACIONES

                    if (ItemId > 0 &&
                        WbsPresupuestoId > 0 &&
                        campo_para_oferta.Length > 0 &&
                        campo_para_oferta.Equals("True"))

                    {
                        var computo = (from x in computos
                                       where x.WbsPresupuestoId == WbsPresupuestoId
                                       where x.ItemId == ItemId
                                       select x).FirstOrDefault();

                        if (computo != null && computo.Id > 0)
                        {
                            workSheet.Cells[i, j].Value = computo.cantidad;

                        }
                    }

                }
            }


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


        [UnitOfWork]
        public bool CambiarComputoCompleto(int PresupuestoId)
        {
            var presupuesto = Repository.Get(PresupuestoId);

            if (presupuesto.computo_completo)
            {
                presupuesto.computo_completo = !presupuesto.computo_completo;
                return false;
            }
            else
            {
                presupuesto.computo_completo = !presupuesto.computo_completo;
                return true;
            }
        }

        public List<PresupuestoDto> ListarPresupuestosDefinitivosAprobados(int RequerimientoId)
        {
            var query = Repository.GetAll()
                   .Where(o => o.vigente)
                   .Where(o => o.RequerimientoId == RequerimientoId)
                   .Where(o => o.es_final == true)
                   .Where(o => o.estado_aprobacion == Presupuesto.EstadoAprobacion.Aprobado)
                   .OrderByDescending(o => o.fecha_registro);

            var lista = (from p in query
                         select new PresupuestoDto()
                         {
                             Id = p.Id,
                             Proyecto = p.Proyecto,
                             codigo = p.codigo,
                             alcance = p.alcance,
                             descripcion = p.descripcion,
                             version = p.version,
                             fecha_registro = p.fecha_registro,
                             es_final = p.es_final
                         }).ToList();

            foreach (var p in lista)
            {
                p.NombreEstadoAprobacion = p.GetDisplayName(p.estado_aprobacion);
                p.NombreEstadoEmision = p.GetDisplayName(p.estado_emision);
                p.NombreClase = p.GetDisplayName(p.Clase.GetValueOrDefault());
            }

            return lista;
        }
        public void CalcularMontosPresupuesto(int Id)
        {
            decimal montoingenieria = 0;
            decimal montoconstruccion = 0;
            decimal montosuministros = 0;
            decimal montosubcontratos = 0;
            var listacomputos = _repositoryComputoPresupuesto.GetAllIncluding(c => c.WbsPresupuesto.Presupuesto, c => c.Item)
                .Where(c => c.vigente == true).Where(c => c.WbsPresupuesto.PresupuestoId == Id)
                .ToList();

            if (listacomputos != null && listacomputos.Count >= 0)
            {
                montoingenieria = (from x in listacomputos where x.Item.GrupoId == 1 select x.costo_total).Sum();
                montoconstruccion = (from x in listacomputos where x.Item.GrupoId == 2 select x.costo_total).Sum();
                montosuministros = (from x in listacomputos where x.Item.GrupoId == 3 select x.costo_total).Sum();
                montosubcontratos = (from x in listacomputos where x.Item.GrupoId == 4 select x.costo_total).Sum();
            }



            var presupuesto = Repository.Get(Id);
            presupuesto.monto_ingenieria = montoingenieria;
            //presupuesto.monto_construccion = montoconstruccion;
            presupuesto.monto_suministros = montosuministros;
            presupuesto.monto_subcontratos = montosubcontratos;
                     


            //Montos Certificados
            decimal montoa = montoconstruccion * (Convert.ToDecimal(0.4119));
            decimal montoi = montoconstruccion * (Convert.ToDecimal(0.03));
            decimal montou = montoconstruccion * (Convert.ToDecimal(0.12));
            decimal montopc = montosuministros * (Convert.ToDecimal(0.10));
            decimal totaladministracion = montoa + montoi + montou + montopc;

            var montoContruccionAUI = montoconstruccion + montoa+montoi+montou;
            presupuesto.monto_construccion = montoContruccionAUI;

            presupuesto.monto_total = montoingenieria + montoconstruccion + montosuministros + montosubcontratos + totaladministracion;
            var actualizado = Repository.Update(presupuesto);


        }

        public ExcelPackage GenerarExcelCargaPresupuesto(PresupuestoDto oferta, int nivel_maximo, bool reporte)
        {

            //Lista Wbs
            var query = _respositoryWbsPresupuesto.GetAllIncluding(x => x.Catalogo)
                .Where(o => o.vigente == true)
                .Where(o => o.PresupuestoId == oferta.Id)
                .Where(o => o.es_actividad == true).ToList();

            var wbs = (from w in query
                       select new WbsPresupuestoDto()
                       {
                           Id = w.Id,
                           PresupuestoId = w.PresupuestoId,
                           fecha_inicial = w.fecha_inicial,
                           fecha_final = w.fecha_final,
                           id_nivel_padre_codigo = w.id_nivel_padre_codigo,
                           id_nivel_codigo = w.id_nivel_codigo,
                           nivel_nombre = w.nivel_nombre,
                           observaciones = w.observaciones,
                           DisciplinaId = w.DisciplinaId,
                           Catalogo = w.Catalogo

                       }).ToList();


            foreach (var w in wbs)
            {
                var name = _respositoryWbsPresupuesto
                    .GetAll()
                    .Where(o => o.vigente == true)
                    .Where(o => o.PresupuestoId == w.PresupuestoId).SingleOrDefault(o => o.id_nivel_codigo == w.id_nivel_padre_codigo);
                if (name != null)
                {
                    w.nombre_padre = name.nivel_nombre;
                }
            }




            var listacomputos = _repositoryComputoPresupuesto.GetAllIncluding(x => x.WbsPresupuesto.Presupuesto.Proyecto, x => x.Item.Grupo)
                                                             .Where(x => x.vigente == true)
                                                             .Where(x => x.WbsPresupuesto.PresupuestoId == oferta.Id)
                                                             .Where(x => x.WbsPresupuesto.vigente)
                                                             .ToList();
            var computos = (from z in listacomputos
                            where z.WbsPresupuesto.PresupuestoId == oferta.Id && z.vigente == true
                            select new ComputoPresupuestoDto
                            {
                                Id = z.Id,
                                ItemId = z.ItemId,
                                WbsPresupuestoId = z.WbsPresupuestoId,
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
                                Cambio = z.Cambio,
                                codigo_item_alterno = z.codigo_item_alterno,
                                item_GrupoId = z.Item.GrupoId
                                ,
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

            var items = _itemservice.ItemsMatrizPresupuesto(oferta.Proyecto.contratoId, oferta.fecha_registro.GetValueOrDefault(), computos);

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Carga Computo");

            workSheet.TabColor = System.Drawing.Color.Azure;

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

            foreach (var itemswbs in wbs)
            {


                int fila = nivel_maximo + 4;
                List<WbsPresupuesto> Jerarquia = new List<WbsPresupuesto>();

                WbsPresupuesto item = _respositoryWbsPresupuesto.Get(itemswbs.Id);
                Jerarquia.Add(item);
                while (item.id_nivel_padre_codigo != ".")
                {
                    //
                    item = _respositoryWbsPresupuesto.GetAll()
                        .Where(X => X.id_nivel_codigo == item.id_nivel_padre_codigo)
                        .Where(C => C.vigente)
                        .Where(C => C.PresupuestoId == oferta.Id)
                        .FirstOrDefault();

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

            workSheet.Row(nivel_maximo).Height = 60;


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
                    workSheet.Cells[c, 5].Value = this.nombrecatalogo(pitem.UnidadId);

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

                        if (Convert.ToString(itemss.WbsPresupuestoId) == wbsid && Convert.ToString(itemss.ItemId) == itemid)
                        {
                            if (reporte)
                            {
                                if (itemss.Cambio != null && itemss.Cambio == ComputoPresupuesto.TipoCambioComputo.Nuevo)
                                {
                                    workSheet.Cells[i, j].Style.Border.Top.Style =
                                    workSheet.Cells[i, j].Style.Border.Left.Style =
                                    workSheet.Cells[i, j].Style.Border.Right.Style =
                                    workSheet.Cells[i, j].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                    workSheet.Cells[i, j].Style.Border.Top.Style =
                                    workSheet.Cells[i, j].Style.Border.Left.Style =
                                  workSheet.Cells[i, j].Style.Border.Right.Style =
                                  workSheet.Cells[i, j].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                    workSheet.Cells[i, j].Style.Border.Top.Color.SetColor(Color.Red);
                                    workSheet.Cells[i, j].Style.Border.Bottom.Color.SetColor(Color.Red);
                                    workSheet.Cells[i, j].Style.Border.Left.Color.SetColor(Color.Red);
                                    workSheet.Cells[i, j].Style.Border.Right.Color.SetColor(Color.Red);

                                }
                                if (itemss.Cambio != null && itemss.Cambio == ComputoPresupuesto.TipoCambioComputo.Editado)
                                {
                                    workSheet.Cells[i, j].Style.Border.Top.Style =
                                    workSheet.Cells[i, j].Style.Border.Left.Style =
                                    workSheet.Cells[i, j].Style.Border.Right.Style =
                                    workSheet.Cells[i, j].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                    workSheet.Cells[i, j].Style.Border.Top.Style =
                                    workSheet.Cells[i, j].Style.Border.Left.Style =
                                  workSheet.Cells[i, j].Style.Border.Right.Style =
                                  workSheet.Cells[i, j].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                    workSheet.Cells[i, j].Style.Border.Top.Color.SetColor(Color.Blue);
                                    workSheet.Cells[i, j].Style.Border.Bottom.Color.SetColor(Color.Blue);
                                    workSheet.Cells[i, j].Style.Border.Left.Color.SetColor(Color.Blue);
                                    workSheet.Cells[i, j].Style.Border.Right.Color.SetColor(Color.Blue);

                                }
                            }
                            //



                            workSheet.Cells[i, j].Value = itemss.cantidad;
                            workSheet.Cells[i, noOfCol + 2].Value = itemss.precio_unitario;
                            if (itemss.codigo_item_alterno != null && itemss.codigo_item_alterno.Length > 0)
                            {
                                workSheet.Cells[i, 3].Value = itemss.codigo_item_alterno;
                            }
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
                workSheet.Cells[i, ncol].FormulaR1C1 = "=ROUND(" + "$" + dcantidad + "*" + "$" + dprecio + ", 2)";

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

            /*
                        decimal mi = 0;
                        decimal mc = 0;
                        decimal ms = 0;

                        decimal mto = 0;

                        mi = Decimal.Parse("0"+workSheet.Cells[sumaingenieria].Value);
                        mc = Decimal.Parse("0" + workSheet.Cells[suma_contruccion].Value);
                        ms = Decimal.Parse("0" + workSheet.Cells[suma_procura].Value);
                        mto = Decimal.Parse("0" + workSheet.Cells[valortotal].Value);

                        var presupuesto = Repository.GetAll().Where(x=> x.Id == oferta.Id).FirstOrDefault();
                        if (presupuesto != null && presupuesto.Id > 0) {
                            presupuesto.monto_ingenieria = mi;
                            presupuesto.monto_construccion = mc;
                            presupuesto.monto_suministros = mc;
                            presupuesto.monto_total=mto;
                            Repository.Update(presupuesto);
                        }*/


            return excel;
        }

        public void EstadoCambioNullPresupuestos(int Id)
        {
            var computos = _repositoryComputoPresupuesto.GetAll()
                .Where(c => c.vigente == true)
                .Where(c => c.WbsPresupuesto.PresupuestoId == Id).ToList();

            foreach (var item in computos)
            {
                item.Cambio = null;
                _repositoryComputoPresupuesto.Update(item);
            }
        }

        public PresupuestoMensaje ActualizarCostos(int Id)
        {

            PresupuestoMensaje resultado = new PresupuestoMensaje();
            List<String> sinprecio = new List<string>();
            resultado.mensaje = "GENERADO";

            decimal gananciacontruccion = 1;
            decimal gananciaprocura = 1;
            var presupuesto = Repository.Get(Id);
            var proyecto = _repositoryProyecto.Get(presupuesto.ProyectoId);


            /* Sección Obtención Ganancias*/
            Ganancia ganancia = null;
            var existeGananciaSNFecha = _gananciarepository.GetAll().Where(c => c.ContratoId == proyecto.contratoId)
                                                         .Where(c => !c.fecha_fin.HasValue)
                                                         .Where(c => c.vigente)
                                                         .Where(c => presupuesto.fecha_registro >= c.fecha_inicio)
                                                         .FirstOrDefault();
            if (existeGananciaSNFecha != null)
            {
                ganancia = existeGananciaSNFecha;
            }
            else
            {

                var gananciaConFechas = _gananciarepository.GetAll().Where(c => c.ContratoId == proyecto.contratoId)
                     .Where(c => c.fecha_inicio <= presupuesto.fecha_registro)
                     .Where(c => c.fecha_fin >= presupuesto.fecha_registro).Where(c => c.vigente == true).FirstOrDefault();

                if (gananciaConFechas != null)
                {
                    ganancia = gananciaConFechas;
                }

            }
            /* FN*/



            /*
                        //calculo de ganancia
                        var ganancia = _gananciarepository.GetAll().Where(c => c.ContratoId == proyecto.contratoId)
                            .Where(c => c.fecha_inicio <= presupuesto.fecha_registro)
                            .Where(c => c.fecha_fin >= presupuesto.fecha_registro).Where(c => c.vigente == true).FirstOrDefault();*/

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
            else
            {
                resultado.mensaje = "NO_GANANCIA";
                resultado.errores = new List<string>();
                return resultado;
            }

            //PRECIARIO

            var preciario = _repositorypreciario.GetAll().Where(c => c.ContratoId == proyecto.contratoId)
                   .Where(c => c.fecha_desde <= presupuesto.fecha_registro)
                   .Where(c => c.fecha_hasta >= presupuesto.fecha_registro)
                   .Where(c => c.vigente == true)
                   .FirstOrDefault();

            if (preciario != null && preciario.Id > 0)
            {


                //SACO LOS COMPUTOS DE LA OFERTA
                var computos = _repositoryComputoPresupuesto.GetAllIncluding(c => c.Item)
                                                               .Where(c => c.vigente)
                                                               .Where(c => c.WbsPresupuesto.PresupuestoId == presupuesto.Id)
                                                               .ToList();

                if (computos.Count > 0)
                {

                    var itemspreciario = _repositorydetallepreciario.GetAll()
                                                                    .Where(c => c.PreciarioId == preciario.Id)
                                                                    .Where(e => e.vigente == true)
                                                                    .ToList();


                    foreach (var actual in computos)
                    {
                        if (actual.Item.GrupoId != 3)
                        {//CALCULOS DE LOS DATOS A EXCEPCION DE LOS DE PROCURA


                            var preciounitario = (from p in itemspreciario
                                                  where p.ItemId == actual.ItemId
                                                  where p.vigente == true
                                                  select p.precio_unitario).FirstOrDefault();

                            if (preciounitario > 0)
                            {

                                actual.precio_base = preciounitario;

                                //Calculo de Ganancia
                                if (actual.Item.GrupoId == 1)
                                {

                                }

                                if (actual.Item.GrupoId == 2)
                                {
                                    if (gananciacontruccion > 1)
                                    {

                                        actual.precio_incrementado = preciounitario * (gananciacontruccion / 100);
                                    }
                                }

                                if (actual.Item.GrupoId == 3)
                                {
                                    if (gananciaprocura > 1)
                                    {
                                        actual.precio_incrementado = preciounitario * (gananciaprocura / 100);
                                    }
                                }



                                Decimal costototal = 0;

                                if (actual.precio_ajustado > 0)
                                {

                                    costototal = Decimal.Round(Decimal.Round(actual.cantidad, 4) * actual.precio_ajustado, 4);
                                    actual.precio_unitario = actual.precio_ajustado;
                                    actual.precio_aplicarse = "precio ajus";
                                }
                                else
                                {
                                    costototal = Decimal.Round(Decimal.Round(actual.cantidad, 4) * preciounitario, 4);
                                    actual.precio_aplicarse = "precio base";
                                    actual.precio_unitario = preciounitario;
                                }

                                actual.costo_total = costototal;
                                actual.fecha_registro = actual.fecha_registro;
                                actual.fecha_actualizacion = actual.fecha_actualizacion;
                                actual.precio_incrementado = actual.precio_incrementado;
                                _repositoryComputoPresupuesto.Update(actual);



                            }

                        }
                        else
                        {
                            actual.costo_total = actual.precio_unitario * actual.cantidad;
                            _repositoryComputoPresupuesto.Update(actual);
                        }



                    }


                }



            }
            else
            {

                resultado.mensaje = "NO_PRECIARIO";
                resultado.errores = new List<string>();
                return resultado;
            }

            resultado.errores = sinprecio;
            return resultado;

        }

        public ExcelPackage GenerarExcelCargaPresupuestoRdo(PresupuestoDto oferta, int nivel_maximo, bool reporte)
        {
            //Lista Wbs
            var query = _respositoryWbsPresupuesto.GetAllIncluding(x => x.Catalogo)
                .Where(o => o.vigente == true)
                .Where(o => o.PresupuestoId == oferta.Id)
                .Where(o => o.es_actividad == true).ToList();

            var wbs = (from w in query
                       select new WbsPresupuestoDto()
                       {
                           Id = w.Id,
                           PresupuestoId = w.PresupuestoId,
                           fecha_inicial = w.fecha_inicial,
                           fecha_final = w.fecha_final,
                           id_nivel_padre_codigo = w.id_nivel_padre_codigo,
                           id_nivel_codigo = w.id_nivel_codigo,
                           nivel_nombre = w.nivel_nombre,
                           observaciones = w.observaciones,
                           DisciplinaId = w.DisciplinaId,
                           Catalogo = w.Catalogo

                       }).ToList();


            foreach (var w in wbs)
            {
                var name = _respositoryWbsPresupuesto
                    .GetAll()
                    .Where(o => o.vigente == true)
                    .Where(o => o.PresupuestoId == w.PresupuestoId).SingleOrDefault(o => o.id_nivel_codigo == w.id_nivel_padre_codigo);
                if (name != null)
                {
                    w.nombre_padre = name.nivel_nombre;
                }
            }




            var listacomputos = _repositoryComputoPresupuesto.GetAllIncluding(x => x.WbsPresupuesto.Presupuesto.Proyecto, x => x.Item).Where(x => x.vigente == true).ToList();
            var computos = (from z in listacomputos
                            where z.WbsPresupuesto.PresupuestoId == oferta.Id && z.vigente == true
                            select new ComputoPresupuestoDto
                            {
                                Id = z.Id,
                                ItemId = z.ItemId,
                                WbsPresupuestoId = z.WbsPresupuestoId,
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
                                Cambio = z.Cambio


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
            // var items = _itemservice.GetItemsporContratoActivo(oferta.Proyecto.contratoId, oferta.fecha_oferta.Value);
            var items = _itemservice.ArbolWbsExcel(oferta.Proyecto.contratoId, oferta.fecha_registro.GetValueOrDefault());
            ExcelPackage excel = new ExcelPackage();
            var n = excel.Workbook.Worksheets.Add("Carga Computo");

            n.TabColor = System.Drawing.Color.Azure;

            n.DefaultRowHeight = 15;

            //Header of table  
            //  

            n.View.ZoomScale = 60;
            int row = nivel_maximo;
            for (int i = 1; i <= row; i++)
            {

                n.Row(i).Style.Font.Bold = true;
            }

            int columna = 6;

            foreach (var itemswbs in wbs.OrderBy(l => l.id_nivel_codigo))
            {


                int fila = nivel_maximo + 4;
                List<WbsPresupuesto> Jerarquia = new List<WbsPresupuesto>();

                WbsPresupuesto item = _respositoryWbsPresupuesto.Get(itemswbs.Id);
                Jerarquia.Add(item);
                while (item.id_nivel_padre_codigo != ".")
                {
                    //
                    item = _respositoryWbsPresupuesto.GetAll()
                        .Where(X => X.id_nivel_codigo == item.id_nivel_padre_codigo)
                        .Where(C => C.vigente)
                        .Where(C => C.PresupuestoId == oferta.Id)
                        .FirstOrDefault();

                    Jerarquia.Add(item);
                }
                int a = Jerarquia.Count();
                int rowtyle = nivel_maximo - 1;
                foreach (var wbsj in Jerarquia)
                {
                    if (rowtyle > 0)
                    {
                        n.Cells[rowtyle, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        n.Cells[rowtyle, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);

                        n.Cells[rowtyle, columna].Style.Border.Left.Style =
                        n.Cells[rowtyle, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        rowtyle--;
                    }
                    if (a > 0)
                    {
                        if (wbsj.es_actividad)
                        {
                            n.Cells[fila - 4, columna].Value = wbsj.nivel_nombre;
                            n.Cells[fila - 4, columna].Value = wbsj.nivel_nombre;
                            n.Column(columna).Width = 25;
                            n.Cells[fila - 4, columna].Style.WrapText = true;
                            n.Cells[fila - 4, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            n.Cells[fila - 4, columna].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            n.Cells[fila - 4, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            n.Cells[fila - 4, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                            n.Cells[fila - 4, columna].Style.Border.Top.Style = n.Cells[fila - 4, columna].Style.Border.Left.Style = n.Cells[fila - 4, columna].Style.Border.Right.Style = n.Cells[fila - 4, columna].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                            n.Cells[fila - 2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            n.Cells[fila - 2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        }
                        else
                        {
                            n.Cells[a, columna].Value = wbsj.nivel_nombre;

                            n.Cells[a, columna].Style.WrapText = true;
                            n.Cells[a, columna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            n.Cells[a, columna].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            n.Cells[a, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            n.Cells[a, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                            n.Cells[a, columna].Style.Border.Left.Style =
                            n.Cells[a, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;



                        }
                    }

                    a--;
                }
                n.Row(fila - 3).Hidden = true;
                n.Cells[fila - 3, columna].Value = itemswbs.Id;
                n.Cells[fila - 3, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                n.Cells[fila - 3, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                n.Cells[fila - 3, columna].Style.Border.Left.Style =
                       n.Cells[fila - 3, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;


                if (itemswbs.DisciplinaId >= 0)
                {
                    n.Cells[fila - 2, columna].Value = itemswbs.Catalogo.nombre;
                    n.Cells[fila - 2, columna].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    n.Cells[fila - 2, columna].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
                    n.Cells[fila - 2, columna].Style.Border.Left.Style =
                       n.Cells[fila - 2, columna].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    n.Cells[fila - 2, columna].Style.Border.Top.Style = n.Cells[fila - 2, columna].Style.Border.Left.Style = n.Cells[fila - 2, columna].Style.Border.Right.Style = n.Cells[fila - 2, columna].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                }
                else
                {
                    n.Row(fila - 2).Hidden = true;
                }

                columna = columna + 1;
            }
            n.Cells[nivel_maximo, 1].Value = "Grupo";
            n.Cells[nivel_maximo, 2].Value = "Id";
            n.Cells[nivel_maximo, 3].Value = "ITEM";
            n.Cells[nivel_maximo, 4].Value = "DESCRIPCIÓN";
            n.Cells[nivel_maximo, 5].Value = "UNIDAD";

            string rango = n.Cells[nivel_maximo, 2].Address + ":" + n.Cells[nivel_maximo, 5].Address;
            n.Cells[rango].AutoFilter = true;
            for (int i = 2; i <= 5; i++)
            {
                n.Cells[nivel_maximo, i].Style.WrapText = true;
                n.Cells[nivel_maximo, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                n.Cells[nivel_maximo, i].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                n.Cells[nivel_maximo + 2, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                n.Cells[nivel_maximo + 2, i].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                n.Cells[nivel_maximo, i].Style.Border.Top.Style = n.Cells[nivel_maximo, i].Style.Border.Left.Style = n.Cells[nivel_maximo, i].Style.Border.Right.Style = n.Cells[nivel_maximo, i].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                n.Cells[nivel_maximo, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                n.Cells[nivel_maximo, i].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            n.Column(3).Width = 15;
            n.Column(4).Width = 80;
            n.Column(5).Width = 15;
            n.Column(3).Style.WrapText = true;
            n.Column(4).Style.WrapText = true;
            n.Column(5).Style.WrapText = true;
            n.Column(1).Style.Font.Bold = true;
            n.Column(1).Hidden = true;
            n.Column(2).Hidden = true;

            n.Row(nivel_maximo).Height = 60;


            //int inicio de las filas
            //int c = 5;
            int c = nivel_maximo + 3;
            n.View.FreezePanes(6, 1);
            n.View.FreezePanes(c, 6);
            foreach (var pitem in items)
            {

                n.Cells[c, 1].Value = pitem.GrupoId;

                n.Cells[c, 2].Value = pitem.Id;
                n.Cells[c, 3].Value = pitem.codigo;
                n.Cells[c, 4].Value = pitem.nombre;
                n.Cells[c, 4].Style.WrapText = true;
                if (pitem.UnidadId != 0)
                {
                    n.Cells[c, 5].Value = this.nombrecatalogo(pitem.UnidadId);

                }

                if (pitem.item_padre == ".")
                {
                    int padre = n.Dimension.End.Column;
                    while (padre > 0)
                    {
                        n.Cells[c, padre].Style.Font.Bold = true;
                        n.Cells[c, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        n.Cells[c, padre].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        n.Cells[c, padre].Style.Border.Top.Style = n.Cells[c, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        padre--;

                    }

                }
                if (pitem.item_padre != "." && pitem.para_oferta == false)
                {
                    int padre = n.Dimension.End.Column;

                    while (padre > 0)
                    {
                        n.Cells[c, padre].Style.Font.Bold = true;
                        n.Cells[c, padre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        n.Cells[c, padre].Style.Fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                        n.Cells[c, padre].Style.Border.Top.Style = n.Cells[c, padre].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        padre--;

                    }

                }

                n.Cells[c, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                n.Cells[c, 2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                n.Cells[c, n.Dimension.End.Column].Style.Border.Right.Style = ExcelBorderStyle.Medium;




                n.Row(c).Style.Locked = false;
                c = c + 1;
            }


            //EMPIEZA PARTE DE LA DERECHA CALCULOS Y TODO

            var noOfCol = n.Dimension.End.Column;
            var noOfRow = n.Dimension.End.Row;

            n.Cells[nivel_maximo, noOfCol + 1].Value = "CANTIDAD ESTIMADA";
            n.Cells[nivel_maximo, noOfCol + 2].Value = "PRECIO UNITARIO";
            n.Cells[nivel_maximo, noOfCol + 3].Value = "COSTO TOTAL ESTIMADO";




            // Estilos a la Parte Derecha de Calculos
            for (int a = noOfCol; a <= noOfCol + 3; a++)
            {
                n.Column(a).Width = 25;
                n.Cells[nivel_maximo, a].Style.WrapText = true;
                n.Cells[nivel_maximo, a].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                n.Cells[nivel_maximo, a].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                n.Cells[nivel_maximo, a].Style.Fill.PatternType = ExcelFillStyle.Solid;
                n.Cells[nivel_maximo, a].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                n.Cells[nivel_maximo, a].Style.Border.Top.Style =
                n.Cells[nivel_maximo, a].Style.Border.Left.Style =
                n.Cells[nivel_maximo, a].Style.Border.Right.Style =
                n.Cells[nivel_maximo, a].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            }
            for (int j = 6; j <= noOfCol; j++)
            {

                //Valores Parte Derecha





                var wbsid = (n.Cells[nivel_maximo + 1, j].Value ?? "").ToString();
                for (int i = nivel_maximo + 3; i <= noOfRow; i++)
                {
                    var itemid = (n.Cells[i, 2].Value ?? "").ToString();

                    foreach (var itemss in computos)
                    {

                        if (Convert.ToString(itemss.WbsPresupuestoId) == wbsid && Convert.ToString(itemss.ItemId) == itemid)
                        {
                            if (reporte)
                            {
                                if (itemss.Cambio != null && itemss.Cambio == ComputoPresupuesto.TipoCambioComputo.Nuevo)
                                {
                                    n.Cells[i, j].Style.Border.Top.Style =
                                    n.Cells[i, j].Style.Border.Left.Style =
                                    n.Cells[i, j].Style.Border.Right.Style =
                                    n.Cells[i, j].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                    n.Cells[i, j].Style.Border.Top.Style =
                                    n.Cells[i, j].Style.Border.Left.Style =
                                  n.Cells[i, j].Style.Border.Right.Style =
                                  n.Cells[i, j].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                    n.Cells[i, j].Style.Border.Top.Color.SetColor(Color.Red);
                                    n.Cells[i, j].Style.Border.Bottom.Color.SetColor(Color.Red);
                                    n.Cells[i, j].Style.Border.Left.Color.SetColor(Color.Red);
                                    n.Cells[i, j].Style.Border.Right.Color.SetColor(Color.Red);

                                }
                                if (itemss.Cambio != null && itemss.Cambio == ComputoPresupuesto.TipoCambioComputo.Editado)
                                {
                                    n.Cells[i, j].Style.Border.Top.Style =
                                    n.Cells[i, j].Style.Border.Left.Style =
                                    n.Cells[i, j].Style.Border.Right.Style =
                                    n.Cells[i, j].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                    n.Cells[i, j].Style.Border.Top.Style =
                                    n.Cells[i, j].Style.Border.Left.Style =
                                  n.Cells[i, j].Style.Border.Right.Style =
                                  n.Cells[i, j].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                    n.Cells[i, j].Style.Border.Top.Color.SetColor(Color.Blue);
                                    n.Cells[i, j].Style.Border.Bottom.Color.SetColor(Color.Blue);
                                    n.Cells[i, j].Style.Border.Left.Color.SetColor(Color.Blue);
                                    n.Cells[i, j].Style.Border.Right.Color.SetColor(Color.Blue);

                                }
                            }
                            //



                            n.Cells[i, j].Value = itemss.cantidad;
                            n.Cells[i, noOfCol + 2].Value = itemss.precio_unitario;
                            n.Cells[i, j].Style.Numberformat.Format = "#,##0.00";
                            n.Cells[i, noOfCol + 1].Style.Numberformat.Format = "#,##0.00";
                            n.Cells[i, noOfCol + 2].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                            n.Cells[i, noOfCol + 3].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";


                        }

                    }


                }




            }

            int ncol = n.Dimension.End.Column;

            for (int i = nivel_maximo + 3; i <= noOfRow; i++)
            {
                var rango_incio = n.Cells[i, 6].Address;
                var rango_final = n.Cells[i, ncol - 3].Address;
                var rangosumar = rango_incio + ":" + rango_final;
                n.Cells[i, ncol - 2].Formula = "=SUM(" + rangosumar + ")";

                var cantidad = n.Cells[i, ncol - 2].Value;
                var precio = n.Cells[i, ncol - 2].Value;

                var dcantidad = n.Cells[i, ncol - 2].Address;
                var dprecio = n.Cells[i, ncol - 1].Address;
                n.Cells[i, ncol].FormulaR1C1 = "=ROUND(" + dcantidad + "*" + dprecio + ", 2)";

            }

            int coln = n.Dimension.End.Column;
            while (coln > 0)
            {
                n.Cells[n.Dimension.End.Row, coln].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                coln--;

            }

            // rango total
            string costoi = n.Cells[nivel_maximo + 3, ncol].Address;
            string costoif = n.Cells[noOfRow, ncol].Address;
            string rangovalortotal = "$" + costoi + ":$" + costoif;


            int bfila = noOfRow;
            bfila = bfila + 2;
            //CALCULOS BASE
            //Ingenieria
            n.Cells[bfila, 4].Value = "Sub - total Ingeniería";
            n.Cells[bfila, 4].Style.WrapText = true;
            n.Cells[bfila, 4].Style.Border.Top.Style =
            n.Cells[bfila, 4].Style.Border.Left.Style =
            n.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            n.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4].Style.Font.Bold = true;


            n.Cells[bfila, 5].Style.Border.Top.Style =
            n.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;


            //suma ingenieria

            string dinicio = n.Cells[nivel_maximo + 3, 1].Address;
            string dfinal = n.Cells[noOfRow, 1].Address;
            string rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            string grupo = "" + 1 + "";

            var formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            n.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula;
            n.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string sumaingenieria = "$" + n.Cells[bfila, noOfCol + 3].Address;


            n.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
            n.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
            n.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
            n.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            bfila++;
            //Procura
            n.Cells[bfila, 4].Value = "Sub-total Procura";
            n.Cells[bfila, 4].Style.WrapText = true;
            n.Cells[bfila, 4].Style.Border.Top.Style =
            n.Cells[bfila, 4].Style.Border.Left.Style =

            n.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4].Style.Font.Bold = true;

            n.Cells[bfila, 5].Style.Border.Top.Style =
               n.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;


            //suma procura

            dinicio = n.Cells[nivel_maximo + 3, 1].Address;
            dfinal = n.Cells[noOfRow, 1].Address;
            rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            grupo = "" + 3 + "";

            formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            n.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula;
            n.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string suma_procura = "$" + n.Cells[bfila, noOfCol + 3].Address;
            n.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //CALCULOS BASE
            //Reembolsables
            bfila++;
            n.Cells[bfila, 4].Value = "Sub-total Reembolsables";
            n.Cells[bfila, 4].Style.WrapText = true;
            n.Cells[bfila, 4].Style.Border.Top.Style =
            n.Cells[bfila, 4].Style.Border.Left.Style =

            n.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4].Style.Font.Bold = true;

            n.Cells[bfila, 5].Style.Border.Top.Style =
            n.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, noOfCol + 3].Value = 0;
            n.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string sumareembolsables = "$" + n.Cells[bfila, noOfCol + 3].Address;

            n.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //Consturccion
            bfila++;
            n.Cells[bfila, 4].Value = "Sub-total Construcción";
            n.Cells[bfila, 4].Style.WrapText = true;
            n.Cells[bfila, 4].Style.Border.Top.Style =
            n.Cells[bfila, 4].Style.Border.Left.Style =

            n.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4].Style.Font.Bold = true;

            n.Cells[bfila, 5].Style.Border.Top.Style =
             n.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            dinicio = n.Cells[nivel_maximo + 3, 1].Address;
            dfinal = n.Cells[noOfRow, 1].Address;
            rangogrupo = "$" + dinicio + ":" + "$" + dfinal;
            grupo = "" + 2 + "";

            formula = "=SUMIF(" + rangogrupo + ", " + grupo + ", " + rangovalortotal + ")";
            n.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula;
            n.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string suma_contruccion = "$" + n.Cells[bfila, noOfCol + 3].Address;
            n.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //Descuento
            bfila++;
            n.Cells[bfila, 4].Value = "Descuento por <Descripción del concepto>";
            n.Cells[bfila, 4].Style.WrapText = true;
            n.Cells[bfila, 4].Style.Border.Top.Style =
            n.Cells[bfila, 4].Style.Border.Left.Style =

            n.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4].Style.Font.Bold = true;

            n.Cells[bfila, 5].Style.Border.Top.Style =
             n.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;


            n.Cells[bfila, noOfCol + 3].Value = 0;
            n.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            n.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            //Descuento
            bfila++;
            n.Cells[bfila, 4].Value = "Administración";
            n.Cells[bfila, 4].Style.WrapText = true;
            n.Cells[bfila, 4].Style.Border.Top.Style =
            n.Cells[bfila, 4].Style.Border.Left.Style =

            n.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4].Style.Font.Bold = true;

            n.Cells[bfila, 5].Style.Border.Top.Style =
                        n.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            string administracion = "$" + n.Cells[bfila, noOfCol + 3].Address;
            n.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;



            n.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;

            //
            bfila++;
            n.Cells[bfila, 4].Value = "Administracion sobre Obra (%)";
            n.Cells[bfila, 4].Style.WrapText = true;
            n.Cells[bfila, 4].Style.Border.Top.Style =
            n.Cells[bfila, 4].Style.Border.Left.Style =

            n.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            n.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4].Style.Font.Bold = true;

            n.Cells[bfila, 5].Value = 0.4119;
            n.Cells[bfila, 5].Style.WrapText = true;

            n.Cells[bfila, 5].Style.Border.Top.Style =
            n.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            n.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 5].Style.Font.Bold = true;
            n.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";

            //contruccion

            string formula_calculo = "=" + n.Cells[bfila, 5].Address + "*" + suma_contruccion;
            n.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 3].Style.WrapText = true;

            string valoradministracion_obra = n.Cells[bfila, noOfCol + 3].Address;

            n.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            //
            bfila++;
            n.Cells[bfila, 4].Value = "Imprevistos sobre Obra (%)";
            n.Cells[bfila, 4].Style.WrapText = true;
            n.Cells[bfila, 4].Style.Border.Top.Style =
            n.Cells[bfila, 4].Style.Border.Left.Style =

            n.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            n.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4].Style.Font.Bold = true;

            n.Cells[bfila, 5].Value = 0.03;
            n.Cells[bfila, 5].Style.WrapText = true;

            n.Cells[bfila, 5].Style.Border.Top.Style =
            n.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            n.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 5].Style.Font.Bold = true;
            n.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";

            //caculo
            formula_calculo = "=" + n.Cells[bfila, 5].Address + "*" + suma_contruccion;
            n.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string valorimprevistos_obra = n.Cells[bfila, noOfCol + 3].Address;
            n.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;
            //
            bfila++;
            n.Cells[bfila, 4].Value = "Utilidad sobre Obra (%)";
            n.Cells[bfila, 4].Style.WrapText = true;
            n.Cells[bfila, 4].Style.Border.Top.Style =
            n.Cells[bfila, 4].Style.Border.Left.Style =

            n.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            n.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4].Style.Font.Bold = true;

            n.Cells[bfila, 5].Value = 0.12;
            n.Cells[bfila, 5].Style.WrapText = true;

            n.Cells[bfila, 5].Style.Border.Top.Style =
            n.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            n.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 5].Style.Font.Bold = true;
            n.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";


            //caculo
            formula_calculo = "=" + n.Cells[bfila, 5].Address + "*" + suma_contruccion;
            n.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 3].Style.WrapText = true;



            string valor_utilidadObra = n.Cells[bfila, noOfCol + 3].Address;
            n.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;
            //
            bfila++;
            n.Cells[bfila, 4].Value = "Administracion sobre Procura Contratista (%)";
            n.Cells[bfila, 4].Style.WrapText = true;
            n.Cells[bfila, 4].Style.Border.Top.Style =
            n.Cells[bfila, 4].Style.Border.Left.Style =

            n.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            n.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4].Style.Font.Bold = true;

            n.Cells[bfila, 5].Value = 0.1;
            n.Cells[bfila, 5].Style.WrapText = true;

            n.Cells[bfila, 5].Style.Border.Top.Style =
            n.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            n.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 5].Style.Font.Bold = true;
            n.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";




            //caculo
            formula_calculo = "=" + n.Cells[bfila, 5].Address + "*" + suma_procura;
            n.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string valor_procura = n.Cells[bfila, noOfCol + 3].Address;
            n.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;


            bfila++;
            n.Cells[bfila, 4].Value = "Administracion sobre Reembolsables (%)";
            n.Cells[bfila, 4].Style.WrapText = true;
            n.Cells[bfila, 4].Style.Border.Top.Style =
            n.Cells[bfila, 4].Style.Border.Left.Style =

            n.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            n.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4].Style.Font.Bold = true;

            n.Cells[bfila, 5].Value = 0.1;
            n.Cells[bfila, 5].Style.WrapText = true;

            n.Cells[bfila, 5].Style.Border.Top.Style =
            n.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            n.Cells[bfila, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            n.Cells[bfila, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 5].Style.Font.Bold = true;
            n.Cells[bfila, 5].Style.Numberformat.Format = "0.0%";


            //caculo
            formula_calculo = "=" + n.Cells[bfila, 5].Address + "*" + sumareembolsables;
            n.Cells[bfila, noOfCol + 3].FormulaR1C1 = formula_calculo;
            n.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 3].Style.WrapText = true;


            string valor_reembolsables = n.Cells[bfila, noOfCol + 3].Address;
            n.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;
            //envio de datos a administracion

            formula_calculo = "=SUM(" + valoradministracion_obra + ":" + valor_reembolsables + ")";
            n.Cells[administracion].Formula = formula_calculo;
            n.Cells[administracion].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[administracion].Style.WrapText = true;


            bfila++;
            n.Cells[bfila, 4].Value = "TOTAL";
            n.Cells[bfila, 4].Style.WrapText = true;
            n.Cells[bfila, 4].Style.Border.Top.Style =
            n.Cells[bfila, 4].Style.Border.Left.Style =

            n.Cells[bfila, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            n.Cells[bfila, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            n.Cells[bfila, 4].Style.Font.Bold = true;
            n.Cells[bfila, 5].Style.Border.Top.Style =
            n.Cells[bfila, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;

            //TOTAL

            formula_calculo = "=SUM(" + sumaingenieria + "+" + suma_procura + "+" + suma_contruccion + "+" + sumareembolsables + "+" + administracion + ")";
            n.Cells[bfila, noOfCol + 3].Formula = formula_calculo;
            n.Cells[bfila, noOfCol + 3].Style.Numberformat.Format =
                "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
            n.Cells[bfila, noOfCol + 3].Style.WrapText = true;
            string valortotal = n.Cells[bfila, noOfCol + 3].Address;
            n.Cells[bfila, noOfCol + 3].Style.Border.Top.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Left.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Right.Style =
         n.Cells[bfila, noOfCol + 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            n.Cells[bfila, noOfCol + 3].Style.Font.Bold = true;



            return excel;
        }

        public bool estageneradopresupuesto(int Id)
        {
            var computos = _repositoryComputoPresupuesto
                         .GetAll()
                         .Where(c => c.WbsPresupuesto.PresupuestoId == Id)
                         .Where(c => c.vigente == true)
                         .Where(c => c.costo_total == 0)
                         .ToList();
            if (computos.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public List<Presupuesto> ListaPresupuestoDefinitivos()
        {
            var presupuesto = Repository.GetAllIncluding(c => c.Proyecto, c => c.Requerimiento.Proyecto)
                                      .Where(c => c.vigente)
                                      .Where(c => c.es_final)
                                      .ToList();

            return presupuesto;

        }

        public string IngresarItemsProcuraExce(HttpPostedFileBase UploadedFile, int maximo_nivel, int PresupuestoId)
        {
            var ComputosPresupuestos = _repositoryComputoPresupuesto.GetAll()
                                                                       .Where(c => c.WbsPresupuesto.PresupuestoId == PresupuestoId)
                                                                       .Where(c => c.WbsPresupuesto.vigente)
                                                                       .Where(c => c.vigente)
                                                                       .Where(c => c.Item.GrupoId == 3)
                                                                       .ToList();

            if (UploadedFile != null)
            {
                if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string fileName = UploadedFile.FileName;
                    string fileContentType = UploadedFile.ContentType;
                    byte[] fileBytes = new byte[UploadedFile.ContentLength];
                    var data = UploadedFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(UploadedFile.ContentLength));

                    using (var package = new ExcelPackage(UploadedFile.InputStream))
                    {

                        var pestana = package.Workbook.Worksheets;
                        var hoja = pestana[2];
                        var numerodecolumnas = hoja.Dimension.End.Column;
                        var numerodefilas = hoja.Dimension.End.Row;

                        //ITERAR LOS ITEMS

                        for (int c = 6; c <= numerodecolumnas - 1; c++) //i=6 columna donde comienzan los items,
                        {
                            for (int f = maximo_nivel + 3; f <= numerodefilas; f++)
                            {

                                //CAMPOS
                                var campo_item = (hoja.Cells[f, 2].Value ?? "").ToString();
                                var campo_wbs = (hoja.Cells[maximo_nivel + 1, c].Value ?? "").ToString();
                                var campo_para_oferta = (hoja.Cells[f, 1].Value ?? "").ToString();
                                var campo_cantidad = (hoja.Cells[f, c].Value ?? "").ToString();
                                var campo_preciounitario = (hoja.Cells[f, numerodecolumnas].Value ?? "0").ToString();

                                //CAMPOS NUEVOS PARA LOS ITEMS NUEVOS DE PROCURA
                                var codigo = (hoja.Cells[f, 3].Value ?? "").ToString();
                                var nombre = (hoja.Cells[f, 4].Value ?? "").ToString();
                                var unidad = (hoja.Cells[f, 5].Value ?? "").ToString();



                                //VALORES
                                var ItemId = 0;
                                var WbsPresupuestoId = 0;

                                decimal cantidad = Convert.ToDecimal(0);

                                if (campo_cantidad.Length > 0 && campo_cantidad.Contains('-'))
                                {
                                    cantidad = Decimal.Parse(campo_cantidad, NumberStyles.Float); //Para que acepte 10 exp e-8, 0,000001
                                }
                                else
                                {

                                    cantidad = Decimal.Parse("0" + campo_cantidad, NumberStyles.Float); //Para que acepte 10 exp e-8, 0,000001

                                }

                                var precio_unitario = Decimal.Parse("0" + campo_preciounitario.ToString());

                                //CONVERSIONES
                                if (campo_item.Length > 0)
                                {
                                    ItemId = Int32.Parse(campo_item);
                                }
                                if (campo_wbs.Length > 0)
                                {
                                    WbsPresupuestoId = Int32.Parse(campo_wbs);
                                }

                                //VERIFICACIONES

                                if (ItemId > 0 &&
                                    WbsPresupuestoId > 0 &&
                                    campo_para_oferta.Length > 0
                                    /*&&
                                    campo_para_oferta.Equals("True")
                                    */)

                                {// CumpleRequisitos para Insetar los que ya existen

                                    if (campo_cantidad.Length == 0) //campo vacio
                                    {
                                        ComputoPresupuesto com = (from e in ComputosPresupuestos
                                                                  where e.WbsPresupuestoId == WbsPresupuestoId
                                                                  where e.ItemId == ItemId
                                                                  select e).FirstOrDefault();
                                        if (com != null && com.Id > 0) // Si encontro el Item
                                        {
                                            com.vigente = false; //Elimina
                                            var update = _repositoryComputoPresupuesto.Update(com);
                                        }
                                    }
                                    else
                                    if (campo_cantidad.Length > 0 && cantidad >= 0 || campo_cantidad.Length > 0 && cantidad < 0)//campo con dato
                                    {

                                        ComputoPresupuesto com = (from e in ComputosPresupuestos
                                                                  where e.WbsPresupuestoId == WbsPresupuestoId
                                                                  where e.ItemId == ItemId
                                                                  select e).FirstOrDefault();

                                        if (com != null && com.Id > 0) // Si encontro el Item
                                        {
                                            if (cantidad > 0 || cantidad < 0)
                                            {
                                                // if (com.cantidad != cantidad)
                                                //{ //Si es diferente actualiza si no nope
                                                com.cantidad = Decimal.Round(cantidad, 4);
                                                com.cantidad_eac = Decimal.Round(cantidad);
                                                com.precio_base = precio_unitario;
                                                com.precio_unitario = precio_unitario;
                                                com.costo_total = Decimal.Round(Decimal.Round(cantidad, 4) * precio_unitario, 4);
                                                com.Cambio = ComputoPresupuesto.TipoCambioComputo.Editado;
                                                com.codigo_item_alterno = codigo.Length > 0 ? codigo : " ";
                                                var update = _repositoryComputoPresupuesto.Update(com);
                                                //}
                                            }
                                            else
                                            {
                                                com.vigente = false; //Elimina
                                                var update = _repositoryComputoPresupuesto.Update(com);
                                            }
                                        }
                                        else // No encontro el Item
                                        {
                                            if (cantidad > 0 && precio_unitario > 0 || cantidad < 0 && precio_unitario > 0)
                                            {// si cantidad mayor a cero? se crea el computo: Nope

                                                ComputoPresupuesto n = new ComputoPresupuesto()
                                                {
                                                    ItemId = ItemId,
                                                    WbsPresupuestoId = WbsPresupuestoId,
                                                    cantidad = Decimal.Round(cantidad, 4),
                                                    cantidad_eac = Decimal.Round(cantidad, 4), //Eac = cantidad 
                                                    precio_unitario = precio_unitario,
                                                    precio_ajustado = 0,
                                                    precio_base = precio_unitario,
                                                    precio_incrementado = 0,
                                                    costo_total = Decimal.Round(Decimal.Round(cantidad, 4) * precio_unitario, 4),
                                                    fecha_registro = DateTime.Now,
                                                    vigente = true,
                                                    estado = true,
                                                    Cambio = ComputoPresupuesto.TipoCambioComputo.Nuevo,
                                                    codigo_item_alterno = codigo.Length > 0 ? codigo : " "
                                                };
                                                var create = _repositoryComputoPresupuesto.Insert(n);

                                            }
                                        }

                                    }
                                }

                                if (WbsPresupuestoId > 0 &&
                                   ItemId == 0)
                                { //ITEMS QUE NO EXISTE NUEVOS PROCURA

                                    if (
                                       (codigo.Length > 0 &&
                                       nombre.Length > 0 &&
                                       unidad.Length > 0 &&
                                       cantidad > 0 &&
                                       precio_unitario > 0) ||
                                       (codigo.Length > 0 &&
                                       nombre.Length > 0 &&
                                       unidad.Length > 0 &&
                                       cantidad < 0 &&
                                       precio_unitario > 0))
                                    {
                                        int EProcuraId = 0;
                                        var EspecialidadId = _repositorycatalogo.GetAll().Where(x => x.codigo == ProyectoCodigos.EPROCURA_CONTRATISTA).FirstOrDefault();
                                        if (EspecialidadId != null && EspecialidadId.Id > 0)
                                        {
                                            EProcuraId = EspecialidadId.Id;
                                        }

                                        var Unidad = _repositorycatalogo.GetAll().Where(x => x.nombre == unidad).FirstOrDefault();

                                        var eItem = itemrepository.GetAllIncluding(i => i.Grupo, i => i.Especialidad)
                                                                 .Where(i => i.nombre == nombre)
                                                                 .Where(i => i.vigente)
                                                                 .Where(i => i.Grupo.codigo == ProyectoCodigos.CODE_PROCURA_CONTRATISTA)
                                                                 .FirstOrDefault();
                                        int IdNuevoItem = 0;
                                        if (eItem != null && eItem.Id > 0)
                                        {
                                            IdNuevoItem = eItem.Id;
                                        }
                                        else
                                        {
                                            Item nuevo = new Item
                                            {
                                                Id = 0,
                                                nombre = nombre,
                                                descripcion = nombre,
                                                GrupoId = 3,
                                                codigo = "7.1." + (_itemservice.GetItemsHijos("7.1.").Count() + 1),
                                                item_padre = "7.1.",
                                                para_oferta = true,
                                                UnidadId = Unidad != null && Unidad.Id > 0 ? Unidad.Id : 0,
                                                vigente = true,
                                                EspecialidadId = EProcuraId


                                            };

                                            IdNuevoItem = itemrepository.InsertAndGetId(nuevo);
                                        }

                                        ComputoPresupuesto com = (from e in ComputosPresupuestos
                                                                  where e.WbsPresupuestoId == WbsPresupuestoId
                                                                  where e.ItemId == ItemId
                                                                  select e).FirstOrDefault();
                                        if (com != null && com.Id > 0)
                                        {
                                            if (cantidad > 0 || cantidad < 0)
                                            {
                                                // if (com.cantidad != cantidad)
                                                //{ //Si es diferente actualiza si no nope
                                                com.cantidad = Decimal.Round(cantidad, 4);
                                                com.cantidad_eac = Decimal.Round(cantidad);
                                                com.precio_base = precio_unitario;
                                                com.precio_unitario = precio_unitario;
                                                com.costo_total = Decimal.Round(Decimal.Round(cantidad, 4) * precio_unitario, 4);
                                                com.Cambio = ComputoPresupuesto.TipoCambioComputo.Editado;
                                                com.codigo_item_alterno = codigo.Length > 0 ? codigo : " ";
                                                var update = _repositoryComputoPresupuesto.Update(com);
                                                //}
                                            }
                                            else
                                            {
                                                com.vigente = false; //Elimina
                                                var update = _repositoryComputoPresupuesto.Update(com);
                                            }
                                        }
                                        else
                                        {
                                            ComputoPresupuesto n = new ComputoPresupuesto
                                            {

                                                ItemId = IdNuevoItem,
                                                WbsPresupuestoId = WbsPresupuestoId,
                                                cantidad = Decimal.Round(cantidad, 4),
                                                cantidad_eac = Decimal.Round(cantidad, 4), //Eac = cantidad
                                                precio_unitario = precio_unitario,
                                                precio_base = precio_unitario,
                                                precio_ajustado = 0,
                                                precio_incrementado = 0,//ver ganancia
                                                costo_total = Decimal.Round(cantidad * precio_unitario, 4),
                                                vigente = true,
                                                fecha_registro = DateTime.Now,
                                                estado = true,
                                                Cambio = ComputoPresupuesto.TipoCambioComputo.Nuevo,
                                                codigo_item_alterno = codigo
                                            };

                                            var computonuevo = _repositoryComputoPresupuesto.Insert(n);
                                        }
                                    }


                                }















                            }

                        }
                        return "Ok";
                    }
                }
                else
                {
                    return "Debe Subir el Formato de Excel Exportado";
                }

            }
            else
            {
                return "sin_archivo";
            }
        }

        public string IngresarItemsSubContratosExce(HttpPostedFileBase UploadedFile, int maximo_nivel, int PresupuestoId)
        {
            var ComputosPresupuestos = _repositoryComputoPresupuesto.GetAll()
                                                                       .Where(c => c.WbsPresupuesto.PresupuestoId == PresupuestoId)
                                                                       .Where(c => c.WbsPresupuesto.vigente)
                                                                       .Where(c => c.vigente)
                                                                       .Where(c => c.Item.GrupoId == 4)
                                                                       .ToList();

            if (UploadedFile != null)
            {
                if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string fileName = UploadedFile.FileName;
                    string fileContentType = UploadedFile.ContentType;
                    byte[] fileBytes = new byte[UploadedFile.ContentLength];
                    var data = UploadedFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(UploadedFile.ContentLength));

                    using (var package = new ExcelPackage(UploadedFile.InputStream))
                    {

                        var pestana = package.Workbook.Worksheets;
                        var hoja = pestana[3];
                        var numerodecolumnas = hoja.Dimension.End.Column;
                        var numerodefilas = hoja.Dimension.End.Row;

                        //ITERAR LOS ITEMS

                        for (int c = 6; c <= numerodecolumnas - 1; c++) //i=6 columna donde comienzan los items,
                        {
                            for (int f = maximo_nivel + 3; f <= numerodefilas; f++)
                            {

                                //CAMPOS
                                var campo_item = (hoja.Cells[f, 2].Value ?? "").ToString();
                                var campo_wbs = (hoja.Cells[maximo_nivel + 1, c].Value ?? "").ToString();
                                var campo_para_oferta = (hoja.Cells[f, 1].Value ?? "").ToString();
                                var campo_cantidad = (hoja.Cells[f, c].Value ?? "").ToString();
                                var campo_preciounitario = (hoja.Cells[f, numerodecolumnas].Value ?? "0").ToString();

                                //CAMPOS NUEVOS PARA LOS ITEMS NUEVOS DE PROCURA
                                var codigo = (hoja.Cells[f, 3].Value ?? "").ToString();
                                var nombre = (hoja.Cells[f, 4].Value ?? "").ToString();
                                var unidad = (hoja.Cells[f, 5].Value ?? "").ToString();



                                //VALORES
                                var ItemId = 0;
                                var WbsPresupuestoId = 0;

                                decimal cantidad = Convert.ToDecimal(0);

                                if (campo_cantidad.Length > 0 && campo_cantidad.Contains('-'))
                                {
                                    cantidad = Decimal.Parse(campo_cantidad, NumberStyles.Float); //Para que acepte 10 exp e-8, 0,000001
                                }
                                else
                                {

                                    cantidad = Decimal.Parse("0" + campo_cantidad, NumberStyles.Float); //Para que acepte 10 exp e-8, 0,000001

                                }



                                var precio_unitario = Decimal.Parse("0" + campo_preciounitario.ToString());

                                //CONVERSIONES
                                if (campo_item.Length > 0)
                                {
                                    ItemId = Int32.Parse(campo_item);
                                }
                                if (campo_wbs.Length > 0)
                                {
                                    WbsPresupuestoId = Int32.Parse(campo_wbs);
                                }

                                //VERIFICACIONES

                                if (ItemId > 0 &&
                                    WbsPresupuestoId > 0 &&
                                    campo_para_oferta.Length > 0 &&
                                    campo_para_oferta.Equals("True"))

                                {// CumpleRequisitos para Insetar los que ya existen

                                    if (campo_cantidad.Length == 0) //campo vacio
                                    {
                                        ComputoPresupuesto com = (from e in ComputosPresupuestos
                                                                  where e.WbsPresupuestoId == WbsPresupuestoId
                                                                  where e.ItemId == ItemId
                                                                  select e).FirstOrDefault();
                                        if (com != null && com.Id > 0) // Si encontro el Item
                                        {
                                            com.vigente = false; //Elimina
                                            var update = _repositoryComputoPresupuesto.Update(com);
                                        }
                                    }
                                    else
                                    if (campo_cantidad.Length > 0 && cantidad >= 0 || campo_cantidad.Length > 0 && cantidad < 0)//campo con dato
                                    {

                                        ComputoPresupuesto com = (from e in ComputosPresupuestos
                                                                  where e.WbsPresupuestoId == WbsPresupuestoId
                                                                  where e.ItemId == ItemId
                                                                  select e).FirstOrDefault();

                                        if (com != null && com.Id > 0) // Si encontro el Item
                                        {
                                            if (cantidad > 0 || cantidad < 0)
                                            {
                                                // if (com.cantidad != cantidad)
                                                //{ //Si es diferente actualiza si no nope
                                                com.cantidad = Decimal.Round(cantidad, 4);
                                                com.cantidad_eac = Decimal.Round(cantidad);
                                                com.precio_base = precio_unitario;
                                                com.precio_unitario = precio_unitario;
                                                com.costo_total = Decimal.Round(Decimal.Round(cantidad, 4) * precio_unitario, 4);
                                                com.Cambio = ComputoPresupuesto.TipoCambioComputo.Editado;
                                                com.codigo_item_alterno = codigo.Length > 0 ? codigo : " ";
                                                var update = _repositoryComputoPresupuesto.Update(com);
                                                //}
                                            }
                                            else
                                            {
                                                com.vigente = false; //Elimina
                                                var update = _repositoryComputoPresupuesto.Update(com);
                                            }
                                        }
                                        else // No encontro el Item
                                        {
                                            if (cantidad > 0 && precio_unitario > 0 || cantidad < 0 && precio_unitario > 0)
                                            {// si cantidad mayor a cero? se crea el computo: Nope

                                                ComputoPresupuesto n = new ComputoPresupuesto()
                                                {
                                                    ItemId = ItemId,
                                                    WbsPresupuestoId = WbsPresupuestoId,
                                                    cantidad = Decimal.Round(cantidad, 4),
                                                    cantidad_eac = Decimal.Round(cantidad, 4), //Eac = cantidad 
                                                    precio_unitario = precio_unitario,
                                                    precio_ajustado = 0,
                                                    precio_base = precio_unitario,
                                                    precio_incrementado = 0,
                                                    costo_total = Decimal.Round(Decimal.Round(cantidad, 4) * precio_unitario, 4),
                                                    fecha_registro = DateTime.Now,
                                                    vigente = true,
                                                    estado = true,
                                                    Cambio = ComputoPresupuesto.TipoCambioComputo.Nuevo,
                                                    codigo_item_alterno = codigo.Length > 0 ? codigo : " "
                                                };
                                                var create = _repositoryComputoPresupuesto.Insert(n);

                                            }
                                        }

                                    }
                                }

                                if (WbsPresupuestoId > 0 &&
                                   ItemId == 0)
                                { //ITEMS QUE NO EXISTE NUEVOS PROCURA

                                    if ((
                                       codigo.Length > 0 &&
                                       nombre.Length > 0 &&
                                       unidad.Length > 0 &&
                                       cantidad > 0 &&
                                       precio_unitario > 0) || (
                                       codigo.Length > 0 &&
                                       nombre.Length > 0 &&
                                       unidad.Length > 0 &&
                                       cantidad < 0 &&
                                       precio_unitario > 0))
                                    {
                                        int ESubId = 0;
                                        var EspecialidadId = _repositorycatalogo.GetAll().Where(x => x.codigo == ProyectoCodigos.ESUBCONTRATOS).FirstOrDefault();

                                        var Unidad = _repositorycatalogo.GetAll().Where(x => x.nombre == unidad).FirstOrDefault();


                                        if (EspecialidadId != null && EspecialidadId.Id > 0)
                                        {
                                            ESubId = EspecialidadId.Id;
                                        }

                                        var eItem = itemrepository.GetAllIncluding(i => i.Grupo, i => i.Especialidad)
                                                                 .Where(i => i.nombre == nombre)
                                                                 .Where(i => i.vigente)
                                                                 .Where(i => i.Grupo.codigo == ProyectoCodigos.CODE_SUBCONTRATOS_CONTRATISTA)
                                                                 .FirstOrDefault();
                                        int IdNuevoItem = 0;
                                        if (eItem != null && eItem.Id > 0)
                                        {
                                            IdNuevoItem = eItem.Id;
                                        }
                                        else
                                        {
                                            Item nuevo = new Item
                                            {
                                                Id = 0,
                                                nombre = nombre,
                                                descripcion = nombre,
                                                GrupoId = 4,
                                                codigo = "7.2." + (_itemservice.GetItemsHijos("7.2.").Count() + 1),
                                                item_padre = "7.2.",
                                                para_oferta = true,
                                                UnidadId = Unidad != null && Unidad.Id > 0 ? Unidad.Id : 0,
                                                vigente = true,
                                                EspecialidadId = ESubId


                                            };

                                            IdNuevoItem = itemrepository.InsertAndGetId(nuevo);
                                        }
                                        ComputoPresupuesto n = new ComputoPresupuesto
                                        {

                                            ItemId = IdNuevoItem,
                                            WbsPresupuestoId = WbsPresupuestoId,
                                            cantidad = Decimal.Round(cantidad, 4),
                                            cantidad_eac = Decimal.Round(cantidad, 4), //Eac = cantidad
                                            precio_unitario = precio_unitario,
                                            precio_base = precio_unitario,
                                            precio_ajustado = 0,
                                            precio_incrementado = 0,//ver ganancia
                                            costo_total = Decimal.Round(cantidad * precio_unitario, 4),
                                            vigente = true,
                                            fecha_registro = DateTime.Now,
                                            estado = true,
                                            Cambio = ComputoPresupuesto.TipoCambioComputo.Nuevo,
                                            codigo_item_alterno = codigo
                                        };

                                        var computonuevo = _repositoryComputoPresupuesto.Insert(n);

                                    }


                                }















                            }

                        }
                        return "Ok";
                    }
                }
                else
                {
                    return "Debe Subir el Formato de Excel Exportado";
                }

            }
            else
            {
                return "sin_archivo";
            }
        }

        public List<PresupuestoDto> ListaPresupuestosLiberados()
        {
            List<PresupuestoDto> querylist = new List<PresupuestoDto>();
            var query = Repository.GetAllIncluding(c => c.Requerimiento).Where(c => c.vigente)
                                              .Where(c => c.es_final)
                                           .Where(c => c.estado_aprobacion == Presupuesto.EstadoAprobacion.Aprobado).ToList().OrderByDescending(c => c.fecha_registro).ToList();

            foreach (var presu in query)
            {
                var ofertaligada = _ofertaComercialPresupuestoRepo.GetAllIncluding(c => c.OfertaComercial.Catalogo)
                                                                .Where(c => c.PresupuestoId.HasValue)
                                                                .Where(c => c.vigente)
                                                                .Where(c => c.OfertaComercial.es_final == 1)
                                                                .Where(c => c.PresupuestoId == presu.Id)

                                                                .FirstOrDefault();
                if (ofertaligada != null && ofertaligada.Id > 0)
                {

                    var estadopresentado = _repositorycatalogo.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_PRESENTADO).FirstOrDefault();
                    var estadoaprobado = _repositorycatalogo.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_APROBADO).FirstOrDefault();
                    var estadocancelado = _repositorycatalogo.GetAll().Where(c => c.codigo == CatalogosCodigos.OFERTA_ESTADO_CANCELADO).FirstOrDefault();




                    if (ofertaligada.OfertaComercial.estado_oferta != estadopresentado.Id && ofertaligada.OfertaComercial.estado_oferta != estadoaprobado.Id && ofertaligada.OfertaComercial.estado_oferta != estadocancelado.Id || (presu.fecha_actualizacion.HasValue && ofertaligada.OfertaComercial.fecha_ultimo_envio.HasValue &&
                      ofertaligada.OfertaComercial.fecha_ultimo_envio.Value.Date < presu.fecha_actualizacion.Value.Date)) //Comparar solo fechas no la hora
                    {
                        querylist.Add(new PresupuestoDto()
                        {
                            Id = presu.Id,
                            codigo_requerimiento = presu.Requerimiento.codigo,
                            codigo_proyecto = presu.Requerimiento.Proyecto.codigo,
                            NombreEstadoAprobacion = presu.estado_aprobacion.ToString(),
                            fecha_registro = presu.fecha_registro,
                            monto_ingenieria = presu.monto_ingenieria,
                            monto_construccion = presu.monto_construccion,
                            monto_suministros = presu.monto_suministros,
                            codigo_oferta = ofertaligada.OfertaComercial.codigo + "_" + ofertaligada.OfertaComercial.version,
                            estado_oferta = ofertaligada.OfertaComercial.Catalogo.nombre,
                            monto_subcontratos = presu.monto_subcontratos,
                            monto_total = presu.monto_total,
                            fecha_actualizacion = presu.fecha_actualizacion,
                            version = presu.version,
                            formatFechaActualizacionPresupuesto = presu.fecha_actualizacion.HasValue ? presu.fecha_actualizacion.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                            formatFechaUltimoEnvioOferta = ofertaligada.OfertaComercial.fecha_ultimo_envio.HasValue ? ofertaligada.OfertaComercial.fecha_ultimo_envio.Value.ToString("dd/MM/yyyy HH:mm:ss") : ""



                        });
                    }

                }
                else
                {
                    querylist.Add(new PresupuestoDto()
                    {
                        Id = presu.Id,
                        codigo_requerimiento = presu.Requerimiento.codigo,
                        codigo_proyecto = presu.Requerimiento.Proyecto.codigo,
                        NombreEstadoAprobacion = presu.estado_aprobacion.ToString(),
                        fecha_registro = presu.fecha_registro,
                        monto_ingenieria = presu.monto_ingenieria,
                        monto_construccion = presu.monto_construccion,
                        monto_suministros = presu.monto_suministros,
                        codigo_oferta = "",
                        estado_oferta = "",
                        monto_subcontratos = presu.monto_subcontratos,
                        monto_total = presu.monto_total,
                        version = presu.version,
                        fecha_actualizacion = presu.fecha_actualizacion,
                        formatFechaActualizacionPresupuesto = "",
                        formatFechaUltimoEnvioOferta = ""


                    });
                }

            }



            return querylist;
        }

        public List<RequerimientoDto> ListadoRequerimientosCola()
        {
            var requerimientos = _repositoryRequerimiento.GetAll().Where(c => c.vigente).ToList();
            var presupuestos = Repository.GetAll().Where(c => c.vigente)
                                             .Where(c => c.es_final)
                                           .Where(c => c.estado_aprobacion == Presupuesto.EstadoAprobacion.Aprobado).ToList();


            var requerimientossinpresupuestoemitos = (from r in requerimientos
                                                      where !(from p in presupuestos select p.RequerimientoId).Contains(r.Id)

                                                      select new RequerimientoDto()
                                                      {
                                                          Id = r.Id,
                                                          proyecto_codigo = r.Proyecto.codigo,
                                                          codigo = r.codigo,
                                                          descripcion = r.descripcion,
                                                          monto_ingenieria = r.monto_ingenieria,
                                                          monto_construccion = r.monto_construccion,
                                                          monto_procura = r.monto_procura,
                                                          tiene_presupuesto = "Sin Presupuesto Emitido"
                                                      }).ToList();

            return requerimientossinpresupuestoemitos;
        }


        //eSTRUCTURA WBS arbol


        public List<WbsPresupuesto> EstructuraWbs(int PresupuestoId)
        {
            List<WbsPresupuesto> all = new List<WbsPresupuesto>();
            var wbs = _respositoryWbsPresupuesto.GetAll().Where(c => c.vigente)
                                         .Where(c => c.id_nivel_padre_codigo == ".")
                                          .Where(c => c.PresupuestoId == PresupuestoId).ToList();
            if (wbs.Count > 0)
            {
                var items_reordenados = (from e in wbs.ToList()
                                         orderby Convert.ToInt32(e.id_nivel_codigo.Replace(".", ""))
                                         select e).ToList();
                foreach (var item in items_reordenados)
                {
                    all.Add(item);
                    var lista = this.ObtenerWbsHijos(PresupuestoId, item.id_nivel_codigo, all);
                }
            }
            return all;

        }

        public List<WbsPresupuesto> ObtenerWbsHijos(int PresupuestoId, string id_nivel_codigo, List<WbsPresupuesto> estructura)
        {
            var wbs = _respositoryWbsPresupuesto.GetAll().Where(c => c.vigente)
                                       .Where(c => c.id_nivel_padre_codigo == id_nivel_codigo)
                                       .Where(c => c.PresupuestoId == PresupuestoId).ToList();
            if (wbs.Count > 0)
            {
                var items_reordenados = (from e in wbs.ToList()
                                         orderby Convert.ToInt32(e.id_nivel_codigo.Replace(".", ""))
                                         select e).ToList();
                foreach (var item in items_reordenados)
                {
                    estructura.Add(item);
                    var hijos = this.ObtenerWbsHijos(PresupuestoId, item.id_nivel_codigo, estructura);
                }
            }

            return estructura;
        }

        public string ValidarItemsProcuraExcel(HttpPostedFileBase UploadedFile, int PresupuestoId, int maximo_nivel, List<Item> lista_procura)
        {
            var ComputosPresupuestos = _repositoryComputoPresupuesto.GetAllIncluding(c => c.Item.Grupo)
                                                                  .Where(c => c.WbsPresupuesto.PresupuestoId == PresupuestoId)
                                                                  .Where(c => c.WbsPresupuesto.vigente)
                                                                  .Where(c => c.vigente)
                                                                  .Where(c => c.Item.Grupo.codigo == ProyectoCodigos.CODE_PROCURA_CONTRATISTA)
                                                                  .ToList();

            var filanuevos = maximo_nivel + lista_procura.Count + 3; // Fila de Items WNuevos
            if (UploadedFile != null)
            {
                if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string fileName = UploadedFile.FileName;
                    string fileContentType = UploadedFile.ContentType;
                    byte[] fileBytes = new byte[UploadedFile.ContentLength];
                    var data = UploadedFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(UploadedFile.ContentLength));

                    using (var package = new ExcelPackage(UploadedFile.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet[2];
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        List<string> repetidos = new List<string>();

                        for (int i = 6; i <= noOfCol - 1; i++)
                        {
                            for (int rowIterator = maximo_nivel + 3; rowIterator <= noOfRow; rowIterator++)
                            {

                                var para_oferta = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString();
                                var itemid = (workSheet.Cells[rowIterator, 2].Value ?? "").ToString();
                                var wbsid = (workSheet.Cells[maximo_nivel + 1, i].Value ?? "").ToString();
                                var cantidad = (workSheet.Cells[rowIterator, i].Value ?? "").ToString();
                                var valor = Decimal.Parse("0" + cantidad, NumberStyles.Float); //acepte 10 exp e-8, 0,000001

                                var precio_unitario = Decimal.Parse("0" + (workSheet.Cells[rowIterator, noOfCol].Value ?? "0").ToString());

                                var ITEMID = 0;
                                var WBSID = 0;
                                if (itemid.Length > 0)
                                {
                                    ITEMID = Int32.Parse(itemid);
                                }
                                if (wbsid.Length > 0)
                                {
                                    WBSID = Int32.Parse(wbsid);
                                }



                                //VARIABLES PARA SUBIR LOS QUE NO EXISTEN
                                var codigo = (workSheet.Cells[rowIterator, 3].Value ?? "").ToString();
                                var nombre = (workSheet.Cells[rowIterator, 4].Value ?? "").ToString();
                                var unidad = (workSheet.Cells[rowIterator, 5].Value ?? "").ToString();

                                if (WBSID > 0 &&
                                  ITEMID == 0 && codigo.Length > 0
                                  && nombre.Length > 0
                                  )
                                {
                                    var existeregistro = (from p in ComputosPresupuestos
                                                          where p.codigo_item_alterno == codigo
                                                          where p.Item.nombre.TrimEnd().TrimStart() == nombre.TrimEnd().TrimStart()
                                                          where p.WbsPresupuestoId == WBSID
                                                          where p.vigente
                                                          select p).FirstOrDefault();
                                    if (existeregistro != null && existeregistro.Id > 0)
                                    {
                                        repetidos.Add("Fila" + rowIterator + "\n");

                                    }
                                }
                            }
                        }
                        if (repetidos.Count > 0)
                        {
                            return "Existe items de procura duplicados: \n" + String.Join(",", repetidos.Distinct().ToList());
                        }

                        for (int i = 6; i <= noOfCol - 1; i++)
                        {
                            for (int rowIterator = filanuevos; rowIterator <= noOfRow; rowIterator++)
                            {

                                var para_oferta = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString();
                                var itemid = (workSheet.Cells[rowIterator, 2].Value ?? "").ToString();
                                var wbsid = (workSheet.Cells[maximo_nivel + 1, i].Value ?? "").ToString();
                                var cantidad = (workSheet.Cells[rowIterator, i].Value ?? "").ToString();
                                var valor = Decimal.Parse("0" + cantidad, NumberStyles.Float); //acepte 10 exp e-8, 0,000001

                                var precio_unitario = Decimal.Parse("0" + (workSheet.Cells[rowIterator, noOfCol].Value ?? "0").ToString());

                                var ITEMID = 0;
                                var WBSID = 0;
                                if (itemid.Length > 0)
                                {
                                    ITEMID = Int32.Parse(itemid);
                                }
                                if (wbsid.Length > 0)
                                {
                                    WBSID = Int32.Parse(wbsid);
                                }



                                //VARIABLES PARA SUBIR LOS QUE NO EXISTEN
                                var codigo = (workSheet.Cells[rowIterator, 3].Value ?? "").ToString();
                                var nombre = (workSheet.Cells[rowIterator, 4].Value ?? "").ToString();
                                var unidad = (workSheet.Cells[rowIterator, 5].Value ?? "").ToString();

                                var itemexiste = (from p in lista_procura
                                                  where p.nombre == nombre
                                                  where p.vigente
                                                  select p).FirstOrDefault();
                                if (itemexiste != null && itemexiste.Id > 0)
                                {
                                    return "Existe un item de procura duplicados:\n" + nombre + " (Fila" + rowIterator + ").";
                                }





                                if (WBSID > 0 &&
                                   ITEMID == 0 && nombre.Length > 0
                                  && precio_unitario == 0)
                                {


                                    return "Ocurrió un Error en la Línea: " + rowIterator + " en la Pestaña de Carga de Procura verifique P.U no puede ser cero";


                                }
                                if (WBSID > 0 &&
                                  ITEMID == 0 && nombre.Length > 0
                                 && codigo.Length == 0)
                                {


                                    return "Ocurrió un Error en la Línea: " + rowIterator + " en la Pestaña de Carga de Procura  debe ingresar un código alterno";


                                }
                            }


                        }
                        return "Ok";
                    }




                }
                else
                {
                    return "Debe Subir el Formato de Excel Exportado";
                }

            }
            else
            {
                return "sin_archivo";
            }
        }

        public string ValidarItemsSubExcel(HttpPostedFileBase UploadedFile, int PresupuestoId, int maximo_nivel, List<Item> lista_procura)
        {
            var ComputosPresupuestos = _repositoryComputoPresupuesto.GetAll()
                                                                  .Where(c => c.WbsPresupuesto.PresupuestoId == PresupuestoId)
                                                                  .Where(c => c.WbsPresupuesto.vigente)
                                                                  .Where(c => c.vigente)
                                                                  .Where(c => c.Item.Grupo.codigo == ProyectoCodigos.CODE_SUBCONTRATOS_CONTRATISTA)
                                                                  .ToList();

            var filanuevos = maximo_nivel + lista_procura.Count + 3; // Fila de Items WNuevos
            if (UploadedFile != null)
            {
                if (UploadedFile.ContentType == "application/vnd.ms-excel" || UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string fileName = UploadedFile.FileName;
                    string fileContentType = UploadedFile.ContentType;
                    byte[] fileBytes = new byte[UploadedFile.ContentLength];
                    var data = UploadedFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(UploadedFile.ContentLength));

                    using (var package = new ExcelPackage(UploadedFile.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet[3];
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;


                        for (int i = 6; i <= noOfCol - 1; i++)
                        {
                            for (int rowIterator = maximo_nivel + 3; rowIterator <= noOfRow; rowIterator++)
                            {

                                var para_oferta = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString();
                                var itemid = (workSheet.Cells[rowIterator, 2].Value ?? "").ToString();
                                var wbsid = (workSheet.Cells[maximo_nivel + 1, i].Value ?? "").ToString();
                                var cantidad = (workSheet.Cells[rowIterator, i].Value ?? "").ToString();
                                var valor = Decimal.Parse("0" + cantidad, NumberStyles.Float); //acepte 10 exp e-8, 0,000001

                                var precio_unitario = Decimal.Parse("0" + (workSheet.Cells[rowIterator, noOfCol].Value ?? "0").ToString());

                                var ITEMID = 0;
                                var WBSID = 0;
                                if (itemid.Length > 0)
                                {
                                    ITEMID = Int32.Parse(itemid);
                                }
                                if (wbsid.Length > 0)
                                {
                                    WBSID = Int32.Parse(wbsid);
                                }



                                //VARIABLES PARA SUBIR LOS QUE NO EXISTEN
                                var codigo = (workSheet.Cells[rowIterator, 3].Value ?? "").ToString();
                                var nombre = (workSheet.Cells[rowIterator, 4].Value ?? "").ToString();
                                var unidad = (workSheet.Cells[rowIterator, 5].Value ?? "").ToString();

                                if (WBSID > 0 &&
                                  ITEMID == 0 && codigo.Length > 0)
                                {
                                    var existeregistro = (from p in ComputosPresupuestos
                                                          where p.codigo_item_alterno == codigo
                                                          where p.WbsPresupuestoId == WBSID
                                                          where p.vigente
                                                          select p).FirstOrDefault();
                                    if (existeregistro != null && existeregistro.Id > 0)
                                    {

                                        return "Ocurrió un Error en la Línea: " + rowIterator + "Subcontratos El Item ya esta registrado en la columna de computos con el mismo nombre verifica o descarga el formato nuevamente";
                                    }
                                }
                            }
                        }


                        for (int i = 6; i <= noOfCol - 1; i++)
                        {
                            for (int rowIterator = filanuevos; rowIterator <= noOfRow; rowIterator++)
                            {

                                var para_oferta = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString();
                                var itemid = (workSheet.Cells[rowIterator, 2].Value ?? "").ToString();
                                var wbsid = (workSheet.Cells[maximo_nivel + 1, i].Value ?? "").ToString();
                                var cantidad = (workSheet.Cells[rowIterator, i].Value ?? "").ToString();
                                var valor = Decimal.Parse("0" + cantidad, NumberStyles.Float); //acepte 10 exp e-8, 0,000001

                                var precio_unitario = Decimal.Parse("0" + (workSheet.Cells[rowIterator, noOfCol].Value ?? "0").ToString());

                                var ITEMID = 0;
                                var WBSID = 0;
                                if (itemid.Length > 0)
                                {
                                    ITEMID = Int32.Parse(itemid);
                                }
                                if (wbsid.Length > 0)
                                {
                                    WBSID = Int32.Parse(wbsid);
                                }



                                //VARIABLES PARA SUBIR LOS QUE NO EXISTEN
                                var codigo = (workSheet.Cells[rowIterator, 3].Value ?? "").ToString();
                                var nombre = (workSheet.Cells[rowIterator, 4].Value ?? "").ToString();
                                var unidad = (workSheet.Cells[rowIterator, 5].Value ?? "").ToString();

                                var itemexiste = (from p in lista_procura
                                                  where p.nombre == nombre
                                                  where p.vigente
                                                  select p).FirstOrDefault();
                                if (itemexiste != null && itemexiste.Id > 0)
                                {
                                    return "Ocurrió un Error en la Línea: " + rowIterator + " Subcontratos El Item ya esta creado con el mismo nombre verifica en la lista principal";
                                }





                                if (WBSID > 0 &&
                                   ITEMID == 0 && nombre.Length > 0
                                  && precio_unitario == 0)
                                {


                                    return "Ocurrió un Error en la Línea: " + rowIterator + " en la Pestaña de Carga de Subcontratos verifique P.U no puede ser cero";


                                }
                                if (WBSID > 0 &&
                                  ITEMID == 0 && nombre.Length > 0
                                 && codigo.Length == 0)
                                {


                                    return "Ocurrió un Error en la Línea: " + rowIterator + " en la Pestaña de Carga de Subcontratos  debe ingresar un código alterno";


                                }
                            }


                        }
                        return "Ok";
                    }




                }
                else
                {
                    return "Debe Subir el Formato de Excel Exportado";
                }

            }
            else
            {
                return "sin_archivo";
            }
        }

        public void EnviarMontosPresupuestoReq(int id)
        {
            var pre = Repository.Get(id);
            var req = _repositoryRequerimiento.Get(pre.RequerimientoId);

            var presupuestos = Repository.GetAll().Where(c => c.RequerimientoId == req.Id).Where
                (c => c.vigente).Where(c => c.es_final).ToList();

            if (presupuestos.Count > 0)
            {

                decimal mi = (from p in presupuestos select p.monto_ingenieria).Sum();
                decimal mc = (from p in presupuestos select p.monto_construccion).Sum();
                decimal mp = (from p in presupuestos select p.monto_suministros).Sum();
                decimal msub = (from p in presupuestos select p.monto_subcontratos).Sum();
                decimal mto = (from p in presupuestos select p.monto_total).Sum();
                req.monto_ingenieria = mi;
                req.monto_construccion = mc;
                req.monto_procura = mp;
                req.monto_subcontrato = msub;
                req.monto_total = mto;

                var resultado = _repositoryRequerimiento.Update(req);
            }


        }

        public string IngresarItemsPresupuestoExcel(HttpPostedFileBase UploadedFile, int maximo_nivel, int PresupuestoId)
        {

            var ComputosPresupuestos = _repositoryComputoPresupuesto.GetAll()
                                                                       .Where(c => c.WbsPresupuesto.PresupuestoId == PresupuestoId)
                                                                       .Where(c => c.WbsPresupuesto.vigente)
                                                                       .Where(c => c.vigente)
                                                                       .ToList();

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
                    //SUBIR COMPUTOS PRESUPUESTOS-ITEM ICS

                    //PACKETES DE EXCEL
                    var pestana = package.Workbook.Worksheets;
                    var hoja = pestana.First();
                    var numerodecolumnas = hoja.Dimension.End.Column;
                    var numerodefilas = hoja.Dimension.End.Row;

                    //ITERAR LOS ITEMS

                    for (int c = 6; c <= numerodecolumnas; c++) //i=6 columna donde comienzan los items,
                    {
                        for (int f = maximo_nivel + 3; f <= numerodefilas; f++)
                        {

                            //CAMPOS
                            var campo_item = (hoja.Cells[f, 2].Value ?? "").ToString();
                            var campo_wbs = (hoja.Cells[maximo_nivel + 1, c].Value ?? "").ToString();
                            var campo_para_oferta = (hoja.Cells[f, 1].Value ?? "").ToString();
                            var campo_cantidad = (hoja.Cells[f, c].Value ?? "").ToString();

                            //VALORES
                            var ItemId = 0;
                            var WbsPresupuestoId = 0;

                            decimal cantidad = Convert.ToDecimal(0);

                            if (campo_cantidad.Length > 0 && campo_cantidad.Contains('-'))
                            {
                                ElmahExtension.LogToElmah(new Exception("Fila: " + f + "Col: " + c + " Cantidad: " + campo_cantidad));
                                /* try{*/
                                cantidad = Decimal.Parse(campo_cantidad, NumberStyles.Float); //Para que acepte 10 exp e-8, 0,000001
                                                                                              /*    }
                                                                                           catch (Exception e)
                                                                                        {
                                                                                            ElmahExtension.LogToElmah(new Exception("Fila: " + f + "Col: " + c + " Cantidad: " + campo_cantidad + " Error: " + e.Message));

                                                                                        }*/
                            }
                            else
                            {
                                try
                                {
                                    cantidad = Decimal.Parse("0" + campo_cantidad, NumberStyles.Float); //Para que acepte 10 exp e-8, 0,000001
                                }
                                catch (Exception e)
                                {
                                    ElmahExtension.LogToElmah(new Exception("Fila: " + f + "Col: " + c + " Cantidad: " + campo_cantidad + " Error " + e.Message));

                                }

                            }


                            //CONVERSIONES
                            if (campo_item.Length > 0)
                            {
                                ItemId = Int32.Parse(campo_item);
                            }
                            if (campo_wbs.Length > 0)
                            {
                                WbsPresupuestoId = Int32.Parse(campo_wbs);
                            }

                            //VERIFICACIONES

                            if (ItemId > 0 &&
                                WbsPresupuestoId > 0 &&
                                campo_para_oferta.Length > 0 &&
                                campo_para_oferta.Equals("True"))

                            {// Cumple con los requerimientos para ingresar a las casillas

                                if (campo_cantidad.Length == 0) //campo vacio
                                {
                                    ComputoPresupuesto com = (from e in ComputosPresupuestos
                                                              where e.WbsPresupuestoId == WbsPresupuestoId
                                                              where e.ItemId == ItemId
                                                              select e).FirstOrDefault();
                                    if (com != null && com.Id > 0) // Si encontro el Item
                                    {
                                        //com.vigente = false; //Elimina
                                        com.cantidad = 0;
                                        com.cantidad_eac = 0;
                                        var update = _repositoryComputoPresupuesto.Update(com);
                                    }
                                }
                                else
                                if (campo_cantidad.Length > 0 && cantidad >= 0 || campo_cantidad.Length > 0 && cantidad < 0)//campo con dato positivo o negativo
                                {

                                    ComputoPresupuesto com = (from e in ComputosPresupuestos
                                                              where e.WbsPresupuestoId == WbsPresupuestoId
                                                              where e.ItemId == ItemId
                                                              select e).FirstOrDefault();

                                    if (com != null && com.Id > 0) // Si encontro el Item
                                    {
                                        if (cantidad > 0 || cantidad < 0) //Verifica si es positivo o negativo
                                        {
                                            if (com.cantidad != cantidad)
                                            { //Si es diferente actualiza si no nope
                                                com.cantidad = Decimal.Round(cantidad, 4);
                                                com.cantidad_eac = Decimal.Round(cantidad, 4);
                                                com.Cambio = ComputoPresupuesto.TipoCambioComputo.Editado;
                                                var update = _repositoryComputoPresupuesto.Update(com);
                                            }
                                        }
                                        else
                                        {
                                            //com.vigente = false; //Elimina
                                            com.cantidad = 0;
                                            com.cantidad_eac = 0;
                                            var update = _repositoryComputoPresupuesto.Update(com);
                                        }
                                    }
                                    else // No encontro el Item
                                    {
                                        if (cantidad > 0 || cantidad < 0)
                                        {// si cantidad mayor a cero? se crea el computo: Nope

                                            ComputoPresupuesto n = new ComputoPresupuesto()
                                            {
                                                ItemId = ItemId,
                                                WbsPresupuestoId = WbsPresupuestoId,
                                                cantidad = Decimal.Round(cantidad, 4),
                                                cantidad_eac = Decimal.Round(cantidad, 4), //Eac = cantidad 
                                                precio_unitario = 0,
                                                precio_ajustado = 0,
                                                precio_base = 0,
                                                precio_incrementado = 0,
                                                costo_total = 0,
                                                fecha_registro = DateTime.Now,
                                                vigente = true,
                                                estado = true,
                                                Cambio = ComputoPresupuesto.TipoCambioComputo.Nuevo
                                            };
                                            var create = _repositoryComputoPresupuesto.Insert(n);

                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return "Ok";
        }

        public ExcelPackage MatrizPresupuestoSecondFormat(PresupuestoDto oferta, int nivel_maximo, bool reporte)
        {
            #region Query WBS
            var wbs = this.EstructuraWbs(oferta.Id);


            #endregion
            #region Query Computos
            var listacomputos = _repositoryComputoPresupuesto
                                                .GetAllIncluding
                                                (x => x.WbsPresupuesto.Presupuesto.Proyecto,
                                                 x => x.Item.Especialidad,
                                                 x => x.Item.Grupo)
                                                 .Where(x => x.Item.EspecialidadId.HasValue)
                                                .Where(x => x.vigente == true)
                                                .Where(x => x.WbsPresupuesto.PresupuestoId == oferta.Id)
                                                .ToList();

            var computos = (from z in listacomputos
                            where z.WbsPresupuesto.PresupuestoId == oferta.Id
                            where z.vigente == true
                            select new ComputoPresupuestoDto
                            {
                                Id = z.Id,
                                ItemId = z.ItemId,
                                WbsPresupuestoId = z.WbsPresupuestoId,
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
                                Cambio = z.Cambio,
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
            #endregion
            #region Estructura Items
            var items = _itemservice.ItemsMatrizPresupuesto(oferta.Proyecto.contratoId, oferta.fecha_registro.GetValueOrDefault(), computos);
            #endregion
            #region Cabecera Excel
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Carga Computo");
            workSheet.TabColor = System.Drawing.Color.Azure;
            workSheet.DefaultRowHeight = 15;
            workSheet.View.ZoomScale = 80;
            int row = nivel_maximo;
            for (int i = 1; i <= row; i++)
            {

                workSheet.Row(i).Style.Font.Bold = true;
            }
            #endregion

            int columna = 6;

            #region Cabecera Superior WBS
            foreach (var itemswbs in wbs.Where(x => x.es_actividad).ToList())
            {
                int fila = nivel_maximo + 4;
                List<WbsPresupuesto> Jerarquia = new List<WbsPresupuesto>();
                WbsPresupuesto item = _respositoryWbsPresupuesto.Get(itemswbs.Id);
                Jerarquia.Add(item);
                while (item.id_nivel_padre_codigo != ".")
                {
                    item = _respositoryWbsPresupuesto.GetAll()
                        .Where(X => X.id_nivel_codigo == item.id_nivel_padre_codigo)
                        .Where(C => C.vigente)
                        .Where(C => C.PresupuestoId == oferta.Id)
                        .FirstOrDefault();

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
            #endregion
            #region Cabecera Items
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

            workSheet.Row(nivel_maximo).Height = 60;


            //int inicio de las filas
            //int c = 5;
            int c = nivel_maximo + 3;
            workSheet.View.FreezePanes(6, 1);
            workSheet.View.FreezePanes(c, 6);


            #endregion
            #region Filas Items
            //items = new List<Item>();
            foreach (var pitem in items)
            {
                workSheet.Cells[c, 1].Value = pitem.Grupo != null ? pitem.Grupo.codigo : "";
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
            #endregion

            #region EMPIEZA PARTE DE LA DERECHA CALCULOS Y TODO

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
            #endregion


            for (int j = 6; j <= noOfCol; j++)
            {
                var celdawbsid = (workSheet.Cells[nivel_maximo + 1, j].Value ?? "").ToString();

                for (int i = nivel_maximo + 3; i <= noOfRow; i++)
                {

                    var celdaitemid = (workSheet.Cells[i, 2].Value ?? "").ToString();

                    int WbsId = 0;
                    int ItemId = 0;
                    if (celdawbsid.Length > 0)
                    {
                        WbsId = Convert.ToInt32(celdawbsid);
                    }
                    if (celdaitemid.Length > 0)
                    {
                        ItemId = Convert.ToInt32(celdaitemid);
                    }

                    var computo = (from d in computos
                                   where d.WbsPresupuestoId == WbsId
                                   where d.ItemId == ItemId
                                   where d.vigente
                                   select d
                                 ).FirstOrDefault();

                    if (computo != null && computo.Id > 0)
                    {
                        if (reporte)
                        {
                            if (computo.Cambio != null && computo.Cambio == ComputoPresupuesto.TipoCambioComputo.Nuevo)
                            {
                                workSheet.Cells[i, j].Style.Border.Top.Style =
                                workSheet.Cells[i, j].Style.Border.Left.Style =
                                workSheet.Cells[i, j].Style.Border.Right.Style =
                                workSheet.Cells[i, j].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                workSheet.Cells[i, j].Style.Border.Top.Style =
                                workSheet.Cells[i, j].Style.Border.Left.Style =
                              workSheet.Cells[i, j].Style.Border.Right.Style =
                              workSheet.Cells[i, j].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                workSheet.Cells[i, j].Style.Border.Top.Color.SetColor(Color.Red);
                                workSheet.Cells[i, j].Style.Border.Bottom.Color.SetColor(Color.Red);
                                workSheet.Cells[i, j].Style.Border.Left.Color.SetColor(Color.Red);
                                workSheet.Cells[i, j].Style.Border.Right.Color.SetColor(Color.Red);

                            }
                            if (computo.Cambio != null && computo.Cambio == ComputoPresupuesto.TipoCambioComputo.Editado)
                            {
                                workSheet.Cells[i, j].Style.Border.Top.Style =
                                workSheet.Cells[i, j].Style.Border.Left.Style =
                                workSheet.Cells[i, j].Style.Border.Right.Style =
                                workSheet.Cells[i, j].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                workSheet.Cells[i, j].Style.Border.Top.Style =
                                workSheet.Cells[i, j].Style.Border.Left.Style =
                              workSheet.Cells[i, j].Style.Border.Right.Style =
                              workSheet.Cells[i, j].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

                                workSheet.Cells[i, j].Style.Border.Top.Color.SetColor(Color.Blue);
                                workSheet.Cells[i, j].Style.Border.Bottom.Color.SetColor(Color.Blue);
                                workSheet.Cells[i, j].Style.Border.Left.Color.SetColor(Color.Blue);
                                workSheet.Cells[i, j].Style.Border.Right.Color.SetColor(Color.Blue);

                            }
                        }

                        workSheet.Cells[i, j].Value = computo.cantidad;
                        workSheet.Cells[i, noOfCol + 2].Value = computo.precio_unitario;
                        workSheet.Cells[i, j].Style.Numberformat.Format = "#,##0.00";
                        workSheet.Cells[i, noOfCol + 1].Style.Numberformat.Format = "#,##0.00";
                        workSheet.Cells[i, noOfCol + 2].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";
                        workSheet.Cells[i, noOfCol + 3].Style.Numberformat.Format = "_-$* #,##0.00_-;-$* #,##0.00_-;_-$* \"-\"??_-;_-@_-";

                    }

                }




            }


            int ncol = workSheet.Dimension.End.Column;
            if (items.Count > 0)
            {
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
                    workSheet.Cells[i, ncol].FormulaR1C1 = "=$" + dcantidad + "*$" + dprecio;

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
            }
            #region Cuerpo Antiguo
            /*
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

    */
            #endregion

            #region Totales Formato Contrato 2

            var t = this.TotalesSecondFormat(computos);
            var hoja = workSheet;
            var rowf = noOfRow + 1;

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
            hoja.Cells[cell].Value = "Administracion Subcontratos  (%)";
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
            return excel;
        }

        public decimal ObtenerPrecioUnitarioItem(int ItemId, int PresupuestoId)
        {

            decimal PU = 0;

            var presupuesto = Repository.GetAllIncluding(c => c.Proyecto).Where(c => c.vigente).Where(c => c.Id == PresupuestoId).FirstOrDefault();

            if (presupuesto != null && presupuesto.Id > 0)
            {
                DateTime fecha = Convert.ToDateTime(presupuesto.fecha_registro);

                var item = _repositorydetallepreciario.GetAllIncluding(c => c.Preciario)
                                              .Where(c => c.vigente)
                                              .Where(c => c.ItemId == ItemId)
                                              .Where(c => c.Preciario.vigente)
                                              .Where(c => fecha >= c.Preciario.fecha_desde)
                                              .Where(c => fecha <= c.Preciario.fecha_hasta)
                                              .Where(c => c.Preciario.ContratoId == presupuesto.Proyecto.contratoId).FirstOrDefault();

                if (item != null && item.Id > 0)
                {
                    PU = item.precio_unitario;
                }

            }
            return PU;
        }

        public TotalesSegundoContrato TotalesSecondFormat(List<ComputoPresupuestoDto> computos)
        {
            if (computos.Count > 0)
            {
                TotalesSegundoContrato t = new TotalesSegundoContrato();
                t.A_VALOR_COSTO_TOTAL_INGENIERÍA_BASICA_YDETALLE_AIU_ANEXO1 = (from c in computos
                                                                               where c.codigo_grupo == ProyectoCodigos.CODE_INGENIERIA
                                                                               select c.costo_total).Sum();
                t.VALOR_PROCURA = (from c in computos
                                   where c.codigo_grupo == ProyectoCodigos.CODE_PROCURA_CONTRATISTA
                                   select c.costo_total).Sum();
                t.Administracion_sobre_Procura_Contratista = (from c in computos
                                                              where c.codigo_grupo == ProyectoCodigos.CODE_PROCURA_CONTRATISTA
                                                              select c.costo_total).Sum() * Convert.ToDecimal(0.10);

                t.B_VALOR_COSTO_DIRECTO_PROCURA_CONTRATISTA_ANEXO2 = t.VALOR_PROCURA + t.Administracion_sobre_Procura_Contratista;
                t.VALOR_SUBCONTRATOS = (from c in computos
                                        where c.codigo_grupo == ProyectoCodigos.CODE_SUBCONTRATOS_CONTRATISTA
                                        select c.costo_total).Sum();
                t.Administracion_sobre_Subcontratos_Contratista = (from c in computos
                                                                   where c.codigo_grupo == ProyectoCodigos.CODE_SUBCONTRATOS_CONTRATISTA
                                                                   select c.costo_total).Sum() * Convert.ToDecimal(0.1838);

                t.C_VALOR_COSTO_DIRECTO_SUBCONTRATOS_CONTRATISTA = t.VALOR_SUBCONTRATOS + t.Administracion_sobre_Subcontratos_Contratista;

                t.VALOR_COSTO_DIRECTO_OBRAS_CIVILES = (from c in computos
                                                       where c.codigo_especialidad == ProyectoCodigos.OBRAS_CIVILES
                                                       select c.costo_total).Sum();
                t.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS = (from c in computos
                                                         where c.codigo_especialidad == ProyectoCodigos.OBRAS_MECANICAS
                                                         select c.costo_total).Sum();
                t.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS = (from c in computos
                                                          where c.codigo_especialidad == ProyectoCodigos.OBRAS_ELECTRICAS
                                                          select c.costo_total).Sum();
                t.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL = (from c in computos
                                                                     where c.codigo_especialidad == ProyectoCodigos.OBRAS_INSTRUMENTOS_CONTROL
                                                                     select c.costo_total).Sum();
                t.VALOR_COSTO_DIRECTO_SERVICIOS_ESPECIALES = (from c in computos
                                                              where c.codigo_especialidad == ProyectoCodigos.SERVICIOS_EPECIALES
                                                              select c.costo_total).Sum();

                t.DESCUENTO_ITEMS_MECÁNICOS_ELÉCTRICOS_INSTRUMENTACIÓN = //(t.VALOR_COSTO_DIRECTO_OBRAS_CIVILES * Convert.ToDecimal(0.01))+
                                                                        (t.VALOR_COSTO_DIRECTO_OBRAS_MECANICAS * Convert.ToDecimal(0.01)) +
                                                                        (t.VALOR_COSTO_DIRECTO_OBRAS_ELECTRICAS * Convert.ToDecimal(0.01)) +
                                                                        (t.VALOR_COSTO_DIRECTO_OBRAS_INSTRUMENTO_Y_CONTROL * Convert.ToDecimal(0.01)) +
                                                                         (t.VALOR_COSTO_DIRECTO_SERVICIOS_ESPECIALES * Convert.ToDecimal(0.01))
                                                                        ;
                t.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN = (from c in computos
                                                        where c.codigo_grupo == ProyectoCodigos.CODE_CONSTRUCCION
                                                        select c.costo_total).Sum();
                t.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN = t.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN - t.DESCUENTO_ITEMS_MECÁNICOS_ELÉCTRICOS_INSTRUMENTACIÓN;

                t.Administracion_sobre_Obra = t.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN * Convert.ToDecimal(0.4119);
           
                t.Imprevistos_sobre_Obra = t.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN * Convert.ToDecimal(0.03);
     
                t.Utilidad_sobre_Obra = t.D_VALOR_COSTO_DIRECTO_CONSTRUCCIÓN * Convert.ToDecimal(0.12);
           
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

        public TotalesSegundoContrato TotalesSecondFormatPresupuesto(int Id)
        {
            #region Query Computos
            var listacomputos = _repositoryComputoPresupuesto
                                                .GetAllIncluding
                                                (x => x.WbsPresupuesto.Presupuesto.Proyecto,
                                                 x => x.Item.Especialidad,
                                                 x => x.Item.Grupo)
                                                 .Where(x => x.Item.EspecialidadId.HasValue)
                                                .Where(x => x.vigente == true)
                                                .Where(x => x.WbsPresupuesto.PresupuestoId == Id)
                                                .ToList();

            var computos = (from z in listacomputos
                            where z.WbsPresupuesto.PresupuestoId == Id
                            where z.vigente == true
                            select new ComputoPresupuestoDto
                            {
                                Id = z.Id,
                                ItemId = z.ItemId,
                                WbsPresupuestoId = z.WbsPresupuestoId,
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
                                Cambio = z.Cambio,
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
            #endregion

            var t = this.TotalesSecondFormat(computos);
            return t;
        }

        public Contrato ObtenerContratoFromPresupuesto(int Id)
        {
            var presupuesto = Repository.GetAllIncluding(c => c.Proyecto.Contrato).Where(c => c.Id == Id).FirstOrDefault();
            var Contrato = presupuesto.Proyecto.Contrato;
            return Contrato;
        }

        public List<string> ValidarNumerosNegativos(HttpPostedFileBase UploadedFile, int maximo_nivel, int PresupuestoId)
        {
            List<string> ListNegativos = new List<string>();
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
                    //ITEMS 1 -9
                    var pestana = package.Workbook.Worksheets;
                    var hoja = pestana.First();
                    var numerodecolumnas = hoja.Dimension.End.Column;
                    var numerodefilas = hoja.Dimension.End.Row;

                    //ITERAR LOS ITEMS

                    for (int c = 6; c <= numerodecolumnas; c++) //i=6 columna donde comienzan los items,
                    {
                        for (int f = maximo_nivel + 3; f <= numerodefilas; f++)
                        {
                            var campo_codigoItem = (hoja.Cells[f, 3].Value ?? "").ToString();
                            var campo_cantidad = (hoja.Cells[f, c].Value ?? "").ToString();
                            if (campo_cantidad.Length > 0 && campo_cantidad.Contains('-'))
                            {
                                ListNegativos.Add("fila #" + f + " - Item: '" + campo_codigoItem + "' (Ingeniería)");
                            }
                        }
                    }

                    //ITEMS PROCURA
                    hoja = pestana[2];
                    numerodecolumnas = hoja.Dimension.End.Column;
                    numerodefilas = hoja.Dimension.End.Row;

                    for (int c = 6; c <= numerodecolumnas; c++) //i=6 columna donde comienzan los items,
                    {
                        for (int f = maximo_nivel + 3; f <= numerodefilas; f++)
                        {
                            var campo_codigoItem = (hoja.Cells[f, 3].Value ?? "").ToString();
                            var campo_cantidad = (hoja.Cells[f, c].Value ?? "").ToString();
                            if (campo_cantidad.Length > 0 && campo_cantidad.Contains('-'))
                            {
                                ListNegativos.Add("fila #" + f + " - Item: '" + campo_codigoItem + "' (Procura)");
                            }
                        }
                    }

                    //SUBCONTRATOS
                    hoja = pestana[3];
                    numerodecolumnas = hoja.Dimension.End.Column;
                    numerodefilas = hoja.Dimension.End.Row;

                    for (int c = 6; c <= numerodecolumnas; c++) //i=6 columna donde comienzan los items,
                    {
                        for (int f = maximo_nivel + 3; f <= numerodefilas; f++)
                        {
                            var campo_codigoItem = (hoja.Cells[f, 3].Value ?? "").ToString();
                            var campo_cantidad = (hoja.Cells[f, c].Value ?? "").ToString();
                            if (campo_cantidad.Length > 0 && campo_cantidad.Contains('-'))
                            {
                                ListNegativos.Add("fila #" + f + " - Item: '" + campo_codigoItem + "' (SubContratos)");
                            }
                        }
                    }

                }
            }

            return ListNegativos;
        }




        public int GuardarArchivo(int PresupuestoId, HttpPostedFileBase UploadedFile)
        {

            if (UploadedFile != null)
            {


                var contador = this.ListaArchivos(PresupuestoId).Count + 1;
                string fileName = UploadedFile.FileName;
                string fileContentType = UploadedFile.ContentType;
                byte[] fileBytes = new byte[UploadedFile.ContentLength];
                var data = UploadedFile.InputStream.Read(fileBytes, 0,
                    Convert.ToInt32(UploadedFile.ContentLength));

                ArchivoPresupuesto n = new ArchivoPresupuesto
                {
                    Id = 0,
                    nombre = fileName,
                    fecha_registro = DateTime.Now,
                    hash = fileBytes,
                    tipo_contenido = fileContentType,
                    PresupuestoId = PresupuestoId
                };
                var archivoid = _archivoPresupuesto.InsertAndGetId(n);

                if (archivoid > 0)
                {
                    return 1;

                }
                else
                {
                    return 0;

                }

            }
            return 0;
        }

        public List<ArchivoPresupuestoDto> ListaArchivos(int PresupuestoId)
        {
            var listaarhivos = _archivoPresupuesto.GetAll().Where(c => c.PresupuestoId == PresupuestoId).ToList();

            var items = (from a in listaarhivos
                         select new ArchivoPresupuestoDto
                         {
                             Id = a.Id,
                             PresupuestoId = a.PresupuestoId,
                             fecha_registro = a.fecha_registro,
                             formatFechaRegistro = a.fecha_registro.ToShortDateString(),
                             hash = a.hash,
                             nombre = a.nombre,
                             tipo_contenido = a.tipo_contenido
                         }).ToList();

            foreach (var i in items)
            {
                i.filebase64 = Convert.ToBase64String(i.hash);
            }
            return items;
        }

        public int EditarArchivo(int Id, HttpPostedFileBase UploadedFile)
        {
            var archivo = _archivoPresupuesto.Get(Id);
            if (UploadedFile != null)
            {
                string fileName = UploadedFile.FileName;
                string fileContentType = UploadedFile.ContentType;
                byte[] fileBytes = new byte[UploadedFile.ContentLength];
                var data = UploadedFile.InputStream.Read(fileBytes, 0,
                    Convert.ToInt32(UploadedFile.ContentLength));

                archivo.hash = fileBytes;
                archivo.nombre = fileName;
                archivo.fecha_registro = DateTime.Now;
                archivo.tipo_contenido = fileContentType;

                var resultado = _archivoPresupuesto.Update(archivo);
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public int EliminarArchivo(int id)
        {
            var archivo = _archivoPresupuesto.Get(id);
            int ofertaComercialID = archivo.PresupuestoId;
            _archivoPresupuesto.Delete(archivo);
            return ofertaComercialID;
        }
        public ArchivoPresupuesto DetalleArchivo(int id)
        {
            return _archivoPresupuesto.Get(id);
        }
        public string hrefoutlook(int id)
        {
            var o = Repository.Get(id);
             string asunto = o.asuntoCorreo;
            string descripcion = o.descripcionCorreo;

            string href = "mailto:" + "?subject=" + asunto + "&body=" + descripcion;
            return href;
        }


    }
}




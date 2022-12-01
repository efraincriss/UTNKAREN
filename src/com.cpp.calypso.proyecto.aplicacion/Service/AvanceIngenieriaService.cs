using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using Castle.Components.DictionaryAdapter;
using Castle.MicroKernel.ModelBuilder.Descriptors;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using OfficeOpenXml;
using System.IO;
using com.cpp.calypso.proyecto.dominio.Models;
using com.cpp.calypso.proyecto.dominio.Constantes;
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto.aplicacion
{
    public class AvanceIngenieriaAsyncBaseCrudAppService :
        AsyncBaseCrudAppService<AvanceIngenieria, AvanceIngenieriaDto, PagedAndFilteredResultRequestDto>,
        IAvanceIngenieriaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<DetalleAvanceIngenieria> _detalleAvanceIngenieriaRepository;
        private readonly IBaseRepository<Proyecto> _proyectoRepository;
        private readonly IBaseRepository<Oferta> _ofertaRepository;
        private readonly IBaseRepository<DetalleItemIngenieria> _detalleitemingenieria;
        private readonly IBaseRepository<Wbs> _wbsrepository;
        private readonly IBaseRepository<Computo> _computorepository;
        private readonly IBaseRepository<Colaborador> _colaboradorepository;
        private readonly IDetalleAvanceIngenieriaAsyncBaseCrudAppService _detalleAvanceIngenieria;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<ColaboradorIngenieria> _colirepository;
        IBaseRepository<Certificado> _certificadopository;

        public AvanceIngenieriaAsyncBaseCrudAppService(
            IBaseRepository<AvanceIngenieria> repository,
            IBaseRepository<DetalleAvanceIngenieria> detalleAvanceIngenieriaRepository,
            IBaseRepository<DetalleItemIngenieria> detalleitemingenieria,
            IBaseRepository<Oferta> ofertaRepository,
            IBaseRepository<Proyecto> proyectoRepository,
            IBaseRepository<Wbs> wbsrepository,
            IBaseRepository<Computo> computorepository,
            IBaseRepository<Colaborador> colaboradorepository,
            IDetalleAvanceIngenieriaAsyncBaseCrudAppService detalleAvanceIngenieria,
            IBaseRepository<Catalogo> catalogoRepository,
            IBaseRepository<ColaboradorIngenieria> colirepository,
            IBaseRepository<Certificado> certificadopository
        ) : base(repository)
        {
            _detalleAvanceIngenieriaRepository = detalleAvanceIngenieriaRepository;
            _detalleitemingenieria = detalleitemingenieria;
            _proyectoRepository = proyectoRepository;
            _ofertaRepository = ofertaRepository;
            _wbsrepository = wbsrepository;
            _computorepository = computorepository;
            _colaboradorepository = colaboradorepository;
            _detalleAvanceIngenieria = detalleAvanceIngenieria;
            _catalogoRepository = catalogoRepository;
            _colirepository = colirepository;
            _certificadopository = certificadopository;
        }

        public decimal GetMontoPresupuestado(int ofertaId)
        {   //
            decimal monto_presupuestado = 0;
            var computos = _detalleAvanceIngenieria.GetComputos(ofertaId);

            foreach (var c in computos)
            {
                monto_presupuestado += c.costo_total;
            }

            return monto_presupuestado;
        }

        public List<AvanceIngenieriaDto> ListarPorOferta(int ofertaId)
        {
            var query = Repository.GetAllIncluding(o => o.Oferta.Proyecto).Where(o => o.vigente == true)
                .Where(o => o.OfertaId == ofertaId);

            var items = (from a in query
                         select new AvanceIngenieriaDto()
                         {
                             Id = a.Id,
                             CertificadoId = a.CertificadoId,
                             OfertaId = a.OfertaId,
                             comentario = a.comentario,
                             descripcion = a.descripcion,
                             fecha_desde = a.fecha_desde,
                             fecha_hasta = a.fecha_hasta,
                             fecha_presentacion = a.fecha_presentacion,
                             monto_ingenieria = a.monto_ingenieria,
                             codigo_oferta = a.Oferta.codigo,
                             codigo_proyecto = a.Oferta.Proyecto.codigo,
                             aprobado = a.aprobado,
                             estado = a.estado,
                             alcance = a.alcance,
                             vigente = a.vigente
                         }).ToList();

            return items;
        }

        public bool Eliminar(int avanceIngenieriaId)
        {
            var count = _detalleAvanceIngenieriaRepository.GetAll()
                .Where(o => o.vigente == true).Count(o => o.AvanceIngenieriaId == avanceIngenieriaId);

            if (count > 0)
            {
                return false;
            }

            var avance = Repository.Get(avanceIngenieriaId);
            avance.vigente = false;

            Repository.Update(avance);
            return true;

        }

        public bool CargarAvanceIngenieria(int OfertaId)
        {
            throw new NotImplementedException();
        }

        public List<AvanceIngenieriaExcel> FiltrarAvancesExcel(List<AvanceIngenieriaExcel> Lista, string codigoproyecto)
        {
            List<AvanceIngenieriaExcel> filtrado = new List<AvanceIngenieriaExcel>();
            foreach (var aitem in Lista)
            {
                if (aitem.codigoProyecto == codigoproyecto)
                {
                    filtrado.Add(aitem);
                }
            }

            return filtrado;
        }

        public List<AvanceIngenieriaExcel> FiltrarAvancesExcelFechas(List<AvanceIngenieriaExcel> Lista,
            DateTime fechadesde, DateTime fechahasta)
        {
            List<AvanceIngenieriaExcel> filtradofechas = new List<AvanceIngenieriaExcel>();
            foreach (var aitem in Lista)
            {

                if (DateTime.Parse(aitem.fecha) >= fechadesde && DateTime.Parse(aitem.fecha) <= fechahasta)
                {
                    filtradofechas.Add(aitem);
                }
            }

            return filtradofechas;
        }

        public bool CrearDetalle(List<Oferta> ofertasdefinitivas, List<AvanceIngenieriaExcel> Lista,
            DateTime fechapresentacion, DateTime fechadesde, DateTime fechahasta)
        {
            foreach (var odefinitivas in ofertasdefinitivas)
            {
                AvanceIngenieriaDto n = new AvanceIngenieriaDto
                {
                    Id = 0,
                    OfertaId = odefinitivas.Id,
                    descripcion = "Avance Ingenieria " + fechadesde.ToString("dd/MM/yyyy") + " " +
                                  fechahasta.ToString("dd/MM/yyyy"),
                    CertificadoId = 0,
                    alcance = "pendiente",
                    fecha_desde = fechadesde,
                    fecha_presentacion = fechapresentacion,
                    fecha_hasta = fechahasta,
                    vigente = true,
                    comentario = "pendiente",
                    aprobado = false,
                    monto_ingenieria = 0,

                };

                var avancecabecera = Repository.InsertAndGetId(MapToEntity(n));

                //Sacar Lista de Computos de la Ofertas
                var wbsofertasQuery = _wbsrepository.GetAllIncluding(c => c.Oferta, c => c.Oferta.Requerimiento,
                    c => c.Oferta.Requerimiento.Proyecto, c => c.Oferta.Requerimiento.Proyecto.Contrato,
                    c => c.Oferta.Requerimiento.Proyecto.Contrato.Cliente);
                var item = (from w in wbsofertasQuery
                            where w.OfertaId == odefinitivas.Id
                            where w.nivel_nombre == "Ingenieria"
                            where w.es_actividad == true

                            select new WbsDto()
                            {

                                Id = w.Id,
                                OfertaId = w.OfertaId,
                                fecha_inicial = w.fecha_inicial,
                                fecha_final = w.fecha_final,
                                id_nivel_padre_codigo = w.id_nivel_padre_codigo,
                                id_nivel_codigo = w.id_nivel_codigo,
                                nivel_nombre = w.nivel_nombre,
                                observaciones = w.observaciones

                            }).SingleOrDefault();


                if (item != null)
                {

                    var computosQuery = _computorepository.GetAll();
                    var computos = (from c in computosQuery
                                    where c.WbsId == item.Id && c.vigente == true
                                    select new ComputoDto
                                    {
                                        Id = c.Id,
                                        WbsId = c.WbsId,
                                        cantidad = c.cantidad,
                                        precio_unitario = c.precio_unitario,
                                        costo_total = c.costo_total,
                                        estado = c.estado,
                                        vigente = c.vigente,
                                        Wbs = c.Wbs,
                                        precio_base = c.precio_base, //Nuevos//
                                        precio_ajustado = c.precio_ajustado,
                                        precio_aplicarse = c.precio_aplicarse,
                                        precio_incrementado = c.precio_incrementado,
                                        ItemId = c.ItemId,
                                        Item = c.Item,
                                        item_codigo = c.Item.codigo,
                                        item_nombre = c.Item.nombre,
                                    }).ToList();


                    foreach (var rcomputo in computos)
                    {
                        //comparo el item del Wbs Con computos Lista

                        /*foreach (var listaexcel in Lista)
                        {
                            if (listaexcel.item.Equals(rcomputo.Item.codigo))
                            {

                                //Crear Detalle
                                DetalleAvanceIngenieria davance = new DetalleAvanceIngenieria
                                {
                                    Id = 0,
                                    AvanceIngenieriaId = avancecabecera,
                                    ComputoId = rcomputo.Id,
                                    cantidad_horas = 0,

                                    vigente = true,

                                };
                                var detalleavance = _detalleAvanceIngenieriaRepository.InsertAndGetId(davance);


                                DetalleItemIngenieria di = new DetalleItemIngenieria
                                {
                                    Id = 0,
                                    ColaboradorId = 1,
                                    DetalleAvanceIngenieriaId = detalleavance,
                                    cantidad_horas = Int32.Parse(listaexcel.hh),
                                    especialidad = 1,
                                    etapa = DetalleItemIngenieria.Etapa.IngDetalle,
                                    fecha_registro = DateTime.Now,
                                    tipo_registro = 0,

                                };
                                var detalleiitem = _detalleitemingenieria.InsertAndGetId(di);
                                return true;
                            }

                        }*/

                    }

                }
            }


            return false;


        }

        public List<Oferta> ListaOfertasDefinitivas(List<AvanceIngenieriaExcel> Lista)
        {
            List<Oferta> ofertasdefinitivas = new List<Oferta>();
            List<Proyecto> proyectos = new List<Proyecto>();
            var proyecto = _proyectoRepository.GetAll().Where(e => e.vigente == true).ToList();



            //recorro el Excel
            foreach (var excelitem in Lista)
            {
                //Comparo proyectos que existan con los registrados
                var encontrado = _proyectoRepository.GetAll().Where(c => c.vigente).Where(c => c.codigo == excelitem.codigoProyecto).FirstOrDefault();

                if (encontrado != null)
                {
                    proyectos.Add(encontrado); //añado los proyectos a un Lista nueva solo los q coincidan

                }
            }
            var pdistintos = proyectos.ToList().Distinct();
            if (pdistintos.Count() > 0)
            {
                foreach (var itemp in pdistintos)//comparo proyectos coincidentes
                {
                    var ofertas = _ofertaRepository.GetAll().Where(e => e.vigente == true).Where(e => e.es_final == true)
                        .Where(e => e.ProyectoId == itemp.Id).ToList();
                    foreach (var itemoferta in ofertas) //comparo ofertas definitivas
                    {
                        ofertasdefinitivas.Add(itemoferta);
                    }
                }
            }
            return ofertasdefinitivas;
        }

        public bool Detalles(List<AvanceIngenieriaExcel> Lista, DateTime presentacion, DateTime fechadesde, DateTime fechahasta)
        {

            List<Oferta> ofertasdefinitivas = new List<Oferta>();
            List<Proyecto> proyectos = new List<Proyecto>();
            var proyecto = _proyectoRepository.GetAll().Where(e => e.vigente == true).ToList();



            //recorro el Excel
            foreach (var excelitem in Lista)
            {
                //Comparo proyectos que existan con los registrados
                var encontrado = _proyectoRepository.GetAll().Where(c => c.vigente).Where(c => c.codigo == excelitem.codigoProyecto).FirstOrDefault();

                if (encontrado != null)
                {
                    proyectos.Add(encontrado); //añado los proyectos a un Lista nueva solo los q coincidan

                }
            }
            var pdistintos = proyectos.ToList().Distinct();
            if (pdistintos.Count() > 0)
            {
                foreach (var itemp in pdistintos)//comparo proyectos coincidentes
                {
                    var ofertas = _ofertaRepository.GetAll().Where(e => e.vigente == true).Where(e => e.es_final == true)
                        .Where(e => e.ProyectoId == itemp.Id).ToList();
                    foreach (var itemoferta in ofertas) //comparo ofertas definitivas
                    {
                        ofertasdefinitivas.Add(itemoferta);
                    }
                }
            }

            /// comienz aqui
            foreach (var odefinitivas in ofertasdefinitivas)
            {
                AvanceIngenieriaDto n = new AvanceIngenieriaDto
                {
                    Id = 0,
                    OfertaId = odefinitivas.Id,
                    descripcion = "Avance Ingenieria " + fechadesde.ToString("dd/MM/yyyy") + " " +
                                  fechahasta.ToString("dd/MM/yyyy"),
                    CertificadoId = 0,
                    alcance = "pendiente",
                    fecha_desde = fechadesde,
                    fecha_presentacion = presentacion,
                    fecha_hasta = fechahasta,
                    vigente = true,
                    comentario = "pendiente",
                    aprobado = false,
                    monto_ingenieria = 0,

                };

                var avancecabecera = Repository.InsertAndGetId(MapToEntity(n));

                //Sacar Lista de Computos de la Ofertas

                var computos = _computorepository.GetAll()
                                .Where(c => c.Wbs.OfertaId == odefinitivas.Id)
                                .Where(c => c.Wbs.es_actividad)
                                .Where(c => c.Wbs.nivel_nombre == "Ingenieria")
                                .Where(c => c.Item.GrupoId == 1)
                                .Where(c => c.vigente).ToList();

                foreach (var listaexcel in Lista)
                {
                    var computo = (from ce in computos
                                   where ce.Wbs.Oferta.Proyecto.codigo == listaexcel.codigoProyecto
                                   // where ce.Item.codigo == listaexcel.item
                                   select ce).FirstOrDefault();
                    if (computo != null)
                    {

                        DetalleAvanceIngenieria davance = new DetalleAvanceIngenieria
                        {
                            Id = 0,
                            AvanceIngenieriaId = avancecabecera,
                            ComputoId = computo.Id,
                            cantidad_horas = 0,
                            vigente = true,
                            fecha_real = DateTime.Parse(listaexcel.fecha),
                            precio_unitario = computo.precio_unitario,
                            valor_real = 0,
                            ingreso_acumulado = 0,
                            calculo_anterior = 0,
                            calculo_diario = 0,
                            cantidad_acumulada = 0,
                            cantidad_acumulada_anterior = 0

                        };
                        var detalleavance = _detalleAvanceIngenieriaRepository.InsertAndGetId(davance);

                        //  var tiporegistro = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.codigo == listaexcel.tipo_registro).FirstOrDefault();
                        // var especialidad = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.codigo == listaexcel.especialidad).FirstOrDefault();

                        // var colaborador = _colaboradorepository.GetAll().Where(c => c.vigente).Where(c => c.tcu == listaexcel.cedula).FirstOrDefault();

                        DetalleItemIngenieria di = new DetalleItemIngenieria
                        {
                            Id = 0,
                            // ColaboradorId = colaborador != null ? colaborador.Id : 345,
                            DetalleAvanceIngenieriaId = detalleavance,
                            // cantidad_horas = Decimal.Parse(listaexcel.hh),
                            // especialidad = especialidad != null ? especialidad.Id : 0,
                            etapa = DetalleItemIngenieria.Etapa.ID,
                            fecha_registro = DateTime.Now,
                            // tipo_registro = tiporegistro != null ? tiporegistro.Id : 0,

                        };
                        var detalleiitem = _detalleitemingenieria.InsertAndGetId(di);

                    }
                }


            }


            return true;


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
        public List<AvanceIngenieriaDto> ListarAvancesDeOfertaSinCertificar(int OfertaId)
        {
            var AvanceObraQuery = Repository.GetAll();
            var items = (from a in AvanceObraQuery
                         where a.vigente == true
                         where a.OfertaId == OfertaId
                         where a.CertificadoId == 0
                         select new AvanceIngenieriaDto()
                         {
                             Id = a.Id,
                             CertificadoId = a.CertificadoId,
                             OfertaId = a.OfertaId,
                             Oferta = a.Oferta,
                             comentario = a.comentario,
                             descripcion = a.descripcion,
                             fecha_desde = a.fecha_desde,
                             fecha_hasta = a.fecha_hasta,
                             fecha_presentacion = a.fecha_presentacion,
                             monto_ingenieria = a.monto_ingenieria,
                             codigo_oferta = a.Oferta.codigo,
                             codigo_proyecto = a.Oferta.Proyecto.codigo,
                             aprobado = a.aprobado,
                             estado = a.estado,
                             alcance = a.alcance,
                             vigente = a.vigente
                         }).ToList();

            return items;
        }

        public List<DetalleAvanceIngenieriaDto> ListarPorAvanceIngenieria(int avanceIngenieriaId)
        {
            var query = _detalleAvanceIngenieriaRepository.GetAllIncluding(o => o.Computo.Item, o => o.Computo).Where(o => o.vigente == true)
                .Where(o => o.AvanceIngenieriaId == avanceIngenieriaId);

            var items = (from a in query
                         where a.estacertificado == false
                         select new DetalleAvanceIngenieriaDto()
                         {
                             Id = a.Id,
                             AvanceIngenieriaId = a.AvanceIngenieriaId,
                             AvanceIngenieria = a.AvanceIngenieria,
                             ComputoId = a.ComputoId,
                             cantidad_horas = a.cantidad_horas,
                             Computo = a.Computo,
                             codigo_item = a.Computo.Item.codigo,
                             descripcion_item = a.Computo.Item.descripcion,
                             vigente = a.vigente,
                             fecha_real = a.fecha_real,
                             precio_unitario = a.precio_unitario,
                             valor_real = a.valor_real,
                             calculo_anterior = a.calculo_anterior,
                             calculo_diario = a.calculo_diario,
                             ingreso_acumulado = a.ingreso_acumulado,
                             cantidad_acumulada = a.cantidad_acumulada,
                             cantidad_acumulada_anterior = a.cantidad_acumulada_anterior,


                         }).ToList();

            foreach (var d in items)
            {
                d.fechar = d.fecha_real.ToShortDateString();
                d.horas_presupuestadas = d.Computo.cantidad;
            }
            return items;
        }

        public List<DetalleAvanceIngenieriaDto> ListarDetallesAvanceIngenieriaProyecto(int ProyectoId)
        {
            List<DetalleAvanceIngenieriaDto> detallesavanceobra = new List<DetalleAvanceIngenieriaDto>();


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

                            var detalles = this.ListarPorAvanceIngenieria(ovance.Id);

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

        public ExcelPackage ObtenerCertificadoIngenieria(int Id)
        {
            /*Datos Cabecera*/

            var oferta = _ofertaRepository.GetAllIncluding(c => c.Requerimiento, c => c.Proyecto).Where(c => c.Id == Id).FirstOrDefault();
            var computos = _computorepository.GetAllIncluding(c => c.Item.Grupo, c => c.Wbs.Oferta).Where(c => c.vigente)
                                                                      .Where(c => c.Item.Grupo.codigo == ProyectoCodigos.CODE_INGENIERIA)
                                                                      .Where(c => c.Wbs.OfertaId == Id)
                                                                      .Where(c => c.Wbs.Oferta.es_final)
                                                                      .ToList();


            var datos = this.Datos(oferta.Id);


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
            h.Cells[cell].Value = oferta.Proyecto.codigo + " " + oferta.Proyecto.nombre_proyecto;
            cell = "C7";
            h.Cells[cell].Value = "";
            cell = "C8";
            h.Cells[cell].Value = "";
            cell = "C9";
            h.Cells[cell].Value = oferta.Proyecto.codigo + "-001";
            cell = "C10";
            h.Cells[cell].Value = "" + DateTime.Now.Date;

            int first_row = 13;
            int count = first_row;


            var data_directos = (from d in datos where d.TipoColaborador == TipoColaborador.Directo select d).ToList();
            var data_indirectos = (from d in datos where d.TipoColaborador == TipoColaborador.Indirecto select d).ToList();
            decimal suma_horas_directos = 0;
            decimal suma_costo_total_directos = 0;
            decimal suma_horas_indirectos = 0;
            decimal suma_costo_total_indirectos = 0;


            decimal monto_total_ingenieria = 0;

            decimal total_actual = 0;
            decimal total_costo_total = 0;

            decimal total_anterior = 0;
            decimal total_costo_anterior = 0;

            decimal total_acumulado = 0;
            decimal total_costo_acumulado = 0;


            if (data_directos.Count > 0)
            {
                suma_horas_directos = (from s in data_directos select s.TotalHoras).Sum();
                suma_costo_total_directos = (from s in data_directos select s.CostoTotal).Sum();
            }
            if (data_indirectos.Count > 0)
            {
                suma_horas_indirectos = (from s in data_indirectos select s.TotalHoras).Sum();
                suma_costo_total_indirectos = (from s in data_indirectos select s.CostoTotal).Sum();
            }
            if (datos.Count > 0)
            {
                total_actual = (from s in datos select s.TotalHoras).Sum();
                total_costo_total = (from s in datos select s.CostoTotal).Sum();
            }


            int datacount = data_directos.Count;
            h.InsertRow(count + 1, datacount - 1);
            foreach (var i in data_directos)
            {
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
                h.Cells[cell].Value = i.TotalHoras;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "F" + count;
                h.Cells[cell].Value = i.Tarifa;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "G" + count;
                h.Cells[cell].Value = i.CostoTotal;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                count++;
            }


            count = first_row + datacount + 3;
            h.InsertRow(count + 1, data_indirectos.Count - 1);
            foreach (var i in data_indirectos)
            {
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
                h.Cells[cell].Value = i.TotalHoras;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "F" + count;
                h.Cells[cell].Value = i.Tarifa;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                cell = "G" + count;
                h.Cells[cell].Value = i.CostoTotal;
                h.Cells[cell].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                count++;
            }

            /*TOTALES DIRECTOS*/
            var d_sub_horas = (from c in h.Cells where c.Value?.ToString().Contains("SUBD") == true select c).FirstOrDefault();
            h.Cells[d_sub_horas.Address].Value = suma_horas_directos;
            var ib = (from c in h.Cells where c.Value?.ToString().Contains("TIB") == true select c).FirstOrDefault();
            h.Cells[ib.Address].Value = suma_horas_directos;

            var d_sub_costo_t = (from c in h.Cells where c.Value?.ToString().Contains("SUBDT") == true select c).FirstOrDefault();
            h.Cells[d_sub_costo_t.Address].Value = suma_costo_total_directos;
            /*TOTALES INDIRECTOS*/

            var d_subi_horas = (from c in h.Cells where c.Value?.ToString().Contains("IND") == true select c).FirstOrDefault();
            h.Cells[d_subi_horas.Address].Value = suma_horas_indirectos;
            var d_subi_costo_t = (from c in h.Cells where c.Value?.ToString().Contains("SUBIT") == true select c).FirstOrDefault();
            h.Cells[d_subi_costo_t.Address].Value = suma_costo_total_indirectos;

            /* TOTAL ACTUAL Y USD*/
            var t_horas = (from c in h.Cells where c.Value?.ToString().Contains("TE") == true select c).FirstOrDefault();
            h.Cells[t_horas.Address].Value = total_actual;
            var t_costo_total = (from c in h.Cells where c.Value?.ToString().Contains("TUA") == true select c).FirstOrDefault();
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


            return excel;
        }

        public List<IngenieriaDatos> Datos(int id) //OfertaId
        {
            List<IngenieriaDatos> datos = new List<IngenieriaDatos>();

            var query = _detalleitemingenieria.GetAllIncluding(c => c.DetalleAvanceIngenieria.AvanceIngenieria,
                                                               c => c.Colaborador.Cargo,
                                                               c => c.DetalleAvanceIngenieria.Computo.Item)
                                            .Where(c => c.DetalleAvanceIngenieria.AvanceIngenieria.OfertaId == id)
                                            .Where(c => c.vigente)
                                            .Where(c => c.DetalleAvanceIngenieria.vigente)
                                            .Where(c => c.DetalleAvanceIngenieria.AvanceIngenieria.vigente)
                                            .ToList();
            var list = (from q in query
                        select new IngenieriaDatos()
                        {
                            Id = q.Id,
                            Rubro = q.DetalleAvanceIngenieria.Computo.Item.codigo,
                            Nombre = q.Colaborador.apellidos + " " + q.Colaborador.nombres,
                            Categoria = q.DetalleAvanceIngenieria.Computo.Item.nombre,
                            Unidad = "HH",
                            TotalHoras = q.cantidad_horas,
                            Tarifa = q.Colaborador.Cargo.precio_unitario,
                            CostoTotal = (q.cantidad_horas * q.Colaborador.Cargo.precio_unitario),
                            TipoColaborador = q.Colaborador.tipo
                        }).ToList();
            datos.AddRange(list);

            /**/

            return datos;

        }

        public string UploadAvanceMasivo(AvanceUpload e)
        {
            var result = "";
            List<String> errors = new List<string>();
            if (e.UploadedFile != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (e.UploadedFile.ContentType == "application/vnd.ms-excel" || e.UploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string fileName = e.UploadedFile.FileName;
                    string fileContentType = e.UploadedFile.ContentType;
                    byte[] fileBytes = new byte[e.UploadedFile.ContentLength];
                    var data = e.UploadedFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(e.UploadedFile.ContentLength));

                    using (var package = new ExcelPackage(e.UploadedFile.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        List<AvanceIngenieriaExcel> datosExcel = new List<AvanceIngenieriaExcel>();
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {

                            AvanceIngenieriaExcel a = new AvanceIngenieriaExcel
                            {
                                fila = rowIterator,
                                descripcionActividad = (workSheet.Cells[rowIterator, 1].Value ?? "").ToString(),
                                codigoProyecto = (workSheet.Cells[rowIterator, 2].Value ?? "").ToString(),
                                hH = (workSheet.Cells[rowIterator, 3].Value ?? "").ToString(),
                                fecha = (workSheet.Cells[rowIterator, 4].Text ?? "").ToString(),
                                mes = (workSheet.Cells[rowIterator, 5].Value ?? "").ToString(),
                                numeroIdentificacion = (workSheet.Cells[rowIterator, 6].Value ?? "").ToString(),
                                apellidosNombres = (workSheet.Cells[rowIterator, 7].Value ?? "").ToString(),
                                directoI = (workSheet.Cells[rowIterator, 8].Value ?? "").ToString(),
                                etapa = (workSheet.Cells[rowIterator, 9].Value ?? "").ToString(),
                            };
                            datosExcel.Add(a);
                        }


                        var filter_list = (from f in datosExcel
                                           where f.fecha != null
                                           where f.fecha != ""
                                           where DateTime.Parse(f.fecha) >= e.fecha_desde
                                           where DateTime.Parse(f.fecha) <= e.fecha_hasta
                                           select f).ToList();



                        List<Proyecto> proyectos = new List<Proyecto>();
                        foreach (var l in filter_list)
                        {
                            var proyecto = _proyectoRepository.GetAll().Where(c => c.vigente).Where(c => c.codigo == l.codigoProyecto).FirstOrDefault();
                            if (proyecto == null)
                            {
                                string error = "Fila: " + l.fila + " el proyecto no existe";
                                // errors.Add(error);
                            }
                            else
                            {
                                var oferta = _ofertaRepository.GetAll().Where(o => o.vigente)
                                                                        .Where(o => o.es_final)
                                                                        .Where(o => o.ProyectoId == proyecto.Id).FirstOrDefault();

                                if (oferta != null && oferta.Id > 0)
                                {


                                    DateTime fecha = DateTime.Parse(l.fecha);
                                    var existe = Repository.GetAll().Where(c => fecha >= c.fecha_desde)
                                                                        .Where(c => fecha <= c.fecha_hasta)
                                                                        .Where(c => c.OfertaId == oferta.Id)
                                                                        .FirstOrDefault();


                                    if (existe != null && existe.Id > 0)
                                    {
                                        existe.fecha_presentacion = e.fecha_presentacion;
                                        Repository.Update(existe);


                                        var avanceIngenieriaId = existe.Id;

                                        var computos = _computorepository.GetAll()
                                         .Where(c => c.Wbs.OfertaId == oferta.Id)
                                         .Where(c => c.Wbs.es_actividad)
                                         .Where(c => c.Wbs.nivel_nombre == "Ingenieria")
                                         .Where(c => c.Item.GrupoId == 1)
                                         .Where(c => c.vigente).ToList();



                                        var col = _colirepository.GetAllIncluding(c => c.Cargo.Item)
                                                                 .Where(c => c.vigente)
                                                                 .Where(c => c.numero_identificacion == l.numeroIdentificacion)
                                                                 .FirstOrDefault();
                                        if (col != null)
                                        {

                                            var computo = (from ce in computos
                                                           where ce.Wbs.Oferta.Proyecto.codigo == l.codigoProyecto
                                                           where ce.ItemId == col.Cargo.ItemId
                                                           select ce).FirstOrDefault();

                                            if (computo != null)
                                            {

                                                var detalleexiste = _detalleAvanceIngenieriaRepository.GetAll().Where(c => c.AvanceIngenieriaId == existe.Id)
                                                                                                              .Where(c => c.ComputoId == computo.Id)
                                                                                                              .Where(c => c.vigente)
                                                                                                              .Where(c => !c.estacertificado)
                                                                                                              .FirstOrDefault();
                                                if (detalleexiste != null)
                                                {

                                                    var detalleAvanceId = detalleexiste.Id;
                                                    var tiporegistro = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == l.descripcionActividad).FirstOrDefault();
                                                    var especialidad = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == l.etapa).FirstOrDefault();

                                                    var etapa = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == l.etapa).FirstOrDefault();
                                                    DateTime fechae = DateTime.Parse(l.fecha);
                                                    var itemexiste = _detalleitemingenieria.GetAll().Where(c => c.DetalleAvanceIngenieriaId == detalleexiste.Id)
                                                                                                   .Where(c => c.ColaboradorId == col.Id)
                                                                                                   .Where(c => c.fecha_registro == fecha)
                                                                                                   .Where(c => c.vigente)
                                                                                                   .FirstOrDefault();

                                                    string datoetapa = "";
                                                    if (etapa != null && etapa.Id > 0)
                                                    {
                                                        if (etapa.nombre == "ID")
                                                        {
                                                            datoetapa = "ID";
                                                        }
                                                        if (etapa.nombre == "IB")
                                                        {
                                                            datoetapa = "IB";
                                                        }
                                                        if (etapa.nombre == "AB")
                                                        {
                                                            datoetapa = "AB";
                                                        }
                                                        if (etapa.nombre == "N/A")
                                                        {
                                                            datoetapa = "N/A";
                                                        }

                                                    }

                                                    if (itemexiste != null)
                                                    {

                                                        itemexiste.cantidad_horas = Decimal.Parse(l.hH);
                                                        _detalleitemingenieria.Update(itemexiste);
                                                    }
                                                    else
                                                    {

                                                        DetalleItemIngenieria di = new DetalleItemIngenieria
                                                        {
                                                            Id = 0,
                                                            ColaboradorId = col.Id,
                                                            DetalleAvanceIngenieriaId = detalleAvanceId,
                                                            cantidad_horas = Decimal.Parse(l.hH),
                                                            especialidad = especialidad != null ? especialidad.Id : 0,
                                                            etapa = datoetapa == "ID" ? DetalleItemIngenieria.Etapa.ID : datoetapa == "IB" ? DetalleItemIngenieria.Etapa.IB : datoetapa == "AB" ? DetalleItemIngenieria.Etapa.AB : DetalleItemIngenieria.Etapa.NA,
                                                            fecha_registro = DateTime.Parse(l.fecha),
                                                            tipo_registro = tiporegistro != null ? tiporegistro.Id : 0,

                                                        };
                                                        var detalleiitem = _detalleitemingenieria.InsertAndGetId(di);
                                                    }
                                                }
                                                else
                                                {
                                                    DetalleAvanceIngenieria davance = new DetalleAvanceIngenieria
                                                    {
                                                        Id = 0,
                                                        AvanceIngenieriaId = avanceIngenieriaId,
                                                        ComputoId = computo.Id,
                                                        cantidad_horas = 0,
                                                        vigente = true,
                                                        fecha_real = DateTime.Parse(l.fecha),
                                                        precio_unitario = computo.precio_unitario,
                                                        valor_real = 0,
                                                        ingreso_acumulado = 0,
                                                        calculo_anterior = 0,
                                                        calculo_diario = 0,
                                                        cantidad_acumulada = 0,
                                                        cantidad_acumulada_anterior = 0

                                                    };
                                                    var detalleAvanceId = _detalleAvanceIngenieriaRepository.InsertAndGetId(davance);
                                                    var tiporegistro = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == l.descripcionActividad).FirstOrDefault();
                                                    var especialidad = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == l.etapa).FirstOrDefault();

                                                    var etapa = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == l.etapa).FirstOrDefault();
                                                    string datoetapa = "";
                                                    if (etapa != null && etapa.Id > 0)
                                                    {
                                                        if (etapa.nombre == "ID")
                                                        {
                                                            datoetapa = "ID";
                                                        }
                                                        if (etapa.nombre == "IB")
                                                        {
                                                            datoetapa = "IB";
                                                        }
                                                        if (etapa.nombre == "AB")
                                                        {
                                                            datoetapa = "AB";
                                                        }
                                                        if (etapa.nombre == "N/A")
                                                        {
                                                            datoetapa = "N/A";
                                                        }


                                                    }
                                                    DetalleItemIngenieria di = new DetalleItemIngenieria
                                                    {
                                                        Id = 0,
                                                        ColaboradorId = col.Id,
                                                        DetalleAvanceIngenieriaId = detalleAvanceId,
                                                        cantidad_horas = Decimal.Parse(l.hH),
                                                        especialidad = especialidad != null ? especialidad.Id : 0,
                                                        etapa = datoetapa == "ID" ? DetalleItemIngenieria.Etapa.ID : datoetapa == "IB" ? DetalleItemIngenieria.Etapa.IB : datoetapa == "AB" ? DetalleItemIngenieria.Etapa.AB : DetalleItemIngenieria.Etapa.NA,
                                                        fecha_registro = DateTime.Parse(l.fecha),
                                                        tipo_registro = tiporegistro != null ? tiporegistro.Id : 0,

                                                    };
                                                    var detalleiitem = _detalleitemingenieria.InsertAndGetId(di);
                                                }
                                            }


                                        }



                                    }
                                    else
                                    {

                                        AvanceIngenieriaDto n = new AvanceIngenieriaDto
                                        {
                                            Id = 0,
                                            OfertaId = oferta.Id,
                                            descripcion = "Avance Ingeniería " + e.fecha_desde.ToString("dd/MM/yyyy") + " " +
                                                          e.fecha_hasta.ToString("dd/MM/yyyy"),
                                            CertificadoId = 0,
                                            alcance = ".",
                                            fecha_desde = e.fecha_desde,
                                            fecha_presentacion = e.fecha_presentacion,
                                            fecha_hasta = e.fecha_hasta,
                                            vigente = true,
                                            comentario = ".",
                                            aprobado = false,
                                            monto_ingenieria = 0,

                                        };

                                        var avanceIngenieriaId = Repository.InsertAndGetId(MapToEntity(n));

                                        var computos = _computorepository.GetAll()
                                         .Where(c => c.Wbs.OfertaId == oferta.Id)
                                         .Where(c => c.Wbs.es_actividad)
                                         .Where(c => c.Wbs.nivel_nombre == "Ingenieria")
                                         .Where(c => c.Item.GrupoId == 1)
                                         .Where(c => c.vigente).ToList();


                                        var col = _colirepository.GetAllIncluding(c => c.Cargo.Item)
                                                                 .Where(c => c.vigente)
                                                                 .Where(c => c.numero_identificacion == l.numeroIdentificacion)
                                                                 .FirstOrDefault();
                                        if (col != null)
                                        {

                                            var computo = (from ce in computos
                                                           where ce.Wbs.Oferta.Proyecto.codigo == l.codigoProyecto
                                                           where ce.ItemId == col.Cargo.ItemId
                                                           select ce).FirstOrDefault();

                                            if (computo != null)
                                            {

                                                DetalleAvanceIngenieria davance = new DetalleAvanceIngenieria
                                                {
                                                    Id = 0,
                                                    AvanceIngenieriaId = avanceIngenieriaId,
                                                    ComputoId = computo.Id,
                                                    cantidad_horas = 0,
                                                    vigente = true,
                                                    fecha_real = DateTime.Parse(l.fecha),
                                                    precio_unitario = computo.precio_unitario,
                                                    valor_real = 0,
                                                    ingreso_acumulado = 0,
                                                    calculo_anterior = 0,
                                                    calculo_diario = 0,
                                                    cantidad_acumulada = 0,
                                                    cantidad_acumulada_anterior = 0

                                                };

                                                var detalleAvanceId = _detalleAvanceIngenieriaRepository.InsertAndGetId(davance);
                                                var tiporegistro = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == l.descripcionActividad).FirstOrDefault();
                                                var especialidad = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == l.etapa).FirstOrDefault();
                                                var etapa = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == l.etapa).FirstOrDefault();

                                                string datoetapa = "";
                                                if (etapa != null && etapa.Id > 0)
                                                {
                                                    if (etapa.nombre == "ID")
                                                    {
                                                        datoetapa = "ID";
                                                    }
                                                    if (etapa.nombre == "IB")
                                                    {
                                                        datoetapa = "IB";
                                                    }
                                                    if (etapa.nombre == "AB")
                                                    {
                                                        datoetapa = "AB";
                                                    }
                                                    if (etapa.nombre == "N/A")
                                                    {
                                                        datoetapa = "N/A";
                                                    }

                                                }

                                                DetalleItemIngenieria di = new DetalleItemIngenieria
                                                {
                                                    Id = 0,
                                                    ColaboradorId = col.Id,
                                                    DetalleAvanceIngenieriaId = detalleAvanceId,
                                                    cantidad_horas = Decimal.Parse(l.hH),
                                                    especialidad = especialidad != null ? especialidad.Id : 0,
                                                    etapa = datoetapa == "ID" ? DetalleItemIngenieria.Etapa.ID : datoetapa == "IB" ? DetalleItemIngenieria.Etapa.IB : datoetapa == "AB" ? DetalleItemIngenieria.Etapa.AB : DetalleItemIngenieria.Etapa.NA,
                                                    fecha_registro = DateTime.Parse(l.fecha),
                                                    tipo_registro = tiporegistro != null ? tiporegistro.Id : 0,

                                                };
                                                var detalleiitem = _detalleitemingenieria.InsertAndGetId(di);

                                            }

                                        }


                                    }





                                }
                            }

                        }/*
                        if (errors.Count > 0)
                        {
                            result = JsonConvert.SerializeObject(errors);
                            return result;
                        }
                        else
                        {/*
                            List<Oferta> base_rdo_definitivas = new List<Oferta>();

                            var diferent_projects = (from p in proyectos select p.Id).ToList().Distinct().ToList();
                            foreach (var Id in diferent_projects) //Ids Proyectos
                            {
                                var proyecto = _proyectoRepository.GetAll().Where(c => c.vigente).Where(c => c.Id == Id).FirstOrDefault();
                                if (proyecto != null)
                                {
                                    var ofertas = _ofertaRepository.GetAll().Where(o => o.vigente)
                                                                            .Where(o => o.es_final)
                                                                            .Where(o => o.ProyectoId == proyecto.Id).ToList();
                                    base_rdo_definitivas.AddRange(ofertas);
                                }

                            }
                            //Bases Rdo Definitivas
                            foreach (var d in base_rdo_definitivas)
                            {
                                var existe = Repository.GetAll().Where(c => c.fecha_desde >= e.fecha_desde)
                                    .Where(c => c.fecha_desde <= e.fecha_hasta)
                                    .Where(c => c.OfertaId == d.Id)
                                   // .Where(c => c.fecha_presentacion == e.fecha_presentacion)
                                   .FirstOrDefault();
                                if (existe != null && existe.Id > 0)
                                {
                                    existe.fecha_presentacion = e.fecha_presentacion;
                                    Repository.Update(existe);


                                    var avanceIngenieriaId = existe.Id;

                                    var computos = _computorepository.GetAll()
                                     .Where(c => c.Wbs.OfertaId == d.Id)
                                     .Where(c => c.Wbs.es_actividad)
                                     .Where(c => c.Wbs.nivel_nombre == "Ingenieria")
                                     .Where(c => c.Item.GrupoId == 1)
                                     .Where(c => c.vigente).ToList();


                                    foreach (var le in filter_list)
                                    {
                                        var col = _colirepository.GetAllIncluding(c => c.Cargo.Item)
                                                                 .Where(c => c.vigente)
                                                                 .Where(c => c.numero_identificacion == le.numeroIdentificacion)
                                                                 .FirstOrDefault();
                                        if (col != null)
                                        {

                                            var computo = (from ce in computos
                                                           where ce.Wbs.Oferta.Proyecto.codigo == le.codigoProyecto
                                                           where ce.ItemId == col.Cargo.ItemId
                                                           select ce).FirstOrDefault();

                                            if (computo != null)
                                            {

                                                var detalleexiste = _detalleAvanceIngenieriaRepository.GetAll().Where(c => c.AvanceIngenieriaId == existe.Id)
                                                                                                              .Where(c => c.ComputoId == computo.Id)
                                                                                                              .Where(c => c.vigente)
                                                                                                              .FirstOrDefault();
                                                if (detalleexiste != null)
                                                {

                                                    var detalleAvanceId = detalleexiste.Id;
                                                    var tiporegistro = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == le.descripcionActividad).FirstOrDefault();
                                                    var especialidad = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == le.etapa).FirstOrDefault();

                                                    var etapa = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == le.etapa).FirstOrDefault();
                                                    DateTime fecha = DateTime.Parse(le.fecha);
                                                    var itemexiste = _detalleitemingenieria.GetAll().Where(c => c.DetalleAvanceIngenieriaId == detalleexiste.Id)
                                                                                                   .Where(c => c.ColaboradorId == col.Id)
                                                                                                   .Where(c => c.fecha_registro == fecha)
                                                                                                   .Where(c => c.vigente)
                                                                                                   .FirstOrDefault();

                                                    string datoetapa = "";
                                                    if (etapa != null && etapa.Id > 0)
                                                    {
                                                        if (etapa.nombre == "ID")
                                                        {
                                                            datoetapa = "ID";
                                                        }
                                                        if (etapa.nombre == "IB")
                                                        {
                                                            datoetapa = "IB";
                                                        }
                                                        if (etapa.nombre == "AB")
                                                        {
                                                            datoetapa = "AB";
                                                        }
                                                        if (etapa.nombre == "N/A")
                                                        {
                                                            datoetapa = "N/A";
                                                        }

                                                    }

                                                    if (itemexiste != null)
                                                    {

                                                        itemexiste.cantidad_horas = Decimal.Parse(le.hH);
                                                        _detalleitemingenieria.Update(itemexiste);
                                                    }
                                                    else
                                                    {

                                                        DetalleItemIngenieria di = new DetalleItemIngenieria
                                                        {
                                                            Id = 0,
                                                            ColaboradorId = col.Id,
                                                            DetalleAvanceIngenieriaId = detalleAvanceId,
                                                            cantidad_horas = Decimal.Parse(le.hH),
                                                            especialidad = especialidad != null ? especialidad.Id : 0,
                                                            etapa = datoetapa == "ID" ? DetalleItemIngenieria.Etapa.ID : datoetapa == "IB" ? DetalleItemIngenieria.Etapa.IB : datoetapa == "AB" ? DetalleItemIngenieria.Etapa.AB : DetalleItemIngenieria.Etapa.NA,
                                                            fecha_registro = DateTime.Parse(le.fecha),
                                                            tipo_registro = tiporegistro != null ? tiporegistro.Id : 0,

                                                        };
                                                        var detalleiitem = _detalleitemingenieria.InsertAndGetId(di);
                                                    }
                                                }
                                                else
                                                {
                                                    DetalleAvanceIngenieria davance = new DetalleAvanceIngenieria
                                                    {
                                                        Id = 0,
                                                        AvanceIngenieriaId = avanceIngenieriaId,
                                                        ComputoId = computo.Id,
                                                        cantidad_horas = 0,
                                                        vigente = true,
                                                        fecha_real = DateTime.Parse(le.fecha),
                                                        precio_unitario = computo.precio_unitario,
                                                        valor_real = 0,
                                                        ingreso_acumulado = 0,
                                                        calculo_anterior = 0,
                                                        calculo_diario = 0,
                                                        cantidad_acumulada = 0,
                                                        cantidad_acumulada_anterior = 0

                                                    };
                                                    var detalleAvanceId = _detalleAvanceIngenieriaRepository.InsertAndGetId(davance);
                                                    var tiporegistro = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == le.descripcionActividad).FirstOrDefault();
                                                    var especialidad = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == le.etapa).FirstOrDefault();

                                                    var etapa = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == le.etapa).FirstOrDefault();
                                                    string datoetapa = "";
                                                    if (etapa != null && etapa.Id > 0)
                                                    {
                                                        if (etapa.nombre == "ID")
                                                        {
                                                            datoetapa = "ID";
                                                        }
                                                        if (etapa.nombre == "IB")
                                                        {
                                                            datoetapa = "IB";
                                                        }
                                                        if (etapa.nombre == "AB")
                                                        {
                                                            datoetapa = "AB";
                                                        }
                                                        if (etapa.nombre == "N/A")
                                                        {
                                                            datoetapa = "N/A";
                                                        }


                                                    }
                                                    DetalleItemIngenieria di = new DetalleItemIngenieria
                                                    {
                                                        Id = 0,
                                                        ColaboradorId = col.Id,
                                                        DetalleAvanceIngenieriaId = detalleAvanceId,
                                                        cantidad_horas = Decimal.Parse(le.hH),
                                                        especialidad = especialidad != null ? especialidad.Id : 0,
                                                        etapa = datoetapa == "ID" ? DetalleItemIngenieria.Etapa.ID : datoetapa == "IB" ? DetalleItemIngenieria.Etapa.IB : datoetapa == "AB" ? DetalleItemIngenieria.Etapa.AB : DetalleItemIngenieria.Etapa.NA,
                                                        fecha_registro = DateTime.Parse(le.fecha),
                                                        tipo_registro = tiporegistro != null ? tiporegistro.Id : 0,

                                                    };
                                                    var detalleiitem = _detalleitemingenieria.InsertAndGetId(di);
                                                }
                                            }


                                        }


                                    }
                                }
                                else
                                {

                                    AvanceIngenieriaDto n = new AvanceIngenieriaDto
                                    {
                                        Id = 0,
                                        OfertaId = d.Id,
                                        descripcion = "Avance Ingeniería " + e.fecha_desde.ToString("dd/MM/yyyy") + " " +
                                                      e.fecha_hasta.ToString("dd/MM/yyyy"),
                                        CertificadoId = 0,
                                        alcance = ".",
                                        fecha_desde = e.fecha_desde,
                                        fecha_presentacion = e.fecha_presentacion,
                                        fecha_hasta = e.fecha_hasta,
                                        vigente = true,
                                        comentario = ".",
                                        aprobado = false,
                                        monto_ingenieria = 0,

                                    };

                                    var avanceIngenieriaId = Repository.InsertAndGetId(MapToEntity(n));

                                    var computos = _computorepository.GetAll()
                                     .Where(c => c.Wbs.OfertaId == d.Id)
                                     .Where(c => c.Wbs.es_actividad)
                                     .Where(c => c.Wbs.nivel_nombre == "Ingenieria")
                                     .Where(c => c.Item.GrupoId == 1)
                                     .Where(c => c.vigente).ToList();


                                    foreach (var le in filter_list)
                                    {
                                        var col = _colirepository.GetAllIncluding(c => c.Cargo.Item)
                                                                 .Where(c => c.vigente)
                                                                 .Where(c => c.numero_identificacion == le.numeroIdentificacion)
                                                                 .FirstOrDefault();
                                        if (col != null)
                                        {

                                            var computo = (from ce in computos
                                                           where ce.Wbs.Oferta.Proyecto.codigo == le.codigoProyecto
                                                           where ce.ItemId == col.Cargo.ItemId
                                                           select ce).FirstOrDefault();

                                            if (computo != null)
                                            {

                                                DetalleAvanceIngenieria davance = new DetalleAvanceIngenieria
                                                {
                                                    Id = 0,
                                                    AvanceIngenieriaId = avanceIngenieriaId,
                                                    ComputoId = computo.Id,
                                                    cantidad_horas = 0,
                                                    vigente = true,
                                                    fecha_real = DateTime.Parse(le.fecha),
                                                    precio_unitario = computo.precio_unitario,
                                                    valor_real = 0,
                                                    ingreso_acumulado = 0,
                                                    calculo_anterior = 0,
                                                    calculo_diario = 0,
                                                    cantidad_acumulada = 0,
                                                    cantidad_acumulada_anterior = 0

                                                };
                                                var detalleAvanceId = _detalleAvanceIngenieriaRepository.InsertAndGetId(davance);
                                                var tiporegistro = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == le.descripcionActividad).FirstOrDefault();
                                                var especialidad = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == le.etapa).FirstOrDefault();
                                                var etapa = _catalogoRepository.GetAll().Where(c => c.vigente).Where(c => c.nombre == le.etapa).FirstOrDefault();

                                                string datoetapa = "";
                                                if (etapa != null && etapa.Id > 0)
                                                {
                                                    if (etapa.nombre == "ID")
                                                    {
                                                        datoetapa = "ID";
                                                    }
                                                    if (etapa.nombre == "IB")
                                                    {
                                                        datoetapa = "IB";
                                                    }
                                                    if (etapa.nombre == "AB")
                                                    {
                                                        datoetapa = "AB";
                                                    }
                                                    if (etapa.nombre == "N/A")
                                                    {
                                                        datoetapa = "N/A";
                                                    }

                                                }

                                                DetalleItemIngenieria di = new DetalleItemIngenieria
                                                {
                                                    Id = 0,
                                                    ColaboradorId = col.Id,
                                                    DetalleAvanceIngenieriaId = detalleAvanceId,
                                                    cantidad_horas = Decimal.Parse(le.hH),
                                                    especialidad = especialidad != null ? especialidad.Id : 0,
                                                    etapa = datoetapa == "ID" ? DetalleItemIngenieria.Etapa.ID : datoetapa == "IB" ? DetalleItemIngenieria.Etapa.IB : datoetapa == "AB" ? DetalleItemIngenieria.Etapa.AB : DetalleItemIngenieria.Etapa.NA,
                                                    fecha_registro = DateTime.Parse(le.fecha),
                                                    tipo_registro = tiporegistro != null ? tiporegistro.Id : 0,

                                                };
                                                var detalleiitem = _detalleitemingenieria.InsertAndGetId(di);

                                            }

                                        }


                                    }
                                }




                            }



                        }
                       */
                        return "OK";
                    }
                }
                else
                {
                    result = "NO_EXCEL";
                }
            }
            else
            {
                result = "SIN_ARCHIVO";
            }
            return result = "";

        }
    }
}
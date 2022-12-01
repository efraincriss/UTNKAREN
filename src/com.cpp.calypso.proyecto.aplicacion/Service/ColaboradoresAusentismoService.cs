using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.seguridad.aplicacion;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.cpp.calypso.proyecto.aplicacion.Service
{
    public class ColaboradoresAusentismoAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradoresAusentismo, ColaboradoresAusentismoDto, PagedAndFilteredResultRequestDto>, IColaboradoresAusentismoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Colaboradores> _colaboradoresRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<ColaboradoresAusentismoRequisitos> _ausentismoRequisitoRepository;
        private readonly IBaseRepository<Usuario> _usuarioRepository;
        private readonly IBaseRepository<Archivo> _ArchivoRepository;

        public ColaboradoresAusentismoAsyncBaseCrudAppService(
            IBaseRepository<ColaboradoresAusentismo> repository,
            IBaseRepository<Colaboradores> colaboradoresRepository,
            IBaseRepository<Catalogo> catalogoRepository,
            IBaseRepository<ColaboradoresAusentismoRequisitos> ausentismoRequisitoRepository,
            IBaseRepository<Usuario> usuarioRepository,
            IBaseRepository<Archivo> ArchivoRepository
            ) : base(repository)
        {
            _colaboradoresRepository = colaboradoresRepository;
            _catalogoRepository = catalogoRepository;
            _ausentismoRequisitoRepository = ausentismoRequisitoRepository;
            _usuarioRepository = usuarioRepository;
            _ArchivoRepository = ArchivoRepository;
        }

        public string ActualizarAusentismoAsync(ColaboradoresAusentismo colaboradoresAusentismo)
        {
            var result = Repository.Update(colaboradoresAusentismo);
            return result.ToString();
        }

        public string CrearAusentismoAsync(ColaboradoresAusentismoDto colaboradoresAusentismo)
        {
            /* Actualizamos el estado del ausentismo de colaborador 
            Colaboradores co = _colaboradoresRepository.Get(colaboradoresAusentismo.colaborador_id);
            _colaboradoresRepository.Update(co);*/

            var result = Repository.InsertAndGetId(MapToEntity(colaboradoresAusentismo));
            return result.ToString();
        }

        public bool EliminarAusentismo(int Id)
        {
            var modelo = Repository.Get(Id);

            if (modelo != null)
            {
                modelo.vigente = false;
                modelo.IsDeleted = true;
                Repository.Update(modelo);
                return true;
            }
            return false;
        }

        public ColaboradoresAusentismo GetAusentismo(int Id)
        {
            var modelo = Repository.Get(Id);
            return modelo;
        }

        public List<ColaboradoresAusentismoDto> GetAusentismos()
        {
            var e = 1;
            var query = Repository.GetAll().Where(c => c.estado == RRHHCodigos.ESTADO_ACTIVO && c.vigente == true);

            var ausentismos = (from d in query
                               select new ColaboradoresAusentismoDto
                               {
                                   Id = d.Id,
                                   colaborador_id = d.colaborador_id,
                                   Colaborador = d.Colaborador,
                                   catalogo_tipo_ausentismo_id = d.catalogo_tipo_ausentismo_id,
                                   fecha_inicio = d.fecha_inicio,
                                   fecha_fin = d.fecha_fin,
                                   estado = d.estado,
                                   vigente = d.vigente,
                                   nombre_ausentismo = d.TipoAusentismo.nombre,
                                   observacion = d.observacion,
                                   nro_identificacion = d.Colaborador.numero_identificacion

                               }).ToList();


            foreach (var i in ausentismos)
            {
                i.nro = e++;

                i.tipo_identificacion = _catalogoRepository.GetAll().Where(c => c.Id == i.Colaborador.catalogo_tipo_identificacion_id).Select(c => c.nombre).FirstOrDefault();
                i.nombres = i.Colaborador.nombres_apellidos;
                i.grupo_personal = _catalogoRepository.GetAll().Where(c => c.Id == i.Colaborador.catalogo_grupo_personal_id).Select(c => c.nombre).FirstOrDefault();
                i.estado_colaborador = i.Colaborador.estado;

                /* NRO LEGAJO */
                if (i.Colaborador.numero_legajo_definitivo == null)
                {
                    i.nro_legajo = i.Colaborador.numero_legajo_temporal;
                }
                else
                {
                    i.nro_legajo = i.Colaborador.numero_legajo_definitivo;
                }

                i.RequisitoArchivos = this.ObtenerArchivos(i.Id);
            }
            return ausentismos;
        }

        public List<ColaboradoresAusentismoDto> GetAusentismosColaborador(int id)
        {
            var query = Repository.GetAll().Where(c => c.colaborador_id == id
                                            //&& c.estado == RRHHCodigos.ESTADO_ACTIVO
                                            && c.vigente == true).ToList();

            if (query != null)
            {
                var ausentismos = (from d in query
                                   select new ColaboradoresAusentismoDto
                                   {
                                       Id = d.Id,
                                       colaborador_id = d.colaborador_id,
                                       catalogo_tipo_ausentismo_id = d.catalogo_tipo_ausentismo_id,
                                       fecha_inicio = d.fecha_inicio,
                                       fecha_fin = d.fecha_fin,
                                       estado = d.estado,
                                       vigente = d.vigente,
                                       nombre_ausentismo = d.TipoAusentismo.nombre,
                                       codigo_ausentismo = d.TipoAusentismo.codigo,
                                       observacion = d.observacion,
                                       formatFechaInicio = d.fecha_inicio.GetValueOrDefault().ToShortDateString(),
                                       formatFechaFin = d.fecha_fin.GetValueOrDefault().ToShortDateString()

                                   }).ToList();

                foreach (var i in ausentismos)
                {
                    var req = _ausentismoRequisitoRepository.GetAll().Where(c => c.colaborador_ausentismo_id == i.Id).ToList();
                    if (req != null)
                    {

                        var requisitos = (from d in req
                                          select new ColaboradoresAusentismoRequisitosDto
                                          {
                                              Id = d.Id,
                                              colaborador_ausentismo_id = d.colaborador_ausentismo_id,
                                              requisito_id = d.requisito_id,
                                              Requisitos = d.Requisitos,
                                              archivo_id = d.archivo_id,
                                              cumple = d.cumple,
                                              Archivo = d.Archivo,
                                          }).ToList();

                        i.requisitos = requisitos;
                    }
                }
                return ausentismos.OrderByDescending(c => c.fecha_fin.Value).ToList();

            }

            return null;

        }

        public List<Colaboradores> consultaFiltrosReporte(ColaboradorReporteDto colaborador)
        {
            var query = _colaboradoresRepository.GetAll().Where(c => c.vigente == true && c.es_externo == false);

            if (colaborador.tipo_identificacion > 0)
            {
                query = query.Where(x => x.catalogo_tipo_identificacion_id == colaborador.tipo_identificacion);
            }
            if (colaborador.numero_identificacion != null)
            {
                query = query.Where(x => x.numero_identificacion.StartsWith(colaborador.numero_identificacion));
            }
            if (colaborador.nombres_apellidos != null)
            {
                query = query.Where(x => x.nombres.ToUpper().Contains(colaborador.nombres_apellidos) || x.primer_apellido.ToUpper().Contains(colaborador.nombres_apellidos) || x.segundo_apellido.ToUpper().Contains(colaborador.nombres_apellidos) || (x.primer_apellido + " " + x.segundo_apellido).ToUpper().Contains(colaborador.nombres_apellidos));
            }
            if (colaborador.id_sap > 0)
            {
                query = query.Where(x => x.empleado_id_sap == colaborador.id_sap || x.candidato_id_sap == colaborador.id_sap);
            }
            if (colaborador.posicion != null)
            {
                query = query.Where(x => x.posicion.StartsWith(colaborador.posicion));
            }
            if (colaborador.estado != null)
            {
                query = query.Where(x => x.estado.StartsWith(colaborador.estado));
            }
            if (colaborador.grupo_personal > 0)
            {
                query = query.Where(x => x.catalogo_grupo_personal_id == colaborador.grupo_personal);
            }
            if (colaborador.encargado_personal > 0)
            {
                query = query.Where(x => x.catalogo_encargado_personal_id == colaborador.encargado_personal);
            }
            if (colaborador.fecha_ingreso_desde != null)
            {
                query = query.Where(x => x.fecha_ingreso >= colaborador.fecha_ingreso_desde);
            }
            if (colaborador.fecha_ingreso_hasta != null)
            {
                query = query.Where(x => x.fecha_ingreso <= colaborador.fecha_ingreso_hasta);
            }

            var colaboradores = Mapper.Map<IQueryable<Colaboradores>, List<Colaboradores>>(query);

            return colaboradores;
        }

        public ExcelPackage reporteInformacionGeneral(ColaboradorReporteDto colaborador)
        {
            var colaboradores = consultaFiltrosReporte(colaborador);

            if (colaboradores.Count > 0)
            {

                DateTime fechaActual = DateTime.Now;

                var aux = fechaActual.ToString("ddMMyyyyhhmm");
                var fecha = fechaActual.ToString("dd/MM/yyyy");

                var usuario = "";
                string user = System.Web.HttpContext.Current.User.Identity.Name.ToString();
                var usuarioencontrado = _usuarioRepository.GetAll().Where(c => c.Cuenta == user).FirstOrDefault();
                if (usuarioencontrado != null && usuarioencontrado.Id > 0)
                {
                    usuario = usuarioencontrado.NombresCompletos;
                }
                #region Alto y Ancho de Columnas del Excel
                ExcelPackage excel = new ExcelPackage();
                //Crear hoja en archivo excel
                var hoja = excel.Workbook.Worksheets.Add("Información de Ausentismos");
                var row = 10;
                //Width de Columnas
                hoja.Column(1).Width = 1.45;
                hoja.Column(2).Width = 9;
                hoja.Column(3).Width = 10;
                hoja.Column(4).Width = 11;
                hoja.Column(5).Width = 9;
                hoja.Column(6).Width = 9;
                hoja.Column(7).Width = 9;
                hoja.Column(8).Width = 18;
                hoja.Column(9).Width = 18;
                hoja.Column(10).Width = 29;
                hoja.Column(11).Width = 15;
                hoja.Column(12).Width = 10;
                hoja.Column(13).Width = 10;
                hoja.Column(14).Width = 10;
                hoja.Column(15).Width = 12;

                //height filas de titulos
                hoja.Row(1).Height = 30;
                hoja.Row(2).Height = 15;
                hoja.Row(3).Height = 30;
                hoja.Row(4).Height = 15;
                hoja.Row(5).Height = 30;
                hoja.Row(6).Height = 30;
                hoja.Row(7).Height = 30;
                hoja.Row(8).Height = 15;
                hoja.Row(9).Height = 30;

                #endregion

                #region Cabecera del Documento
                //Cabecera de Documento
                hoja.Cells["B1:O1"].Style.Font.Bold = true;
                hoja.Cells["B1:O1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B1:O1"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B1:O1"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B1:O1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                hoja.Cells["E1"].Value = "Reporte de Información de Ausentismos";
                hoja.Cells["E1"].Style.Font.Color.SetColor(Color.White);
                hoja.Cells["E1"].Style.Font.Name = "Arial";
                hoja.Cells["E1"].Style.Font.Size = 20;

                hoja.Cells["B3:O3"].Style.Font.Bold = true;
                hoja.Cells["B3:O3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B3:O3"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B3:O3"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B3:O3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                hoja.Cells["B3"].Value = "Una vez completado cargar en el CSC Ecuador";
                hoja.Cells["B3"].Style.Font.Color.SetColor(Color.White);
                hoja.Cells["B3"].Style.Font.Name = "Calibri";
                hoja.Cells["B3"].Style.Font.Size = 12;

                hoja.Cells["B5:O5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B5:O5"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B5:O5"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B5:O5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["B5"].Value = "Proyecto:";
                hoja.Cells["B5"].Style.Font.Name = "Calibri";
                hoja.Cells["B5"].Style.Font.Size = 12;
                hoja.Cells["D5"].Value = "";
                hoja.Cells["D5"].Style.Font.Name = "Calibri";
                hoja.Cells["D5"].Style.Font.Size = 10;

                hoja.Cells["B6:O6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B6:O6"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B6:O6"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B6:O6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["B6"].Value = "Cargado por:";
                hoja.Cells["B6"].Style.Font.Name = "Calibri";
                hoja.Cells["B6"].Style.Font.Size = 12;
                hoja.Cells["D6"].Value = usuario;
                hoja.Cells["D6"].Style.Font.Name = "Calibri";
                hoja.Cells["D6"].Style.Font.Size = 10;

                hoja.Cells["B7:O7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B7:O7"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B7:O7"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B7:O7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["B7"].Value = "Fecha:";
                hoja.Cells["B7"].Style.Font.Name = "Calibri";
                hoja.Cells["B7"].Style.Font.Size = 12;
                hoja.Cells["D7"].Value = fecha;
                hoja.Cells["D7"].Style.Font.Name = "Calibri";
                hoja.Cells["D7"].Style.Font.Size = 10;

                //Cabecera de la tabla de Alta de Colaboradores
                var titleCell = hoja.Cells["B9:O9"]; // Celdas de títulos



                titleCell.Style.Font.Bold = true;
                titleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                titleCell.Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                titleCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                titleCell.Style.WrapText = true;

                titleCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                titleCell.Style.Border.Right.Color.SetColor(Color.White);
                hoja.Cells["B9:O9"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                hoja.Cells["B9:O9"].Style.Border.Right.Color.SetColor(Color.White);

                #endregion

                #region Cabeceras de los Datos
                hoja.Cells["B9"].Value = "ID SAP";
                hoja.Cells["C9"].Value = "TIPO DE IDENTIFICACIÓN";
                hoja.Cells["D9"].Value = "No. DE IDENTIFICACIÓN";
                hoja.Cells["E9"].Value = "META4";
                hoja.Cells["F9"].Value = "LEGAJO TEMPORAL";
                hoja.Cells["G9"].Value = "LEGAJO DEFINITIVO";
                hoja.Cells["H9"].Value = "APELLIDOS";
                hoja.Cells["I9"].Value = "NOMBRES";
                hoja.Cells["J9"].Value = "APELLIDOS Y NOMBRES";
                hoja.Cells["K9"].Value = "TIPO DE AUSENTISMO";
                hoja.Cells["L9"].Value = "FECHA DE INICIO";
                hoja.Cells["M9"].Value = "FECHA FIN";
                hoja.Cells["N9"].Value = "DIAS AUSENCIA";
                hoja.Cells["O9"].Value = "ESTADO";

                #endregion

                foreach (var i in colaboradores)
                {
                    var query = Repository.GetAll().Where(c => c.colaborador_id == i.Id);

                    if (colaborador.tipo_ausentismo != null)
                    {
                        query = query.Where(x => x.catalogo_tipo_ausentismo_id == colaborador.tipo_ausentismo);
                    }
                    if (colaborador.fecha_inicio_desde != null)
                    {
                        query = query.Where(x => x.fecha_inicio >= colaborador.fecha_inicio_desde);
                    }
                    if (colaborador.fecha_inicio_hasta != null)
                    {
                        query = query.Where(x => x.fecha_inicio <= colaborador.fecha_inicio_hasta);
                    }
                    if (colaborador.fecha_fin_desde != null)
                    {
                        query = query.Where(x => x.fecha_fin >= colaborador.fecha_fin_desde);
                    }
                    if (colaborador.fecha_fin_hasta != null)
                    {
                        query = query.Where(x => x.fecha_fin <= colaborador.fecha_fin_hasta);
                    }

                    var ausentismo = Mapper.Map<IQueryable<ColaboradoresAusentismo>, List<ColaboradoresAusentismo>>(query);

                    if (ausentismo != null)
                    {
                        foreach (var c in ausentismo)
                        {
                            #region Estilo
                            var body = hoja.Cells["B" + row + ":O" + row];
                            body.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            body.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            body.Style.Font.Name = "Calibri";
                            body.Style.Font.Size = 9;
                            body.Style.WrapText = true;
                            body.Style.Border.BorderAround(ExcelBorderStyle.Dotted, System.Drawing.Color.Black);

                            body.Style.Border.Right.Style = ExcelBorderStyle.Dotted;
                            body.Style.Border.Right.Color.SetColor(Color.Black);

                            hoja.Row(row).Height = 24;

                            #endregion

                            hoja.Cells["B" + row].Value = i.empleado_id_sap.HasValue ? i.empleado_id_sap.Value.ToString() : "";
                            hoja.Cells["C" + row].Value = i.catalogo_tipo_identificacion_id == null ? "" :
                                                          i.TipoIdentificacion != null ?
                                                          i.TipoIdentificacion.nombre
                                                          : "";
                            hoja.Cells["D" + row].Value = i.numero_identificacion;
                            hoja.Cells["E" + row].Value = i.meta4;
                            hoja.Cells["F" + row].Value = i.numero_legajo_temporal;
                            hoja.Cells["G" + row].Value = i.numero_legajo_definitivo;
                            hoja.Cells["H" + row].Value = i.segundo_apellido == null ? i.primer_apellido : i.primer_apellido + " " + i.segundo_apellido;
                            hoja.Cells["I" + row].Value = i.nombres;
                            hoja.Cells["J" + row].Value = i.nombres_apellidos;

                            hoja.Cells["K" + row].Value = c.catalogo_tipo_ausentismo_id > 0 ?
                                                          c.TipoAusentismo != null ?
                                                          c.TipoAusentismo.nombre : "" : "";

                            hoja.Cells["L" + row].Value = c.fecha_inicio.HasValue ?
                                                                                          c.fecha_inicio.GetValueOrDefault().ToShortDateString()
                                                                                          : " ";
                            hoja.Cells["L" + row].Style.Numberformat.Format = "dd/mm/yyyy";
                            hoja.Cells["M" + row].Value = c.fecha_fin.HasValue ?
                                                                                          c.fecha_fin.GetValueOrDefault().ToShortDateString()
                                                                                          : "";
                            hoja.Cells["M" + row].Style.Numberformat.Format = "dd/mm/yyyy";
                            if (c.fecha_fin.HasValue && c.fecha_inicio.HasValue)
                            {
                                hoja.Cells["N" + row].Value = c.fecha_fin - c.fecha_inicio + new TimeSpan(1, 0, 0, 0);
                            }
                            else
                            {
                                hoja.Cells["N" + row].Value = "";
                            }
                            hoja.Cells["O" + row].Value = c.estado;

                            row++;
                        }


                    }

                }

                //System.IO.FileInfo filename = new System.IO.FileInfo(@"C:\CPP\Colaboradores\Reportes\ReporteAusentismos" + aux + usuario + ".xlsx");
                //excel.SaveAs(filename);

                return excel;
            }
            else
            {
                return null;
            }
        }

        public bool ActualizarAusentismoColaborador(int ColaboradorId)
        {
            Colaboradores e = _colaboradoresRepository.Get(ColaboradorId);
            e.tiene_ausentismo = true;
            var update = _colaboradoresRepository.Update(e);
            if (update != null && update.Id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ColaboradoresAusentismoRequisitosDto ObtenerArchivos(int Id)
        {
            var query = _ausentismoRequisitoRepository.GetAllIncluding(c => c.Archivo).Where(c => c.colaborador_ausentismo_id == Id).FirstOrDefault();
            if (query != null && query.Id > 0)
            {
                ColaboradoresAusentismoRequisitosDto e = new ColaboradoresAusentismoRequisitosDto();
                e.Id = query.Id;
                e.Archivo = query.Archivo;
                e.archivo_id = query.archivo_id;
                e.colaborador_ausentismo_id = query.colaborador_ausentismo_id;

                return e;
            }

            else
            {
                return new ColaboradoresAusentismoRequisitosDto();
            }

        }

        public void SubirPdf(int ColaboradorAusentismoRequisitoId, ArchivoDto archivo)
        {
            var baja = _ausentismoRequisitoRepository.Get(ColaboradorAusentismoRequisitoId);
            if (baja != null && baja.archivo_id > 0)
            {
                var archivoId = baja.archivo_id;
                var file = baja.Archivo;

                baja.archivo_id = null;
                _ausentismoRequisitoRepository.Update(baja);
                _ArchivoRepository.Delete(file);

            }
            var entity = _ArchivoRepository.Insert(Mapper.Map<ArchivoDto, Archivo>(archivo));


            baja.Archivo = entity;
            _ausentismoRequisitoRepository.Update(baja);
        }

        public int EditarAusentismo(ColaboradoresAusentismo e)
        {
            var entity = Repository.Get(e.Id);
            entity.fecha_inicio = e.fecha_inicio;
            entity.fecha_fin = e.fecha_fin;
            entity.observacion = e.observacion;

            var result = Repository.Update(entity);
            return entity.Id;

        }

        public bool ValidarExisteAusentimo(int tipoAusentimosId, int ColaboradorId, DateTime fechaDesde, DateTime fechaHasta, int Id)
        {
            var ausentimos = Repository.GetAll().Where(c => c.colaborador_id == ColaboradorId)
                                                .Where(c => c.vigente)
                                                //.Where(c => c.estado == RRHHCodigos.ESTADO_ACTIVO)
                                                .ToList();
            var existe_fecha_desde = (from a in ausentimos
                                      where a.fecha_inicio.HasValue
                                      where a.fecha_fin.HasValue
                                      where fechaDesde >= a.fecha_inicio
                                      where fechaDesde <= a.fecha_fin
                                   
                                      where a.Id != Id
                                      select a).FirstOrDefault();
            var existe_fecha_hasta = (from a in ausentimos
                                      where a.fecha_inicio.HasValue
                                      where a.fecha_fin.HasValue
                                      where fechaHasta >= a.fecha_inicio
                                      where fechaHasta <= a.fecha_fin
                                     
                                      where a.Id != Id
                                      select a).FirstOrDefault();
            if (existe_fecha_desde != null && existe_fecha_desde.Id > 0)
            {
                return true;

            }
            if (existe_fecha_hasta != null && existe_fecha_hasta.Id > 0)
            {
                return true;
            }

            return false;

        }

        public bool DeleteAusentimo(int Id)
        {
            var e = Repository.Get(Id);
            e.vigente = false;
            e.IsDeleted = true;
            var update = Repository.Update(e);
           return update.Id>0 ? true : false;
        }
    }
}


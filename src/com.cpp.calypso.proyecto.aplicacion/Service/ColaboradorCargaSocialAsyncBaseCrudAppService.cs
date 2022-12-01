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
    public class ColaboradorCargaSocialAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradorCargaSocial, ColaboradorCargaSocialDto, PagedAndFilteredResultRequestDto>, IColaboradorCargaSocialAsyncBaseCrudAppService
    {
        private readonly ICatalogoAsyncBaseCrudAppService _catalogoRepository;
        private readonly IBaseRepository<Colaboradores> _colaboradoresRepository;
        private readonly IBaseRepository<ColaboradorDiscapacidad> _discapacidadRepository;
        private readonly IBaseRepository<Usuario> _usuarioRepository;

        public ColaboradorCargaSocialAsyncBaseCrudAppService(
            IBaseRepository<ColaboradorCargaSocial> repository,
            ICatalogoAsyncBaseCrudAppService catalogoRepository,
            IBaseRepository<Colaboradores> colaboradoresRepository,
            IBaseRepository<ColaboradorDiscapacidad> discapacidadRepository,
            IBaseRepository<Usuario> usuarioRepository
            ) : base(repository)
        {
            _catalogoRepository = catalogoRepository;
            _colaboradoresRepository = colaboradoresRepository;
            _discapacidadRepository = discapacidadRepository;
            _usuarioRepository = usuarioRepository;

        }

        public List<ColaboradorCargaSocialDto> GetCargas(int Id)
        {
            var cont = 1;
            var query = Repository.GetAll().Where(c => c.ColaboradoresId == Id && c.vigente == true);

            List<ColaboradorCargaSocialDto> cargas =
            (from c in query
             select new ColaboradorCargaSocialDto
             {
                 Id = c.Id,
                 ColaboradoresId = c.ColaboradoresId,
                 parentesco_id = c.parentesco_id,
                 idTipoIdentificacion = c.idTipoIdentificacion,
                 nro_identificacion = c.nro_identificacion,
                 primer_apellido = c.primer_apellido,
                 segundo_apellido = c.segundo_apellido,
                 nombres = c.nombres,
                 idGenero = c.idGenero,
                 fecha_nacimiento = c.fecha_nacimiento,
                 pais_nacimiento = c.pais_nacimiento,
                 PaisId = c.PaisId,
                 estado_civil = c.estado_civil,
                 fecha_matrimonio = c.fecha_matrimonio,
                 Colaboradores = c.Colaboradores,
                 Pais = c.Pais,
                 vigente = c.vigente,
                 por_sustitucion = c.por_sustitucion,
                 nombres_apellidos = c.nombres_apellidos,
                 nombre_sustituto = c.por_sustitucion == true ? "SI" : "NO",
                 viene_registro_civil = c.viene_registro_civil
             }).ToList(); ;

            foreach (var i in cargas)
            {
                i.nro = cont++;
                i.apellidos = i.primer_apellido + ' ' + i.segundo_apellido;

                var catalogoIdentificacion = _catalogoRepository.GetCatalogo(i.idTipoIdentificacion);
                i.tipoIdentificacion = catalogoIdentificacion.nombre;

                var catalogoparentesco = _catalogoRepository.GetCatalogo(i.parentesco_id);
                i.parentesco = catalogoparentesco.nombre;

                var catalogoGenero = _catalogoRepository.GetCatalogo(i.idGenero);
                i.nombre_genero = catalogoGenero.nombre;
            }

            return cargas;
        }

        public string UniqueIdentification(int Id, string nro)
        {
            var result = "NO";
            var identificacion = Repository.GetAll().Where(d => d.nro_identificacion == nro && d.vigente == true);

            if (identificacion != null)
            {
                foreach (var i in identificacion)
                {

                    if (i.ColaboradoresId == Id)
                    {
                        result = "SI";
                        break;
                    }
                    else
                    {
                        result = "ID " + i.Id;
                    }


                }
            }
            else
            {
                result = "NO";
            }

            return result;
        }

        public ColaboradorCargaSocialDto GetInfoCargaSocialWS(int tipoIdentificacion, string cedula)
        {
            var d = Repository.GetAll().Where(c => c.idTipoIdentificacion == tipoIdentificacion
            && c.nro_identificacion == cedula).FirstOrDefault();

            if (d != null)
            {

                ColaboradorCargaSocialDto carga = new ColaboradorCargaSocialDto()
                {
                    Id = d.Id,
                    ColaboradoresId = d.ColaboradoresId,
                    parentesco_id = d.parentesco_id,
                    idTipoIdentificacion = d.idTipoIdentificacion,
                    nro_identificacion = d.nro_identificacion,
                    primer_apellido = d.primer_apellido,
                    segundo_apellido = d.segundo_apellido,
                    nombres = d.nombres,
                    idGenero = d.idGenero,
                    fecha_nacimiento = d.fecha_nacimiento,
                    pais_nacimiento = d.pais_nacimiento,
                    PaisId = d.PaisId,
                    estado_civil = d.estado_civil,
                    fecha_matrimonio = d.fecha_matrimonio,
                    Colaboradores = d.Colaboradores,
                    Pais = d.Pais,
                    vigente = d.vigente,
                    por_sustitucion = d.por_sustitucion,
                    nombres_apellidos = d.nombres_apellidos,
                    viene_registro_civil = d.viene_registro_civil
                };


                return carga;
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

                #region Alto y Ancho de la Columnas
                ExcelPackage excel = new ExcelPackage();
                //Crear hoja en archivo excel
                var hoja = excel.Workbook.Worksheets.Add("Información de Cargas Familiares ");
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
                hoja.Column(12).Width = 18;
                hoja.Column(13).Width = 18;
                hoja.Column(14).Width = 29;
                hoja.Column(15).Width = 9;
                hoja.Column(16).Width = 10;
                hoja.Column(17).Width = 10;
                hoja.Column(18).Width = 11;
                hoja.Column(19).Width = 14;
                hoja.Column(20).Width = 11;

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

                hoja.Cells["B1:T1"].Style.Font.Bold = true;
                hoja.Cells["B1:T1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B1:T1"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B1:T1"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B1:T1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                hoja.Cells["E1"].Value = "Reporte de Información de Cargas Familiares";
                hoja.Cells["E1"].Style.Font.Color.SetColor(Color.White);
                hoja.Cells["E1"].Style.Font.Name = "Arial";
                hoja.Cells["E1"].Style.Font.Size = 20;

                hoja.Cells["B3:T3"].Style.Font.Bold = true;
                hoja.Cells["B3:T3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B3:T3"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B3:T3"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B3:T3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                hoja.Cells["B3"].Value = "Una vez completado cargar en el CSC Ecuador";
                hoja.Cells["B3"].Style.Font.Color.SetColor(Color.White);
                hoja.Cells["B3"].Style.Font.Name = "Calibri";
                hoja.Cells["B3"].Style.Font.Size = 12;

                hoja.Cells["B5:T5"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B5:T5"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B5:T5"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B5:T5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["B5"].Value = "Proyecto:";
                hoja.Cells["B5"].Style.Font.Name = "Calibri";
                hoja.Cells["B5"].Style.Font.Size = 12;
                hoja.Cells["D5"].Value = "";
                hoja.Cells["D5"].Style.Font.Name = "Calibri";
                hoja.Cells["D5"].Style.Font.Size = 10;

                hoja.Cells["B6:T6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B6:T6"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B6:T6"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B6:T6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["B6"].Value = "Cargado por:";
                hoja.Cells["B6"].Style.Font.Name = "Calibri";
                hoja.Cells["B6"].Style.Font.Size = 12;
                hoja.Cells["D6"].Value = usuario;
                hoja.Cells["D6"].Style.Font.Name = "Calibri";
                hoja.Cells["D6"].Style.Font.Size = 10;

                hoja.Cells["B7:T7"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                hoja.Cells["B7:T7"].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                hoja.Cells["B7:T7"].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                hoja.Cells["B7:T7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["B7"].Value = "Fecha:";
                hoja.Cells["B7"].Style.Font.Name = "Calibri";
                hoja.Cells["B7"].Style.Font.Size = 12;
                hoja.Cells["D7"].Value = fecha;
                hoja.Cells["D7"].Style.Font.Name = "Calibri";
                hoja.Cells["D7"].Style.Font.Size = 10;

                //Cabecera de la tabla de Alta de Colaboradores
                var titleCell = hoja.Cells["B9:T9"]; // Celdas de títulos

                titleCell.Style.Font.Bold = true;
                titleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                titleCell.Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
                titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                titleCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                titleCell.Style.WrapText = true;

                titleCell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                titleCell.Style.Border.Right.Color.SetColor(Color.White);
                hoja.Cells["B9:T9"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                hoja.Cells["B9:T9"].Style.Border.Right.Color.SetColor(Color.White);

                #endregion

                #region Cabecera de la Columnas

                hoja.Cells["B9"].Value = "ID SAP";
                hoja.Cells["C9"].Value = "TIPO DE IDENTIFICACIÓN";
                hoja.Cells["D9"].Value = "No. DE IDENTIFICACIÓN";
                hoja.Cells["E9"].Value = "META4";
                hoja.Cells["F9"].Value = "LEGAJO TEMPORAL";
                hoja.Cells["G9"].Value = "LEGAJO DEFINITIVO";
                hoja.Cells["H9"].Value = "APELLIDOS";
                hoja.Cells["I9"].Value = "NOMBRES";
                hoja.Cells["J9"].Value = "APELLIDOS Y NOMBRES";
                hoja.Cells["K9"].Value = "PARENTESCO";
                hoja.Cells["L9"].Value = "APELLIDO PATERNO";
                hoja.Cells["M9"].Value = "APELLIDO MATERNO";
                hoja.Cells["N9"].Value = "NOMBRES";
                hoja.Cells["O9"].Value = "SEXO";
                hoja.Cells["P9"].Value = "FECHA DE NACIMIENTO";
                hoja.Cells["Q9"].Value = "TIPO DE IDENTIFICACIÓN";
                hoja.Cells["R9"].Value = "No. DE IDENTIFICACIÓN";
                hoja.Cells["S9"].Value = "PERSONA CON DISCAPACIDAD";
                hoja.Cells["T9"].Value = "SUSTITUTO";

                #endregion


                foreach (var i in colaboradores)
                {
                    var cargasQuery = Repository.GetAll().Where(c => c.ColaboradoresId == i.Id);
                    var cargas = Mapper.Map<IQueryable<ColaboradorCargaSocial>, List<ColaboradorCargaSocial>>(cargasQuery);

                    if (cargas != null && cargas.Count > 0)
                    {
                        foreach (var c in cargas)
                        {
                            #region Estilo
                            var body = hoja.Cells["B" + row + ":T" + row];
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
                            hoja.Cells["C" + row].Value = i.catalogo_tipo_identificacion_id == null ? "" : i.TipoIdentificacion != null ? i.TipoIdentificacion.nombre : "";
                            hoja.Cells["D" + row].Value = i.numero_identificacion;
                            hoja.Cells["E" + row].Value = i.meta4;
                            hoja.Cells["F" + row].Value = i.numero_legajo_temporal;
                            hoja.Cells["G" + row].Value = i.numero_legajo_definitivo;
                            hoja.Cells["H" + row].Value = i.segundo_apellido == null ? i.primer_apellido : i.primer_apellido + " " + i.segundo_apellido;
                            hoja.Cells["I" + row].Value = i.nombres;
                            hoja.Cells["J" + row].Value = i.nombres_apellidos;
                            hoja.Cells["K" + row].Value = c.parentesco_id > 0 ? _catalogoRepository.GetCatalogo(c.parentesco_id).nombre : "";
                            hoja.Cells["L" + row].Value = c.primer_apellido;
                            hoja.Cells["M" + row].Value = c.segundo_apellido;
                            hoja.Cells["N" + row].Value = c.nombres;
                            hoja.Cells["O" + row].Value = c.idGenero > 0 ? _catalogoRepository.GetCatalogo(c.idGenero).nombre : "";
                            hoja.Cells["P" + row].Value = String.Format("{0:dd/MM/yyyy}", c.fecha_nacimiento.HasValue ? c.fecha_nacimiento.GetValueOrDefault().ToShortDateString() : "");
                            hoja.Cells["Q" + row].Value = c.idTipoIdentificacion > 0 ? _catalogoRepository.GetCatalogo(c.idTipoIdentificacion).nombre : "";
                            hoja.Cells["R" + row].Value = c.nro_identificacion;

                            var discapacidad = _discapacidadRepository.GetAll().Where(d => d.ColaboradorCargaSocialId == c.Id).FirstOrDefault();

                            hoja.Cells["S" + row].Value = discapacidad != null ? "SI" : "NO";
                            hoja.Cells["T" + row].Value = c.por_sustitucion == true ? "SI" : "NO";

                            row++;
                        }


                    }

                }

                //System.IO.FileInfo filename = new System.IO.FileInfo(@"C:\CPP\Colaboradores\Reportes\ReporteCargasFamiliares" + aux + usuario + ".xlsx");
                //excel.SaveAs(filename);

                return excel;
            }
            else
            {
                return null;
            }
        }

    }
}

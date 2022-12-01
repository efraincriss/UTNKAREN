using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Interface;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.aplicacion.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;
using com.cpp.calypso.proyecto.dominio.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace com.cpp.calypso.proyecto.aplicacion.Acceso.Service
{
    public class ValidacionRequisitoAsyncBaseCrudAppService : AsyncBaseCrudAppService<Colaboradores, ColaboradoresDto, PagedAndFilteredResultRequestDto>, IValidacionRequisitoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<RequisitoColaborador> _requisitoColaboradoRepository;
        private readonly IApplication Application;
        private readonly IBaseRepository<ColaboradorRequisito> _colaboradorRequisitoRepository;
        private readonly IBaseRepository<ColaboradorResponsabilidad> _colaboradorResponsabilidadRepository;
        private readonly IBaseRepository<Requisitos> _requisitoRepository;
        private readonly IBaseRepository<Colaboradores> _colaboradorRepository;
        public ICatalogoAsyncBaseCrudAppService _catalogoService { get; }


        public ValidacionRequisitoAsyncBaseCrudAppService(
            IBaseRepository<Colaboradores> repository,
            IBaseRepository<RequisitoColaborador> requisitoColaboradoRepository,
            IApplication application,
            IBaseRepository<ColaboradorRequisito> colaboradorRequisitoRepository,
            IBaseRepository<ColaboradorResponsabilidad> colaboradorResponsabilidadRepository,
               IBaseRepository<Colaboradores> colaboradorRepository,
            IBaseRepository<Requisitos> requisitoRepository,
            ICatalogoAsyncBaseCrudAppService catalogoService
            ) : base(repository)
        {
            _requisitoColaboradoRepository = requisitoColaboradoRepository;
            Application = application;
            _colaboradorRequisitoRepository = colaboradorRequisitoRepository;
            _colaboradorResponsabilidadRepository = colaboradorResponsabilidadRepository;
            _requisitoRepository = requisitoRepository;
            _colaboradorRepository = colaboradorRepository;
            _catalogoService = catalogoService;

        }

        public List<ValidacionRequisitoDto> ObtenerRequisitos(InputRequisitosDto input)
        {
            var currentUserIdentificacion = Application.GetCurrentUser().Identificacion;

            var colaborador = Repository
                .GetAll()
                .FirstOrDefault(o => o.numero_identificacion == currentUserIdentificacion);

            //Colaboradores
            if (colaborador == null)
            {
                return new List<ValidacionRequisitoDto>();
            }
            var query = _requisitoColaboradoRepository.GetAll()
                .Include(o => o.Requisitos).Include(o => o.Requisitos.Responsable)
                .Where(o => o.rolId == input.AccionId)
                .Where(o => o.tipo_usuarioId == input.GrupoPersonalId)
                .Where(o => o.vigente);

            if (input.TipoBajaId.HasValue)
            {
                query = query.Where(o => o.catalogo_motivo_baja_id == input.TipoBajaId.Value);
            }

            var gruposPersonales = query.ToList();

            var listado = new List<ValidacionRequisitoDto>();
            var count = 1;
            foreach (var grupo in gruposPersonales)
            {
                var responsabilidad = _colaboradorResponsabilidadRepository
                    .GetAll()
                    .Where(o => o.catalogo_responsable_id == grupo.Requisitos.responsableId)
                    .FirstOrDefault(o => o.colaborador_id == colaborador.Id);

                if (responsabilidad != null)
                {
                    var colaboradorRequisito = _colaboradorRequisitoRepository
                        .GetAll()
                        .Where(o => o.vigente)
                        .Where(o => o.ColaboradoresId == input.ColaboradorId)
                        .FirstOrDefault(o => o.RequisitosId == grupo.RequisitosId);


                    if (colaboradorRequisito == null)
                    {
                        var colRequ = new ColaboradorRequisito()
                        {
                            ColaboradoresId = input.ColaboradorId,
                            RequisitosId = grupo.RequisitosId,
                            cumple = false,
                            vigente = true,
                        };

                        colaboradorRequisito = _colaboradorRequisitoRepository.Insert(colRequ);
                    }

                    bool editable = responsabilidad.acceso == "M";

                    var dto = new ValidacionRequisitoDto()
                    {
                        Id = count,
                        ColaboradorRequisitoId = colaboradorRequisito.Id,
                        RequisitoId = grupo.RequisitosId,
                        Obligatorio = grupo.GetObligatorio(),
                        Codigo = grupo.Requisitos.codigo,
                        Cumple = colaboradorRequisito.GetCumple(),
                        CumpleBool = colaboradorRequisito.cumple,
                        FechaEmision = colaboradorRequisito.fecha_emision,
                        FechaCaducidad = colaboradorRequisito.fecha_caducidad,
                        Nombre = grupo.Requisitos.nombre,
                        ArchivoId = colaboradorRequisito.ArchivoId,
                        Editable = editable,
                        Observacion = colaboradorRequisito.observacion,
                        Vigente = colaboradorRequisito.vigente,
                        AplicaCaducidad = grupo.Requisitos.GetAplicaCaducidad(),
                        TiempoVigenciaMaximo = grupo.Requisitos.GetTiempoVigencia(),
                        Responsable = grupo.Requisitos.Responsable.nombre
                    };
                    count++;
                    listado.Add(dto);
                }
            }

            return listado;
        }


        public int UpdateApi(CreateColaboradorRequisitoDto input)
        {
            //var entity = Mapper.Map<CreateColaboradorRequisitoDto, ColaboradorRequisito>(input);
            var entity = _colaboradorRequisitoRepository.GetAll().Where(c => c.Id == input.Id).Where(c => c.vigente).FirstOrDefault();

            entity.fecha_caducidad = input.fecha_caducidad;
            entity.fecha_emision = input.fecha_emision;
            if (input.ArchivoId.HasValue)
            {
                entity.ArchivoId = input.ArchivoId;
            }
            entity.cumple = input.cumple;
            var result = _colaboradorRequisitoRepository.Update(entity);
            return entity.Id;
        }


        public string FechasValidas(int requisitoId, DateTime fechaEmision, DateTime fechaCaducidad)
        {
            var requisito = _requisitoRepository.Get(requisitoId);

            if (!requisito.caducidad.HasValue)
                return "OK";

            if (!requisito.caducidad.Value)
                return "OK";

            if (!requisito.tiempo_vigencia.HasValue)
                return "OK";

            var months = requisito.tiempo_vigencia.Value;

            var maxDate = fechaEmision.AddMonths(months).Date;

            if (fechaCaducidad <= maxDate)
                return "OK";

            if (fechaEmision > fechaCaducidad)
                return "La fecha de emisión no puede ser mayor a la fecha de caducidad";

            return $"La fecha de caducidad no debe ser mayor a {maxDate.Date}"; ;
        }
        #region ES: Reporte Requisitos
        public ExcelPackage ExcelCumplimientoIndividual(InputRequisitosReporteDto input)

        {
            var colaborador = _colaboradorRepository.Get(input.ColaboradorId);


            var dto = Mapper.Map<Colaboradores, ColaboradoresDetallesDto>(colaborador);
            List<ColaboradoresDetallesDto> lista = new List<ColaboradoresDetallesDto>();
            lista.Add(dto);
            input.Colaboradores = lista;


            ExcelPackage excel = new ExcelPackage();
            var hoja = excel.Workbook.Worksheets.Add("Cumplimiento Individual");
            hoja.DefaultRowHeight = 16;

            hoja.View.ZoomScale = 90;

            // CABECERA
            hoja.Row(2).Height = 50;
            hoja.Cells["B2:I2"].Merge = true;
            hoja.Cells["B2:I2"].Value = "REPORTE DE CUMPLIMIENTO DE REQUISITOS";
            hoja.Cells["B2:I2"].Style.WrapText = true;
            hoja.Cells["B2:I2"].Style.Font.Size = 14;
            hoja.Cells["B2:I2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B2:I2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B2:I2"].Style.Font.Bold = true;
            hoja.Cells["B2:I2"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells["B2:I2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["B2:I2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            hoja.Cells["B2:I2"].Style.Font.Color.SetColor(Color.White);

            hoja.Row(3).Height = 30;
            hoja.Cells["B3:I3"].Merge = true;
            hoja.Cells["B3:I3"].Value = "AL: " + DateTime.Now.ToShortDateString();
            hoja.Cells["B3:I3"].Style.WrapText = true;
            hoja.Cells["B3:I3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B3:I3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B3:I3"].Style.Font.Bold = true;
            hoja.Cells["B3:I3"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells["B3:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["B3:I3"].Style.Fill.BackgroundColor.SetColor(Color.White);


            hoja.Cells["C5"].Value = "TIPO IDENTIFICACIÓN:";
            hoja.Cells["C5"].Style.WrapText = true;
            hoja.Cells["C5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C5"].Style.Font.Bold = true;

            if (dto.TipoIdentificacionNombre != null)
            {
                hoja.Cells["D5"].Value = dto.TipoIdentificacionNombre;
            }

            hoja.Cells["C6"].Value = "NO. IDENTIFICACIÓN:";
            hoja.Cells["C6"].Style.WrapText = true;
            hoja.Cells["C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C6"].Style.Font.Bold = true;

            if (dto.Identificacion != null)
            {
                hoja.Cells["D6"].Value = dto.Identificacion;
            }
            hoja.Cells["C7"].Value = "NOMBRES COMPLETOS:";
            hoja.Cells["C7"].Style.WrapText = true;
            hoja.Cells["C7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C7"].Style.Font.Bold = true;

            if (dto.NombresApellidos != null)
            {
                hoja.Cells["D7"].Value = dto.NombresApellidos;
            }
            hoja.Cells["C8"].Value = "FECHA INGRESO";
            hoja.Cells["C8"].Style.WrapText = true;
            hoja.Cells["C8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C8"].Style.Font.Bold = true;

            if (dto.FechaRegistro != null)
            {
                hoja.Cells["D8"].Value = dto.FechaRegistro;
            }
            hoja.Cells["C9"].Value = "DEPARTAMENTO:";
            hoja.Cells["C9"].Style.WrapText = true;
            hoja.Cells["C9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C9"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C9"].Style.Font.Bold = true;
            if (dto.Departamento != null)
            {
                hoja.Cells["D9"].Value = dto.Departamento;
            }

            hoja.Cells[4, 2, 10, 9].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells[4, 2, 10, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[4, 2, 10, 9].Style.Fill.BackgroundColor.SetColor(Color.White);

            //TABLA

            hoja.Cells["B11"].Value = "CÓDIGO";
            hoja.Cells["B11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B11"].Style.Font.Bold = true;
            hoja.Cells["B11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["C11"].Value = "REQUISITO";
            hoja.Cells["C11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["C11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C11"].Style.Font.Bold = true;
            hoja.Cells["C11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["D11"].Value = "TIPO CADUCIDAD";
            hoja.Cells["D11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["D11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["D11"].Style.Font.Bold = true;
            hoja.Cells["D11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["E11"].Value = "OBLIGATORIO";
            hoja.Cells["E11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["E11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["E11"].Style.Font.Bold = true;
            hoja.Cells["E11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["F11"].Value = "CUMPLE";
            hoja.Cells["F11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["F11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["F11"].Style.Font.Bold = true;
            hoja.Cells["F11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["G11"].Value = "FECHA REGISTRO";
            hoja.Cells["G11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["G11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["G11"].Style.Font.Bold = true;
            hoja.Cells["G11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["H11"].Value = "FECHA VENCIMIENTO";
            hoja.Cells["H11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["H11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["H11"].Style.Font.Bold = true;
            hoja.Cells["H11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["I11"].Value = "ESTADO";
            hoja.Cells["I11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["I11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["I11"].Style.Font.Bold = true;
            hoja.Cells["I11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);



            hoja.Cells[11, 2, 11, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[11, 2, 11, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            hoja.Cells[11, 2, 11, 9].Style.Font.Color.SetColor(Color.White);

            // ICONOS

            //
            string pathpetroamazonas = System.Web.HttpContext.Current.Server.MapPath("~/Views/LogosCPP/_petroamazonas.png");
            string patharbolecuador = System.Web.HttpContext.Current.Server.MapPath("~/Views/LogosCPP/_arbolecuador.png");
            string pathcpp = System.Web.HttpContext.Current.Server.MapPath("~/Views/LogosCPP/_cpp.png");


            if (File.Exists((string)pathcpp))
            {
                Image _logocpp = Image.FromFile(pathcpp);
                var picture = hoja.Drawings.AddPicture("cpp", _logocpp);
                picture.SetPosition(4, 0, 1, 4);
                picture.SetSize(50);

            }
            //TAMAÑOS COLUMNAS

            hoja.Column(1).Width = 3;
            hoja.Column(2).Width = 22;
            hoja.Column(3).Width = 50;
            hoja.Column(4).Width = 22;
            hoja.Column(5).Width = 22;
            hoja.Column(6).Width = 22;
            hoja.Column(7).Width = 22;
            hoja.Column(8).Width = 22;
            hoja.Column(9).Width = 22;




            //DATOS 



            var datos = this.ObtenerRequisitosAccesoColaborador(input);

            if (datos.Count > 0)
            {
                int row = 12;
                foreach (var item in datos)

                {
                    hoja.Cells["B" + row].Value = item.Codigo;
                    hoja.Cells["C" + row].Value = item.Requisito;
                    hoja.Cells["D" + row].Value = item.TieneCaducidad;
                    hoja.Cells["E" + row].Value = item.Obligatorio;
                    hoja.Cells["F" + row].Value = item.Cumple;
                    hoja.Cells["G" + row].Value = item.FechaRegistro;
                    hoja.Cells["H" + row].Value = item.FechaCaducidad;
                    hoja.Cells["I" + row].Value = item.Estado;

                    hoja.Cells["B" + row].Style.WrapText = true;
                    hoja.Cells["C" + row].Style.WrapText = true;
                    hoja.Cells["D" + row].Style.WrapText = true;
                    hoja.Cells["E" + row].Style.WrapText = true;
                    hoja.Cells["F" + row].Style.WrapText = true;
                    hoja.Cells["G" + row].Style.WrapText = true;
                    hoja.Cells["H" + row].Style.WrapText = true;
                    hoja.Cells["I" + row].Style.WrapText = true;
                    hoja.Cells["I" + row].Style.Font.Bold = true;

                    hoja.Cells["B" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["C" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["D" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["E" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["F" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["G" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["H" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["I" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;



                    hoja.Cells["B" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["C" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["D" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["E" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["F" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["G" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["H" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["I" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    hoja.Cells["G" + row].Style.Numberformat.Format = "DD/MM/YYYY";
                    hoja.Cells["H" + row].Style.Numberformat.Format = "DD/MM/YYYY";

                    if (item.Estado == "OK")
                    {
                        hoja.Cells["I" + row].Style.Font.Color.SetColor(Color.Green);

                    }
                    else if (item.Estado == "ALERTA")
                    {
                        hoja.Cells["I" + row].Style.Font.Color.SetColor(Color.Orange);

                    }
                    else if (item.Estado == "VENCIDO")
                    {
                        hoja.Cells["I" + row].Style.Font.Color.SetColor(Color.Red);

                    }

                    row++;
                }
            }
            //FORMATO A UNA PAGINA
            hoja.View.PageBreakView = true;
            hoja.PrinterSettings.PrintArea = hoja.Cells[2, 2, hoja.Dimension.End.Row, hoja.Dimension.End.Column];
            hoja.PrinterSettings.FitToPage = true;

            //hoja.Cells[2, 11, hoja.Dimension.End.Row, hoja.Dimension.End.Column].AutoFilter = true;


            return excel;
        }

        public ExcelPackage ExcelListaCumplimiento(InputRequisitosReporteDto input)
        {
            ExcelPackage excel = new ExcelPackage();
            var hoja = excel.Workbook.Worksheets.Add("Lista de Cumplimiento");
            hoja.DefaultRowHeight = 16;

            hoja.View.ZoomScale = 90;

            // CABECERA
            hoja.Row(2).Height = 50;
            hoja.Cells["B2:K2"].Merge = true;
            hoja.Cells["B2:K2"].Value = "REPORTE DE CUMPLIMIENTO DE REQUISITOS";
            hoja.Cells["B2:K2"].Style.WrapText = true;
            hoja.Cells["B2:K2"].Style.Font.Size = 14;
            hoja.Cells["B2:K2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B2:K2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B2:K2"].Style.Font.Bold = true;
            hoja.Cells["B2:K2"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells["B2:K2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["B2:K2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            hoja.Cells["B2:K2"].Style.Font.Color.SetColor(Color.White);

            hoja.Row(3).Height = 30;
            hoja.Cells["B3:K3"].Merge = true;
            hoja.Cells["B3:K3"].Value = "AL: " + DateTime.Now.ToShortDateString();
            hoja.Cells["B3:K3"].Style.WrapText = true;
            hoja.Cells["B3:K3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B3:K3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B3:K3"].Style.Font.Bold = true;
            hoja.Cells[3, 2, 5, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[3, 2, 5, 11].Style.Fill.BackgroundColor.SetColor(Color.White);


            //TABLA

            hoja.Cells["B6"].Value = "DEPARTAMENTO";
            hoja.Cells["B6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B6"].Style.Font.Bold = true;
            hoja.Cells["B6"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["C6"].Value = "IDENTIFICACIÓN";
            hoja.Cells["C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["C6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C6"].Style.Font.Bold = true;
            hoja.Cells["C6"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["D6"].Value = "NOMBRES COMPLETOS";
            hoja.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["D6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["D6"].Style.Font.Bold = true;
            hoja.Cells["D6"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["E6"].Value = "REQUISITO";
            hoja.Cells["E6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["E6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["E6"].Style.Font.Bold = true;
            hoja.Cells["E6"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["F6"].Value = "TIENE CADUCIDAD";
            hoja.Cells["F6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["F6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["F6"].Style.Font.Bold = true;
            hoja.Cells["F6"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["G6"].Value = "OBLIGATORIO";
            hoja.Cells["G6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["G6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["G6"].Style.Font.Bold = true;
            hoja.Cells["G6"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["H6"].Value = "CUMPLE";
            hoja.Cells["H6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["H6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["H6"].Style.Font.Bold = true;
            hoja.Cells["H6"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["I6"].Value = "FECHA REGISTRO";
            hoja.Cells["I6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["I6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["I6"].Style.Font.Bold = true;
            hoja.Cells["I6"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["J6"].Value = "FECHA VENCIMIENTO";
            hoja.Cells["J6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["J6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["J6"].Style.Font.Bold = true;
            hoja.Cells["J6"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["K6"].Value = "ESTADO";
            hoja.Cells["K6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["K6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["K6"].Style.Font.Bold = true;
            hoja.Cells["K6"].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            hoja.Cells[6, 2, 6, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[6, 2, 6, 11].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            hoja.Cells[6, 2, 6, 11].Style.Font.Color.SetColor(Color.White);

            // ICONOS

            //
            string pathpetroamazonas = System.Web.HttpContext.Current.Server.MapPath("~/Views/LogosCPP/_petroamazonas.png");
            string patharbolecuador = System.Web.HttpContext.Current.Server.MapPath("~/Views/LogosCPP/_arbolecuador.png");
            string pathcpp = System.Web.HttpContext.Current.Server.MapPath("~/Views/LogosCPP/_cpp.png");


            if (File.Exists((string)pathcpp))
            {
                Image _logocpp = Image.FromFile(pathcpp);
                var picture = hoja.Drawings.AddPicture("cpp", _logocpp);
                picture.SetPosition(2, 1, 1, 4);
                picture.SetSize(35);

            }
            //TAMAÑOS COLUMNAS

            hoja.Column(1).Width = 3;
            hoja.Column(2).Width = 22;
            hoja.Column(3).Width = 22;
            hoja.Column(4).Width = 22;
            hoja.Column(5).Width = 22;
            hoja.Column(6).Width = 22;
            hoja.Column(7).Width = 22;
            hoja.Column(8).Width = 22;
            hoja.Column(9).Width = 22;
            hoja.Column(10).Width = 22;
            hoja.Column(11).Width = 22;

            var datos = this.ObtenerRequisitosAccesoColaborador(input);

            if (datos.Count > 0)
            {
                int row = 7;
                foreach (var item in datos)

                {
                    hoja.Cells["B" + row].Value = item.Departamento;
                    hoja.Cells["C" + row].Value = item.Identificacion;
                    hoja.Cells["D" + row].Value = item.NombresCompletos;
                    hoja.Cells["E" + row].Value = item.Requisito;
                    hoja.Cells["F" + row].Value = item.TieneCaducidad;
                    hoja.Cells["G" + row].Value = item.Obligatorio;
                    hoja.Cells["H" + row].Value = item.Cumple;
                    hoja.Cells["I" + row].Value = item.FechaRegistro;
                    hoja.Cells["J" + row].Value = item.FechaCaducidad;
                    hoja.Cells["K" + row].Value = item.Estado;

                    hoja.Cells["B" + row].Style.WrapText = true;
                    hoja.Cells["C" + row].Style.WrapText = true;
                    hoja.Cells["D" + row].Style.WrapText = true;
                    hoja.Cells["E" + row].Style.WrapText = true;
                    hoja.Cells["F" + row].Style.WrapText = true;
                    hoja.Cells["G" + row].Style.WrapText = true;
                    hoja.Cells["H" + row].Style.WrapText = true;
                    hoja.Cells["I" + row].Style.WrapText = true;
                    hoja.Cells["J" + row].Style.WrapText = true;
                    hoja.Cells["K" + row].Style.WrapText = true;
                    hoja.Cells["K" + row].Style.Font.Bold = true;

                    hoja.Cells["B" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["C" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["D" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["E" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["F" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["G" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["H" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["I" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["J" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["K" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    hoja.Cells["B" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["C" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["D" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["E" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["F" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["G" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["H" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["I" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["J" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["K" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    hoja.Cells["I" + row].Style.Numberformat.Format = "DD/MM/YYYY";
                    hoja.Cells["J" + row].Style.Numberformat.Format = "DD/MM/YYYY";

                    if (item.Estado == "OK")
                    {
                        hoja.Cells["K" + row].Style.Font.Color.SetColor(Color.Green);

                    }
                    else if (item.Estado == "ALERTA")
                    {
                        hoja.Cells["K" + row].Style.Font.Color.SetColor(Color.Orange);

                    }
                    else if (item.Estado == "VENCIDO")
                    {
                        hoja.Cells["K" + row].Style.Font.Color.SetColor(Color.Red);

                    }

                    row++;
                }
            }
            //FORMATO A UNA PAGINA
            hoja.View.PageBreakView = true;
            hoja.PrinterSettings.PrintArea = hoja.Cells[2, 2, hoja.Dimension.End.Row, hoja.Dimension.End.Column];
            hoja.PrinterSettings.FitToPage = true;

            //hoja.Cells[2, 11, hoja.Dimension.End.Row, hoja.Dimension.End.Column].AutoFilter = true;
            return excel;
        }

        public List<Requisitos> ListaRequisitosReporte(int tipoaccion)
        {
            var tipos_requisitos = _requisitoColaboradoRepository.GetAll()
                                                               .Where(c => c.Accion.codigo == "ACC")
                                                               .Where(c => c.vigente)
                                                               .Select(c => c.Requisitos).Include(c => c.TipoRequisito)
                                                               .Distinct()
                                                               .ToList();
            return tipos_requisitos;

        }

        public List<RequisitosReporteDto> ObtenerRequisitosAcceso(InputRequisitosReporteDto input)
        {

            //OBTENGO TODOS LOS  REQUISITOS GRUPO PERSONAL DE ACCESO
            var query = _requisitoColaboradoRepository.GetAll()
                .Include(o => o.Requisitos)
               .Where(o => o.vigente);

            if (input.AccionId > 0)
            {
                query = query.Where(o => o.rolId == input.AccionId); //CATALOGO ACCION ACCESOS
            }

            if (input.Obligatorios)
            {
                query.Where(o => o.obligatorio);
            }
            var gruposPersonales = query.ToList();



            var listado = new List<RequisitosReporteDto>();

            foreach (var grupo in gruposPersonales)
            {
                ColaboradorRequisito colaboradorfinal = null;
                var colaboradorRequisito = _colaboradorRequisitoRepository
                    .GetAllIncluding(c => c.Colaboradores.Sector)
                    .Where(o => o.vigente);
                if (input.DepartamentoId > 0)
                {
                    colaboradorRequisito.Where(o => o.Colaboradores.catalogo_sector_id == input.DepartamentoId); //CATALOGO ACCION ACCESOS
                }
                if (input.Vencidos)
                {
                    colaboradorRequisito.Where(o => o.fecha_caducidad < DateTime.Now);
                }

                else
                {
                    colaboradorfinal = colaboradorRequisito.FirstOrDefault(o => o.RequisitosId == grupo.RequisitosId);
                }





                if (colaboradorfinal != null)
                {


                    var dto = new RequisitosReporteDto()
                    {
                        Codigo = grupo.Requisitos.codigo,
                        Departamento = colaboradorfinal.Colaboradores.Sector != null ? colaboradorfinal.Colaboradores.Sector.nombre : "",
                        Identificacion = colaboradorfinal.Colaboradores.numero_identificacion,
                        NombresCompletos = colaboradorfinal.Colaboradores.nombres_apellidos,
                        Requisito = grupo.Requisitos.nombre,
                        TieneCaducidad = grupo.Requisitos.GetAplicaCaducidad(),
                        Obligatorio = grupo.GetObligatorio(),
                        Cumple = colaboradorfinal.GetCumple(),
                        FechaRegistro = colaboradorfinal.fecha_emision,
                        FechaCaducidad = colaboradorfinal.fecha_caducidad,
                        Estado = this.AlertaFechas(colaboradorfinal.fecha_caducidad, input.DiasVencimiento)

                    };

                    listado.Add(dto);
                }
            }

            return listado.Distinct().ToList();

        }

        public List<ColaboradoresDetallesDto> BuscarPorIdentificacionNombre(string identificacion = "", string nombre = "")
        {
            var query = _colaboradorRepository.GetAll()
                .Include(o => o.GrupoPersonal)
                ;

            if (!identificacion.IsNullOrEmpty())
            {
                query = query.Where(o => o.numero_identificacion.StartsWith(identificacion));
            }

            if (!nombre.IsNullOrEmpty())
            {
                query = query.Where(o =>
                    o.nombres_apellidos.Contains(nombre));
            }

            var entities = query.ToList();

            var dto = Mapper.Map<List<Colaboradores>, List<ColaboradoresDetallesDto>>(entities);

            return dto;
        }

        public string AlertaFechas(DateTime? FechaCaducidad, int DiasVencimiento)
        {
            if (FechaCaducidad.HasValue)
            {
                if (FechaCaducidad.Value < DateTime.Now)
                {
                    return "VENCIDO";
                }
                else
                if (FechaCaducidad.Value < DateTime.Now.AddDays(-DiasVencimiento))
                {
                    return "ALERTA";
                }
                else if (FechaCaducidad.Value > DateTime.Now.AddDays(DiasVencimiento))
                {
                    return "OK";
                }

                else
                {

                    return "NO_APLICA";
                }

            }
            else
            {
                return "NO TIENE FECHA DE CADUCIDAD VERIFIQUE";
            }
        }

        public List<RequisitosReporteDto> ObtenerRequisitosAccesoColaborador(InputRequisitosReporteDto input)
        {


            //Todos los Colaboradores//
            var listados = _colaboradorRepository.GetAll()
               .Include(o => o.GrupoPersonal)
               .Where(c => c.estado == "ACTIVO").ToList();
            var dtos = Mapper.Map<List<Colaboradores>, List<ColaboradoresDetallesDto>>(listados);



            //OBTENGO TODOS LOS  REQUISITOS GRUPO PERSONAL DE ACCESO
            var query = _requisitoColaboradoRepository.GetAll()
                .Include(o => o.Requisitos)
                // .Where(o => o.Accion.codigo =="ACC")
                // .Where(o => o.rolId == input.AccionId) //CATALOGO ACCION ACCESOS
                .Where(o => o.vigente);


            if (input.AccionId > 0)
            {
                query = query.Where(o => o.rolId == input.AccionId); //CATALOGO ACCION ACCESOS
            }


            var gruposPersonales = query.ToList();

            if (!input.Obligatorios)
            {
                gruposPersonales = (from g in gruposPersonales
                                    where g.obligatorio == false
                                    select g).ToList();
            }

            var listado = new List<RequisitosReporteDto>();



            foreach (var grupo in gruposPersonales)
            {
                if (input.Colaboradores != null && input.Colaboradores.Count > 0)
                {
                    var colaboradores = (from c in input.Colaboradores
                                         where c.GrupoPersonalId == grupo.tipo_usuarioId  //tipo_usuario_id=catalogo_grupo_personalId
                                         select c).ToList();


                    foreach (var c in colaboradores)
                    {
                        var ListadocolaboradorRequisito = _colaboradorRequisitoRepository
                       .GetAll()
                       .Where(o => o.vigente)
                       .Where(o => o.ColaboradoresId == c.Id)
                       .Where(o => o.Colaboradores.catalogo_grupo_personal_id == grupo.tipo_usuarioId)
                      .Where(o => o.RequisitosId == grupo.RequisitosId).ToList();

                        if (input.DepartamentoId > 0)
                        {
                            ListadocolaboradorRequisito = (from l in ListadocolaboradorRequisito
                                                           where l.Colaboradores.catalogo_sector_id == input.DepartamentoId
                                                           select l).ToList();
                        }


                        if (ListadocolaboradorRequisito != null)
                        {
                            foreach (var colaboradorRequisito in ListadocolaboradorRequisito)
                            {

                                var dto = new RequisitosReporteDto()
                                {
                                    Id = colaboradorRequisito.Id,
                                    NombresCompletos = c.NombresApellidos,
                                    Identificacion = c.Identificacion,
                                    Departamento = c.Departamento,
                                    Codigo = colaboradorRequisito.Requisitos.codigo,
                                    Requisito = colaboradorRequisito.Requisitos.nombre,
                                    TieneCaducidad = colaboradorRequisito.Requisitos.GetAplicaCaducidad(),
                                    Obligatorio = grupo.GetObligatorio(),
                                    Cumple = colaboradorRequisito.GetCumple(),
                                    FechaRegistro = colaboradorRequisito.fecha_emision,
                                    FechaCaducidad = colaboradorRequisito.fecha_caducidad,
                                    Estado = this.AlertaFechas(colaboradorRequisito.fecha_caducidad, input.DiasVencimiento),
                                    fecha_registro = colaboradorRequisito.fecha_emision.HasValue ? colaboradorRequisito.fecha_emision.GetValueOrDefault().ToShortDateString() : "",
                                    fecha_caducidad = colaboradorRequisito.fecha_caducidad.HasValue ? colaboradorRequisito.fecha_caducidad.GetValueOrDefault().ToShortDateString() : "",

                                };

                                listado.Add(dto);
                            }
                        }


                    }

                }
                else
                {


                    var colaboradores = (from c in dtos
                                         where c.GrupoPersonalId == grupo.tipo_usuarioId  //tipo_usuario_id=catalogo_grupo_personalId
                                         select c).ToList();


                    foreach (var c in colaboradores)
                    {
                        var ListadocolaboradorRequisito = _colaboradorRequisitoRepository
                       .GetAll()
                       .Where(o => o.vigente)
                       .Where(o => o.ColaboradoresId == c.Id)
                       .Where(o => o.Colaboradores.catalogo_grupo_personal_id == grupo.tipo_usuarioId)
                       .Where(o => o.RequisitosId == grupo.RequisitosId).ToList();

                        if (input.DepartamentoId > 0)
                        {
                            ListadocolaboradorRequisito = (from l in ListadocolaboradorRequisito
                                                           where l.Colaboradores.catalogo_sector_id == input.DepartamentoId
                                                           select l).ToList();
                        }


                        if (ListadocolaboradorRequisito != null)
                        {
                            foreach (var colaboradorRequisito in ListadocolaboradorRequisito)
                            {

                                var dto = new RequisitosReporteDto()
                                {
                                    Id = colaboradorRequisito.Id,
                                    NombresCompletos = c.NombresApellidos,
                                    Identificacion = c.Identificacion,
                                    Departamento = c.Departamento,
                                    Codigo = colaboradorRequisito.Requisitos.codigo,
                                    Requisito = colaboradorRequisito.Requisitos.nombre,
                                    TieneCaducidad = colaboradorRequisito.Requisitos.GetAplicaCaducidad(),
                                    Obligatorio = grupo.GetObligatorio(),
                                    Cumple = colaboradorRequisito.GetCumple(),
                                    FechaRegistro = colaboradorRequisito.fecha_emision,
                                    FechaCaducidad = colaboradorRequisito.fecha_caducidad,
                                    Estado = this.AlertaFechas(colaboradorRequisito.fecha_caducidad, input.DiasVencimiento),
                                    fecha_registro = colaboradorRequisito.fecha_emision.HasValue ? colaboradorRequisito.fecha_emision.GetValueOrDefault().ToShortDateString() : "",
                                    fecha_caducidad = colaboradorRequisito.fecha_caducidad.HasValue ? colaboradorRequisito.fecha_caducidad.GetValueOrDefault().ToShortDateString() : "",

                                };

                                listado.Add(dto);
                            }
                        }


                    }

                }
            }

            if (input.Vencidos)
            {
                listado = (from l in listado
                           where l.Estado == "VENCIDO"
                           select l).ToList();
            }


            return listado;


        }

        #endregion

        #region Asignaciones Requisitos Usuario


        public List<ModelAsiganciones> ListaAsignados(int colaboradorId)
        {
            var asignaciones = new List<ModelAsiganciones>();
            var query = _colaboradorResponsabilidadRepository.GetAllIncluding(c => c.Colaboradores)
                                                             .Where(c => c.colaborador_id == colaboradorId).ToList();
            var responsableRequisitos = _catalogoService.APIObtenerCatalogos(CatalogosCodigos.RESPONSABLEREQUISITO);

            foreach (var r in responsableRequisitos)
            {
                var read = (from q in query where q.acceso == "R" where q.catalogo_responsable_id == r.Id select q).FirstOrDefault();
                var write = (from q in query where q.acceso == "M" where q.catalogo_responsable_id == r.Id select q).FirstOrDefault();
                var a = new ModelAsiganciones()
                {
                    catalogoReponsabilidadId = r.Id,
                    nombreResponsabilidad = r.nombre,
                    colaboradorId = colaboradorId,
                    readId = read != null ? read.Id : 0,
                    writeId = write != null ? write.Id : 0,
                    read = read != null ? true : false,
                    write = write != null ? true : false
                };
                asignaciones.Add(a);
            }
            return asignaciones;

        }

        public string DeleteAsigancion(int Id)
        {
            try
            {
                _colaboradorResponsabilidadRepository.Delete(_colaboradorResponsabilidadRepository.Get(Id));
                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public string Asignar(int CatalogoResponsableId, int ColaboradorId, string acceso)
        {
            try
            {
                var n = new ColaboradorResponsabilidad();
                n.catalogo_responsable_id = CatalogoResponsableId;
                n.colaborador_id = ColaboradorId;
                n.acceso = acceso;

                var r = _colaboradorResponsabilidadRepository.InsertAndGetId(n);
                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public List<ModelClassReactUser> SearchUsuario(int ColaboradorId, int CatalogoResponsableId)
        {
            /*var colaboradores = _colaboradorRepository.GetAll().Where(c => c.estado != "INACTIVO").Where(c => c.vigente)
                .Where(c => c.nombres_apellidos.Contains(search) || c.numero_identificacion.StartsWith(search))
                .OrderBy(c => c.primer_apellido)
                  .ToList();*/
            /*var list = (from c in colaboradores
                        select new ModelClassReactUser()
                        {
                            dataKey = c.Id,
                            label = c.numero_identificacion + " - " + c.primer_apellido + " " + c.segundo_apellido + " " + c.nombres,
                            value = c.Id,
                            read = this.SearchValidOption("R", c.Id, CatalogoResponsableId),
                            write = this.SearchValidOption("M", c.Id, CatalogoResponsableId),
                            both = this.SearchValidOption("R", c.Id, CatalogoResponsableId) && this.SearchValidOption("M", c.Id, CatalogoResponsableId) ? true : false

                        }).ToList();
            return list;
            */
            return null;
        }

        public bool SearchValidOption(string acceso, int ColaboradorId, int CatalogoResponsableId)
        {
            var query = _colaboradorResponsabilidadRepository.GetAll().Where(c => c.colaborador_id == ColaboradorId).Where(c => c.catalogo_responsable_id == CatalogoResponsableId).Where(c => c.acceso == acceso).FirstOrDefault();
            if (query != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<ColaboradorResponsabilidad> SearchAsignacionesUsuario(int ColaboradorId)
        {
            var query = _colaboradorResponsabilidadRepository.GetAllIncluding(c => c.Colaboradores)
                                                               .Where(c => c.colaborador_id == ColaboradorId)
                                                               .OrderBy(c => c.Colaboradores.primer_apellido).ToList();
            return query;
        }

        public List<ModelClassReactUser> buscarColaborador(string search)
        {
            var colaboradores = _colaboradorRepository.GetAll().Where(c => c.estado != "INACTIVO").Where(c => c.vigente)
              .Where(c => c.nombres_apellidos.Contains(search) || c.numero_identificacion.StartsWith(search))
              .OrderBy(c => c.primer_apellido)
                .ToList();
            var list = (from c in colaboradores
                        select new ModelClassReactUser()
                        {
                            dataKey = c.Id,
                            label = c.primer_apellido + " " + c.segundo_apellido + " " + c.nombres,
                            value = c.Id,
                            other = c.numero_identificacion

                        }).ToList();
            return list;
        }

        public string ActualizaryCrear(ModelAsiganciones m)
        {
            if (m.readId > 0)
            {
                var e = _colaboradorResponsabilidadRepository.Get(m.readId);
                if (!m.read)
                {
                    _colaboradorResponsabilidadRepository.Delete(e);
                }
            }
            else
            {
                if (m.read)
                {
                    var n = new ColaboradorResponsabilidad()
                    {
                        Id = 0,
                        catalogo_responsable_id = m.catalogoReponsabilidadId,
                        colaborador_id = m.colaboradorId,
                        acceso = "R",
                    };
                    _colaboradorResponsabilidadRepository.Insert(n);
                }

            }
            if (m.writeId > 0)
            {
                var e = _colaboradorResponsabilidadRepository.Get(m.writeId);
                if (!m.write)
                {
                    _colaboradorResponsabilidadRepository.Delete(e);
                }
            }
            else
            {
                if (m.write)
                {
                    var n = new ColaboradorResponsabilidad()
                    {
                        Id = 0,
                        catalogo_responsable_id = m.catalogoReponsabilidadId,
                        colaborador_id = m.colaboradorId,
                        acceso = "M",
                    };
                    _colaboradorResponsabilidadRepository.Insert(n);
                }
            }
            return "OK";

        }
        #endregion
    }
}

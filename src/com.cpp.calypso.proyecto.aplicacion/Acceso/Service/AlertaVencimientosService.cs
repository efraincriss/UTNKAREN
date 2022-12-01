using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Dto;
using com.cpp.calypso.proyecto.aplicacion.Acceso.Interface;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace com.cpp.calypso.proyecto.aplicacion.Acceso.Service
{
    public class  AlertaVencimientosAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradorRequisito, ColaboradorRequisitoDto, PagedAndFilteredResultRequestDto>, IAlertaVencimientosAsyncBaseCrudAppService
    {
        public AlertaVencimientosAsyncBaseCrudAppService(
            IBaseRepository<ColaboradorRequisito> repository
        ) : base(repository)
        {
        }

        public ExcelPackage ExcelCumplimientoIndividual(InputAlertaVencimientoReporteDto input)
        {
            var requisitosQuery = Repository.GetAll().Include(o => o.Colaboradores.Sector).Include(o => o.Requisitos)
                .Where(o => o.Requisitos.caducidad == true).Where(o => o.activo);

            if (input.ApellidosNombres != null)
            {
                requisitosQuery = requisitosQuery.Where(o => o.Colaboradores.nombres_apellidos.Contains(input.ApellidosNombres));
            }

            if (input.Identificacion != null)
            {
                requisitosQuery = requisitosQuery.Where(o => o.Colaboradores.numero_identificacion.Contains(input.Identificacion));
            }

            if (input.RequisitosId != null)
            {
                requisitosQuery = requisitosQuery.Where(o => input.RequisitosId.Contains(o.RequisitosId));
            }

            if (input.DepartamentoId != 0)
            {
                requisitosQuery = requisitosQuery.Where(o => o.Colaboradores.catalogo_sector_id == input.DepartamentoId);
            }

            var requisitos = requisitosQuery.OrderBy(o => o.fecha_caducidad).ToList();
            var datos = new List<ColaboradorRequisito>();
            foreach (var requisito in requisitos)
            {
                if (requisito.fecha_caducidad.HasValue)
                {
                    var fechaLimite = DateTime.Now.AddDays(input.DiasVencimiento);
                    if (requisito.fecha_caducidad.Value <= fechaLimite)
                    {
                        datos.Add(requisito);
                    }
                }
            }



            ExcelPackage excel = new ExcelPackage();
            var hoja = excel.Workbook.Worksheets.Add("Lista de Vencimientos");
            hoja.DefaultRowHeight = 16;
            hoja.View.ZoomScale = 90;

            // CABECERA
            hoja.Row(2).Height = 50;
            hoja.Cells["B2:H2"].Merge = true;
            hoja.Cells["B2:H2"].Value = "REPORTE DE PROXIMOS VENCIMIENTOS";
            hoja.Cells["B2:H2"].Style.WrapText = true;
            hoja.Cells["B2:H2"].Style.Font.Size = 14;
            hoja.Cells["B2:H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B2:H2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B2:H2"].Style.Font.Bold = true;
            hoja.Cells["B2:H2"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells["B2:H2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["B2:H2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            hoja.Cells["B2:H2"].Style.Font.Color.SetColor(Color.White);

            hoja.Row(3).Height = 16;
            hoja.Cells["B3:H3"].Merge = true;
            hoja.Cells["B3:H3"].Value = "Vencimientos Desde: " + DateTime.Now.ToShortDateString();
            hoja.Cells["B3:H3"].Style.WrapText = true;
            hoja.Cells["B3:H3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B3:H3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B3:H3"].Style.Font.Bold = true;
            hoja.Row(4).Height = 16;
            hoja.Cells["B4:H4"].Style.WrapText = true;
            hoja.Cells["B4:H4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B4:H4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B4:H4"].Style.Font.Bold = true;
            hoja.Cells["B4:H4"].Merge = true;
            hoja.Cells["B4:H4"].Value = "Vencimientos Hasta: " + DateTime.Now.AddDays(input.DiasVencimiento).ToShortDateString();
            hoja.Cells[3, 2, 5, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[3, 2, 5, 8].Style.Fill.BackgroundColor.SetColor(Color.White);


            // TABLA
            hoja.Row(6).Style.Font.Size = 10;
            hoja.Row(6).Style.WrapText = true;

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

            hoja.Cells["F6"].Value = "FECHA REGISTRO";
            hoja.Cells["F6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["F6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["F6"].Style.Font.Bold = true;
            hoja.Cells["F6"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["G6"].Value = "FECHA VENCIMIENTO";
            hoja.Cells["G6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["G6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["G6"].Style.Font.Bold = true;
            hoja.Cells["G6"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["H6"].Value = "DIAS PARA VENCIMIENTO";
            hoja.Cells["H6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["H6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["H6"].Style.Font.Bold = true;
            hoja.Cells["H6"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Row(6).Height = 27;
            hoja.Cells[6, 2, 6, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[6, 2, 6, 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            hoja.Cells[6, 2, 6, 8].Style.Font.Color.SetColor(Color.White);

            // LOGO
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
            hoja.Column(3).Width = 14;
            hoja.Column(4).Width = 38;
            hoja.Column(5).Width = 38;
            hoja.Column(6).Width = 13;
            hoja.Column(7).Width = 13;
            hoja.Column(8).Width = 13;

            if (datos.Count > 0)
            {
                int row = 7;
                foreach (var item in datos)
                {
                    hoja.Row(row).Height = 15;
                    hoja.Row(row).Style.Font.Size = 9;
                    var now = DateTime.Now;
                    var days = Convert.ToInt32((item.fecha_caducidad.Value.Date - now.Date).TotalDays);
                    var departamento = item.Colaboradores.Sector != null ? item.Colaboradores.Sector.nombre : "" ;

                    hoja.Cells["B" + row].Value = departamento;
                    hoja.Cells["C" + row].Value = item.Colaboradores.numero_identificacion;
                    hoja.Cells["D" + row].Value = item.Colaboradores.nombres_apellidos;
                    hoja.Cells["E" + row].Value = item.Requisitos.nombre;
                    hoja.Cells["F" + row].Value = item.fecha_emision;
                    hoja.Cells["G" + row].Value = item.fecha_caducidad;
                    hoja.Cells["H" + row].Value = days;

                    hoja.Cells["B" + row].Style.WrapText = true;
                    hoja.Cells["C" + row].Style.WrapText = true;
                    hoja.Cells["D" + row].Style.WrapText = true;
                    hoja.Cells["E" + row].Style.WrapText = true;
                    hoja.Cells["F" + row].Style.WrapText = true;
                    hoja.Cells["G" + row].Style.WrapText = true;
                    hoja.Cells["H" + row].Style.WrapText = true;

                    hoja.Cells["B" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["C" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["D" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["E" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["F" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["G" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["H" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    hoja.Cells["B" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["C" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["D" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["E" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["F" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["G" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["H" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center; 


                    hoja.Cells["F" + row].Style.Numberformat.Format = "DD/MM/YYYY";
                    hoja.Cells["G" + row].Style.Numberformat.Format = "DD/MM/YYYY";

                    row++;
                }
            }

 

            //FORMATO A UNA PAGINA
            hoja.View.PageBreakView = true;
            hoja.PrinterSettings.PrintArea = hoja.Cells[2, 2, hoja.Dimension.End.Row, hoja.Dimension.End.Column];
            hoja.PrinterSettings.FitToPage = true;


            return excel;
        }
    }
}

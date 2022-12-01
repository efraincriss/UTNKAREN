using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Dto;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Interface;
using com.cpp.calypso.proyecto.aplicacion.Transporte.Models;
using com.cpp.calypso.proyecto.dominio.Transporte;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using com.cpp.calypso.proyecto.aplicacion.Dto;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.Constantes;

namespace com.cpp.calypso.proyecto.aplicacion.Transporte.Service
{
    public class RutaAsyncBaseCrudAppService : AsyncBaseCrudAppService<Ruta, RutaDto, PagedAndFilteredResultRequestDto>, IRutaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<Lugar> _lugarrepository;
        private readonly IBaseRepository<RutaHorario> _horarioRepository;
        private readonly IBaseRepository<RutaHorarioVehiculo> _horariovechiculoRepository;
        private readonly IBaseRepository<RutaParada> _paradarepository;
        private readonly IBaseRepository<Vehiculo> _vehiculoRepository;


        private readonly IBaseRepository<ConsumoTransporte> _consumotransporteRepository;
        private readonly IBaseRepository<OperacionDiaria> _operacionDiariaRepository;
        private readonly IBaseRepository<OperacionDiariaRuta> _operacionRutaRepository;

        private readonly IBaseRepository<Usuario> _usuarioRepository;
        private readonly IBaseRepository<dominio.Proveedor.Proveedor> _proveedorrepository;
        private readonly IBaseRepository<ServicioProveedor> _servicioProveedorRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;

        private readonly IBaseRepository<SolicitudVianda> _solicitudVianda;
        private readonly IBaseRepository<DistribucionVianda> _distribucionVianda;
        private readonly IBaseRepository<DetalleDistribucion> _detalledistribucionVianda;
        private readonly IBaseRepository<Colaboradores> _colaboradores;

        public RutaAsyncBaseCrudAppService(
                        IBaseRepository<Ruta> repository,
                        IBaseRepository<Lugar> lugarrepository,
                        IBaseRepository<RutaHorario> horarioRepository,
                        IBaseRepository<RutaParada> paradarepository,
                        IBaseRepository<Vehiculo> vehiculoRepository,
                        IBaseRepository<RutaHorarioVehiculo> horariovechiculoRepository,
                        IBaseRepository<ConsumoTransporte> consumotransporteRepository,
                        IBaseRepository<OperacionDiaria> operacionDiariaRepository,
                        IBaseRepository<OperacionDiariaRuta> operacionRutaRepository,
                        IBaseRepository<Usuario> usuarioRepository,
                        IBaseRepository<dominio.Proveedor.Proveedor> proveedorrepository,
                        IBaseRepository<ServicioProveedor> servicioProveedorRepository,
                        IBaseRepository<Catalogo> catalogoRepository,
                        IBaseRepository<SolicitudVianda> solicitudVianda,
                        IBaseRepository<DistribucionVianda> distribucionVianda,
                        IBaseRepository<DetalleDistribucion> detalledistribucionVianda,
                        IBaseRepository<Colaboradores> colaboradores
            ) : base(repository)
        {
            _lugarrepository = lugarrepository;
            _horarioRepository = horarioRepository;
            _paradarepository = paradarepository;
            _vehiculoRepository = vehiculoRepository;
            _horariovechiculoRepository = horariovechiculoRepository;
            _consumotransporteRepository = consumotransporteRepository;
            _operacionDiariaRepository = operacionDiariaRepository;
            _operacionRutaRepository = operacionRutaRepository;
            _usuarioRepository = usuarioRepository;
            _proveedorrepository = proveedorrepository;
            _servicioProveedorRepository = servicioProveedorRepository;
            _catalogoRepository = catalogoRepository;
            _solicitudVianda = solicitudVianda;
            _distribucionVianda = distribucionVianda;
            _colaboradores = colaboradores;
            _detalledistribucionVianda = detalledistribucionVianda;
        }

        public int EditarRuta(Ruta ruta)
        {
            var E = Repository.Get(ruta.Id);
            E.Nombre = ruta.Nombre;
            E.Descripcion = ruta.Descripcion;
            E.OrigenId = ruta.OrigenId;
            E.DestinoId = ruta.DestinoId;
            E.Distancia = ruta.Distancia;
            E.Duracion = ruta.Duracion;
            E.Sector = ruta.Sector;
            var Chofer = Repository.Update(E);
            return E.Id;
        }

        public int EliminarRuta(int id)
        {
            var paradas = _paradarepository.GetAll().Where(c => c.RutaId == id).ToList().Count;
            var horarios = _horarioRepository.GetAll().Where(c => c.RutaId == id).ToList().Count;

            if (paradas == 0 && horarios == 0)
            {
                var ruta = Repository.Get(id);

                Repository.Delete(ruta);
                return ruta.Id;
            }
            else
            {
                return -1;
            }
        }

        public ExcelPackage ExcelPersonasTransportadas(InputReporteTransporte input)
        {

            var proveedor = _proveedorrepository.Get(input.ProveedorId.Value);
            ExcelPackage excel = new ExcelPackage();
            var hoja = excel.Workbook.Worksheets.Add("Personas Transportadas");
            hoja.DefaultRowHeight = 16;

            hoja.View.ZoomScale = 70;

            // CABECERA
            hoja.Row(2).Height = 50;
            hoja.Cells["B2:N2"].Merge = true;
            hoja.Cells["B2:N2"].Value = "Reporte Diario de Personas Transportadas";
            hoja.Cells["B2:N2"].Style.WrapText = true;
            hoja.Cells["B2:N2"].Style.Font.Size = 14;
            hoja.Cells["B2:N2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B2:N2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B2:N2"].Style.Font.Bold = true;
            hoja.Cells["B2:N2"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells["B2:N2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["B2:N2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            hoja.Cells["B2:N2"].Style.Font.Color.SetColor(Color.White);

            hoja.Row(3).Height = 30;
            hoja.Cells["B3:N3"].Merge = true;
            hoja.Cells["B3:N3"].Value = "AL: " + DateTime.Now.ToShortDateString();
            hoja.Cells["B3:N3"].Style.WrapText = true;
            hoja.Cells["B3:N3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B3:N3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B3:N3"].Style.Font.Bold = true;
            hoja.Cells["B3:N3"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells["B3:N3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["B3:N3"].Style.Fill.BackgroundColor.SetColor(Color.White);


            hoja.Cells["C5"].Value = "Fecha Desde: ";
            hoja.Cells["C5"].Style.WrapText = true;
            hoja.Cells["C5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C5"].Style.Font.Bold = true;

            if (input.FechaDesde.HasValue)
            {
                hoja.Cells["D5"].Value = input.FechaDesde.Value;
                hoja.Cells["D5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                hoja.Cells["D5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["D5"].Style.Numberformat.Format = "DD/MM/YYYY";
            }

            hoja.Cells["C6"].Value = "Fecha Hasta: ";
            hoja.Cells["C6"].Style.WrapText = true;
            hoja.Cells["C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C6"].Style.Font.Bold = true;

            if (input.FechaHasta.HasValue)
            {
                hoja.Cells["D6"].Value = input.FechaHasta.Value;
                hoja.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                hoja.Cells["D6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["D6"].Style.Numberformat.Format = "DD/MM/YYYY";
            }
            hoja.Cells["C7"].Value = "Proveedor: ";
            hoja.Cells["C7"].Style.WrapText = true;
            hoja.Cells["C7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C7"].Style.Font.Bold = true;

            if (proveedor != null && input.ProveedorId.Value > 0)
            {
                hoja.Cells["D7"].Value = proveedor.razon_social;
            }


            hoja.Cells[4, 2, 10, 14].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells[4, 2, 10, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[4, 2, 10, 14].Style.Fill.BackgroundColor.SetColor(Color.White);

            //TABLA

            hoja.Cells["B11"].Value = "N";
            hoja.Cells["B11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B11"].Style.Font.Bold = true;
            hoja.Cells["B11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["C11"].Value = "FECHA";
            hoja.Cells["C11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["C11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C11"].Style.Font.Bold = true;
            hoja.Cells["C11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["D11"].Value = "RUTA";
            hoja.Cells["D11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["D11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["D11"].Style.Font.Bold = true;
            hoja.Cells["D11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["E11"].Value = "SECTOR";
            hoja.Cells["E11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["E11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["E11"].Style.Font.Bold = true;
            hoja.Cells["E11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["F11"].Value = "ORIGEN";
            hoja.Cells["F11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["F11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["F11"].Style.Font.Bold = true;
            hoja.Cells["F11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["G11"].Value = "DESTINO";
            hoja.Cells["G11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["G11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["G11"].Style.Font.Bold = true;
            hoja.Cells["G11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["H11"].Value = "HORA INICIO";
            hoja.Cells["H11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["H11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["H11"].Style.Font.Bold = true;
            hoja.Cells["H11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["I11"].Value = "HORA FIN";
            hoja.Cells["I11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["I11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["I11"].Style.Font.Bold = true;
            hoja.Cells["I11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["J11"].Value = "PROVEEDOR";
            hoja.Cells["J11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["J11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["J11"].Style.Font.Bold = true;
            hoja.Cells["J11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["K11"].Value = "CONDUCTOR";
            hoja.Cells["K11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["K11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["K11"].Style.Font.Bold = true;
            hoja.Cells["K11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            var celda = "L11";
            hoja.Cells[celda].Value = "CODIGO INVENTARIO VEHICULO";
            hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celda].Style.Font.Bold = true;
            hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            celda = "M11";
            hoja.Cells[celda].Value = "TOTAL PERSONAS TRANSPORTADAS";
            hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celda].Style.Font.Bold = true;
            hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            celda = "N11";
            hoja.Cells[celda].Value = "TOTAL CAPACIDAD";
            hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celda].Style.Font.Bold = true;
            hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            hoja.Cells[11, 2, 11, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[11, 2, 11, 14].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            hoja.Cells[11, 2, 11, 14].Style.Font.Color.SetColor(Color.White);

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
            hoja.Column(3).Width = 35;
            hoja.Column(4).Width = 35;
            hoja.Column(5).Width = 35;
            hoja.Column(6).Width = 35;
            hoja.Column(7).Width = 35;
            hoja.Column(8).Width = 35;
            hoja.Column(9).Width = 35;
            hoja.Column(10).Width = 35;
            hoja.Column(11).Width = 35;
            hoja.Column(12).Width = 35;
            hoja.Column(13).Width = 35;
            hoja.Column(14).Width = 35;


            //DATOS 
            var query = _operacionRutaRepository.GetAllIncluding(c => c.RutaHorarioVehiculo.RutaHorario.Ruta.Sector,
                c => c.RutaHorarioVehiculo.RutaHorario.Ruta.Origen,
                c => c.RutaHorarioVehiculo.RutaHorario.Ruta.Destino,
                                                                    c => c.RutaHorarioVehiculo.Vehiculo.Proveedor,

                                                                    c => c.OperacionDiaria.Vehiculo.Proveedor).Where(c => !c.IsDeleted)
                                                                    .Where(c => c.FechaInicio.HasValue)
                                                                     .Where(c => c.FechaFin.HasValue)
                                                                    // .Where(c =>c.FechaInicio.Value>= input.FechaDesde.Value   )
                                                                    //.Where(c => c.FechaFin.Value <= input.FechaHasta.Value )
                                                                    .ToList();
            if (!input.check)
            {
                if (input.ProveedorId != null && input.ProveedorId.Value > 0)
                {
                    query = query.Where(c => c.OperacionDiaria.Vehiculo.ProveedorId == input.ProveedorId.Value).ToList();
                }
                if (input.RutaId != null && input.RutaId.Value > 0)
                {
                    query = query.Where(c => c.RutaHorarioVehiculo.RutaHorario.RutaId == input.RutaId.Value).ToList();
                }
            }

            var consumos_transporte = _consumotransporteRepository.GetAll().ToList();
            var datos = (from c in query
                         where c.FechaInicio.Value.Date >= input.FechaDesde.Value.Date
                         where c.FechaFin.Value.Date <= input.FechaHasta.Value.Date
                         select new PersonasTransportadasDto()
                         {
                             Id=c.Id,
                             Fecha = c.FechaInicio.GetValueOrDefault().ToShortDateString() + " - " + c.FechaFin.GetValueOrDefault().ToShortDateString(),
                             Ruta = c.RutaHorarioVehiculo.RutaHorario.Ruta.Nombre,
                             Sector = c.RutaHorarioVehiculo.RutaHorario.Ruta.Sector.nombre,
                             Origen = c.RutaHorarioVehiculo.RutaHorario.Ruta.Origen.Nombre,
                             Destino = c.RutaHorarioVehiculo.RutaHorario.Ruta.Destino.Nombre,
                             HoraInicio = c.FechaInicio.Value.ToShortTimeString(),
                             HoraFinRuta = c.FechaFin.Value.ToShortTimeString(),
                             Proveedor = c.RutaHorarioVehiculo.Vehiculo.Proveedor.razon_social,
                             Conductor = _usuarioRepository.Get(c.OperacionDiaria.ChoferId.Value) != null ? _usuarioRepository.Get(c.OperacionDiaria.ChoferId.Value).NombresCompletos : "No Encontro Chofer en Usuario",
                             CodigoInventarioVehiculo = c.RutaHorarioVehiculo.Vehiculo.CodigoEquipoInventario,
                             TotalPersonasTransportadas = (from t in consumos_transporte where t.OperacionDiariaRutaId == c.Id select t).ToList().Count,
                             TotalCapacidad = c.RutaHorarioVehiculo.Vehiculo.Capacidad.ToString()

                         }).ToList();

            if (datos.Count > 0)
            {
                int row = 12;
                int cont = 1;
                foreach (var item in datos)

                {
                    hoja.Cells["B" + row].Value = cont;
                    hoja.Cells["C" + row].Value = item.Fecha;
                    hoja.Cells["D" + row].Value = item.Ruta;
                    hoja.Cells["E" + row].Value = item.Sector;
                    hoja.Cells["F" + row].Value = item.Origen;
                    hoja.Cells["G" + row].Value = item.Destino;
                    hoja.Cells["H" + row].Value = item.HoraInicio;
                    hoja.Cells["I" + row].Value = item.HoraFinRuta;
                    hoja.Cells["J" + row].Value = item.Proveedor;
                    hoja.Cells["K" + row].Value = item.Conductor;
                    hoja.Cells["L" + row].Value = item.CodigoInventarioVehiculo;
                    hoja.Cells["M" + row].Value = item.TotalPersonasTransportadas;
                    hoja.Cells["N" + row].Value = item.TotalCapacidad;


                    hoja.Cells["B" + row].Style.WrapText = true;
                    hoja.Cells["C" + row].Style.WrapText = true;
                    hoja.Cells["D" + row].Style.WrapText = true;
                    hoja.Cells["E" + row].Style.WrapText = true;
                    hoja.Cells["F" + row].Style.WrapText = true;
                    hoja.Cells["G" + row].Style.WrapText = true;
                    hoja.Cells["H" + row].Style.WrapText = true;
                    hoja.Cells["I" + row].Style.WrapText = true;
                    hoja.Cells["I" + row].Style.Font.Bold = true;
                    hoja.Cells["J" + row].Style.WrapText = true;
                    hoja.Cells["K" + row].Style.WrapText = true;
                    hoja.Cells["L" + row].Style.Font.Bold = true;
                    hoja.Cells["M" + row].Style.WrapText = true;
                    hoja.Cells["N" + row].Style.WrapText = true;


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
                    hoja.Cells["L" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["M" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

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
                    hoja.Cells["L" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["M" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["N" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    hoja.Cells[row, 2, row, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja.Cells[row, 2, row, 14].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(201, 201, 201));

                    hoja.Cells[row, 2, row, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    row++;

                    var consumos_tranporte = _consumotransporteRepository.GetAllIncluding(c => c.Colaborador, c => c.OperacionDiariaRuta)
                        .Where(c=>c.ColaboradorId.HasValue)
                        .Where(c => c.OperacionDiariaRutaId == item.Id).ToList();

                    if (consumos_tranporte.Count > 0) {
                        int filainicio = row;
                        int filafin = row;
                        foreach (var t in consumos_tranporte)
                        {

                            hoja.Cells["D" + row].Value = t.Colaborador.numero_identificacion;
                            hoja.Cells["E" + row].Value = t.Colaborador.nombres_apellidos;
                            hoja.Cells["F" + row].Value = t.FechaEmbarque.HasValue?""+t.FechaEmbarque:"";
                            row++;
                            filafin++;
                        }
                        hoja.Cells[filainicio, 2, filafin, 3].Merge = true;
                        hoja.Cells[filainicio, 2, filafin, 3].Value = "Personas Transportadas";
                        hoja.Cells[filainicio, 2, filafin, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja.Cells[filainicio, 2, filafin, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja.Cells[filainicio, 2, filafin, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja.Cells[filainicio, 2, filafin, 3].Style.Fill.BackgroundColor.SetColor(Color.White);
                        hoja.Cells[filainicio, 2, filafin, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    }
                    row++;

                    cont++;

                }
            }

            //FORMATO A UNA PAGINA
            hoja.View.PageBreakView = true;
            hoja.PrinterSettings.PrintArea = hoja.Cells[2, 2, hoja.Dimension.End.Row, hoja.Dimension.End.Column];
            hoja.PrinterSettings.FitToPage = true;

            //hoja.Cells[2, 11, hoja.Dimension.End.Row, hoja.Dimension.End.Column].AutoFilter = true;


            return excel;
        }

        public ExcelPackage ExcelTrabajosDiarios(InputReporteTransporte input)
        {

            var proveedor = _proveedorrepository.Get(input.ProveedorId.Value);
            var vehiculo = _vehiculoRepository.Get(input.VehiculoId.Value);
            ExcelPackage excel = new ExcelPackage();
            var hoja = excel.Workbook.Worksheets.Add("Trabajos Diarios");
            hoja.DefaultRowHeight = 16;

            hoja.View.ZoomScale = 70;

            // CABECERA
            hoja.Row(2).Height = 50;
            hoja.Cells["B2:K2"].Merge = true;
            hoja.Cells["B2:K2"].Value = "Reporte Diario de Trabajo por Proveedor y Vehículo";
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
            hoja.Cells["B3:K3"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells["B3:K3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["B3:K3"].Style.Fill.BackgroundColor.SetColor(Color.White);


            hoja.Cells["C5"].Value = "Fecha : ";
            hoja.Cells["C5"].Style.WrapText = true;
            hoja.Cells["C5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C5"].Style.Font.Bold = true;

            if (input.Fecha.HasValue)
            {
                hoja.Cells["D5"].Value = input.Fecha.Value;
                hoja.Cells["D5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                hoja.Cells["D5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["D5"].Style.Numberformat.Format = "DD/MM/YYYY";
            }

            hoja.Cells["C6"].Value = "Proveedor: ";
            hoja.Cells["C6"].Style.WrapText = true;
            hoja.Cells["C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C6"].Style.Font.Bold = true;

            if (proveedor != null && input.ProveedorId.Value > 0)
            {
                hoja.Cells["D6"].Value = proveedor.razon_social;
            }
            hoja.Cells["C7"].Value = "Vehículo: ";
            hoja.Cells["C7"].Style.WrapText = true;
            hoja.Cells["C7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C7"].Style.Font.Bold = true;

            if (vehiculo != null && input.VehiculoId.Value > 0)
            {
                hoja.Cells["D7"].Value = vehiculo.CodigoEquipoInventario + " - " + vehiculo.NumeroPlaca + " - " + vehiculo.Marca;
            }

            hoja.Cells[4, 2, 10, 11].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells[4, 2, 10, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[4, 2, 10, 11].Style.Fill.BackgroundColor.SetColor(Color.White);

            //TABLA

            hoja.Cells["B11"].Value = "N";
            hoja.Cells["B11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B11"].Style.Font.Bold = true;
            hoja.Cells["B11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["C11"].Value = "FECHA";
            hoja.Cells["C11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["C11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C11"].Style.Font.Bold = true;
            hoja.Cells["C11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["D11"].Value = "PROVEEDOR";
            hoja.Cells["D11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["D11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["D11"].Style.Font.Bold = true;
            hoja.Cells["D11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["E11"].Value = "CONDUCTOR";
            hoja.Cells["E11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["E11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["E11"].Style.Font.Bold = true;
            hoja.Cells["E11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["F11"].Value = "CODIGO INVENTARIO VEHICULO";
            hoja.Cells["F11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["F11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["F11"].Style.Font.Bold = true;
            hoja.Cells["F11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["G11"].Value = "HORA INICIO";
            hoja.Cells["G11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["G11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["G11"].Style.Font.Bold = true;
            hoja.Cells["G11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["H11"].Value = "HORA FIN";
            hoja.Cells["H11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["H11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["H11"].Style.Font.Bold = true;
            hoja.Cells["H11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["I11"].Value = "KM VEHICULO INICIO";
            hoja.Cells["I11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["I11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["I11"].Style.Font.Bold = true;
            hoja.Cells["I11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["J11"].Value = "KM VEHICULO FIN";
            hoja.Cells["J11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["J11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["J11"].Style.Font.Bold = true;
            hoja.Cells["J11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["K11"].Value = "KM VEHICULO TOTAL";
            hoja.Cells["K11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["K11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["K11"].Style.Font.Bold = true;
            hoja.Cells["K11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells[11, 2, 11, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[11, 2, 11, 11].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            hoja.Cells[11, 2, 11, 11].Style.Font.Color.SetColor(Color.White);

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
            hoja.Column(3).Width = 35;
            hoja.Column(4).Width = 35;
            hoja.Column(5).Width = 35;
            hoja.Column(6).Width = 35;
            hoja.Column(7).Width = 35;
            hoja.Column(8).Width = 35;
            hoja.Column(9).Width = 35;
            hoja.Column(10).Width = 35;
            hoja.Column(11).Width = 35;

            //DATOS 
            var query = _operacionRutaRepository.GetAllIncluding(c => c.RutaHorarioVehiculo.RutaHorario.Ruta.Sector,
                    c => c.RutaHorarioVehiculo.RutaHorario.Ruta.Origen,
                c => c.RutaHorarioVehiculo.RutaHorario.Ruta.Destino,
                                                                    c => c.RutaHorarioVehiculo.Vehiculo.Proveedor,
                                                                    c => c.OperacionDiaria.Vehiculo.Proveedor).Where(c => !c.IsDeleted)

                                                                    .ToList();
            if (!input.check)
            {

                if (input.ProveedorId != null && input.ProveedorId.Value > 0)
                {
                    query = query.Where(c => c.OperacionDiaria.Vehiculo.ProveedorId == input.ProveedorId.Value).ToList();
                }
                if (input.VehiculoId != null && input.VehiculoId.Value > 0)
                {
                    query = query.Where(c => c.RutaHorarioVehiculo.VehiculoId == input.VehiculoId.Value).ToList();
                }
            }

            var datos = (from c in query
                         where c.OperacionDiaria.FechaInicio.HasValue
                         where c.OperacionDiaria.FechaFin.HasValue
                         where c.OperacionDiaria.FechaInicio.Value.ToShortDateString() == input.Fecha.Value.ToShortDateString()
                         where c.OperacionDiaria.FechaFin.Value.ToShortDateString() == input.Fecha.Value.ToShortDateString()
                         select new TrabajoDiarioDto()
                         {
                             Fecha = c.FechaInicio.GetValueOrDefault().ToShortDateString() + " - " + c.FechaFin.GetValueOrDefault().ToShortDateString(),
                             Proveedor = c.RutaHorarioVehiculo.Vehiculo.Proveedor.razon_social,
                             Conductor = _usuarioRepository.Get(c.OperacionDiaria.ChoferId.Value) != null ? _usuarioRepository.Get(c.OperacionDiaria.ChoferId.Value).NombresCompletos : "No Encontro Chofer en Usuario",
                             CodigoInventarioVehiculo = c.RutaHorarioVehiculo.Vehiculo.CodigoEquipoInventario,
                             HoraInicio = c.FechaInicio.Value.ToShortTimeString(),
                             HoraFinRuta = c.FechaFin.Value.ToShortTimeString(),
                             KilometrajeVehiculoInicio = c.OperacionDiaria.KilometrajeInicio.ToString(),
                             KilometrajeVehiculoFin = c.OperacionDiaria.KilometrajeFinal.ToString(),
                             KilometrajeVehiculoTotal = (c.OperacionDiaria.KilometrajeFinal - c.OperacionDiaria.KilometrajeInicio) + ""

                         }).ToList();

            if (datos.Count > 0)
            {
                int row = 12;
                int cont = 1;
                foreach (var item in datos)

                {
                    hoja.Cells["B" + row].Value = cont;
                    hoja.Cells["C" + row].Value = item.Fecha;
                    hoja.Cells["D" + row].Value = item.Proveedor;
                    hoja.Cells["E" + row].Value = item.Conductor;
                    hoja.Cells["F" + row].Value = item.CodigoInventarioVehiculo;
                    hoja.Cells["G" + row].Value = item.HoraInicio;
                    hoja.Cells["H" + row].Value = item.HoraFinRuta;
                    hoja.Cells["I" + row].Value = item.KilometrajeVehiculoInicio;
                    hoja.Cells["J" + row].Value = item.KilometrajeVehiculoFin;
                    hoja.Cells["K" + row].Value = item.KilometrajeVehiculoTotal;

                    hoja.Cells["B" + row].Style.WrapText = true;
                    hoja.Cells["C" + row].Style.WrapText = true;
                    hoja.Cells["D" + row].Style.WrapText = true;
                    hoja.Cells["E" + row].Style.WrapText = true;
                    hoja.Cells["F" + row].Style.WrapText = true;
                    hoja.Cells["G" + row].Style.WrapText = true;
                    hoja.Cells["H" + row].Style.WrapText = true;
                    hoja.Cells["I" + row].Style.WrapText = true;
                    hoja.Cells["I" + row].Style.Font.Bold = true;
                    hoja.Cells["J" + row].Style.WrapText = true;
                    hoja.Cells["K" + row].Style.WrapText = true;

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




                    cont++;
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

        public ExcelPackage ExcelViajes(InputReporteTransporte input)
        {


            var proveedor = _proveedorrepository.Get(input.ProveedorId.Value);
            var vehiculo = _vehiculoRepository.Get(input.VehiculoId.Value);

            ExcelPackage excel = new ExcelPackage();
            var hoja = excel.Workbook.Worksheets.Add("Reporte Diario de Viajes");
            hoja.DefaultRowHeight = 16;

            hoja.View.ZoomScale = 90;

            // CABECERA
            hoja.Row(2).Height = 50;
            hoja.Cells["B2:L2"].Merge = true;
            hoja.Cells["B2:L2"].Value = "Reporte Diario de Viajes por Proveedor/vehículo";
            hoja.Cells["B2:L2"].Style.WrapText = true;
            hoja.Cells["B2:L2"].Style.Font.Size = 14;
            hoja.Cells["B2:L2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B2:L2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B2:L2"].Style.Font.Bold = true;
            hoja.Cells["B2:L2"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells["B2:L2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["B2:L2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            hoja.Cells["B2:L2"].Style.Font.Color.SetColor(Color.White);

            hoja.Row(3).Height = 30;
            hoja.Cells["B3:L3"].Merge = true;
            hoja.Cells["B3:L3"].Value = "AL: " + DateTime.Now.ToShortDateString();
            hoja.Cells["B3:L3"].Style.WrapText = true;
            hoja.Cells["B3:L3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B3:L3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B3:L3"].Style.Font.Bold = true;
            hoja.Cells["B3:L3"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells["B3:L3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["B3:L3"].Style.Fill.BackgroundColor.SetColor(Color.White);


            hoja.Cells["C5"].Value = "Fecha Desde: ";
            hoja.Cells["C5"].Style.WrapText = true;
            hoja.Cells["C5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C5"].Style.Font.Bold = true;

            if (input.FechaDesde.HasValue)
            {
                hoja.Cells["D5"].Value = input.FechaDesde;
                hoja.Cells["D5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                hoja.Cells["D5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["D5"].Style.Numberformat.Format = "DD/MM/YYYY";
            }

            hoja.Cells["C6"].Value = "Fecha Hasta: ";
            hoja.Cells["C6"].Style.WrapText = true;
            hoja.Cells["C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C6"].Style.Font.Bold = true;

            if (input.FechaHasta.HasValue)
            {
                hoja.Cells["D6"].Value = input.FechaHasta;
                hoja.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                hoja.Cells["D6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["D6"].Style.Numberformat.Format = "DD/MM/YYYY";
            }
            hoja.Cells["C7"].Value = "Proveedor: ";
            hoja.Cells["C7"].Style.WrapText = true;
            hoja.Cells["C7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C7"].Style.Font.Bold = true;

            if (proveedor != null && input.ProveedorId.Value > 0)
            {
                hoja.Cells["D7"].Value = proveedor.razon_social;
            }

            hoja.Cells["C8"].Value = "Vehículo: ";
            hoja.Cells["C8"].Style.WrapText = true;
            hoja.Cells["C8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C8"].Style.Font.Bold = true;

            if (vehiculo != null && input.VehiculoId.Value > 0)
            {
                hoja.Cells["D8"].Value = vehiculo.CodigoEquipoInventario + " - " + vehiculo.NumeroPlaca + " - " + vehiculo.Marca;
            }


            hoja.Cells[4, 2, 10, 12].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells[4, 2, 10, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[4, 2, 10, 12].Style.Fill.BackgroundColor.SetColor(Color.White);

            //TABLA

            hoja.Cells["B11"].Value = "Nª";
            hoja.Cells["B11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B11"].Style.Font.Bold = true;
            hoja.Cells["B11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["C11"].Value = "FECHA";
            hoja.Cells["C11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["C11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C11"].Style.Font.Bold = true;
            hoja.Cells["C11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["D11"].Value = "PROVEEDOR";
            hoja.Cells["D11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["D11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["D11"].Style.Font.Bold = true;
            hoja.Cells["D11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["E11"].Value = "CONDUCTOR";
            hoja.Cells["E11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["E11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["E11"].Style.Font.Bold = true;
            hoja.Cells["E11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["F11"].Value = "CÓDIGO INVENTARIO VEHÍCULO";
            hoja.Cells["F11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["F11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["F11"].Style.Font.Bold = true;
            hoja.Cells["F11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["G11"].Value = "RUTA";
            hoja.Cells["G11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["G11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["G11"].Style.Font.Bold = true;
            hoja.Cells["G11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["H11"].Value = "SECTOR";
            hoja.Cells["H11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["H11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["H11"].Style.Font.Bold = true;
            hoja.Cells["H11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["I11"].Value = "ORIGEN";
            hoja.Cells["I11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["I11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["I11"].Style.Font.Bold = true;
            hoja.Cells["I11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["J11"].Value = "DESTINO";
            hoja.Cells["J11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["J11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["J11"].Style.Font.Bold = true;
            hoja.Cells["J11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["K11"].Value = "HORA INICIO ";
            hoja.Cells["K11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["K11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["K11"].Style.Font.Bold = true;
            hoja.Cells["K11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            var celda = "L11";
            hoja.Cells[celda].Value = "HORA FIN RUTA";
            hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celda].Style.Font.Bold = true;
            hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            hoja.Cells[11, 2, 11, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[11, 2, 11, 12].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            hoja.Cells[11, 2, 11, 12].Style.Font.Color.SetColor(Color.White);

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
            hoja.Column(4).Width = 33;
            hoja.Column(5).Width = 33;
            hoja.Column(6).Width = 33;
            hoja.Column(7).Width = 33;
            hoja.Column(8).Width = 33;
            hoja.Column(9).Width = 33;
            hoja.Column(10).Width = 33;
            hoja.Column(11).Width = 33;
            hoja.Column(12).Width = 33;




            //DATOS 

            //DATOS 
            var query = _operacionRutaRepository.GetAllIncluding(c => c.RutaHorarioVehiculo.RutaHorario.Ruta.Sector,
                                                                    c => c.RutaHorarioVehiculo.Vehiculo.Proveedor,
                                                                        c => c.RutaHorarioVehiculo.RutaHorario.Ruta.Origen,
                c => c.RutaHorarioVehiculo.RutaHorario.Ruta.Destino,
                                                                    c => c.OperacionDiaria.Vehiculo.Proveedor).Where(c => !c.IsDeleted)
                                                                              .Where(c => c.OperacionDiaria.FechaInicio.HasValue)
                                                                     .Where(c => c.OperacionDiaria.FechaFin.HasValue)
                                                                    //  .Where(c => c.OperacionDiaria.FechaInicio.Value >= input.FechaDesde.Value )
                                                                    //.Where(c => c.OperacionDiaria.FechaFin.Value <= input.FechaHasta.Value  )
                                                                    .ToList();

            if (!input.check)
            {
                if (input.ProveedorId != null && input.ProveedorId.Value > 0)
                {
                    query = query.Where(c => c.OperacionDiaria.Vehiculo.ProveedorId == input.ProveedorId.Value).ToList();
                }
                if (input.VehiculoId != null && input.VehiculoId.Value > 0)
                {
                    query = query.Where(c => c.RutaHorarioVehiculo.VehiculoId == input.VehiculoId.Value).ToList();
                }
            }



            var datos = (from c in query

                         where c.OperacionDiaria.FechaInicio.Value.Date >= input.FechaDesde.Value.Date
                         where c.OperacionDiaria.FechaFin.Value.Date <= input.FechaHasta.Value.Date
                         select new DiarioViajeVehiculoDto()
                         {
                             Fecha = c.FechaInicio.GetValueOrDefault().ToShortDateString() + " - " + c.FechaFin.GetValueOrDefault().ToShortDateString(),
                             Ruta = c.RutaHorarioVehiculo.RutaHorario.Ruta.Nombre,
                             Sector = c.RutaHorarioVehiculo.RutaHorario.Ruta.Sector.nombre,
                             Origen = c.RutaHorarioVehiculo.RutaHorario.Ruta.Origen.Nombre,
                             Destino = c.RutaHorarioVehiculo.RutaHorario.Ruta.Destino.Nombre,
                             HoraInicio = c.FechaInicio.Value.ToShortTimeString(),
                             HoraFinRuta = c.FechaFin.Value.ToShortTimeString(),
                             Proveedor = c.RutaHorarioVehiculo.Vehiculo.Proveedor.razon_social,
                             Conductor = _usuarioRepository.Get(c.OperacionDiaria.ChoferId.Value) != null ? _usuarioRepository.Get(c.OperacionDiaria.ChoferId.Value).NombresCompletos : "No Encontro Chofer en Usuario",
                             CodigoInventarioVehiculo = c.RutaHorarioVehiculo.Vehiculo.CodigoEquipoInventario,



                         }).ToList();

            if (datos.Count > 0)
            {
                int row = 12;
                int cont = 1;
                foreach (var item in datos)

                {
                    hoja.Cells["B" + row].Value = cont;
                    hoja.Cells["C" + row].Value = item.Fecha;
                    hoja.Cells["D" + row].Value = item.Proveedor;
                    hoja.Cells["E" + row].Value = item.Conductor;
                    hoja.Cells["F" + row].Value = item.CodigoInventarioVehiculo;
                    hoja.Cells["G" + row].Value = item.Ruta;
                    hoja.Cells["H" + row].Value = item.Sector;
                    hoja.Cells["I" + row].Value = item.Origen;
                    hoja.Cells["J" + row].Value = item.Destino;
                    hoja.Cells["K" + row].Value = item.HoraInicio;
                    hoja.Cells["L" + row].Value = item.HoraFinRuta;



                    hoja.Cells["B" + row].Style.WrapText = true;
                    hoja.Cells["C" + row].Style.WrapText = true;
                    hoja.Cells["D" + row].Style.WrapText = true;
                    hoja.Cells["E" + row].Style.WrapText = true;
                    hoja.Cells["F" + row].Style.WrapText = true;
                    hoja.Cells["G" + row].Style.WrapText = true;
                    hoja.Cells["H" + row].Style.WrapText = true;
                    hoja.Cells["I" + row].Style.WrapText = true;
                    hoja.Cells["I" + row].Style.Font.Bold = true;
                    hoja.Cells["J" + row].Style.WrapText = true;
                    hoja.Cells["K" + row].Style.WrapText = true;
                    hoja.Cells["L" + row].Style.Font.Bold = true;



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
                    hoja.Cells["L" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


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
                    hoja.Cells["L" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;




                    cont++;
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

        public bool existecode(string code, int id = 0)
        {
            var X = Repository.GetAll().Where(c => c.Codigo == code).Where(c => c.Id != id).ToList();

            return X.Count > 0 ? true : false;
        }

        public Ruta GetDetalles(int id)
        {
            var ruta = Repository.GetAll().Where(c => c.Id == id).Include(c => c.Origen).Include(c => c.Destino).Include(c => c.Sector).FirstOrDefault();
            return ruta;
        }

        public int IngresarRuta(Ruta ruta)
        {
            if (ruta.OrigenId != ruta.DestinoId)
            {
                ruta.Codigo = this.nextcode();
                var id = Repository.InsertAndGetId(ruta);
                return id;
            }
            else
            {
                return -1;
            }
        }

        public List<Ruta> ListadeRutasporProveedor(int ProveedorId)
        {
            var rutas = _horariovechiculoRepository.GetAll().Where(c => c.Vehiculo.ProveedorId == ProveedorId)
                                                           .Select(c => c.RutaHorario.Ruta).Distinct().ToList();

            return rutas;
        }
        public List<Vehiculo> ListadeVehiculosporProveedor(int ProveedorId)
        {
            var vehiculos = _vehiculoRepository.GetAll().Where(c => c.ProveedorId == ProveedorId).ToList();
            return vehiculos;
        }

        public List<RutaDto> Listar()
        {
            var query = Repository.GetAll().Include(c => c.Origen).Include(c => c.Destino).Include(c => c.Sector).ToList();
            var choferes = (from c in query
                            select new RutaDto()
                            {
                                Id = c.Id,
                                Codigo = c.Codigo,
                                Descripcion = c.Descripcion,
                                Nombre = c.Nombre,
                                NombreDestino = c.Destino.Nombre,
                                NombreOrigen = c.Origen.Nombre,
                                NombreSector = c.Sector.nombre,
                                Distancia = c.Distancia,
                                Duracion = c.Duracion,
                                DestinoId = c.DestinoId,
                                OrigenId = c.OrigenId,
                                SectorId = c.SectorId
                            }).ToList();
            return choferes;

        }

        public List<Lugar> ListarLugares()
        {
            var lugares = _lugarrepository.GetAll().ToList();

            return lugares;
        }
        public string nextcode()
        {
            int sec_number = 1;
            var list_code = Repository.GetAll().Where(c => !c.IsDeleted).Select(c => c.Codigo).ToList();
            if (list_code.Count > 0)
            {
                List<int> numeracion = (from l in list_code
                                        where l.Length == 8
                                        select Convert.ToInt32(l.Substring(3, 5))).ToList();

                if (numeracion.Count > 0)
                {
                    sec_number = numeracion.Max() + 1;
                }


            }
            return "RUT" + String.Format("{0:00000}", sec_number);
        }



        #region Retiro Viandas Trasnportista Reportes Api

        public ExcelPackage ObtenerReporteRetiroViandas(InputRetiroTransportista input)
        {
            var query_distribuciones_vianda = _distribucionVianda.GetAllIncluding(c => c.Proveedor, c => c.conductor_asignado)
                                               .Where(c => c.fecha == input.fecha)
                                               .Where(c => c.tipo_comida_id == input.tipoComidaId)
                                               .ToList();
            /*date without hours */
            input.fecha = input.fecha.Date;

            ExcelPackage excel = new ExcelPackage();

            if (query_distribuciones_vianda.Count == 0)
            {
                ExcelPackage nope = new ExcelPackage();
                var hoja = nope.Workbook.Worksheets.Add("NO EXISTEN REGISTROS");
                hoja.DefaultRowHeight = 18.75;
                hoja.View.ZoomScale = 100;

                //var roles = _usuarioRepository.Get(1).Roles; Verificar

                var catalogo = _catalogoRepository.Get(input.tipoComidaId);
                // CABECERA
                hoja.Cells["A1:H1"].Merge = true;
                hoja.Cells["A1:H1"].Value = $"{input.fecha} - {catalogo.nombre}";
                hoja.Cells["A1:H1"].Style.WrapText = true;
                hoja.Cells["A1:H2"].Style.Font.Size = 14;
                hoja.Cells["A1:H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                hoja.Cells["A1:H2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["A1:H2"].Style.Font.Bold = true;
                hoja.Cells["A2:H2"].Merge = true;
                hoja.Cells["A2:H2"].Value = "NO ENCONTRARON REGISTROS";
                hoja.Cells["A2:H2"].Style.WrapText = true;

                hoja.Column(1).Width = 7.30;
                hoja.Column(2).Width = 23.30;
                hoja.Column(3).Width = 23.30;
                hoja.Column(4).Width = 14.71;
                hoja.Column(5).Width = 21.43;
                hoja.Column(6).Width = 9.57;
                hoja.Column(7).Width = 6.71;
                hoja.Column(8).Width = 5.71;

                // TABLAS

                return nope;
            }
            else
            {
                if (input.check)
                {
                    var query = (from t in query_distribuciones_vianda where t.conductor_asignado_id.HasValue select t.conductor_asignado).Distinct().ToList();
                    var transportistas = (from t in query
                                          select new TransportistasDatos()
                                          {
                                              Id = t.Id,
                                              apellidos = t.primer_apellido + t.segundo_apellido,
                                              nombres = t.nombres,
                                              nombres_completos = t.nombres_apellidos,
                                              nro_identificacion = t.numero_identificacion,
                                              estado = t.estado
                                          }
                                          ).OrderBy(c => c.apellidos).ToList();

                    foreach (var t in transportistas)
                    {
                        var hoja = excel.Workbook.Worksheets.Add(t.nombres_completos);
                        hoja.DefaultRowHeight = 18.75;
                        hoja.View.ZoomScale = 100;

                        //var roles = _usuarioRepository.Get(1).Roles; Verificar

                        var catalogo = _catalogoRepository.Get(input.tipoComidaId);
                        // CABECERA
                        hoja.Cells["A1:H1"].Merge = true;
                        hoja.Cells["A1:H1"].Value = $"{input.fecha} - {catalogo.nombre}";
                        hoja.Cells["A1:H1"].Style.WrapText = true;
                        hoja.Cells["A1:H2"].Style.Font.Size = 14;
                        hoja.Cells["A1:H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja.Cells["A1:H2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        hoja.Cells["A1:H2"].Style.Font.Bold = true;
                        hoja.Cells["A2:H2"].Merge = true;
                        hoja.Cells["A2:H2"].Value = "TRANSPORTISTA: " + t.nombres_completos;
                        hoja.Cells["A2:H2"].Style.WrapText = true;

                        hoja.Column(1).Width = 7.30;
                        hoja.Column(2).Width = 23.30;
                        hoja.Column(3).Width = 23.30;
                        hoja.Column(4).Width = 14.71;
                        hoja.Column(5).Width = 21.43;
                        hoja.Column(6).Width = 9.57;
                        hoja.Column(7).Width = 6.71;
                        hoja.Column(8).Width = 5.71;

                        // TABLAS


                        int initrow = hoja.Dimension.End.Row;
                        var group = query_distribuciones_vianda.Where(c => c.conductor_asignado_id == t.Id)
                            .GroupBy(c => c.ProveedorId).ToList();

                        foreach (var p in group)
                        {
                            foreach (var x in p)
                            {
                                var listado = new List<RetiroTransportistaModel>();
                                var proveedor = _proveedorrepository.Get(x.ProveedorId);

                                var detalles_distribuciones = _detalledistribucionVianda
                                                                        .GetAllIncluding
                                                                          (
                                                                           c => c.SolicitudVianda.solicitante,
                                                                           c => c.SolicitudVianda.disciplina)
                                                                        .Where(c => c.DistribucionViandaId == x.Id)
                                                                        .ToList();
                                var details = (from d in detalles_distribuciones
                                               select new
                           RetiroTransportistaModel()
                                               {
                                                   Viandas = d.total_asignado,
                                                   Solicitante = d.SolicitudVianda.solicitante.nombres_apellidos,
                                                   Disciplina = d.SolicitudVianda.disciplina.nombre,
                                                   Hielo = 1,
                                                   Transportista = t.nombres_completos,
                                                   Locacion = d.SolicitudVianda.locacion.nombre
                                               }).ToList();
                                listado.AddRange(details);


                                initrow = hoja.Dimension.End.Row;

                                var excelTemp = CrearTablaRecivoVianda(excel, hoja, initrow + 2, proveedor.razon_social, listado);
                            }



                        }






                        hoja.View.PageBreakView = true;
                        hoja.PrinterSettings.PrintArea = hoja.Cells[1, 1, hoja.Dimension.End.Row, hoja.Dimension.End.Column];
                        hoja.PrinterSettings.FitToPage = true;
                    }



                    return excel;
                }
                else
                {



                    var hoja = excel.Workbook.Worksheets.Add("Reporte Retiro Transportista");
                    hoja.DefaultRowHeight = 18.75;
                    hoja.View.ZoomScale = 100;

                    //var roles = _usuarioRepository.Get(1).Roles; Verificar


                    var transportista = _colaboradores.Get(input.conductor_asignado_id);
                    var catalogo = _catalogoRepository.Get(input.tipoComidaId);
                    // CABECERA
                    hoja.Cells["A1:H1"].Merge = true;
                    hoja.Cells["A1:H1"].Value = $"{input.fecha} - {catalogo.nombre}";
                    hoja.Cells["A1:H1"].Style.WrapText = true;
                    hoja.Cells["A1:H2"].Style.Font.Size = 14;
                    hoja.Cells["A1:H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["A1:H2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["A1:H2"].Style.Font.Bold = true;
                    hoja.Cells["A2:H2"].Merge = true;
                    hoja.Cells["A2:H2"].Value = "TRANSPORTISTA: " + transportista.nombres_apellidos;
                    hoja.Cells["A2:H2"].Style.WrapText = true;

                    hoja.Column(1).Width = 7.30;
                    hoja.Column(2).Width = 23.30;
                    hoja.Column(3).Width = 23.30;
                    hoja.Column(4).Width = 14.71;
                    hoja.Column(5).Width = 21.43;
                    hoja.Column(6).Width = 9.57;
                    hoja.Column(7).Width = 6.71;
                    hoja.Column(8).Width = 5.71;


                    int initrow = hoja.Dimension.End.Row;
                    var group = query_distribuciones_vianda.Where(c => c.conductor_asignado_id == input.conductor_asignado_id)
                             .GroupBy(c => c.ProveedorId).ToList();

                    foreach (var p in group)
                    {
                        foreach (var x in p)
                        {
                            var listado = new List<RetiroTransportistaModel>();
                            var proveedor = _proveedorrepository.Get(x.ProveedorId);

                            var detalles_distribuciones = _detalledistribucionVianda
                                                                    .GetAllIncluding
                                                                      (
                                                                       c => c.SolicitudVianda.solicitante,
                                                                       c => c.SolicitudVianda.disciplina)
                                                                    .Where(c => c.DistribucionViandaId == x.Id)
                                                                    .ToList();
                            var details = (from d in detalles_distribuciones
                                           select new
                       RetiroTransportistaModel()
                                           {
                                               Viandas = d.total_asignado,
                                               Solicitante = d.SolicitudVianda.solicitante.nombres_apellidos,
                                               Disciplina = d.SolicitudVianda.disciplina.nombre,
                                               Hielo = 1,
                                               Transportista = transportista.nombres_apellidos,
                                               Locacion = d.SolicitudVianda.locacion.nombre
                                           }).ToList();
                            listado.AddRange(details);


                            initrow = hoja.Dimension.End.Row;

                            var niced = CrearTablaRecivoVianda(excel, hoja, initrow + 2, proveedor.razon_social, listado);
                        }



                    }

                    return excel;
                }
            }
        }


        public ExcelPackage CrearTablaRecivoVianda(ExcelPackage excel, ExcelWorksheet hoja, int initRow, string razonSocial, List<RetiroTransportistaModel> listado)
        {
            //var hoja = excel.Workbook.Worksheets[1];
            int count = initRow;


            string row = "B" + count + ":G" + count;
            hoja.Cells[row].Merge = true;
            hoja.Cells[row].Value = "RESTAURANT '" + razonSocial + "' ()";
            hoja.Cells[row].Style.WrapText = true;
            hoja.Cells[row].Style.Font.Size = 18;
            hoja.Cells[row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[row].Style.Font.Bold = true;
            hoja.Row(count).Height = 24;

            count++;
            hoja.Cells[$"B{count}"].Value = "SOLICITANTE";
            hoja.Cells[$"C{count}"].Value = "TRANSPORTISTA";
            hoja.Cells[$"D{count}"].Value = "FASE";
            hoja.Cells[$"E{count}"].Value = "LOCACION";
            hoja.Cells[$"F{count}"].Value = "VIANDAS";
            hoja.Cells[$"G{count}"].Value = "HIELO";
            hoja.Cells[$"B{count}:G{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[$"B{count}:G{count}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[$"B{count}:G{count}"].Style.WrapText = true;
            hoja.Cells[$"B{count}:G{count}"].Style.Font.Bold = true;
            hoja.Cells[$"B{count}:G{count}"].Style.Font.Size = 12;
            hoja.Cells[$"B{count}:G{count}"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells[$"B{count}:F{count}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[$"B{count}:F{count}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(130, 214, 216));
            hoja.Row(count).Height = 19.50;

            count++;
            int indexDatosInit = count;
            foreach (var retiro in listado)
            {
                hoja.Row(count).Height = 18.75;
                hoja.Cells[$"B{count}"].Value = retiro.Solicitante;
                hoja.Cells[$"C{count}"].Value = retiro.Transportista;
                hoja.Cells[$"D{count}"].Value = retiro.Disciplina;
                hoja.Cells[$"E{count}"].Value = retiro.Locacion;
                hoja.Cells[$"F{count}"].Value = retiro.Viandas;
                hoja.Cells[$"G{count}"].Value = retiro.Hielo;
                hoja.Cells[$"B{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                hoja.Cells[$"C{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                hoja.Cells[$"D{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                hoja.Cells[$"E{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                hoja.Cells[$"F{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                hoja.Cells[$"G{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                hoja.Cells[$"F{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                hoja.Cells[$"G{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                hoja.Cells[$"B{count}:G{count}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells[$"B{count}:G{count}"].Style.Font.Size = 11;
                //hoja.Cells[$"B{count}:G{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                count++;
            }

            hoja.Cells[$"E{count}"].Value = "TOTAL: ";
            hoja.Cells[$"E{count}"].Style.Font.Bold = true;
            hoja.Cells[$"E{count}"].Style.Font.Size = 12;
            hoja.Cells[$"E{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[$"F{count}"].Formula = $"=SUM(F{indexDatosInit}:F{count - 1})";
            hoja.Cells[$"F{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[$"G{count}"].Formula = $"=SUM(G{indexDatosInit}:G{count - 1})";
            hoja.Cells[$"G{count}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            hoja.Cells[$"E{count}:G{count}"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells[$"E{count}:G{count}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[$"E{count}:G{count}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[$"F{count}:G{count}"].Style.Font.Color.SetColor(Color.Red);

            hoja.Cells[$"F{indexDatosInit}:G{count}"].Style.Font.Bold = true;

            return excel;
        }



        public List<TransportistasDatos> ObtenerTransportistas()
        {
            var query = _distribucionVianda.GetAllIncluding(c => c.Proveedor)
                                           .Where(c => c.conductor_asignado_id.HasValue).Select(c => c.conductor_asignado).Distinct().ToList();
            var transportistas = (from t in query
                                  select new TransportistasDatos()
                                  {
                                      Id = t.Id,
                                      apellidos = t.primer_apellido + t.segundo_apellido,
                                      nombres = t.nombres,
                                      nombres_completos = t.nombres_apellidos,
                                      nro_identificacion = t.numero_identificacion,
                                      estado = t.estado
                                  }
                                  ).OrderBy(c => c.apellidos).ToList();

            return transportistas;
        }

        public List<CatalogoDto> ObtenerTiposComidaViandas()
        {
            var entities = _catalogoRepository.GetAll()
                .Where(o => o.vigente)
                .Where(o => o.TipoCatalogo.codigo == CatalogosCodigos.CATALOGO_TIPO_COMIDA)
                .Where(o => o.valor_texto == CatalogosCodigos.VALOR_TEXTO_VIANDA)
                .ToList();

            var catalogosDtos = Mapper.Map<List<Catalogo>, List<CatalogoDto>>(entities);

            return catalogosDtos;
        }

        public ExcelPackage ExcelPersonasTransportadasNombres(InputReporteTransporte input)
        {


            var proveedor = _proveedorrepository.Get(input.ProveedorId.Value);
            ExcelPackage excel = new ExcelPackage();
            var hoja = excel.Workbook.Worksheets.Add("Personas Transportadas");
            hoja.DefaultRowHeight = 16;

            hoja.View.ZoomScale = 70;

            // CABECERA
            hoja.Row(2).Height = 50;
            hoja.Cells["B2:N2"].Merge = true;
            hoja.Cells["B2:N2"].Value = "Reporte Diario de Personas Transportadas";
            hoja.Cells["B2:N2"].Style.WrapText = true;
            hoja.Cells["B2:N2"].Style.Font.Size = 14;
            hoja.Cells["B2:N2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B2:N2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B2:N2"].Style.Font.Bold = true;
            hoja.Cells["B2:N2"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells["B2:N2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["B2:N2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            hoja.Cells["B2:N2"].Style.Font.Color.SetColor(Color.White);

            hoja.Row(3).Height = 30;
            hoja.Cells["B3:N3"].Merge = true;
            hoja.Cells["B3:N3"].Value = "AL: " + DateTime.Now.ToShortDateString();
            hoja.Cells["B3:N3"].Style.WrapText = true;
            hoja.Cells["B3:N3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B3:N3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B3:N3"].Style.Font.Bold = true;
            hoja.Cells["B3:N3"].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells["B3:N3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells["B3:N3"].Style.Fill.BackgroundColor.SetColor(Color.White);


            hoja.Cells["C5"].Value = "Fecha Desde: ";
            hoja.Cells["C5"].Style.WrapText = true;
            hoja.Cells["C5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C5"].Style.Font.Bold = true;

            if (input.FechaDesde.HasValue)
            {
                hoja.Cells["D5"].Value = input.FechaDesde.Value;
                hoja.Cells["D5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                hoja.Cells["D5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["D5"].Style.Numberformat.Format = "DD/MM/YYYY";
            }

            hoja.Cells["C6"].Value = "Fecha Hasta: ";
            hoja.Cells["C6"].Style.WrapText = true;
            hoja.Cells["C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C6"].Style.Font.Bold = true;

            if (input.FechaHasta.HasValue)
            {
                hoja.Cells["D6"].Value = input.FechaHasta.Value;
                hoja.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                hoja.Cells["D6"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                hoja.Cells["D6"].Style.Numberformat.Format = "DD/MM/YYYY";
            }
            hoja.Cells["C7"].Value = "Proveedor: ";
            hoja.Cells["C7"].Style.WrapText = true;
            hoja.Cells["C7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["C7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C7"].Style.Font.Bold = true;

            if (proveedor != null && input.ProveedorId.Value > 0)
            {
                hoja.Cells["D7"].Value = proveedor.razon_social;
            }


            hoja.Cells[4, 2, 10, 14].Style.Border.BorderAround(ExcelBorderStyle.Medium);
            hoja.Cells[4, 2, 10, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[4, 2, 10, 14].Style.Fill.BackgroundColor.SetColor(Color.White);

            //TABLA

            hoja.Cells["B11"].Value = "N";
            hoja.Cells["B11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["B11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["B11"].Style.Font.Bold = true;
            hoja.Cells["B11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["C11"].Value = "FECHA";
            hoja.Cells["C11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["C11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["C11"].Style.Font.Bold = true;
            hoja.Cells["C11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["D11"].Value = "RUTA";
            hoja.Cells["D11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["D11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["D11"].Style.Font.Bold = true;
            hoja.Cells["D11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["E11"].Value = "SECTOR";
            hoja.Cells["E11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["E11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["E11"].Style.Font.Bold = true;
            hoja.Cells["E11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["F11"].Value = "ORIGEN";
            hoja.Cells["F11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["F11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["F11"].Style.Font.Bold = true;
            hoja.Cells["F11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["G11"].Value = "DESTINO";
            hoja.Cells["G11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["G11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["G11"].Style.Font.Bold = true;
            hoja.Cells["G11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["H11"].Value = "HORA INICIO";
            hoja.Cells["H11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            hoja.Cells["H11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["H11"].Style.Font.Bold = true;
            hoja.Cells["H11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["I11"].Value = "HORA FIN";
            hoja.Cells["I11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["I11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["I11"].Style.Font.Bold = true;
            hoja.Cells["I11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["J11"].Value = "PROVEEDOR";
            hoja.Cells["J11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["J11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["J11"].Style.Font.Bold = true;
            hoja.Cells["J11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            hoja.Cells["K11"].Value = "CONDUCTOR";
            hoja.Cells["K11"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells["K11"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells["K11"].Style.Font.Bold = true;
            hoja.Cells["K11"].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            var celda = "L11";
            hoja.Cells[celda].Value = "CODIGO INVENTARIO VEHICULO";
            hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celda].Style.Font.Bold = true;
            hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);

            celda = "M11";
            hoja.Cells[celda].Value = "TOTAL PERSONAS TRANSPORTADAS";
            hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celda].Style.Font.Bold = true;
            hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            celda = "N11";
            hoja.Cells[celda].Value = "TOTAL CAPACIDAD";
            hoja.Cells[celda].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            hoja.Cells[celda].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            hoja.Cells[celda].Style.Font.Bold = true;
            hoja.Cells[celda].Style.Border.BorderAround(ExcelBorderStyle.Medium);


            hoja.Cells[11, 2, 11, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
            hoja.Cells[11, 2, 11, 14].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 103, 159));
            hoja.Cells[11, 2, 11, 14].Style.Font.Color.SetColor(Color.White);

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
            hoja.Column(3).Width = 35;
            hoja.Column(4).Width = 35;
            hoja.Column(5).Width = 35;
            hoja.Column(6).Width = 35;
            hoja.Column(7).Width = 35;
            hoja.Column(8).Width = 35;
            hoja.Column(9).Width = 35;
            hoja.Column(10).Width = 35;
            hoja.Column(11).Width = 35;
            hoja.Column(12).Width = 35;
            hoja.Column(13).Width = 35;
            hoja.Column(14).Width = 35;


            //DATOS 
            var query = _operacionRutaRepository.GetAllIncluding(c => c.RutaHorarioVehiculo.RutaHorario.Ruta.Sector,
                c => c.RutaHorarioVehiculo.RutaHorario.Ruta.Origen,
                c => c.RutaHorarioVehiculo.RutaHorario.Ruta.Destino,
                                                                    c => c.RutaHorarioVehiculo.Vehiculo.Proveedor,

                                                                    c => c.OperacionDiaria).Where(c => !c.IsDeleted)
                                                                    .Where(c => c.FechaInicio.HasValue)
                                                                     .Where(c => c.FechaFin.HasValue)
                                                                    // .Where(c =>c.FechaInicio.Value>= input.FechaDesde.Value   )
                                                                    //.Where(c => c.FechaFin.Value <= input.FechaHasta.Value )
                                                                    .ToList();
            if (!input.check)
            {
                if (input.ProveedorId != null && input.ProveedorId.Value > 0)
                {
                    query = query.Where(c => c.OperacionDiaria.Vehiculo.ProveedorId == input.ProveedorId.Value).ToList();
                }
                if (input.RutaId != null && input.RutaId.Value > 0)
                {
                    query = query.Where(c => c.RutaHorarioVehiculo.RutaHorario.RutaId == input.RutaId.Value).ToList();
                }
            }

            var consumos_transporte = _consumotransporteRepository.GetAll().ToList();
            var datos = (from c in query
                         where c.FechaInicio.Value.Date >= input.FechaDesde.Value.Date
                         where c.FechaFin.Value.Date <= input.FechaHasta.Value.Date
                         select new PersonasTransportadasDto()
                         {
                             Fecha = c.FechaInicio.GetValueOrDefault().ToShortDateString() + " - " + c.FechaFin.GetValueOrDefault().ToShortDateString(),
                             Ruta = c.RutaHorarioVehiculo.RutaHorario.Ruta.Nombre,
                             Sector = c.RutaHorarioVehiculo.RutaHorario.Ruta.Sector.nombre,
                             Origen = c.RutaHorarioVehiculo.RutaHorario.Ruta.Origen.Nombre,
                             Destino = c.RutaHorarioVehiculo.RutaHorario.Ruta.Destino.Nombre,
                             HoraInicio = c.FechaInicio.Value.ToShortTimeString(),
                             HoraFinRuta = c.FechaFin.Value.ToShortTimeString(),
                             Proveedor = c.RutaHorarioVehiculo.Vehiculo.Proveedor.razon_social,
                             Conductor = _usuarioRepository.Get(c.OperacionDiaria.ChoferId.Value) != null ? _usuarioRepository.Get(c.OperacionDiaria.ChoferId.Value).NombresCompletos : "No Encontro Chofer en Usuario",
                             CodigoInventarioVehiculo = c.RutaHorarioVehiculo.Vehiculo.CodigoEquipoInventario,
                             TotalPersonasTransportadas = (from t in consumos_transporte where t.OperacionDiariaRutaId == c.Id select t).ToList().Count,
                             TotalCapacidad = c.RutaHorarioVehiculo.Vehiculo.Capacidad.ToString()

                         }).ToList();

            if (datos.Count > 0)
            {
                int row = 12;
                int cont = 1;
                foreach (var item in datos)

                {
                    hoja.Cells["B" + row].Value = cont;
                    hoja.Cells["C" + row].Value = item.Fecha;
                    hoja.Cells["D" + row].Value = item.Ruta;
                    hoja.Cells["E" + row].Value = item.Sector;
                    hoja.Cells["F" + row].Value = item.Origen;
                    hoja.Cells["G" + row].Value = item.Destino;
                    hoja.Cells["H" + row].Value = item.HoraInicio;
                    hoja.Cells["I" + row].Value = item.HoraFinRuta;
                    hoja.Cells["J" + row].Value = item.Proveedor;
                    hoja.Cells["K" + row].Value = item.Conductor;
                    hoja.Cells["L" + row].Value = item.CodigoInventarioVehiculo;
                    hoja.Cells["M" + row].Value = item.TotalPersonasTransportadas;
                    hoja.Cells["N" + row].Value = item.TotalCapacidad;


                    hoja.Cells["B" + row].Style.WrapText = true;
                    hoja.Cells["C" + row].Style.WrapText = true;
                    hoja.Cells["D" + row].Style.WrapText = true;
                    hoja.Cells["E" + row].Style.WrapText = true;
                    hoja.Cells["F" + row].Style.WrapText = true;
                    hoja.Cells["G" + row].Style.WrapText = true;
                    hoja.Cells["H" + row].Style.WrapText = true;
                    hoja.Cells["I" + row].Style.WrapText = true;
                    hoja.Cells["I" + row].Style.Font.Bold = true;
                    hoja.Cells["J" + row].Style.WrapText = true;
                    hoja.Cells["K" + row].Style.WrapText = true;
                    hoja.Cells["L" + row].Style.Font.Bold = true;
                    hoja.Cells["M" + row].Style.WrapText = true;
                    hoja.Cells["N" + row].Style.WrapText = true;


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
                    hoja.Cells["L" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja.Cells["M" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

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
                    hoja.Cells["L" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["M" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja.Cells["N" + row].Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                    cont++;

                    row++;

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

        #endregion

    }
}

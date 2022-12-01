using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Dto;
using com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Interfaces;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.RecursosHumanos;
using Castle.Core.Internal;
using LinqToExcel.Extensions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data.SqlClient;
using AutoMapper;
using com.cpp.calypso.comun.entityframework;
using com.cpp.calypso.proyecto.dominio.Constantes;
using Newtonsoft.Json;

namespace com.cpp.calypso.proyecto.aplicacion.RecursosHumanos.Services
{
    public class ActualizacionSueldoAsyncBaseCrudAppService : AsyncBaseCrudAppService<ActualizacionSueldo, ActualizacionSueldoDto, PagedAndFilteredResultRequestDto>, IActualizacionSueldoAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<CategoriasEncargado> _categoriaEncargadoRepository;
        private readonly IBaseRepository<DetalleActualizacionSueldo> _detalleActualizacionSueldoRepository;
        private readonly IBaseRepository<Catalogo> _catalogoRepository;
        private readonly IBaseRepository<AdminRotacion> _rotacionRepository;

        public readonly IBaseRepository<Colaboradores> _colaboradoresBaseRepository;

        private readonly IBaseRepository<ColaboradorCargaSocial> _cargaSocialRepository;
        private readonly IBaseRepository<ColaboradorDiscapacidad> _colaboradorDiscapacidadRepository;
        private readonly IBaseRepository<ColaboradoresHuellaDigital> _ColaboradorHuellaRepository;
        private readonly IBaseRepository<Capacitacion> _capacitacionesRepository;
        private readonly IBaseRepository<ContactoEmergencia> _contactoEmergencia;
        private readonly IBaseRepository<Pais> _paisRepository;
        private readonly IBaseRepository<Parroquia> _paarroquiaRepository;
        private readonly IBaseRepository<Contacto> _contactoBaseRepository;
        public ActualizacionSueldoAsyncBaseCrudAppService(
            IBaseRepository<ActualizacionSueldo> repository,
            IBaseRepository<CategoriasEncargado> categoriaEncargadoRepository,
            IBaseRepository<DetalleActualizacionSueldo> detalleActualizacionSueldoRepository,
            IBaseRepository<Catalogo> catalogoRepository,
            IBaseRepository<AdminRotacion> rotacionRepository,
            IBaseRepository<Colaboradores> colaboradoresBaseRepository,
            IBaseRepository<ColaboradorCargaSocial> cargaSocialRepository,
            IBaseRepository<ColaboradorDiscapacidad> colaboradorDiscapacidadRepository,
            IBaseRepository<ColaboradoresHuellaDigital> ColaboradorHuellaRepository,
            IBaseRepository<Capacitacion> capacitacionesRepository,
            IBaseRepository<ContactoEmergencia> contactoEmergencia,
            IBaseRepository<Pais> paisRepository,
            IBaseRepository<Parroquia> paarroquiaRepository,
            IBaseRepository<Contacto> contactoBaseRepository


         ) : base(repository)
        {
            _categoriaEncargadoRepository = categoriaEncargadoRepository;
            _detalleActualizacionSueldoRepository = detalleActualizacionSueldoRepository;
            _catalogoRepository = catalogoRepository;
            _rotacionRepository = rotacionRepository;
            _colaboradoresBaseRepository = colaboradoresBaseRepository;
            _cargaSocialRepository = cargaSocialRepository;
            _colaboradorDiscapacidadRepository = colaboradorDiscapacidadRepository;
            _ColaboradorHuellaRepository = ColaboradorHuellaRepository;
            _capacitacionesRepository =capacitacionesRepository;
            _contactoEmergencia = contactoEmergencia;
            _paisRepository=paisRepository;
            _paarroquiaRepository = paarroquiaRepository;
            _contactoBaseRepository = contactoBaseRepository;
        }
       


        public ExcelPackage DescargarPlantillaCargaMasivaDeJornales()
        {
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/RecursosHumanos/FormatoCargaJornales.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("Carga de Jornales", pck.Workbook.Worksheets[1]);
            }

            ExcelWorksheet jornales = excel.Workbook.Worksheets[1];

            var categoriasEncargado = _categoriaEncargadoRepository.GetAll()
                .Include(o => o.Categoria)
                .Include(o => o.Encargado)
                .Where(o => o.Encargado.codigo == "J52")
                .ToList();

            int countFilas = 2;
            int secuencial = 1;
            foreach (var categoria in categoriasEncargado)
            {
                var sueldoActual = buscarSueldoActual(categoria.Id);
                jornales.Cells["A" + countFilas].Value = secuencial;
                jornales.Cells["B" + countFilas].Value = categoria.Categoria.codigo;
                jornales.Cells["C" + countFilas].Value = categoria.Categoria.nombre;
                jornales.Cells["D" + countFilas].Value = sueldoActual;
                countFilas++;
                secuencial++;
            }

            return excel;
        }

        public ExcelPackage CargaMasivaDeSueldosJornales(HttpPostedFileBase uploadedFile, string observacioens, string fecha)
        {
             if (uploadedFile != null)
                {
                    if (uploadedFile.ContentType == "application/vnd.ms-excel" || uploadedFile.ContentType ==
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {

                        ExcelPackage excel = new ExcelPackage();
                    
                        string fileContentType = uploadedFile.ContentType;
                        byte[] fileBytes = new byte[uploadedFile.ContentLength];
                        
                        ExcelWorksheet jornales = null;
                        
                        using (var package = new ExcelPackage(uploadedFile.InputStream))
                        {
                            jornales = excel.Workbook.Worksheets.Add("Capacitaciones", package.Workbook.Worksheets[1]);
                            DateTime fechaInicio = Convert.ToDateTime(fecha);

                            var numberOfColumns = jornales.Dimension.End.Column;
                            var numberOfRows = jornales.Dimension.End.Row;

                            var ultimaCargaMasiva = Repository.GetAll().OrderByDescending(o => o.CreationTime)
                                .FirstOrDefault();
                            if (ultimaCargaMasiva != null)
                            {
                                ultimaCargaMasiva.FechaFin = fechaInicio.AddDays(-1);
                            }

                            var cabecera = new ActualizacionSueldo
                            {
                                Observaciones = observacioens,
                                FechaCarga = DateTime.Now,
                                NumeroRegistros = 1,
                                UrlArchivo = "",
                                FechaInicio = fechaInicio
                            };

                            Repository.Insert(cabecera);

                            for (int rowIterator = 2; rowIterator <= numberOfRows; rowIterator++)
                            {
                                var sueldoActual = Decimal.Parse((jornales.Cells["D" + rowIterator].Value ?? -1).ToString());
                                var sueldoNuevo = Decimal.Parse((jornales.Cells["E" + rowIterator].Value ?? -1).ToString());
                                var sueldoIsNull = jornales.Cells["E" + rowIterator].Value;
                                if (sueldoNuevo == sueldoActual || sueldoIsNull == null)
                                {
                                    continue;
                                }

                                var categoriaCodigo = (jornales.Cells["B" + rowIterator].Value ?? "").ToString();
                                var categoriaEncargado = _categoriaEncargadoRepository
                                    .GetAll()
                                    .Include(o => o.Categoria)
                                    .FirstOrDefault(o => o.Categoria.codigo == categoriaCodigo);

                                var detalle = new DetalleActualizacionSueldo
                                {
                                    ActualizacionSueldo = cabecera,
                                    CategoriaEncargadoId = categoriaEncargado.Id,
                                    ValorSueldo = sueldoNuevo,
                                };

                                _detalleActualizacionSueldoRepository.Insert(detalle);
                                

                                string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    var query =
                                        $"UPDATE [SCH_RRHH].[colaboradores] set [SCH_RRHH].[colaboradores].remuneracion_mensual = {sueldoNuevo} WHERE [SCH_RRHH].[colaboradores].estado = 'ACTIVO' AND [SCH_RRHH].[colaboradores].catalogo_grupo_id = {categoriaEncargado.CategoriaId}";
                                    SqlCommand command = new SqlCommand(query, connection);
                                    command.Connection.Open();
                                    var result = command.ExecuteNonQuery();
                                    command.Connection.Close();
                                }


                                jornales.Cells["F" + rowIterator].Value = "Actualizado Correctamente";

                            }
                        }
                        
                        return excel;
                    }

                    return new ExcelPackage();
                }

            return new ExcelPackage();
        }

        public decimal buscarSueldoActual(int CategoriaId)
        {
            var actualizacionSueldo = _detalleActualizacionSueldoRepository.GetAll()
                .Include(o => o.ActualizacionSueldo)
                .Where(o => o.CategoriaEncargadoId == CategoriaId)
                .OrderByDescending(o => o.CreationTime)
                .FirstOrDefault();

            if (actualizacionSueldo != null)
            {
                return actualizacionSueldo.ValorSueldo;
            }

            return 0;
        }

        public List<ActualizacionSueldoDto> ObtenerTodasLasActualizacionesDeSaldos()
        {
            var actualizaciones = Repository.GetAll()
                .Include(o => o.DetalleActualizacionSueldos.Select(x => x.CategoriaEncargado.Categoria))
                .OrderByDescending(o => o.FechaCarga)
                .ToList();
            var dtos = Mapper.Map<List<ActualizacionSueldo>, List<ActualizacionSueldoDto>>(actualizaciones);
            return dtos;
        }

        public List<DetalleActualizacionSueldoDto> ObtenerDetallesDeUnaActualizacion(int actualizacionId)
        {
            var detalles = _detalleActualizacionSueldoRepository.GetAll()
                .Include(o => o.CategoriaEncargado.Categoria)
                .Where(o => o.ActualizacionSueldoId == actualizacionId)
                .ToList();

            var dtos = Mapper.Map<List<DetalleActualizacionSueldo>, List<DetalleActualizacionSueldoDto>>(detalles);
            return dtos;
        }

        public ExcelPackage DescargarPlantillaActualizacionMasivaDeColaboradores()
        {
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/RecursosHumanos/FormatoActualizacionColaboradores.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("COLABORADORES", pck.Workbook.Worksheets[1]);
                excel.Workbook.Worksheets.Add("CATALOGOS", pck.Workbook.Worksheets[2]);
            }

            ExcelWorksheet colaboradores = excel.Workbook.Worksheets[1];
            ExcelWorksheet catalogos = excel.Workbook.Worksheets[2];

            var etnias = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "ETNIA");
            int countFilasEtnia = 2;
            foreach (var catalogo in etnias)
            {
                catalogos.Cells["A" + countFilasEtnia].Value = catalogo.Id;
                catalogos.Cells["B" + countFilasEtnia].Value = catalogo.nombre;
                countFilasEtnia++;
            }

            var estadoCivil = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "ESTADOCIVIL");
            int countFilasEstadoCivil = 2;
            foreach (var catalogo in estadoCivil)
            {
                catalogos.Cells["D" + countFilasEstadoCivil].Value = catalogo.Id;
                catalogos.Cells["E" + countFilasEstadoCivil].Value = catalogo.nombre;
                countFilasEstadoCivil++;
            }

            var sitioTrabajo = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "SITIODETRABAJO");
            int countFilasSitioTrabajo = 2;
            foreach (var catalogo in sitioTrabajo)
            {
                catalogos.Cells["G" + countFilasSitioTrabajo].Value = catalogo.Id;
                catalogos.Cells["H" + countFilasSitioTrabajo].Value = catalogo.nombre;
                countFilasSitioTrabajo++;
            }

            var area = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "AREA");
            int countFilasArea = 2;
            foreach (var catalogo in area)
            {
                catalogos.Cells["J" + countFilasArea].Value = catalogo.Id;
                catalogos.Cells["K" + countFilasArea].Value = catalogo.nombre;
                countFilasArea++;
            }

            var cargo = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "CARGO");
            int countFilasCargo = 2;
            foreach (var catalogo in cargo)
            {
                catalogos.Cells["M" + countFilasCargo].Value = catalogo.Id;
                catalogos.Cells["N" + countFilasCargo].Value = catalogo.nombre;
                countFilasCargo++;
            }

            var vinculoLaboral = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "VINCULOLABORAL");
            int countFilasVinculoLaboral = 2;
            foreach (var catalogo in vinculoLaboral)
            {
                catalogos.Cells["P" + countFilasVinculoLaboral].Value = catalogo.Id;
                catalogos.Cells["Q" + countFilasVinculoLaboral].Value = catalogo.nombre;
                countFilasVinculoLaboral++;
            }

            var clases = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "CLASE");
            int countFilasClase = 2;
            foreach (var catalogo in clases)
            {
                catalogos.Cells["S" + countFilasClase].Value = catalogo.Id;
                catalogos.Cells["T" + countFilasClase].Value = catalogo.nombre;
                countFilasClase++;
            }

            var asociacion = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "ASOCIACION");
            int countFilasAsociacion = 2;
            foreach (var catalogo in asociacion)
            {
                catalogos.Cells["V" + countFilasAsociacion].Value = catalogo.Id;
                catalogos.Cells["W" + countFilasAsociacion].Value = catalogo.nombre;
                countFilasAsociacion++;
            }

            var divisionPersonal = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "DIVISIONPERSONAL");
            int countFilasDivisionPersonal = 2;
            foreach (var catalogo in divisionPersonal)
            {
                catalogos.Cells["Y" + countFilasDivisionPersonal].Value = catalogo.Id;
                catalogos.Cells["Z" + countFilasDivisionPersonal].Value = catalogo.nombre;
                countFilasDivisionPersonal++;
            }

            var subDivisionPersonal = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "SUBDIVISIONPERSONAL");
            int countFilasSubDivisionPersonal = 2;
            foreach (var catalogo in subDivisionPersonal)
            {
                catalogos.Cells["AB" + countFilasSubDivisionPersonal].Value = catalogo.Id;
                catalogos.Cells["AC" + countFilasSubDivisionPersonal].Value = catalogo.nombre;
                countFilasSubDivisionPersonal++;
            }

            var tipoContrato = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "TIPOCONTRATO");
            int countFilasTipoContrato = 2;
            foreach (var catalogo in tipoContrato)
            {
                catalogos.Cells["AE" + countFilasTipoContrato].Value = catalogo.Id;
                catalogos.Cells["AF" + countFilasTipoContrato].Value = catalogo.nombre;
                countFilasTipoContrato++;
            }

            var claseContrato = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "CLASECONTRATO");
            int countFilasClaseContrato = 2;
            foreach (var catalogo in claseContrato)
            {
                catalogos.Cells["AH" + countFilasClaseContrato].Value = catalogo.Id;
                catalogos.Cells["AI" + countFilasClaseContrato].Value = catalogo.nombre;
                countFilasClaseContrato++;
            }

            var grupoPersonal = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "GRUPOPERSONAL");
            int countFilasGrupoPersonal = 2;
            foreach (var catalogo in grupoPersonal)
            {
                catalogos.Cells["AK" + countFilasGrupoPersonal].Value = catalogo.Id;
                catalogos.Cells["AL" + countFilasGrupoPersonal].Value = catalogo.nombre;
                countFilasGrupoPersonal++;
            }

            var subGrupoCuartil = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "SUBGRUPOCUARTIL");
            int countFilasSubGrupoCuartil = 2;
            foreach (var catalogo in subGrupoCuartil)
            {
                catalogos.Cells["AN" + countFilasSubGrupoCuartil].Value = catalogo.Id;
                catalogos.Cells["AO" + countFilasSubGrupoCuartil].Value = catalogo.nombre;
                countFilasSubGrupoCuartil++;
            }

            var rotaciones = _rotacionRepository.GetAll().ToList();
            int countFilasRotacion = 2;
            foreach (var catalogo in rotaciones)
            {
                catalogos.Cells["AQ" + countFilasRotacion].Value = catalogo.Id;
                catalogos.Cells["AR" + countFilasRotacion].Value = catalogo.codigo;
                countFilasRotacion++;
            }

            catalogos.Cells["AT" + 2].Value = "SI";
            catalogos.Cells["AT" + 3].Value = "NO";

            var encuadre = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "ENCUADRE");
            int countFilasEncuadre = 2;
            foreach (var catalogo in encuadre)
            {
                catalogos.Cells["AV" + countFilasEncuadre].Value = catalogo.Id;
                catalogos.Cells["AW" + countFilasEncuadre].Value = catalogo.nombre;
                countFilasEncuadre++;
            }

            var encargado = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "ENCARGADO");
            int countFilasEncargado = 2;
            foreach (var catalogo in encargado)
            {
                catalogos.Cells["AY" + countFilasEncargado].Value = catalogo.Id;
                catalogos.Cells["AZ" + countFilasEncargado].Value = catalogo.nombre;
                countFilasEncargado++;
            }

            var sector = _catalogoRepository.GetAll().Include(o => o.TipoCatalogo)
                .Where(o => o.TipoCatalogo.codigo == "SECTOR");
            int countFilasSector = 2;
            foreach (var catalogo in sector)
            {
                catalogos.Cells["BB" + countFilasSector].Value = catalogo.Id;
                catalogos.Cells["BC" + countFilasSector].Value = catalogo.nombre;
                countFilasSector++;
            }


            return excel;
        }
    
        
        public ExcelPackage CargaMasivaDeActualizacionColaboradores(HttpPostedFileBase uploadedFile)
        {
             if (uploadedFile != null)
                {
                    if (uploadedFile.ContentType == "application/vnd.ms-excel" || uploadedFile.ContentType ==
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {

                        ExcelPackage excel = new ExcelPackage();
                    
                        string fileContentType = uploadedFile.ContentType;
                        byte[] fileBytes = new byte[uploadedFile.ContentLength];
                        
                        ExcelWorksheet colaboradores = null;
                        
                        using (var package = new ExcelPackage(uploadedFile.InputStream))
                        {

                            colaboradores = excel.Workbook.Worksheets.Add("COLABORADORES", package.Workbook.Worksheets[1]);
                            excel.Workbook.Worksheets.Add("CATALOGOS", package.Workbook.Worksheets[2]);

                            var numberOfColumns = colaboradores.Dimension.End.Column;
                            var numberOfRows = colaboradores.Dimension.End.Row;

                            for (int rowIterator = 3; rowIterator <= numberOfRows; rowIterator++)
                            {
                                var cedula = (colaboradores.Cells["C" + rowIterator].Value ?? "").ToString();
                                var codigoSap = (colaboradores.Cells["A" + rowIterator].Value ?? "").ToString();
                                if (cedula.IsNullOrEmpty() && codigoSap.IsNullOrEmpty())
                                {
                                    colaboradores.Cells["AD" + rowIterator].Value = "Debe ingresar al menos la cédula o código SAP";
                                    colaboradores.Cells["A" + rowIterator + ":AC" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    colaboradores.Cells["A" + rowIterator + ":AC" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                    continue;
                                }

                                var intCodigoSap = codigoSap.IsNullOrEmpty() ? -1 : Int32.Parse(codigoSap);
                                var colaborador = _colaboradoresBaseRepository
                                    .GetAll().FirstOrDefault(o =>
                                        o.numero_identificacion == cedula || o.empleado_id_sap == intCodigoSap);

                                if (colaborador == null)
                                {
                                    colaboradores.Cells["AD" + rowIterator].Value = "Colaborador no encontrado";
                                    colaboradores.Cells["A" + rowIterator + ":AC" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    colaboradores.Cells["A" + rowIterator + ":AC" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                    continue;
                                }

                            if (colaborador == null && colaborador.estado==RRHHCodigos.ESTADO_ALTAANULADA)
                            {
                                colaboradores.Cells["AD" + rowIterator].Value = "No se puede actualizar el colaborador se encuentra en estado ALTA ANULADA";
                                colaboradores.Cells["A" + rowIterator + ":AC" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                colaboradores.Cells["A" + rowIterator + ":AC" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                continue;
                            }


                            var nombres = colaboradores.Cells["D" + rowIterator].Value;
                                if(nombres != null)
                                {
                                    colaborador.nombres_apellidos = nombres.ToString();
                                }

                                var etnia = colaboradores.Cells["E" + rowIterator].Value;
                                if(etnia != null)
                                {
                                    var etniaString = etnia.ToString();
                                    var etniaCatalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == etniaString);
                                    colaborador.catalogo_etnia_id = etniaCatalogo.Id;
                                }

                                var estadoCivil = colaboradores.Cells["F" + rowIterator].Value;
                                if(estadoCivil != null)
                                {
                                    var estadoCivilString = estadoCivil.ToString();
                                    var estadoCivilCatalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == estadoCivilString);
                                    colaborador.catalogo_estado_civil_id = estadoCivilCatalogo.Id;
                                }

                                
                                var area = colaboradores.Cells["H" + rowIterator].Value;
                                if(area != null)
                                {
                                    var areaString = area.ToString();
                                    var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == areaString);
                                    colaborador.catalogo_area_id = catalogo.Id;
                                }

                                var cargo = colaboradores.Cells["I" + rowIterator].Value;
                                if(cargo != null)
                                {
                                    var cargoString = cargo.ToString();
                                    var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == cargoString);
                                    colaborador.catalogo_cargo_id = catalogo.Id;
                                }

                                var vinculoLaboral = colaboradores.Cells["J" + rowIterator].Value;
                                if(vinculoLaboral != null)
                                {
                                    var vinculoLaboralString = vinculoLaboral.ToString();
                                    var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == vinculoLaboralString);
                                    colaborador.catalogo_vinculo_laboral_id = catalogo.Id;
                                }

                                var clase = colaboradores.Cells["K" + rowIterator].Value;
                                if(clase != null)
                                {
                                    var claseString = clase.ToString();
                                    var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == claseString);
                                    colaborador.catalogo_clase_id = catalogo.Id;
                                }

                                var asociacion = colaboradores.Cells["L" + rowIterator].Value;
                                if(asociacion != null)
                                {
                                    var asociacionString = asociacion.ToString();
                                    var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == asociacionString);
                                    colaborador.catalogo_asociacion_id = catalogo.Id;
                                }

                                var divisionGrupoPersonal = colaboradores.Cells["M" + rowIterator].Value;
                                if(divisionGrupoPersonal != null)
                                {
                                    var divisionGrupoPersonalString = divisionGrupoPersonal.ToString();
                                    var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == divisionGrupoPersonalString);
                                    colaborador.catalogo_division_personal_id = catalogo.Id;
                                }

                                var subDivisionGrupoPersonal = colaboradores.Cells["N" + rowIterator].Value;
                                if(subDivisionGrupoPersonal != null)
                                {
                                    var subDivisionGrupoPersonalString = subDivisionGrupoPersonal.ToString();
                                    var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == subDivisionGrupoPersonalString);
                                    colaborador.catalogo_subdivision_personal_id = catalogo.Id;
                                }

                                var tipoContrato = colaboradores.Cells["O" + rowIterator].Value;
                                if(tipoContrato != null)
                                {
                                    var tipoContratoString = tipoContrato.ToString();
                                    var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == tipoContratoString);
                                    colaborador.catalogo_tipo_contrato_id = catalogo.Id;
                                }

                                var claseContrato = colaboradores.Cells["P" + rowIterator].Value;
                                if(claseContrato != null)
                                {
                                    var claseContratoString = claseContrato.ToString();
                                    var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == claseContratoString);
                                    colaborador.catalogo_clase_contrato_id = catalogo.Id;
                                }

                                var grupo = colaboradores.Cells["Q" + rowIterator].Value;
                                if(grupo != null)
                                {
                                    var grupoString = grupo.ToString();
                                    var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == grupoString);
                                    colaborador.catalogo_grupo_id = catalogo.Id;
                                }

                                var subGrupo = colaboradores.Cells["R" + rowIterator].Value;
                                if(subGrupo != null)
                                {
                                    var subGrupoString = subGrupo.ToString();
                                    var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == subGrupoString);
                                    colaborador.catalogo_subgrupo_id = catalogo.Id;
                                }

                                var rotacion = colaboradores.Cells["S" + rowIterator].Value;
                                if(rotacion != null)
                                {
                                    var rotacionString = rotacion.ToString();
                                    var catalogoRotacion = _rotacionRepository.FirstOrDefault(o => o.codigo == rotacionString);
                                    colaborador.AdminRotacionId = catalogoRotacion.Id;
                                }

                                var posicion = colaboradores.Cells["T" + rowIterator].Value;
                                if(posicion != null)
                                {
                                    colaborador.posicion =  posicion.ToString();
                                }

                                var fechaCaducidadContrato = colaboradores.Cells["U" + rowIterator].Value;
                                if(fechaCaducidadContrato != null)
                                {
                                    colaborador.fecha_caducidad_contrato =  Convert.ToDateTime(fechaCaducidadContrato);
                                } 

                                var ejecutor = colaboradores.Cells["V" + rowIterator].Value;
                                if(ejecutor != null)
                                {
                                    var ejecutorString = ejecutor.ToString();
                                    colaborador.ejecutor_obra =  ejecutorString == "SI" ? true : false
                                    ;
                                }

                                var remuneracion = colaboradores.Cells["W" + rowIterator].Value;
                                if(remuneracion != null)
                                {
                                    colaborador.remuneracion_mensual =  Decimal.Parse(remuneracion.ToString());
                                }

                                var numeroHijos = colaboradores.Cells["X" + rowIterator].Value;
                                if(numeroHijos != null)
                                {
                                    colaborador.numero_hijos =  Int32.Parse(numeroHijos.ToString());
                                }

                                var encuadre = colaboradores.Cells["Y" + rowIterator].Value;
                                if(encuadre != null)
                                {
                                    var encuadreString = encuadre.ToString();
                                    var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == encuadreString);
                                    colaborador.catalogo_encuadre_id = catalogo.Id;
                                }

                                var encargado = colaboradores.Cells["Z" + rowIterator].Value;
                                if(encargado != null)
                                {
                                    var encargadoString = encargado.ToString();
                                    var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == encargadoString);
                                    colaborador.catalogo_encargado_personal_id = catalogo.Id;
                                }

                                var sector = colaboradores.Cells["AA" + rowIterator].Value;
                                if(sector != null)
                                {
                                    var sectorString = sector.ToString();
                                    var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == sectorString);
                                    colaborador.catalogo_sector_id = catalogo.Id;
                                }

                                var sapLocal = colaboradores.Cells["AC" + rowIterator].Value;
                                if(sapLocal != null)
                                {
                                    colaborador.empleado_id_sap_local =  Int32.Parse(sapLocal.ToString());
                                }
                                
                                colaboradores.Cells["AD" + rowIterator].Value = "Actualizado Correctamente";

                            }
                        }
                        
                        return excel;
                    }
                    return new ExcelPackage();
                }
            return new ExcelPackage();
        }


        public ExcelPackage CargaMasivaReingresoColaboradores(HttpPostedFileBase uploadedFile)
        {
            if (uploadedFile != null)
            {
                if (uploadedFile.ContentType == "application/vnd.ms-excel" || uploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {

                    ExcelPackage excel = new ExcelPackage();

                    string fileContentType = uploadedFile.ContentType;
                    byte[] fileBytes = new byte[uploadedFile.ContentLength];

                    ExcelWorksheet colaboradores = null;

                    using (var package = new ExcelPackage(uploadedFile.InputStream))
                    {

                        colaboradores = excel.Workbook.Worksheets.Add("COLABORADORES", package.Workbook.Worksheets[1]);
                        excel.Workbook.Worksheets.Add("CATALOGOS", package.Workbook.Worksheets[2]);

                        var numberOfColumns = colaboradores.Dimension.End.Column;
                        var numberOfRows = colaboradores.Dimension.End.Row;

                        for (int rowIterator = 3; rowIterator <= numberOfRows; rowIterator++)
                        {
                            var cedula = (colaboradores.Cells["C" + rowIterator].Value ?? "").ToString();
                            var codigoSap = (colaboradores.Cells["A" + rowIterator].Value ?? "").ToString();

                            if (cedula.IsNullOrEmpty() && codigoSap.IsNullOrEmpty())
                            {
                                colaboradores.Cells["AD" + rowIterator].Value = "Debe ingresar al menos la cédula o código SAP";
                                colaboradores.Cells["A" + rowIterator + ":AC" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                colaboradores.Cells["A" + rowIterator + ":AC" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                continue;
                            }

                            var intCodigoSap = codigoSap.IsNullOrEmpty() ? -1 : Int32.Parse(codigoSap);

                
                            var e = _colaboradoresBaseRepository
                                .GetAll().Where(c=>c.estado=="INACTIVO").FirstOrDefault(o =>
                                    o.numero_identificacion == cedula || o.empleado_id_sap == intCodigoSap);

                            if (e == null)
                            {
                                colaboradores.Cells["AD" + rowIterator].Value = "No se encontro registros del colaborador";
                                colaboradores.Cells["A" + rowIterator + ":AC" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                colaboradores.Cells["A" + rowIterator + ":AC" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                continue;
                            }

                            var fechaInicio = (colaboradores.Cells["E"+rowIterator].Text ?? "01/01/1999").ToString();
                            DateTime tempfechaIngreso = DateTime.Parse(fechaInicio);



                            var colaborador = new Colaboradores()
                            {
                                fecha_ingreso = tempfechaIngreso,
                                estado = RRHHCodigos.ESTADO_TEMPORAL,
                                candidato_id_sap = intCodigoSap,
                                empleado_id_sap_local = intCodigoSap,

                                primer_apellido = e.primer_apellido,
                                segundo_apellido = e.segundo_apellido,
                                nombres = e.nombres,
                                catalogo_genero_id = e.catalogo_genero_id,
                               catalogo_etnia_id = e.catalogo_etnia_id,
                                PaisId = e.PaisId,
                                pais_pais_nacimiento_id = e.pais_pais_nacimiento_id,
                                catalogo_estado_civil_id = e.catalogo_estado_civil_id,
                                numero_hijos = e.numero_hijos,
                                catalogo_formacion_educativa_id = e.catalogo_formacion_educativa_id,
                                es_sustituto = e.es_sustituto,
                                fecha_sustituto_desde = e.fecha_sustituto_desde,
                                //catalogo_encargado_personal_id = temp.catalogo_encargado_personal_id,
                                ContratoId = e.ContratoId,
                                catalogo_destino_estancia_id = e.catalogo_destino_estancia_id,
                                catalogo_area_id = null,
                                catalogo_sector_id = null,
                                catalogo_cargo_id = null,
                                catalogo_clase_id = null,
                                catalogo_vinculo_laboral_id = null,
                                catalogo_asociacion_id = null,
                                catalogo_encuadre_id = null,
                                catalogo_planes_beneficios_id = null,
                                catalogo_plan_beneficios_id = null,
                                catalogo_plan_salud_id = null,
                                catalogo_cobertura_dependiente_id = null,
                                catalogo_apto_medico_id = null,
                                catalogo_division_personal_id = null,
                                catalogo_subdivision_personal_id = null,
                                catalogo_funcion_id = null,
                                catalogo_tipo_contrato_id = null,
                                catalogo_clase_contrato_id = null,
                                fecha_caducidad_contrato = null,
                                ejecutor_obra = false,
                                catalogo_tipo_nomina_id = null,
                                catalogo_periodo_nomina_id = null,
                                catalogo_grupo_personal_id = null,
                                catalogo_grupo_id = null,
                                catalogo_subgrupo_id = null,
                                remuneracion_mensual = null,
                                ContactoId = e.ContactoId,
                                codigo_seguridad_qr = e.codigo_seguridad_qr,
                                codigo_dactilar = e.codigo_dactilar,
                                AdminRotacionId = e.AdminRotacionId,
                                catalogo_banco_id = e.catalogo_banco_id,
                                catalogo_codigo_incapacidad_id = e.catalogo_codigo_incapacidad_id,
                                catalogo_codigo_siniestro_id = e.catalogo_codigo_siniestro_id,
                                catalogo_forma_pago_id = e.catalogo_forma_pago_id,

                                catalogo_sitio_trabajo_id = e.catalogo_sitio_trabajo_id,
                                catalogo_tipo_cuenta_id = e.catalogo_tipo_cuenta_id,
                                catalogo_tipo_identificacion_id = e.catalogo_tipo_identificacion_id,
                                catalogo_via_pago_id = e.catalogo_via_pago_id,
                                empleado_id_sap = e.empleado_id_sap,
                                empresa_id = e.empresa_id,
                                es_carga_masiva = e.es_carga_masiva,
                                es_externo = e.es_externo,
                                empresa_tercero = e.empresa_tercero,
                                es_visita = e.es_visita,
                                meta4 = e.meta4,
                                fecha_alta = e.fecha_alta,
                                fecha_carga_masiva = e.fecha_carga_masiva,
                                fecha_matrimonio = e.fecha_matrimonio,
                                fecha_nacimiento = e.fecha_nacimiento,
                                fecha_registro_civil = e.fecha_registro_civil,

                               
                                validacion_cedula = e.validacion_cedula,
                                viene_registro_civil = e.viene_registro_civil,
                                vigente = true,
                                tiene_ausentismo = e.tiene_ausentismo,
                                posicion = e.posicion,
                                numero_legajo_temporal = e.numero_legajo_temporal,
                                numero_cuenta = e.numero_cuenta,
                                numero_identificacion = e.numero_identificacion,
                                numero_legajo_definitivo = e.numero_legajo_definitivo,
                                es_reingreso = true,
                               



                            };


                            var nombres = colaboradores.Cells["D" + rowIterator].Value;
                            if (nombres != null)
                            {
                                colaborador.nombres_apellidos = nombres.ToString();
                            }

                            var etnia = colaboradores.Cells["F" + rowIterator].Value;
                            if (etnia != null)
                            {
                                var etniaString = etnia.ToString();
                                var etniaCatalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == etniaString);
                                colaborador.catalogo_etnia_id = etniaCatalogo.Id;
                            }

                            var estadoCivil = colaboradores.Cells["G" + rowIterator].Value;
                            if (estadoCivil != null)
                            {
                                var estadoCivilString = estadoCivil.ToString();
                                var estadoCivilCatalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == estadoCivilString);
                                colaborador.catalogo_estado_civil_id = estadoCivilCatalogo.Id;
                            }
                            var sitio_trabajo = colaboradores.Cells["H" + rowIterator].Value;
                            if (sitio_trabajo != null)
                            {
                                var sitio_trabajoString = sitio_trabajo.ToString();
                                var estadoCivilCatalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == sitio_trabajoString);
                                colaborador.catalogo_sitio_trabajo_id = estadoCivilCatalogo.Id.ToString(); //Catalogo String
                            }

                            var agrupacion_para_requisitos = colaboradores.Cells["AC" + rowIterator].Value;
                            if (agrupacion_para_requisitos != null)
                            {
                                var agrupacion_para_requisitosString = agrupacion_para_requisitos.ToString();
                                var agrupacion_para_requisitosCatalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == agrupacion_para_requisitosString);
                                colaborador.catalogo_grupo_personal_id = agrupacion_para_requisitosCatalogo.Id; //Catalogo String
                            }
                            




                            var area = colaboradores.Cells["I" + rowIterator].Value;
                            if (area != null)
                            {
                                var areaString = area.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == areaString);
                                colaborador.catalogo_area_id = catalogo != null ? catalogo.Id : 0;
                            }

                            var cargo = colaboradores.Cells["J" + rowIterator].Value;
                            if (cargo != null)
                            {
                                var cargoString = cargo.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == cargoString);
                                colaborador.catalogo_cargo_id = catalogo != null ? catalogo.Id : 0; ;
                            }

                            var vinculoLaboral = colaboradores.Cells["K" + rowIterator].Value;
                            if (vinculoLaboral != null)
                            {
                                var vinculoLaboralString = vinculoLaboral.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == vinculoLaboralString);
                                colaborador.catalogo_vinculo_laboral_id = catalogo != null ? catalogo.Id : 0;
                            }

                            var clase = colaboradores.Cells["L" + rowIterator].Value;
                            if (clase != null)
                            {
                                var claseString = clase.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == claseString);
                                colaborador.catalogo_clase_id = catalogo != null ? catalogo.Id : 0;
                            }

                            var asociacion = colaboradores.Cells["M" + rowIterator].Value;
                            if (asociacion != null)
                            {
                                var asociacionString = asociacion.ToString().ToUpper().TrimEnd().TrimStart();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre==asociacionString);
                                colaborador.catalogo_asociacion_id = catalogo!=null?catalogo.Id:0;
                            }

                            var divisionGrupoPersonal = colaboradores.Cells["N" + rowIterator].Value;
                            if (divisionGrupoPersonal != null)
                            {
                                var divisionGrupoPersonalString = divisionGrupoPersonal.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == divisionGrupoPersonalString);
                                colaborador.catalogo_division_personal_id = catalogo != null ? catalogo.Id : 0;
                            }

                            var subDivisionGrupoPersonal = colaboradores.Cells["O" + rowIterator].Value;
                            if (subDivisionGrupoPersonal != null)
                            {
                                var subDivisionGrupoPersonalString = subDivisionGrupoPersonal.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == subDivisionGrupoPersonalString);
                                colaborador.catalogo_subdivision_personal_id = catalogo != null ? catalogo.Id : 0;
                            }

                            var tipoContrato = colaboradores.Cells["P" + rowIterator].Value;
                            if (tipoContrato != null)
                            {
                                var tipoContratoString = tipoContrato.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == tipoContratoString);
                                colaborador.catalogo_tipo_contrato_id = catalogo != null ? catalogo.Id : 0;
                            }

                            var claseContrato = colaboradores.Cells["Q" + rowIterator].Value;
                            if (claseContrato != null)
                            {
                                var claseContratoString = claseContrato.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == claseContratoString);
                                colaborador.catalogo_clase_contrato_id = catalogo != null ? catalogo.Id : 0;
                            }

                            var grupo = colaboradores.Cells["R" + rowIterator].Value;
                            if (grupo != null)
                            {
                                var grupoString = grupo.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == grupoString);
                                colaborador.catalogo_grupo_id = catalogo != null ? catalogo.Id : 0;
                            }

                            var subGrupo = colaboradores.Cells["S" + rowIterator].Value;
                            if (subGrupo != null)
                            {
                                var subGrupoString = subGrupo.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == subGrupoString);
                                colaborador.catalogo_subgrupo_id = catalogo != null ? catalogo.Id : 0;
                            }

                            var rotacion = colaboradores.Cells["T" + rowIterator].Value;
                            if (rotacion != null)
                            {
                                var rotacionString = rotacion.ToString();
                                var catalogoRotacion = _rotacionRepository.FirstOrDefault(o => o.codigo == rotacionString);
                                colaborador.AdminRotacionId = catalogoRotacion != null? catalogoRotacion.Id:0;
                            }

                            var posicion = colaboradores.Cells["U" + rowIterator].Value;
                            if (posicion != null)
                            {
                                colaborador.posicion = posicion.ToString();
                            }

                            var fechaCaducidadContrato = colaboradores.Cells["V" + rowIterator].Value;
                            if (fechaCaducidadContrato != null)
                            {
                                colaborador.fecha_caducidad_contrato = Convert.ToDateTime(fechaCaducidadContrato);
                            }

                            var ejecutor = colaboradores.Cells["W" + rowIterator].Value;
                            if (ejecutor != null)
                            {
                                var ejecutorString = ejecutor.ToString();
                                colaborador.ejecutor_obra = ejecutorString == "SI" ? true : false
                                ;
                            }

                            var remuneracion = colaboradores.Cells["X" + rowIterator].Value;
                            if (remuneracion != null)
                            {
                                colaborador.remuneracion_mensual = Decimal.Parse(remuneracion.ToString());
                            }

                            var numeroHijos = colaboradores.Cells["Y" + rowIterator].Value;
                            if (numeroHijos != null)
                            {
                                colaborador.numero_hijos = Int32.Parse(numeroHijos.ToString());
                            }

                            var encuadre = colaboradores.Cells["Z" + rowIterator].Value;
                            if (encuadre != null)
                            {
                                var encuadreString = encuadre.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == encuadreString);
                                colaborador.catalogo_encuadre_id = catalogo != null ? catalogo.Id : 0;
                            }





                            var encargado = colaboradores.Cells["AA" + rowIterator].Value;
                            if (encargado != null)
                            {
                                var encargadoString = encargado.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == encargadoString);
                                colaborador.catalogo_encargado_personal_id = catalogo != null ? catalogo.Id : 0;
                            }

                            var sector = colaboradores.Cells["AB" + rowIterator].Value;
                            if (sector != null)
                            {
                                var sectorString = sector.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == sectorString);
                                colaborador.catalogo_sector_id = catalogo != null ? catalogo.Id : 0;
                            }

                      

                            var colaboradorExterno = _colaboradoresBaseRepository.GetAll().Where(c => c.es_externo.HasValue)
                                                                        .Where(c => c.es_externo.Value)
                                                                        .Where(c => c.vigente)
                                                                        .Where(c => c.numero_identificacion == cedula)
                                                                        .Where(c => c.estado == RRHHCodigos.ESTADO_ACTIVO)
                                                                        .FirstOrDefault();


                            if (colaboradorExterno != null)
                            {
                                var colexterno = _colaboradoresBaseRepository.Get(colaboradorExterno.Id);
                                colexterno.estado = RRHHCodigos.BAJA_INACTIVO;
                                colexterno.vigente = false;
                                _colaboradoresBaseRepository.Update(colexterno);
                            }



                            int colaboradorReingresoId = _colaboradoresBaseRepository.InsertAndGetId(colaborador);


                            ///---------------- Clonar Tablas Existentes--------------///

                            //ColaboradoresCargasSociales
                            var CargasSocialesExistentes = _cargaSocialRepository.GetAll().Where(c => c.vigente).Where(c => c.ColaboradoresId == e.Id).ToList();
                            if (CargasSocialesExistentes.Count > 0)
                            {
                                foreach (var cargaSocial in CargasSocialesExistentes)
                                {
                                    var nuevaCargaSocial = new ColaboradorCargaSocial()
                                    {
                                        ColaboradoresId = colaboradorReingresoId,
                                        estado_civil = cargaSocial.estado_civil,
                                        fecha_matrimonio = cargaSocial.fecha_matrimonio,
                                        fecha_nacimiento = cargaSocial.fecha_nacimiento,
                                        idGenero = cargaSocial.idGenero,
                                        idTipoIdentificacion = cargaSocial.idTipoIdentificacion,
                                        nombres = cargaSocial.nombres,
                                        nombres_apellidos = cargaSocial.nombres_apellidos,
                                        nro_identificacion = cargaSocial.nro_identificacion,
                                        PaisId = cargaSocial.PaisId,
                                        pais_nacimiento = cargaSocial.pais_nacimiento,
                                        parentesco_id = cargaSocial.parentesco_id,
                                        por_sustitucion = cargaSocial.por_sustitucion,
                                        primer_apellido = cargaSocial.primer_apellido,
                                        segundo_apellido = cargaSocial.segundo_apellido,
                                        viene_registro_civil = cargaSocial.viene_registro_civil,
                                        vigente = cargaSocial.vigente

                                    };


                                    _cargaSocialRepository.Insert(nuevaCargaSocial);
                                }
                            }
                            //ColaboradoresDiscapacidades
                            var ColaboradoresDiscapacidades = _colaboradorDiscapacidadRepository.GetAll().Where(c => c.vigente).Where(c => c.ColaboradoresId == e.Id).ToList();
                            if (ColaboradoresDiscapacidades.Count > 0)
                            {
                                foreach (var discap in ColaboradoresDiscapacidades)
                                {
                                    var nuevDiscapacidad = new ColaboradorDiscapacidad()
                                    {
                                        vigente = discap.vigente,
                                        catalogo_porcentaje_id = discap.catalogo_porcentaje_id,
                                        catalogo_tipo_discapacidad_id = discap.catalogo_tipo_discapacidad_id,
                                        ColaboradorCargaSocialId = discap.ColaboradorCargaSocialId,
                                        ColaboradoresId = colaboradorReingresoId,

                                    };

                                    _colaboradorDiscapacidadRepository.Insert(nuevDiscapacidad);
                                }
                            }
                            //Colaboradores Huellas 
                            var ColaboradoresHuellas = _ColaboradorHuellaRepository.GetAll().Where(c => c.vigente).Where(c => c.colaborador_id == e.Id).ToList();
                            if (ColaboradoresHuellas.Count > 0)
                            {
                                foreach (var huella in ColaboradoresHuellas)
                                {
                                    var nuevaHuella = new ColaboradoresHuellaDigital()
                                    {
                                        vigente = huella.vigente,
                                        huella = huella.huella,
                                        catalogo_dedo_id = huella.catalogo_dedo_id,
                                        colaborador_id = colaboradorReingresoId,
                                        fecha_registro = huella.fecha_registro,
                                        IsDeleted = huella.IsDeleted,
                                        plantilla_base64 = huella.plantilla_base64,
                                        principal = huella.principal,

                                    };
                                    _ColaboradorHuellaRepository.Insert(nuevaHuella);
                                }
                            }
                            // Colaboradores Requisitos No se Clonará
                            // Colaboradores Servicios No se Clonará cada contratacion son nuevas condiciones

                            // Contactos Emergencias
                            var ContactosEmergencias = _contactoEmergencia.GetAll().Where(c => c.ColaboradorId == e.Id).ToList();
                            if (ContactosEmergencias.Count > 0)
                            {
                                foreach (var contactoe in ContactosEmergencias)
                                {
                                    var nContactoEmergencia = new ContactoEmergencia()
                                    {
                                        Identificacion = contactoe.Identificacion,
                                        Direccion = contactoe.Direccion,
                                        Nombres = contactoe.Nombres,
                                        Celular = contactoe.Celular,
                                        ColaboradorId = colaboradorReingresoId,
                                        Telefono = contactoe.Telefono,
                                        Relacion = contactoe.Relacion,
                                        UrbanizacionComuna = contactoe.UrbanizacionComuna,


                                    };

                                    _contactoEmergencia.Insert(nContactoEmergencia);
                                }
                            }
                            // Capacitaciones
                            var Capacitaciones = _capacitacionesRepository.GetAll().Where(c => c.ColaboradoresId == e.Id).ToList();
                            if (Capacitaciones.Count > 0)
                            {
                                foreach (var capacitacion in Capacitaciones)
                                {
                                    var nuevaCapacitacion = new Capacitacion()
                                    {
                                        ColaboradoresId = capacitacion.ColaboradoresId,
                                        CatalogoNombreCapacitacionId = capacitacion.CatalogoNombreCapacitacionId,
                                        CatalogoTipoCapacitacionId = capacitacion.CatalogoTipoCapacitacionId,
                                        Fecha = capacitacion.Fecha,
                                        Fuente = capacitacion.Fuente,
                                        Horas = capacitacion.Horas,
                                        Observaciones = capacitacion.Observaciones,

                                    };

                                    _capacitacionesRepository.Insert(nuevaCapacitacion);
                                }
                            }





















               




                        }
                    }

                    return excel;
                }
                return new ExcelPackage();
            }
            return new ExcelPackage();
        }


        public ExcelPackage ActualizacionMasivaReingresoColaboradores(HttpPostedFileBase uploadedFile)
        {
            if (uploadedFile != null)
            {
                if (uploadedFile.ContentType == "application/vnd.ms-excel" || uploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {

                    ExcelPackage excel = new ExcelPackage();

                    string fileContentType = uploadedFile.ContentType;
                    byte[] fileBytes = new byte[uploadedFile.ContentLength];

                    ExcelWorksheet colaboradores = null;

                    using (var package = new ExcelPackage(uploadedFile.InputStream))
                    {

                        colaboradores = excel.Workbook.Worksheets.Add("COLABORADORES", package.Workbook.Worksheets[1]);
                        excel.Workbook.Worksheets.Add("CATALOGOS", package.Workbook.Worksheets[2]);

                        var numberOfColumns = colaboradores.Dimension.End.Column;
                        var numberOfRows = colaboradores.Dimension.End.Row;

                        for (int rowIterator = 3; rowIterator <= numberOfRows; rowIterator++)
                        {
                            var cedula = (colaboradores.Cells["A" + rowIterator].Value ?? "").ToString();
                            var codigoSap = (colaboradores.Cells["D" + rowIterator].Value ?? "").ToString();
                            var codigoSapLocal = (colaboradores.Cells["E" + rowIterator].Value ?? "").ToString();


                            if (cedula.IsNullOrEmpty() && codigoSap.IsNullOrEmpty())
                            {
                                colaboradores.Cells["AD" + rowIterator].Value = "Debe ingresar al menos la cédula o código SAP";
                                colaboradores.Cells["A" + rowIterator + ":AC" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                colaboradores.Cells["A" + rowIterator + ":AC" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                continue;
                            }

                            var intCodigoSap = codigoSap.IsNullOrEmpty() ? -1 : Int32.Parse(codigoSap);
                            var intCodigoSapLocal = codigoSap.IsNullOrEmpty() ? -1 : Int32.Parse(codigoSapLocal);


                            var e = _colaboradoresBaseRepository.GetAll().Where(c => c.estado == "TEMPORAL").FirstOrDefault(o =>o.numero_identificacion == cedula || o.empleado_id_sap == intCodigoSap);

                            if (e == null)
                            {
                                colaboradores.Cells["AD" + rowIterator].Value = "No se encontro registros del colaborador";
                                colaboradores.Cells["A" + rowIterator + ":AC" + rowIterator].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                colaboradores.Cells["A" + rowIterator + ":AC" + rowIterator].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
                                continue;
                            }

                            var colaborador = _colaboradoresBaseRepository.Get(e.Id);



                            var nombres = colaboradores.Cells["B" + rowIterator].Value;
                            if (nombres != null)
                            {
                                colaborador.nombres_apellidos = nombres.ToString();
                            }

                            var meta4 = colaboradores.Cells["c" + rowIterator].Value;
                            if (meta4 != null) {
                                colaborador.meta4 = meta4.ToString();
                            }

                            if (intCodigoSap > 0) {
                                colaborador.empleado_id_sap = intCodigoSap;
                            }
                            if (intCodigoSapLocal > 0) {
                                colaborador.empleado_id_sap_local = intCodigoSapLocal;
                            }

                            var pais= colaboradores.Cells["F" + rowIterator].Value;
                            if (pais != null) {

                                var paisEncontrado = _paisRepository.GetAll().Where(c => c.nombre == pais.ToString()).FirstOrDefault();
                                if (paisEncontrado != null) {
                                    colaborador.PaisId = paisEncontrado.Id;
                                }
                            }
                            var calleNumero = colaboradores.Cells["H" + rowIterator].Value;

                            var celular = colaboradores.Cells["I" + rowIterator].Value;

                            var parroquia= colaboradores.Cells["G" + rowIterator].Value.ToString();


                            if (colaborador.ContactoId.HasValue) {

                                var contacto = _contactoBaseRepository.Get(colaborador.ContactoId.Value);
                                if (contacto != null) { 
                                var parroquiaEncontrada = _paarroquiaRepository.GetAll().Where(c => c.nombre == parroquia).FirstOrDefault();


                                if (calleNumero != null) { 
                                contacto.calle_principal = calleNumero.ToString();
                                }
                                if (celular != null) {
                                    contacto.celular = celular.ToString().Length < 10 ? "0" + celular.ToString() : celular.ToString();
                                }
                                if (parroquia != null) {
                                        if (parroquiaEncontrada != null) {
                                    contacto.ParroquiaId = parroquiaEncontrada.Id;
                                        }
                                    }
                                }
                            }

                            var asociacion = colaboradores.Cells["J" + rowIterator].Value;
                            if (asociacion != null)
                            {
                                var asociacionString = asociacion.ToString().ToUpper().TrimEnd().TrimStart();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == asociacionString);
                                colaborador.catalogo_asociacion_id = catalogo != null ? catalogo.Id : 0;
                            }

                            var encuadre = colaboradores.Cells["K" + rowIterator].Value;
                            if (encuadre != null)
                            {
                                var encuadreString = encuadre.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == encuadreString);
                                colaborador.catalogo_encuadre_id = catalogo != null ? catalogo.Id : 0;
                            }

                            var planBeneficio= colaboradores.Cells["L" + rowIterator].Value;
                            if (planBeneficio != null) {

                                var planBeneficioString = planBeneficio.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == planBeneficioString);
                                colaborador.catalogo_plan_beneficios_id= catalogo != null ? catalogo.Id : 0;

                            }


                            var planSalud = colaboradores.Cells["M" + rowIterator].Value;
                            if (planSalud != null)
                            {

                                var planSaludString = planSalud.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == planSaludString);
                                colaborador.catalogo_plan_salud_id = catalogo != null ? catalogo.Id : 0;

                            }
                            var coberturaDependiente = colaboradores.Cells["N" + rowIterator].Value;
                            if (coberturaDependiente != null)
                            {

                                var coberturaDependienteString = coberturaDependiente.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == coberturaDependienteString);
                                colaborador.catalogo_cobertura_dependiente_id = catalogo != null ? catalogo.Id : 0;

                            }

                            var planesBeneficio = colaboradores.Cells["O" + rowIterator].Value;
                            if (planesBeneficio != null)
                            {

                                var planesBeneficioString = planesBeneficio.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == planesBeneficioString);
                                colaborador.catalogo_planes_beneficios_id = catalogo != null ? catalogo.Id : 0;
                            }

                            var tipoAptoMedico = colaboradores.Cells["P" + rowIterator].Value;
                            if (tipoAptoMedico != null)
                            {

                                var tipoAptoMedicoString = tipoAptoMedico.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == tipoAptoMedicoString);
                                colaborador.catalogo_apto_medico_id = catalogo != null ? catalogo.Id : 0;
                            }

                            var funcion = colaboradores.Cells["Q" + rowIterator].Value;
                            if (funcion != null)
                            {
                                var funcionString = funcion.ToString();
                                var catalogo = _catalogoRepository.FirstOrDefault(o => o.nombre == funcionString);
                                colaborador.catalogo_funcion_id = catalogo != null ? catalogo.Id : 0;
                            }


                            //Update Colaborador

                            if (e.fecha_ingreso.HasValue) {
                                var fechaCaducicadContrato = e.fecha_ingreso.Value.AddYears(1);
                                colaborador.fecha_caducidad_contrato = fechaCaducicadContrato;
                             }

                            colaborador.estado = RRHHCodigos.ESTADO_ACTIVO;
                            _colaboradoresBaseRepository.Update(colaborador);


                        }
                    }

                    return excel;
                }
                return new ExcelPackage();
            }
            return new ExcelPackage();
        }

    }
}

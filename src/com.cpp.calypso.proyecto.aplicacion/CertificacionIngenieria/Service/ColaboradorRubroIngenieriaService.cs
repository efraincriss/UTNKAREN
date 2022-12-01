using AutoMapper;
using com.cpp.calypso.comun.aplicacion;
using com.cpp.calypso.comun.dominio;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Dto;
using com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Interface;
using com.cpp.calypso.proyecto.dominio;
using com.cpp.calypso.proyecto.dominio.CertificacionIngenieria;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.cpp.calypso.proyecto.aplicacion.CertificacionIngenieria.Service
{
    public class ColaboradorRubroIngenieriaAsyncBaseCrudAppService : AsyncBaseCrudAppService<ColaboradorRubroIngenieria, ColaboradorRubroIngenieriaDto, PagedAndFilteredResultRequestDto>, IColaboradorRubroIngenieriaAsyncBaseCrudAppService
    {
        private readonly IBaseRepository<DetallePreciario> repositoryPreciario;
        private readonly IBaseRepository<Colaboradores> _colaboradorRepository;
        private readonly IBaseRepository<Item> _itemRepository;
        private readonly IBaseRepository<Contrato> _contratoRepository;

        public ColaboradorRubroIngenieriaAsyncBaseCrudAppService(
            IBaseRepository<ColaboradorRubroIngenieria> repository,
            IBaseRepository<DetallePreciario> repositoryPreciario,
            IBaseRepository<Colaboradores> colaboradorRepository,
            IBaseRepository<Item> itemRepository,
            IBaseRepository<Contrato> contratoRepository
        ) : base(repository)
        {
            this.repositoryPreciario = repositoryPreciario;
            _colaboradorRepository = colaboradorRepository;
            _itemRepository = itemRepository;
            _contratoRepository = contratoRepository;
        }

        public async Task<ResultadoColaboradorRubro> CrearColaboradorRubroAsync(ColaboradorRubroIngenieriaDto dto)
        {
            int contadorregistrosExistentes = 0;
            /* Comprobar fechas*/
            var fechaFin = dto.FechaFin.HasValue ? dto.FechaFin.Value : DateTime.Now;
            var contratos = _contratoRepository.GetAll().Where(o => o.vigente).ToList();

            foreach (var contrato in contratos)
            {
                var existe = ComprobarExistenciaRubroColaborador(dto.FechaInicio, fechaFin, dto.ContratoId, dto.ColaboradorId, dto.Id);
                if (existe)
                {
                    contadorregistrosExistentes++;
                } else
                {
                    /* Buscar un registro anterior del colaborador HU-CI-03 */
                    var registroAnterior = Repository.GetAll()
                        .Where(o => o.ColaboradorId == dto.ColaboradorId)
                        .Where(o => o.ContratoId == contrato.Id)
                        .OrderByDescending(o => o.Id)
                        .FirstOrDefault();

                    /* Se actualizará fecha fin del registro anterior con “fecha inicio nuevo periodo – 1*/
                    if (registroAnterior != null)
                    {
                        registroAnterior.FechaFin = DateTime.Now.AddDays(-1);
                    }

                    var preciario = repositoryPreciario.GetAll()
                    .Where(o => o.vigente)
                    .Where(o => o.Preciario.ContratoId == contrato.Id)
                    .Where(o => o.ItemId == dto.ItemId)
                    .FirstOrDefault();

                    dto.ContratoId = contrato.Id;
                    dto.RubroId = preciario.Id;
                    dto.Tarifa = preciario.precio_unitario;

                    var entity = Mapper.Map<ColaboradorRubroIngenieria>(dto);

                    await Repository.InsertAsync(entity);
                    
                }
            }

            if (contadorregistrosExistentes == contratos.Count)
            {
                return new ResultadoColaboradorRubro
                {
                    Success = false,
                    Message = "Ya existe un rubro vigente para el colaborador en estas fechas"
                };
            } else
            {
                return new ResultadoColaboradorRubro
                {
                    Success = true,
                    Message = ""
                };
            }
        }

        public List<ColaboradorRubroIngenieriaDto> ObtenerColaboresRubros()
        {
            var carpetas = Repository.GetAll().Include(o => o.Rubro.Item).Include(o => o.Colaborador).Include(o => o.Contrato).ToList();
            var dtos = Mapper.Map<List<ColaboradorRubroIngenieriaDto>>(carpetas);
            return dtos;
        }

        public List<ColaboradorRubroIngenieriaDto> ObtenerColaboresRubrosConFiltros(DateTime? fechaInicio, DateTime? fechaFin)
        {
            /** Se debe sacar los datos de un solo contrato porque los datos del otro contrato son exactamente igual */
            var contrato = _contratoRepository.GetAll()
                .Where(o => o.vigente)
                .FirstOrDefault();

            var queryCarpetas = Repository.GetAll()
                .Include(o => o.Rubro.Item)
                .Include(o => o.Colaborador)
                .Include(o => o.Contrato);


            if (fechaInicio.HasValue)
            {
                queryCarpetas = queryCarpetas
                    .Where(o => o.FechaInicio >= fechaInicio && o.FechaInicio <= fechaFin)
                    .Where(o => o.FechaFin <= fechaFin || o.FechaFin == null);
            }
            var carpetas = queryCarpetas
                .Where(o => o.ContratoId == contrato.Id)
                .ToList();

            var dtos = Mapper.Map<List<ColaboradorRubroIngenieriaDto>>(carpetas);
            return dtos;
        }

        public ResponseColaboradorItemIngenieriaDto ComprobarExistenciaItemEnContrato(int itemId)
        {
            var contratos = _contratoRepository.GetAll()
                .Where(o => o.vigente)
                .ToList();

            bool valido = true;
            List<string> messages = new List<string>();

            var item = _itemRepository.Get(itemId);

            foreach (var contrato in contratos)
            {
                var preciario = repositoryPreciario.GetAll()
                    .Where(o => o.vigente)
                    .Where(o => o.Preciario.ContratoId == contrato.Id)
                    .Where(o => o.ItemId == itemId)
                    .FirstOrDefault();

                if (item == null)
                {
                    valido = false;
                    messages.Add($"No existe un item ${item.codigo} - ${item.nombre} para el contrato ${contrato.Codigo}");
                }
            }

            return new ResponseColaboradorItemIngenieriaDto()
            {
                Mensajes = messages,
                Valido = valido
            };
        }

        public bool ComprobarExistenciaRubroColaborador(DateTime fechainicio, DateTime fechafin, int ContratoId, int colaboradorId, int rubroColaboradorId)
        {
            var rubroColaboradorQuery = Repository.GetAll()
                .Include(c => c.Contrato)
                .Where(c => c.ContratoId == ContratoId)
                .Where(c => c.ColaboradorId == colaboradorId);



            if (rubroColaboradorId > 0)
            {
                rubroColaboradorQuery = rubroColaboradorQuery.Where(o => o.Id != rubroColaboradorId);
            }

            var rubroColaborador = rubroColaboradorQuery.ToList();

            bool result = false;
            foreach (var item in rubroColaborador)
            {
                if (fechainicio >= item.FechaInicio && fechafin <= item.FechaFin)
                {
                    result = true;
                    ;
                    break;
                }

                if (item.FechaInicio > fechainicio && item.FechaFin < fechafin)
                {
                    result = true;
                    ;
                    break;
                }

                if (fechainicio > item.FechaInicio && fechainicio < item.FechaFin)
                {
                    result = true;
                    ;
                    break;
                }

                if (fechafin > item.FechaInicio && fechafin < item.FechaFin)
                {
                    result = true;
                    ;
                    break;
                }
            }

            return result;
        }

        public ResultadoColaboradorRubro EditarColaboradorRubroAsync(ColaboradorRubroIngenieriaDto dto)
        {
            var contratos = _contratoRepository.GetAll().Where(o => o.vigente).ToList();

            var colaboradorRubroTemp = Repository.GetAll()
                .Include(o => o.Rubro)
                .Where(o => o.Id == dto.Id)
                .FirstOrDefault();
            var colaboradorId = colaboradorRubroTemp.ColaboradorId;
            var itemId = colaboradorRubroTemp.Rubro.ItemId;

            foreach (var contrato in contratos)
            {


                var colaboradorRubo = Repository.GetAll()
                    .Where(o => o.Rubro.ItemId == itemId)
                    .Where(o => o.ContratoId == contrato.Id)
                    .Where(o => o.ColaboradorId == colaboradorId)
                    .FirstOrDefault();

                if (colaboradorRubo != null)
                {
                    var preciario = repositoryPreciario.GetAll()
                    .Where(o => o.vigente)
                    .Where(o => o.Preciario.ContratoId == contrato.Id)
                    .Where(o => o.ItemId == dto.ItemId)
                    .FirstOrDefault();

                    colaboradorRubo.RubroId = preciario.Id;
                    colaboradorRubo.FechaInicio = dto.FechaInicio;
                    colaboradorRubo.FechaFin = dto.FechaFin;
                    colaboradorRubo.Tarifa = preciario.precio_unitario;

                    Repository.Update(colaboradorRubo);
                }

            }

            return new ResultadoColaboradorRubro
            {
                Success = true,
                Message = ""
            };
        }

        public List<DetallePreciarioDto> GetPreciariosPorContrato(int contratoId)
        {
            var detalles = repositoryPreciario.GetAll()
                .Include(o => o.Item)
                .Include(o => o.Preciario)
                .Include(o => o.Item.Catalogo.TipoCatalogo)
                .Where(o => o.Preciario.ContratoId == contratoId)
                .Where(o => o.vigente)
                .Where(o => o.Item.item_padre.StartsWith("1.1") || o.Item.item_padre.StartsWith("1.2"))
                .Where(o => o.Item.para_oferta == true)
                .ToList();

            var dtos = Mapper.Map<List<DetallePreciarioDto>>(detalles);
            return dtos;
        }

        public List<ItemDto> GetItems()
        {
            var items = _itemRepository.GetAll().Include(o => o.Catalogo.TipoCatalogo)
                .Where(o => o.vigente)
                .Where(o => o.item_padre.StartsWith("1.1") || o.item_padre.StartsWith("1.2"))
                .Where(o => o.para_oferta == true)
                .ToList();
            var dtos = Mapper.Map<List<ItemDto>>(items);
            foreach (var item in dtos)
            {
                item.nombre = item.codigo + " - " + item.nombre;
            }
            return dtos;
        }

        public ResultadoColaboradorRubro Eliminar(int id)
        {

            var colaboradorRubroTemp = Repository.GetAll()
                .Include(o => o.Rubro)
                .Where(o => o.Id == id)
                .FirstOrDefault();

            var colaboradorId = colaboradorRubroTemp.ColaboradorId;
            var itemId = colaboradorRubroTemp.Rubro.ItemId;
            var fechaInicio = colaboradorRubroTemp.FechaInicio;
            var fechaFin = colaboradorRubroTemp.FechaFin;

            var contratos = _contratoRepository.GetAll().Where(o => o.vigente).ToList();

            foreach (var contrato in contratos)
            {


                var colaboradorRubo = Repository.GetAll()
                    .Where(o => o.Rubro.ItemId == itemId)
                    .Where(o => o.ContratoId == contrato.Id)
                    .Where(o => o.ColaboradorId == colaboradorId)
                    .Where(o => o.FechaInicio == fechaInicio)
                    .Where(o => o.FechaFin == fechaFin)
                    .FirstOrDefault();

                Repository.Delete(colaboradorRubo.Id);

            }

            return new ResultadoColaboradorRubro
            {
                Success = true,
                Message = ""
            };

        }

        public ExcelPackage DescargarPlantillaCargaMasivaTarifas(int contratoId)
        {
            ExcelPackage excel = new ExcelPackage();

            string filename = System.Web.HttpContext.Current.Server.MapPath("~/Views/PlantillaWord/CertificacionIngenieria/FormatoCargaTarifas.xlsx");
            if (File.Exists((string)filename))
            {
                FileInfo newFile = new FileInfo(filename);
                ExcelPackage pck = new ExcelPackage(newFile);
                excel.Workbook.Worksheets.Add("CARGA", pck.Workbook.Worksheets[1]);
                excel.Workbook.Worksheets.Add("PARÁMETROS", pck.Workbook.Worksheets[2]);
            }

            ExcelWorksheet tarifas = excel.Workbook.Worksheets[1];
            ExcelWorksheet catalogos = excel.Workbook.Worksheets[2];

            var detallesPreciarios = repositoryPreciario.GetAll().Include(o => o.Item).Include(o => o.Preciario).Include(o => o.Item.Catalogo.TipoCatalogo)
                .Where(o => o.Preciario.ContratoId == contratoId)
                .Where(o => o.vigente)
                .Where(o => o.Item.codigo.StartsWith("1"))
                .Where(o => o.Item.para_oferta == true)
                .ToList();

            int countFilasDetallesPreciarios = 2;
            foreach (var preciario in detallesPreciarios)
            {
                catalogos.Cells["A" + countFilasDetallesPreciarios].Value = preciario.Item.codigo;
                catalogos.Cells["B" + countFilasDetallesPreciarios].Value = preciario.Id;
                catalogos.Cells["C" + countFilasDetallesPreciarios].Value = preciario.Item.nombre;
                catalogos.Cells["D" + countFilasDetallesPreciarios].Value = preciario.precio_unitario;
                countFilasDetallesPreciarios++;
            }

            catalogos.Cells["E1"].Value = contratoId;


            var validacionCodigoItems = tarifas.DataValidations.AddListValidation("B:B");
            validacionCodigoItems.Formula.ExcelFormula = "=PARÁMETROS!$A$2:$A$" + countFilasDetallesPreciarios;
            validacionCodigoItems.AllowBlank = true;

            /*for (int i = 2; i < 31; i++)
            {
                tarifas.Cells["C" + i].FormulaR1C1 = "=BUSCARV(B2;PARÁMETROS!A:D;3;FALSO)";
                tarifas.Cells["D" + i].FormulaR1C1 = "=BUSCARV(B2;PARÁMETROS!A:E;4;FALSO)";
            }*/
            
            return excel;
        }

        public ExcelPackage CargaMasivaDeTarifas(HttpPostedFileBase uploadedFile)
        {
            if (uploadedFile != null)
            {
                if (uploadedFile.ContentType == "application/vnd.ms-excel" || uploadedFile.ContentType ==
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    ExcelPackage excel = new ExcelPackage();

                    string fileContentType = uploadedFile.ContentType;
                    byte[] fileBytes = new byte[uploadedFile.ContentLength];

                    ExcelWorksheet tarifas = null;
                    ExcelWorksheet catalogos = null;

                    using (var package = new ExcelPackage(uploadedFile.InputStream))
                    {
                        tarifas = excel.Workbook.Worksheets.Add("CARGA", package.Workbook.Worksheets[1]);
                        catalogos = excel.Workbook.Worksheets.Add("PARÁMETROS", package.Workbook.Worksheets[2]);

                        var numberOfColumns = tarifas.Dimension.End.Column;
                        var numberOfRows = tarifas.Dimension.End.Row;

                        for (int rowIterator = 2; rowIterator <= numberOfRows; rowIterator++)
                        {
                            /* Validar ingreso de la identificacion*/
                            var identificacion = (tarifas.Cells["A" + rowIterator].Value ?? "").ToString();
                            if (identificacion == "")
                            {
                                tarifas.Cells["G" + rowIterator].Value = "Debe ingresar la identificación del colaborador";
                                continue;
                            }

                            /* Buscar el colaborador */
                            var colaborador = _colaboradorRepository
                                .GetAll()
                                .FirstOrDefault(o => o.numero_identificacion == identificacion);
                            if (colaborador == null)
                            {
                                tarifas.Cells["G" + rowIterator].Value = "No se encontró al colaborador";
                                continue;
                            }

                            /* Validar el ingreso del Rubro*/
                            var rubro = (tarifas.Cells["B" + rowIterator].Value ?? "").ToString();
                            if (rubro == "")
                            {
                                tarifas.Cells["G" + rowIterator].Value = "Debe seleccionar el rubro";
                                continue;
                            }

                            /* Buscar detallePreciaro*/
                            var contratoId = Int32.Parse(catalogos.Cells["E1"].Value.ToString());
                            var detallePreciario = repositoryPreciario.GetAll()
                                .Where(o => o.Item.codigo == rubro)
                                .Where(o => o.vigente == true)
                                .FirstOrDefault(o => o.Preciario.ContratoId == contratoId);
                            if (detallePreciario == null)
                            {
                                tarifas.Cells["G" + rowIterator].Value = "No se encontró el preciario del item";
                                continue;
                            }

                            /* Validar el ingreso de la fecha inicio*/
                            var fechaInicioString = (tarifas.Cells["E" + rowIterator].Text ?? "").ToString();
                            if (fechaInicioString == "")
                            {
                                tarifas.Cells["G" + rowIterator].Value = "Debe ingresar la fecha de inicio";
                                continue;
                            }
                            DateTime fechaInicio = DateTime.Parse(fechaInicioString);

                            /* Fecha Fin*/
                            var fechaFinString = (tarifas.Cells["F" + rowIterator].Text ?? "").ToString();

                            /* Buscar registro duplicados */
                            var duplicados = Repository.GetAll()
                                .Where(o => o.ContratoId == contratoId)
                                .Where(o => o.ColaboradorId == colaborador.Id)
                                .Where(o => o.RubroId == detallePreciario.Id)
                                .Where(o => o.FechaInicio == fechaInicio)
                                .ToList();
                            if (duplicados.Count > 0)
                            {
                                tarifas.Cells["G" + rowIterator].Value = "Registro ya ingresado anteriormente";
                                continue;
                            }

                            /* Validar Periodos */
                            DateTime? tempFechaFin = fechaFinString != "" ? DateTime.Parse(fechaFinString) : DateTime.Now;
                            var existe = ComprobarExistenciaRubroColaborador(fechaInicio, tempFechaFin.Value, contratoId, colaborador.Id, 0);
                            if (existe)
                            {
                                tarifas.Cells["G" + rowIterator].Value = "Ya existe una tarifa vigente en las fechas ingresadas";
                                continue;
                            }

                            var rubroColaborador = new ColaboradorRubroIngenieria()
                            {
                                FechaInicio = fechaInicio,
                                ColaboradorId = colaborador.Id,
                                RubroId = detallePreciario.Id,
                                Tarifa = detallePreciario.precio_unitario,
                                ContratoId = contratoId
                            };
                            if (fechaFinString != "")
                            {
                                rubroColaborador.FechaFin = DateTime.Parse(fechaFinString);
                            }
                            Repository.Insert(rubroColaborador);
                            tarifas.Cells["G" + rowIterator].Value = "Actualizado Correctamente";
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
